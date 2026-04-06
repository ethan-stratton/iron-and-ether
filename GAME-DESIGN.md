# Iron & Ether — Game Design Document (v3)

_A magical knight with a gun (we call it a "wand" in-game). Zelda 1 structure meets Aria of Sorrow acquisition meets HoD Spell Fusion spectacle._

_For full narrative, characters, and lore: see **STORY-BIBLE.md**_

---

## Elevator Pitch

Top-down adventure game in alternate-reality Camelot. You're a knight with a magic wand that evolves based on enemy essences you collect. Three combinable slots (Form, Behavior, Essence) produce hundreds of weapon variations, and specific 3-slot combos trigger **Spell Fusions** — curated, named, spectacular attacks with unique visuals. Explore a Zelda 1-style overworld, find relics, grow in power, and visually transform as your arsenal expands.

## Art Style

- **Zelda 1 screen-by-screen rooms** — single-screen areas, walk to edge to transition
- **Pixel art with modern shading** — not flat NES colors, subtle lighting and shade
- **Visual spectacle on abilities** — the world is simple, the magic is flashy
- **Player visually transforms** with power level (Altered Beast progression)

---

## The Wand (Primary Combat System)

Three slots. Fill them with components from enemy drops.

### Forms (How the wand fires)

| Form | Description | Feel |
|------|-------------|------|
| **Bolt** | Fast straight shot | Reliable, precise |
| **Shotgun** | Tight cone of 5 pellets toward cursor | Close-medium range burst |
| **Wave** | Sine-wave travel path (Medusa Head style) | Sweeping, covers odd angles, crowd control |
| **Chain** | Jumps between nearby enemies, long range, visible arcs | Lazy aim, crowd punish |
| **Orbit** | Projectile circles around you | Defensive, walk-into-enemies |
| **Seed** | Dropped at your feet as landmines, triggers on enemy proximity | Traps, area denial, kiting tool |
| **Saw** | Boomerang buzzsaw, travels out and returns | Skill shot, double-hit potential |
| **Nova** | Point-blank omnidirectional burst (12 projectiles) | Walk-into-enemies melee, huge risk/reward |
| **Lance** | Piercing beam line, 250f mid-range, brief 0.3s hold | Highest per-shot damage, high commitment |
| **Volley** | Three-round burst, tight spread, mid-range | Burst-fire rhythm, DPS between Bolt and Wave |

### Behaviors (What happens on contact)

| Behavior | Description |
|----------|-------------|
| **Shatter** | Splits into fragments on impact |
| **Linger** | Creates persistent damage zone |
| **Bounce** | Reflects off walls/enemies |
| **Bloom** | Projectile grows over time, increasing hitbox and damage |
| **Possess** | On kill, enemy briefly fights for you (Aria of Sorrow Puppet Master) |
| **Stasis** | On hit, freezes enemy + nearby projectiles in a time bubble |

**Promoted to Relics (v3):**
- **Pierce** → Relic. Too universally good for a slot. Now a passive: "all projectiles pierce 1 enemy"
- **Siphon** → Relic/Potions. Life steal is a passive effect, not a behavior

**Cut (v3):** Bind (unfun), Pulse-as-behavior (redundant with Pulse essence)

### Essences (Magical nature — VISUAL TRANSFORMATION is key)

Each essence dramatically changes how the weapon looks and feels. Not just a tint.

| Essence | Theme | Visual Identity | Status Effect |
|---------|-------|----------------|---------------|
| **Cinder** | Fire, combustion | EXPLOSIONS on every hit. Fireballs. Ember trails | Ignite (DoT) |
| **Bone** | Necromancy, structure | Bone constructs. Spears stick in enemies. Skeleton summons | Fracture (crit vulnerability) |
| **Silk** | Web, entanglement | Visible threads connecting enemies. Web patches | Snare (slow) |
| **Pulse** | Lightning, force | Electrical arcs. Screen flash. Visible lightning chains | Stagger (stun) |
| **Hollow** | Void, gravity | Black holes. Visual distortion. Things get sucked in | Silence (disable abilities) |
| **Sap** | Nature, overgrowth | Vines, thorns, flowers sprout. All hits heal slightly | Leech (heal on hit, stacking) |
| **Dark** | Shadow, corruption | Bats, smoke, spectral shapes. Enemies flee in terror | Fear (enemies run briefly) |

**Cut/Reworked (v3):**
- **Brine** → replaced by **Dark**. Brine was least-used, acid didn't feel magical enough. Dark opens up horror/gothic territory and pairs perfectly with Camelot mythology (Morgan le Fay, the Questing Beast, the shadow side of Arthurian legend)
- **Sap** reworked — now owns the healing niche since Siphon moved to relics. Sap essence = all hits heal slightly

---

## Spell Fusions (Named Curated Combos)

When you equip a specific Form + Behavior + Essence, it triggers a **Spell Fusion** — a named attack with unique visuals and behavior beyond what the components produce individually.

~20-30 total across the grid. Players discover them by experimenting.

### Tier 3 "Super" Fusions (the endgame chase)

These are the marquee combos. Named after mythology and religion. Players will build toward these.

| Form | Behavior | Essence | Fusion Name | Effect |
|------|----------|---------|-------------|--------|
| Bolt | Shatter | Bone | **Dragon's Teeth** | Bone bolt shatters into skeleton soldiers that fight briefly |
| Seed | Bloom | Sap | **Yggdrasil's Root** | Seed grows into a massive tree that attacks with vines and heals you |
| Chain | Bounce | Pulse | **Indra's Arrow** | Lightning bounces off every surface AND chains between all enemies |
| Orbit | Stasis | Hollow | **Event Horizon** | Black hole orbits you, freezing and crushing everything it touches |
| Shotgun | Linger | Cinder | **Dragonbreath** | Cone of fire, leaves burning ground everywhere |
| Wave | Linger | Hollow | **Rift Tide** | Sine wave leaves void tears that suck enemies in |
| Saw | Bounce | Bone | **Death's Scythe** | Spinning bone blade bounces infinitely off walls and enemies |
| Bolt | Stasis | Pulse | **Gae Bolg** | Lightning bolt freezes time on impact, then detonates |
| Shotgun | Possess | Dark | **Gebel's Swarm** | Shotgun fires swarms of bats that possess enemies on kill |
| Orbit | Bloom | Cinder | **Surtr's Crown** | Fire ring orbits you, growing until it fills the screen |
| Seed | Shatter | Bone | **Grave Call** | Bone mine erupts into a wave of skeleton warriors |
| Wave | Bloom | Sap | **Tiamat's Garden** | Sine wave grows into a wall of thorns and vines |
| Chain | Stasis | Hollow | **Chronos' Chain** | Chains between enemies freezing each one in time |
| Bolt | Linger | Cinder | **Pillars of Muspelheim** | Fire bolt leaves pillars of flame along its path |
| Saw | Shatter | Pulse | **Vajra** | Lightning buzzsaw shatters into arcing bolts on return |
| Orbit | Possess | Dark | **Morrigan's Court** | Dark spirits orbit you, possessing any enemy they touch |

### Castlevania-Inspired Fusions (direct rips, recontextualized)

| Inspiration | Fusion | Effect |
|-------------|--------|--------|
| HoD Ice Book + Cross | Orbit + Shatter + Hollow | **Cocytus** — Ice shards orbit, shatter outward on command |
| HoD Wind Book + Axe | Saw + Bloom + Hollow | **Boreas' Gale** — Buzzsaw becomes tornado, drags enemies in |
| HoD Summon Book | Seed + Possess + Bone | **Solomon's Seal** — Landmine summons skeleton champion on trigger |
| HoD Bolt Book + Holy Water | Seed + Linger + Pulse | **Mjölnir's Mark** — Lightning mine leaves persistent shock field |
| AoS Valkyrie | Orbit + Stasis + Bone | **Brynhildr** — Spinning bone swords orbit you, freezing on contact |
| Bullet Bill transform | Bolt + Bloom + Dark | **Fomorian's Charge** — YOU become the projectile, rocketing forward as a shadow bullet |

---

## Familiar (Placeholder System)

_TODO: Design fully. Core concept established._

- Acquired through adventure (quest/boss/hidden area)
- Fights alongside you independently
- Appearance changes based on current **Essence**:
  - Cinder → fire sprite
  - Bone → skeleton knight
  - Sap → vine creature
  - Pulse → spark elemental
  - Hollow → void wisp
  - Silk → spider companion
  - Dark → shadow familiar (bat? raven?)
- Potentially 3-5 familiar base types with different behaviors
- Familiar has its own progression? (Curse of Darkness style)
- **Open:** How do familiars interact with Spell Fusions? Do they enhance them?

---

## Charge Mechanic

**Acquired as a Relic** (not default). Hold fire to charge your wand shot.

- **Charged shots deal 3× damage**
- Some Form + Essence combos gain special charged effects:
  - Charged Bolt + Pulse = full-screen lightning strike
  - Charged Shotgun + Cinder = massive explosion cone
  - Charged Wave + Hollow = screen-wide void ripple
- Charge time ~1.5 seconds. Visual buildup on wand (glow, particles, screen shake)
- Risk/reward: standing still to charge in a combat arena

---

## Movement & Defense

| Ability | How Acquired | Description |
|---------|-------------|-------------|
| **Walk** | Default | WASD 4-directional |
| **Shield Bash** | Buy/find shield | Short forward charge, blocks projectiles + damages enemies |
| **Horse** | Story/quest unlock | Fast overworld traversal, different combat feel while mounted |

