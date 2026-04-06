# Combo Design Notes

Design ideas, inspirations, and implementation status for dual combos and spell fusions.
Parsed by the Forbidden Laboratory API for display.

## Dual Combos — Form + Behavior

### Ricochet (Bolt + Bounce)
- **Inspiration:** Gungeon bouncy bullets
- **Design:** Bolts gain +20% speed on each bounce, glow brighter with each hit. Room geometry play.
- **Status:** Named only

### Splinter Shot (Bolt + Shatter)
- **Design:** Bolt splits into 3 fragments on hit
- **Status:** Named only

### Signal Flare (Bolt + Linger)
- **Design:** Bolt leaves fire trail, illuminates area
- **Status:** Named only

### Comet (Bolt + Bloom)
- **Inspiration:** FF7 Comet materia
- **Design:** Bolt grows in size as it travels, explodes big on impact
- **Status:** Named only

### Freeze Ray (Bolt + Stasis)
- **Design:** Bolt leaves freeze trail
- **Status:** Named only

### Hex Bolt (Bolt + Possess)
- **Inspiration:** Castlevania curse debuffs
- **Design:** Bolt marks enemy, next hit deals 2× damage
- **Status:** Named only

### Firebomb (Shotgun + Linger)
- **Inspiration:** Molotov cocktails from every game ever
- **Design:** Shotgun burst leaves fire puddle on impact
- **Status:** Named only

### Buckshot (Shotgun + Bounce)
- **Inspiration:** Enter the Gungeon shotgun + bouncy
- **Design:** Pellets bounce once off walls, tighter spread. Room-clearing in corridors
- **Status:** Named only

### Cluster Bomb (Shotgun + Shatter)
- **Inspiration:** Worms / Gungeon cluster munitions
- **Design:** Each pellet splits on hit. Screen full of projectiles
- **Status:** Named only

### Undertow (Wave + Linger)
- **Design:** Wave leaves persistent zone. Could rain projectiles downward in the zone like a waterfall
- **Status:** Named only

### Echo (Wave + Bounce)
- **Design:** Bouncing sine waves that persist longer
- **Status:** Named only

### Tsunami (Wave + Bloom)
- **Design:** Wave grows huge, massive AoE
- **Status:** Named only

### Pinball (Orbit + Bounce)
- **Design:** Orbiting projectiles bounce off walls when they expire
- **Status:** Named only

### Supernova (Orbit + Bloom)
- **Design:** Orbiting projectiles grow, explode when they die
- **Status:** Named only

### Stasis Field (Orbit + Stasis)
- **Design:** Orbiting projectiles create freeze zones
- **Status:** Named only

### Ball Lightning (Chain + Bounce)
- **Inspiration:** Diablo 2 Ball Lightning
- **Design:** Slow-moving orb that bounces off walls, zapping nearby enemies as it travels
- **Status:** Named only

### Fork Lightning (Chain + Shatter)
- **Design:** Chain splits at each jump, hitting more targets
- **Status:** Named only

### Landmine (Seed + Bloom)
- **Inspiration:** Brotato / Vampire Survivors mines
- **Design:** Seed grows to size, then POPS on proximity or timer. Satisfying delayed explosion
- **Status:** Named only

### Claymore (Seed + Shatter)
- **Design:** Seed explodes into directional fragments
- **Status:** Named only

### Pop Green (Seed + Bounce)
- **Inspiration:** One Piece — Usopp's Pop Greens
- **Design:** Seed bounces along ground before detonating, unpredictable trajectory
- **Status:** Named only

### Frost Bulb (Seed + Stasis)
- **Inspiration:** Elden Ring frost pots
- **Design:** Seed detonates into freeze AoE
- **Status:** Named only

### Cordyceps (Seed + Possess)
- **Inspiration:** The Last of Us
- **Design:** Seed lands on enemy, possesses them. Thematic body-horror
- **Status:** Named only

### Creeping Moss (Seed + Linger)
- **Design:** Growing linger zone (20→80px over 12s), slows enemies inside. Organic mossy patch visuals with spore particles
- **Status:** ✅ Implemented (commit 5f01c01)

### Buzzsaw (Saw + Bounce)
- **Inspiration:** Terraria boomerangs
- **Design:** Saw bounces faster, persists longer. Room becomes a blender
- **Status:** Named only

### Fragmentation (Nova + Shatter)
- **Inspiration:** Real fragmentation grenades
- **Design:** Nova ring shatters into outward bolts at max radius. Melee → ranged transition
- **Status:** Named only

### Flash Freeze (Nova + Stasis)
- **Inspiration:** Mega Man ice weapons
- **Design:** Nova burst freezes everything in range. Hard CC
- **Status:** Named only

### Death Ray (Lance + Bloom)
- **Design:** Beam grows wider over time
- **Status:** Named only

### Scorched Earth (Lance + Linger)
- **Design:** Beam leaves fire zones along its path
- **Status:** Named only

### Trick Shot (Volley + Bounce)
- **Design:** Volley rounds ricochet off walls
- **Status:** Named only

### Barrage (Volley + Shatter)
- **Design:** Each volley round splits on impact
- **Status:** Named only

## Dual Combos — Form + Essence

### Fire Bolt (Bolt + Cinder)
- **Design:** Bolt with flame trail, ignite on hit
- **Status:** Named only

