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
  * The Most Resplendent
  * Fulminate
  * Forager
  * Counterplay
  * Enshroud
  * Gale Force Winds
  ## Exotic
  * Equivalent Exchange
  * Pay to Win
  * Shelter in Place
  * Off your own Supply
  * "Social Distance, Please!"
  * Early Berth gets the Hearst
  * "Absolutely no Stops!"

# Overview
This document explains ATGIG's Core system, Cheats, by illustrating what each specific Cheat does passively or through conditionals. There are over 30 available Cheats. Due to the amount of Cheats and with respect to readability, direct links to each Cheat's code are provided following their descriptions and function breakdowns. Visuals will accompany explanations, but only for Functional Cheats.
* It is worth noting that Functional Cheats receive upgraded behaviors on Weapons at the fifth (Fated) rarity. Points preceded with "(Fated)" discuss a Cheat's increased strength at that level. 

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
