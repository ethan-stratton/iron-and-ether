namespace IronAndEther;

/// <summary>How the projectile moves through space.</summary>
public enum Form
{
    None,    // No form = self-buff/aura
    Bolt,    // Straight line, fast, narrow
    Shotgun, // Tight cone of pellets
    Wave,    // Sine wave travel
    Orbit,   // Circles around player
    Chain,   // Jumps between nearby targets
    Seed,    // Placed stationary at player feet
    Saw,     // Spinning blade that returns
    Nova,    // Expanding shockwave ring
    Lance,   // Piercing beam line, brief hold
    Volley,  // Three-round burst fire
}

/// <summary>What happens on contact or over time.</summary>
public enum Behavior
{
    None,    // No behavior = single clean hit
    Shatter, // Splits into fragments on impact
    Linger,  // Creates persistent zone
    Bounce,  // Reflects off surfaces/enemies
    Bloom,   // Grows over time
    Possess, // Possesses enemy on kill
    Stasis,  // Freezes enemies in area
}

/// <summary>Elemental/thematic quality.</summary>
public enum Essence
{
    None,    // No essence = pure kinetic
    Cinder,  // Heat, combustion — Ignite DoT
    Dark,    // Shadow, bats, fear — Fear
    Bone,    // Rigidity — Crit vulnerability
    Silk,    // Entanglement — Slow
    Pulse,   // Disruption — Stagger/stun
    Hollow,  // Void — Silence enemy abilities
    Sap,     // Growth — Leech HP on hit
}

/// <summary>A weapon loadout: one slot per axis. Any can be None.</summary>
public struct Loadout
{
    public Form Form;
    public Behavior Behavior;
    public Essence Essence;

    public Loadout(Form form, Behavior behavior, Essence essence)
    {
        Form = form;
        Behavior = behavior;
        Essence = essence;
    }

    public static Loadout Empty => new(Form.None, Behavior.None, Essence.None);

    public int FilledSlots =>
        (Form != Form.None ? 1 : 0) +
        (Behavior != Behavior.None ? 1 : 0) +
        (Essence != Essence.None ? 1 : 0);

    public string Description => $"{FormName} + {BehaviorName} + {EssenceName}";

    public string FormName => Form == Form.None ? "---" : Form.ToString();
    public string BehaviorName => Behavior == Behavior.None ? "---" : Behavior.ToString();
    public string EssenceName => Essence == Essence.None ? "---" : Essence.ToString();

    /// <summary>Human-readable name for the current combo.</summary>
    public string ComboName
    {
        get
        {
            if (FilledSlots == 0) return "Empty Shell";

            string name = "";
            // Essence prefix
            if (Essence != Essence.None)
                name += Essence switch
                {
                    Essence.Cinder => "Fire ",
                    Essence.Dark => "Shadow ",
                    Essence.Bone => "Bone ",
                    Essence.Silk => "Silk ",
                    Essence.Pulse => "Shock ",
                    Essence.Hollow => "Void ",
                    Essence.Sap => "Vital ",
                    _ => ""
                };

            // Behavior modifier
            if (Behavior != Behavior.None)
                name += Behavior switch
                {
                    Behavior.Shatter => "Shattering ",
                    Behavior.Linger => "Lingering ",
                    Behavior.Bounce => "Ricochet ",
                    Behavior.Bloom => "Blooming ",
                    Behavior.Possess => "Possessing ",
                    Behavior.Stasis => "Freezing ",
                    _ => ""
                };

            // Form noun
            name += Form switch
            {
                Form.None => "Aura",
                Form.Bolt => "Bolt",
                Form.Shotgun => "Blast",
                Form.Wave => "Wave",
                Form.Orbit => "Ring",
                Form.Chain => "Arc",
                Form.Seed => "Mine",
                Form.Saw => "Saw",
                Form.Nova => "Nova",
                Form.Lance => "Lance",
                Form.Volley => "Volley",
                _ => "Shot"
            };

            return name.Trim();
        }
    }
}

/// <summary>Resolved projectile parameters from a loadout.</summary>
public struct ProjectileParams
{
    // Identity
    public Form FormType;
    public Behavior BehaviorType;
    
    // Movement
    public float Speed;
    public float Gravity;
    public float Lifetime;
    public float Size;
    public float OrbitRadius;
    public float BeamLength;
    public float OrbitSpeed;
    public int MaxChainTargets;
    public bool IsStationary;

    // Wave sine
    public float WaveAmplitude;
    public float WaveFrequency;

    // Behavior
    public bool Piercing;
    public int ShatterCount;
    public float LingerDuration;
    public float LingerRadius;
    public bool Bounces;
    public int MaxBounces;
    public float BloomGrowRate;
    public float BloomMaxScale;
    public bool Possesses;
    public float StasisRadius;
    public float StasisDuration;

    // Relic passives
    public bool PierceOne;
    public int PierceCount;

    // Essence
    public float BaseDamage;
    public float StatusChance;
    public float StatusDuration;
    public EssenceColor Color;
    public Essence EssenceType;

    // Returns
    public bool Returns;

    // Timing
    public float FireRate;
    public int BurstCount;

