Welcome!

Current Version: MVP 0.1.6 (10/27/2025)

## Contents:

## Details
* [Game Description](#game-description)
* [Responsibilities](#responsibilities)

## Pursuits
* Player
	* [Camera Clipping Countermeasure](#camera-clipping-countermeasure)
	* [Slope Traversal](#slope-traversal)
 	* Melee Attacks
  	* Evasion & Sprinting
* Weapons
	* Weapon Saving
 		* Orderly Weapon Spawning
   	* Statistical Cheats
	* Functional Cheats
 		* "Wait! Now I'm Ready!"
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
 	* Aim Assist
  	* Performance
  		* Rarity
  	* Double Functional Cheats
* Enemies
	* Behavior
	* Attacks
 		* Melee
   			* Standard
      		* Bosses
        * Range
        * Charge
        	* Standard
         	* Bosses  
		* Pounce
		* Jump
* Game
	* Viricide
    * Inventory Page
    * Requisitions Page





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

Bulleted below are detailed accounts of ATGIG's notable pursuits, accompanied by visuals and organized by category. 

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
                {
                    playerRigid.AddForce(Vector3.up * slopeForce);
                }
            }

            else if (Vector3.Dot(hit.normal, transform.forward) > 0)
            {
                if (vertInput < 0)
                {
                    playerRigid.AddForce(Vector3.up * slopeForce);
                }

                else
                {
                    playerRigid.AddForce(-Vector3.up * slopeForce);
                }
            }
        
            //Handles Horizontal slope traversal
            if (Vector3.Dot(sideVector, transform.forward) > 0)
            {
                if (horizInput > 0)
                {
                    playerRigid.AddForce(-Vector3.up * slopeForce);
                }

                else
                {
                    playerRigid.AddForce(Vector3.up * slopeForce);
                }
            }

            else if (Vector3.Dot(sideVector, transform.forward) < 0)
            {
                if (horizInput < 0)
                {
                    playerRigid.AddForce(-Vector3.up * slopeForce);
                }

                else
                {
                    Debug.Log("To the Right");
                    playerRigid.AddForce(Vector3.up * slopeForce);
                }
            }
        }     
```
![ezgif-3227e372bb6e1c](https://github.com/user-attachments/assets/79f54600-81d4-472c-a9a2-5896ed81e2aa)
