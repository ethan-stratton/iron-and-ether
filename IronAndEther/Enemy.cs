using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronAndEther;

public enum EnemyType
{
    Squire,         // Basic 4-way shooter, weak
    GuardGargoyle,  // Hops erratically, AOE burst on land
    HollowPlate,    // Pursues, backpedals & fires seeking shots
    ShieldMaiden,   // Has shield facing player, guards allies, slow turn
    HedgeSniper,    // Stationary/hidden, pops up to fire fast bolt
    TowerGuard,     // Slow shielded, 3-shot burst
    Webspinner,     // Ceiling spider — drops down, weaves webs, retreats
    Thief,          // Spawns when player returns to room with dropped items, steals loot, runs away
    Maw,            // Living darkness — ether drain aura, bite attack, laugh projectile spread
    Charger,        // Telegraphs with flash, then lunges across screen — CotM Were-Jaguar
    Gremlin,        // Tiny, fast, erratic bouncing — CotM Fleaman
    Wraith,         // Teleports near player, slashes, vanishes — CotM Devil
}

public class Enemy
{
    private static int _nextId = 0;
    public int Id;
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 WallSlideDir;  // wall avoidance: slide direction when blocked
    public float WallSlideTimer;  // how long to keep sliding
    public Vector2 MoveDirection; // smoothed movement direction for visual lerp
    public bool WallSlideStarted; // flag for dust puff on first frame of slide
    public float Hp;
    public float MaxHp;
    public float Size = 14f;
    public bool Alive = true;
    public float Speed = 80f;
    public float HitFlash;
    public Color BaseColor = new Color(220, 50, 50);

    public Form DropForm = Form.None;
    public Behavior DropBehavior = Behavior.None;
    public Essence DropEssence = Essence.None;

    // Status effects
    public float SlowTimer;
    public float SlowAmount = 0.5f;
    public float StaggerTimer;
    public float IgniteTimer;
    public float IgniteDps = 5f;
    public float FearTimer;
    public float ConfuseTimer; // maze confusion: wander randomly
    public float SilenceTimer;
    public float LeechStack;
    
    // Hit juice
    public Vector2 KnockbackVel;
    public float SquashTimer; // shrink on hit, spring back
    public float HitCooldown; // multi-hit cooldown (Impaler drill etc.)
    
    // Spawn animation
    public float SpawnTimer; // countdown during spawn (invulnerable + no AI)
    public float SpawnDuration; // total spawn time for lerp
    public bool Spawning => SpawnTimer > 0;

    // Possess
    public bool Possessed;
    public bool Pacified; // Mollificare: can't die, won't attack, until hit again
    public float PossessTimer;
    public string FusionTag; // for special behaviors (e.g., "Saibaman" self-destruct)
    public string KilledByFusion; // track which fusion dealt the killing blow
    public float ShriekerTimer; // Gae Bolg: time left as a shrieker pulsing hazard
    public float ShriekerPulseTimer; // pulse cooldown

    // Stagger/Break system
    public float Stagger;        // current stagger buildup
    public float MaxStagger;     // threshold to trigger Break
    public float BreakTimer;     // >0 = in Break state (2.5 seconds)
    public bool IsBroken => BreakTimer > 0;
    public Essence Weakness = Essence.None; // element that builds stagger 2x faster
    public Essence Weakness2 = Essence.None; // secondary weakness for dual-type enemies
    public Essence ElementType = Essence.None; // visual element identity (glow/particles)
    public int FractureStacks;   // Bone essence: next N hits get +50% stagger

    // Stasis
    public float StasisTimer;
    
    // Type & AI
    public EnemyType Type = EnemyType.Squire;
    public float ShootTimer;        // countdown to next shot
    public float FacingAngle;       // direction enemy is "looking" (shield maiden, etc.)
    public float AiStateTimer;      // general purpose AI timer
    public int AiPhase;             // 0=idle, 1=active, etc. (type-specific)
    public Vector2 AiTargetPos;     // Target position for movement AI (Gremlin jump, etc.)
    public bool ShieldUp = true;    // for shielded enemies
    public float TurnSpeed = 8f;    // radians/sec for shield maiden
    public Vector2 HopTarget;       // gargoyle hop destination
    public float DistanceTraveled;   // cumulative movement for leg animation (Webspinner)
    public Vector2 PrevPosition;     // previous frame position for distance tracking
    public string Bark;              // floating text bark (set by AI, drawn + cleared by Game1)
    public float BarkTimer;          // time remaining for current bark display
    
    // Pending shots: Game1 reads and clears these each frame
    public struct PendingShot
    {
        public Vector2 Direction;
        public float Speed;
        public float Damage;
        public float Size;
        public float Lifetime;
        public bool Seeking; // gently homes toward player
    }
    public List<PendingShot> PendingShots = new();

    public Enemy(Vector2 pos, float hp)
    {
        Id = _nextId++;
        Position = pos;
        Hp = hp;
        MaxHp = hp;
        MaxStagger = hp * 0.35f;
    }

    public void Update(float dt, Vector2 playerPos, Rectangle arena = default)
    {
        if (!Alive) return;
        if (SpawnTimer > 0) { SpawnTimer -= dt; return; } // spawning — no AI, no movement
        if (HitFlash > 0) HitFlash -= dt;
        if (SquashTimer > 0) SquashTimer -= dt;
        if (HitCooldown > 0) HitCooldown -= dt;
        
        // Track distance for procedural leg animation
        DistanceTraveled += Vector2.Distance(Position, PrevPosition);
        PrevPosition = Position;
        
        // Knockback: apply and decay
        if (KnockbackVel.LengthSquared() > 1f)
        {
            Position += KnockbackVel * dt;
            KnockbackVel *= MathF.Pow(0.02f, dt); // fast exponential decay
        }

        // Stasis: completely frozen
        if (StasisTimer > 0)
        {
            StasisTimer -= dt;
            return;
        }

        // Status timers
        if (SlowTimer > 0) SlowTimer -= dt;
        if (StaggerTimer > 0) StaggerTimer -= dt;
        if (BreakTimer > 0)
        {
            BreakTimer -= dt;
            if (BreakTimer <= 0) Stagger = 0;
        }
        if (IgniteTimer > 0)
        {
            IgniteTimer -= dt;
            Hp -= IgniteDps * dt;
            if (!IsBroken) Stagger += 2f * dt; // slow stagger buildup from burning
            if (Hp <= 0) { Alive = false; return; }
        }
        if (FearTimer > 0) FearTimer -= dt;
        if (SilenceTimer > 0) SilenceTimer -= dt;
        
        // Gae Bolg Shrieker: frozen in place, pulsing electricity
        if (ShriekerTimer > 0)
        {
            ShriekerTimer -= dt;
            ShriekerPulseTimer -= dt;
            StasisTimer = ShriekerTimer; // keep frozen
            Speed = 0;
            if (ShriekerTimer <= 0) { Alive = false; } // dies when shrieker expires
        }

        // Possess timer
        if (Possessed)
        {
            PossessTimer -= dt;
            if (PossessTimer <= 0) { Alive = false; return; }
        }

        if (StaggerTimer > 0) return; // stunned
        if (Pacified)
        {
            // Gentle wander — slow languid movement
            float wanderSpeed = 18f;
            AiStateTimer -= dt;
            if (AiStateTimer <= 0)
            {
                // Pick a new random wander direction
                AiPhase++;
                int hash = Id * 7919 + AiPhase * 104729;
                float angle = (hash & 0xFFFF) / 65535f * MathF.PI * 2f;
                AiStateTimer = 2f + ((hash >> 16) & 0xFF) / 255f * 2f;
                Velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * wanderSpeed;
            }
            
            // Gradual deceleration between direction changes (languid drift)
            Velocity *= (1f - 0.3f * dt);
            
            // Apply movement
            Position += Velocity * dt;
            
            // Wall avoidance — clamp to arena and bounce
            if (Position.X < arena.Left + Size) { Position = new Vector2(arena.Left + Size, Position.Y); Velocity = new Vector2(MathF.Abs(Velocity.X), Velocity.Y); }
            if (Position.X > arena.Right - Size) { Position = new Vector2(arena.Right - Size, Position.Y); Velocity = new Vector2(-MathF.Abs(Velocity.X), Velocity.Y); }
            if (Position.Y < arena.Top + Size) { Position = new Vector2(Position.X, arena.Top + Size); Velocity = new Vector2(Velocity.X, MathF.Abs(Velocity.Y)); }
            if (Position.Y > arena.Bottom - Size) { Position = new Vector2(Position.X, arena.Bottom - Size); Velocity = new Vector2(Velocity.X, -MathF.Abs(Velocity.Y)); }
            
            return;
        }
        
        ShootTimer -= dt;
        AiStateTimer -= dt;
        if (WallSlideTimer > 0) WallSlideTimer -= dt;
        if (WallSlideTimer <= 0) WallSlideStarted = false;

        Vector2 toPlayer = playerPos - Position;
        float dist = toPlayer.Length();
        Vector2 dir = dist > 1f ? toPlayer / dist : Vector2.UnitX;
        float moveSpeed = Speed;
        if (SlowTimer > 0) moveSpeed *= SlowAmount;

        if (Possessed)
        {
            // Movement handled by Game1 (seek enemies)
        }
        else if (FearTimer > 0)
        {
            Position -= dir * moveSpeed * dt;
        }
        else if (WallSlideTimer > 0)
        {
            // Slide along wall: combine slide direction with player direction
            Vector2 slideDir = WallSlideDir;
            float dot = Vector2.Dot(slideDir, dir);
            if (dot < 0) slideDir = -slideDir;
            // Ease-in on slide start: blend increases from 0.5 to 0.8 over timer
            float slideStrength = 0.5f + 0.3f * (1f - WallSlideTimer / 0.4f);
            Vector2 targetDir = Vector2.Normalize(slideDir * slideStrength + dir * (1f - slideStrength));
            // Smooth turn: lerp MoveDirection toward target
            if (MoveDirection.LengthSquared() < 0.01f) MoveDirection = dir;
            MoveDirection = Vector2.Lerp(MoveDirection, targetDir, dt * 8f); // smooth 8x/s
            if (MoveDirection.LengthSquared() > 0.01f)
                MoveDirection = Vector2.Normalize(MoveDirection);
            // Ease speed: start slower (SmoothStart), ramp up
            float slideProgress = 1f - (WallSlideTimer / 0.4f);
            float speedMult = 0.6f + 0.4f * slideProgress * slideProgress; // quadratic ease-in
            Position += MoveDirection * moveSpeed * speedMult * dt;
            WallSlideStarted = true;
        }
        else
        {
            UpdateAI(dt, playerPos, dir, dist, moveSpeed, arena);
        }
    }
    
