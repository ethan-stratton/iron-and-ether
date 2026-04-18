# Iron & Ether: SHMUP Variant — Game Design Document (v1)

_The same combinatorial weapon system. Vertical scrolling shooter. Einhänder meets Gunbird meets Ikaruga._

---

## Elevator Pitch

A vertical-scrolling shmup where your ship's weapon is built from three combinable slots — **Form × Behavior × Essence** — producing 420 distinct weapon configurations. Steal components from destroyed enemies mid-run (Einhänder arm-steal). Each stage has a visual identity driven by a dominant Essence. Short, intense runs. Mastery through combination discovery.

---

## Why Shmup?

The original Iron & Ether was an arena brawler. The Form × Behavior × Essence system was designed for moment-to-moment combat variety. Shmups are *the* genre where weapon feel matters most — every shot pattern, every screen-filling super, every subtle behavioral difference is felt immediately because:

1. **You're always shooting.** In a top-down adventure, you fight intermittently. In a shmup, the weapon IS the game.
2. **343+ combos become 343+ shot types.** A shmup lives or dies on shot variety. We have it for free.
3. **Extraction from enemies** maps to Einhänder's weapon-stealing. Kill a mid-boss, grab its component, slot it in mid-flight.
4. **Spell Fusions become screen-clearing supers.** The named combos already feel like bomb attacks.
5. **Scope is smaller.** No overworld. No NPC dialog. 6-8 stages, each 4-5 minutes. Shippable.

---

## Core Loop

```
Fly → Shoot → Kill enemies → Extract components → Slot them → Shoot differently → Repeat
```

### Per-Stage Flow
1. **Stage start** — you have whatever loadout you brought (or defaults)
2. **Scrolling combat** — waves of enemies, environmental hazards, mini-bosses
3. **Extraction moments** — certain enemies drop Form/Behavior/Essence orbs on death
4. **Mid-boss** — always drops a guaranteed component (Einhänder style: physically grab it off their wreckage)
5. **Stage boss** — pattern-heavy fight, tests your current loadout
6. **Between stages** — brief loadout screen, swap/lock components, see what Spell Fusions are available

### The Extraction

This is the hook. Enemies don't just explode — certain ones (marked with a subtle glow matching their component type) release a floating orb when destroyed:

- 🔴 **Red orb** = Form component
- 🔵 **Blue orb** = Behavior component  
- 🟡 **Yellow orb** = Essence component

Fly into the orb to collect it. It goes into your **reserve** (3 slots, one per type). Press a button to **hot-swap** any slot mid-flight. Your weapon changes instantly.

**The tension:** Do you grab the Orbit form even though your current Bolt+Bounce+Pulse is shredding? The new component might unlock a Spell Fusion. Or it might be worse for this boss. Risk/reward every time.

---

## The Weapon System (Adapted for Shmup)

### Forms (Shot Pattern)

| Form | Shmup Behavior | Closest Classic Equivalent |
|------|----------------|---------------------------|
| **Bolt** | Fast straight shots, narrow stream | Gradius Laser |
| **Spread** | 5-way fan, wide coverage | Contra Spread Gun |
| **Wave** | Sine-wave projectiles, sweeping coverage | R-Type Wave Cannon (charged) |
| **Chain** | Shots arc to nearest enemy, auto-aim | Thunder Force Lightning |
| **Orbit** | Rotating options/satellites that damage on contact | Gradius Options (ring mode) |
| **Seed** | Drop mines behind you, explode on enemy proximity | Zanac ground weapon |
| **Saw** | Boomerang shots that return, double-hit | Axelay boomerang |
| **Nova** | Radial burst, point-blank omnidirectional | Bomb-lite, short range |
| **Lance** | Charged beam, brief hold, massive damage line | R-Type Wave Cannon |
| **Volley** | 3-round burst with slight spread | DoDonPachi laser |

### Behaviors (On-Hit Effect)

