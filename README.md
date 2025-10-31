Welcome!

Current Version: MVP 0.1.6 (10/27/2025)

# Contents:

## Details
* [Game Description](#game-description)
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

## Details
### Game Description
All That Glitters is Gold (ATGIG) is a third-person Action game which combines high-density Horde combat, Looter-shooter attributes, and gunplay to craft a lite experience that aspires to whet the "Power-Fantasy" appetite.
### Responsibilities
I am solo-developing All That Glitters is Gold. As such, I am responsible for:
* Player abilities (movement, evasion, melee & sprinting)
* Player systems (Slope Traversal and Anti-camera clipping)
* Weapon attributes (type, damage, rate of fire, etc.)
* Weapon augmentations (rarity, cheats)
* Weapon saving
* Enemy attack types (charge, pounce, jump, etc.)
* Gametypes (Viricide)
* Out-of-gameplay systems (Inventory Management, Weapon Kiosk)

Bulleted below are detailed accounts of ATGIG's most notable pursuits, accompanied by visuals and organized by category. 

## Pursuits
### Player
#### Camera Clipping Countermeasure
To prevent instances of the Player's Camera clipping through walls, a Ray is cast starting from the Player-Character's rear and ending at the Camera position. When a surface intersects with the end point, the Camera is assigned an offset position, comprised of the Raycast hit point, oriented to the surface's Normal direction. Settings are in place to tune the offset further Horizontally or Vertically, and/or to increase the strength of this effect through multiplication. This approach has shown to be effective in reducing occurences of wall clipping (barring extreme cases), and is also performative when handling corners. 
```csharp
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
                    playerCamera.transform.position = offset * offsetMult;

                    Debug.DrawRay(offsetCheckPos.transform.position, (playerCamera.transform.position - offsetCheckPos.transform.position).normalized * collideCheck, Color.yellow);
                    Debug.DrawLine(hit.point, offset * offsetMult, Color.red);

                }
            }                  
        }
```
![ezgif-6fdf065644ca79](https://github.com/user-attachments/assets/d182ec60-ce5a-430e-99cd-1730825e1ea6)

#### Slope Traversal
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
![ezgif-3227e372bb6e1c](https://github.com/user-attachments/assets/79f54600-81d4-472c-a9a2-5896ed81e2aa)

### Weapons
#### Weapon Saving
ATGIG's Weapon Saving system uses Stream reading and Stream writing to both respawn and catalog Weapons held in a Player's inventory. Upon level reset by defeat, pause menu restart, or main menu visit, the Player's Inventory records attributes of its Weapons and saves them to a file as a string that is between seven to nine characters in length. Each value within this string represents a Weapon's constituent parts. For example:

12014581

* <ins>1</ins>2014581 - Denotes Weapon type.
* 1<ins>2</ins>014581 - Denotes Weapon rarity.
* 12<ins>0</ins>14581 - Denotes Weapon Exotic property; It is either Exotic (1) or not (0).
* 120<ins>1</ins>4581 - Denotes Statistical Cheat #1.
* 1201<ins>4</ins>581 - Denotes Statistical Cheat #2.
* 12014<ins>5</ins>81 - Denotes Statistical Cheat #3.
* 120145<ins>8</ins>1 - Denotes Statistical Cheat #4.
* 1201458<ins>1</ins> - Denotes Functional Cheat

The above string describes a Weapon with these properties: 
* Full Fire Rifle (1)
* Rarity 2 (2)
* Non-exotic (0)
* Statistical Cheats:
	*  Deep Yield (1)
 	*  Deeper Stores (4)
  	*  Far Sight (5)
  	*  Hastier Hands (8)
* Functional Cheat:
	* Efficacy (1)

String length for Weapons depends on that weapon's rarity. Rarity 1 weapons do not receive Functional Cheats. For example, "3102367" represents the following traits: 
* Pistol (3)
* Rarity 1 (1)
* Non-exotic (0)
* Statistical Cheats:
	* Deeper Yield (2)
 	* Deep Stores (3)
  	* Farther Sight (6)
  	* Hasty Hands (7)
 
Another example, "440246863", details this Weapon's features: 
* Semi Fire Rifle (4)
* Rarity 4 (4)
* Non-exotic (0)
* Statistical Cheats: 
	* Deeper Yield (2)
	* Deeper Stores (4)
 	* Farther Sight (6)
  	* Hastier Hands (8)
* Functional Cheats:
	* Positive-Negative (6)
 	* Rude Awakening (3)

When a game starts, the "WeaponManager" finds the inventory file, titled "inventory.txt", and reads its contents, creating new Weapons with the recorded characteristics attached.

In its earliest form, it only recorded Weapons as strings that were eight characters long, as each Weapon was designed to have the same structure. Moreover, the system often delivered the Weapons back to the inventory out of the order in which they were added. Playtests revealed that, primarily due to the lacking strength of Functional Cheats at the time, that Weapons required a fundamental change in power. For MVP 0.1.3, Weapons could now possess double Functional Cheats, and the Weapon Saving system was upgraded to handle this new case, along with handling cases where Rarity 1 weapons lacked Functional Cheats at all. For MVP 0.1.5, the "WeaponManager" was upgraded to use a Coroutine for Weapon respawns, delaying the spawn of a new Weapon for a short time. This naturally fixed the issue of disorderly Weapon returns to the inventory. Currently, this system can handle small and large inventory sizes, some sizes beyond 100, and can return Weapons to the Player with no issue.
```csharp
//From PlayerInventoryScript.cs
CreateInventoryFile(); //If "inventory.txt" doesn't exist, it makes a new file. Otherwise, it overwrites the existing file. 

using (StreamWriter write = new StreamWriter(filepath))
{
	if (inventory.Count > 0)
	{
		for (int i = 0; i < inventory.Count; i++)
		{
			if (inventory[i].name == "testFullFireRifle" || inventory[i].name == "testFullFireRifle_Exotic")
			{
				write.Write("1");
				if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
				//Writes "1" and so on, up to 5. 

				if (inventory[i].GetComponent<FirearmScript>().isExotic == true)
				//Writes "1". Otherwise, writes "0". 

				if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
				{
					//Statistical Cheat process
					if (inventory[i].GetComponent<DeepYield>())
					{
						write.Write("1");
					}

					if (inventory[i].GetComponent<DeeperYield>())					
					//Writes "2" and so on, up to 8. 7-8 use WriteLine statements as they conclude recording.
				}

				if(inventory[i].GetComponent<FirearmScript>().weaponRarity == 2 || inventory[i].GetComponent<FirearmScript>().weaponRarity == 3)
				{
					//Statistical Cheat process, followed by:
					if (inventory[i].GetComponent<WaitNowImReady>())
					{
						write.WriteLine("0");
					}

					if (inventory[i].GetComponent<Efficacy>())
					//Writes "1" and so on, up to 9. Newer Cheats use special characters: (!, @, #, $, %, and ^)
				}

				if (inventory[i].GetComponent<FirearmScript>().weaponRarity >= 4)
				{
					//Statistical Cheat process, followed by:
					if (inventory[i].GetComponent<FirearmScript>().isExotic == true)
					{
						if (inventory[i].GetComponent<EquivalentExchange>())
						{
                                    write.Write("A");
						}
						//If Weapon is Exotic, letters A-G are used.

						if (inventory[i].GetComponent<WaitNowImReady>())
						{
                                    write.WriteLine("0");
						}
					}

					else
					{
						//The next character comes from a pool between (9, 4, 5, 6, 8, !, @, or #)
						if (inventory[i].GetComponent<AllElseFails>())
						{
							write.Write("9");
						}

						//the last character comes from a pool between (0, 1, 2, 7, 3, $, %, or ^)
						if (inventory[i].GetComponent<Cadence>())
						{
							write.WriteLine("7");
						}
					}
				}
			}
			//This is repeated for all other Weapon types
		}
	}
}
```
```csharp
//From WeaponManagerScript.cs
string c = "Comic Sans"; //Initializing a string.

for (int s = 0; s < player.readdedWeps.Count; s++)
{
	c = player.readdedWeps[s];
	wep = c[0];
	wepStr = wep.ToString();

	rar = c[1];
	rarStr = rar.ToString();

	exo = c[2];
	exoStr = exo.ToString();

	cOne = c[3];
	cOneStr = cOne.ToString();

	cTwo = c[4];
	cTwoStr = cTwo.ToString();

	cThree = c[5];
	cThreeStr = cThree.ToString();

	cFour = c[6];
	cFourStr = cFour.ToString();

	if(player.readdedWeps[s].Length == 8)
	{
		cFive = c[7];
		cFiveStr = cFive.ToString();
	}

	if(player.readdedWeps[s].Length == 9)
	{
		cFive = c[7];
		cFiveStr = cFive.ToString();

		cSix = c[8];
		cSixStr = cSix.ToString();
	}

	if (wepStr == "1")
	{
		GameObject item = Instantiate(weapons[0], transform.position, transform.rotation);
		item.name = weapons[0].name;

		if (rarStr == "1")
		{
			item.GetComponent<FirearmScript>().weaponRarity = 1;
		}
		//And so on, assigning rarities up to 5 for weapons.

		if (cOneStr == "1")
		//Adds the component, "Deep Yield". Adds "Deeper Yield" if cOneStr is "2".

		if (cTwoStr == "3")
		//Adds the component, "Deep Stores". Adds "Deeper Stores" if cTwoStr is "4".

		if (cThreeStr == "5")
		//Adds the component, "Far Sight". Adds "Farther Sight" if cThreeStr is "6".

		if (cFourStr == "7")
		//Adds the component, "Hasty Hands". Adds "Hastier Hands" if cFourStr is "8".

		if (player.readdedWeps[s].Length == 8)
		{
			if (cFiveStr == "0")
			{
				item.GetComponent<FirearmScript>().cheatRNG = 450;
				item.AddComponent<WaitNowImReady>();

				item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procOne;
				item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
			}
			//And so on, up to 9, or special characters: (!, @, #, $, %, or ^)
		}

		if (player.readdedWeps[s].Length == 9)
		{
			if (cFiveStr == "A")
			//Exotic Cheats are recorded between A-G. In this case, "A" adds Exotic Functional Cheat "Equivalent Exchange". 

			//Otherwise, the next component comes from a pool between (9, 4, 5, 6, 8, !, @, or #)
			if (cFiveStr == "5")
			{
				item.GetComponent<FirearmScript>().fcnChtOne = 425;
				item.AddComponent<MaliciousWindUp>();
				item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
			}

			//The last component comes from a pool between (0, 1, 2, 7, 3, $, %, or ^)
			if (cSixStr == "2")
			{
				item.GetComponent<FirearmScript>().fcnChtTwo = 505;
				item.AddComponent<Inoculated>();
				item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;
			}
		}

		yield return new WaitForSeconds(spawnDelayTimer);
	}
	//This is repeated for all other Weapon types
}
```
https://github.com/user-attachments/assets/870869e0-f4c6-422f-ba14-e0012f1fc8d5

#### Cheats
Cheats are ATGIG's core system, everpresent in and out of gameplay, and primary contributor to this game's "Power-Fantasy" goal. Deeper explanations on what they specifically do can be found on the [Cheats](Core_cheats.md) file.

Cheats are applied to Weapons through Random Number Generation (RNG). The moment a Weapon is created, several methods are called to choose and apply what are known as Statistical Cheats and Functional Cheats: 
* Statistical Cheats permanently upgrade a Weapon's base performance metrics, excluding base damage.
* Functional Cheats extend a Weapon's offensive, neutral, or defensive potential through conditional triggers.

A number is randomized between a set range. The chosen Cheat is determined by what range this value sits within. All cheats are divided into distinct pools: 
* Yields (Stat Cheats that increase active magazine sizes)
* Stores (Stat Cheats that increase max ammunition reserves)
* Sights (Stat Cheats that increase effective ranges)
* Hands (Stat Cheats that increase reload speeds)
* Functional Cheats
	* Rarity 1 Weapons cannot roll Cheats of this kind.
 	* Rarity 2-3 Weapons have access to one pool of all 16 Functional Cheats
  	* Rarity 4-5 Weapons have access to two pools of eight Functional Cheats

For both Cheat types, the defined ranges are usually within units of 50 (ex. 50-100), up until Rarity 4-5, when the defined ranges are limited to units of 10 (ex. 410-420) for Functional Cheats only. This system is performative, and has yet to produce instances of two of the same Cheat being generated for a Weapon. Cheats are also not weighted to generate more often than others; Every Cheat has a fair chance to be generated. Exotics are curated Weapons, and do not require random Cheat generation. Weapons being reproduced by the WeaponManager have Cheats directly added based on characters identifying its components, and also do not require Cheat generation.  
```csharp
public virtual void AmmoCheats()
{
        if (isExotic == true)
		//Exotics generate the best Cheat variant and leave the method.
		return;

        if (saved == true)
        //Weapons made by the WeaponManager add Cheats directly, so there is no need to generate.
		return;

        ammoCheatOne = Random.Range(0, 101);
        ammoCheatTwo = Random.Range(100, 201);

        if(ammoCheatOne <= 50)
        //Adds the component, "Deep Yield". 

        if (ammoCheatOne > 50)
        //Adds the component, "Deeper Yield". 

        if (ammoCheatTwo <= 150)
        //Adds the component, "Deep Stores". 

        if (ammoCheatTwo > 150)
        //Adds the component, "Deeper Stores".      

}
//Other Statistical Cheat methods operate identically.
//...
public virtual void CheatGenerator()
{
	if(isExotic == true)
	{
		//Exotic weapons use negative numbers to denote what Functional Cheats to receive.
 		cheatRNG = cheatOverride;
		if(cheatRNG == -1)
		//Adds Cheats, "Equivalent Exchange" + "Wait! Now I'm Ready!"

		if(cheatRNG == -2)
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
		fcnChtOne = Random.Range(400, 481);
		if(fcnChtOne <= 410)
		{
			gameObject.AddComponent<AllElseFails>();
			gameObject.GetComponent<AllElseFails>().proc = procOne;
		}

		if (fcnChtOne > 410 && fcnChtOne <= 420)
		//Adds Cheat "Not With a Stick", and so on, with last range between (471 and 480)

		//Pool #2
		fcnChtTwo = Random.Range(480, 561);
		if (fcnChtTwo <= 490)
		{
			gameObject.AddComponent<WaitNowImReady>();
			gameObject.GetComponent<WaitNowImReady>().proc = procTwo;
	
		}

		if (fcnChtTwo > 490 && fcnChtTwo <= 500)
		//Adds Cheat "Efficacy" and so on, with last range between (551 and 560)
	}
}
```

### Enemy Attacks
#### Pounce
Enemies that are assigned the Pounce attack style commit to combat in the following steps: 
* Once the Player is both seen and the distance (the range between a Player and this enemy) is less or equal to this enemy's attack range, their NavMesh destination is cleared.
* The enemy casts a Ray at the Player and records their last known position. 
* After recording, the enemy employs Vector3.Lerp and dashes towards that position at high speed. A Ray is cast for the duration of this movement to check for Player detection.
	* Another distance value between the recorded position and the enemy is being tracked and compared to their attack stopping distance. If that distance falls underneath their stopping point, their attack finishes.
 	* Contact with a Player inflicts damage to them, applied Rigidbody force, and their attack finishes.

 After attacking, they enter a one-second cooldown in which they cannot move. Upon conclusion, they can commit this attack again.
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
https://github.com/user-attachments/assets/346d3b90-27a8-454b-993a-9a2c9669f5f8

#### Jump
Enemies that are assigned the Jump attack style commit to combat in the following steps: 
* Once the Player is both seen and the distance from one another is less or equal to this enemy's attack range, their NavMesh destination is cleared, and their Agent component is disabled.
* The enemy casts a Ray at the Player and records their last known position. A Rigidbody with frozen rotations is added if the component is not detected.
* Rigidbody force is then both applied upwards, in the direction of their last recorded Player position, and forwards, resulting in a high, fast-moving jump.
	* If they return to ground during this attack, a timer decrements, down to zero, which at that point they re-record the Player's position and initiate another jump.
 	* If the distance between the Player and the enemy falls under its attack "limit", the action converts to a guided attack using Vector3.Lerp. They cast a Ray to check for Player detection at this stage.
  		* Contact with a Player inflicts damage to them, applied Rigidbody force, and their attack finishes.
    	* If they are grounded during this "lock-on" stage, the logic responsible for resetting Player position recordings after a short time will occur, permitting another jump.
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
https://github.com/user-attachments/assets/415b9358-cd5a-4b24-a67d-689109c7b7ad
