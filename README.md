# Iron & Ether

A top-down action game set in alternate-reality Camelot. You're a knight with a magic wand that evolves based on enemy essences you collect. Three combinable slots (Form × Behavior × Essence) produce hundreds of weapon variations, and specific combos trigger **Spell Fusions** — curated, spectacular attacks with unique visuals.

*Zelda 1 room structure meets Aria of Sorrow soul acquisition meets HoD Spell Fusion spectacle.*

## How to Run

**Requirements:** [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

```bash
git clone https://github.com/ethan-stratton/iron-and-ether.git
cd iron-and-ether/IronAndEther
dotnet run
```

## Controls

| Action | Input |
|--------|-------|
| Move | **WASD** |
| Aim | **Mouse** (cursor is a reticle) |
| Shoot | **Left Click** (hold for continuous fire) |
| Charge Attack | **Right Click** (hold & release) |
| Dash | **Space** (requires Shield relic) |
| Reload | **R** |
| Quick-swap Loadout | **Q** (requires Quickswap relic) |
| Swap Wand Slots | **1 / 2 / 3** |
| Interact / Talk | **E** |
| Open Inventory | **Tab** |
| Pause | **Escape** |

## The Wand System

Your wand has three slots you fill with components dropped by enemies:

- **Form** — How it fires (Bolt, Shotgun, Wave, Chain, Orbit, Seed, Saw, Nova, Lance, Volley)
- **Behavior** — What happens on contact (Shatter, Linger, Bounce, Bloom, Possess, Stasis)
- **Essence** — Elemental nature with visual transformation (Fire, Frost, Volt, Venom, Shadow, Radiant, Void)

Empty slots use safe defaults. Mix and match — there are **343 possible combinations**, and specific 3-slot combos unlock named **Spell Fusions** with unique effects.

Kill enemies to extract their components (Aria of Sorrow style — not random drops).

## Inventory

Press **Tab** to open your sack. Drag components with **Left Click**, discard with **Right Click**. Equip components to your three wand slots from here.

## Gameplay Loop

1. Enter a room — enemies spawn
2. Kill enemies — they drop wand components (Form, Behavior, or Essence)
3. Equip new components to experiment with combos
4. Discover Spell Fusions for powerful named attacks
5. Explore rooms, talk to NPCs, find relics that grant permanent upgrades
6. Get stronger, go deeper

## Project Structure

```
IronAndEther/
├── Game1.cs          # Main game (yes, it's one big file — it's a prototype)
├── Enemy.cs          # Enemy types and behavior
├── Projectile.cs     # Projectile system
├── Components.cs     # Form, Behavior, Essence definitions
├── Content/          # Sprites, room data, fonts
└── Program.cs        # Entry point
```

## Design Docs

- **[GAME-DESIGN.md](GAME-DESIGN.md)** — Full game design document (wand system, enemies, relics, fusions)
- **[SHMUP-GDD.md](SHMUP-GDD.md)** — Alternate shmup mode design

## Tech

- **Engine:** MonoGame (C# / .NET 9)
- **Rendering:** All fallback/procedural — no sprite sheet required to run
- **Platform:** Windows, macOS, Linux

## Status

Playable prototype. Core wand system works, multiple rooms explorable, NPCs with dialogue, several enemy types, inventory system, spell fusions discoverable. Art is programmer-art placeholder.

---

*Built by Ethan & friends. Design doc has the full vision.*
