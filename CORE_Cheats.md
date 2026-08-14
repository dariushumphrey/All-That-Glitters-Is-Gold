# Welcome!

# Resplendent: Cheats

## Contents
* [Overview](#overview)
* Platforms
  * [Default Platform](#default-platform)
  * [Efficient Platform](#efficient-platform)
  * [Chatter Platform](#chatter-platform)
  * [Tempered Platform](#tempered-platform)
  * [Siphonic Platform](#siphonic-platform)
  * [Mining Platform](#mining-platform)
  * [Trenchant Platform](#trenchant-platform)
  * [Cache Platform](#cache-platform)
* Stat Cheats
  * [Yields](#yields)
  * [Stores](#stores)
  * [Sights](#sights)
  * [Hands](#hands)
* Function Cheats
  * ["Wait! Now I'm Ready!"](#wait-now-im-ready)
  * [Efficacy](#efficacy)
  * [Inoculated](#inoculated)
  * [Rude Awakening](#rude-awakening)
  * [Not with a Stick](#not-with-a-stick)
  * [Malicious Wind-Up](#malicious-wind-up)
  * [Positive-Negative](#positive-negative)
  * [Cadence](#cadence)
  * [Good Things Come](#good-things-come)
  * [All Else Fails](#all-else-fails)
  * [The Most Resplendent](#the-most-resplendent)
  * [Fulminate](#fulminate)
  * [Forager](#forager)
  * [Counterplay](#counterplay)
  * [Enshroud](#enshroud)
  * [Gale Force Winds](#gale-force-winds)
  * [Activator Drone](#activator-drone)
  * [Ossify](#ossify)
  * [Bolster](#bolster)
  * [Perfect Record](#perfect-record)
* Exotic Function Cheats
  * [Equivalent Exchange](#equivalent-exchange)
  * [Pay to Win](#pay-to-win)
  * [Superweapon](#superweapon)
  * [Volant](#volant)
  * ["Social Distance, Please!"](#social-distance-please)
  * [Early Berth gets the Hearst](#early-berth-gets-the-hearst)
  * ["Absolutely no Stops!"](#absolutely-no-stops)
  * [Flashpoint](#flashpoint)
  * [Defiance](#defiance)
  * [It Writhes](#it-writhes)

# Overview
This document explains Resplendent's Core system, Cheats, by illustrating what each specific Cheat does passively or through conditions. There are 8 Stat cheats, 30 Function cheats, and 8 Platforms. With respect to readability, direct links to each Cheat's code are provided following their descriptions and function breakdowns. Visuals are only provided for Function cheats and passive-based Platforms. Sections are structured in the order that they are generated in-game.
* Points preceded with "(Fated)" discuss a Function cheat's enhanced strength while at that rarity.

# Platforms
Platforms specifically: 
* Modify a weapon's properties of damage, rate of fire, recoil, aim assist, and camera zoom.
* Enable passive benefits that are active indefinitely, or activate on confirmed surface or enemy hits, confirmed enemy defeats, or certain player actions.

## Default Platform
The Default platform does not modify any weapon property or provide any passive bonus. Weapons with the Default platform operate at base performance.
The [Default Platform](/Assets/Scripts/Weapons/Platform%20Types/DefaultPlatform.cs) can be viewed here.

## Efficient Platform
The Efficient platform: 
* Increases weapon damage by 10%.
* Reduces weapon rate of fire by 40%.
* Reduces weapon recoil by 10%.
* Reduces weapon aim assist by 35%.
* Increases player camera zoom by 20%.

The [Efficient Platform](/Assets/Scripts/Weapons/Platform%20Types/EfficientPlatform.cs) can be viewed here.

## Chatter Platform
The Chatter platform: 
* Reduces weapon damage by 10%.
* Increases weapon rate of fire by 40%.
* Increases weapon recoil by 10%.
* Increases weapon aim assist by 35%.
* Increases player camera zoom by 10%.

The [Chatter Platform](/Assets/Scripts/Weapons/Platform%20Types/ChatterPlatform.cs) can be viewed here.

## Tempered Platform
The Tempered platform: 
* Increases weapon damage by 17.5%.
* Increases weapon rate of fire by 20%.
* Increases weapon aim assist by 50%.
* Increases player camera zoom by 30%.

The [Tempered Platform](/Assets/Scripts/Weapons/Platform%20Types/TemperedPlatform.cs) can be viewed here.

## Siphonic Platform
The Siphonic platform restores a player's health and shield by 1% of their maximum health and shield on confirmed enemy hits. Melee kills by a player restore 15% of their health and shield. The Siphonic platform increases weapon damage by 5%.

The [Siphonic Platform](/Assets/Scripts/Weapons/Platform%20Types/SiphonicPlatform.cs) can be viewed here.

## Mining Platform
The Mining platform creates and detonates a Lucent cluster at the weapon's hit point, granting the weapon "explosive Lucent rounds". The Mining platform increases weapon damage by 5%.

A "Lucent cluster" is a game object that detonates when shot, damaging nearby enemies.

The [Mining Platform](/Assets/Scripts/Weapons/Platform%20Types/MiningPlatform.cs) can be viewed here. The code for [Lucent clusters](/Assets/Scripts/Game/LucentScript.cs) can be viewed here.

## Trenchant Platform
The Trenchant platform applies a health debuff on confirmed enemy hits, increasing an enemy's damage taken. Players that evade near enemies apply a movement debuff called "Slowed", reducing their speed by 50%. Players that melee attack enemies apply a "damage-over-time" debuff, inflicting damage over a duration. The Trenchant platform increases weapon damage by 5%.

* The [Trenchant Platform](/Assets/Scripts/Weapons/Platform%20Types/TrenchantPlatform.cs) can be viewed here.
* The [Health debuff](/Assets/Scripts/Weapons/DebuffScript.cs) code can be viewed here.
* The [Slowed debuff](/Assets/Scripts/Weapons/SlowedScript.cs) code can be viewed here.
* The [Damage-over-time](/Assets/Scripts/Weapons/DamageOverTimeScript.cs) code can be viewed here.

## Cache Platform
The Cache platform regenerates all of the player's grenades every 2 seconds (if a grenade is not already at max count). If a weapon has the Function cheat "Activator Drone", the drone's armament changes into a mini-rocket launcher. The Cache platform increases weapon damage by 5%.

The [Cache Platform](/Assets/Scripts/Weapons/Platform%20Types/CachePlatform.cs) can be viewed here. The [Activator Drone](/Assets/Scripts/Weapons/Added%20Function%20Cheats/ADDrone.cs#L108C21-L131C22)'s attack conversion can be found here.

# Stat Cheats
## Yields
Yield Stat cheats increase a weapon's maximum magazine size by percentages: 
* Deep Yield provides a 12% increase.
* Deeper Yield provides a 24% increase.

For example, a Rarity 1 Burst Fire Rifle has a magazine size of 27.
* Deep Yield increases the size from 27 to 30.
* Deeper Yield increases the size from 27 to 33. 

Deep Yield can be viewed [here.](/Assets/Scripts/Weapons/Magazine%20Cheats/DeepYield.cs)
Deeper Yield can be viewed [here.](/Assets/Scripts/Weapons/Magazine%20Cheats/DeeperYield.cs)

## Stores
Stores Stat cheats increase a weapon's maximum ammo reserves by percentages: 
* Deep Stores provides a 15% increase.
* Deeper Stores provides a 30% increase.

For example, a Rarity 1 Machine Gun has a reserve size of 420. 
* Deep Stores increases the size from 420 to 483.
* Deeper Stores increases the size from 420 to 526. 

Deep Stores can be viewed [here.](/Assets/Scripts/Weapons/Magazine%20Cheats/DeepStores.cs)
Deeper Stores can be viewed [here.](/Assets/Scripts/Weapons/Magazine%20Cheats/DeeperStores.cs)

## Sights
Sights Stat cheats increase a weapon's effective range by a percentage of its total range: 
* Far Sight provides a 10% increase.
* Farther Sight provies a 20% increase.

For example, a Rarity 1 Shotgun has an effective range of 12 meters, and a total range of 20 meters.
* Far Sight takes 10% of 20 and increases the shotgun's effective range from 12 meters to 14 meters.
* Farther Sight takes 20% of 20 and increases the shotgun's effective range from 12 meters to 16 meters.

Far Sight can be viewed [here.](/Assets/Scripts/Weapons/Range%20Cheats/FarSight.cs)
Farther Sight can be viewed [here.](/Assets/Scripts/Weapons/Range%20Cheats/FartherSight.cs)

## Hands
Hands Stat cheats increase a Weapons' Reload Speed by a percentage:
* Hasty Hands provides a 15% increase.
* Hastier Hands provies a 25% increase.

For example, a Rarity 1 Submachine Gun has a reload speed of 1.25 seconds.
* Hasty Hands increases the reload speed from 1.25 seconds to 1.06 seconds.
* Hastier Hands increases the reload speed from 1.25 seconds to 0.94 seconds.

Hasty Hands can be viewed [here.](/Assets/Scripts/Weapons/Reload%20Speed%20Cheats/HastyHands.cs)
Hastier Hands can be viewed [here.](/Assets/Scripts/Weapons/Reload%20Speed%20Cheats/HastierHands.cs)

# Function Cheats
## "Wait! Now I'm Ready!" 
"Wait! Now I'm Ready!" adds 10% of a Player's maximum Shield strength onto their current Shield strength when a Weapon has defeated an Enemy.
* (Fated) "Wait! Now I'm Ready!" increases the percentage of this effect to 20%.

If it detects that it has overhealed a Player's Shield, their current Shield value is assigned to their maximum Shield value. This Cheat does not add onto their Shield if their strength is already at maximum.

* [WaitNowImReady.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/WaitNowImReady.cs) can be viewed here.

![WaitNowImReady](https://github.com/user-attachments/assets/4b694e05-a5be-4b68-a7f6-38d17b3c4e08)

## Efficacy
Efficacy adds 1% of a Weapon's base damage onto its current damage when it confirms a hit on an Enemy. Efficacy restores the weapon's original damage when it reloads. 
* (Fated) Efficacy' damage percent increases to 2%. Reloads no longer restores damage to its starting value, but will allow a Weapon to increase damage up to 125% from its base value. If Efficacy has increased damage beyond the imposed damage cap, the Weapon's damage is assigned to the damage cap.

* [Efficacy.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Efficacy.cs) can be viewed here.

![Efficacy](https://github.com/user-attachments/assets/f2f17867-c787-4c38-9ccf-f88b8c3744cc)

## Inoculated
Inoculated adds 5% of a Player's maximum Health onto their current Health when a Weapon has defeated an Enemy.
* (Fated) Inoculated increases the percentage of this effect to 10%.

If it detects that it has overhealed a Player's Health, their current Health value is assigned to their maximum Health. This Cheat does not add onto their Health if it is full.

* [Inoculated.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Inoculated.cs) can be viewed here.

![Inoculated](https://github.com/user-attachments/assets/72062244-f617-45dd-9c7b-67eb3b06253d)

## Rude Awakening
Rude Awakening grants one use of an area-of-effect (AOE) projection that inflicts 1,000% of a Weapon's damage. Uses are gained when a Weapon defeats an Enemy, and can grow to a stack of three uses.
* (Fated) Rude Awakening increases maximum stacks to six. Enemy defeats grants two uses instead of one, and Weapon damage is increased by 20% while at least one use is held.

If it detects that it has granted a use beyond the maximum allowed, their current use value is assigned to their maximum use limit. 

* [Rude Awakening.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/RudeAwakening.cs) can be viewed here.

![RudeAwakening](https://github.com/user-attachments/assets/e2451f42-3511-4832-a9e8-198c659b2251)

## Not with a Stick
Not with a Stick adds 30% of a Weapon's maximum Range onto their Effective Range when a Weapon has defeated an Enemy. Not with a Stick restores the weapon's original Effective Range when it reloads.
* (Fated) When a Weapon's Effective Range matches their maximum Range, Not with a Stick increases its Aim Assist value by 50%, producing a "lock-on" effect. Reloads no longer restores Effective Range to normal, but this effect remains active for 20 seconds.

* [NotWithAStick.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/NotWithAStick.cs) can be viewed here.

![NotWithAStick](https://github.com/user-attachments/assets/8bd4822a-896b-47c0-a3e1-7e8363e7b469)

## Malicious Wind-Up
Malicious Wind-Up increases a Weapon's Reload Speed by 0.75% when it confirms a hit on an Enemy. Reloads applies the new speed, and restores the original Reload Speed when the effect ends.
* (Fated) Malicious Wind-Up increases its effect strength to 1.5%. Enemy defeats adds 5% of a Weapon's maximum ammunition reserves to its current ammunition reserves. If it detects that it has granted reserve ammunition beyond the maximum allotted, its current reserves are assigned to its maximum reserves.

* [MaliciousWindUp.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/MaliciousWindUp.cs) can be viewed here.

![MaliciousWindUp](https://github.com/user-attachments/assets/fbc91120-a525-4531-82c5-f1ec9675dced)

## Positive-Negative
Positive-Negative generates a charge through movement, up to 100%. Building charge at least halfway applies damage-over-time when a Weapon confirms a hit on an Enemy. Idling rapidly loses charge.
* (Fated) Positive-Negative' damage-over-time strength increases, inflicting 100% more damage and applying it every half-second.

The damage-over-time effect applies 100% of a Weapon's base damage as its own damage once every second, for a ten second duration.

* [PositiveNegative.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Positive-Negative/PositiveNegative.cs) can be viewed here. Its companion script, [PosNegDOT.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Positive-Negative/PosNegDOT.cs) can be viewed here.

![PositiveNegative](https://github.com/user-attachments/assets/0d32d51d-738a-474e-8c7e-37ad791848f6)

## Cadence
Cadence produces an explosive called a "Lucent Cluster" on every third confirmed Enemy defeat by a Weapon.
* (Fated) Cadence now produces Lucent Clusters on every third confirmed Enemy hit.

Lucent Clusters periodically appear during play as both a passive damage mechanic and a contributor to Players' wealth. Destroying clusters adds onto a Player's "Lucent" balance with its full worth, while also inflicting 150% of that worth in damage to nearby Enemies. Clusters can also detonate other clusters, producing a "chain reaction" effect.

* [Cadence.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Cadence.cs) can be viewed here.

![Cadence](https://github.com/user-attachments/assets/5dd232a6-d001-4909-9e57-1ed0c4c1ea10)

## Good Things Come
Good Things Come increases Player and Weapon attributes while being in combat for three seconds: 
* Player Movement Speed increases by 10%.
* Player Damage Resistance increases by 20%.
* Weapon Recoil is reduced by 45%.

Disengaging from combat for five seconds will restore these attributes to their default settings.

* (Fated) Good Things Come increases the strength of its effects and grants one more benefit:
  * Player Movement Speed increases by 20%.
  * Player Damage Resistance increases by 40%
  * Weapon Recoil is reduced by 90%.
  * Weapon gains Infinite Ammunition, preventing consumption of its reserves on reloads.
* Good Things Come' benefits go into effect immediately once combat begins.

Combat is defined as attacking or having been attacked in the past three seconds. If the Player hasn't been damaged or if a Weapon hasn't inflicted damage in five seconds, that is considered being out of combat.

* [GoodThingsCome.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/GoodThingsCome.cs) can be viewed here.

![GoodThingsCome](https://github.com/user-attachments/assets/08e0eb08-2413-4fa7-976d-a46e2668c3e7)

## All Else Fails
All Else Fails permits invulnerability for three seconds upon the full depletion of a Player's Shield. All Else Fails then waits twenty seconds before its effect can be used again. 
* (Fated) All Else Fails' immunity duration increases to five seconds, and its cooldown period is reduced to ten seconds.

All Else Fails produces immunity by taking the full damage meant for a Player and adding the value back to them as Health.

* [AllElseFails.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/AllElseFails.cs) can be viewed here.

![AllElseFails](https://github.com/user-attachments/assets/0a8f66d2-3fda-4835-a646-97a1fb7dcb0e)

## The Most Resplendent
The Most Resplendent grants a use for a "Hard Lucent" crystal that can be attached to surfaces or Enemies. The crystal produces Lucent Clusters passively or when shot by a Weapon for five seconds.
* (Fated) The Most Resplendent' use cap increases to two. Physically colliding with the crystal destroys it, adding 35% of a Player's maximum Health onto their current Health.

Uses are gained through achieving ten confirmed hits on Enemies. Crystals attached to combatants are smaller when compared to their full size when attached to surfaces. Applying 2,000 damage to the crystal or allowing the crystal to expire casts a shockwave that damages Enemies and detonates Lucent Clusters. Shooting the crystal creates a miniature Lucent Cluster at the hit spot, often detonating almost immediately. 

* [TheMostResplendent.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/The%20Most%20Resplendent/TheMostResplendent.cs) can be viewed here. Its companion script, [TMRHardLucentScript.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/The%20Most%20Resplendent/TMRHardLucentScript.cs) can be viewed here.

![TheMostResplendent](https://github.com/user-attachments/assets/7b49b4ab-4651-4641-8f8d-b97e622b1487)

## Fulminate
Fulminate increases damage of the Player's "Destruct Grenade" by 7%, up to a 70% cap, for seven seconds. Achieving a Melee kill casts a free Destruct Grenade.
* (Fated) Fulminate now allows for another Destruct Grenade to be thrown when the first Destruct Grenade is thrown, in addition to its previous effects.

Destruct Grenades are explosive munitions that inflicts 9,000 damage in an 8m radius. After collision with any surface, they detonate after one second. 

It requires 35 confirmed Enemy hits to reach the 70% damage cap. Any Enemy hit during the duration will refresh the timer. Destruct Grenades cast on Melee kills is not limited by cooldowns and does not require an active timer to activate, but Fulminate is required to be active in order to throw double Destruct Grenades. 

* [Fulminate.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Fulminate.cs) can be viewed here. [PlayerMeleeScript.cs](/Assets/Scripts/Player/PlayerMeleeScript.cs#L69-L75) and [PlayerInventoryScript.cs](/Assets/Scripts/Player/PlayerInventoryScript.cs#L1141-L1159) hold the remainder of Fulminate's actions (Grenade casts on Melee kills and damage application, respectively).

![Fulminate](https://github.com/user-attachments/assets/c13632e7-9c54-4a26-8bc8-dde19929ca42)

## Forager
Forager produces a burst of items, called "pickups", on Enemy defeats. This burst possesses ten of the following at random: 
* Health pickups that add 1% of a Player's max Health onto their current Health.
* Shield pickups that add 2% of a Player's max Shield onto their current Shield.
* Ammo pickups that add 15% of a Weapon's max magazine size onto their current magazine.
* Miniature Lucent Clusters that detonate after 0.25 seconds.

Ammo pickups can overflow a Weapon's current magazine up to 150% of its maximum size.

* (Fated) Forager' item burst count increases to 20. Pickup strength becomes stronger, with one additional feature:
  * Health pickup strength increases to 2%.
  * Shield pickup strength increases to 4%.
  * Ammo pickup strength increases to 30%.
  * Every tenth confirmed hit on a non-immune Boss enemy produces a burst.

* [Forager.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Forager/Forager.cs) can be viewed here. [PlayerMeleeScript.cs](/Assets/Scripts/Player/PlayerMeleeScript.cs#L77-L82) handles Forager's burst triggers on Melee defeats.

![Forager](https://github.com/user-attachments/assets/e8507ac7-1645-4063-826e-cc95212b6443)

## Counterplay
Counterplay casts two Lucent Clusters that detonate after 0.25 seconds and permanently increase a Weapon's damage by 10% when a Player is hit during their immunity. This damage effect can stack up to three times.
* (Fated) Counterplay now casts the Player's "Solution Grenade" if they've been hit during their immunity. The damage effect can now stack up to ten times.

Solution Grenades are gaseous munitions that applies significant damage-over-time in a 7m radius, applying 875 damage every 0.25 seconds, for an effect duration of two seconds.

* [Counterplay.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Counterplay.cs) can be viewed here. [ReplevinScript.cs](/Assets/Scripts/Enemies/ReplevinScript.cs#L695-L698) handles the condition that Counterplay requires to activate.

![Counterplay](https://github.com/user-attachments/assets/34471ad6-ac59-4c9c-8323-30936be99dab)

## Enshroud
Enshroud increases Melee attack range by 15%, up to a 200% cap, for seven seconds. Achieving a Melee kill casts a free Fogger Grenade.
* (Fated) Enshroud now passively allows Fogger Grenades from any source to apply damage-over-time. 

Fogger Grenades are smoke munitions that apply a Movement Speed debuff to Enemies in a 10m radius. After collision with any surface, they detonate after two seconds. 

Melee attack range is described as the distance required to initiate a Melee attack. Enshroud caps Melee attack range to 21m. Any Enemy hit during the duration will refresh the timer. Fogger Grenades cast on Melee kills are limited by a 12 second cooldown.
  * (Fated) Fogger Grenade cast on Melee kills' cooldown is reduced to six seconds. Fogger Grenade casts either through Melee kills with Enshroud or throws will allow them to apply damage-over-time, applying 150 damage once every second, for an effect duration of 20 seconds.

* [Enshroud.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Enshroud.cs) can be viewed here. [PlayerMeleeScript.cs](/Assets/Scripts/Player/PlayerMeleeScript.cs#L84-L98) holds the remainder of Enshroud's actions (Fogger Grenade casts on Melee kills).

![Enshroud](https://github.com/user-attachments/assets/f42258a3-93d5-4db5-a91c-ed3d1121aa84)

## Gale Force Winds
Gale Force Winds generates a charge through movement, with improved generation from Sprinting. Having 100% charge grants a use to cast torrential winds that applies Health and Movement Speed debuffs to Enemies.
* (Fated) Gale Force Winds' charge generation rate is doubled. Winds' travel range and speed increases by 50%. Winds can now apply damage-over-time.

Winds are cast by shooting surfaces or Enemies with a full charge. They can follow Enemies that walk through its volume, seeking another target to follow once their tracked enemy has been defeated. If they have no targets, the winds become stationary.
* (Fated) Winds can apply damage-over-time, applying 125 damage once every second, for an effect duration of 20 seconds.

* [GaleForceWinds.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/GaleForceWinds.cs) can be viewed here. Its companion script, [GFWStatusApplicator.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/GFWStatusApplicator.cs), can be viewed here.

![GaleForceWinds](https://github.com/user-attachments/assets/c1a57ef4-09bd-4b97-a8dc-da656a3b8cf4)

## Activator Drone
Activator Drone creates a combat drone that orbits the player and attacks enemies within a 40m radius. Its name comes from its ability to activate a weapon's passive-based Platforms: 
* Siphonic Platform - Confirmed hits increase the player's health and shield.
* Mining Platform - Confirmed hits create and shatter a Lucent cluster at the hit location.
* Trenchant Platform - Confirmed hits apply health debuffs. The drone cannot apply Trenchant's other debuffs.
* Cache Platform - Changes its armament into a mini-rocket launcher.

* (Fated) Activator Drones can trigger a weapon's second Function cheat on confirmed hits or enemy defeats:
  * ["Wait! Now I'm Ready!"](#wait-now-im-ready)
  * [Efficacy](#efficacy)
  * [Inoculated](#inoculated)
  * [Cadence](#cadence)
  * [Enshroud](#enshroud)
  * [Bolster](#bolster)

Manually aiming will use the Activator Drone's laser designator. Aiming at an enemy will assign it as the drone's next target. Otherwise, the drone checks its array of targets and selects one at random to attack. If its target is obstructed, the drone will find another target.
 
* [ActivatorDrone.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/ActivatorDrone.cs) can be viewed here. [ADDrone.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/ADDrone.cs), the code for the physical drone, can be viewed here.

<img width="600" height="337" alt="ActivatorDrone" src="https://github.com/user-attachments/assets/f7be4481-a203-4f30-ad99-388cbba6693b" />

## Ossify
Ossify increases the player's damage resistance by 3% on confirmed hits, up to 30%, for 10 seconds.
* (Fated) Damage resistance now increases by 5% on confirmed hits, up to 50%, for 20 seconds.

* [Ossify.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Ossify.cs) can be viewed here.

<img width="600" height="337" alt="Ossify" src="https://github.com/user-attachments/assets/3c6b1d88-24a2-4c12-bc2f-e30784f669f2" />

## Bolster
Bolster reduces the player's shield recharge delay time by 30% on confirmed enemy defeats, up to 90%. At max reduction, the effect remains enabled for 20 seconds.
* (Fated) At max reduction, the effect becomes indefinite until the weapon is switched.

* [Bolster.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Bolster.cs) can be viewed here.

<img width="600" height="337" alt="Bolster" src="https://github.com/user-attachments/assets/dd2364da-ed0b-4248-af9e-96e6d59f56c8" />

## Perfect Record
Perfect Record increases weapon & melee damage by 40% after 3 seconds without taking damage. When hit, the effect is disabled for 5 seconds.
* (Fated) Weapon and melee damage is increased by 80%. After 3 seconds without damage taken, The effect become indefinite, disabling the timeout effect when hit.

* [PerfectRecord.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/PerfectRecord.cs) can be viewed here.

<img width="600" height="337" alt="PerfectRecord" src="https://github.com/user-attachments/assets/efa12611-e312-4da0-b9c1-5b26ed4e7416" />

# Exotic Function Cheats
## Equivalent Exchange
Equivalent Exchange adds 35% of Enemy damage received directly to the Weapon's damage and the Player's current Health. Weapon damage can permanently increase up to 150% of its original damage.

Equivalent Exchange is assigned to the Full Fire Rifle Weapon type. Its companion Cheat is [Inoculated.](#inoculated)
* [EquivalentExchange.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/EquivalentExchange.cs) can be viewed here.

![EquivalentExchange](https://github.com/user-attachments/assets/1023fa5b-6465-41a8-b7a1-7905211a3f7a)

## Pay to Win
Pay to Win converts 30,000 of the Player's "Lucent" currency into 150 stacks of a 50% Weapon damage increase. 

Stacks are removed three at a time when a Weapon hits an Enemy, and Lucent cannot be converted until all stacks have been removed.

Pay to Win is assigned to the Machine Gun Weapon type. Its companion Cheat is [The Most Resplendent.](#the-most-resplendent)
* [PayToWin.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/PayToWin.cs) can be viewed here.

![PayToWin](https://github.com/user-attachments/assets/59d4a25c-aadd-48dc-bd56-bc71f4f9547d)

## Superweapon
Superweapon grants one stack of 10% damage resistance on confirmed enemy defeats, up to 80% with 8 stacks. Toggling Superweapon enables the ability to charge and fire a high-damage laser while aiming. Standing still significantly increases the charge rate. The laser inflict 1,000% of weapon damage per damage resistance stack, up to 8,000% with 8 stacks. 

Superweapon is assigned to the Pistol Weapon type. Its companion cheat is [Counterplay.](#counterplay)

* [Superweapon.cs](Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/Superweapon.cs) can be viewed here.

<img width="600" height="337" alt="Superweapon" src="https://github.com/user-attachments/assets/627003d7-2664-4d9d-8bee-4b753754f4b7" />

## Volant
Volant activates the player's zero gravity controls, enabling character flight until their shield is broken or if Volant is manually disabled.

Volant is assigned to the Burst Fire Rifle Weapon type. Its companion Cheat is ["Wait! Now I'm Ready!".](#wait-now-im-ready)

* [Volant.cs](Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/Volant.cs) can be viewed here.

<img width="600" height="337" alt="Volant" src="https://github.com/user-attachments/assets/6f9c5c1c-2e09-42e1-afe5-f2ca2c492109" />

## "Social Distance, Please!"
"Social Distance, Please!" applies a 30% damage increase on Enemy hits. Damaged enemies receive a Health debuff that doubles damage taken. Defeated, debuffed Enemies spread 400% of the Weapon's damage in a 10m radius.

The damage increase lasts for ten seconds. Upon expiration, the Weapon's damage is restored to its default value. The timer is extended when a non-debuffed Enemy has a Health debuff applied.

"Social Distance, Please!" is assigned to the Shotgun Weapon type. Its companion Cheat is [Not with a Stick.](#not-with-a-stick)

* [SocialDistancePlease.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/Social%20Distance%2C%20Please!/SocialDistancePlease.cs) can be viewed here. [EnemyHealthScript.cs](/Assets/Scripts/Enemies/EnemyHealthScript.cs#L331-L351) holds the damage spread behavior when an Enemy is defeated.

![SocialDistancePlease](https://github.com/user-attachments/assets/8f908001-ccda-4da4-bf98-e429250e1131)

## Early Berth gets the Hearst
Early Berth gets the Hearst applies and triggers a Berth explosion on every other confirmed Enemy hit.

A "Berth" is described as a status effect unique to Enemies that provides dangerous attack augmentations and induces explosions on defeats. Early Berth gets the Hearst, when the condition is met, applies the Berth condition and immediately triggers the detonation behavior.

Early Berth gets the Hearst is assigned to the Single Fire Rifle Weapon type. Its companion Cheat is [Efficacy.](#efficacy)

* [EarlyBerthGetsTheHearst.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/EarlyBerthGetsTheHearst.cs) can be viewed here.

![EarlyBerthGetsTheHearst](https://github.com/user-attachments/assets/1514e231-ec94-4fa6-b284-bddf9de358d9)

## "Absolutely no Stops!"
"Absolutely no Stops!" increases a Weapon's damage by 200%, Rate of Fire by 50%, and triggers an automatic reload upon expending the magazine. 

This effect remains active until the Weapon has expended all reserve ammunition or if the Player stops firing.

"Absolutely no Stops!" is assigned to the Submachine Gun Weapon type. Its companion Cheat is [Forager.](#forager)

* [AbsolutelyNoStops.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/AbsolutelyNoStops.cs) can be viewed here.

![AbsolutelyNoStops](https://github.com/user-attachments/assets/02fe64d2-ee0d-4f84-9fad-da95ab4e928f)

## Flashpoint
Flashpoint swaps Nebulous At Best's munitions with floating Lucent mines. Mines persist for 1 minute, and all mines are detonated on manual input. Ten mines can be active at a time; additional mines detonates the oldest mine. Switching weapons does not detonate mines, though hits by other weapons can detonate a mine.

Flashpoint is assigned to the Grenade Launcher weapon type. Its companion cheat is [Positive-Negative.](#positive-negative)

* [Flashpoint.cs](Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/Flashpoint.cs) can be viewed here.

<img width="600" height="337" alt="Flashpoint" src="https://github.com/user-attachments/assets/85cfc337-0081-4c0e-a1a7-64d0944d78a0" />

## Defiance
Defiance increases weapon & melee damage by 100% while Deleterious is equipped. Guarding against enemy attacks reflects 1,000% of damage onto the attacker.

Defiance is assigned to the Opening Shot weapon type. Its companion cheat is [All Else Fails.](#all-else-fails)

* [Defiance.cs](Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/Defiance.cs) can be viewed here.

<img width="600" height="337" alt="Defiance" src="https://github.com/user-attachments/assets/70e9a7d8-5a9e-4c17-b088-5d5f8eb647ba" />

## It Writhes
It Writhes swaps Bad Grief's munitions with tandem explosives, detonating twice on surface or enemy hits. Detonations stagger enemies, recoiling them backwards and pausing their actions for 1 second. Enemies staggered by It Writhes have a 50% chance to trigger a Berth explosion (see [Early Berth gets the Hearst](#early-berth-gets-the-hearst) for description on the Berth effect). 

It Writhes is assigned to the Anti-materiel Laser Rifle (AMLR) weapon type. Its companion cheat is [Gale Force Winds.](#gale-force-winds)

* Though known outwardly as "It Writhes", its script name is "Repurposed Form". [RepurposedForm.cs](Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/RepurposedForm.cs) can be viewed here.

<img width="600" height="337" alt="ItWrithes" src="https://github.com/user-attachments/assets/2d93078d-ff2b-4dd4-88ce-eac8197ae525" />