No dodge roll. No teleport. Knight-appropriate movement.

**Why play this game?** The fantasy of being a knight whose wand gets increasingly insane. The joy of discovery (what does THIS combo do?). The visual spectacle of Spell Fusions. The Zelda-style exploration with Aria-style acquisition. Movement stays simple — the complexity is in your wand, not your legs.

---

## Relics (Visible Passive Items)

Found in the world (Zelda-style — chests, shops, quest rewards). Permanently equipped. Visible on a relic bar.

| Relic | Effect |
|-------|--------|
| **Grail Fragment** | Passive HP regen |
| **Round Table Seal** | Damage resistance |
| **Morgan's Mirror** | Chance to reflect projectiles |
| **Pendragon's Crown** | Increased essence drop rate |
| **Giant's Tooth** | All projectiles larger |
| **Merlin's Lens** | Reveals hidden paths/secrets on map |
| **Lancelot's Vow** | Damage boost at full HP |
| **Mordred's Spite** | Damage boost at low HP |
| **Percival's Horn** | Horse moves faster, charge does damage |
| **Caliburn Shard** | All projectiles pierce 1 enemy _(was Pierce behavior)_ |
| **Nimue's Tear** | All hits restore small HP _(was Siphon behavior)_ |
| **Wand of Merlin** | Unlocks Charge mechanic (hold to power up shots) |

### Ring Slots (2 equipped)

Small passives found/bought throughout the world.

| Ring | Effect |
|------|--------|
| **Ring of Haste** | Faster fire rate |
| **Ring of Reach** | Increased projectile range |
| **Ring of Fortune** | Better drop rates |
| **Ring of the Ox** | Knockback on hit |
| **Ring of the Owl** | Wider projectile spread |
| **Ring of the Cat** | Move faster |

### Potions (Buyable from Shops)

Simple consumables. Stack in inventory. Use from hotkey.

| Potion | Effect |
|--------|--------|
| **Health Potion** | Restore HP |
| **Wand Oil** | Temporary fire rate + damage boost |
| **Ironbark Tonic** | Temporary damage resistance |
| **Ether Draught** | Temporary Spell Fusion damage boost |
| **Antidote** | Cure status effects |

---

## Progression & World Structure

### Zelda 1 Model
- **Overworld:** Grid of single-screen rooms. Walk to edge → screen transitions
- **Dungeons:** Themed areas with keys, locked doors, boss at end
- **Shops/NPCs:** Buy shield, potions, rings, get hints about Spell Fusions, lore
- **Secrets:** Hidden rooms, wand-blast-able walls

### Essence Acquisition (Aria of Sorrow model)
- Enemy drops are **rare, not guaranteed**
- Each enemy type has a specific component it can drop
- First time getting a new component = celebration moment (slow-mo, name flash, visual demo)
- Finding new components drives exploration

### Power Curve (Altered Beast model)
Player visually transforms as they fill wand slots:
- **0-1 slots:** Basic knight, simple wand, muted
- **2 slots:** Wand glows, subtle aura
- **3 slots:** Full visual transformation based on essence
- **Spell Fusion active:** Maximum spectacle, unique appearance

---

## Opening Sequence (The Hook)

Blend of Zelda 1 + A Link to the Past + Castle Crashers:

1. **Dream sequence** — Morgan le Fay appears, gives exposition. "Arthur has fallen. Percival seeks the Grail. You must carry the Wand." Brief, atmospheric, sets stakes.
2. **Wake up in chaos** — Castle Crashers energy. Knights fighting and dying around you. Battlefield. You're unarmed.
3. **The Cave** — Zelda 1 moment. Amid the chaos, a cave/tent to enter. Inside: "It's dangerous to go alone. Take this." Pick up the Wand.
4. **Thrown into combat immediately** — first enemies attack as you exit. No tutorial text. Learn by doing.

### Design Philosophy: "Hard Fun"

_"Work is more fun than fun."_ — Nolan Bushnell

Players should feel **pride** in their mastery. Not difficulty for its own sake, but:
- Combo discovery requires experimentation, not hand-holding
- Spell Fusions aren't listed in a menu — you find them by trying
- Enemies are designed to punish one strategy and reward switching
- The game respects the player's intelligence

Easy fun = given everything. Hard fun = earned everything. This game is earned.

### Design Philosophy: "Autotelic Reward"

_Inspired by Jane McGonigal's Reality is Broken and the research on intrinsic vs extrinsic motivation._

The goal isn't maximum engagement time. The goal is a game that makes the player **happier even when they're not playing.** The reward isn't loot, stats, or numbers going up — it's mastery, understanding, and creative discovery.

**Core principles:**

1. **Reward curiosity over optimization.** The combo system (343 possibilities) exists to be explored, not min-maxed. Players who experiment discover more than players who follow guides.

2. **Reward observation over brute force.** Enemies telegraph their weaknesses through visual behavior, not UI labels. The Ether Sight relic *confirms* what an attentive player already noticed. Information is power — earned through paying attention.

3. **Reward empathy as a valid path.** Hollow essence reveals enemy death thoughts. This is not a power upgrade — it's an emotional one. Players who choose understanding over efficiency discover a hidden narrative layer. The empathetic path is valid, hard-won, and leads somewhere different.

4. **Make knowledge the upgrade, not stats.** Armors should change *how you perceive the world* (see telegraphs earlier, sense hidden paths), not just +10% damage. The player gets better because *they* get better.

5. **No hedonic treadmill.** Avoid escalating number inflation. A hard-won BREAK finisher at hour 1 should feel as good as one at hour 20. Fiero from genuine challenge doesn't decay — it compounds.

6. **The hollowness test.** If the player puts the game down and feels empty, we failed. If they put it down thinking about the Webspinner's children, or proud of a combo they invented, or better at reading patterns — we succeeded.

### Hollow Essence — The Empathy Path

Hollow essence is **rare and hard to acquire** in campaign mode. It is the last essence the player encounters, gated behind significant progression.

- Hollow reveals the inner thoughts of dying enemies (death barks)
- Without Hollow, enemies die silently — the player never knows they had thoughts
- With Hollow, every kill carries weight: the Webspinner was building a home, the Gargoyle dreamed of flying
- Players who rush through with pure damage builds **never see these lines** and don't know they exist
- This creates organic word-of-mouth: "did you know the enemies talk when they die?"
- Hollow's narrative reward is **understanding**, not power — it doesn't make killing easier, it makes it *mean something*

### Skillful Kill Animations

Difficult or well-timed kills get **unique death visuals** — not bigger numbers, but the game *acknowledging* that what you did was hard.

**Context-specific deaths:**

| Kill Condition | Animation | Why It Feels Good |
|---|---|---|
| Wraith killed during 0.5s materialize window | Freezes mid-phase, cracks like glass, shatters into light (not shadow) | You caught something between worlds |
| Charger BREAK'd mid-lunge | Momentum carries body forward, crumbles, leaves skid trail of debris | Physics sells the impact of stopping unstoppable force |
| Gremlin killed mid-hop | Ragdoll arc, comically long hang time, bounces once | Tonal contrast — sad and funny |
| Any enemy BREAK'd with weakness element | Element *consumes* them visually (see below) | Correct read = mastery, not just a multiplier |

**Weakness-element death bursts (during BREAK finisher):**

| Weakness Element | Death Visual |
|---|---|
| Cinder | Ash disintegration — enemy burns from extremities inward, crumbles to floating ash |
| Pulse | Spark burst — enemy shatters into crackling electric fragments, arcs between pieces |
| Bone | Fracture cascade — enemy cracks like stone, pieces fall and shatter on ground |
| Silk | Cocoon collapse — threads wrap inward, cocoon forms, then crumbles to dust |
| Dark | Shadow dissolution — enemy's shadow detaches and pulls them into the floor |
| Hollow | Light reveal — enemy becomes translucent, inner light visible, gently fades |
| Sap | Overgrowth — roots/vines erupt from enemy, flowers bloom briefly, then wilt |

**Design rule:** These only trigger on BREAK finishers with the matching weakness element. Regular kills with weakness still get the standard death. The finisher version is the *ultimate* read: right element, right timing, decisive execution.

---

## Enemy Design Principles

- Each enemy type embodies 1-2 components (their behavior demonstrates what they drop)
- Varied behaviors (chargers, shooters, flankers, summoners, shielded)
- Some enemies weak/resistant to specific essences
- Bosses use full Spell Fusion-level attacks (preview of what player can become)

### Tiered Enemy Roster (Inspired by CotM Bestiary)

**Tier 1 — Common (Catacomb/Early)**
| Enemy | Behavior | Drops | Inspired By |
|---|---|---|---|
| Skeleton Archer | Lobs bone projectiles in parabolic arc | Bone essence | CotM Bone Head |
| Gremlin | Tiny, fast, erratic bouncing | Bounce behavior | CotM Fleaman |
| Crawler | Small, spawns 3-4 at a time, swarm pressure | Cinder essence | CotM Poison Worm |
| Wisp | Floats in sine-wave path, hard to hit | Pulse essence | CotM Medusa Head / Will O' Wisp |
| Mud Golem | Slow, tanky, walks straight at player | Linger behavior | CotM Mud Man |