### Shock Bolt (Bolt + Pulse)
- **Design:** Bolt with lightning arc, stagger on hit
- **Status:** Named only

### Shadow Bolt (Bolt + Dark)
- **Design:** Bolt with shadow trail, fear on hit
- **Status:** Named only

### Thorn Bolt (Bolt + Sap)
- **Design:** Bolt with vine particles, leech on hit
- **Status:** Named only

### Dragon Spit (Shotgun + Cinder)
- **Design:** Fire shotgun, leaves small burn patches
- **Status:** Named only

### Bat Burst (Shotgun + Dark)
- **Design:** Pellets are bat-shaped, fear on hit
- **Status:** Named only

### Phantom Wave (Wave + Hollow)
- **Design:** Ghost-like wave, silences enemies
- **Status:** Named only

### Vine Whip (Wave + Sap)
- **Design:** Green wave, leech on hit
- **Status:** Named only

### Ring of Fire (Orbit + Cinder)
- **Design:** Fire orbs orbit, leave flame trails
- **Status:** Named only

### Dark Halo (Orbit + Dark)
- **Design:** Shadow orbs orbit, fear aura
- **Status:** Named only

### Arc Lightning (Chain + Pulse)
- **Design:** Chain with visible lightning arcs between jumps
- **Status:** Named only

### Chain Fire (Chain + Cinder)
- **Design:** Chain sets each enemy on fire, fire spreads
- **Status:** Named only

### Fire Trap (Seed + Cinder)
- **Potential rework:** Meteor — bolt with huge parabolic arc, shadow grows underneath, fire explosion on landing
- **Status:** Named only

### Spore Mine (Seed + Sap)
- **Design:** Seed heals player when enemies die in its zone
- **Status:** Named only

### Nightshade (Seed + Dark)
- **Design:** Dark seed, fear burst on detonation
- **Status:** Named only

### Shock Root (Seed + Pulse)
- **Design:** Lightning seed, chain stagger on detonation
- **Status:** Named only

### Void Sprout (Seed + Hollow)
- **Design:** Void seed, silence AoE on detonation
- **Status:** Named only

### Bone Garden (Seed + Bone)
- **Design:** Bone spikes erupt from ground
- **Status:** Named only

### Bone Saw (Saw + Bone)
- **Design:** Saw leaves bone shrapnel trail
- **Status:** Named only

### Flame Disc (Saw + Cinder)
- **Design:** Flaming buzzsaw, ignites on contact
- **Status:** Named only

### Hellfire (Nova + Cinder)
- **Design:** Fire nova, ignite everything in range
- **Status:** Named only

### Eclipse (Nova + Dark)
- **Design:** Dark nova, screen darkens briefly, fear everything
- **Status:** Named only

### Sun Lance (Lance + Cinder)
- **Design:** Fire beam, scorch visual
- **Status:** Named only

### Void Beam (Lance + Dark)
- **Design:** Dark beam, fear along its path
- **Status:** Named only

### Storm Volley (Volley + Pulse)
- **Design:** Seeking volley rounds with exhaust trails, lightning arc visual between rounds
- **Status:** Named only

### Bone Volley (Volley + Bone)
- **Design:** Bone projectile burst, crit vulnerability
- **Status:** Named only

## Dual Combos — Behavior + Essence

### Bone Shrapnel (Shatter + Bone)
- **Design:** Fragments are bone-white, crit vulnerability
- **Status:** Named only

### Fireworks (Shatter + Cinder)
- **Design:** Colorful fire fragments, festive explosion
- **Status:** Named only

### Bonfire (Linger + Cinder)
- **Design:** Fire zone burns longer and hotter
- **Status:** Named only

### Swamp (Linger + Sap)
- **Design:** Long-lasting leech zone, heavy slow
- **Status:** Named only

### Tesla Ball (Bounce + Pulse)
- **Design:** Bouncing lightning orb, arcs to nearby enemies on each bounce
- **Status:** Named only

### Bone Bounce (Bounce + Bone)
- **Design:** Bouncing bone projectiles, crit on bounce hits
- **Status:** Named only

### Overgrowth (Bloom + Sap)
- **Design:** Growing projectile that heals player on enemy kills
- **Status:** Named only

### Shadow Bloom (Bloom + Dark)
- **Design:** Growing dark projectile, fear burst on bloom
- **Status:** Named only

### Time Stop (Stasis + Hollow)
- **Design:** Hard freeze + silence combo, longest CC duration
- **Status:** Named only

### Dark Pact (Possess + Dark)
- **Design:** Possessed enemies deal shadow damage, stronger possession
- **Status:** Named only

## Spell Fusions — New / Planned

### Brahmastra (Volley + Bloom + Cinder)
- **Inspiration:** Hindu divine weapon that destroys everything in its path
- **Design:** Volley rockets converge → rotating red reticle on ground (1.5s charge) → massive screen-filling nuke explosion. Max trauma, long hitstop, kills everything in blast radius. The "oh shit" button.
- **Status:** Planned

### Meteor (Bolt + Cinder — Fire Bolt rework?)
- **Design:** Bolt with huge parabolic arc, shadow grows underneath as it falls, massive fire explosion on landing. Visual spectacle.
- **Status:** Idea stage
