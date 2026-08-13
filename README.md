# Welcome!

<img width="600" height="337" alt="itch io landing GIF" src="https://github.com/user-attachments/assets/e92da317-60a3-47ae-8b2f-29c4a850258e" />


Current Version: MVP 0.1.9 (8/11/2026)

# Contents:

## Details
* [Game Description](#game-description)
* [Installation Instructions](#installation-instructions)
* [Project Objective](#project-objective)
* [Technologies](#technologies)
* [Responsibilities](#responsibilities)

## Pursuits
* Player
	* [Camera Clipping Countermeasure](#camera-clipping-countermeasure)
	* [Slope Traversal](#slope-traversal)
* Weapons
	* [Weapon Saving](#weapon-saving) 
   	* [Cheats](#cheats)
* Enemy Attacks
	* [Pounce](#pounce)
	* [Jump](#jump)
	* [Range](#range)
 	* [Melee](#melee)

# Details
## Game Description
Resplendent is a third-person Action game which combines horde combat, RNG, and gunplay to craft a lite experience that aspires to whet the looter-shooter appetite.
* Resplendent can be downloaded and played on itch.io: [Link](https://dilladev.itch.io/atgig)
## Installation Instructions
* Install Unity 2021, as this is the version of the Engine that I develop with: [Link](https://unity.com/releases/editor/whats-new/2021.1.29f1#installs)
	* The link takes you to the patched version of the engine. I specifically use Unity 2021.1.16f1, but Unity has disclosed that this version is susceptible to the security flaw they detected at the start of October. As of 11/5/2025, Unity Hub refuses to finish installation of this patched version for me. I have no reason to believe, however, that the provided version will not load the project.
* Clone this repository.
	* Open Github Desktop, Go to File > Clone Repository.
 	* Paste the URL link in the repository box.
  	* Specify a location that you'd like the project to install.
  		* I personally made a folder after its namesake and placed the project there.
* Open the project through Unity Hub
	* Add the provided Unity version to Unity Hub's "Installs" category (Installs > Locate > Find version)
 	* Specify the project's editor version, then open the project.
## Project Objective
Resplendent's primary objective is to provide brief, favorable play sessions by offering renditions of experiences encountered in the looter-shooter genre, such as:
* The acquisition and surprise of weapons containing interesting traits combinations
* The feeling of mastery in understanding a weapon's build
* The toppling of challenges based on one's perceived increase in strength.

Games like Destiny 2, The Division 2, Remnant II, and Warhammer 40K: Space Marine 2 each serve as inspirations. Resplendent is my interpretation of experiences learned from those games, and can be felt in the player, weapon, and enemy systems within.
## Technologies
* Unity 3D (Version 2021.1.16f)
* C#
* Blender
* Substance Painter
## Responsibilities
I solo-developed Resplendent. As such, I am responsible for:
* Player abilities (movement, evasion, melee attacks, sprinting, and guarding)
* Player systems (Slope Traversal and Anti-camera clipping)
* Weapon attributes (type, damage, rate of fire, etc.)
* Weapon augmentations (rarity, Cheats & Platforms)
* Weapon saving
* Enemy attack types (Melee, Range, Pounce, Jump)
* Gametypes (Campaign, Viricide)
* Out-of-gameplay systems (Main Menu Inventory management, Requisitions Kiosk)

Bulleted below are detailed accounts of Resplendent's most notable pursuits, accompanied by visuals and organized by category. 

# Pursuits
## Player
### Camera Clipping Countermeasure
The player's camera avoids clipping through walls in the following steps: 
* A Ray is cast starting from the player's rear and ending at the camera's position.
* When a surface intersects with the end point of the Ray, the camera's position is assigned to the Raycast hit point, oriented to the surface's Normal direction.
	* The position can be further pushed horizontally or vertically using an offset.

This approach has shown to be effective in reducing occurrences of wall clipping (barring extreme cases).

```csharp
//From PlayerCameraScript.cs
Vector3 offset;
RaycastHit hit;
if (Physics.Raycast(offsetCheckPos.transform.position, (playerCamera.transform.position - offsetCheckPos.transform.position).normalized, out hit, collideCheck, cameraOnly))
{
	if (hit.point != null)
	{
		if(hit.collider.tag == "Projectile" || hit.collider.tag == "Enemy" || hit.collider.tag == "Lucent" || hit.collider.tag == "Ammo" || hit.collider.tag == "Corpse")
		{
			//Do nothing
		}
                
		else
		{
			offset = hit.point + (hit.normal + new Vector3(0, offsetPushY, offsetPushZ));
			playerCamera.transform.position = offset;

			Debug.DrawRay(offsetCheckPos.transform.position, (playerCamera.transform.position - offsetCheckPos.transform.position).normalized * collideCheck, Color.yellow);
			Debug.DrawLine(hit.point, offset * offsetMult, Color.red);

		}
	}                  
}
```
![ezgif-6fdf065644ca79](https://github.com/user-attachments/assets/d182ec60-ce5a-430e-99cd-1730825e1ea6)

### Slope Traversal
Slope traversal is handled through retrieval and interpretation of the Dot Product between two vectors:
* The value is retrieved from the slope's surface Normal and the Player's forward direction if handling Vertical movement. 
* The Cross Product between a Player's position and the slope's surface Normal is retrieved first, followed by the Dot Product between that value and the Player's forward direction if handling Horizontal movement.

If the Dot Product is less than zero:
* (Vertical) Going upwards while facing upwards will apply force upwards. Going downwards while facing upwards will apply force downwards.
* (Horizontal) Going left while the slope is to the right will apply force downwards. Going right while the slope is to the right will apply force upwards.

If the Dot Product is greater than zero:
* (Vertical) Going downwards while facing downwards will apply force downwards. Going upwards while facing downwards will apply force upwards.
* (Horizontal) Going right while the slope is to the left will apply force downwards. Going left while the slope is to the left will apply force upwards.

Through this approach, the Player can traverse slopes up to 40 degrees, dependent on amount of force application. Traversals beyond 40 degrees require more force to be applied.

<details>

<summary> snippet from PlayerMoveScript.cs </summary>

```csharp
Vector3 sideVector;
RaycastHit hit;
if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, slopeCheckLength))
{

	Debug.DrawRay(transform.position, Vector3.Cross(transform.forward, hit.normal), Color.red);
	Debug.DrawRay(transform.position, hit.normal + new Vector3(0, 0, hit.normal.z), Color.blue);
	Debug.DrawRay(transform.position, transform.forward, Color.yellow);

	sideVector = Vector3.Cross(transform.position, hit.normal).normalized;

	//Handles vertical slope traversal
	if (Vector3.Dot(hit.normal, transform.forward) < 0)
	{
		if (vertInput < 0)
		{
			playerRigid.AddForce(-Vector3.up * slopeForce);
		}

		else
		//Force is applied upwards
	}

	else if (Vector3.Dot(hit.normal, transform.forward) > 0)
	{
		if (vertInput < 0)
		{
			playerRigid.AddForce(Vector3.up * slopeForce);
 		}

		else
		//Force is applied downwards
	}
        
	//Handles Horizontal slope traversal
	if (Vector3.Dot(sideVector, transform.forward) > 0)
	{
		if (horizInput > 0)
		{
			playerRigid.AddForce(-Vector3.up * slopeForce);
		}

		else
		//Force is applied upwards
	}

	else if (Vector3.Dot(sideVector, transform.forward) < 0)
	{
		if (horizInput < 0)
		{
			playerRigid.AddForce(-Vector3.up * slopeForce);
		}

		else
		//Force is applied upwards
	}
}     
```
</details>

![ezgif-3227e372bb6e1c](https://github.com/user-attachments/assets/79f54600-81d4-472c-a9a2-5896ed81e2aa)

## Weapons
### Weapon Saving
Resplendent's Weapon Saving system uses Stream I/O to both respawn and catalog weapons held in a player's inventory. When advancing levels or upon defeat, the player's inventory records attributes of its weapons and saves them to a file as a string that can be 5, 9, 10, or 11 characters in length. Each value within this string represents a weapon's constituent parts. For example:

020151458
* <ins>0</ins>20151458 - Denotes weapon type.
* 0<ins>2</ins>0151458 - Denotes weapon rarity.
* 02<ins>0</ins>151458 - Denotes weapon Exotic property; it is either Exotic (1) or not (0).
* 020<ins>1</ins>51458 - Denotes weapon favorite property; it is either a favorite (1) or not (0).
* 0201<ins>5</ins>1458 - Denotes weapon Platform.
* 02015<ins>1</ins>458 - Denotes Stat Cheat #1.
* 020151<ins>4</ins>58 - Denotes Stat Cheat #2.
* 0201514<ins>5</ins>8 - Denotes Stat Cheat #3.
* 02015145<ins>8</ins> - Denotes Stat Cheat #4.

The above string describes a Weapon with these properties: 
* Full Fire Rifle (0)
* Rarity 2 (2)
* Non-exotic (0)
* Favorite (1)
* Siphonic Platform (5)
* Statistical Cheats:
	*  Deep Yield (1)
 	*  Deeper Stores (4)
  	*  Far Sight (5)
  	*  Hastier Hands (8)

String length for weapons depends on that weapon's rarity. Rarity 1 weapons only receive Platforms. For example, "21002" represents the following traits: 
* Pistol (2)
* Rarity 1 (1)
* Non-exotic (0)
* Not a favorite (0)
* Efficient Platform (2)
 
Another example, "340182468&+", details this weapon's features: 
* Burst Fire Rifle (3)
* Rarity 4 (4)
* Non-exotic (0)
* Favorite (1)
* Cache Platform (8)
* Statistical Cheats: 
	* Deeper Yield (2)
	* Deeper Stores (4)
 	* Farther Sight (6)
  	* Hastier Hands (8)
* Functional Cheats:
	* Activator Drone (&)
 	* Bolster (+)

When a game starts, the "WeaponManager" finds the inventory file, titled "inventory.txt", and reads its contents, creating new weapons with the recorded characteristics attached.

History:
* In its earliest form, it only recorded weapons as strings that were eight characters long, as each Weapon was designed to have the same structure. Moreover, without a delay time between spawns, the system often delivered the weapons back to the inventory out of order.
* MVP 0.1.3 - Due to the lacking strength of Function Cheats at the time during a playtest, weapons required a fundamental change in power. Weapons were updated to allow two Function Cheats to roll, and to also allow no Function Cheats to roll on Rarity 1 weapons.
* MVP 0.1.5 - The "Weapon Manager" was upgraded to use a Coroutine for weapon respawns, delaying the spawn of new weapons. This naturally fixed the issue of disorderly Weapon returns to the inventory.
* MVP 0.1.6 - MVP 0.1.7 - Methods "WriteOnReset()" (From PlayerInventoryScript.cs) and "RespawnWeapons()" (From WeaponManagerScript.cs) received significant reductions in code length:
	* WriteOnReset()
 		* Old length: 2,347 lines
   		* New length: 401 lines (83-84% reduction)
	* RespawnWeapons()
		* Old length: 2,820 lines
  		* New length: 568 lines (79-80% reduction)

The changes in size made the Weapon Saving system more maintainable, making the integration of weapon types such as the Grenade Launcher (MVP 0.1.8), Opening Shot & AMLR (MVP 0.1.9) easier to perform.

<details>
<summary> snippet from PlayerInventoryScript.cs </summary>
	
```csharp
CreateInventoryFile(); //If "inventory.txt" doesn't exist, it makes a new file. Otherwise, it overwrites the existing file. 

using (StreamWriter write = new StreamWriter(filepath))
{
	if (inventory.Count > 0)
	{
		for (int i = 0; i < inventory.Count; i++)
		{
			if (inventory[i].name == "Full Fire Rifle" || inventory[i].name == "Outstanding Warrant")
			{
				write.Write("1");
			}

			if (inventory[i].name == "Machine Gun" || inventory[i].name == "The Dismissal")
			//Writes "2", And so on, up to 7.

			if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
			{
				write.Write("1");
			}

			if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 2)
			//Writes "2" and so on, up to 5.

			if (inventory[i].GetComponent<FirearmScript>().isExotic == true)
			{
				write.Write("1");
			}

			else
			//Writes "0".

			if (inventory[i].GetComponent<FirearmScript>().favorite)
			{
				write.Write("1");
			}

			else
			//Writes "0"

			if(inventory[i].GetComponent<DefaultPlatform>())
			{
				if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
				{
			 		write.WriteLine("1");
				}

				else
				//Writes "1" but does not create a new line if weapon rarity is not equal to 1
			}

			if(inventory[i].GetComponent<FirearmScript>().weaponRarity >= 2)
			{
				if (inventory[i].GetComponent<DeepYield>())
				{
			 		write.Write("1");
				}

				if (inventory[i].GetComponent<DeeperYield>())
				//Writes "2"

				//Records remaining Stat cheats...

				if(inventory[i].GetComponent<FirearmScript>().weaponRarity == 2)
				{

					if (inventory[i].GetComponent<HastyHands>())
					{
						write.WriteLine("7");
					}

					if (inventory[i].GetComponent<HastierHands>())
					//Writes "8" and starts a new line
				}

				if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 3)
				//Records last Stat cheat and detects, records Function cheat, then starts a new line

				if (inventory[i].GetComponent<FirearmScript>().weaponRarity >= 4)
				//Records last Stat cheat and detects, records Function cheats, then starts a new line
			}			
		}
	}
}
```
</details>

<details>
<summary> snippet from WeaponManagerScript.cs </summary>
	
```csharp
yield return new WaitForSeconds(spawnDelayTimer); //Weapons are spawned on a delay to return weapons in recorded order

string c = "Comic Sans"; //Initializing a string.

for (int s = 0; s < player.readdedWeps.Count; s++)
{
	c = player.readdedWeps[s];
	wepStr = c[0].ToString();
	rarStr = c[1].ToString();
	exoStr = c[2].ToString();
	favStr = c[3].ToString();
	pltStr = c[4].ToString();

	if (player.readdedWeps[s].Length == 9)
	{
		cOneStr = c[5].ToString();
		cTwoStr = c[6].ToString();
		cThreeStr = c[7].ToString();
		cFourStr = c[8].ToString();
	}          

	if(player.readdedWeps[s].Length == 10)
	{
		cOneStr = c[5].ToString();
		cTwoStr = c[6].ToString();
		cThreeStr = c[7].ToString();
		cFourStr = c[8].ToString();
		cFiveStr = c[9].ToString();
	}

	if(player.readdedWeps[s].Length == 11)
	{
		cOneStr = c[5].ToString();
		cTwoStr = c[6].ToString();
		cThreeStr = c[7].ToString();
		cFourStr = c[8].ToString();
		cFiveStr = c[9].ToString();
		cSixStr = c[10].ToString();
	}

	if (wepStr == "0")
	{
		GameObject item = Instantiate(weapons[0], transform.position, transform.rotation);
		item.name = weapons[0].name;
	}

	if (wepStr == "1")
	//Creates a Machine Gun, and so on, up to the last Weapon type

	if (rarStr == "1")
	{
		item.GetComponent<FirearmScript>().weaponRarity = 1;
	}
	//And so on, assigning rarities up to 5 for weapons.

	if(favStr == "1")
	{
		item.GetComponent<FirearmScript>().favorite = true;
	}

	else
	//Weapon favorite value is set to false

	if(pltStr == "1")
	{
 		item.AddComponent<DefaultPlatform>();
	}

	if (pltStr == "2")
	//Adds the Efficient Platform component	

	if (player.readdedWeps[s].Length >= 9)
	{
		if (cOneStr == "2")
		//Adds the component, "Deeper Yield".

		if (cTwoStr == "3")
		//Adds the component, "Deep Stores". Adds "Deeper Stores" if cTwoStr is "4".

		if (cThreeStr == "5")
		//Adds the component, "Far Sight". Adds "Farther Sight" if cThreeStr is "6".

		if (cFourStr == "7")
		//Adds the component, "Hasty Hands". Adds "Hastier Hands" if cFourStr is "8".
	}

	if (player.readdedWeps[s].Length == 10)
	{
		if (cFiveStr == "0")
		//Adds Cheat "Wait! Now I'm Ready!"
		
		if (cFiveStr == "1")
		//Adds Cheat Efficacy, and so on.
	}

	if (player.readdedWeps[s].Length == 11)
	{
		if (cFiveStr == "A")
		//Adds Exotic Cheat Equivalent Exchange. Letters are always Exotic Cheats
		//"cFiveStr" could also be: B, C, D, E, F, G, H, I, or J

		if (cFiveStr == "9")
		//Adds Cheats All Else Fails. "cFiveStr" could also be: 4, 5, 6, 8, !, @, #, &, or *

		if (cSixStr == "0")
        //Adds Cheat "Wait! Now I'm Ready!" "cSixStr" could also be: 1, 2, 7, 3, $, %, ^, +, or -
	}

	yield return new WaitForSeconds(spawnDelayTimer);
}
```
</details>


https://github.com/user-attachments/assets/c3b1653f-7c1c-4bc8-8f8e-6d6ed61741bd


### Cheats
Cheats are Resplendent's Core system, granting permanent bonuses to weapons. Explanations and visuals for what each Cheat specifically does can be found on the [Cheats](CORE_Cheats.md) file.

Cheats are applied to weapons through Random Number Generation (RNG). The moment a Weapon is created, methods are called to choose and apply what are known as Stat Cheats and Function Cheats: 
* Stat Cheats upgrade a weapon's max ammo, reload speed or range attributes.
* Function Cheats extend a weapon's offensive, neutral, or defensive potential through conditional triggers.
  	* Its sibling system, Platforms, modify a weapon's base damage, recoil, or fire rate performance or enable passive benefits.

A number is randomized between a set range. The chosen Cheat is determined by where the value sits within that range. All cheats are divided into distinct pools: 
* Yields (Stat Cheats that increase magazine sizes)
* Stores (Stat Cheats that increase max ammunition reserves)
* Sights (Stat Cheats that increase effective ranges)
* Hands (Stat Cheats that increase reload speeds)
* Platforms (Performance modifiers or intrinsic effects)
* Functional Cheats
	* Rarity 1 weapons can only roll Platforms
 	* Rarity 2 weapons can roll Platforms and Stat cheats only
  	* Rarity 3 weapons can roll Platforms, Stat cheats, and only one Function cheat.
  	* Rarity 4 weapons and up can roll Platforms, Stat cheats, and two Function cheats.

Notes:
* Two of the same Cheat cannot be generated on a weapon.
* Cheats are not weighted to generate more often than others; every Cheat is equally likely to be chosen.
* Exotics are curated weapons and do not require random Cheat generation.
* Weapons being reproduced by the Weapon Manager do not require Cheat generation, as components are chosen based on characters.

<details> 

<summary> snippet from FirearmScript.cs </summary>

```csharp
public virtual void AmmoCheats()
{
	if (isExotic == true)
	//Exotics generate the best Cheat variant and leave the method.
	return;

	if (saved == true)
	//Weapons made by the WeaponManager add Cheats directly, so there is no need to generate.
	return;

	cheatRNG = Random.Range(0, 101);
	if(cheatRNG <= 50)
	{
		gameObject.AddComponent<DeepYield>();
	}

	else
	//Adds the component, "Deeper Yield". 

	cheatRNG = Random.Range(100, 201);

	if (cheatRNG <= 150)
	{
		gameObject.AddComponent<DeepStores>();
	}

	else
	//Adds the component, "Deeper Stores".      
}
//The Platform method and other Stat Cheat methods operate identically.
//...
public virtual void CheatGenerator()
{
	if(isExotic == true)
	{
		//Exotic weapons use negative numbers to denote what Functional Cheats to receive.
		if(cheatRNG == -1)
		{
			gameObject.AddComponent<EquivalentExchange>();
			gameObject.AddComponent<WaitNowImReady>();
		}

		if(cheatRNG == -2)
		//Adds components, "Absolutely no Stops!" + Forager
		//And so on, up to -7. 

		return;
	}

	if (saved == true)
	{
		//Weapons made by the WeaponManager add Cheats directly, so there is no need to generate.
		return;
	}

	if(weaponRarity == 2 || weaponRarity == 3)
	{
		cheatRNG = Random.Range(400, 1201);
		if (cheatRNG <= 450)
		{
			gameObject.AddComponent<WaitNowImReady>();
			gameObject.GetComponent<WaitNowImReady>().proc = procOne;
			procTwo.GetComponent<Text>().text = " ";
		} //Adds Cheat, "Wait! Now I'm Ready!"

		if (cheatRNG > 450 && cheatRNG <= 500)
		//Adds Efficacy, and so on, with last range between (1151 and 1200).

	}

	if(weaponRarity >= 4)
	{
		//Pool #1 
		cheatRNG = Random.Range(400, 481);
		if(cheatRNG  <= 410)
		{
			gameObject.AddComponent<AllElseFails>();
			gameObject.GetComponent<AllElseFails>().proc = procOne;
		}

		if (cheatRNG > 410 && cheatRNG <= 420)
		//Adds Cheat "Not With a Stick", and so on, with last range between (471 and 480)

		//Pool #2
		cheatRNG  = Random.Range(480, 561);
		if (cheatRNG  <= 490)
		{
			gameObject.AddComponent<WaitNowImReady>();
			gameObject.GetComponent<WaitNowImReady>().proc = procTwo;
		}

		if (cheatRNG > 490 && cheatRNG <= 500)
		//Adds Cheat "Efficacy" and so on, with last range between (551 and 560)
	}
}
```
</details>

https://github.com/user-attachments/assets/255d3d39-c299-49a2-bcec-0ed34364f432

## Enemy Attacks
### Pounce
Pounce enemies commit to combat in the following steps: 
* The enemy approaches the player until they reach their engagement distance.
* The enemy casts a Ray at the player and records their last known position.
* The enemy rapidly dashes towards that position using Vector3.Lerp. An attack Ray is cast for the duration of this movement to detect the player.
	* When they reach the recorded position, they enter an attack timeout for a set duration. The process repeats.
 	* Confirmed hits on a player inflicts damage to them and applies Rigidbody force. They enter an attack timeout for a set duration. The process repeats.

 <details>

<summary> snippet from ReplevinScript.cs </summary>

```csharp
if(!HaveIDied())
{
	self.speed = moveSpeed;
	distance = player.transform.position - transform.position;
	Vector3 rayOrigin = attackStartPoint.transform.position;
	RaycastHit hit, hitTheSequel;

	if (distance.magnitude <= meleeRangeMin && CanSeePlayer())
	{
		self.ResetPath();
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

		if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin) && !recorded)
		{
			if (hit.collider.tag == "Player")
			{
				lastPlayerPosition = hit.point;
				recorded = true;

			}
		}

		transform.position = Vector3.Lerp(transform.position, lastPlayerPosition, gapClose * Time.deltaTime);
		lastKnownDistance = lastPlayerPosition - transform.position;

		if (lastKnownDistance.magnitude <= pounceLimit)
		{
			slamTimeout = true;
		}

		if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hitTheSequel, 2f))
		{
			if (hitTheSequel.collider.tag == "Player" && canAttackAgain)
			{
				if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
				{
					if (gameObject.GetComponent<DebuffScript>() == null)
					{
						gameObject.AddComponent<DebuffScript>();
					}

					slamTimeout = true;
					canAttackAgain = false;
				}

				else
				{
					hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
					hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

					Vector3 knockbackDir = transform.forward;
					knockbackDir.y = 0;
					hitTheSequel.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);
					manager.damageDealt += damage;

					slamTimeout = true;
					canAttackAgain = false;
				}
			}
		}
	}

	else
	{
		if (self.enabled == false)
		{
			self.enabled = true;
		}

		recorded = false;
		self.SetDestination(player.transform.position);
	}

}

else
{
	self.enabled = false;
}
```

</details>


https://github.com/user-attachments/assets/a87aede6-7b74-4057-8096-5e0a92113944


### Jump
Jump enemies commit to combat in the following steps: 
* The enemy approaches the player until they reach their engagement distance.
* The enemy casts a Ray at the player and records their last known position. A Rigidbody with frozen rotations is added if the component is not detected.
* Rigidbody force is applied both upwards and forwards, in the direction of the player, resulting in a high, fast-moving jump.
	* If they return to ground during the attack, a timer decrements down to zero. Reaching zero re-records the player's position and initiates another jump.
* If the distance between the Player and the enemy falls under its attack "limit", the action converts to a seeking attack using Vector3.Lerp.
  * Confirmed hits on a player inflicts damage to them and applies Rigidbody force. They enter an attack timeout for a set duration. The process repeats.
  * If they happen to be grounded during this "lock-on" stage for too long, they will re-record the player's position, restarting its jump.

<details>

<summary> snippet from ReplevinScript.cs </summary>

```csharp
if (!HaveIDied())
{
	self.speed = moveSpeed;
	distance = player.transform.position - transform.position;
	Vector3 rayOrigin = attackStartPoint.transform.position;
	RaycastHit hit, hitTheSequel;

	if (distance.magnitude <= meleeRangeMin && CanSeePlayer())
	{
		if (self.enabled == true)
		{
			self.ResetPath();
			self.enabled = false;
		}

		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

		if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin) && !recorded)
		{
			if (hit.collider.tag == "Player")
			{
				lastPlayerPosition = (hit.point - transform.position).normalized;
				recorded = true;

				if (gameObject.GetComponent<Rigidbody>() == null)
				{
					gameObject.AddComponent<Rigidbody>();
					gameObject.GetComponent<Rigidbody>().freezeRotation = true;
				}                      

				gameObject.GetComponent<Rigidbody>().AddForce((lastPlayerPosition + Vector3.up) * jumpForce, ForceMode.Impulse);
				gameObject.GetComponent<Rigidbody>().AddForce((transform.forward * forwardForce), ForceMode.Impulse);

			}

		}

		if(AmIGrounded())
		{
			airtimeShort -= Time.deltaTime;
			if (airtimeShort <= 0f)
			{
				airtimeShort = airtimeReset;
				if(lockOn)
				{
					lockOn = false;
				}

				recorded = false;
			}
		}

		else
		{
			airtimeShort = airtimeReset;                  
		}

		if (distance.magnitude <= jumpLimit)
		{
			lockOn = true;
		}

		if (lockOn && Time.timeScale == 1)
		{
			transform.position = Vector3.Lerp(transform.position, player.transform.position, gapClose);

			if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hitTheSequel, 2f))
			{
				if (hitTheSequel.collider.tag == "Player" && canAttackAgain)
				{

					if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
					{
						if (gameObject.GetComponent<DebuffScript>() == null)
						{
							gameObject.AddComponent<DebuffScript>();
						}

						if (gameObject.GetComponent<Rigidbody>() != null)
						{
							gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
							gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
						}

						slamTimeout = true;
						canAttackAgain = false;
					}

					else
					{
						hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
						hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

						Vector3 knockbackDir = transform.forward;
						knockbackDir.y = 0;
						hitTheSequel.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

						if (gameObject.GetComponent<Rigidbody>() != null)
						{
							gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
							gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
						}

						manager.damageDealt += damage;

						slamTimeout = true;
						canAttackAgain = false;
					}
				}
			}
		}             
	}

	else
	{
		if (self.enabled == false)
		{
			self.enabled = true;

			if (gameObject.GetComponent<Rigidbody>() != null)
			{
				gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
				gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

				Destroy(gameObject.GetComponent<Rigidbody>());
			}

		}

		recorded = false;
		airtimeShort = airtimeReset;
		self.SetDestination(player.transform.position);
	}
}

else
{
	self.enabled = false;
}
```

</details>


https://github.com/user-attachments/assets/20f8eb31-41c5-4a69-90d7-a1e48f3d94b5


### Range
Range enemies commit to combat in the following steps: 
* The enemy approaches the player until they reach their engagement distance.
* When in range, they record and move towards a position to their left, right, front, or back, relative to the player's position.
* In parallel, the enemy fires a volley of projectiles after a delay. Range enemies can attack and move simultaneously.
	* After the attack, they enter an attack timeout for a set duration. The process repeats.

<details>

<summary> snippet from ReplevinScript.cs </summary>

```csharp
if (distance.magnitude <= rangeEngagementDistance && CanSeePlayer())
{
	if (amSentry)
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);
	}

	else
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

		if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, rangeATKMin) && !recorded)
		{
			if (hit.collider.tag == "Player")
			{
				if (strafeTimer != strafeReset)
				{
					strafeTimer = strafeReset;
				} //Resets strafe timer

				self.ResetPath();

				int strafeAction = Random.Range(0, 4);
				if (strafeAction == 0)
				{
					strafeCalc = Vector3.Cross(distance, transform.up);
					strafePos = transform.position + strafeCalc * strafeDistance;
				} //Strafes to the right

				else if (strafeAction == 1)
				{
					strafeCalc = Vector3.Cross(distance, -transform.up);
					strafePos = transform.position + strafeCalc * strafeDistance;
				} //Strafes to the left

				else if (strafeAction == 2)
				{
					strafePos = transform.position + distance * strafeDistance / 2;
				} //Moves forward

				else
				{
					strafePos = transform.position - distance * strafeDistance;
				} //Moves backwards

				Vector3 strafeDirection = strafePos - transform.position;
				if (!Physics.Raycast(rayOrigin, strafeDirection.normalized, out hit, strafeDirection.magnitude, contactOnly))
				{
					self.SetDestination(strafePos);
					recorded = true;
				}
			}
		}

		lastKnownDistance = strafePos - transform.position;

		//Enemy resets recorded state when within previous strafe position for a duration
		if (lastKnownDistance.magnitude <= strafeLimit)
		{
			strafeTimer -= Time.deltaTime;
			if (strafeTimer <= 0f)
			{
				recorded = false;
			}
		}

	}

	if (!attackLock)
	{
		if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, rangeEngagementDistance, contactOnly))
		{
			if (hit.collider.tag == "Player")
			{
				//Selects a random duration to delay next attack, then initiates attack
				int randomTime = 0;

				float[] delays = { 1f, 1.25f, 1.5f, 2f };
				randomTime = Random.Range(0, delays.Length);
				rangeCooldown = delays[randomTime];

				StartCoroutine(RangeAttackShot()); //RangeAttackShot() fires a projectile volley
				attackLock = true;
			}

			if (hit.collider.tag == "Enemy")
			{
				Task.current.Fail();
			}
		}
	}

	//Delays attack behavior until timer expires
	if (attackLock && rangeTimeout)
	{
		attackAgain += Time.deltaTime;
		if (attackAgain >= rangeCooldown)
		{
			attackAgain = 0.0f;
			attackLock = false;
			rangeTimeout = false;
		}
	}
}

else
{
	if (amSentry)
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);
	}

	else
	{
		recorded = false;
		self.SetDestination(player.transform.position);
	}
}
```

</details>


https://github.com/user-attachments/assets/6d3d6c00-f491-49ad-8a73-5314971e79f5


### Melee
Melee enemies commit to combat in the following steps: 
* When within engagement distance from the player, an attack timer decrements to zero.
	* While their attack is not locked in, they constantly record and move towards a strafing position to their left or right relative to the player's position. 
 	* Moving out of range forces them towards the player with the option to strafe left or right again.
  	* When the attack timer reaches zero, their attack is locked in.
* When the attack locks in, the enemy approaches for a physical attack until their timeout duration reaches zero or if they hit the player successfully.
	* Confirmed hits or zeroed timers end the attack and randomly select a new attack timer duration. The process repeats.

<details>

<summary> snippet from ReplevinScript.cs </summary>

```csharp
if (distance.magnitude <= meleeEngagementDistance)
{
	transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

	meleeAttackTimer -= Time.deltaTime;
	if (meleeAttackTimer <= 0f)
	{
		meleeAttackTimer = 0f;
		attackLock = true;
	}
}

if (!attackLock)
{
	if (distance.magnitude <= meleeRangeCheck)
	{
		if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin) && !recorded)
		{
			if (hit.collider.tag == "Player")
			{
				self.ResetPath();

				if (strafeRight)
				{
					strafeCalc = Vector3.Cross(distance, transform.up);
					strafePos = transform.position + strafeCalc * strafeDistance;
				}

				else if (strafeLeft)
				{
					strafeCalc = Vector3.Cross(distance, -transform.up);
					strafePos = transform.position + strafeCalc * strafeDistance;
				}

				self.SetDestination(strafePos);
				recorded = true;
			}
		}
	}

	else
	{
		strafePos = transform.position + distance * strafeDistance / 2;
		self.SetDestination(strafePos);

		int strafeAction = Random.Range(0, 2);
 		if (strafeAction == 0)
		{
			strafeRight = true;
			strafeLeft = false;
		}

		else
		{
			strafeLeft = true;
			strafeRight = false;
		}

		recorded = true;
	}

	lastKnownDistance = strafePos - transform.position;

	//Enemy resets recorded state when within previous strafe position for a duration
	if (lastKnownDistance.magnitude <= strafeLimit)
	{
		recorded = false;
	}
}

else
{
	meleeTimeout -= Time.deltaTime;

	if (meleeTimeout > 0f)
	{
		self.SetDestination(player.transform.position);

		if (!GetComponent<BerthScript>())
		{
			subject.materials[materialIndex].color = attackTell;
		}

		if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, 1.25f))
		{
			if (hit.collider.tag == "Player")
			{
				if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
				{
					if (gameObject.GetComponent<DebuffScript>() == null)
					{
						gameObject.AddComponent<DebuffScript>();
					}

					if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat)
					{
						hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;
					}
                                            
				}				

				else
				{
					hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
					hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

					//This code shoves the Player with particular force in their opposite direction.
					//This is a melee attack, shoving the player with less force, subtly offsetting the player upwards to distinguish it from a charge.
					Vector3 knockbackDir = -hit.collider.transform.forward;
					hit.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

					manager.damageDealt += damage;
				}

				//Selects a random duration to delay next attack, then initiates attack
				int randomTime = 0;

				float[] delays = { 5f, 7f, 10f };
				randomTime = Random.Range(0, delays.Length);
				meleeReset = delays[randomTime];

				meleeAttackTimer = meleeReset;
				if (recorded)
				{
					recorded = false;
				}

				if (!GetComponent<BerthScript>())
				{
					subject.materials[materialIndex].color = Color.red;
				}

				meleeTimeout = 0f;
				attackLock = false;
			}
		}
	}

	else
	{
		//Selects a random duration to delay next attack, then initiates attack
 		int randomTime = 0;

		float[] delays = { 5f, 7f, 10f };
		randomTime = Random.Range(0, delays.Length);
		meleeReset = delays[randomTime];

		meleeAttackTimer = meleeReset;
		if (recorded)
		{
			recorded = false;
		}

		if (!GetComponent<BerthScript>())
		{
			subject.materials[materialIndex].color = Color.red;
		}

		meleeTimeout = meleeCooldown;
		attackLock = false;
	}
}
```

</details>


https://github.com/user-attachments/assets/a2dd9704-0662-46ba-927d-9867e237c565