**Tier 2 — Uncommon (Mid-game)**
| Enemy | Behavior | Drops | Inspired By |
|---|---|---|---|
| Bone Turret | Stationary tower, fires in 4 directions periodically | Shatter behavior | CotM Bone Tower |
| Charger | Telegraphs (flash), then lunges across screen | Bolt form | CotM Were-Jaguar |
| Siren | Confuses player controls briefly on hit (reverse movement) | Dark essence | CotM Succubus |
| Storm Caller | Airborne, spawns electric orbs that zap downward | Chain form | CotM Thunder Demon |
| Spike Caller | Flies overhead, spawns ground hazard lines beneath | Seed form | CotM Earth Demon |
| Treant | Roots erupt from ground toward player position | Sap essence | CotM Alraune |

**Tier 3 — Rare/Dangerous (Late-game)**
| Enemy | Behavior | Drops | Inspired By |
|---|---|---|---|
| Wraith | Teleports near player, attacks, vanishes | Possess behavior | CotM Devil |
| Basilisk | Line-of-sight beam that petrifies (long stasis) if player is in front | Stasis behavior | CotM Catoblepas |
| Sprinter | Zips across arena at high speed, drops premium loot | Haste Band / rare | CotM Skeleton Medalist |
| Seraph | Rare holy enemy, dangerous multi-attack pattern | Hollow essence | CotM Fallen Angel |
| Dullahan | Headless knight, throws head as homing projectile, tanky | Saw form | CotM Dullahan |

### Boss Designs

**Legion (Mid-game Boss)**
- **Phase 1 — The Shell:** Massive writhing mass of bodies. Player must destroy outer shell sections. Each section has a different elemental resistance (fire-weak north, ice-weak south, etc.). Shell segments break off and become minor enemies when damaged enough.
- **Phase 2 — The Core:** Shell removed, vulnerable fleshy core exposed. Core fires tentacle sweeps (like CotM) and spawns smaller body-horror minions. Core periodically regenerates shell fragments that must be re-broken.
- **Design intent:** Forces player to use different essence loadouts for different shell sections. Rewards preparation and combo versatility.

**Iron Golem (Late-game Boss)**
- **Phase 1 — The Fortress:** Massive slow golem that fills 1/3 of the screen. Creates shockwaves when it stomps (ground hazards). Punches send projectile walls. Periodically slams both fists creating expanding ring (like player Nova). Immune to frontal damage — must hit back/sides.
- **Phase 2 — Overheating:** At 50% HP, golem glows red. Faster attacks, leaves linger fire zones where it walks. Steam vents periodically erupt from its joints (dodge windows). Now vulnerable from any angle but hits twice as hard.
- **Phase 3 — Meltdown:** At 20% HP, armor plates fall off (become bouncing projectile hazards). Exposed machinery underneath is highly vulnerable. Golem becomes erratic — random charges, spinning attacks. Final explosion on death rewards a relic.
- **Design intent:** Tests player's trap/turret game (Bouncing Betty, Nikola's Coil, Medusa's Garden). Kiting boss that rewards Seed-form mastery. Phase 3 chaos rewards calm play under pressure.

---

## Sir Dagonet — The Fraud Knight (Recurring NPC)

_The gap between reputation and reality creates tension, comedy, and meaning._

### Concept

Arthur's court jester, knighted as a joke — but promoted to a major recurring NPC. He's the game's King (OPM) / Mr. Satan / Dan Hibiki: a fraud whose value lies in something nobody measures.

In actual Arthurian legend, Dagonet once charged King Mark of Cornwall in full armor and Mark fled — not because Dagonet was dangerous, but because anyone charging that recklessly must be insane. He won through pure bluff. The fraud archetype is baked into 15th-century source material.

### Pre-Meeting Reputation (Ambiguous NPC Dialogue)

Before the player meets Dagonet, NPCs hype him in ways that could be genuine or sarcastic:

- **Palamedes:** "Sir Dagonet? He once made the King of Cornwall flee without drawing a blade. That is... a kind of power."
- **Dinadan:** "If you find Dagonet alive out here, that alone proves God has a sense of humor."
- **Dying soldier:** "Dagonet rode through here three days ago. We thought reinforcements had come. He was looking for his horse."
- **Bedivere:** "Arthur loved that man. I never understood why. Maybe that IS why."

### First Encounter (Level 3 or 4)

Found hiding inside a hollow tree stump in the Wyldwood, wearing plate armor that's clearly too big, eating cheese. His horse is gone. He's been "holding this position" for six days.

### The Dragon Scene (Early Recurring Appearance)

One of Dagonet's first reappearances after the initial meeting. The player enters a screen and finds Dagonet posed heroically, one foot on a dead creature, addressing Morien mid-monologue:

> "MORIEN! You're just in time. You JUST missed it. A DRAGON. Enormous. Teeth like swords. I slew it with a single thrust — right through the heart. It barely had time to—"

The screen composition reveals the "dragon" is a foot-long newt. It's not even dead — it's sleeping.

> "—it put up a tremendous fight. I'm actually still shaking. Do you want to buy its scales? Very rare. Fireproof."

He sells "Dragon Scales" for an absurd price. They appear to be useless junk. **Gameplay hook:** one of the junk items in his inventory this visit is actually a component for a Spell Fusion. The player discovers this 10+ hours later. The joke has a mechanical payoff — Dagonet's fraud accidentally delivers real value, as always.

He immediately treats Morien as his rescue party and starts giving tactical assessments that are... actually correct. His read on enemy patrol patterns is dead accurate. His understanding of the Wyldwood is better than anyone else's. But every time combat happens near him, he panics, hides, or "covers the rear flank" (runs away).

The player realizes quickly: this man cannot fight. At all. He is a Knight of the Round Table with zero combat ability.

### Gameplay Role — The Useful Fraud

**What he can't do:**
- Fight (0 combat ability, refuses to enter danger)
- Be brave in any traditional sense
- Stop talking

**What he actually does:**
- **Map intel:** He wandered the Wyldwood for weeks while lost. He's accidentally mapped more of it than any scout. Marks rooms on the player's map — including secret ones.
- **Spell Fusion hints:** He watched Fae creatures fight from hiding spots. Describes combo effects he witnessed — "The vine creatures, when they catch fire, they don't die. They *spread.*" Encoded hints for undiscovered Spell Fusions.
- **Merchant (sort of):** Hoarded random junk while hiding — some valuable. Weird prices, random inventory. Prices garbage high ("this is a VERY good stick") and accidentally underprices rare things.
- **Lore drops:** Overheard things while hiding. Saw things nobody else saw *because* nobody notices the jester. Knows more about Morgan's plan than anyone expects — delivered accidentally, mid-ramble.

**Recurring:** After first encounter, Dagonet appears in random locations throughout the rest of the game. Always in a ridiculous hiding spot. Always with new intel. Mobile shop + hint system wrapped in a comedy character.

### The Emotional Turn (Act III)

Morien is separated, cornered, or incapacitated. A chokepoint is blocked — not by a combat challenge, but by something requiring *distraction.* Steel Tides soldiers or Morgan's sentries guard the path.

Dagonet is there. Hiding, as usual.

He steps out.

He doesn't fight. He performs. Walks out in his oversized armor, announces himself as "SIR DAGONET, KNIGHT OF THE ROUND TABLE, SLAYER OF THE CORNISH KING, CHAMPION OF TWELVE TOURNAMENTS" — all lies, delivered with absolute conviction. Guards hesitate. Some laugh. One recognizes the name (the reputation works).

In that window: Morien gets through.

Dagonet takes a hit. A bad one. Goes down. When Morien circles back:

> "I know what I am, Morien. I've always known. Arthur knew too. He knighted me anyway. You know what he said? 'Courage isn't the absence of fear, Dagonet. It's cheese in a hollow stump while the world ends around you.'"
>
> "...I may have added the cheese part."

He survives. Not a tragic death — that would undercut the character. Recovers, still a coward, still hides. But the player knows he *chose* to step out that one time. And it mattered.

### Player Experience Arc

1. First encounter: "lmao this guy is useless"
2. Mid-game: "okay his hints are actually cracked, where is he?"
3. Act III moment: "...oh. Oh no. DAGONET."
4. Post-game: "he was my favorite character"

### Thematic Mirror

Dagonet is the inverse of the villain framework. Morgan is terrifying because her reputation matches reality. Mordred is terrifying because his reputation *understates* reality. Dagonet's reputation overstates reality — but his *value* is in something nobody measures.

He connects to Morien's arc: Morien is an outsider respected for his blade but not part of the inner circle. Dagonet is an insider everyone treats as a joke. Both underestimated for different reasons. They should recognize something in each other.

---

## NPC Character Design

_Full narrative profiles in STORY-BIBLE.md. This section covers **gameplay roles.**_

### Core Recurring NPCs

| NPC | Gameplay Role | Where Found | Mechanic |
|-----|-------------|-------------|----------|
| **Sir Palamedes** | Primary Merchant | Level 2+, fixed camp location | Sells weapons/relics/potions. Inventory gates to story progress — upgrades massively post-twist. Was sandbagging. |
| **Sir Dinadan** | Roaming Quest Giver | Found in different zones each visit | ~15-20 quests. Best writing in the game. Rewards: Spell Fusion components, map reveals, recipes. |
| **Sir Dagonet** | Mobile Hint/Shop/Intel | Random hiding spots, changes each visit | Map intel, Spell Fusion hints, junk merchant with hidden gems. See full section above. |
| **Sir Brunor** | Hard-Quest Giver | Fixed post, late-Act I onward | ~8-10 quests. Hardest optional content. Best relics + rare components. Entirely missable. |
| **The Lady of the Lake (Nimue)** | Hidden Merchant / Lore | Drowned Valley underwater cave | Rarest relics and components. Speaks in riddles. Knows the Sigh's true origin. |
| **Sir Gareth (Beaumains)** | Budget Merchant | Ashlands, near front line | Fair prices on food/potions. The anti-Kay. Accessible early. |
| **Brother Caradoc** | Healer / Blessed Items | Fractured Cathedral | Free healing. Sells anti-Fae blessed items. |