    public static ProjectileParams Resolve(Loadout loadout)
    {
        var p = new ProjectileParams();
        p.FormType = loadout.Form;
        p.BehaviorType = loadout.Behavior;

        // --- Base damage scales with filled slots ---
        p.BaseDamage = loadout.FilledSlots switch
        {
            0 => 4f,
            1 => 8f,
            2 => 14f,
            3 => 20f,
            _ => 4f
        };
        p.Lifetime = 2f;
        p.Size = 4f;
        p.Speed = 400f;
        p.FireRate = 0.2f;
        p.BurstCount = 1;

        // --- Form ---
        switch (loadout.Form)
        {
            case Form.None:
                p.Speed = 0;
                p.IsStationary = true;
                p.OrbitRadius = 30f;
                p.Lifetime = 3f;
                p.FireRate = 1.0f;
                break;
            case Form.Bolt:
                p.Speed = 700f;
                p.Size = 4f;
                p.FireRate = 0.75f; // Sniper: slow, precise, hits hard
                p.BaseDamage *= 1.4f; // Higher per-shot damage to compensate
                break;
            case Form.Shotgun:
                p.Speed = 450f;
                p.Size = 3f;
                p.Lifetime = 0.4f;
                p.BurstCount = 5;
                p.FireRate = 0.75f; // Pump action faster than bolt
                break;
            case Form.Wave:
                p.Speed = 300f;
                p.WaveAmplitude = 40f;
                p.WaveFrequency = 6f;
                p.Size = 6f;
                p.Lifetime = 3f;
                p.FireRate = 0.40f; // Fast — area denial, sweeping
                break;
            case Form.Orbit:
                p.Speed = 0f;
                p.OrbitRadius = 50f;
                p.OrbitSpeed = 5f;
                p.Size = 5f;
                p.Lifetime = 4f;
                p.FireRate = 0.70f;
                break;
            case Form.Chain:
                p.Speed = 500f;
                p.MaxChainTargets = 8;
                p.Size = 4f;
                p.FireRate = 0.70f; // Slower — each shot does a LOT of work
                break;
            case Form.Seed:
                p.Speed = 0f;
                p.IsStationary = true;
                p.Size = 6f;
                p.Lifetime = 8f;
                p.FireRate = 0.80f;
                break;
            case Form.Saw:
                p.Speed = 300f;
                p.Size = 8f;
                p.Lifetime = 1.5f;
                p.Piercing = true;
                p.FireRate = 0.80f;
                p.Returns = true;
                break;
            case Form.Nova:
                p.Speed = 150f;
                p.Size = 3f;
                p.Lifetime = 0.4f;       // expanding ring duration
                p.BurstCount = 20;       // 20 projectiles form the ring
                p.Piercing = true;       // ring passes through enemies
                p.FireRate = 1.10f; // slow — commit to being close
                p.BaseDamage *= 1.2f;    // reward for risk
                break;
            case Form.Lance:
                p.Speed = 0f;            // not a projectile — it's a beam
                p.Size = 6f;             // beam width (thick enough for layered glow)
                p.Lifetime = 0.3f;       // beam hold duration
                p.Piercing = true;
                p.FireRate = 0.90f;       // slower than bolt, high commitment
                p.BaseDamage *= 1.5f;    // highest per-hit of any form
                p.IsStationary = true;
                p.BeamLength = 250f;     // mid-range beam
                break;
            case Form.Volley:
                p.Speed = 500f;
                p.Size = 3f;
                p.BurstCount = 3;        // three-round burst
                p.FireRate = 0.85f;       // time between volleys
                break;
        }

        // --- Behavior ---
        switch (loadout.Behavior)
        {
            case Behavior.Shatter:
                p.ShatterCount = 5;
                break;
            case Behavior.Linger:
                p.LingerDuration = 2.5f;
                p.LingerRadius = 30f;
                break;
            case Behavior.Bounce:
                p.Bounces = true;
                p.MaxBounces = 4;
                break;
            case Behavior.Bloom:
                p.BloomGrowRate = 2f;
                p.BloomMaxScale = 3f;
                // No inherent pierce — bloom grows but pops on hit
                break;
            case Behavior.Possess:
                p.Possesses = true;
                break;
            case Behavior.Stasis:
                p.StasisRadius = 40f;
                p.StasisDuration = 1.5f;
                break;
        }

        // Behavior fire rate multiplier
        p.FireRate *= loadout.Behavior switch
        {
            Behavior.Shatter => 1.2f,
            Behavior.Bloom => 1.3f,
            _ => 1.0f,
        };

        // --- Essence ---
        p.EssenceType = loadout.Essence;
        p.StatusChance = 0.3f;
        p.StatusDuration = 2f;
        switch (loadout.Essence)
        {
            case Essence.None:
                p.Color = new EssenceColor(200, 200, 200);
                p.StatusChance = 0f;
                break;
            case Essence.Cinder:
                p.Color = new EssenceColor(255, 100, 30);
                p.BaseDamage *= 1.1f;
                break;
            case Essence.Dark:
                p.Color = new EssenceColor(120, 40, 160);
                p.BaseDamage *= 1.05f;
                break;
            case Essence.Bone:
                p.Color = new EssenceColor(220, 220, 210);
                p.BaseDamage *= 1.15f;
                break;
            case Essence.Silk:
                p.Color = new EssenceColor(180, 100, 220);
                break;
            case Essence.Pulse:
                p.Color = new EssenceColor(255, 240, 60);
                break;
            case Essence.Hollow:
                p.Color = new EssenceColor(60, 20, 80);
                p.BaseDamage *= 0.9f;
                break;
            case Essence.Sap:
                p.Color = new EssenceColor(100, 220, 50);
                break;
        }

        return p;
    }
}

public struct EssenceColor
{
    public byte R, G, B;
    public EssenceColor(byte r, byte g, byte b) { R = r; G = g; B = b; }
}
