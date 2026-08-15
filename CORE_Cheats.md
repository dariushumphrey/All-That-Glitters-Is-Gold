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

<img width="600" height="337" alt="SiphonicPlatform" src="https://github.com/user-attachments/assets/e081a3e1-0b6d-49fa-80c5-ba835c211b27" />

## Mining Platform
The Mining platform creates and detonates a Lucent Cluster at the weapon's hit point, granting the weapon "explosive Lucent rounds". The Mining platform increases weapon damage by 5%.

A Lucent Cluster is a game object that detonates when shot, damaging nearby enemies (see [Cadence](#cadence) for description on Lucent Clusters). 


The [Mining Platform](/Assets/Scripts/Weapons/Platform%20Types/MiningPlatform.cs) can be viewed here. The code for [Lucent clusters](/Assets/Scripts/Game/LucentScript.cs) can be viewed here.

<img width="600" height="337" alt="MiningPlatform" src="https://github.com/user-attachments/assets/8a9ad078-2df7-4a84-9c38-2e4ae671b047" />

## Trenchant Platform
The Trenchant platform applies a health debuff on confirmed enemy hits, increasing an enemy's damage taken. Players that evade near enemies apply a movement debuff called "Slowed", reducing their speed by 50%. Players that melee attack enemies apply a "damage-over-time" debuff, inflicting damage over a duration. The Trenchant platform increases weapon damage by 5%.

* The [Trenchant Platform](/Assets/Scripts/Weapons/Platform%20Types/TrenchantPlatform.cs) can be viewed here.
* The [Health debuff](/Assets/Scripts/Weapons/DebuffScript.cs) code can be viewed here.
* The [Slowed debuff](/Assets/Scripts/Weapons/SlowedScript.cs) code can be viewed here.
* The [Damage-over-time](/Assets/Scripts/Weapons/DamageOverTimeScript.cs) code can be viewed here.

<img width="600" height="337" alt="TrenchantPlatform" src="https://github.com/user-attachments/assets/cd39b0e2-c1e3-4ff0-bdc6-e533e7a6a735" />

## Cache Platform
The Cache platform regenerates all of the player's grenades every 2 seconds (if a grenade is not already at max count). If a weapon has the Function cheat "Activator Drone", the drone's armament changes into a mini-rocket launcher. The Cache platform increases weapon damage by 5%.

The [Cache Platform](/Assets/Scripts/Weapons/Platform%20Types/CachePlatform.cs) can be viewed here. The [Activator Drone](/Assets/Scripts/Weapons/Added%20Function%20Cheats/ADDrone.cs#L108C21-L131C22)'s attack conversion can be found here.

<img width="600" height="337" alt="CachePlatform" src="https://github.com/user-attachments/assets/79a495e5-c0df-4b37-9213-0c35696f77bd" />

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
"Wait! Now I'm Ready!" adds 10% of a player's maximum shield onto their current shield on confirmed enemy defeats. Additional shield is not added if the player's shield is full.
* (Fated) "Wait! Now I'm Ready!" adds 20% of a player's maximum shield onto their current shield on confirmed enemy defeats.

* [WaitNowImReady.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/WaitNowImReady.cs) can be viewed here.

![WaitNowImReady](https://github.com/user-attachments/assets/4b694e05-a5be-4b68-a7f6-38d17b3c4e08)

## Efficacy
Efficacy adds 1% of a weapon's base damage onto its current damage on confirmed enemy hits. Efficacy restores the weapon's original damage when it reloads. 
* (Fated) Efficacy adds 2% of a weapon's base damage onto its current damage on confirmed enemy hits. Reloads no longer restores damage to its starting value, but weapon damage can only increase up to 125% from its base value.

* [Efficacy.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Efficacy.cs) can be viewed here.

![Efficacy](https://github.com/user-attachments/assets/f2f17867-c787-4c38-9ccf-f88b8c3744cc)

## Inoculated
Inoculated adds 5% of a player's maximum health onto their current health on confirmed enemy defeats. Additional health is not added if the player's health is full.
* (Fated) Inoculated adds 10% of a player's maximum health onto their current health on confirmed enemy defeats.

* [Inoculated.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Inoculated.cs) can be viewed here.

![Inoculated](https://github.com/user-attachments/assets/72062244-f617-45dd-9c7b-67eb3b06253d)

## Rude Awakening
Rude Awakening grants one stack of an area-of-effect (AOE) attack that inflicts 1,000% of a weapon's damage on confirmed enemy defeats. Rude Awakening can stack up to 3 uses. 
* (Fated) Rude Awakening increases maximum AOE stacks to 6. Enemy defeats grants two stacks instead of one, and weapon damage is increased by 20% while at least one stack is held.

* [Rude Awakening.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/RudeAwakening.cs) can be viewed here.

![RudeAwakening](https://github.com/user-attachments/assets/e2451f42-3511-4832-a9e8-198c659b2251)

## Not with a Stick
Not with a Stick adds 30% of a weapon's maximum range onto its current effective range on confirmed enemy defeats. Not with a Stick restores the weapon's original effective range when it reloads.
* (Fated) When a weapon's effective range matches their maximum range, Not with a Stick increases the weapon's aim assist value by 50%. Reloads no longer restores effective range to normal, but this effect remains active for 20 seconds.

* [NotWithAStick.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/NotWithAStick.cs) can be viewed here.

![NotWithAStick](https://github.com/user-attachments/assets/8bd4822a-896b-47c0-a3e1-7e8363e7b469)

## Malicious Wind-Up
Malicious Wind-Up increases a weapon's reload speed by 0.75% on confirmed enemy hits. Reloads apply the new speed, and restores the original reload speed when the effect ends.
* (Fated) Malicious Wind-Up increases a weapon's reload speed by 1.5% on confirmed enemy hits. Enemy defeats adds 5% of a weapon's maximum ammo reserves to its current ammo reserves. Additional ammo is not added if its reserves are full.

* [MaliciousWindUp.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/MaliciousWindUp.cs) can be viewed here.

![MaliciousWindUp](https://github.com/user-attachments/assets/fbc91120-a525-4531-82c5-f1ec9675dced)

## Positive-Negative
Positive-Negative generates a charge through movement, up to 100%. When the charge is halfway full, Positive-Negative applies damage-over-time on confirmed enemy hits. Idling rapidly loses charge.
* (Fated) Positive-Negative's damage-over-time strength increases, applying 100% more damage every half-second.

The damage-over-time effect uses 100% of a weapon's base damage as its own damage once every second for a 10 second duration. Positive-Negative applies its own specific damage-over-time effect, allowing it to stack alongside other damage-over-time sources like from the Trenchant Platform or the player's Solution Grenades.

* [PositiveNegative.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Positive-Negative/PositiveNegative.cs) can be viewed here. Its companion script, [PosNegDOT.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Positive-Negative/PosNegDOT.cs) can be viewed here.

![PositiveNegative](https://github.com/user-attachments/assets/0d32d51d-738a-474e-8c7e-37ad791848f6)

## Cadence
Cadence produces an explosive called a Lucent Cluster on every third confirmed enemy defeat.
* (Fated) Cadence now produces Lucent Clusters on every third confirmed enemy hit.

Lucent Clusters are passive damage mechanics that inflicts damage to nearby enemies when destroyed, appearing occasionally on confirmed enemy defeats. Lucent Clusters can also detonate other clusters, producing a "chain reaction" effect.

* [Cadence.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Cadence.cs) can be viewed here.

![Cadence](https://github.com/user-attachments/assets/5dd232a6-d001-4909-9e57-1ed0c4c1ea10)

## Good Things Come
Good Things Come increases player and weapon attributes while being in combat for three seconds: 
* Player movement speed increases by 10%.
* Player damage resistance increases by 20%.
* Weapon recoil is reduced by 45%.

Disengaging from combat for five seconds will restore these attributes to their default states.

* (Fated) Good Things Come increases the strength of its effects and grants one more benefit:
  * Player movement speed increases by 20%.
  * Player damage resistance increases by 40%
  * Weapon recoil is reduced by 90%.
  * Weapon gains Infinite Ammunition, preventing consumption of its reserves on reloads.
* Good Things Come triggers immediately once combat begins.

Combat is defined as attacking or having been attacked in the past 3 seconds. If the player hasn't been damaged or if a weapon hasn't inflicted damage in 5 seconds, Good Things Come determines that combat has been exited.

* [GoodThingsCome.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/GoodThingsCome.cs) can be viewed here.

![GoodThingsCome](https://github.com/user-attachments/assets/08e0eb08-2413-4fa7-976d-a46e2668c3e7)

## All Else Fails
All Else Fails grants invulnerability for five seconds when the player's shield is depleted. All Else Fails then enters a 10 second cooldown before the effect can be used again. 
* (Fated) All Else Fails can be activated again immediately after the effect expires.

* [AllElseFails.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/AllElseFails.cs) can be viewed here.

![AllElseFails](https://github.com/user-attachments/assets/0a8f66d2-3fda-4835-a646-97a1fb7dcb0e)

## The Most Resplendent
The Most Resplendent grants one stack to cast a Hard Lucent crystal that can be attached to surfaces or enemies. The crystal produces Lucent Clusters passively or when shot by a weapon for 5 seconds.
* (Fated) The Most Resplendent's stack cap increases to 2. Physically colliding with the crystal destroys it, adding 35% of the player's maximum health onto their current health.

One stack is gained after ten confirmed enemy hits. Crystals attached to combatants are smaller. Inflicting 2,000 damage to the crystal or allowing the crystal to expire casts a shockwave that damages enemies and detonates Lucent Clusters. Shooting the crystal creates a miniature Lucent Cluster at the hit spot.

(see [Cadence](#cadence) for description on Lucent Clusters).

* [TheMostResplendent.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/The%20Most%20Resplendent/TheMostResplendent.cs) can be viewed here. Its companion script, [TMRHardLucentScript.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/The%20Most%20Resplendent/TMRHardLucentScript.cs) can be viewed here.

![TheMostResplendent](https://github.com/user-attachments/assets/7b49b4ab-4651-4641-8f8d-b97e622b1487)

## Fulminate
Fulminate increases damage of the player's Destruct Grenade by 7%, up to a 70%, for 7 seconds. Any confirmed enemy defeat with a melee attack creates and detonates a Destruct Grenade.
* (Fated) While active, after throwing a Destruct Grenade, Fulminate throws a delayed Destruct Grenade at no cost.

Destruct Grenades are explosive munitions that inflicts 9,000 damage in an 8m radius. After collision with any surface, they detonate after one second. 

Fulminates requires 35 confirmed enemy hits to reach the 70% damage cap. Any confirmed enemy hits will refresh the timer while active.

* [Fulminate.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Fulminate.cs) can be viewed here. [PlayerMeleeScript.cs](/Assets/Scripts/Player/PlayerMeleeScript.cs#L69-L75) and [PlayerInventoryScript.cs](/Assets/Scripts/Player/PlayerInventoryScript.cs#L1141-L1159) hold the remainder of Fulminate's actions (Grenade casts on Melee kills and damage application, respectively).

![Fulminate](https://github.com/user-attachments/assets/c13632e7-9c54-4a26-8bc8-dde19929ca42)

## Forager
Forager produces a burst of pickup items on confirmed enemy defeats. The burst possesses ten of the following at random: 
* Health pickups that add 1% of a player's max health onto their current health.
* Shield pickups that add 2% of a player's max shield onto their current shield.
* Ammo pickups that add 15% of a weapon's max magazine size onto their current magazine.
* Miniature Lucent Clusters that detonate after 0.25 seconds (see [Cadence](#cadence) for description on Lucent Clusters).

Forager's ammo pickups can overflow a weapon's current ammo count beyond its maximum magazine size, up to 150%. For example, a Rarity 3 Full Fire Rifle has a maximum magazine size of 28: 
 * Forager's ammo pickups add ammunition up to an overflowed max magazine size of 42.

* (Fated) Forager's pickup burst count increases to 20. Pickup strength becomes stronger, and Forager gains an additional feature:
  * Health pickup strength increases to 2%.
  * Shield pickup strength increases to 4%.
  * Ammo pickup strength increases to 30%.
  * Every tenth confirmed hit on a non-immune Boss enemy produces a burst.

* [Forager.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Forager/Forager.cs) can be viewed here. [PlayerMeleeScript.cs](/Assets/Scripts/Player/PlayerMeleeScript.cs#L77-L82) handles Forager's burst triggers on Melee defeats.

![Forager](https://github.com/user-attachments/assets/e8507ac7-1645-4063-826e-cc95212b6443)

## Counterplay
Counterplay casts two Lucent Clusters that detonate after 0.25 seconds when a player is hit while immune or while guarding with an Opening Shot. When triggered, Counterplay grants one stack of a 10% weapon damage increase, up to 3 stacks for a 30% damage increase.
* (Fated) When triggered, Counterplay now casts and detonates the player's Solution Grenade. Counterplay's max stack count is increased to 10.

Solution Grenades are toxic munitions that applies damage-over-time in a 7m radius, inflicting 875 damage every 0.25 seconds. After collision with any surface, they detonate after one second. The grenade's damage-over-time effect lasts for 2 seconds.

(see [Cadence](#cadence) for description on Lucent Clusters).

* [Counterplay.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Counterplay.cs) can be viewed here. [ReplevinScript.cs](/Assets/Scripts/Enemies/ReplevinScript.cs#L695-L698) handles the condition that Counterplay requires to activate.

![Counterplay](https://github.com/user-attachments/assets/34471ad6-ac59-4c9c-8323-30936be99dab)

## Enshroud
Enshroud increases the player's melee attack range by 15%, up to 200%, for 7 seconds. Any confirmed enemy defeat with a melee attack casts a Fogger Grenade, entering a 12 second cooldown when triggered.
* (Fated) Enshroud now enables Fogger Grenades from any source to apply damage-over-time, inflicting 150 damage once every second for 20 seconds. Enshroud reduces the cooldown of Fogger Grenades on melee defeats to 6 seconds. 

Fogger Grenades are gaseous munitions that apply the debuff "Slowed" to enemies in a 10m radius, reducing an enemy's movement speed by 50%. After collision with any surface, they trigger after two seconds. 

Melee attack range is defined as the distance required to initiate a melee attack. Enshroud caps the player's melee attack range to 21m. 

* [Enshroud.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Enshroud.cs) can be viewed here. [PlayerMeleeScript.cs](/Assets/Scripts/Player/PlayerMeleeScript.cs#L84-L98) holds the remainder of Enshroud's actions (Fogger Grenade casts on Melee kills).

![Enshroud](https://github.com/user-attachments/assets/f42258a3-93d5-4db5-a91c-ed3d1121aa84)

## Gale Force Winds
Gale Force Winds generates a charge through movement, with improved generation from Sprinting. Having 100% charge grants a cast for torrential winds that applies health and movement speed debuffs to enemies.
* (Fated) Gale Force Winds' charge generation rate is doubled. Winds can now apply damage-over-time, and their detection range and speed increases by 50%.

Winds are cast by shooting surfaces or enemies when Gale Force Winds is toggled. Any enemy that walks through its volume (if it is without a target) is tracked. The winds become stationary when without a target.
* (Fated) Winds can apply damage-over-time, applying 125 damage once every second, for an effect duration of 20 seconds.

* [GaleForceWinds.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/GaleForceWinds.cs) can be viewed here. Its companion script, [GFWStatusApplicator.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/GFWStatusApplicator.cs), can be viewed here.

![GaleForceWinds](https://github.com/user-attachments/assets/c1a57ef4-09bd-4b97-a8dc-da656a3b8cf4)

## Activator Drone
Activator Drone creates a combat drone that orbits the player and attacks enemies within a 40m radius. Its name comes from its ability to activate a weapon's passive-based Platforms: 
* [Siphonic Platform](#siphonic-platform)- Confirmed hits by the drone increase the player's health and shield.
* [Mining Platform](#mining-platform) - Confirmed hits by the drone create and shatter a Lucent cluster at the hit location.
* [Trenchant Platform](#trenchant-platform) - Confirmed hits by the drone apply health debuffs. The drone cannot apply Trenchant's other debuffs.
* [Cache Platform](#cache-platform)- The drone's weapon is changed to a mini-rocket launcher.

Manually aiming will use the Activator Drone's laser designator. Aiming at an enemy will assign it as the drone's target. Otherwise, the drone checks its array of targets and selects one at random to attack. If its target is obstructed, the drone will search for another target.

* (Fated) Activator Drones can trigger a weapon's second Function cheat on confirmed hits or enemy defeats:
  * ["Wait! Now I'm Ready!"](#wait-now-im-ready)
  * [Efficacy](#efficacy)
  * [Inoculated](#inoculated)
  * [Cadence](#cadence)
  * [Enshroud](#enshroud)
  * [Bolster](#bolster)
 
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
Equivalent Exchange adds 35% of damage received by an enemy directly to the weapon's damage and the player's current health. Weapon damage can permanently increase up to 150% of its original damage.

Equivalent Exchange is assigned to the Exotic Full Fire Rifle "Outstanding Warrant". Its companion cheat is [Inoculated.](#inoculated)
* [EquivalentExchange.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/EquivalentExchange.cs) can be viewed here.

![EquivalentExchange](https://github.com/user-attachments/assets/1023fa5b-6465-41a8-b7a1-7905211a3f7a)

## Pay to Win
Pay to Win converts 30,000 of the Player's "Lucent" currency into 150 stacks of a 50% Weapon damage increase. 

Stacks are removed three at a time on confirmed enemy hits, and additional Lucent cannot be converted until all stacks have been removed.

Pay to Win is assigned to the Exotic Machine Gun "The Dismissal". Its companion cheat is [The Most Resplendent.](#the-most-resplendent)
* [PayToWin.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/PayToWin.cs) can be viewed here.

![PayToWin](https://github.com/user-attachments/assets/59d4a25c-aadd-48dc-bd56-bc71f4f9547d)

## Superweapon
Superweapon grants one stack of 10% damage resistance on confirmed enemy defeats, up to 80% with 8 stacks. Toggling Superweapon enables the ability to charge and fire a high-damage laser while aiming. Standing still significantly increases the charge rate. The laser inflict 1,000% of weapon damage per damage resistance stack, up to 8,000% with 8 stacks. 

Superweapon is assigned to the Exotic Pistol "Apathetic". Its companion cheat is [Counterplay.](#counterplay)

* [Superweapon.cs](Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/Superweapon.cs) can be viewed here.

<img width="600" height="337" alt="Superweapon" src="https://github.com/user-attachments/assets/627003d7-2664-4d9d-8bee-4b753754f4b7" />

## Volant
Volant activates the player's zero gravity controls, enabling character flight until their shield is broken or if Volant is manually disabled.

Volant is assigned to the Exotic Burst Fire Rifle "Mercies". Its companion cheat is ["Wait! Now I'm Ready!".](#wait-now-im-ready)

* [Volant.cs](Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/Volant.cs) can be viewed here.

<img width="600" height="337" alt="Volant" src="https://github.com/user-attachments/assets/6f9c5c1c-2e09-42e1-afe5-f2ca2c492109" />

## "Social Distance, Please!"
"Social Distance, Please!" applies a 30% damage increase on confirmed enemy hits and a health debuff that increases damage taken. Defeated, debuffed Enemies spread 400% of the Weapon's damage in a 10m radius.

The damage increase lasts for ten seconds. The timer is extended when a non-debuffed enemy receives a health debuff.

"Social Distance, Please!" is assigned to the Exotic Shotgun "Viral Shadow". Its companion cheat is [Not with a Stick.](#not-with-a-stick)

* [SocialDistancePlease.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/Social%20Distance%2C%20Please!/SocialDistancePlease.cs) can be viewed here. [EnemyHealthScript.cs](/Assets/Scripts/Enemies/EnemyHealthScript.cs#L331-L351) holds the damage spread behavior when an Enemy is defeated.

![SocialDistancePlease](https://github.com/user-attachments/assets/8f908001-ccda-4da4-bf98-e429250e1131)

## Early Berth gets the Hearst
Early Berth gets the Hearst applies and triggers a Berth explosion on every other confirmed enemy hit.

A "Berth" is described as a status effect unique to enemies that provides dangerous attack augmentations and triggers explosions on defeats. Early Berth gets the Hearst, when the condition is met, applies the Berth condition and immediately triggers the detonation behavior.

Early Berth gets the Hearst is assigned to the Exotic Single Fire Rifle "Contempt For Fellows". Its companion cheat is [Efficacy.](#efficacy)

* [EarlyBerthGetsTheHearst.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/EarlyBerthGetsTheHearst.cs) can be viewed here.

![EarlyBerthGetsTheHearst](https://github.com/user-attachments/assets/1514e231-ec94-4fa6-b284-bddf9de358d9)

## "Absolutely no Stops!"
"Absolutely no Stops!" increases Underfoot's fire rate by 50% and triggers an automatic reload upon expending the magazine. 

This effect remains active until Underfoot has expended all reserve ammunition or if the player stops firing.

"Absolutely no Stops!" is assigned to the Exotic Submachine Gun "Underfoot". Its companion cheat is [Forager.](#forager)

* [AbsolutelyNoStops.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/AbsolutelyNoStops.cs) can be viewed here.

![AbsolutelyNoStops](https://github.com/user-attachments/assets/02fe64d2-ee0d-4f84-9fad-da95ab4e928f)

## Flashpoint
Flashpoint swaps Nebulous At Best's munitions with floating Lucent mines. Mines persist for 1 minute, and all mines are detonated on manual input. Ten mines can be active at a time; additional mines detonates the oldest mine. Switching weapons does not detonate mines, though hits by other weapons can detonate a mine.

Flashpoint is assigned to the Exotic Grenade Launcher "Nebulous At Best". Its companion cheat is [Positive-Negative.](#positive-negative)

* [Flashpoint.cs](Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/Flashpoint.cs) can be viewed here.

<img width="600" height="337" alt="Flashpoint" src="https://github.com/user-attachments/assets/85cfc337-0081-4c0e-a1a7-64d0944d78a0" />

## Defiance
Defiance increases weapon & melee damage by 100% while Deleterious is equipped. Guarding against enemy attacks reflects 1,000% of damage onto the attacker.

Defiance is assigned to the Exotic Opening Shot "Deleterious". Its companion cheat is [All Else Fails.](#all-else-fails)

* [Defiance.cs](Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/Defiance.cs) can be viewed here.

<img width="600" height="337" alt="Defiance" src="https://github.com/user-attachments/assets/70e9a7d8-5a9e-4c17-b088-5d5f8eb647ba" />

## It Writhes
It Writhes swaps Bad Grief's munitions with tandem explosives, detonating twice on surface or enemy hits. Detonations stagger enemies, recoiling them backwards and pausing their actions for 1 second. Enemies staggered by It Writhes have a 50% chance to trigger a Berth explosion (see [Early Berth gets the Hearst](#early-berth-gets-the-hearst) for description on the Berth effect). 

It Writhes is assigned to the Exotic Anti-materiel Laser Rifle (AMLR) "Bad Grief". Its companion cheat is [Gale Force Winds.](#gale-force-winds)

* Though known outwardly as "It Writhes", its script name is "Repurposed Form". [RepurposedForm.cs](Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/RepurposedForm.cs) can be viewed here.

<img width="600" height="337" alt="ItWrithes" src="https://github.com/user-attachments/assets/2d93078d-ff2b-4dd4-88ce-eac8197ae525" />