### Quest-Giving NPCs (Non-Recurring)

| NPC | Quest Type | Zone | Reward Category |
|-----|-----------|------|-----------------|
| **Sir Yvain** | Beast hunts | Whispering Swamp | Essences, hunt-specific relics |
| **Sir Bors** | Holy site defense | Fractured Cathedral | Lore, blessed components |
| **Sir Pelleas** | Rescue (break Fae charm) | Whispering Swamp | Ring reward |
| **Sir Tor** | Escort mission | Ashlands | Unlocks Tor as ally at Palamedes' camp |
| **Sir Tristan** | Heal the wounded knight | Iron March | Relic + Cerdic intel (missable) |
| **Dame Ragnelle** | Curse-breaking | Wyldwood Hearth | Unique Spell Fusion recipe |
| **The Green Knight** | Honor challenge | Wyldwood border | Immediate relic + timed Act III quest |
| **Old King Pellinore** | Ghost encounter (fight or talk) | Swamp depths | Pellinore's Spear OR Questing Beast intel |
| **Sir Safir** | Messenger / delivery quests | Roaming | Minor components, map intel |
| **Sir Lamorak** | Ghost / lore (no quest) | Drowned Valley | Pellinore bloodline lore |
| **Henwen the Sow** | Follow the pig | Various | Hidden rooms, buried treasure |

### Lore / Atmosphere NPCs (Non-Interactive or Minimal)

| NPC | Type | Zone |
|-----|------|------|
| **Sir Marhaus** | Dead body, environmental lore | Ashlands |
| **Parzival** | Seen in the distance, never spoken to until finale | Various |
| **Mordred** | Appears once, watches, leaves | Mid-game |

---

## Quest System & Spell Fusion Unlocks

_Spell Fusions are discovered in two ways: (1) the player equips the right 3-slot combo and it triggers automatically (experimental discovery), or (2) an NPC quest rewards the **recipe** — the Fusion name appears in a journal and the components are identified, so the player knows what to hunt for._