| Behavior | Shmup Effect |
|----------|-------------|
| **Shatter** | Projectile splits into fragments on enemy hit — clears nearby bullets too |
| **Linger** | Creates a damage zone on impact (area denial, amazing for chokepoints) |
| **Bounce** | Shots ricochet off screen edges and enemies |
| **Bloom** | Projectiles grow as they travel — tiny near you, massive at screen top |
| **Possess** | Killed enemy briefly becomes a drone fighting for you |
| **Stasis** | On hit, freezes enemy + nearby **bullets** in a time bubble (defensive!) |

### Essences (Element + Visual Identity)

| Essence | Visual | Gameplay Effect |
|---------|--------|----------------|
| **Cinder** | Fire trails, explosions, screen shake | Damage over time, explosion radius on kill |
| **Bone** | Skeletal projectiles, spear constructs | Extra hits crit. Killed enemies leave bone shards (passive damage) |
| **Silk** | Thread connections, web patches | Slow enemies. Connected enemies share damage |
| **Pulse** | Lightning arcs, screen flash | Stagger (interrupt enemy attack patterns) |
| **Hollow** | Black holes, visual distortion | Gravity — pulls bullets and enemies toward impact point |
| **Sap** | Vines, flowers, green particles | Heal on hit (tiny). Killed enemies sprout healing flowers |
| **Dark** | Bats, smoke, shadow | Fear — enemies scatter briefly, break formation |

---

## Spell Fusions (Supers)

When all three slots form a named combo, your weapon transforms entirely AND you gain a **Fusion Bomb** — a screen-clearing super unique to that combination.

Examples (adapted from original GDD):

| Fusion | Form+Behavior+Essence | Super Effect |
|--------|----------------------|-------------|
| **Hellfire** | Spread + Shatter + Cinder | Screen fills with fragmenting fireballs. Everything burns. |
| **Websnare** | Chain + Linger + Silk | Lightning web covers the screen. All enemies frozen 3s. All bullets eaten. |
| **Bone Garden** | Seed + Linger + Bone | Mine field that sprouts bone spires. Area denial for 8s. |
| **Void Maw** | Nova + Stasis + Hollow | Black hole at ship position. Sucks in all bullets and enemies for 4s. |
| **Phantom Flock** | Orbit + Possess + Dark | 6 ghost ships orbit you for 10s, each firing your base weapon. |
| **Overgrowth** | Wave + Bloom + Sap | Giant vine wave fills screen top to bottom. Full heal. |
| **Mjolnir** | Lance + Bounce + Pulse | Lightning beam bounces between all enemies on screen. Each bounce = screen shake. |
| **Necro Swarm** | Volley + Possess + Bone | Every enemy killed for 5s becomes a skeleton drone. Drone swarm. |

Players can discover all 420 combos. ~30 are Spell Fusions. The rest are "unnamed" but still combine their components meaningfully.

---

## Stage Design

6 stages. Each themed around a dominant Essence, which determines the enemy types, hazards, and aesthetic.

| Stage | Theme | Essence | Setting | Boss |
|-------|-------|---------|---------|------|
| 1 | **The Furnace** | Cinder | Industrial hellscape, molten rivers, falling debris | Iron Golem — lava vomit patterns |
| 2 | **The Ossuary** | Bone | Cathedral of bones, rib-cage tunnels, skull turrets | Bone Hydra — multi-head tracking shots |
| 3 | **The Tangle** | Silk | Overgrown forest canopy, web bridges, cocooned ships | Weaver Queen — web maze + bullet curtains |
| 4 | **The Storm** | Pulse | Thundercloud fortress, lightning pillars, static fields | Storm Engine — arena-wide telegraph attacks |
| 5 | **The Hollow** | Hollow | Void rift, gravity distortion, reality tears | Singularity — screen warping, bullet absorption |
| 6 | **The Throne** | Dark | Shadow castle, all essences combined | Final boss uses ALL essences in phases |

### Stage Structure (per stage, ~4-5 min)
```
[Scroll section 1] → [Mini-boss] → [Scroll section 2] → [Mid-boss + component drop] → [Scroll section 3] → [Boss]
```

