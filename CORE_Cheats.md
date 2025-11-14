Welcome!

# All That Glitter is Gold: Cheats

## Contents
* [Overview](#overview) 
* Statistical Cheats
  * [Yields](#yields)
  * [Stores](#stores)
  * [Sights](#sights)
  * [Hands](#hands)
* Functional Cheats
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
* Exotic Functional Cheats
  * [Equivalent Exchange](#equivalent-exchange)
  * [Pay to Win](#pay-to-win)
  * [Shelter in Place](#shelter-in-place)
  * [Off your own Supply](#off-your-own-supply)
  * ["Social Distance, Please!"](#social-distance-please)
  * [Early Berth gets the Hearst](#early-berth-gets-the-hearst)
  * ["Absolutely no Stops!"](#absolutely-no-stops)

# Overview
This document explains ATGIG's Core system, Cheats, by illustrating what each specific Cheat does passively or through conditionals. There are over 30 available Cheats. Due to the amount of Cheats and with respect to readability, direct links to each Cheat's code are provided following their descriptions and function breakdowns. Visuals will accompany explanations, but only for Functional Cheats.
* It is worth noting that Functional Cheats receive upgraded behaviors on Weapons at the fifth (Fated) rarity. Points preceded with "(Fated)" discuss a Cheat's increased strength at that level.
* Exotic Functional Cheats are powerful Cheats that are specific to one Weapon type and that come with a companion Cheat from the standard pool. The Weapon type will be specified after a Cheat's description. 

# Statistical Cheats
## Yields
Yield Statistical Cheats increase the maximums of Weapons' magazine sizes by percentages: 
* Deep Yield provides a 12% increase.
* Deeper Yield provides a 24% increase.

This percentage is converted to decimal form and then multiplied by that Weapon's maximum magazine size. An integer variable is assigned the operation's result. The Weapon's magazine size is added onto by this variable.

* Deep Yield can be viewed [here.](/Assets/Scripts/Weapons/Magazine%20Cheats/DeepYield.cs)
* Deeper Yield can be viewed [here.](/Assets/Scripts/Weapons/Magazine%20Cheats/DeeperYield.cs)

## Stores
Stores Statistical Cheats increase the maximums of Weapons' ammunition reserves by percentages: 
* Deep Stores provides a 15% increase.
* Deeper Stores provides a 30% increase.

This percentage is converted to decimal form and then multiplied by that Weapon's maximum ammo reserve size. An integer variable is assigned the operation's result. The Weapon's reserves is added onto by this variable.

* Deep Stores can be viewed [here.](/Assets/Scripts/Weapons/Magazine%20Cheats/DeepStores.cs)
* Deeper Stores can be viewed [here.](/Assets/Scripts/Weapons/Magazine%20Cheats/DeeperStores.cs)

## Sights
Sights Statistical Cheats increase the Effective Range of a Weapon by a percentage of that weapon's maximum Range: 
* Far Sight provides a 10% increase.
* Farther Sight provies a 20% increase.

This percentage is converted to decimal form and then multiplied by that Weapon's maximum Range. An integer variable is assigned the operation's result. The Weapon's Effective Range is added onto by this variable.

* Far Sight can be viewed [here.](/Assets/Scripts/Weapons/Range%20Cheats/FarSight.cs)
* Farther Sight can be viewed [here.](/Assets/Scripts/Weapons/Range%20Cheats/FartherSight.cs)

## Hands
Hands Statistical Cheats increase a Weapons' Reload Speed by a percentage:
* Hasty Hands provides a 15% increase.
* Hastier Hands provies a 25% increase.

This percentage is converted to decimal form and then multiplied by that Weapon's reload speed. An integer variable is assigned the operation's result. The Weapon's Reload Speed value is subtracted by this variable.

* Hasty Hands can be viewed [here.](/Assets/Scripts/Weapons/Reload%20Speed%20Cheats/HastyHands.cs)
* Hastier Hands can be viewed [here.](/Assets/Scripts/Weapons/Reload%20Speed%20Cheats/HastierHands.cs)

# Functional Cheats
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

# Exotic Functional Cheats
## Equivalent Exchange
Equivalent Exchange adds 35% of Enemy damage received directly to the Weapon's damage and the Player's current Health. Weapon damage can permanently increase up to 150% of its original damage.

Equivalent Exchange is assigned to the Full Fire Rifle Weapon type. Its companion Cheat is ["Wait! Now I'm Ready!".](#wait-now-im-ready)
* [EquivalentExchange.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/EquivalentExchange.cs) can be viewed here.

![EquivalentExchange](https://github.com/user-attachments/assets/1023fa5b-6465-41a8-b7a1-7905211a3f7a)

## Pay to Win
Pay to Win converts 5,280 of the Player's "Lucent" currency into 150 stacks of a 50% Weapon damage increase. 

Stacks are removed three at a time when a Weapon hits an Enemy, and Lucent cannot be converted until all stacks have been removed.

Pay to Win is assigned to the Machine Gun Weapon type. Its companion Cheat is [The Most Resplendent.](#the-most-resplendent)
* [PayToWin.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/PayToWin.cs) can be viewed here.

![PayToWin](https://github.com/user-attachments/assets/59d4a25c-aadd-48dc-bd56-bc71f4f9547d)

## Shelter in Place
Shelter in Place increases Weapon damage by 100% and provides 80% damage resistance so long as a Player refrains from movement. Movement of any kind, excluding Enemy attack knockbacks, ends the effect.

Shelter in Place is assigned to the Pistol Weapon type. Its companion Cheat is [Positive-Negative.](#positive-negative)

* [ShelterInPlace.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/ShelterInPlace.cs) can be viewed here.

![ShelterInPlace](https://github.com/user-attachments/assets/5afb239e-5b3f-4ab4-9650-f0c184067e64)

## Off your own Supply
Off your own Supply provides increases to the following attributes at the expense of a sacrificed Shield for 15 seconds: 
* Movement Speed increases by 10%.
* Reload Speed increases by 80%.
* Weapon damage increases by 140%.
* Weapon recoil is zeroed.

The Player's Shield is unable to regenerate while the effects are active. 

Off your own Supply is assigned to the Semi Fire Rifle Weapon type. Its companion Cheat is [Inoculated.](#inoculated)

* [OffYourOwnSupply.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/Exotic%20Cheats/OffYourOwnSupply.cs) can be viewed here.

![OffYourOwnSupply](https://github.com/user-attachments/assets/f8a20342-5ebd-4f9b-8e70-93d8c752230a)

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