    private void UpdateAI(float dt, Vector2 playerPos, Vector2 dirToPlayer, float dist, float moveSpeed, Rectangle arena)
    {
        switch (Type)
        {
            case EnemyType.Squire:
                // Walk toward player, fire 4-way every 2s
                if (dist > 30f) Position += dirToPlayer * moveSpeed * dt;
                if (ShootTimer <= 0)
                {
                    ShootTimer = 2f;
                    for (int i = 0; i < 4; i++)
                    {
                        float a = i * MathF.PI / 2f;
                        PendingShots.Add(new PendingShot
                        {
                            Direction = new Vector2(MathF.Cos(a), MathF.Sin(a)),
                            Speed = 120f, Damage = 5f, Size = 4f, Lifetime = 2f,
                        });
                    }
                }
                break;
                
            case EnemyType.GuardGargoyle:
                // Phase 0: pick hop target. Phase 1: hop. Phase 2: land burst.
                if (AiPhase == 0)
                {
                    // Pick random spot near player
                    float a = (float)new Random(Id + (int)(AiStateTimer * 100)).NextDouble() * MathF.PI * 2f;
                    float hopDist = 60f + (float)new Random(Id + 7).NextDouble() * 80f;
                    HopTarget = playerPos + new Vector2(MathF.Cos(a), MathF.Sin(a)) * hopDist;
                    AiPhase = 1;
                    AiStateTimer = 0.4f; // hop duration
                }
                else if (AiPhase == 1)
                {
                    // Hop toward target
                    Vector2 toTarget = HopTarget - Position;
                    float tDist = toTarget.Length();
                    if (tDist > 5f)
                        Position += (toTarget / tDist) * moveSpeed * 2.5f * dt; // fast hop
                    if (AiStateTimer <= 0)
                    {
                        // Land: AOE burst
                        AiPhase = 2;
                        AiStateTimer = 1.2f; // cooldown before next hop
                        if (BarkTimer <= 0) { Bark = "THOOM"; BarkTimer = 0.6f; }
                        int burstCount = 8;
                        for (int i = 0; i < burstCount; i++)
                        {
                            float ba = i * MathF.PI * 2f / burstCount;
                            PendingShots.Add(new PendingShot
                            {
                                Direction = new Vector2(MathF.Cos(ba), MathF.Sin(ba)),
                                Speed = 100f, Damage = 8f, Size = 5f, Lifetime = 1.5f,
                            });
                        }
                    }
                }
                else // Phase 2: resting on ground
                {
                    if (AiStateTimer <= 0) AiPhase = 0;
                }
                break;
                
            case EnemyType.HollowPlate:
                // Pursue until close, then backpedal and fire seeking shots
                if (dist > 120f)
                {
                    if (dist < 200f && BarkTimer <= 0 && AiPhase == 0) { Bark = "...found you."; BarkTimer = 1.5f; AiPhase = 1; }
                    Position += dirToPlayer * moveSpeed * dt;
                }
                else
                {
                    // Backpedal
                    Position -= dirToPlayer * moveSpeed * 0.6f * dt;
                    if (ShootTimer <= 0)
                    {
                        ShootTimer = 1.5f;
                        PendingShots.Add(new PendingShot
                        {
                            Direction = dirToPlayer,
                            Speed = 90f, Damage = 10f, Size = 5f, Lifetime = 3f,
                            Seeking = true,
                        });
                    }
                }
                break;
                
            case EnemyType.ShieldMaiden:
                // Slowly turn shield toward player, walk toward nearest ally
                float targetAngle = MathF.Atan2(dirToPlayer.Y, dirToPlayer.X);
                float angleDiff = targetAngle - FacingAngle;
                // Normalize to -PI..PI
                while (angleDiff > MathF.PI) angleDiff -= MathF.PI * 2f;
                while (angleDiff < -MathF.PI) angleDiff += MathF.PI * 2f;
                FacingAngle += MathHelper.Clamp(angleDiff, -TurnSpeed * dt, TurnSpeed * dt);
                
                // Walk toward player but stop at mid range
                if (dist > 80f)
                    Position += dirToPlayer * moveSpeed * 0.7f * dt;
                break;
                
            case EnemyType.HedgeSniper:
                // Phase 0: hidden (no movement). Phase 1: pop up, shoot, hide again.
                if (AiPhase == 0)
                {
                    // Hidden — wait for timer
                    if (AiStateTimer <= 0 && dist < 300f)
                    {
                        AiPhase = 1;
                        AiStateTimer = 0.3f; // pop-up duration before shot
                        Bark = "..."; BarkTimer = 0.4f;
                    }
                }
                else if (AiPhase == 1)
                {
                    if (AiStateTimer <= 0)
                    {
                        // Fire fast bolt at player
                        PendingShots.Add(new PendingShot
                        {
                            Direction = dirToPlayer,
                            Speed = 300f, Damage = 15f, Size = 3f, Lifetime = 1.5f,
                        });
                        AiPhase = 2;
                        AiStateTimer = 0.2f; // brief visible after shot
                    }
                }
                else // Phase 2: visible cooldown, then hide
                {
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 0;
                        AiStateTimer = 2.5f + (Id % 3) * 0.5f; // stagger hide timers
                    }
                }
                break;
                
            case EnemyType.TowerGuard:
                // Slow pursuit, shield blocks frontal damage, 3-shot burst
                if (dist > 60f)
                    Position += dirToPlayer * moveSpeed * 0.4f * dt; // very slow
                FacingAngle = MathF.Atan2(dirToPlayer.Y, dirToPlayer.X);
                
                if (ShootTimer <= 0)
                {
                    ShootTimer = 2.5f;
                    float spread = 0.2f;
                    for (int i = -1; i <= 1; i++)
                    {
                        float a = MathF.Atan2(dirToPlayer.Y, dirToPlayer.X) + i * spread;
                        PendingShots.Add(new PendingShot
                        {
                            Direction = new Vector2(MathF.Cos(a), MathF.Sin(a)),
                            Speed = 140f, Damage = 8f, Size = 4f, Lifetime = 2f,
                        });
                    }
                }
                break;
                
            case EnemyType.Webspinner:
                // Phase 0: hidden on ceiling, wait then drop
                // Phase 1: active — chase and fire webs
                // Phase 2: retreating to ceiling
                bool canShootWeb = StaggerTimer <= 0 && !IsBroken && FearTimer <= 0 && SilenceTimer <= 0;
                if (AiPhase == 0)
                {
                    // Hidden on ceiling — wait for timer AND player proximity
                    if (AiStateTimer <= 0 && dist < 200f)
                    {
                        AiPhase = 1;
                        AiStateTimer = 3f + (Id % 3) * 0.33f; // 3-4s active
                        ShootTimer = 0.5f; // brief delay before first web
                        string[] spiderBarks = { "*click click*", "...", "*hiss*" };
                        Bark = spiderBarks[Id % spiderBarks.Length]; BarkTimer = 0.8f;
                    }
                }
                else if (AiPhase == 1)
                {
                    // Active: move toward player at 70% speed
                    if (dist > 25f)
                        Position += dirToPlayer * moveSpeed * 0.7f * dt;
                    
                    // Fire web every 1.5s
                    if (ShootTimer <= 0 && canShootWeb)
                    {
                        ShootTimer = 1.5f;
                        PendingShots.Add(new PendingShot
                        {
                            Direction = dirToPlayer,
                            Speed = 100f, Damage = 0f, Size = 6f, Lifetime = 3f,
                            Seeking = false,
                        });
                    }
                    
                    // After active timer expires, retreat
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 2;
                        AiStateTimer = 0.8f; // retreat duration
                    }
                }
                else // Phase 2: retreating
                {
                    // Move away from player at 120% speed
                    Position -= dirToPlayer * moveSpeed * 1.2f * dt;
                    
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 0;
                        AiStateTimer = 2f + (Id % 5) * 0.5f; // 2-4s hidden
                    }
                }
                break;
            
            case EnemyType.Thief:
                // Phase 0: Rummaging — bobbing in place at drop site, unaware
                // Phase 1: Startled — brief "!" freeze
                // Phase 2: Fleeing — runs away with wall avoidance, countdown to escape
                // Phase 3: Escape — jump + smoke bomb + vanish
                if (AiPhase == 0)
                {
                    AiStateTimer += dt;
                    // Rummaging bob — stays in place
                    Position.Y += MathF.Sin(AiStateTimer * 4f) * 0.3f;
                    
                    if (dist < 120f)
                    {
                        AiPhase = 1;
                        AiStateTimer = 0.4f;
                        Bark = "!"; BarkTimer = 0.8f;
                    }
                }
                else if (AiPhase == 1)
                {
                    AiStateTimer -= dt;
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 2;
                        AiStateTimer = 8f + (Id % 3); // countdown: 8-10s before escape
                        string[] fleeBarks = { "Mine! MINE!", "Finders keepers!", "You threw it away!", "No take-backs!" };
                        Bark = fleeBarks[Id % fleeBarks.Length]; BarkTimer = 1.5f;
                    }
                }
                else if (AiPhase == 2)
                {
                    // AiStateTimer counts down automatically (line 181)
                    
                    // Escape when timer expires
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 3;
                        AiStateTimer = 0.6f; // escape animation duration
                        Bark = "Heh."; BarkTimer = 0.8f;
                        break;
                    }
                    
                    // Flee with proper wall avoidance
                    if (dist > 1f)
                    {
                        Vector2 fleeDir = -dirToPlayer;
                        
                        // Strong wall repulsion — push away from walls
                        float wallMargin = 60f;
                        Vector2 wallPush = Vector2.Zero;
                        float lDist = Position.X - arena.Left;
                        float rDist = arena.Right - Position.X;
                        float tDist = Position.Y - arena.Top;
                        float bDist = arena.Bottom - Position.Y;
                        if (lDist < wallMargin) wallPush.X += MathF.Pow(1f - lDist / wallMargin, 2) * 3f;
                        if (rDist < wallMargin) wallPush.X -= MathF.Pow(1f - rDist / wallMargin, 2) * 3f;
                        if (tDist < wallMargin) wallPush.Y += MathF.Pow(1f - tDist / wallMargin, 2) * 3f;
                        if (bDist < wallMargin) wallPush.Y -= MathF.Pow(1f - bDist / wallMargin, 2) * 3f;
                        
                        Vector2 moveDir = fleeDir + wallPush;
                        if (moveDir.LengthSquared() > 0.01f) moveDir = Vector2.Normalize(moveDir);
                        Position += moveDir * moveSpeed * 1.8f * dt;
                    }
                    
                    // Hard clamp to arena
                    Position.X = MathHelper.Clamp(Position.X, arena.Left + 12, arena.Right - 12);
                    Position.Y = MathHelper.Clamp(Position.Y, arena.Top + 12, arena.Bottom - 12);
                    
                    // Throw caltrops behind every 1.5s
                    if (ShootTimer <= 0 && dist < 250f)
                    {
                        ShootTimer = 1.5f;
                        PendingShots.Add(new PendingShot
                        {
                            Direction = -dirToPlayer,
                            Speed = 60f, Damage = 3f, Size = 3f, Lifetime = 3f,
                        });
                    }
                    
                    // Hit bark (using ShieldUp as "has barked on hit" flag)
                    // ShieldUp is reset each time thief takes damage in Game1
                }
                else if (AiPhase == 3)
                {
                    // Escape animation: rise upward, then vanish
                    AiStateTimer -= dt;
                    Position.Y -= 120f * dt; // jump upward
                    if (AiStateTimer <= 0)
                    {
                        // Vanish — mark as dead but don't drop loot (thief escapes with it)
                        Alive = false;
                        Hp = 0;
                        // PendingShots signals Game1 to spawn smoke + smiley via special marker
                        PendingShots.Add(new PendingShot { Direction = Vector2.Zero, Speed = -1f, Damage = 0, Size = 0, Lifetime = 0 });
                    }
                }
                break;
                
            case EnemyType.Maw:
                // Phase 0: Idle drift — slow creeping toward player with occasional teleport
                // Phase 1: Bite telegraph — jaw opens over 0.6s
                // Phase 2: Bite snap — lunge to player position
                // Phase 3: Laugh — spread of dark projectiles
                
                float mawDrift = moveSpeed * 0.4f;
                
                if (AiPhase == 0)
                {
                    // Slow menacing drift toward player
                    if (dist > 40f)
                        Position += dirToPlayer * mawDrift * dt;
                    
                    // Random short-range teleport (blink)
                    if (AiStateTimer <= 0 && dist > 100f)
                    {
                        // Teleport closer
                        Vector2 blinkTarget = playerPos + (-dirToPlayer) * (dist * 0.4f);
                        blinkTarget.X += (Id * 73 % 100 - 50) * 0.5f; // slight offset
                        blinkTarget.Y += (Id * 37 % 100 - 50) * 0.5f;
                        blinkTarget.X = MathHelper.Clamp(blinkTarget.X, arena.Left + 30, arena.Right - 30);
                        blinkTarget.Y = MathHelper.Clamp(blinkTarget.Y, arena.Top + 30, arena.Bottom - 30);
                        Position = blinkTarget;
                        AiStateTimer = 4f + (Id % 3); // 4-6s between blinks
                    }
                    
                    // Choose next attack
                    if (ShootTimer <= 0)
                    {
                        if (dist < 120f)
                        {
                            AiPhase = 1; // bite
                            AiStateTimer = 0.6f; // telegraph duration
                        }
                        else
                        {
                            AiPhase = 3; // laugh
                            AiStateTimer = 0.4f; // wind-up
                        }
                        ShootTimer = 2.5f;
                    }
                }
                else if (AiPhase == 1)
                {
                    // Bite telegraph — jaw opens, facing player
                    FacingAngle = MathF.Atan2(dirToPlayer.Y, dirToPlayer.X);
                    // AiStateTimer counts down automatically
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 2;
                        AiStateTimer = 0.15f; // snap duration
                        // Lunge toward player
                        KnockbackVel = dirToPlayer * 400f;
                    }
                }
                else if (AiPhase == 2)
                {
                    // Bite snap — damage dealt in Game1 (check proximity)
                    FacingAngle = MathF.Atan2(dirToPlayer.Y, dirToPlayer.X);
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 0;
                        AiStateTimer = 3f + (Id % 2);
                        ShootTimer = 2f;
                    }
                }
                else if (AiPhase == 3)
                {
                    // Laugh — fire spread of dark orbs
                    FacingAngle = MathF.Atan2(dirToPlayer.Y, dirToPlayer.X);
                    if (AiStateTimer <= 0)
                    {
                        float baseAngle = MathF.Atan2(dirToPlayer.Y, dirToPlayer.X);
                        int orbCount = 7;
                        float spreadAngle = MathF.PI * 0.5f; // 90 degree spread
                        for (int o = 0; o < orbCount; o++)
                        {
                            float angle = baseAngle - spreadAngle / 2f + (spreadAngle * o / (orbCount - 1));
                            Vector2 dir = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
                            PendingShots.Add(new PendingShot
                            {
                                Direction = dir,
                                Speed = 80f,
                                Damage = 8f,
                                Size = 6f,
                                Lifetime = 3f,
                            });
                        }
                        Bark = "Ha ha ha..."; BarkTimer = 1.2f;
                        AiPhase = 0;
                        AiStateTimer = 3f;
                        ShootTimer = 3f;
                    }
                }
                break;
            
            case EnemyType.Charger:
                // Phase 0: Prowl — strafe around player at medium distance
                // Phase 1: Telegraph — flash red, lock aim direction
                // Phase 2: Lunge — dash across screen in locked direction
                // Phase 3: Recovery — stunned briefly after charge
                
                if (AiPhase == 0)
                {
                    // Circle-strafe around player
                    float strafeAngle = MathF.Atan2(Position.Y - playerPos.Y, Position.X - playerPos.X);
                    strafeAngle += 1.5f * dt; // orbit speed
                    float orbitDist = 180f;
                    Vector2 orbitTarget = playerPos + new Vector2(MathF.Cos(strafeAngle), MathF.Sin(strafeAngle)) * orbitDist;
                    Vector2 toOrbit = orbitTarget - Position;
                    if (toOrbit.LengthSquared() > 1f)
                        Position += Vector2.Normalize(toOrbit) * moveSpeed * 1.2f * dt;
                    FacingAngle = MathF.Atan2(dirToPlayer.Y, dirToPlayer.X);
                    
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 1;
                        AiStateTimer = 0.6f; // telegraph duration
                        // Lock charge direction
                        FacingAngle = MathF.Atan2(dirToPlayer.Y, dirToPlayer.X);
                    }
                }
                else if (AiPhase == 1)
                {
                    // Telegraph — hold still, flash handled in draw
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 2;
                        AiStateTimer = 0.3f; // charge duration
                        KnockbackVel = new Vector2(MathF.Cos(FacingAngle), MathF.Sin(FacingAngle)) * 900f;
                    }
                }
                else if (AiPhase == 2)
                {
                    // Lunging — knockback velocity carries us, damage in Game1
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 3;
                        AiStateTimer = 0.8f; // recovery stun
                        KnockbackVel = Vector2.Zero;
                    }
                }
                else if (AiPhase == 3)
                {
                    // Recovery — dazed, vulnerable
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 0;
                        AiStateTimer = 2f + (Id % 3); // 2-4s before next charge
                    }
                }
                break;
            
            case EnemyType.Gremlin:
                // Erratic bouncing — hops in random arcs toward player, fast, tiny
                // Phase 0: On ground, pick jump target
                // Phase 1: In air (arc jump)
                
                if (AiPhase == 0)
                {
                    // Brief pause on ground
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 1;
                        // Jump toward player with randomization
                        float jumpAngle = MathF.Atan2(dirToPlayer.Y, dirToPlayer.X);
                        jumpAngle += ((Id * 131 + (int)(ShootTimer * 100)) % 100 - 50) * 0.02f; // ±~57° random spread
                        float jumpDist = 60f + (Id * 47 % 80); // 60-140px jump distance
                        AiTargetPos = Position + new Vector2(MathF.Cos(jumpAngle), MathF.Sin(jumpAngle)) * jumpDist;
                        // Clamp to arena
                        AiTargetPos = new Vector2(
                            MathHelper.Clamp(AiTargetPos.X, arena.Left + 20, arena.Right - 20),
                            MathHelper.Clamp(AiTargetPos.Y, arena.Top + 20, arena.Bottom - 20));
                        AiStateTimer = 0.25f; // jump duration
                        ShootTimer += 1f; // use as hop counter for randomness seed
                    }
                }
                else if (AiPhase == 1)
                {
                    // Arc toward target — linear interp with vertical offset for "hop" feel
                    float jumpProg = 1f - (AiStateTimer / 0.25f);
                    jumpProg = MathHelper.Clamp(jumpProg, 0f, 1f);
                    Vector2 start = Position;
                    Vector2 toTarget = AiTargetPos - Position;
                    float step = moveSpeed * 2.5f * dt;
                    if (toTarget.LengthSquared() > step * step)
                        Position += Vector2.Normalize(toTarget) * step;
                    
                    if (AiStateTimer <= 0 || Vector2.Distance(Position, AiTargetPos) < 8f)
                    {
                        Position = AiTargetPos;
                        AiPhase = 0;
                        AiStateTimer = 0.3f + (Id * 23 % 30) * 0.01f; // 0.3-0.6s ground pause
                        
                        // Shoot on landing occasionally
                        if ((int)(ShootTimer) % 3 == 0)
                        {
                            PendingShots.Add(new PendingShot
                            {
                                Direction = dirToPlayer,
                                Speed = 120f,
                                Damage = 5f,
                                Size = 3f,
                                Lifetime = 2f,
                            });
                        }
                    }
                }
                break;
            
            case EnemyType.Wraith:
                // Phase 0: Invisible/fading in — teleport near player
                // Phase 1: Materialize — brief visible period
                // Phase 2: Slash attack
                // Phase 3: Fade out and reposition
                
                if (AiPhase == 0)
                {
                    // Invisible, waiting to materialize
                    if (AiStateTimer <= 0)
                    {
                        // Teleport near player (offset to side/behind)
                        float spawnAngle = MathF.PI * 2f * (Id * 71 + (int)(ShootTimer * 10) % 100) / 100f;
                        float spawnDist = 60f + (Id * 37 % 40);
                        Vector2 newPos = playerPos + new Vector2(MathF.Cos(spawnAngle), MathF.Sin(spawnAngle)) * spawnDist;
                        newPos.X = MathHelper.Clamp(newPos.X, arena.Left + 30, arena.Right - 30);
                        newPos.Y = MathHelper.Clamp(newPos.Y, arena.Top + 30, arena.Bottom - 30);
                        Position = newPos;
                        AiPhase = 1;
                        AiStateTimer = 0.5f; // materialize duration
                        ShootTimer += 1f;
                    }
                }
                else if (AiPhase == 1)
                {
                    // Materializing — becoming visible
                    FacingAngle = MathF.Atan2(dirToPlayer.Y, dirToPlayer.X);
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 2;
                        AiStateTimer = 0.15f; // slash duration
                        // Lunge toward player
                        KnockbackVel = dirToPlayer * 300f;
                    }
                }
                else if (AiPhase == 2)
                {
                    // Slashing — damage checked in Game1
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 3;
                        AiStateTimer = 0.6f; // visible before fading
                        KnockbackVel = Vector2.Zero;
                    }
                }
                else if (AiPhase == 3)
                {
                    // Fading out
                    if (AiStateTimer <= 0)
                    {
                        AiPhase = 0;
                        AiStateTimer = 1.5f + (Id % 3) * 0.5f; // 1.5-3s invisible
                    }
                }
                break;
        }
    }

    public bool TakeDamage(float amount, Vector2 sourceDir = default)
    {
        if (Spawning) return false; // invulnerable during spawn
        // Shield block: if hit from the front of shield
        if (ShieldUp && (Type == EnemyType.ShieldMaiden || Type == EnemyType.TowerGuard))
        {
            Vector2 shieldFacing = new Vector2(MathF.Cos(FacingAngle), MathF.Sin(FacingAngle));
            if (sourceDir.LengthSquared() > 0)
            {
                sourceDir.Normalize();
                float dot = Vector2.Dot(sourceDir, shieldFacing);
                if (dot < -0.3f) // hit from the front (source coming toward shield)
                {
                    amount *= 0.15f; // 85% damage reduction
                    HitFlash = 0.05f;
                    return false; // blocked, not killed
                }
            }
        }
        
        if (FearTimer > 0) amount *= 1.15f;
        Hp -= amount;
        HitFlash = 0.1f;
        SquashTimer = 0.1f;
        if (Hp <= 0)
        {
            if (Pacified) { Hp = 1f; return false; } // pacified enemies can't die
            Alive = false;
            return true;
        }
        return false;
    }

    public Rectangle HitBox => new(
        (int)(Position.X - Size / 2), (int)(Position.Y - Size / 2),
        (int)Size, (int)Size);

    public void Draw(SpriteBatch sb, Texture2D pixel, float totalTime = 0f)
    {
        if (!Alive) return;
        Color c = BaseColor;
        if (HitFlash > 0) c = Color.White;
        else if (IsBroken) c = ((int)(BreakTimer * 20) % 2 == 0) ? Color.White : BaseColor; // rapid flash when broken
        else if (StasisTimer > 0) c = Color.Lerp(BaseColor, new Color(200, 230, 255), 0.7f);
        else if (Possessed) c = Color.Lerp(BaseColor, new Color(255, 215, 0), 0.6f);
        else if (Pacified) c = Color.Lerp(BaseColor, new Color(240, 235, 220), 0.7f);
        else if (StaggerTimer > 0) c = ((int)(StaggerTimer * 20) % 2 == 0) ? Color.White : BaseColor;
        else if (IgniteTimer > 0) c = Color.Lerp(BaseColor, new Color(255, 140, 40), 0.5f);
        else if (SlowTimer > 0)
        {
            float slowPulse = (MathF.Sin(SlowTimer * 8f) + 1f) / 2f;
            c = Color.Lerp(BaseColor, new Color(100, 180, 60), 0.5f + slowPulse * 0.2f); // green vine tint
        }
        else if (FearTimer > 0) c = Color.Lerp(BaseColor, new Color(120, 40, 160), 0.5f);

        if (FusionTag != "Skeleton")
        {
            // Squash: shrink briefly on hit then spring back
            float scale = 1f;
            if (SquashTimer > 0) scale = 0.7f + 0.3f * (1f - SquashTimer / 0.1f);
            int drawSize = (int)(Size * scale);
            int drawOffset = (int)(Size - drawSize) / 2;
            int bx = (int)(Position.X - Size / 2) + drawOffset;
            int by = (int)(Position.Y - Size / 2) + drawOffset;
            
            // Pacified sway — gentle side-to-side bob
            if (Pacified)
            {
                float sway = MathF.Sin(totalTime * 1.5f + Id * 2.3f) * 2f;
                float bob = MathF.Sin(totalTime * 2f + Id * 1.7f) * 1.5f;
                bx += (int)sway;
                by += (int)bob;
            }
            // HedgeSniper: invisible when hidden
            if (Type == EnemyType.HedgeSniper && AiPhase == 0)
            {
                // Draw faint outline only
                Color hiddenC = c * 0.15f;
                sb.Draw(pixel, new Rectangle(bx, by, drawSize, drawSize), hiddenC);
            }
            else if (Type == EnemyType.Maw)
            {
                // Maw draws its own body — skip default rect
            }
            else
            {
                sb.Draw(pixel, new Rectangle(bx, by, drawSize, drawSize), c);
            }
            
            int cx = (int)Position.X;
            int cy = (int)Position.Y;
            
            // Type-specific overlays
            switch (Type)
            {
                case EnemyType.GuardGargoyle:
                    // Wings: two small triangular rects on sides
                    if (AiPhase == 1) // flapping during hop
                    {
                        float flap = MathF.Sin(AiStateTimer * 30f) * 3f;
                        sb.Draw(pixel, new Rectangle(bx - 4, by + 2 + (int)flap, 4, drawSize / 2), c * 0.7f);
                        sb.Draw(pixel, new Rectangle(bx + drawSize, by + 2 - (int)flap, 4, drawSize / 2), c * 0.7f);
                    }
                    else
                    {
                        sb.Draw(pixel, new Rectangle(bx - 3, by + 3, 3, drawSize / 2), c * 0.6f);
                        sb.Draw(pixel, new Rectangle(bx + drawSize, by + 3, 3, drawSize / 2), c * 0.6f);
                    }
                    // Horns
                    sb.Draw(pixel, new Rectangle(cx - 3, by - 3, 2, 3), c * 0.8f);
                    sb.Draw(pixel, new Rectangle(cx + 2, by - 3, 2, 3), c * 0.8f);
                    break;
                    
                case EnemyType.ShieldMaiden:
                case EnemyType.TowerGuard:
                    // Shield: rectangle extending in facing direction
                    if (ShieldUp)
                    {
                        Vector2 shieldDir = new Vector2(MathF.Cos(FacingAngle), MathF.Sin(FacingAngle));
                        Vector2 shieldPos = Position + shieldDir * (Size * 0.6f);
                        Vector2 shieldPerp = new Vector2(-shieldDir.Y, shieldDir.X);
                        float shieldW = Type == EnemyType.TowerGuard ? Size * 0.9f : Size * 0.7f;
                        Color shieldColor = HitFlash > 0 ? Color.White : 
                            Type == EnemyType.TowerGuard ? new Color(160, 160, 180) : new Color(180, 160, 120);
                        // Draw shield as thick line
                        Vector2 s1 = shieldPos + shieldPerp * shieldW / 2f;
                        Vector2 s2 = shieldPos - shieldPerp * shieldW / 2f;
                        float sRot = MathF.Atan2(s2.Y - s1.Y, s2.X - s1.X);
                        int sLen = (int)Vector2.Distance(s1, s2);
                        int sThick = Type == EnemyType.TowerGuard ? 4 : 3;
                        sb.Draw(pixel, new Rectangle((int)s1.X, (int)s1.Y, sLen, sThick),
                            null, shieldColor, sRot, new Vector2(0, sThick / 2f), SpriteEffects.None, 0);
                    }
                    break;
                    
                case EnemyType.HedgeSniper:
                    if (AiPhase > 0)
                    {
                        // "Popped up" — draw eye/scope dot
                        sb.Draw(pixel, new Rectangle(cx - 1, cy - 1, 3, 3), new Color(255, 60, 60));
                    }
                    break;
                    
                case EnemyType.HollowPlate:
                    // Visor slit — dark horizontal line
                    sb.Draw(pixel, new Rectangle(bx + 2, cy - 1, drawSize - 4, 2), new Color(20, 20, 40));
                    break;
                    
                case EnemyType.Webspinner:
                    if (AiPhase == 0)
                    {
                        // Hidden: draw faintly at top of body + fangs
                        Color hiddenSpider = new Color(60, 50, 70) * 0.15f;
                        sb.Draw(pixel, new Rectangle(bx, by, drawSize, drawSize), hiddenSpider);
                        sb.Draw(pixel, new Rectangle(cx - 1, by + drawSize + 1, 1, 2), new Color(200, 30, 30) * 0.2f);
                        sb.Draw(pixel, new Rectangle(cx + 1, by + drawSize + 1, 1, 2), new Color(200, 30, 30) * 0.2f);
                    }
                    else
                    {
                        // Active/Retreating: procedural legs driven by movement distance
                        // Step cycle: legs alternate in pairs based on cumulative distance
                        float stepLength = 8f; // pixels per full step cycle
                        float phase = DistanceTraveled / stepLength * MathF.PI * 2f;
                        Color legCol = new Color(60, 50, 70) * 0.8f;
                        float legLen = Size * 0.9f;
                        // 4 leg pairs — even pairs step together, odd pairs step together (alternating gait)
                        for (int lp = 0; lp < 4; lp++)
                        {
                            // Alternate: pairs 0,2 are in phase, pairs 1,3 are anti-phase
                            float pairPhase = phase + (lp % 2 == 0 ? 0 : MathF.PI);
                            float stepOffset = MathF.Sin(pairPhase + lp * 0.4f); // slight stagger within group
                            float spreadAngle = (lp - 1.5f) * 0.5f; // base splay angle
                            
                            // Left leg — extends left-ish, tip moves forward/back with step
                            float laBase = MathF.PI + spreadAngle;
                            float lTipX = cx + MathF.Cos(laBase) * legLen + stepOffset * 2f;
                            float lTipY = cy - 2 + lp * 3 + MathF.Sin(laBase) * legLen * 0.5f - MathF.Abs(stepOffset) * 1.5f; // lift during step
                            // Two-segment leg: body→knee→tip
                            float lKneeX = (cx + lTipX) * 0.5f + MathF.Abs(stepOffset) * 1f; // knee lifts outward during step
                            float lKneeY = (cy - 2 + lp * 3 + lTipY) * 0.5f - 1.5f; // knee always slightly above midpoint
                            sb.Draw(pixel, new Rectangle((int)MathF.Min(cx, lKneeX), (int)MathF.Min(cy - 2 + lp * 3, lKneeY), (int)MathF.Abs(lKneeX - cx) + 1, 1), legCol);
                            sb.Draw(pixel, new Rectangle((int)MathF.Min(lKneeX, lTipX), (int)MathF.Min(lKneeY, lTipY), (int)MathF.Abs(lTipX - lKneeX) + 1, 1), legCol);
                            
                            // Right leg — mirror
                            float raBase = -spreadAngle;
                            float rTipX = cx + MathF.Cos(raBase) * legLen - stepOffset * 2f;
                            float rTipY = cy - 2 + lp * 3 + MathF.Sin(raBase) * legLen * 0.5f - MathF.Abs(stepOffset) * 1.5f;
                            float rKneeX = (cx + rTipX) * 0.5f - MathF.Abs(stepOffset) * 1f;
                            float rKneeY = (cy - 2 + lp * 3 + rTipY) * 0.5f - 1.5f;
                            sb.Draw(pixel, new Rectangle((int)MathF.Min(cx, rKneeX), (int)MathF.Min(cy - 2 + lp * 3, rKneeY), (int)MathF.Abs(rKneeX - cx) + 1, 1), legCol);
                            sb.Draw(pixel, new Rectangle((int)MathF.Min(rKneeX, rTipX), (int)MathF.Min(rKneeY, rTipY), (int)MathF.Abs(rTipX - rKneeX) + 1, 1), legCol);
                        }
                        // Red fangs
                        sb.Draw(pixel, new Rectangle(cx - 1, by + drawSize, 1, 2), new Color(200, 30, 30));
                        sb.Draw(pixel, new Rectangle(cx + 1, by + drawSize, 1, 2), new Color(200, 30, 30));
                    }
                    break;
                
                case EnemyType.Thief:
                    // Small hooded figure — dark cloak, glinting eyes
                    Color cloakCol = new Color(50, 40, 35);
                    Color cloakEdge = new Color(70, 55, 45);
                    float flicker = 0.5f + 0.5f * MathF.Sin(AiStateTimer * 6f + Id);
                    
                    // Cloak body (triangle-ish)
                    sb.Draw(pixel, new Rectangle(cx - 5, cy - 4, 10, 12), cloakCol);
                    sb.Draw(pixel, new Rectangle(cx - 6, cy + 2, 12, 6), cloakEdge); // cloak flare
                    sb.Draw(pixel, new Rectangle(cx - 7, cy + 6, 14, 2), cloakCol * 0.6f); // hem
                    
                    // Hood
                    sb.Draw(pixel, new Rectangle(cx - 4, cy - 8, 8, 5), cloakCol);
                    sb.Draw(pixel, new Rectangle(cx - 3, cy - 9, 6, 2), cloakEdge);
                    
                    // Glinting eyes
                    Color eyeCol = new Color(255, 200, 50) * flicker;
                    sb.Draw(pixel, new Rectangle(cx - 2, cy - 6, 1, 1), eyeCol);
                    sb.Draw(pixel, new Rectangle(cx + 1, cy - 6, 1, 1), eyeCol);
                    
                    // Stolen loot sack on back (small circle)
                    sb.Draw(pixel, new Rectangle(cx + 4, cy - 2, 5, 5), new Color(140, 110, 70) * 0.8f);
                    sb.Draw(pixel, new Rectangle(cx + 5, cy - 1, 3, 3), new Color(160, 130, 80) * 0.6f);
                    
                    break;
                    
                case EnemyType.Maw:
                    // The Maw: living darkness with floating grin and eyes
                    // Override the default body rect — draw nothing solid, just darkness
                    
                    float mawTime = (float)System.Environment.TickCount * 0.001f;
                    float mawPulse = (MathF.Sin(mawTime * 2f + Id) + 1f) / 2f;
                    
                    // Dark haze body — overlapping translucent layers
                    for (int layer = 0; layer < 5; layer++)
                    {
                        float layerOffset = MathF.Sin(mawTime * (1f + layer * 0.3f) + layer * 1.2f) * 4f;
                        float layerOffsetY = MathF.Cos(mawTime * (0.8f + layer * 0.2f) + layer * 0.7f) * 3f;
                        int lSize = (int)(Size * (1.2f - layer * 0.08f));
                        Color hazeCol = layer < 2 
                            ? new Color(60, 10, 10) * (0.15f + mawPulse * 0.08f)
                            : new Color(30, 5, 20) * (0.12f + mawPulse * 0.05f);
                        sb.Draw(pixel, new Rectangle(
                            cx - lSize / 2 + (int)layerOffset, 
                            cy - lSize / 2 + (int)layerOffsetY, 
                            lSize, lSize), hazeCol);
                    }
                    
                    // Red glow core — ether bleeding out
                    float glowSize = Size * 0.6f + mawPulse * 8f;
                    sb.Draw(pixel, new Rectangle(
                        cx - (int)(glowSize / 2), cy - (int)(glowSize / 2),
                        (int)glowSize, (int)glowSize), new Color(120, 20, 20) * (0.12f + mawPulse * 0.06f));
                    
                    // Eyes — two small bright white dots
                    float eyeBob = MathF.Sin(mawTime * 1.5f) * 2f;
                    int eyeY = cy - 8 + (int)eyeBob;
                    float eyeGlow = 0.7f + 0.3f * MathF.Sin(mawTime * 3f);
                    Color mawEyeCol = Color.White * eyeGlow;
                    sb.Draw(pixel, new Rectangle(cx - 8, eyeY, 3, 3), mawEyeCol);
                    sb.Draw(pixel, new Rectangle(cx + 5, eyeY, 3, 3), mawEyeCol);
                    // Eye glow halo
                    sb.Draw(pixel, new Rectangle(cx - 10, eyeY - 2, 7, 7), mawEyeCol * 0.15f);
                    sb.Draw(pixel, new Rectangle(cx + 3, eyeY - 2, 7, 7), mawEyeCol * 0.15f);
                    
                    // Teeth — curved row of individual white rectangles
                    float jawOpen = 0f;
                    if (AiPhase == 1) // bite telegraph — jaw opening
                        jawOpen = 1f - (AiStateTimer / 0.6f); // 0→1 as timer counts down
                    else if (AiPhase == 2) // bite snap
                        jawOpen = MathF.Max(0f, 1f - (0.15f - AiStateTimer) / 0.05f); // snap shut quickly
                    else if (AiPhase == 3) // laugh
                        jawOpen = 0.3f + 0.2f * MathF.Sin(mawTime * 12f); // rapid open/close
                    
                    int teethCount = 9;
                    float teethSpan = Size * 0.7f;
                    int teethY = cy + 4 + (int)eyeBob;
                    float gapY = jawOpen * 12f; // jaw separation
                    
                    for (int tooth = 0; tooth < teethCount; tooth++)
                    {
                        float tx = cx - teethSpan / 2f + (teethSpan * tooth / (teethCount - 1));
                        int th = 3 + (tooth % 3 == 1 ? 2 : 0); // alternating heights
                        int tw = 2 + (tooth % 2);
                        Color toothCol = Color.White * (0.85f + 0.15f * MathF.Sin(mawTime * 2f + tooth));
                        // Upper teeth (pointing down)
                        sb.Draw(pixel, new Rectangle((int)tx, teethY - (int)(gapY / 2) - th, tw, th), toothCol);
                        // Lower teeth (pointing up)
                        sb.Draw(pixel, new Rectangle((int)tx, teethY + (int)(gapY / 2), tw, th), toothCol);
                    }
                    
                    // Grin curve — slight arc connecting teeth edges
                    if (jawOpen < 0.1f)
                    {
                        sb.Draw(pixel, new Rectangle(cx - (int)(teethSpan / 2) - 2, teethY - 1, (int)teethSpan + 4, 2), Color.White * 0.4f);
                    }
                    
                    // Purple ether wisps drifting off sides (drawn as small rects)
                    for (int w = 0; w < 4; w++)
                    {
                        float wAngle = mawTime * 0.8f + w * MathF.PI / 2f;
                        float wDist = Size * 0.5f + 10f + MathF.Sin(mawTime * 1.5f + w * 2f) * 8f;
                        int wx = cx + (int)(MathF.Cos(wAngle) * wDist);
                        int wy = cy + (int)(MathF.Sin(wAngle) * wDist * 0.6f);
                        float wAlpha = 0.2f + 0.15f * MathF.Sin(mawTime * 3f + w);
                        sb.Draw(pixel, new Rectangle(wx - 2, wy - 2, 4, 4), new Color(140, 80, 200) * wAlpha);
                    }
                    
                    break;
                
                case EnemyType.Charger:
                {
                    // Were-Jaguar: muscular beast, low to ground
                    float chargerTime = (float)System.Environment.TickCount * 0.001f;
                    bool telegraphing = AiPhase == 1;
                    bool charging = AiPhase == 2;
                    bool stunned = AiPhase == 3;
                    
                    // Body color — flashes red during telegraph
                    Color bodyCol = HitFlash > 0 ? Color.White : new Color(180, 120, 60);
                    if (telegraphing)
                    {
                        float flash = MathF.Sin(chargerTime * 20f) > 0 ? 1f : 0f;
                        bodyCol = Color.Lerp(bodyCol, new Color(255, 50, 50), flash * 0.7f);
                    }
                    if (stunned) bodyCol = Color.Lerp(bodyCol, new Color(100, 100, 120), 0.5f);
                    if (Possessed) bodyCol = Color.Lerp(bodyCol, new Color(255, 215, 0), 0.4f);
                    
                    // Low wide body
                    int bw = (int)(Size * 1.6f), bh = (int)(Size * 0.8f);
                    sb.Draw(pixel, new Rectangle(cx - bw / 2, cy - bh / 2 + 2, bw, bh), bodyCol);
                    // Head (forward of body)
                    float fx = MathF.Cos(FacingAngle) * Size * 0.6f;
                    float fy = MathF.Sin(FacingAngle) * Size * 0.6f;
                    int headS = (int)(Size * 0.7f);
                    sb.Draw(pixel, new Rectangle(cx + (int)fx - headS / 2, cy + (int)fy - headS / 2, headS, headS), bodyCol * 1.1f);
                    // Eyes — red glow
                    Color chEyeCol = charging ? new Color(255, 60, 0) : new Color(255, 160, 60);
                    sb.Draw(pixel, new Rectangle(cx + (int)fx - 3, cy + (int)fy - 2, 2, 2), chEyeCol);
                    sb.Draw(pixel, new Rectangle(cx + (int)fx + 1, cy + (int)fy - 2, 2, 2), chEyeCol);
                    // Charge trail — motion blur rects behind when charging
                    if (charging)
                    {
                        for (int t = 1; t <= 4; t++)
                        {
                            float trailAlpha = 0.15f - t * 0.03f;
                            int tx2 = cx - (int)(MathF.Cos(FacingAngle) * t * 12);
                            int ty2 = cy - (int)(MathF.Sin(FacingAngle) * t * 12);
                            sb.Draw(pixel, new Rectangle(tx2 - bw / 2, ty2 - bh / 2 + 2, bw, bh), bodyCol * trailAlpha);
                        }
                    }
                    // Stun stars when recovering
                    if (stunned)
                    {
                        for (int s = 0; s < 3; s++)
                        {
                            float sa = chargerTime * 3f + s * MathF.PI * 2f / 3f;
                            int starX = cx + (int)(MathF.Cos(sa) * 14f);
                            int starY = cy - (int)Size - 4 + (int)(MathF.Sin(sa * 2f) * 3f);
                            sb.Draw(pixel, new Rectangle(starX, starY, 2, 2), Color.Yellow * 0.6f);
                        }
                    }
                    break;
                }
                
                case EnemyType.Gremlin:
                {
                    // Tiny mischievous creature — oversized head, tiny body
                    float gremTime = (float)System.Environment.TickCount * 0.001f;
                    bool jumping = AiPhase == 1;
                    
                    Color gremCol = HitFlash > 0 ? Color.White : new Color(80, 200, 80);
                    if (Possessed) gremCol = Color.Lerp(gremCol, new Color(255, 215, 0), 0.4f);
                    
                    // Vertical hop offset when jumping
                    float hopOffset = 0f;
                    if (jumping && AiStateTimer > 0)
                    {
                        float prog = 1f - (AiStateTimer / 0.25f);
                        hopOffset = -MathF.Sin(prog * MathF.PI) * 16f; // arc up 16px
                    }
                    
                    int dy = cy + (int)hopOffset;
                    
                    // Small body
                    int gbw = (int)(Size * 0.8f), gbh = (int)(Size * 0.6f);
                    sb.Draw(pixel, new Rectangle(cx - gbw / 2, dy + 2, gbw, gbh), gremCol * 0.9f);
                    // Big head
                    int ghS = (int)(Size * 1.1f);
                    sb.Draw(pixel, new Rectangle(cx - ghS / 2, dy - ghS + 4, ghS, ghS), gremCol);
                    // Eyes — wide, slightly crazed
                    Color gremEyeCol = new Color(255, 255, 100);
                    sb.Draw(pixel, new Rectangle(cx - 4, dy - ghS + 6, 3, 3), gremEyeCol);
                    sb.Draw(pixel, new Rectangle(cx + 1, dy - ghS + 6, 3, 3), gremEyeCol);
                    // Pupils
                    sb.Draw(pixel, new Rectangle(cx - 3, dy - ghS + 7, 1, 1), Color.Black);
                    sb.Draw(pixel, new Rectangle(cx + 2, dy - ghS + 7, 1, 1), Color.Black);
                    // Wide grin
                    sb.Draw(pixel, new Rectangle(cx - 3, dy - ghS + 10, 7, 1), new Color(40, 40, 40));
                    // Shadow on ground when jumping
                    if (jumping && hopOffset < -2f)
                    {
                        float shadowAlpha = MathHelper.Clamp(-hopOffset / 16f, 0f, 0.4f);
                        int shadowW = (int)(Size * (1f + (-hopOffset / 32f)));
                        sb.Draw(pixel, new Rectangle(cx - shadowW / 2, cy + (int)Size / 2, shadowW, 2), Color.Black * shadowAlpha);
                    }
                    break;
                }
                
                case EnemyType.Wraith:
                {
                    // Hooded spectre — translucent, phasing in/out
                    float wrTime = (float)System.Environment.TickCount * 0.001f;
                    
                    // Alpha based on phase
                    float wrAlpha = 0.85f;
                    if (AiPhase == 0) wrAlpha = 0.05f; // nearly invisible
                    else if (AiPhase == 1) // materializing
                        wrAlpha = MathHelper.Lerp(0.1f, 0.85f, 1f - (AiStateTimer / 0.5f));
                    else if (AiPhase == 3) // fading out
                        wrAlpha = MathHelper.Lerp(0.85f, 0.05f, 1f - (AiStateTimer / 0.6f));
                    wrAlpha = MathHelper.Clamp(wrAlpha, 0.05f, 0.9f);
                    if (HitFlash > 0) wrAlpha = MathF.Max(wrAlpha, 0.7f);
                    
                    Color wrCol = new Color(120, 60, 180);
                    if (Possessed) wrCol = Color.Lerp(wrCol, new Color(255, 215, 0), 0.4f);
                    
                    // Hooded cloak — triangle-ish shape: wider at bottom
                    int hoodW = (int)(Size * 0.7f);
                    int hoodH = (int)(Size * 0.5f);
                    int cloakW = (int)(Size * 1.3f);
                    int cloakH = (int)(Size * 1.0f);
                    // Hood
                    sb.Draw(pixel, new Rectangle(cx - hoodW / 2, cy - (int)Size / 2, hoodW, hoodH), wrCol * (wrAlpha * 0.8f));
                    // Cloak body (wider)
                    sb.Draw(pixel, new Rectangle(cx - cloakW / 2, cy - (int)Size / 2 + hoodH - 2, cloakW, cloakH), wrCol * (wrAlpha * 0.6f));
                    // Wispy bottom edges
                    for (int w = 0; w < 3; w++)
                    {
                        float wOff = MathF.Sin(wrTime * 2f + w * 2f) * 4f;
                        int wX = cx - cloakW / 2 + (cloakW * w / 2);
                        int wY = cy - (int)Size / 2 + hoodH + cloakH - 2;
                        sb.Draw(pixel, new Rectangle(wX + (int)wOff, wY, 4, 6), wrCol * (wrAlpha * 0.3f));
                    }
                    // Eyes — glowing points under hood
                    if (wrAlpha > 0.3f)
                    {
                        Color wrEyeCol = new Color(200, 100, 255) * wrAlpha;
                        float eyePulse = 0.7f + 0.3f * MathF.Sin(wrTime * 4f);
                        sb.Draw(pixel, new Rectangle(cx - 4, cy - (int)Size / 2 + 4, 2, 2), wrEyeCol * eyePulse);
                        sb.Draw(pixel, new Rectangle(cx + 2, cy - (int)Size / 2 + 4, 2, 2), wrEyeCol * eyePulse);
                    }
                    // Slash arc during attack
                    if (AiPhase == 2)
                    {
                        float slashProg = 1f - (AiStateTimer / 0.15f);
                        float slashAngle = FacingAngle - 0.5f + slashProg * 1.0f;
                        for (int s = 0; s < 5; s++)
                        {
                            float sa2 = slashAngle + s * 0.12f;
                            float sd = Size * 0.8f + s * 3f;
                            int sx = cx + (int)(MathF.Cos(sa2) * sd);
                            int sy2 = cy + (int)(MathF.Sin(sa2) * sd);
                            sb.Draw(pixel, new Rectangle(sx, sy2, 3, 1), Color.White * (0.6f - s * 0.1f));
                        }
                    }
                    break;
                }
            }
        }

        // Skeleton minion: humanoid bone figure
        if (FusionTag == "Skeleton")
        {
            int cx = (int)Position.X;
            int cy = (int)Position.Y;
            Color bone = HitFlash > 0 ? Color.White : new Color(220, 220, 210);
            Color darkBone = HitFlash > 0 ? Color.White : new Color(180, 180, 170);
            if (Possessed) { bone = Color.Lerp(bone, new Color(255, 215, 0), 0.4f); darkBone = Color.Lerp(darkBone, new Color(255, 215, 0), 0.3f); }
            // Walk animation: sway based on time
            float walkCycle = MathF.Sin(Position.X * 0.1f + Position.Y * 0.1f + (float)System.Environment.TickCount * 0.006f);
            int legSway = (int)(walkCycle * 2f);
            int armSway = (int)(-walkCycle * 2f);
            // Skull (round-ish)
            sb.Draw(pixel, new Rectangle(cx - 2, cy - 8, 5, 5), bone);
            sb.Draw(pixel, new Rectangle(cx - 1, cy - 9, 3, 1), bone); // top of skull
            // Eyes (dark sockets)
            sb.Draw(pixel, new Rectangle(cx - 1, cy - 7, 1, 1), Color.Black);
            sb.Draw(pixel, new Rectangle(cx + 1, cy - 7, 1, 1), Color.Black);
            // Spine
            sb.Draw(pixel, new Rectangle(cx, cy - 3, 1, 6), darkBone);
            // Ribcage
            sb.Draw(pixel, new Rectangle(cx - 2, cy - 3, 5, 1), bone);
            sb.Draw(pixel, new Rectangle(cx - 2, cy - 1, 5, 1), bone);
            // Pelvis
            sb.Draw(pixel, new Rectangle(cx - 2, cy + 2, 5, 1), darkBone);
            // Left leg
            sb.Draw(pixel, new Rectangle(cx - 2 + legSway, cy + 3, 1, 4), bone);
            // Right leg
            sb.Draw(pixel, new Rectangle(cx + 2 - legSway, cy + 3, 1, 4), bone);
            // Left arm
            sb.Draw(pixel, new Rectangle(cx - 3 + armSway, cy - 2, 1, 4), bone);
            // Right arm
            sb.Draw(pixel, new Rectangle(cx + 3 - armSway, cy - 2, 1, 4), bone);
            // Override the rectangle draw — skip the default HitBox draw above
            // (we drew on top of it, which is fine at this scale)
        }

        if (Hp < MaxHp)
        {
            int barW = (int)Size;
            int barH = 2;
            int barX = (int)(Position.X - Size / 2);
            int barY = (int)(Position.Y - Size / 2) - 5;
            sb.Draw(pixel, new Rectangle(barX, barY, barW, barH), Color.DarkRed);
            sb.Draw(pixel, new Rectangle(barX, barY, (int)(barW * (Hp / MaxHp)), barH), Color.Red);
        }

        // Stagger bar (under HP bar)
        if (MaxStagger > 0 && (Stagger > 0 || IsBroken))
        {
            int barW = (int)Size;
            int barH = 1;
            int barX = (int)(Position.X - Size / 2);
            int barY = (int)(Position.Y - Size / 2) - 3;
            sb.Draw(pixel, new Rectangle(barX, barY, barW, barH), new Color(80, 60, 20));
            if (!IsBroken)
            {
                int fillW = (int)(barW * MathHelper.Clamp(Stagger / MaxStagger, 0f, 1f));
                sb.Draw(pixel, new Rectangle(barX, barY, fillW, barH), new Color(255, 180, 40));
            }
            else
            {
                // Flash empty bar when broken
                if ((int)(BreakTimer * 10) % 2 == 0)
                    sb.Draw(pixel, new Rectangle(barX, barY, barW, barH), new Color(255, 255, 100));
            }
        }

        // "BREAK!" text when broken — glow rectangle above enemy
        if (IsBroken)
        {
            Color breakCol = ((int)(BreakTimer * 10) % 2 == 0) ? new Color(255, 255, 80) : Color.White;
            int tx = (int)(Position.X - 8);
            int ty = (int)(Position.Y - Size / 2) - 14;
            // Glow pulse behind
            float pulse = 0.5f + 0.5f * MathF.Sin(BreakTimer * 20f);
            sb.Draw(pixel, new Rectangle(tx - 2, ty - 1, 20, 6), breakCol * (0.2f + 0.15f * pulse));
        }

        // Status pips
        int pipX = (int)(Position.X - Size / 2);
        int pipY = (int)(Position.Y - Size / 2) - 9;
        int pipOff = 0;
        if (IgniteTimer > 0) { sb.Draw(pixel, new Rectangle(pipX + pipOff, pipY, 3, 3), new Color(255, 140, 40)); pipOff += 4; }
        if (SlowTimer > 0) { sb.Draw(pixel, new Rectangle(pipX + pipOff, pipY, 3, 3), new Color(100, 180, 60)); pipOff += 4; }
        if (StaggerTimer > 0) { sb.Draw(pixel, new Rectangle(pipX + pipOff, pipY, 3, 3), new Color(255, 240, 60)); pipOff += 4; }
        if (FearTimer > 0) { sb.Draw(pixel, new Rectangle(pipX + pipOff, pipY, 3, 3), new Color(120, 40, 160)); pipOff += 4; }
        if (SilenceTimer > 0) {
            // Red X above enemy — silence indicator
            Color silCol = new Color(200, 40, 60) * MathHelper.Clamp(SilenceTimer, 0f, 1f);
            int sx = (int)Position.X, sy = (int)Position.Y - (int)Size / 2 - 14;
            sb.Draw(pixel, new Rectangle(sx - 4, sy - 4, 2, 2), silCol);
            sb.Draw(pixel, new Rectangle(sx + 3, sy - 4, 2, 2), silCol);
            sb.Draw(pixel, new Rectangle(sx - 2, sy - 2, 2, 2), silCol);
            sb.Draw(pixel, new Rectangle(sx + 1, sy - 2, 2, 2), silCol);
            sb.Draw(pixel, new Rectangle(sx - 1, sy, 2, 2), silCol);
            sb.Draw(pixel, new Rectangle(sx - 2, sy + 2, 2, 2), silCol);
            sb.Draw(pixel, new Rectangle(sx + 1, sy + 2, 2, 2), silCol);
            sb.Draw(pixel, new Rectangle(sx - 4, sy + 4, 2, 2), silCol);
            sb.Draw(pixel, new Rectangle(sx + 3, sy + 4, 2, 2), silCol);
            pipOff += 4;
        }
        if (LeechStack > 0) { sb.Draw(pixel, new Rectangle(pipX + pipOff, pipY, 3, 3), new Color(100, 220, 50)); pipOff += 4; }
        if (Possessed) { sb.Draw(pixel, new Rectangle(pipX + pipOff, pipY, 3, 3), new Color(255, 215, 0)); pipOff += 4; }
        if (Pacified) {
            // Soft milky halo around pacified enemy
            int haloR = (int)Size / 2 + 6;
            Color haloCol = new Color(240, 235, 220) * 0.15f;
            sb.Draw(pixel, new Rectangle((int)Position.X - haloR, (int)Position.Y - haloR, haloR * 2, haloR * 2), haloCol);
        }
        if (StasisTimer > 0) { sb.Draw(pixel, new Rectangle(pipX + pipOff, pipY, 3, 3), new Color(200, 230, 255)); }
    }
}
