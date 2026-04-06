using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronAndEther;

public class Projectile
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Lifetime;
    public float Age;
    public bool Alive = true;
    public ProjectileParams Params;
    public float Size;

    // Orbit state
    public float OrbitAngle;
    public string FusionName;
    
    // Bloom state
    public float CurrentScale = 1f;
    
    // Saw return state
    public bool Returning;
    
    // Linger trail timer
    public float LingerTrailTimer;
    
    // Hit tracking for piercing projectiles
    public HashSet<int> HitEnemies = new();
    public int Bounces; // wall bounce counter (unused — bouncing handled by Params.Bounces)

    // Chain homing
    public int HomingTargetId = -1;  // Enemy.Id to gently home toward
    public float HomingStrength = 3f; // turn rate in radians/sec

    // Lance beam
    public Vector2 BeamDirection;
    public float BeamLen; // length of beam from Position (origin) to tip

    // Hiten arc slash
    public float SlashBaseAngle;  // center angle of the arc
    public float SlashSweep;      // total angular sweep (radians)
    public float SlashRadius;     // distance from origin to arc

    // Hiten sword swing (straight blade that rotates)
    public float SwingStartAngle; // angle at t=0
    public float SwingEndAngle;   // angle at t=1
    public float BladeLength;     // how long the blade is

    // Grace period: no collision with walls/enemies during this time
    public float GracePeriod;     // seconds remaining (0 = normal)

    // Seed arc
    public Vector2 ArcStart;
    public Vector2 ArcTarget;
    public float ArcDuration; // >0 means arcing
    public float ArcTimer;
    public bool ArcLanded;
    
    // Arc height for visual (parabolic, 0 at start and end, max at midpoint)
    public float ArcHeight => ArcDuration > 0 && !ArcLanded
        ? MathF.Sin(MathHelper.Clamp(ArcTimer / ArcDuration, 0f, 1f) * MathF.PI) * Vector2.Distance(ArcStart, ArcTarget) * 0.5f
        : 0f;

    // Wave sine state
    public float WaveTime;
    public Vector2 BaseVelocity;
    public Vector2 BasePosition;

    public Projectile(Vector2 pos, Vector2 vel, ProjectileParams parms)
    {
        Position = pos;
        Velocity = vel;
        Params = parms;
        Lifetime = parms.Lifetime;
        Size = parms.Size;
        BaseVelocity = vel;
        BasePosition = pos;
    }

    public void Update(float dt, Vector2 playerPos, Rectangle arenaBounds)
    {
        // Arc projectiles don't age until landing (Age reset on land anyway)
        if (ArcDuration <= 0 || ArcLanded)
        {
            Age += dt;
            if (Age >= Lifetime) { Alive = false; return; }
        }

        // Seed arc: parabolic flight to target
        if (ArcDuration > 0 && !ArcLanded)
        {
            ArcTimer += dt;
            float t = MathHelper.Clamp(ArcTimer / ArcDuration, 0f, 1f);
            // Ease out for satisfying landing
            float easeT = 1f - (1f - t) * (1f - t);
            Position = Vector2.Lerp(ArcStart, ArcTarget, easeT);
            if (t >= 1f)
            {
                ArcLanded = true;
                Position = ArcTarget;
                Age = 0f; // reset lifetime — countdown starts on landing
            }
            return; // don't run normal movement during arc
        }

        if (Params.Gravity > 0)
            Velocity.Y += Params.Gravity * dt;

        if (Params.OrbitSpeed > 0 && Params.OrbitRadius > 0)
        {
            OrbitAngle += Params.OrbitSpeed * dt;
            Position = playerPos + new Vector2(
                MathF.Cos(OrbitAngle) * Params.OrbitRadius,
                MathF.Sin(OrbitAngle) * Params.OrbitRadius);
        }
        else if (Params.WaveAmplitude > 0)
        {
            // Sine wave movement perpendicular to travel direction
            WaveTime += dt;
            BasePosition += BaseVelocity * dt;
            if (BaseVelocity.LengthSquared() > 0)
            {
                Vector2 dir = Vector2.Normalize(BaseVelocity);
                Vector2 perp = new Vector2(-dir.Y, dir.X);
                Position = BasePosition + perp * MathF.Sin(WaveTime * Params.WaveFrequency) * Params.WaveAmplitude;
            }
            else
            {
                Position = BasePosition;
            }
        }
        else if (!Params.IsStationary)
        {
            Position += Velocity * dt;
        }

        if (Params.Bounces && Params.MaxBounces > 0)
        {
            if (Position.X < arenaBounds.Left || Position.X > arenaBounds.Right)
            {
                Velocity.X = -Velocity.X;
                Position.X = MathHelper.Clamp(Position.X, arenaBounds.Left, arenaBounds.Right);
                Params.MaxBounces--;
            }
            if (Position.Y < arenaBounds.Top || Position.Y > arenaBounds.Bottom)
            {
                Velocity.Y = -Velocity.Y;
                Position.Y = MathHelper.Clamp(Position.Y, arenaBounds.Top, arenaBounds.Bottom);
                Params.MaxBounces--;
            }
        }

        if (GracePeriod > 0) GracePeriod -= dt;
        
        if (!Params.Bounces)
        {
            if (GracePeriod <= 0 && (Position.X < arenaBounds.Left - 2 || Position.X > arenaBounds.Right + 2 ||
                Position.Y < arenaBounds.Top - 2 || Position.Y > arenaBounds.Bottom + 2))
                Alive = false;
        }

        if (Params.BloomGrowRate > 0)
        {
            CurrentScale = MathHelper.Clamp(CurrentScale + Params.BloomGrowRate * dt, 1f, Params.BloomMaxScale);
        }

        // Saw boomerang return
        if (Params.Returns && !Returning && Age > Lifetime * 0.4f)
        {
            Returning = true;
            HitEnemies.Clear();
        }
        if (Returning)
        {
            Vector2 toPlayer = playerPos - Position;
            if (toPlayer.LengthSquared() > 0)
            {
                toPlayer.Normalize();
                Velocity = Vector2.Lerp(Velocity, toPlayer * Params.Speed * 1.2f, dt * 5f);
            }
            if (Vector2.Distance(Position, playerPos) < 20f && Age > 0.3f)
                Alive = false;
        }
    }

    public Rectangle HitBox
    {
        get
        {
            float s = Size * CurrentScale;
            if (BeamLen > 0 && BeamDirection.LengthSquared() > 0.1f)
            {
                Vector2 tip = Position + BeamDirection * BeamLen;
                float minX = MathF.Min(Position.X, tip.X);
                float minY = MathF.Min(Position.Y, tip.Y);
                float maxX = MathF.Max(Position.X, tip.X);
                float maxY = MathF.Max(Position.Y, tip.Y);
                float halfW = s * 0.5f + 4f;
                return new Rectangle((int)(minX - halfW), (int)(minY - halfW),
                    (int)(maxX - minX + halfW * 2), (int)(maxY - minY + halfW * 2));
            }
            return new Rectangle((int)(Position.X - s / 2), (int)(Position.Y - s / 2), (int)s, (int)s);
        }
    }

    public void Draw(SpriteBatch sb, Texture2D pixel)
    {
        if (!Alive) return;
        float s = Size * CurrentScale;
        var color = new Color(Params.Color.R, Params.Color.G, Params.Color.B);

        // Trail afterimages for all projectiles
        if (Velocity.LengthSquared() > 1f)
        {
            var normVel = Vector2.Normalize(Velocity);
            for (int i = 1; i <= 3; i++)
            {
                var trailPos = Position - normVel * i * s;
                float alpha = 0.3f - i * 0.08f;
                sb.Draw(pixel, new Rectangle((int)(trailPos.X - s * 0.4f), (int)(trailPos.Y - s * 0.4f),
                    (int)(s * 0.8f), (int)(s * 0.8f)), color * alpha);
            }
        }

        // Saw spinning draw vs normal square
        if (Params.Returns) // Only Saw spins
        {
            float rotation = Age * 15f;
            if (FusionName == "DragonTooth")
            {
                // Fang: elongated triangle pointing in velocity direction
                float fangAngle = Velocity.LengthSquared() > 0 ? MathF.Atan2(Velocity.Y, Velocity.X) : rotation;
                int fangLen = (int)(s * 2.5f);
                int fangW = (int)(s * 0.6f);
                // Main fang body (long narrow rectangle)
                sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, fangLen, fangW),
                    null, new Color(240, 240, 230), fangAngle, new Vector2(fangLen * 0.3f, fangW / 2f), SpriteEffects.None, 0);
                // Fang tip (smaller, slightly offset forward)
                int tipLen = (int)(s * 1.2f);
                int tipW = (int)(s * 0.3f);
                Vector2 tipOff = new Vector2(MathF.Cos(fangAngle), MathF.Sin(fangAngle)) * s * 0.8f;
                sb.Draw(pixel, new Rectangle((int)(Position.X + tipOff.X), (int)(Position.Y + tipOff.Y), tipLen, tipW),
                    null, new Color(220, 210, 200), fangAngle, new Vector2(tipLen * 0.2f, tipW / 2f), SpriteEffects.None, 0);
                // Dark core line
                sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, fangLen - 2, MathHelper.Max(1, fangW / 3)),
                    null, new Color(180, 170, 160) * 0.6f, fangAngle, new Vector2(fangLen * 0.3f, fangW / 6f), SpriteEffects.None, 0);
            }
            else if (FusionName == "GorgonBite")
            {
                // Snake bite — two fang shapes converging
                float biteRot = MathF.Atan2(Velocity.Y, Velocity.X);
                float biteAlpha = MathHelper.Clamp((Lifetime - Age) * 10f, 0f, 1f);
                int fangLen = (int)(s * 1.5f);
                int fangW = MathHelper.Max((int)(s * 0.4f), 2);
                sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, fangLen, fangW),
                    null, new Color(80, 200, 80) * biteAlpha, biteRot - 0.15f, new Vector2(0, fangW), SpriteEffects.None, 0);
                sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, fangLen, fangW),
                    null, new Color(80, 200, 80) * biteAlpha, biteRot + 0.15f, new Vector2(0, 0), SpriteEffects.None, 0);
                sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, fangLen / 2, MathHelper.Max(fangW / 2, 1)),
                    null, new Color(120, 255, 80) * biteAlpha * 0.7f, biteRot, new Vector2(0, fangW / 4f), SpriteEffects.None, 0);
            }
            else if (FusionName == "GorgonSpit")
            {
                // Poison glob — pulsing green ball
                float pulse = 1f + MathF.Sin(Age * 12f) * 0.15f;
                float spitAlpha = MathHelper.Clamp((Lifetime - Age) * 4f, 0f, 1f);
                int r = (int)(s * pulse);
                sb.Draw(pixel, new Rectangle((int)Position.X - r - 1, (int)Position.Y - r - 1, r * 2 + 2, r * 2 + 2),
                    new Color(30, 100, 30) * spitAlpha * 0.5f);
                sb.Draw(pixel, new Rectangle((int)Position.X - r, (int)Position.Y - r, r * 2, r * 2),
                    new Color(70, 200, 60) * spitAlpha);
                int cr = MathHelper.Max(r / 2, 1);
                sb.Draw(pixel, new Rectangle((int)Position.X - cr, (int)Position.Y - cr, cr * 2, cr * 2),
                    new Color(140, 255, 100) * spitAlpha * 0.8f);
            }
            else if (FusionName == "Death's Scythe")
            {
                // Scythe: one crescent blade — elongated rectangle + slight curve via offset tip
                int bladeW = (int)(s * 2f);
                int bladeH = (int)(s * 0.4f);
                // Main blade
                sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, bladeW, bladeH),
                    null, color, rotation, new Vector2(bladeW * 0.35f, bladeH / 2f), SpriteEffects.None, 0);
                // Curved tip (offset slightly)
                int tipS = (int)(s * 0.6f);
                Vector2 tipOff = new Vector2(MathF.Cos(rotation), MathF.Sin(rotation)) * s * 0.7f;
                sb.Draw(pixel, new Rectangle((int)(Position.X + tipOff.X), (int)(Position.Y + tipOff.Y), tipS, tipS),
                    null, color * 0.8f, rotation + 0.5f, new Vector2(tipS / 2f, tipS / 2f), SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, (int)s, (int)s),
                    null, color, rotation, new Vector2(s / 2f, s / 2f), SpriteEffects.None, 0);
            }
        }
        else if ((FusionName == "HitenSlash" || FusionName == "HitenAfterimage") && BladeLength > 0)
        {
            // Hiten — straight blade swinging from SwingStartAngle to SwingEndAngle
            int thick = MathHelper.Max((int)(Size * CurrentScale), 2);
            float swingT = MathHelper.Clamp(Age / Lifetime, 0f, 1f);
            // Smooth swing with ease-out (fast start, decelerate)
            float easedT = 1f - (1f - swingT) * (1f - swingT);
            float currentAngle = MathHelper.Lerp(SwingStartAngle, SwingEndAngle, easedT);

            if (FusionName == "HitenSlash")
            {
                float hAlpha = MathHelper.Clamp(Age * 30f, 0f, 1f) * MathHelper.Clamp((Lifetime - Age) * 8f, 0f, 1f);
                Vector2 srcOrigin = new Vector2(0, 0.5f);
                // Blade tapers toward the tip: 3 segments
                int segments = 4;
                float segLen = BladeLength / segments;
                // Offset blade start slightly from player center
                Vector2 bladeStart = Position + new Vector2(MathF.Cos(currentAngle), MathF.Sin(currentAngle)) * 8f;
                for (int seg = 0; seg < segments; seg++)
                {
                    float t = (float)seg / segments;
                    float taper = 1f - t * 0.65f; // thick at base, thin at tip
                    float segThick = thick * taper;
                    Vector2 segPos = bladeStart + new Vector2(MathF.Cos(currentAngle), MathF.Sin(currentAngle)) * (seg * segLen);
                    // Outer glow — silk purple
                    sb.Draw(pixel, segPos, null, new Color(180, 140, 220) * hAlpha * 0.4f, currentAngle,
                        srcOrigin, new Vector2(segLen + 1, segThick + 8), SpriteEffects.None, 0);
                    // Blade body — near white
                    sb.Draw(pixel, segPos, null, new Color(240, 240, 255) * hAlpha, currentAngle,
                        srcOrigin, new Vector2(segLen + 1, segThick), SpriteEffects.None, 0);
                    // White-hot core
                    sb.Draw(pixel, segPos, null, Color.White * hAlpha, currentAngle,
                        srcOrigin, new Vector2(segLen + 1, MathHelper.Max(segThick * 0.3f, 1)), SpriteEffects.None, 0);
                }
                // Tip point — small bright dot at the end
                Vector2 tip = bladeStart + new Vector2(MathF.Cos(currentAngle), MathF.Sin(currentAngle)) * BladeLength;
                sb.Draw(pixel, tip, null, Color.White * hAlpha, currentAngle,
                    new Vector2(0.5f, 0.5f), new Vector2(3, 3), SpriteEffects.None, 0);
            }
            else // HitenAfterimage — trails behind (uses slightly earlier swing position)
            {
                float lagT = MathHelper.Clamp(easedT - 0.15f, 0f, 1f); // trails behind main blade
                float lagAngle = MathHelper.Lerp(SwingStartAngle, SwingEndAngle, lagT);
                float haAlpha = MathHelper.Clamp((Lifetime - Age) * 4f, 0f, 1f) * 0.5f;
                Vector2 srcOrigin = new Vector2(0, 0.5f);
                Vector2 bladeStart = Position + new Vector2(MathF.Cos(lagAngle), MathF.Sin(lagAngle)) * 8f;
                int segments = 3;
                float segLen = BladeLength / segments;
                for (int seg = 0; seg < segments; seg++)
                {
                    float t = (float)seg / segments;
                    float taper = 1f - t * 0.6f;
                    float segThick = thick * taper;
                    Vector2 segPos = bladeStart + new Vector2(MathF.Cos(lagAngle), MathF.Sin(lagAngle)) * (seg * segLen);
                    sb.Draw(pixel, segPos, null, new Color(160, 120, 220) * haAlpha * taper, lagAngle,
                        srcOrigin, new Vector2(segLen + 1, segThick + 4), SpriteEffects.None, 0);
                }
            }
        }
        else if ((FusionName == "DragonSlayer" || FusionName == "DragonSlayerAfter") && BladeLength > 0)
        {
            // Dragon Slayer — colossal bone greatsword cleave (Berserk)
            int thick = MathHelper.Max((int)(Size * CurrentScale), 3);
            float swingT = MathHelper.Clamp(Age / Lifetime, 0f, 1f);
            // Heavy swing — slow start, accelerate (ease-in)
            float easedT = swingT * swingT;
            float currentAngle = MathHelper.Lerp(SwingStartAngle, SwingEndAngle, easedT);

            if (FusionName == "DragonSlayer")
            {
                float hAlpha = MathHelper.Clamp(Age * 20f, 0f, 1f) * MathHelper.Clamp((Lifetime - Age) * 6f, 0f, 1f);
                Vector2 srcOrigin = new Vector2(0, 0.5f);
                int segments = 5;
                float segLen = BladeLength / segments;
                Vector2 bladeStart = Position + new Vector2(MathF.Cos(currentAngle), MathF.Sin(currentAngle)) * 6f;
                for (int seg = 0; seg < segments; seg++)
                {
                    float t = (float)seg / segments;
                    // Dragon Slayer shape: wide at middle, tapers at both ends (greatsword profile)
                    float taper = seg < 2 ? 0.7f + t * 0.6f : 1.3f - (t - 0.4f) * 0.7f;
                    float segThick = thick * taper;
                    Vector2 segPos = bladeStart + new Vector2(MathF.Cos(currentAngle), MathF.Sin(currentAngle)) * (seg * segLen);
                    // Dark edge outline
                    sb.Draw(pixel, segPos, null, new Color(60, 50, 40) * hAlpha * 0.6f, currentAngle,
                        srcOrigin, new Vector2(segLen + 1, segThick + 10), SpriteEffects.None, 0);
                    // Bone body — warm off-white
                    sb.Draw(pixel, segPos, null, new Color(230, 220, 200) * hAlpha, currentAngle,
                        srcOrigin, new Vector2(segLen + 1, segThick), SpriteEffects.None, 0);
                    // Bright edge highlight
                    sb.Draw(pixel, segPos, null, new Color(255, 250, 240) * hAlpha * 0.7f, currentAngle,
                        srcOrigin, new Vector2(segLen + 1, MathHelper.Max(segThick * 0.25f, 1)), SpriteEffects.None, 0);
                }
                // Blunt tip — Dragon Slayer has a squared-off end
                Vector2 tip = bladeStart + new Vector2(MathF.Cos(currentAngle), MathF.Sin(currentAngle)) * BladeLength;
                float tipThick = thick * 0.5f;
                sb.Draw(pixel, tip, null, new Color(200, 190, 170) * hAlpha, currentAngle + MathF.PI / 2f,
                    new Vector2(0.5f, 0.5f), new Vector2(tipThick, 4), SpriteEffects.None, 0);
            }
            else // DragonSlayerAfter — heavy trailing shadow
            {
                float lagT = MathHelper.Clamp(easedT - 0.12f, 0f, 1f);
                float lagAngle = MathHelper.Lerp(SwingStartAngle, SwingEndAngle, lagT);
                float haAlpha = MathHelper.Clamp((Lifetime - Age) * 3f, 0f, 1f) * 0.4f;
                Vector2 srcOrigin = new Vector2(0, 0.5f);
                Vector2 bladeStart = Position + new Vector2(MathF.Cos(lagAngle), MathF.Sin(lagAngle)) * 6f;
                int segments = 4;
                float segLen = BladeLength / segments;
                for (int seg = 0; seg < segments; seg++)
                {
                    float t = (float)seg / segments;
                    float taper = seg < 2 ? 0.7f + t * 0.6f : 1.3f - (t - 0.4f) * 0.7f;
                    float segThick = thick * taper;
                    Vector2 segPos = bladeStart + new Vector2(MathF.Cos(lagAngle), MathF.Sin(lagAngle)) * (seg * segLen);
                    sb.Draw(pixel, segPos, null, new Color(160, 140, 120) * haAlpha * taper, lagAngle,
                        srcOrigin, new Vector2(segLen + 1, segThick + 4), SpriteEffects.None, 0);
                }
            }
        }
        else if (BeamDirection.LengthSquared() > 0.1f)
        {
            float rotation = MathF.Atan2(BeamDirection.Y, BeamDirection.X);
            float fadeIn = MathHelper.Clamp(Age * 8f, 0f, 1f);
            float fadeOut = MathHelper.Clamp((Lifetime - Age) * 5f, 0f, 1f);
            float alpha = fadeIn * fadeOut;

            if (BeamLen > 0)
            {
                int len = (int)BeamLen;
                int thick = MathHelper.Max((int)(Size * CurrentScale), 2);
                // ═══ SUN LANCE: tapered flaming lance beam ═══
                if (FusionName == "SunLance" && BeamDirection.LengthSquared() > 0.1f)
                {
                    int segments = (int)(BeamLen / 3f);
                    Vector2 dirN = BeamDirection;
                    if (dirN.LengthSquared() > 0.1f) dirN.Normalize();
                    float perpX = -dirN.Y;
                    float perpY = dirN.X;
                    
                    for (int seg = 0; seg < segments; seg++)
                    {
                        float t = (float)seg / segments;
                        float baseWidth = Size * 2.5f;
                        float tipWidth = 2f;
                        float w = baseWidth * (1f - t) + tipWidth * t;
                        Vector2 segPos = Position + dirN * (BeamLen * t);
                        
                        float glowW = w + 8f;
                        int halfGW = (int)(glowW / 2f);
                        for (int pw = -halfGW; pw <= halfGW; pw++)
                        {
                            int px = (int)(segPos.X + perpX * pw);
                            int py = (int)(segPos.Y + perpY * pw);
                            sb.Draw(pixel, new Rectangle(px, py, 1, 1), new Color(255, 120, 20) * 0.3f);
                        }
                        
                        float coreT = MathF.Max(0, 1f - t * 1.2f);
                        Color coreCol = new Color(
                            255,
                            (int)(180 + 75 * coreT),
                            (int)(40 + 180 * coreT));
                        int halfCW = (int)(w / 2f);
                        for (int pw = -halfCW; pw <= halfCW; pw++)
                        {
                            int px = (int)(segPos.X + perpX * pw);
                            int py = (int)(segPos.Y + perpY * pw);
                            sb.Draw(pixel, new Rectangle(px, py, 1, 1), coreCol);
                        }
                    }
                }
                // ═══ BONE SPEAR: segmented bone lance ═══
                else if (FusionName == "BoneSpear" && BeamDirection.LengthSquared() > 0.1f)
                {
                    Vector2 dirN = BeamDirection;
                    if (dirN.LengthSquared() > 0.1f) dirN.Normalize();
                    float perpX = -dirN.Y;
                    float perpY = dirN.X;
                    int boneLen = (int)BeamLen;
                    
                    Color boneColor = new Color(220, 205, 170);
                    Color boneDark = new Color(180, 165, 130);
                    for (int seg = 0; seg < boneLen; seg += 3)
                    {
                        float t = (float)seg / boneLen;
                        float w = Size * (1f - t * 0.6f);
                        if (t > 0.85f) w *= (1f - t) / 0.15f;
                        Vector2 segPos = Position + dirN * seg;
                        Color segCol = (seg / 3 % 2 == 0) ? boneColor : boneDark;
                        int halfBW = (int)(MathF.Max(w, 1f) / 2f);
                        for (int pw = -halfBW; pw <= halfBW; pw++)
                        {
                            int px = (int)(segPos.X + perpX * pw);
                            int py = (int)(segPos.Y + perpY * pw);
                            sb.Draw(pixel, new Rectangle(px, py, 1, 1), segCol);
                        }
                    }
                    Vector2 tip = Position + dirN * boneLen;
                    sb.Draw(pixel, new Rectangle((int)(tip.X - 1), (int)(tip.Y - 1), 3, 3), Color.White);
                }
                else if (FusionName == "Gungnir")
                {
                    int glowH = thick + 6;
                    int coreH = MathHelper.Max(thick / 3, 2);
                    sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, len, glowH),
                        null, new Color(40, 10, 50) * 0.5f, rotation, new Vector2(0, glowH / 2f), SpriteEffects.None, 0);
                    sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, len, thick),
                        null, new Color(130, 50, 180) * alpha, rotation, new Vector2(0, thick / 2f), SpriteEffects.None, 0);
                    sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, len, coreH),
                        null, new Color(200, 140, 255) * alpha * 0.8f, rotation, new Vector2(0, coreH / 2f), SpriteEffects.None, 0);
                }
                else if (FusionName == "Surya")
                {
                    // Surya's Wrath: blazing solar lance — tapered beam with heat shimmer,
                    // dancing flame edges, and white-hot core that pulses
                    Vector2 dirN = BeamDirection;
                    if (dirN.LengthSquared() > 0.1f) dirN.Normalize();
                    float perpX = -dirN.Y;
                    float perpY = dirN.X;
                    
                    int segments = (int)(BeamLen / 2.5f);
                    float pulsePhase = Age * 12f;
                    
                    for (int seg = 0; seg < segments; seg++)
                    {
                        float t = (float)seg / segments;
                        Vector2 segPos = Position + dirN * (BeamLen * t);
                        
                        // Taper: wide at base, pointed at tip
                        float baseWidth = Size * 2.2f;
                        float tipWidth = 1.5f;
                        float w = baseWidth * (1f - t * 0.7f) + tipWidth * t;
                        
                        // Heat shimmer: perpendicular wave that travels along the beam
                        float shimmer = MathF.Sin(pulsePhase + t * 20f) * (2f + t * 3f);
                        float shimmerOffset = shimmer * (1f - t * 0.5f); // less shimmer at base
                        
                        // Flame edge: random-ish dancing tongues
                        float flame1 = MathF.Sin(pulsePhase * 1.7f + t * 15f + seg * 0.3f) * (3f + t * 4f);
                        float flame2 = MathF.Sin(pulsePhase * 2.3f + t * 18f + seg * 0.7f) * (2f + t * 3f);
                        float edgeW = w + MathF.Abs(flame1) + MathF.Abs(flame2);
                        
                        // Outer glow — deep orange, wide, with flame edges
                        int halfGW = (int)(edgeW / 2f + 2);
                        for (int pw = -halfGW; pw <= halfGW; pw++)
                        {
                            float dist = MathF.Abs(pw) / (halfGW + 0.1f);
                            float glowAlpha = (1f - dist) * 0.25f * alpha * (1f - t * 0.3f);
                            if (glowAlpha < 0.01f) continue;
                            int px = (int)(segPos.X + perpX * (pw + shimmerOffset));
                            int py = (int)(segPos.Y + perpY * (pw + shimmerOffset));
                            sb.Draw(pixel, new Rectangle(px, py, 1, 1), new Color(255, 60, 10) * glowAlpha);
                        }
                        
                        // Mid layer — bright orange-yellow
                        int halfMW = (int)(w / 2f);
                        for (int pw = -halfMW; pw <= halfMW; pw++)
                        {
                            float dist = MathF.Abs(pw) / (halfMW + 0.1f);
                            float midAlpha = (1f - dist * dist) * alpha * (1f - t * 0.4f);
                            int px = (int)(segPos.X + perpX * (pw + shimmerOffset * 0.5f));
                            int py = (int)(segPos.Y + perpY * (pw + shimmerOffset * 0.5f));
                            // Color shifts from yellow-white at base to deep orange at tip
                            Color midCol = new Color(
                                255,
                                (int)(200 - 120 * t),
                                (int)(80 - 60 * t));
                            sb.Draw(pixel, new Rectangle(px, py, 1, 1), midCol * midAlpha);
                        }
                        
                        // Core — white-hot, thin, pulsing
                        float corePulse = 0.8f + 0.2f * MathF.Sin(pulsePhase * 3f + t * 8f);
                        float coreW = MathF.Max(w * 0.3f, 1.5f) * corePulse;
                        int halfCW = (int)(coreW / 2f);
                        for (int pw = -halfCW; pw <= halfCW; pw++)
                        {
                            int px = (int)(segPos.X + perpX * pw);
                            int py = (int)(segPos.Y + perpY * pw);
                            float coreAlpha = alpha * 0.95f * (1f - t * 0.5f);
                            sb.Draw(pixel, new Rectangle(px, py, 1, 1), new Color(255, 255, 220) * coreAlpha);
                        }
                    }
                    
                    // Tip flare — bright point at beam end
                    Vector2 tip = Position + dirN * BeamLen;
                    float flareSize = 3f + MathF.Sin(pulsePhase * 2f) * 1.5f;
                    int fs = (int)flareSize;
                    sb.Draw(pixel, new Rectangle((int)tip.X - fs, (int)tip.Y - fs, fs * 2, fs * 2),
                        new Color(255, 200, 100) * alpha * 0.4f);
                    sb.Draw(pixel, new Rectangle((int)tip.X - 1, (int)tip.Y - 1, 3, 3),
                        Color.White * alpha * 0.9f);
                }
                else if (FusionName == "Longinus")
                {
                    int glowH = thick + 4;
                    int coreH = MathHelper.Max(thick / 3, 2);
                    sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, len, glowH),
                        null, new Color(180, 210, 240) * alpha * 0.2f, rotation, new Vector2(0, glowH / 2f), SpriteEffects.None, 0);
                    sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, len, thick),
                        null, new Color(200, 220, 245) * alpha, rotation, new Vector2(0, thick / 2f), SpriteEffects.None, 0);
                    sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, len, coreH),
                        null, new Color(240, 245, 255) * alpha * 0.9f, rotation, new Vector2(0, coreH / 2f), SpriteEffects.None, 0);
                }
                else if (FusionName == "Impaler")
                {
                    int glowH = thick + 8;
                    int coreH = MathHelper.Max(thick / 2, 2);
                    Vector2 srcOrigin = new Vector2(0, 0.5f);
                    Color drillColor = new Color(200, 180, 140);
                    sb.Draw(pixel, Position, null, drillColor * alpha * 0.3f, rotation,
                        srcOrigin, new Vector2(len, glowH), SpriteEffects.None, 0);
                    sb.Draw(pixel, Position, null, drillColor * alpha * 0.9f, rotation,
                        srcOrigin, new Vector2(len, thick), SpriteEffects.None, 0);
                    sb.Draw(pixel, Position, null, Color.White * alpha * 0.6f, rotation,
                        srcOrigin, new Vector2(len, coreH), SpriteEffects.None, 0);
                    Vector2 beamN = BeamDirection;
                    if (beamN.LengthSquared() > 0) beamN.Normalize();
                    float perpX2 = -beamN.Y;
                    float perpY2 = beamN.X;
                    float drillSpeed = Age * 25f;
                    // Drill spiral particles along full beam length
                    for (int g = 0; g < 16; g++)
                    {
                        float t3 = ((float)g / 16f + drillSpeed * 0.1f) % 1f;
                        Vector2 gp = Position + beamN * (t3 * len);
                        float wave = MathF.Sin(drillSpeed + g * 1.2f) * (thick * 0.8f);
                        gp += new Vector2(perpX2, perpY2) * wave;
                        sb.Draw(pixel, new Rectangle((int)gp.X - 1, (int)gp.Y - 1, 3, 3),
                            new Color(255, 220, 150) * alpha * 0.7f);
                    }
                    // Spinning drill crosses along full beam length
                    for (int dc = 0; dc < 5; dc++)
                    {
                        float dt2 = (dc + 1) / 6f;
                        Vector2 crossPos = Position + beamN * (dt2 * len);
                        float crossAngle = drillSpeed * 3f + dc * 1.3f;
                        for (int arm = 0; arm < 4; arm++)
                        {
                            float a2 = crossAngle + arm * MathF.PI / 2f;
                            float radius = thick * (0.5f + 0.3f * dt2);
                            Vector2 tp = crossPos + new Vector2(MathF.Cos(a2), MathF.Sin(a2)) * radius;
                            sb.Draw(pixel, new Rectangle((int)tp.X, (int)tp.Y, 2, 2), Color.White * alpha * 0.8f);
                        }
                    }
                }
                else if (FusionName == "ThunderLance")
                {
                    int glowH = thick + 4;
                    int coreH = MathHelper.Max(thick / 3, 2);
                    Vector2 srcOrigin = new Vector2(0, 0.5f);
                    Color elecColor = new Color(100, 150, 255);
                    sb.Draw(pixel, Position, null, elecColor * alpha * 0.2f, rotation,
                        srcOrigin, new Vector2(len, glowH), SpriteEffects.None, 0);
                    sb.Draw(pixel, Position, null, elecColor * alpha * 0.8f, rotation,
                        srcOrigin, new Vector2(len, thick), SpriteEffects.None, 0);
                    sb.Draw(pixel, Position, null, new Color(200, 220, 255) * alpha * 0.7f, rotation,
                        srcOrigin, new Vector2(len, coreH), SpriteEffects.None, 0);
                    Vector2 tipPos3 = Position + BeamDirection * len / MathF.Max(BeamDirection.Length(), 0.01f);
                    float chargeSize = MathF.Min(Age * 30f, 18f);
                    float pulse = (MathF.Sin(Age * 12f) + 1f) / 2f;
                    int gs = (int)(chargeSize * 2.5f);
                    sb.Draw(pixel, new Rectangle((int)tipPos3.X - gs/2, (int)tipPos3.Y - gs/2, gs, gs),
                        elecColor * 0.15f * alpha);
                    int bs = (int)(chargeSize);
                    sb.Draw(pixel, new Rectangle((int)tipPos3.X - bs/2, (int)tipPos3.Y - bs/2, bs, bs),
                        new Color(140, 180, 255) * alpha * (0.7f + pulse * 0.3f));
                    int cs2 = (int)(chargeSize * 0.4f);
                    sb.Draw(pixel, new Rectangle((int)tipPos3.X - cs2/2, (int)tipPos3.Y - cs2/2, cs2, cs2),
                        Color.White * alpha * 0.6f);
                }
                else if (FusionName == "BindingLance")
                {
                    // Silk-wrapped lance: base beam + spiraling silk threads
                    Color silkCol = new Color(200, 180, 220);
                    Vector2 srcOrigin = new Vector2(0, 0.5f);
                    int coreH = MathHelper.Max(thick / 2, 2);
                    // Base beam (faint)
                    sb.Draw(pixel, Position, null, silkCol * alpha * 0.3f, rotation,
                        srcOrigin, new Vector2(len, thick + 2), SpriteEffects.None, 0);
                    sb.Draw(pixel, Position, null, silkCol * alpha * 0.7f, rotation,
                        srcOrigin, new Vector2(len, thick), SpriteEffects.None, 0);
                    sb.Draw(pixel, Position, null, new Color(230, 220, 245) * alpha * 0.6f, rotation,
                        srcOrigin, new Vector2(len, coreH), SpriteEffects.None, 0);
                    // Spiraling silk threads wrapping the beam
                    Vector2 beamDir = BeamDirection;
                    if (beamDir.LengthSquared() > 0.1f) beamDir.Normalize();
                    Vector2 perp = new Vector2(-beamDir.Y, beamDir.X);
                    for (int tw = 0; tw < (int)(len / 8f); tw++)
                    {
                        float t = (float)tw / (len / 8f);
                        float spiralAngle = Age * 10f + tw * 1.2f;
                        float spiralR = thick * 0.6f * MathF.Sin(spiralAngle);
                        Vector2 threadPos = Position + beamDir * (len * t) + perp * spiralR;
                        float bright = MathF.Abs(MathF.Sin(spiralAngle)) > 0.7f ? 0.9f : 0.4f;
                        sb.Draw(pixel, new Rectangle((int)threadPos.X, (int)threadPos.Y, 2, 2),
                            new Color(220, 200, 240) * (alpha * bright));
                    }
                }
                else if (FusionName == "Hydra")
                {
                    // Hydra beam: electric beam body + dragon head at tip
                    Color beamColor = new Color(100, 160, 255);
                    int glowH = thick + 4;
                    int coreH = MathHelper.Max(thick / 3, 2);
                    Vector2 srcOrigin = new Vector2(0, 0.5f);
                    // Beam body
                    sb.Draw(pixel, Position, null, beamColor * alpha * 0.2f, rotation,
                        srcOrigin, new Vector2(len, glowH), SpriteEffects.None, 0);
                    sb.Draw(pixel, Position, null, beamColor * alpha * 0.8f, rotation,
                        srcOrigin, new Vector2(len, thick), SpriteEffects.None, 0);
                    sb.Draw(pixel, Position, null, new Color(200, 220, 255) * alpha * 0.7f, rotation,
                        srcOrigin, new Vector2(len, coreH), SpriteEffects.None, 0);
                    
                    // Dragon head at tip
                    Vector2 dirN = BeamDirection;
                    if (dirN.LengthSquared() > 0.1f) dirN.Normalize();
                    Vector2 tip = Position + dirN * len;
                    float perpX = -dirN.Y, perpY = dirN.X;
                    
                    // Head: triangular snout pointing forward
                    float headLen = 10f;
                    float headW = 6f;
                    // Snout triangle (3 rects forming a wedge)
                    for (int hl = 0; hl < 5; hl++)
                    {
                        float t = (float)hl / 5f;
                        float w = headW * (1f - t * 0.7f); // tapers to point
                        Vector2 hp = tip + dirN * (hl * headLen / 5f);
                        int hw = (int)w;
                        sb.Draw(pixel, new Rectangle((int)(hp.X - perpX * hw/2 - perpY * hw/2),
                            (int)(hp.Y - perpY * hw/2 + perpX * hw/2), hw, hw),
                            beamColor * alpha);
                    }
                    
                    // Red eyes — two dots on either side of the head base
                    float eyeOff = 3f;
                    int ex1 = (int)(tip.X + perpX * eyeOff);
                    int ey1 = (int)(tip.Y + perpY * eyeOff);
                    int ex2 = (int)(tip.X - perpX * eyeOff);
                    int ey2 = (int)(tip.Y - perpY * eyeOff);
                    sb.Draw(pixel, new Rectangle(ex1, ey1, 2, 2), new Color(255, 40, 40) * alpha);
                    sb.Draw(pixel, new Rectangle(ex2, ey2, 2, 2), new Color(255, 40, 40) * alpha);
                    
                    // Jaw: open mouth with teeth — two angled rects diverging from snout tip
                    Vector2 snoutTip = tip + dirN * headLen;
                    float jawAngle = 0.25f + MathF.Sin(Age * 12f) * 0.1f; // jaw opens/closes slightly
                    // Upper jaw
                    Vector2 jawUp = snoutTip + new Vector2(
                        dirN.X * 4f + perpX * MathF.Sin(jawAngle) * 4f,
                        dirN.Y * 4f + perpY * MathF.Sin(jawAngle) * 4f);
                    sb.Draw(pixel, new Rectangle((int)snoutTip.X, (int)snoutTip.Y,
                        Math.Max(Math.Abs((int)jawUp.X - (int)snoutTip.X), 2),
                        Math.Max(Math.Abs((int)jawUp.Y - (int)snoutTip.Y), 2)),
                        new Color(180, 210, 255) * alpha);
                    // Lower jaw
                    Vector2 jawDn = snoutTip + new Vector2(
                        dirN.X * 4f - perpX * MathF.Sin(jawAngle) * 4f,
                        dirN.Y * 4f - perpY * MathF.Sin(jawAngle) * 4f);
                    sb.Draw(pixel, new Rectangle((int)snoutTip.X, (int)snoutTip.Y,
                        Math.Max(Math.Abs((int)jawDn.X - (int)snoutTip.X), 2),
                        Math.Max(Math.Abs((int)jawDn.Y - (int)snoutTip.Y), 2)),
                        new Color(180, 210, 255) * alpha);
                    // Teeth: small white dots along jaw edges
                    for (int tooth = 0; tooth < 3; tooth++)
                    {
                        float tt = (tooth + 1) / 4f;
                        Vector2 tu = Vector2.Lerp(snoutTip, jawUp, tt);
                        Vector2 td = Vector2.Lerp(snoutTip, jawDn, tt);
                        sb.Draw(pixel, new Rectangle((int)tu.X, (int)tu.Y, 1, 1), Color.White * alpha);
                        sb.Draw(pixel, new Rectangle((int)td.X, (int)td.Y, 1, 1), Color.White * alpha);
                    }
                }
                else
                {
                    // Default lance beam — 3 layers, all rotating around Position via Vector2 scale
                    Color beamColor = new Color(Params.Color.R, Params.Color.G, Params.Color.B);
                    int glowH = thick + 6;
                    int coreH = MathHelper.Max(thick / 2, 2);
                    // Origin at left-center of 1x1 pixel — all layers share exact same pivot
                    Vector2 srcOrigin = new Vector2(0, 0.5f);
                    sb.Draw(pixel, Position, null, beamColor * alpha * 0.25f, rotation,
                        srcOrigin, new Vector2(len, glowH), SpriteEffects.None, 0);
                    sb.Draw(pixel, Position, null, beamColor * alpha * 0.9f, rotation,
                        srcOrigin, new Vector2(len, thick), SpriteEffects.None, 0);
                    sb.Draw(pixel, Position, null, Color.White * alpha * 0.7f, rotation,
                        srcOrigin, new Vector2(len, coreH), SpriteEffects.None, 0);
                }
            }
            else
            {
                // Legacy beam segment fallback
                int w = (int)(s * 3f);
                int h = (int)(s);
                sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, w, h),
                    null, color * alpha, rotation, new Vector2(w / 2f, h / 2f), SpriteEffects.None, 0);
                sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, w, MathHelper.Max(h / 2, 1)),
                    null, Color.White * alpha * 0.4f, rotation, new Vector2(w / 2f, h / 4f), SpriteEffects.None, 0);
            }
        }
        else if (FusionName == "SilkSnare")
        {
            // Bouncing silk spool: rotating thread bobbin trailing silk wisps
            float rotation = Age * 8f;
            int cx = (int)Position.X, cy = (int)Position.Y;
            int spoolR = (int)(s * 0.7f);
            Color silkCol = new Color(200, 180, 220);
            Color shimmer = new Color(230, 220, 245);
            
            // Glow halo
            sb.Draw(pixel, new Rectangle(cx - spoolR - 2, cy - spoolR - 2, (spoolR + 2) * 2, (spoolR + 2) * 2),
                silkCol * 0.2f);
            // Spool body — rotating segments
            for (int seg = 0; seg < 6; seg++)
            {
                float a = rotation + seg * MathF.PI / 3f;
                int sx = cx + (int)(MathF.Cos(a) * spoolR);
                int sy = cy + (int)(MathF.Sin(a) * spoolR);
                sb.Draw(pixel, new Rectangle(sx - 1, sy - 1, 2, 2), silkCol * 0.8f);
            }
            // Bright center
            sb.Draw(pixel, new Rectangle(cx - 1, cy - 1, 2, 2), shimmer * 0.9f);
            
            // Trailing silk wisps
            if (Velocity.LengthSquared() > 1f)
            {
                var normVel = Vector2.Normalize(Velocity);
                float perpX = -normVel.Y, perpY = normVel.X;
                for (int t = 0; t < 6; t++)
                {
                    float trailT = (t + 1) * 5f;
                    float wave = MathF.Sin(Age * 12f + t * 2f) * (3f + t * 0.8f);
                    int tx = (int)(Position.X - normVel.X * trailT + perpX * wave);
                    int ty = (int)(Position.Y - normVel.Y * trailT + perpY * wave);
                    float tAlpha = 0.6f - t * 0.08f;
                    sb.Draw(pixel, new Rectangle(tx, ty, 1, 1), silkCol * tAlpha);
                }
            }
        }
        else if (FusionName == "CrystalSpiderweb")
        {
            // Crystal silk projectile: faceted gem shape with rainbow refraction
            float rotation = Age * 6f;
            int cx = (int)Position.X, cy = (int)Position.Y;
            int gemR = (int)(s * 0.8f);
            
            // Rainbow refraction glow
            float hue = (Age * 120f) % 360f;
            Color rc = HueToColor(hue);
            sb.Draw(pixel, new Rectangle(cx - gemR - 3, cy - gemR - 3, (gemR + 3) * 2, (gemR + 3) * 2), rc * 0.15f);
            
            // 6-facet gem body
            for (int f = 0; f < 6; f++)
            {
                float a = rotation + f * MathF.PI / 3f;
                int fx = cx + (int)(MathF.Cos(a) * gemR);
                int fy = cy + (int)(MathF.Sin(a) * gemR);
                float fHue = (hue + f * 60f) % 360f;
                sb.Draw(pixel, new Rectangle(fx - 1, fy - 1, 3, 3), HueToColor(fHue) * 0.7f);
            }
            // Core
            sb.Draw(pixel, new Rectangle(cx - 1, cy - 1, 3, 3), Color.White * 0.8f);
            // Facet connecting lines
            for (int f = 0; f < 6; f++)
            {
                float a1 = rotation + f * MathF.PI / 3f;
                float a2 = rotation + ((f + 1) % 6) * MathF.PI / 3f;
                int x1 = cx + (int)(MathF.Cos(a1) * gemR), y1 = cy + (int)(MathF.Sin(a1) * gemR);
                int x2 = cx + (int)(MathF.Cos(a2) * gemR), y2 = cy + (int)(MathF.Sin(a2) * gemR);
                int mx = (x1 + x2) / 2, my = (y1 + y2) / 2;
                sb.Draw(pixel, new Rectangle(mx, my, Math.Max(Math.Abs(x2 - x1), 1), Math.Max(Math.Abs(y2 - y1), 1)),
                    new Color(220, 210, 240) * 0.5f);
            }
        }
        else if (FusionName == "WebTrap")
        {
            // Web projectile: small silk ball with trailing threads
            int cx = (int)Position.X, cy = (int)Position.Y;
            Color silkCol = new Color(200, 180, 220);
            sb.Draw(pixel, new Rectangle(cx - 2, cy - 2, 4, 4), silkCol * 0.7f);
            sb.Draw(pixel, new Rectangle(cx - 1, cy - 1, 2, 2), Color.White * 0.5f);
            if (Velocity.LengthSquared() > 1f)
            {
                var normVel = Vector2.Normalize(Velocity);
                for (int t = 0; t < 4; t++)
                {
                    float trailT = (t + 1) * 4f;
                    float wave = MathF.Sin(Age * 10f + t * 3f) * 2f;
                    int tx = (int)(Position.X - normVel.X * trailT - normVel.Y * wave);
                    int ty = (int)(Position.Y - normVel.Y * trailT + normVel.X * wave);
                    sb.Draw(pixel, new Rectangle(tx, ty, 1, 1), silkCol * (0.4f - t * 0.08f));
                }
            }
        }
        else if (FusionName == "ZeusChain")
        {
            // Electric shackle: D-shaped manacle with keyhole gap, trailing chain links
            float moveAngle = Velocity.LengthSquared() > 1f ? MathF.Atan2(Velocity.Y, Velocity.X) : 0f;
            int cx = (int)Position.X, cy = (int)Position.Y;
            float alpha = 0.9f;
            
            // Electric glow halo
            int glowR = (int)(s * 1.2f);
            float glowPulse = 0.3f + 0.15f * MathF.Sin(Age * 20f);
            sb.Draw(pixel, new Rectangle(cx - glowR, cy - glowR, glowR * 2, glowR * 2),
                new Color(80, 120, 220) * glowPulse);
            
            // Shackle body: ring of 8 segments forming a rough circle with a gap at bottom
            int shackleR = (int)(s * 0.9f);
            for (int seg = 0; seg < 7; seg++) // 7 of 8 = gap at bottom
            {
                float a1 = (MathF.PI * 2f / 8) * seg + moveAngle - MathF.PI / 2f;
                int sx = cx + (int)(MathF.Cos(a1) * shackleR);
                int sy = cy + (int)(MathF.Sin(a1) * shackleR);
                // Thick metal pixel
                Color metalCol = seg < 2 || seg > 5 ? new Color(200, 210, 230) : new Color(160, 175, 200);
                sb.Draw(pixel, new Rectangle(sx - 1, sy - 1, 3, 3), metalCol * alpha);
            }
            // Keyhole: bright dot at top of shackle
            int khx = cx + (int)(MathF.Cos(moveAngle - MathF.PI / 2f) * shackleR);
            int khy = cy + (int)(MathF.Sin(moveAngle - MathF.PI / 2f) * shackleR);
            float sparkle = MathF.Sin(Age * 30f) > 0.2f ? 1f : 0.5f;
            sb.Draw(pixel, new Rectangle(khx - 1, khy - 1, 2, 2), new Color(220, 240, 255) * sparkle);
            
            // Crossbar at the opening gap
            float gapAngle = moveAngle + MathF.PI / 2f; // bottom
            int g1x = cx + (int)(MathF.Cos(gapAngle - 0.3f) * shackleR);
            int g1y = cy + (int)(MathF.Sin(gapAngle - 0.3f) * shackleR);
            int g2x = cx + (int)(MathF.Cos(gapAngle + 0.3f) * shackleR);
            int g2y = cy + (int)(MathF.Sin(gapAngle + 0.3f) * shackleR);
            sb.Draw(pixel, new Rectangle(Math.Min(g1x, g2x), Math.Min(g1y, g2y),
                Math.Max(Math.Abs(g2x - g1x), 2), Math.Max(Math.Abs(g2y - g1y), 2)),
                new Color(180, 190, 210) * alpha);
            
            // Trailing chain links behind the projectile
            if (Velocity.LengthSquared() > 1f)
            {
                var normVel = Vector2.Normalize(Velocity);
                float perpX = -normVel.Y, perpY = normVel.X;
                for (int t = 0; t < 5; t++)
                {
                    float trailT = (t + 1) * 6f;
                    float sway = MathF.Sin(Age * 15f + t * 2.3f) * (2f + t * 0.5f);
                    int tx = (int)(Position.X - normVel.X * trailT + perpX * sway);
                    int ty = (int)(Position.Y - normVel.Y * trailT + perpY * sway);
                    float tAlpha = 0.7f - t * 0.12f;
                    // Alternating link orientation
                    if (t % 2 == 0)
                        sb.Draw(pixel, new Rectangle(tx - 2, ty - 1, 4, 2), new Color(160, 180, 220) * tAlpha);
                    else
                        sb.Draw(pixel, new Rectangle(tx - 1, ty - 2, 2, 4), new Color(140, 165, 200) * tAlpha);
                }
                // Electric spark at chain tail
                float tailSpark = MathF.Sin(Age * 25f) > 0.5f ? 0.8f : 0.2f;
                int tailX = (int)(Position.X - normVel.X * 32f);
                int tailY = (int)(Position.Y - normVel.Y * 32f);
                sb.Draw(pixel, new Rectangle(tailX, tailY, 2, 2), new Color(200, 230, 255) * tailSpark);
            }
        }
        else if (FusionName == "Morrigan's Court")
        {
            // Spectral phantom: hooded figure shape — tall narrow body + wider hood
            float bob = MathF.Sin(Age * 6f + OrbitAngle * 3f) * 2f; // gentle bob
            int bodyW = (int)(s * 0.5f);
            int bodyH = (int)(s * 1.4f);
            int hoodW = (int)(s * 0.9f);
            int hoodH = (int)(s * 0.5f);
            float alpha = 0.7f + MathF.Sin(Age * 4f) * 0.2f; // shimmer
            // Body
            sb.Draw(pixel, new Rectangle((int)(Position.X - bodyW / 2), (int)(Position.Y - bodyH / 2 + bob),
                bodyW, bodyH), color * alpha);
            // Hood
            sb.Draw(pixel, new Rectangle((int)(Position.X - hoodW / 2), (int)(Position.Y - bodyH / 2 - hoodH / 3 + bob),
                hoodW, hoodH), color * (alpha * 0.8f));
        }
        else if (FusionName == "Gebel's Swarm")
        {
            // Bat: wide body that flaps (height oscillates)
            float flap = MathF.Abs(MathF.Sin(Age * 18f)); // 0-1 flap cycle
            float rotation = MathF.Atan2(Velocity.Y, Velocity.X);
            int w = (int)(s * 2f); // wide wingspan
            int h = MathHelper.Max((int)(s * (0.3f + flap * 0.7f)), 2); // flaps from thin to full
            // Wing shadow (slightly larger, darker)
            sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, w + 2, h + 1),
                null, new Color(40, 10, 50) * 0.5f, rotation, new Vector2((w + 2) / 2f, (h + 1) / 2f), SpriteEffects.None, 0);
            // Main bat body
            sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, w, h),
                null, color, rotation, new Vector2(w / 2f, h / 2f), SpriteEffects.None, 0);
        }
        else if (FusionName == "Senbonzakura")
        {
            // Cherry blossom petal: elongated diamond shape, rotates slowly as it floats
            float petalRot = Age * 4f + Position.X * 0.05f; // brisk tumble
            int petalLen = (int)(s * 1.3f);
            int petalW = MathHelper.Max((int)(s * 0.4f), 1);
            float fadeAlpha = MathHelper.Clamp((Lifetime - Age) * 4f, 0f, 1f);
            
            // Outer glow
            sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, petalLen + 2, petalW + 2),
                null, color * 0.3f * fadeAlpha, petalRot, new Vector2((petalLen + 2) / 2f, (petalW + 2) / 2f), SpriteEffects.None, 0);
            // Petal body
            sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, petalLen, petalW),
                null, color * fadeAlpha, petalRot, new Vector2(petalLen / 2f, petalW / 2f), SpriteEffects.None, 0);
            // White edge highlight
            sb.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, petalLen - 1, MathHelper.Max(1, petalW / 3)),
                null, Color.White * 0.4f * fadeAlpha, petalRot, new Vector2((petalLen - 1) / 2f, petalW / 6f), SpriteEffects.None, 0);
        }
        else
        {
            // Seed in arc: draw elevated with shadow below
            if (ArcDuration > 0 && !ArcLanded)
            {
                float h = ArcHeight;
                // Ground shadow (grows as seed gets closer to landing)
                float t = MathHelper.Clamp(ArcTimer / ArcDuration, 0f, 1f);
                float shadowSize = 3f + t * 5f;
                float shadowAlpha = 0.15f + t * 0.25f;
                sb.Draw(pixel, new Rectangle((int)(Position.X - shadowSize/2), (int)(Position.Y - shadowSize/4),
                    (int)shadowSize, (int)(shadowSize * 0.5f)), Color.Black * shadowAlpha);
                // Seed body drawn elevated
                sb.Draw(pixel, new Rectangle((int)(Position.X - s / 2), (int)(Position.Y - h - s / 2),
                    (int)s, (int)s), color);
                // Glow trail during flight
                sb.Draw(pixel, new Rectangle((int)(Position.X - s * 0.3f), (int)(Position.Y - h - s * 0.3f),
                    (int)(s * 0.6f), (int)(s * 0.6f)), color * 0.4f);
            }
            // Seed landed/stationary: pulsing countdown
            else if (Params.IsStationary && ArcDuration > 0)
            {
                float lifeLeft = Lifetime - Age;
                float lifePct = MathHelper.Clamp(lifeLeft / Lifetime, 0f, 1f);
                bool armed = Age > 0.5f;
                // Arming phase: grow from dim to bright over 0.5s
                float armAlpha = armed ? 1f : MathHelper.Clamp(Age / 0.5f, 0.2f, 1f);
                // Pulse frequency increases as lifetime decreases
                float freq = armed ? 3f + (1f - lifePct) * 15f : 1f; // slow pulse during arming
                float pulse = (MathF.Sin(Age * freq) + 1f) / 2f;
                float pulseScale = 1f + pulse * 0.3f;
                float ps = s * pulseScale;
                // Outer glow (pulsing)
                sb.Draw(pixel, new Rectangle((int)(Position.X - ps * 0.7f), (int)(Position.Y - ps * 0.7f),
                    (int)(ps * 1.4f), (int)(ps * 1.4f)), color * (0.15f + pulse * 0.2f) * armAlpha);
                // Core
                sb.Draw(pixel, new Rectangle((int)(Position.X - ps / 2), (int)(Position.Y - ps / 2),
                    (int)ps, (int)ps), color * armAlpha);
                // Bright center
                float cs = ps * 0.4f;
                sb.Draw(pixel, new Rectangle((int)(Position.X - cs / 2), (int)(Position.Y - cs / 2),
                    (int)cs, (int)cs), Color.White * (0.3f + pulse * 0.4f) * armAlpha);
                // Warning ring when close to detonation
                if (lifeLeft < 2f)
                {
                    float ring = s * (2f + pulse * 1.5f);
                    sb.Draw(pixel, new Rectangle((int)(Position.X - ring / 2), (int)(Position.Y - 1),
                        (int)ring, 2), color * (0.3f + pulse * 0.4f));
                    sb.Draw(pixel, new Rectangle((int)(Position.X - 1), (int)(Position.Y - ring / 2),
                        2, (int)ring), color * (0.3f + pulse * 0.4f));
                }
            }
            else
            {
                sb.Draw(pixel, new Rectangle((int)(Position.X - s / 2), (int)(Position.Y - s / 2),
                    (int)s, (int)s), color);
            }
        }
    }
    
    private static Color HueToColor(float hue)
    {
        float h = (hue % 360f) / 60f;
        int hi = (int)h % 6;
        float f = h - (int)h;
        byte v = 255, p = 0, q = (byte)(255 * (1f - f)), t = (byte)(255 * f);
        return hi switch
        {
            0 => new Color(v, t, p), 1 => new Color(q, v, p), 2 => new Color(p, v, t),
            3 => new Color(p, q, v), 4 => new Color(t, p, v), _ => new Color(v, p, q),
        };
    }
}
