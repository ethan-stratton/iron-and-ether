# Iron & Ether — Backlog (as of 2026-04-04)

## 🔴 Critical — Must Ship in Awakening Demo

### Bugs / Polish
- [ ] Test all latest commits (jump, BREAK finisher, ui-bugfix-batch-3) — Ethan
- [ ] Menu fadeout smoothness — may need more tuning after Ethan tests
- [ ] Cursor visibility — upgraded to 2px outline reticle, awaiting feedback
- [ ] Lightning bolt appearance — replaced with jagged bolt, awaiting feedback
- [ ] Verify bridge passability after duplicate-loop fix

### Missing Gameplay
- [x] Room 6 puzzle chain drawing code — bone cage + electric field visuals ✅ (already implemented)
- [ ] Single-slot combos — commit partial work from subagent (Ward, Quake Stomp, Aegis Burst, Ignition Cloak, Static Field, Exoskeleton)
- [x] Wand of Merlin gameplay behavior ✅ (charge shot fully implemented — hold RMB, scale damage/size/speed)
- [ ] Pellinore's Oath relic effect — currently "trophy relic — no gameplay effect yet" (line 12810)
- [ ] Merchant death handling — uses separate system, not NPC struct

### Distribution
- [ ] Build Mac binaries (osx-arm64 + osx-x64) and zip for Ethan's family

## 🟡 Important — Should Ship in Awakening Demo

### Visual Polish
- [ ] Morien title screen PNG — black outline blocks particles (needs image edit)
- [ ] Add Caliburn's shard to inventory panel (not just HUD)
- [ ] Boss arena more visually distinct from regular rooms

### Design
- [ ] Elemental-shield cycling mini-boss for Awakening
- [ ] Discovery-forcing room design for Gauntlet
- [ ] Creative combo reward system (beyond ether)

### Balance
- [ ] Full playtest pass — all 18 triples, all 21 duals, all 6 single-slots
- [ ] Stagger thresholds per enemy type tuning pass
- [ ] Ether economy balance (income vs. spend rates)

## 🟢 Nice to Have — Awakening Demo

- [ ] Perfect dodge system (Bayonetta Witch Time — dodge within 0.15s of hit → time slow + free counter)
- [ ] Attunement/reload mechanic (shelved — needs more design)
- [ ] Knockback toward cursor (from God of War analysis)
- [ ] "Ether full" nag pulse on HUD
- [ ] First-combo-discovery freeze frame
- [ ] Generous bolt hit detection (near-miss grazing)
- [ ] Reset `_crackedWallBroken` on room re-init if needed
- [ ] Sound effects pass — finisher sound, combo discovery chime, attunement sounds

## 🔵 Campaign Only — NOT for Awakening

### Systems
- [ ] Scrolling camera (decouple arena from screen, camera follow with lerp, room edge transitions)
- [ ] Prone/stealth system (vision cone AI, cover, grass)
- [ ] Double jump (Roc's Mantle accessory)
- [ ] Grapple hook accessory
- [ ] Shade Cloak (invincible dodge upgrade)
- [ ] Movement accessory slot UI in inventory

### Areas
- [ ] **Wyldwood** — first campaign area, Fae forest with repeating screens, scrolling camera debut
- [ ] Overworld hub connecting Awakening → Wyldwood → further areas
- [ ] New Game+ remix (elemental weaknesses shuffled, dungeon locations changed)

### Enemies
- [ ] Per-enemy behavioral identity (one-sentence roles — from DMC5 analysis)
- [ ] Enemy coordination system (optional, emergent flanking preferred)
- [ ] Vision cone AI for stealth-compatible enemies

### Narrative
- [ ] Morgan le Fay encounters (complex antagonist arc)
- [ ] Mordred buildup (pure evil, sequel hook)
- [ ] Oberon & Titania (Wyldwood bosses)
- [ ] Sir Dagonet questline (recurring NPC)
- [ ] Implement prologue crawl polish (gold text, music sync)

### Graphics (from video analysis — MEMORY.md)
- [ ] Pixel-perfect parallax
- [ ] Interleaved Gradient Noise (IGN) for debanding
- [ ] Subtle bloom
- [ ] Depth of field
- [ ] Player-centered vignette
- [ ] LUT color grading per biome

---

_Updated: 2026-04-04 00:10 UTC_
