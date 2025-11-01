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
  * Efficacy
  * Inoculated
  * Rude Awakening
  * Not With a Stick
  * Malicious Wind-Up
  * Positive-Negative
  * Cadence
  * Good Things Come
  * All Else Fails
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
"Wait! Now I'm Ready!" adds 5% of a Player's maximum Shield capacity onto their current Shield strength when a Weapon with this cheat has defeated an Enemy.
* (Fated) "Wait! Now I'm Ready!" increases the percentage of this effect to 10%.

This percentage is converted to decimal form and then multiplied by the Player's maximum Shield amount. An integer variable, "shieldGain", is assigned the operation's result. When a Weapon confirms a kill on a defeated Enemy, the Player's current Shield strength is added onto by "shieldGain". If it detects that it has overhealed a Player's Shield, their current Shield value is assigned to their maximum Shield value. This Cheat does not add onto their Shield if their strength is already at maximum.

* [WaitNowImReady.cs](/Assets/Scripts/Weapons/Added%20Function%20Cheats/WaitNowImReady.cs) can be viewed here.

![WaitNowImReady](https://github.com/user-attachments/assets/4b694e05-a5be-4b68-a7f6-38d17b3c4e08)
