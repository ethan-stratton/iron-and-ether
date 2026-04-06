# The Awakening — Guided Gameplay Mode

## Room Layout (3×3 Grid)
```
[The Gauntlet]    [The Armory]       [Proving Grounds]
[Knight's Hall]   [The Awakening]    [The Wellspring]
[Lady's Shrine]   [Lava Sanctum]     [Bone Wyrm]
```

## Adjacency
```
[1: Gauntlet]  ←→ [2: Armory]      ←→ [3: Proving Grounds]
     ↕                  ↕                       ↕
[4: Knight's Hall] ←→ [5: Awakening]  ←→ [6: Wellspring]
     ↕                  ↕                       ↕
[7: Lady's Shrine] ←→ [8: Lava Sanctum] ←→ [9: Bone Wyrm]
```

## Room Names (displayed bottom-right)
- Room 1: The Gauntlet
- Room 2: The Armory
- Room 3: The Proving Grounds
- Room 4: The Knight's Hall
- Room 5: The Awakening
- Room 6: The Wellspring
- Room 7: The Lady's Shrine
- Room 8: The Lava Sanctum
- Room 9: The Bone Wyrm

## Door Types
- Open archway (always passable)
- Castle gate (Lance-keyed — shoot with Lance to shatter)
- Mystical barrier (Wave-keyed — shoot with Wave to dispel)
- Frozen gate (Flame-keyed — shoot with Flame to melt)
- Breakable wall (shield-dash to smash through)
- Sealed door (Grimoire-flag — opens when Grimoire obtained)

## Room Details

### Room 5 — The Awakening (START)
- Theme: Stone ruins, dim lighting, beam of light on wand
- Player spawns center. Wand floats on pedestal, glowing bob animation
- Walk into wand → SOTN pickup sequence (flash, ring, particles, gold text)
- All 4 doors open archways — can leave without wand but nothing interactable elsewhere
- Intro text box: "Unlock the Grimoire and find your true power"

### Room 2 — The Armory (FORMS)
- Theme: Castle training grounds, weapon racks, straw on floor
- Three form crystal pedestals: Wave, Beam, Lance
- 4-5 scarecrow dummies: stationary, damage numbers, hit flash, straw particles, respawn 5s
- Text: "1/2/3 to switch forms"
- Door W→Room 1: Castle gate (Lance-keyed)
- Door E→Room 3: Mystical barrier (Wave-keyed)
- Door S→Room 5: Open archway

### Room 1 — The Gauntlet (BEHAVIORS)
- Theme: Dark arena, torchlit, gladiatorial pit
- Entry: Castle gate from Room 2 (Lance) OR open south door from Room 4
- Three behavior crystals: Burst, Shatter, Bloom
- Three enemy waves, each resistant (25% dmg) to wrong behavior:
  - Swarm (tiny fast) → Burst
  - Armored (slow heavy) → Shatter
  - Evasive (teleporting) → Bloom
- Door S→Room 4: Open archway

### Room 4 — The Knight's Hall (SHIELD)
- Theme: Castle great hall, banners, knight NPC
- Knight dialogue → shoot three sigil targets → shield drops
- "Shield Obtained — SPACE to dash"
- Door E→Room 5: Open archway
- Door N→Room 1: Open archway
- Door S→Room 7: Breakable wall (shield-dash)

### Room 6 — The Wellspring (ESSENCES)
- Theme: Crystal cave, glowing pools
- Four essence crystals: Flame, Frost, Pulse, Bone (Bone slightly hidden)
- Collection mini-effects per element
- Door W→Room 5: Open archway
- Door N→Room 3: Frozen gate (Flame-keyed)
- Door S→Room 9: Sealed (Grimoire-flag)

### Room 3 — The Proving Grounds (CHALLENGE / CALIBURN)
- Theme: PORTAL → floating island in stormy sky
- Entry: Wave barrier from Room 2 OR Flame gate from Room 6
- Combat challenge: 3 waves, increasing difficulty
- Clear → Caliburn Shard drops with SOTN effect + money explosion
- Redo sigil for repeatable scaled challenge + big rewards
- Door W→Room 2: open (came through barrier)
- Door S→Room 6: open (came through gate)

### Room 7 — The Lady's Shrine (DUAL COMBOS / NIMUE'S TEAR)
- Theme: Sacred lake grotto, still water, shrine
- Entry: Breakable wall from Room 4 (shield-dash)
- Shrine quest: fire any Dual Combo at target
- Nimue's Tear rises from chalice
- "Two become one. Imagine what three could do..."
- Door N→Room 4: open
- Door E→Room 8: open archway

### Room 8 — The Lava Sanctum (GRIMOIRE)
- Theme: Volcanic chamber, lava moat, embers
- Grimoire on island, lava moat surrounds it
- Puzzle: Frost shots freeze lava into obsidian platforms
- THE REVEAL: blackout → slots light up → fusion name → explosion
- Door N→Room 5: open archway
- Door W→Room 7: open archway
- Door E→Room 9: sealed (opens on Grimoire pickup)

### Room 9 — The Bone Wyrm (BOSS)
- Theme: Massive cavern, bones in walls, red ambient
- Entry: sealed from Room 8 + sealed from Room 6 (both Grimoire-gated)
- Bone Wyrm boss: charge, flame breath, frost ground, bone armor, final phase
- Dragon Slayer = intended power fantasy
- Victory: Haste Band + Phylactery drop, end screen

## Flow — No Dead Ends
- Start: can walk to 2,4,6,8 freely. Nothing works without wand.
- Two paths to Room 1: Lance gate (Room 2) or open south (Room 4)
- Two paths to Room 3: Wave barrier (Room 2) or Flame gate (Room 6)
- Room 7: shield-dash from Room 4
- Room 8: enter freely, need Frost for puzzle
- Room 9: need Grimoire

## Master Task List
### Systems (build first)
1. [ ] Text box / dialogue system
2. [ ] Item pickup entity (floating, glow, walk-over)
3. [ ] SOTN pickup effect (flash, ring, particles, rising text)
4. [ ] Door entity system (states + barrier types + keys)
5. [ ] NPC entity (stationary, proximity dialogue)
6. [ ] "The Awakening" game mode + mode select
7. [ ] Room-specific content spawning per mode
8. [ ] Room name display (bottom-right corner)

### Per-Room
9. [ ] Room 5: wand pedestal, light beam, pickup trigger
10. [ ] Room 2: form crystals, scarecrows, damage numbers, straw particles
11. [ ] Room 4: knight NPC, sigil targets, shield pickup
12. [ ] Room 6: essence crystals (4), collection effects
13. [ ] Room 1: behavior crystals, enemy variants, resistance system
14. [ ] Room 3: portal, floating island, challenge waves, redo, scaling
15. [ ] Room 7: shrine, sacred water, Lady dialogue, combo target
16. [ ] Room 8: lava tiles, frost-obsidian mechanic, Grimoire reveal
17. [ ] Room 9: Bone Wyrm boss, phases, elemental weakness, victory
