# The Wyldwood — Area Design Draft

_First campaign area after the Awakening tutorial. Fae forest. Where the scrolling camera debuts._

---

## Concept

The Wyldwood is the western threat from the prologue — a forest that's been creeping east, swallowing villages. It's ruled by Oberon and Titania, the Fae Sovereigns, who believe they're restoring the land to its "true" state (before humans). The trees move when you're not looking. Paths loop. The deeper you go, the less sense direction makes.

**Player motivation:** Morien's father was last seen heading west to investigate the forest's advance. The trail leads here.

**Design mantra:** "The forest doesn't want you here."

---

## Map Structure

### Scrolling Camera
This is the first area to use rooms larger than one screen. Each room scrolls 2-3 screens wide or tall. Camera follows the player with asymptotic averaging (Squirrel Eiserloh method). Clamps to room bounds.

Screen transitions at room edges (same as Awakening), but now rooms are bigger.

### Room Layout (3×3 grid, 9 rooms, some multi-screen)

```
[W1: Eaves]     [W2: Hollow]     [W3: Thorngate]
[W4: Mossdeep]  [W5: Heart]      [W6: Canopy]
[W7: Rootweb]   [W8: Reflecting] [W9: Throne]
```

**W1 — The Eaves (Entry, 2 screens wide)**
- Forest edge. Dead village visible behind you (impassable). Trees ahead grow denser.
- First encounter with Wisps (new enemy). Teaching room.
- NPC: woodcutter's ghost, gives warning + first hint about iron weakness.
- Connects: East to W2, South to W4.

**W2 — The Hollow (2 screens wide)**
- Depression in the forest. Mushroom ring (fairy ring) in center.
- Standing in fairy ring causes screen to shift/warp (palette swap, trees rearrange). Disorienting.
- Puzzle: must place iron (Bone essence projectile) in the ring to dispel the illusion and reveal the real exit.
- Connects: West to W1, East to W3, South to W5.

**W3 — Thorngate (1 screen, vertical)**
- Dense thorn wall with a gate. Guarded by a Thornback (mini-boss).
- Gate opens after Thornback defeated. Shortcut: stays open permanently.
- Connects: West to W2, South to W6.

**W4 — Mossdeep (2 screens tall, vertical scrolling)**
- Deep ravine with moss-covered platforms. Waterfall.
- Vertical scrolling debut. Player descends.
- Bottom: submerged ruin entrance → leads to W7.
- Connects: North to W1, East to W5, South to W7.

**W5 — The Heart (3×2 screens, largest room)**
- Central clearing. Ancient oak so large it's visible from 2 screens away.
- The oak is alive — it's a Fae creature. It watches you (eyes in the bark).
- Hub room: all 4 cardinal exits lead to adjacent rooms.
- First encounter with screen-repeating mechanic: walking south from W5 loops back to W5's north edge (can't leave south until you solve the oak's riddle).
- Connects: All 4 directions.

**W6 — The Canopy (2 screens wide)**
- Elevated. You're walking on branches above the forest floor.
- Vertical depth — enemies below you are visible but can't be hit (depth of field!).
- Grapple point preview: visible anchor points you can't use yet (teaches future grapple mechanic).
- Connects: West to W5, South to W9, North to W3.

**W7 — The Rootweb (2 screens wide)**
- Underground. Massive root system of the ancient oak.
- Roots move slowly — some are walkable platforms, some block paths on a timer.
- Puzzle: time your movement through shifting root corridors.
- Father's campsite found here — cold fire, his journal (lore dump about Morgan's involvement).
- Connects: North to W4, East to W8.

**W8 — The Reflecting Pool (1 screen)**
- Still water pool that shows a different version of the forest (Fae realm overlay).
- Standing at the pool edge lets you see hidden paths in adjacent rooms (map reveal mechanic).
- Nimue connection: she appears in the reflection, gives cryptic guidance about the Sovereigns.
- Connects: West to W7, North to W5, East to W9.

**W9 — The Faerie Throne (2 screens wide, boss room)**
- Oberon and Titania's throne room. Open-air clearing with standing stones.
- Boss fight: alternating phases. Oberon attacks (aggressive, nature magic), Titania shields (defensive, charm spells). Must break Titania's shield before damaging Oberon.
- Phase 2: they merge into a single entity (the Sovereign). Combined moveset.
- Victory: forest stops advancing, paths unloop, fast travel unlocked through Wyldwood.
- Connects: West to W6, South to W8.