---

## Difficulty & Scoring

### Difficulty Curve
- **Stage 1**: Slow enemies, wide gaps, teaches extraction
- **Stage 2-3**: Faster patterns, environmental hazards introduced
- **Stage 4-5**: Bullet density increases, extraction becomes risky (orbs appear in dangerous positions)
- **Stage 6**: No extraction — you fight with what you have. Tests mastery of your build.

### Scoring
- **Kill combo** — chain kills within 1s for multiplier
- **Extraction combo** — grab 3 orbs in 3 seconds = score burst
- **Spell Fusion discovery** — first time finding a named combo = huge bonus
- **No-hit bonus** — per section
- **Boss time** — faster kill = more points

### Lives & Continues
- 3 hits per run (not lives — hits). HP bar, not instant death.
- **Sap essence heals.** This is the only healing in the game. Creates a real tension: Sap is defensive but sacrifices damage essence.
- No continues. Run-based. High score board.

---

## Controls

```
Move:       WASD or Arrow Keys (8-directional)
Shoot:      Hold SPACE or Z (auto-fire)  
Focus:      Hold SHIFT — slow movement, tighter hitbox (Cave-style)
Swap Form:  1 key (cycles reserve)
Swap Behav: 2 key
Swap Ess:   3 key
Fusion Bomb: X key (if Spell Fusion active, consumes it)
```

Hitbox is tiny (2×2 pixels at game scale). Ship sprite is larger but only the core takes damage.

---

## Visual Style

- **320×240 at 3× scale** (same as Title of Liberty / Rat Park)
- **Single-file HTML/Canvas** — same stack
- **Pixel art with particle excess** — every shot, every explosion, every extraction should feel JUICY
- Essences dramatically change the visual tone of your shots (not just tint — Cinder has trails and sparks, Hollow warps space, Dark spawns bats)
- Screen shake on big hits. Flash on Spell Fusion activation.
- **Backgrounds scroll parallax** — 2-3 layers per stage

---

## Scope & MVP

### Prototype (2 weeks)
- Ship movement + shooting
- 3 Forms (Bolt, Spread, Wave)
- 2 Behaviors (Shatter, Bounce)  
- 2 Essences (Cinder, Pulse)
- Component extraction from enemies
- Hot-swap mid-flight
- 1 stage with mini-boss and boss
- 1 Spell Fusion (Mjolnir: Lance+Bounce+Pulse)

### Full Game
- All 10 Forms, 6 Behaviors, 7 Essences
- 6 stages
- ~30 Spell Fusions
- Scoring + high score board
- Unlockable gallery (Fusion discovery log)

---

## Key Inspirations

| Game | What We Take |
|------|-------------|
| **Einhänder** | Weapon stealing from enemies mid-flight. The physicality of grabbing components. |
| **Gunbird** | Character variety → our combo variety. Short intense stages. Personality. |
| **Ikaruga** | Polarity system → our Essence swapping. Strategic color/type matching. |
| **Gradius** | Options → our Orbit form. Power-up bar → our 3-slot system. |
| **Cave (DoDonPachi etc.)** | Bullet density, tiny hitbox, scoring depth, focus mode. |
| **Geometry Wars** | Arena feel, visual excess, the joy of screen-filling destruction. |
| **Aria of Sorrow** | Kill-to-acquire. The excitement of "what will this enemy give me?" |

---

## Open Questions

- **Co-op?** Gunbird is 2-player. Two ships with independent loadouts could be incredible. But doubles scope.
- **Roguelike elements?** Random component drops per run = different build each time. Could add huge replayability but harder to balance.
- **Ship types?** Multiple ships with different base stats (speed vs power vs hitbox size). Or just one ship, all variety from components.
- **Vertical vs horizontal?** This doc assumes vertical (Gunbird/DoDonPachi). Horizontal (Einhänder/R-Type) changes level design significantly. Vertical is more natural for our 320×240 aspect ratio.

---

_The weapon system was always the soul of Iron & Ether. This format lets it breathe._