_Some Fusions are experiment-only (no quest). Some are quest-only (can't stumble into them). Most can be found either way, but the quest gives you the recipe so you're not guessing._

### Design Rules
- **Tier 3 "Super" Fusions** = always have a quest path (these are the endgame chase; players shouldn't have to brute-force 343 combos)
- **Castlevania-Inspired Fusions** = mixed (some quest, some experiment-only as easter eggs)
- **Quests can reward 1-3 recipes** depending on quest difficulty
- **Quest givers hint at the Fusion thematically** — Dinadan describes what he saw, Dagonet rambles about creature behavior, Brunor's hard quests reward the best Fusions
- **Some quests reward components, not recipes** — getting the missing piece you need to complete a combo you've been chasing

### Spell Fusion Quest Table

#### Tier 3 "Super" Fusions — Quest Paths

| Fusion | Form | Behavior | Essence | Quest Giver | Quest Summary | Zone |
|--------|------|----------|---------|-------------|---------------|------|
| **Dragon's Teeth** | Bolt | Shatter | Bone | **Sir Brunor** | "The Bone Fields are overrun. Something is raising the dead faster than they fall. End it." Clear a necromantic node guarded by endless skeleton waves. Must destroy the source while being swarmed. | Ashlands (deep) |
| **Yggdrasil's Root** | Seed | Bloom | Sap | **Dame Ragnelle** | Break her curse by planting a Sap seed in enchanted ground and defending it as it grows into a purifying tree. She describes the fusion as her "true memory" — the way the world was before the Desolation. | Wyldwood Hearth |
| **Indra's Arrow** | Chain | Bounce | Pulse | **Sir Dinadan** | "There's a canyon full of iron pillars. Lightning jumps between them. Something lives in there and it's *angry.*" Navigate an environmental puzzle where lightning arcs between pillars; defeat the Storm Caller alpha at the center. | Drowned Valley |
| **Event Horizon** | Orbit | Stasis | Hollow | **The Lady of the Lake** | Nimue speaks of a "wound in the world" — a void tear in the deepest part of the Drowned Valley that's consuming everything around it. Reach it. Stabilize it. She gives the recipe as payment: "You've seen the void. Now wield it." | Drowned Valley (deep) |
| **Dragonbreath** | Shotgun | Linger | Cinder | **Sir Yvain** | His lion is dying from Fae frostbite. Track down a fire-drake in the swamp and harvest its flame gland. The drake fights in close range with cone attacks — a preview of what the Fusion does. Yvain gives the recipe in gratitude. | Whispering Swamp |
| **Rift Tide** | Wave | Linger | Hollow | **Sir Bors** | A holy well is being consumed by void corruption. The water itself moves in warped sine patterns. Purify the well by destroying void nodes while avoiding the rippling terrain. Bors calls it "the tide between worlds." | Fractured Cathedral |
| **Death's Scythe** | Saw | Bounce | Bone | **Old King Pellinore** (ghost) | Pellinore's ghost challenges Morien: "My bloodline hunts what cannot be caught. Prove you are worthy." A gauntlet fight in a bone-walled arena where projectiles bounce endlessly. Survive long enough and Pellinore yields the recipe + lore. | Swamp depths |
| **Gae Bolg** | Bolt | Stasis | Pulse | **Sir Feirefiz** | If Feirefiz is spared in Level 4, he tells Morien of a weapon his Saracen scholars described — "a spear that freezes fate." He gives a partial recipe and marks a location where Morien can witness the phenomenon: a lightning-struck battlefield where time stutters around impact points. Reach it, survive the time-distortion enemies, complete the recipe. | Fractured Cathedral outskirts |
| **Gebel's Swarm** | Shotgun | Possess | Dark | **Sir Dagonet** | Dagonet, rambling as usual: "I was hiding in a bell tower and these BAT THINGS came out of nowhere and they just — they went INTO the other creatures and the creatures started fighting EACH OTHER and I thought I was losing my mind—" He marks the bell tower. Go there. Fight the dark bat swarm. Survive. Dagonet accidentally described the Fusion. | Wyldwood (various) |
| **Surtr's Crown** | Orbit | Bloom | Cinder | **Sir Brunor** | "A fire giant stalks the borderlands. It wears a crown of growing flame. I've seen three knights try. None returned." Solo boss fight against a massive fire elemental whose orbiting ring of fire grows over time. Defeat it before the ring fills the arena. | Iron March border |
| **Grave Call** | Seed | Shatter | Bone | **Sir Dinadan** | "There's a graveyard where the dead bury themselves, and when you step on the graves, they EXPLODE upward. I am not making this up. Go see for yourself. Actually, don't. Actually, do — someone needs to deal with it." Clear the erupting graveyard. The explosions are the Fusion in action. | Ashlands |
| **Tiamat's Garden** | Wave | Bloom | Sap | **Henwen the Sow** (follow quest) | Following the magical pig through a Wyldwood trail leads to a hidden garden where waves of thorns and vines have grown into an impenetrable wall of beauty. Examine it. A Sap creature guards it — defeat it, and the garden yields the recipe inscribed on a living leaf. | Wyldwood (hidden) |
| **Chronos' Chain** | Chain | Stasis | Hollow | **The Green Knight** | The Green Knight's timed quest (from Act I) resolves here. Return to the crossroads in Act III. Time has literally frozen around it — enemies mid-stride, rain suspended. The Green Knight waits, impressed you returned. Fight him in the frozen space. He yields the recipe: "You understand patience. Now understand eternity." | Wyldwood border |
| **Pillars of Muspelheim** | Bolt | Linger | Cinder | **Sir Gareth** | Gareth asks for help defending his supply cache from a Cinder-type siege. Waves of fire creatures attack, leaving linger trails everywhere. Survive all waves. Gareth finds the recipe in a scorched journal on one of the enemy corpses — "They were trying to build this." | Ashlands |
| **Vajra** | Saw | Shatter | Pulse | **Sir Dinadan** | "I threw a rock at something electrical and it SHATTERED INTO LIGHTNING. I'm done. I'm going home. Here's where it was." The "rock" was a Pulse-charged mineral. Navigate to the mineral deposit, fight the Pulse enemies guarding it, shatter the mineral to release the recipe. Dinadan's directions are, surprisingly, perfect. | Drowned Valley |
| **Morrigan's Court** | Orbit | Possess | Dark | **Sir Dagonet** | Dagonet found an ancient Fae court in a cave — ghostly dark spirits circling a throne, possessing any creature that wanders in. "I watched for three hours. I was too scared to leave. It was actually kind of beautiful." The cave is real. Clear it. The throne holds the recipe. | Wyldwood (deep) |

#### Castlevania-Inspired Fusions — Quest Paths

| Fusion | Components | Quest Giver | Quest Summary | Zone |
|--------|-----------|-------------|---------------|------|
| **Cocytus** | Orbit + Shatter + Hollow | **The Lady of the Lake** | "The ice remembers what the water forgets." Nimue describes a frozen Fae battlefield where orbiting shards of ice shatter outward perpetually. Find it. The battlefield is a combat puzzle — shards damage enemies AND Morien. Navigate it, defeat the frozen guardian, claim the recipe from the center. | Drowned Valley (frozen wing) |
| **Boreas' Gale** | Saw + Bloom + Hollow | **Sir Yvain** | A tornado-like Fae entity is destroying the swamp. Yvain's lion tracked it. It's a living buzzsaw of void energy that grows as it moves. Defeat it before it reaches the inhabited area. Its core contains the recipe. | Whispering Swamp |
| **Solomon's Seal** | Seed + Possess + Bone | **Sir Bors** | Bors discovered an ancient binding circle in the cathedral crypt — a "seal of Solomon" that summons skeletal guardians when triggered. The circle is corrupted. Purify it (fight through waves of possessed skeletons) and Bors transcribes the recipe from the restored seal. | Fractured Cathedral (crypt) |
| **Mjölnir's Mark** | Seed + Linger + Pulse | **Sir Dinadan** | "Someone left lightning landmines in a field. A FIELD. Who DOES that? Anyway, three soldiers walked into it and now it's a persistent shock zone and nobody can get through." Clear the minefield by triggering them carefully. At the center: a Pulse artifact containing the recipe. Dinadan: "You're welcome for the directions. I accept payment in not being asked to go there." | Iron March |
| **Brynhildr** | Orbit + Stasis + Bone | **Sir Brunor** | "There is a barrow. Inside, a Valkyrie sleeps. Her bone swords orbit her endlessly, freezing anything that approaches. She's been there for centuries. I need what's behind her." The hardest combat puzzle in the game — get past the orbiting freeze-swords to reach the inner chamber. Recipe + a relic. | Wyldwood Hearth (deep) |
| **Fomorian's Charge** | Bolt + Bloom + Dark | **The Green Knight** | Offered as an alternative reward during his crossroads challenge (Act I). Instead of the timed quest (Chronos' Chain), choose the immediate combat trial: the Green Knight turns Morien into a living shadow projectile and he must destroy targets in one charge. Master it, keep the recipe. Can only choose one — Fomorian's Charge OR Chronos' Chain from this NPC. | Wyldwood border |

#### Additional Fusions (New — Expanding the Grid)

| Form | Behavior | Essence | Fusion Name | Effect | Quest Giver | Quest Summary |
|------|----------|---------|-------------|--------|-------------|---------------|
| Nova | Shatter | Cinder | **Ragnarök** | Point-blank explosion that shatters into fireballs in all directions, each leaving linger fire | **Sir Brunor** | "A fire giant's heart still beats inside a dead colossus. It pulses. Everything around it burns." Reach the colossus interior, destroy the heart. The explosion IS the Fusion. |
| Lance | Stasis | Pulse | **Rhongomyniad** | Arthur's legendary spear — piercing beam that freezes time along its entire path, then detonates the frozen line | **Sir Kay** | Kay, grudgingly: "The King's spear-technique. He can't wield it anymore. Someone should." Prove yourself to Kay through a combat trial at Camelot's gate. Hardest single-encounter in the game. |
| Volley | Bloom | Dark | **Morrígan's Flock** | Three-round burst of shadow ravens that grow in flight, possessing what they hit | **Sir Lamorak** (ghost) | Lamorak's ghost asks Morien to avenge his murder by defeating a phantom of his killer. The phantom uses this Fusion against you. Survive it, learn it. |
| Chain | Linger | Sap | **Mistletoe's Chains** | Lightning vines chain between enemies, leaving lingering heal-on-hit zones where they connect | **Sir Pelleas** | After breaking his Fae enchantment, Pelleas remembers something: "The Lady's garden had vines of lightning. They healed the earth where they touched." He marks the garden. Go there. Fight through it. |
| Saw | Linger | Dark | **Arondight's Shadow** | Lancelot's corrupted sword — buzzsaw leaves a trail of dark zones that fear enemies into each other | **Sir Tristan** (if healed) | Tristan tells Morien about Lancelot's descent: "His blade left shadows. I saw it once. The shadows were afraid of each other." Find Lancelot's abandoned camp. The sword's residue still lingers. |
| Shotgun | Stasis | Bone | **Niflheim Blast** | Bone pellet shotgun that freezes on hit, shattering frozen enemies into more bone projectiles | **Sir Dagonet** | "I sneezed in a bone cave and everything FROZE and then SHATTERED and then MORE BONES and I ran. I ran so fast, Morien." The cave is real. Clear it. |
| Nova | Possess | Sap | **Green Man's Embrace** | Point-blank vine burst that possesses enemies, turning them into Sap allies who heal Morien | **Brother Caradoc** | The monk's garden has been corrupted — plants possess animals that wander in. Help him purify it. In doing so, learn to control the effect. |
| Volley | Bounce | Pulse | **Tesla's Volley** | Three-shot burst of lightning that bounces off walls, each bounce adding another bolt | **Sir Safir** | Palamedes' brother brings a message: "My brother says you should see the lightning gallery in the eastern ruins. He wouldn't say more." Go there. The gallery is a natural bouncing-lightning phenomenon. |
| Lance | Bloom | Sap | **Excalibur's Light** | Piercing beam of golden-green energy that grows wider, healing everything in its path (allies, Morien) while damaging enemies | **Parzival** (finale only) | The only Fusion rewarded in the main story. During the finale, if Morien clears Parzival's path successfully, Parzival turns and — for the first and only time — speaks directly to Morien. "Take this. For what you've given." The ultimate Fusion. Post-game only. |
| Wave | Shatter | Silk | **Arachne's Loom** | Sine-wave that shatters into web threads on impact, creating a net that snares everything in the room | **Dame Ragnelle** (post-curse) | In her true form, Ragnelle teaches Morien the art of Fae weaving: "The old magic moves in waves. When it breaks, it catches." Combat trial in her hidden workshop. |
| Orbit | Linger | Pulse | **Camelot's Ward** | Electrical field orbits Morien, leaving persistent shock zones that form a defensive perimeter | **Sir Gawain** | Gawain, at the bridge: "I've held this position for nine days. You want to know how? Watch." He demonstrates a technique. Survive the wave he held alone. He teaches the recipe. |
| Seed | Stasis | Dark | **Mordred's Trap** | Dark landmines that freeze time AND fear enemies — they're stuck in place, terrified, unable to act | **Sir Kay** | Kay finds this recipe in Mordred's abandoned quarters: "The traitor left his notes. Might as well use them against him." Uncomfortable to wield — it's Mordred's technique. |

---

## Villain Design Framework

_Based on the 4-pillar terrifying villain framework: **Visible Evil → Overwhelming Power → Compelling Goal → Unpredictability**, plus **Aura** (presence, calm, how others react)._

_Full character details in STORY-BIBLE.md. This section covers how villains function as **game design** — what the player experiences._

### Morgan le Fay (Primary Antagonist — The Aizen)

Morgan is the villain the player should think about after they stop playing.

**Visible Evil (not just revealed evil):**
- The player must *participate* in Morgan's evil unknowingly. The Leyline Anchor missions in Act I-II should have consequences the player witnesses — an NPC ally dies because of guidance Morgan gave in a dream, a village falls because the anchor Morien activated weakened its wards. The betrayal at midgame hits harder when the player realizes *they* did it.
- Key design rule: at least one moment before the Act II twist where the player follows Morgan's instructions and something terrible happens as a direct result.

**Overwhelming Power:**
- When Aglovale dies and Morien confronts Morgan's astral form in rage, she dismisses his attack effortlessly. Not a fight — a swat. One animation. The power gap must be *shown*, not told.
- The Glass Fortress (Level 7) should reinforce this: Morgan controls the environment itself. The boss fight isn't "damage her health bar" — it's "survive her arena while finding the one thing she can't control."

**Compelling Goal (already strong):**
- "I was trying to save Logres. The Grail quest will fail. Parzival doesn't have the compassion to ask the Question. I was the backup plan."
- This line reframes the entire game. The player should feel genuinely uncomfortable defeating her. She's a Justice Antagonist whose *diagnosis* is correct — only her methods are wrong. Maybe.

**Unpredictability (biggest lever):**
- During Act III, when Morien destroys the anchors he activated, some have fail-safes. Morgan left messages at each: "I knew you'd come here, Morien." She anticipated his rebellion. She understands people — that's her power, not magic.
- Then at the Glass Fortress: she's genuinely surprised by ONE thing. The Faerie's Sigh bonding to Morien. The weapon she built chose someone else. That single crack in her omniscience makes her human and *more* terrifying — she's not a god, she's just smarter than everyone, and the one variable she missed is the thing she gave away.

**Aura:**
- Morgan is always calm. Always. Even in defeat. Her final line is delivered without desperation. She still thinks she's right.
- Voice/text tone: measured, almost kind. Never raises her voice. NPCs who mention her speak carefully, like the name itself is dangerous.

### Oberon & Titania (Fae Sovereigns — World Antagonists)

These two are not "evil" in human terms. They ARE the threat. Like a hurricane with opinions.

**Visible Evil:**
- The Wyldwood zones *are* their evil. Trees that move, animals that are monsters, landscape that attacks. Every Fae zone is Oberon and Titania's violence made environment. No cutscene needed — the player lives it.

**Overwhelming Power:**
- Seed an early encounter (Act I or II) where the player accidentally wanders near Oberon's presence. No fight. No dialogue. The screen warps, colors shift, controls briefly feel wrong. The game itself tells you: *you are near something that doesn't acknowledge your existence.* Then it passes. That's it. That's enough.
- The finale merge (Oberon + Titania) is the payoff. A World Antagonist made flesh. Not reasoning with. Not negotiating. Just IS.

**Compelling Goal:**
- From their perspective, they're restoring the natural order. Humans paved over the wild world. The merger isn't revenge — it's gardening. This is terrifying because they're not wrong from a non-human perspective.

**Unpredictability:**
- Oberon finds the war "amusing." Have him appear once and *help* Morien — not out of kindness, but entertainment. A force of nature that sometimes blows your way. That's scarier than consistent hostility.

**Aura:**
- Inhuman indifference. They don't hate you. They don't notice you. The rare moments they do notice you should feel like a deer realizing it's being watched by something ancient.

### Mordred (The Sequel Griffith — Devil Antagonist)

Mordred never fights in this game. That's the point.

**Visible Evil:**
- Show consequences of his political moves: displaced knights mention him, broken alliances trace back to him, NPCs reference things he's done off-screen. The player assembles the picture over time.
- He's positioning while everyone else bleeds. By post-credits, the player should feel dread because they *haven't* fought him.

**Overwhelming Power:**
- Not combat power — political power. People obey him. Armies shift at his word. Knights who should be helping Morien are tied up dealing with Mordred's machinations. His power is felt through absence and misdirection.

**Unpredictability:**
- Mordred appears ONCE in the entire game. Briefly. Does nothing. Just watches Morien. Then leaves. No dialogue, or one line at most.
- The player should spend the rest of the game wondering what that was about.

**Aura:**
- Quiet confidence. When he appears, every NPC nearby is nervous. He's the only character who makes other *villains* uncomfortable. Morgan mentions him carefully. That tells you everything.

### Villain Design Summary

| Principle | Morgan le Fay | Oberon/Titania | Mordred |
|-----------|--------------|----------------|---------|
| Visible evil | Player unknowingly causes harm via her guidance | Wyldwood zones = their violence as environment | Political fallout visible through NPC dialogue |
| Power gap | Swats Morien's attack; controls Glass Fortress arena | Screen warps near their presence; finale merge | Armies move at his word; power felt through absence |
| Goal | Save Logres her way (might be right) | Restore wild nature (not wrong, just inhuman) | Pure ambition, no noble motive (contrast to Morgan) |
| Unpredictability | "I knew you'd come here" + one genuine surprise | Helps Morien once, for amusement | Appears once, does nothing, leaves |
| Aura | Always calm, even in defeat | Inhuman indifference | Makes other villains nervous |

---

## Inspirations

- **Zelda 1** — world structure, exploration, screen-by-screen rooms
- **Castlevania: Aria of Sorrow** — kill → rare essence drop, enemy abilities as player powers
- **Castlevania: Harmony of Dissonance** — spell books × subweapons, visual spectacle per combo
- **Castlevania: Circle of the Moon** — DSS card grid, discovery joy
- **Castlevania: Curse of Darkness** — Innocent Devil familiar system
- **Castlevania: Curse of the Moon** — Gebel's bat swarm → Dark essence fusions
- **Altered Beast** — visual power transformation
- **Shovel Knight: Plague of Shadows** — multi-axis crafting
- **Axiom Verge** — weird bullet paths, Kilver-style close range, wall-traveling projectiles
- **Binding of Isaac** — item synergies (items, not core system)

---

## Development Phases

### Phase 1 — The Wand Feels Good (arena prototype)
- [x] Basic arena, player, shooting, enemies
- [x] 3-axis component system
- [x] Test mode, dash, fire rate, damage numbers, status effects
- [ ] **Wave fix** — sine wave travel path
- [ ] **Chain fix** — double range, visible lightning arcs
- [ ] **Seed fix** — drop at feet as landmines, bigger damage
- [ ] **Add Possess behavior** (on kill → enemy fights for you)
- [ ] **Add Stasis behavior** (on hit → time freeze bubble)
- [ ] **Replace Brine → Dark essence** (shadow/bats/fear)
- [ ] **Promote Pierce → Caliburn Shard relic**
- [ ] **Promote Siphon → Nimue's Tear relic**
- [ ] **Remove Bind behavior**
- [ ] **Rework Sap** — owns healing niche now
- [ ] Familiar placeholder (visible companion, essence-reactive skin, minimal AI)
- [ ] Cinder explosions (actual particle bursts)
- [ ] Pulse lightning arcs (visible electrical chains)
- [ ] Shield Bash (replaces dash as primary defensive move)
- [ ] Player visual changes based on loadout
- [ ] 5+ Spell Fusions with unique visuals and names
- [ ] Saw form (boomerang)
- [ ] Charge relic prototype

### Phase 2 — The World Exists
- [ ] Zelda 1 screen-by-screen overworld (small: 8×8 grid?)
- [ ] Screen transitions
- [ ] 5+ distinct enemy types with real AI
- [ ] Rare essence drops (not every kill)
- [ ] Shop NPC (buy shield, potions, rings)
- [ ] First dungeon with keys/boss

### Phase 3 — Systems Depth
- [ ] Familiar system (1-2 familiars, essence-reactive visuals)
- [ ] 5-10 relics placed in world
- [ ] Ring system (2 slots)
- [ ] Spell Fusion discovery UI
- [ ] Full visual transformation system
- [ ] Horse traversal for overworld

### Phase 4 — Content & Polish
- [ ] 20-30 Spell Fusions (mythology/religion naming)
- [ ] Full map + dungeon set
- [ ] Sound design + music
- [ ] Boss fights testing combo knowledge
- [ ] Balancing pass

---

## Open Questions
- [ ] How many overworld screens? (Zelda 1 had 128)
- [ ] How many dungeons?
- [ ] Story beyond "knight explores Camelot"? (see STORY-BIBLE.md)
- [ ] Multiplayer potential?
- [ ] How does the horse work in dungeons? (Probably can't ride indoors)
- [ ] How many familiars and how do they level/evolve?
- [ ] **"Why play this game?"** — the combo discovery loop + visual spectacle + exploration. But needs a hook beyond mechanics. What's the emotional pull?

---

## Combat Loop — Stagger & Break System (v4, 2026-04-03)

_Inspired by FF7R stagger, Sekiro posture, Doom Eternal resource loop, and the Balatro insight: power fantasy comes from earned agency, not spectacle._

### Core Loop

```
Basic attacks (free) → build Stagger → BREAK → Unleash Fusion (costs Ether) → Ether drops from broken enemy → repeat
```

Every enemy has **HP** and a **Stagger Bar**. The combat rhythm is:

1. **Soften** with raw Form attacks (Bolt, Lance) — free, builds Stagger slowly
2. **Pressure** with dual combos (Form+Behavior or Form+Essence) — costs light Ether, builds Stagger faster, applies status effects
3. **BREAK** — Stagger bar fills → enemy enters Break state (2-3 seconds)
   - Takes **3× damage** from all sources
   - Drops **Ether shards** when hit (primary Ether income)
   - Flashing "BREAK!" indicator above head
   - Visually reels/flashes/staggers
4. **Execute** with Spell Fusions (triple combos) — costs heavy Ether, massive damage on Broken enemies
5. **Collect Ether** → fund next Fusion cycle

### Why This Works

- **Raw attacks matter** — they're your Stagger builders, not just weak filler
- **Dual combos are the workhorse** — status effects + Stagger pressure
- **Fusions are the payoff** — expensive, powerful, best used during Break windows
- **Ether economy forces aggression** — passive regen is a trickle, killing is the real income
- **Players can't just spam their best combo** — Ether gates the top-tier stuff

### Ether Economy

| Source | Ether Gain |
|--------|-----------|
| Passive regen | ~1 EP/sec (trickle, never enough alone) |
| Hitting Broken enemies | 3-5 EP per hit (primary source) |
| Killing any enemy | 2 EP flat |
| Ether crystal pickups | 10 EP (rare room drops) |

| Cost | Ether |
|------|-------|
| Raw Form attack | 0 EP (free) |
| Dual combo attack | 3-8 EP |
| Spell Fusion attack | 15-35 EP (varies by Fusion) |

### Element Counters

Enemies have ONE weakness, shown visually (not UI — read the creature):

| Enemy Type | Visual Tell | Weakness | Counter Effect |
|------------|-------------|----------|---------------|
| Nature/plant | Leaf particles, green glow | **Cinder** | 2× Stagger buildup |
| Bone/undead | Static crackle, pale | **Pulse** | 2× Stagger buildup |
| Fire/magma | Ember trails, red glow | **Bone** | 2× Stagger buildup |

**Key design rule:** Counters make Stagger FASTER, not damage higher. Wrong element still works — it just takes longer to reach Break. No hard gatekeeping, no forced menu pauses.

### Status Effects (Essence-Driven)

| Essence | Status | Duration | Combat Role |
|---------|--------|----------|-------------|
| **Cinder** | Ignite | 3s DoT | Sustained pressure. Stagger ticks over time. Fire spreads to nearby enemies |
| **Pulse** | Stagger | Instant | Burst disruption. Big single-hit Stagger spike. Briefly interrupts enemy attacks |
| **Bone** | Fracture | 4s debuff | Setup/payoff. Next 3 hits deal +50% Stagger. Broken enemies drop bone fragments (bonus Ether) |

Each essence has a distinct ROLE:
- **Cinder** = fire and forget (set them burning, focus elsewhere)
- **Pulse** = interrupt and burst (clutch saves, fast Break on single targets)
- **Bone** = setup for big damage windows (Fracture then swap to Fusion)

### Loadout Presets (Quick-Swap System)

Players can save up to 6 loadout presets (component combinations).

| Input | Action |
|-------|--------|
| 1, 2, 3 | Cycle Form, Behavior, Essence (existing) |
| Scroll wheel | Cycle through saved presets |
| 4-9 (keyboard) | Direct-select preset 1-6 |

- Presets assigned in inventory screen
- Swapping is instant — wand color flash + particle burst
- Enables mid-fight tactical switching without menu pauses
- Scroll wheel = fast for mouse users, number keys = laptop fallback

### Enemy Stagger Tiers

| Tier | Stagger Bar | HP | Examples |
|------|------------|-----|---------|
| Minion | 20 | 15-25 | Slimes, bats, basic mobs |
| Standard | 50 | 40-60 | Knights, wolves, archers |
| Elite | 100 | 80-120 | Mini-bosses, armored enemies |
| Boss | 200+ | 200+ | Wyrm, chapter bosses |

Bosses may have **multiple Break phases** — Stagger bar refills after each Break, but subsequent Breaks last longer.

### Combo Unlocking (Revised v4)

- **Dual combos** — always available when 2 slots filled. No relic required
- **Spell Fusions (triple combos)** — require the **Grimoire** relic
- **Tome of Binding** — REMOVED as gating relic. Repurposed or cut entirely

### Demo Essences (Vertical Slice)

For the 9-room demo, only 3 essences are available:
- **Cinder** (fire/destruction)
- **Pulse** (lightning/disruption)
- **Bone** (necromancy/structure)

Silk, Hollow, Sap, Dark are full-game essences not present in the demo.

This gives: 2 Forms × 4 Behaviors (incl. None) × 4 Essences (incl. None) = 32 base loadouts → 21 named dual combos + 6 named Spell Fusions + 12 unnamed triple slots to design.

---

_The wand is a gun. We know it. The knight doesn't._

---

## Combat Feel — Design Principles (from God of War / Doom / DMC5 analysis)

### Knockback Toward Cursor (Selective)
Certain weapons push enemies toward the player's cursor rather than away from the impact direction. This makes combat feel like you're *directing* the battlefield — placing enemies where you want them.

**Currently applied to:** Bone Spear (thrust knockback aims toward cursor)
**Candidate weapons:** Any high-knockback weapon where "placement" feels thematic (e.g., heavy slam attacks, charged shots). NOT applied universally — fast weapons like Bolt should push away naturally.

### Ether Full Glow
When ether bar reaches max, it pulses with a lambent breathing glow — slow blue shimmer with outer halo. Visual nag that says "you have resources, spend them." Discourages hoarding (Sekiro spirit emblem problem).

### First-Combo Discovery Moment
When a player activates a named combo (dual or triple) for the first time ever, the game:
1. Brief hitstop freeze (0.12s)
2. Screen shake
3. "NEW COMBO DISCOVERED" banner + combo name displayed large and bright (1.5s)

Makes every new combination feel like an *event*. DMC5's reverse-engineered emotion: the excitement of discovery.

### Enemy Behavioral Identity (Future — Post-Weapon-Lock)
Each enemy type needs a **one-sentence behavioral role** that creates distinct tactical situations:
- "The one you kite" — fast, fragile, punishes standing still
- "The one you interrupt" — charges a devastating attack, stagger-vulnerable during windup
- "The one that shields others" — must be flanked or broken first
- "The one you herd" — explodes on death, push into groups with knockback weapons

Enemies should embody element weaknesses that force loadout switching (Doom Eternal model). Cinder→nature enemies, Pulse→bone/undead, Bone→fire. Players can't default to one combo forever.

**Emergent AI:** Enemies should react to player weapon types. Shielded enemies might dodge lance beams but be vulnerable to spread shots. Flying enemies resist knockback but are weak to homing. This creates natural weapon-switching pressure without artificial gates.

### Generous Hit Detection (Under Consideration)
Near-miss bolt projectiles could "graze" enemies for reduced damage. Rewards aggressive play, reduces frustration from pixel-perfect aim requirements.
**Status:** Deferred. May conflict with "players should aim" philosophy. Keep as optional polish-pass feature. Current tight collision encourages skill mastery which may be more satisfying long-term.

---

## Villain Design Framework — Pure Evil vs Complex Antagonists

_Based on OSP's "Pure Evil" villain analysis. Reference for all dialogue, quests, boss design, and cutscenes._

### Core Principle
Not every villain needs to be sympathetic. Pure Evil villains work when you **fully commit**. Complex antagonists work when you **fully commit**. Half-assing either kills both. Never give a Pure Evil villain a sympathetic motive, and never give a complex antagonist Pure Evil panache.

### The Spectrum

| Character | Archetype | Commitment |
|---|---|---|
| **Mordred** | **Pure Evil** | Simple motive (power), total confidence, loves being villainous, no sympathy, no redemption |
| **Morgan le Fay** | **Complex Antagonist** | Hero of her own story. Genuinely believes she's saving Britain. Methods are the problem, not motives |
| **King Oberon** | **Pure Chaos** | Not evil — wild nature personified. Finds the war amusing. No plan, just vibes. Hexadecimal archetype |
| **Queen Titania** | **Committed Ideologue** | One core belief ("humans are pests"), unwavering confidence, leans pure evil in presentation |
| **Cerdic of Wessex** | **Complex Antagonist** | His people are starving. Reasonable enemy. Mirror of Morgan |

### Pure Evil Checklist (Mordred, and any boss/NPC who fits)

1. **Irrelevant backstory** — how they got this way doesn't matter. Don't explain it. Don't justify it.
2. **Simple, selfish motivation** — power, revenge, fun. One sentence max.
3. **Unwavering confidence** — they know exactly who they are. No self-doubt. No moral questioning.
4. **Visibly enjoying villainy** — every scene should feel like they're having a blast. Smirking. Showing off.
5. **Third-act breakdown** — when they finally lose control, the mask cracks HARD. The audience has been waiting for this. Make it count.
6. **Give them stuff to DO** — a villain who just sits on a throne is boring. Scheming, taunting, wrecking things, interacting with foils. They need screen time doing villain things.

### Complex Villain Foil (Critical for Pure Evil)

Pure Evil villains need a complex character nearby to bounce off. This contrast is what keeps them interesting.

- **Mordred's foil = Morgan le Fay.** Her genuine love for him vs. his exploitation of that love. She schemes to put him on the throne; he'd discard her the moment she's inconvenient. This is the Joker/Harley dynamic — the pure evil character is MORE evil because someone loves them and they don't care.
- **Titania's foil = Oberon.** She's committed and hostile; he's amused and detached. Her intensity highlights his chaos; his indifference highlights her conviction.

### Boss Design Application

- **Pure Evil bosses:** Taunt during the fight. Show off. Have fun. Then when the player starts winning, the confidence cracks → desperation phase. The transition from composed to desperate IS the third-act breakdown.
- **Complex antagonist bosses (Aglovale, Cerdic):** The player shouldn't WANT to win. Emotional weight. No taunting — these fights should feel tragic or reluctant.
- **Chaos bosses (Fae creatures, Oberon encounters):** Unpredictable. They're not angry, they're entertained. The fight is a game to them.

### Dialogue Guidelines

| Archetype | Dialogue Voice | What They NEVER Say |
|---|---|---|
| Pure Evil (Mordred) | Confident, amused, dismissive. Short sentences. Never explains himself. | "I had no choice" / "You don't understand" / anything sympathetic |
| Complex (Morgan) | Measured, passionate when challenged, maternal toward Mordred. Believes every word. | "I enjoy this" / pure villain monologues / cackling |
| Chaos (Oberon) | Whimsical, poetic, detached. Finds everything funny. | Anything angry or desperate. He doesn't care enough. |
| Ideologue (Titania) | Cold, absolute, regal. States facts, not opinions. | Self-doubt. Humor. She is deadly serious always. |
| Justice (Cerdic) | Blunt, military, tired. A king doing ugly work. | Villain speeches. He doesn't think he's a villain. |

### The Mordred Rule
Since Mordred is a sequel hook (never fought directly in this game), every appearance should build anticipation. The player should:
1. **Love watching him** — charisma is contagious. He's cool.
2. **Hate him** — because he's clearly exploiting Morgan, dismissing everyone, and enjoying the chaos.
3. **Desperately want to fight him** — and can't. Yet. That's the sequel hook.

Save the breakdown for the sequel. In THIS game, Mordred is untouchable and unbothered. That makes the eventual crack even better.

---

## Overworld Design Philosophy — Lessons from Zelda 1 (1986)

_Based on analysis of the original Legend of Zelda overworld. Reference for world layout, environmental design, and player guidance._

### Core Principle: Hostile Architecture as Storytelling
The Desolation War IS the overworld. The land is dying. The emptiness isn't a limitation — it's the story. Every barren field, abandoned camp, and silent ruin communicates that this world has been hollowed out. Resist filling every screen with content. Strategic emptiness makes discoveries feel earned and populated areas feel precious.

### Screen-by-Screen Spatial Design
- Each screen transition is a **commitment** — you can't see where you came from
- In hostile zones (Wyldwood, Ashlands), use visual similarity between screens to disorient. Shared tile patterns, repeated layouts, slight variations. The player's spatial memory should struggle
- In friendly zones (Palamedes' camp, Fractured Cathedral), use **distinct landmarks**. The relief of recognition after hostile sameness is powerful
- The Wyldwood should literally repeat screens — Fae magic rewriting reality. The mechanic IS the lore

### Environmental Legibility (Show, Don't Signpost)
- Spell Fusions are our key/lock system. Environmental puzzles should be solvable through **observation**, not UI markers
- Visual cue language: dark-veined walls = Bone-weak, shimmering barriers = Pulse-weak, overgrown roots = Cinder-burnable, frozen obstacles = Cinder-meltable, cracked stone = Shatter-breakable
- **Never put a marker on these.** Trust the player to learn the visual vocabulary
- NPC hints should be cryptic but fair (Dinadan, Dagonet, old men in caves)
- Spell Fusion discovery should be social — players sharing combos IRL/online, not reading a menu list

### Enemy Respawn & Attrition Economy
- Overworld enemies respawn on screen re-entry. No permanent "clearing." The war is ongoing
- Damage accumulates across screens. Healing is earned through combat mastery (Siphon/Nimue's Tear, ether management), not freely given
- This creates the Zelda 1 asymmetry: abundant punishment, limited recovery
- Bad players feel exhaustion. Good players feel flow. The combo system IS the survival tool

### Cryptic Guidance Hierarchy
1. **Environmental cues** — visual patterns the player learns to read (primary)
2. **NPC hints** — cryptic but fair. Dagonet accidentally reveals fusion recipes. Dinadan describes threats sarcastically. Palamedes gives blunt warnings (secondary)
3. **Old men in caves** — purchasable directions, fragment clues (tertiary)
4. **Player-to-player sharing** — the game's opacity should create community. 343 combos means no single player finds everything (emergent)

### Openness vs Survival Gating
- Most zones accessible from early game. No hard locks on exploration
- Survival is gated by gear and skill, not invisible walls
- Players CAN walk into high-level zones with starter gear. They'll die. That's the point
- "The world says yes to your feet while saying no to your survival"
- First successful trip through a dangerous zone should feel like a genuine achievement

### Music as Psychological Architecture
- Zone music should loop into invisibility — become the ambient hum of the area
- The TRANSITION between zone tracks matters more than the tracks themselves
- Crossing from safe to hostile zones should be a sonic descent
- Boss/dungeon music contrast should signal "you are in a different kind of danger now"

### New Game+ (Second Quest Philosophy)
- Remix enemy elemental weaknesses
- Move dungeon/secret entrances
- Change which Spell Fusions solve environmental puzzles
- Weaponize player knowledge — what they learned in the first playthrough should be unreliable
- The world that finally made sense becomes unfamiliar again

## Enemy AI Philosophy (from Into the Breach / AI and Games analysis, 2026-04-03)

### Core Principle: Perceived Intelligence > Actual Intelligence
The player's perception of how smart enemies are matters infinitely more than how smart they actually are. Into the Breach's enemies are individually dumb (score tiles, pick best, act) but feel smart because their *intentions are telegraphed*. God of War's Atreus feels helpful because he *tells you* what he's noticing.

### The Five Rules

1. **Enemy AI stays simple** — Chase/attack/stagger state machines with archetype-specific behavior weights. No coordination between enemies. If two enemies flank the player, that's emergent, not coded.

2. **Telegraph everything** — Visual wind-ups, positional tells, elemental glows before attacks. The player must be able to read what's coming. A readable enemy that kills you feels fair. A "clever" enemy that ambushes you feels cheap.

3. **Utility scoring per-enemy** — Each enemy evaluates its own situation. Archetype-specific weights:
   - Swarm → weight "get close" heavily, zero self-preservation
   - Armored → slow advance, shield-facing, punish head-on attacks
   - Evasive → maintain distance, teleport when cornered
   - Elemental → approach element-appropriate zones

4. **Never coordinate enemies** — No group tactics, no flanking AI. If enemies happen to surround the player, that's emergent geometry. Players will think it's intentional anyway (Into the Breach proved this).

5. **Nerf first, buff later** — Start every enemy slightly too easy and tighten. Never start with enemies that feel frustrating. Into the Breach's biggest difficulty wins came from *nerfing* the AI (picking 2nd-best option, anti-repeat scoring). Davis: "Good enough is generally what I'm aiming for."

### Unsolvable Prevention
Every combat encounter must be beatable with ANY loadout. Specific combos make encounters *easier*, not *possible*. If a room requires a combo the player doesn't have, that's a design failure.

### Barks — Visual Communication System
Since we're pixel art with no voice acting, "barks" are visual:

**Enemy barks:**
- **"!" symbol** — flashes above enemy when they spot the player or change state
- **Wind-up glow** — enemy's body color intensifies 0.3s before attack
- **Stagger sparks** — visual crackle when approaching stagger break
- **Death tells** — different dissolution per type (swarm pops, armored cracks, evasive fades)

**Player barks:**
- **"!" above Morien** — first time firing a new combo
- **"WEAKNESS"** — floating text when hitting an enemy with their weakness
- **"RESIST"** — floating text when hitting with wrong type (dimmed, smaller)

**Environmental barks:**
- Crystals pulse near relevant essence puzzles
- Water ripples when magic is cast nearby
- Bones rattle near Bone-element secrets
- Cracked surfaces shimmer subtly (no UI markers, trust the player)

### Difficulty Tuning Levers (for NG+ and future)
- Tighten chase vector randomness (enemies track more precisely)
- Reduce attack wind-up duration (less telegraph time)
- Remove "anti-repeat" scoring (enemies can target same thing twice)
- Add spawn replacement (kill an enemy, another spawns after delay)
- Elemental weakness multiplier reduction (2x → 1.5x)

## Movement System — Accessory Slot (2026-04-04)

### Core Concept
One button (Spacebar), multiple equippable movement abilities. ALttP Pegasus Boots / Roc's Feather / Hookshot model. Swap in inventory.

### Awakening Accessories
| Accessory | Input | Effect |
|-----------|-------|--------|
| **Iron Greaves** (default) | Space (standing still) | Cosmetic jump — visual Y offset, shadow scales, landing dust |
| **Knight's Shield** | Space + direction | Dash with i-frames |

### Campaign Accessories (Future)
| Accessory | Input | Effect |
|-----------|-------|--------|
| **Roc's Mantle** | Space (upgrades Greaves) | Double jump |
| **Shade Cloak** | Space + direction (upgrades Shield) | Invincible dodge through enemies (Hollow Knight) |
| **Grapple Hook** | Space + aim at anchor point | Pull to anchor / pull small enemies to you |
| **Prowl Cloak** | Space (hold) | Go prone/stealth — requires vision cone AI, cover system |

### Design Rules
- Only one accessory equipped at a time — meaningful choice
- Accessories are found/earned, not bought
- Each accessory enables unique traversal puzzles in its area
- Must be discoverable through environmental teaching (not tutorials)

## BREAK Finisher System (2026-04-04)

### Trigger Conditions
1. **Melee Finisher** — Enemy in BREAK state, ≤25% max HP, player within 60px, press E
2. **Ranged Finisher** — Kill a broken enemy with any triple Spell Fusion (auto-triggers)

### Animation Sequence (1.2 seconds total)
| Phase | Time | What Happens |
|-------|------|--------------|
| Zoom In | 1.2→0.8s | Camera scales to 1.25x centered on enemy, gameplay freezes |
| Jitter | 0.8→0.3s | Enemy turns white, shakes left-right (Smash Bros hit-freeze) |
| Death Burst | 0.3→0.0s | Element-specific explosion, camera zooms back out |

### Element-Specific Death
| Essence | Visual | Particle Style |
|---------|--------|---------------|
| Cinder | Crumble to ember | Orange-to-red, large, hot |
| Pulse | Overload/explode | Blue-white sparks, electric |
| Bone | Skeleton collapses | Pale chunks, slower/heavier |
| None | Iron shrapnel | Grey-white fragments |

### Particle Budget
- 120 main burst + 24-particle white flash ring + 16-particle colored secondary ring + 20 lingering embers
- High trauma (0.7) for real screen shake
- "FINISH [E]" prompt with pulsing red border appears on eligible enemies

### Design Intent
- Reward aggressive play and mastery of stagger/break system
- Create cinematic punctuation in combat (the "Smash Bros KO screen" moment)
- 2× ether reward incentivizes going for finishers

## Attunement / Reload Concept (2026-04-04, SHELVED — needs more design)

### Concept: Dissonance & Resonance
- Casting builds Dissonance (meter on reticle). Complex combos = more dissonance.
- High dissonance (>75%): spells weaker (damage falloff, visual distortion). NOT a hard stop.
- Press R to Attune (1s channel): ring contracts toward sweet spot.
  - **Perfect:** Full reset + Resonance Burst (3 casts at 1.5× damage, 0 EP, boosted visuals)
  - **Good:** Full reset, no bonus
  - **Miss:** 50% reset + 0.5s casting lockout
- Why better than Gears: optional (soft penalty), reward is exciting (not just "normal"), player-controlled frequency, interacts with combo system.

### Status: Shelved — revisit after Awakening demo is complete.

## Cosmetic Jump (2026-04-04)

### Current Implementation
- Spacebar (when standing still, or when no shield equipped)
- Visual-only: Y offset on sprite, shadow shrinks at peak, landing dust puff (10 particles)
- Parabolic arc: 280 velocity, 700 gravity, ~56px peak height
- No collision changes — purely cosmetic for now
- Coexists with shield dash: Space + movement = dash, Space standing = jump

### Future: Gameplay Jump
- Add collision avoidance (jump over low projectiles, ground hazards)
- Realistic physics (variable jump height based on hold duration)
- Air attacks (slam, plunging attack)
- Double jump upgrade (Roc's Mantle accessory)