---

## New Enemies

| Enemy | Behavior | Weakness | Resist | Size |
|-------|----------|----------|--------|------|
| **Wisp** | Floats in sine-wave, fires homing spark | Bone | Pulse | Small |
| **Spriggan** | Emerges from trees, melee swipe + root grab (holds player 1s) | Cinder | Bone | Medium |
| **Thornback** | Tank, slow, covered in thorns (contact damage). Charges when hit | Pulse | Cinder | Large |
| **Pixie Swarm** | Cluster of 4-6 tiny units acting as one. Scatter when hit, reform | Cinder (AoE) | Bone | Tiny×6 |
| **Will-o'-Wisp** | Lures player toward traps. If followed too long, teleports player back to room start | Bone | All magic | Small |
| **Dryad** | Healer — stands near tree, pulses heal aura for all Fae enemies within radius. Priority target | Cinder | Pulse | Medium |
| **Green Knight** | Elite. Mirror of player — uses its own wand-gun equivalent. Dodges, retaliates, adapts | Player's weakness | Player's essence | Medium |

### The Green Knight (Elite Enemy, possible recurring)
The Green Knight is a Fae warrior who mimics Morien. It uses the same combo system but with Fae essences. It adapts to your loadout:
- If you use Cinder, it equips Pulse
- If you use Pulse, it equips Bone  
- If you use Bone, it equips Cinder
- It counters your Form: if you use Bolt, it dodges laterally. If you use Lance, it strafes.

This forces the player to switch loadouts mid-fight or use unexpected combos. Perfect discovery-forcing enemy.

---

## Puzzles

### Iron Dispels Fae Magic
Core mechanic: Bone essence = iron in the fiction. Fae illusions, barriers, and traps are dispelled by Bone projectiles. This creates tension — Bone is resisted by most Fae enemies (Spriggan, Pixie), but required for environmental puzzles.

### Screen Looping
Some rooms loop — walking off one edge brings you to the opposite edge. The tell is subtle (a specific mushroom arrangement, or the stars shifting). Breaking the loop requires hitting a specific object with the right essence.

### The Oak's Riddle (W5)
The ancient oak speaks in verse. Asks three questions, each requiring the player to demonstrate a specific combo. Wrong answer = enemy wave. Right answer = path south opens.

### Root Timing (W7)
Roots shift every 5 seconds. Some block paths, some create bridges. Player must observe the pattern and time their movement. Cinder can burn roots temporarily (3s window).

---

## Visual Identity

### Palette
- Deep greens, teals, purples. Bioluminescent mushrooms and flowers.
- Fog between trees (use IGN for debanding when we implement shaders).
- Particle: floating pollen, fireflies, drifting leaves.

### LUT
- Cool, desaturated base with warm bioluminescent pops.
- Deeper rooms = more purple shift.
- W9 (throne): golden hour light through canopy.

### Music
- Ambient: layered nature sounds + ethereal vocals (think Ori).
- Combat: percussion enters, vocals intensify.
- Boss: full orchestral Fae anthem.

---

## Narrative Beats

1. **W1:** "Your father came this way. The woodcutter saw him enter. That was three weeks ago."
2. **W5:** The oak knows your father. Offers information for a price (the three riddles).
3. **W7:** Father's journal reveals Morgan warned him about the Wyldwood. He came anyway. "If the forest falls, the rot spreads faster."
4. **W8:** Nimue in the reflecting pool: "The Sovereigns believe they're saving the land. They're not wrong. But their method will kill everyone in it."
5. **W9:** Oberon and Titania. Oberon is arrogant, Titania is sorrowful. She didn't want war. He did. After defeat, Titania gives you information about where your father went next (east, toward Cerdic's territory).

---

## Technical Requirements

- [ ] Scrolling camera system (asymptotic averaging, room-bound clamping)
- [ ] Rooms larger than one screen (decouple `_arena` from `ScreenW/ScreenH`)
- [ ] Screen-edge transitions at room boundaries (not screen boundaries)
- [ ] Screen-loop mechanic (walking off edge → opposite edge)
- [ ] Root platform timing system
- [ ] Fae illusion palette swap effect
- [ ] Healer aura (Dryad) — pulse ring visual + heal tick on nearby enemies
- [ ] Green Knight adaptive AI
- [ ] Reflecting Pool map reveal overlay

---

_Draft: 2026-04-04 00:12 UTC — React, don't design from scratch. Tell me what excites you and what's wrong._
