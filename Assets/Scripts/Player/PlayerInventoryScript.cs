using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerInventoryScript : MonoBehaviour
{
    //Optional
    public string filepath = "inventory.txt";

    public int lucentFunds;
    public int fogGrenadeCharges = 0; //number of Fogger Grenades
    public int solGrenadeCharges = 0; //number of Solution Grenades
    public int desGrenadeCharges = 0; //number of Destruct Grenades
    public List<GameObject> inventory = new List<GameObject>(10); //list of Weapons
    public List<GameObject> grenades = new List<GameObject>(2); //list of Grenades
    public Transform gunPlace; //Anchor position that Weapons are assigned to
    public float throwStrength;
    public float throwMin, throwMax;
    public bool blueKey, redKey = false; //confirms ownership of colored key if true
    public GameObject bKey, rKey; //objects that visually convey key ownership

    //weaponCurAmmo - text that displays Weapon's current ammo
    //weaponResAmmo - text that displays Weapons' reserve ammo
    public Text weaponCurAmmo, weaponResAmmo, lucentText, grenadeText;

    //weaponLoad - slider that represents Weapon reload progress
    //grenadeThrow - slider that represent Grenade throw strength
    public Slider weaponLoad, grenadeThrow;
    public Image weaponAmmoPage;

    private Image weaponPage; //In-game Inventory page
    internal Image reticleSprite;
    private Text wepName, wepStats, flavor;
    private Text cheatOne, cheatTwo, cheatThree, cheatFour, cheatTraitOne, cheatTraitTwo;

    //invMonitor - text that displays inventory position/total inventory size
    //rarityCheck - text that displays Weapon rarity
    private Text invMonitor, rarityCheck, dismantleText;
    internal int selection, grenadeSelection; //index values used to select Weapons/Grenades
    private float dismantleTimer = 1f;
    private float dismantleTimerReset;
    internal float wepStateTimer = 0.5f; //Time before hiding Weapon state canvas
    internal float wepStateTimerReset;
    internal bool throwing = false; //Player is in active throwing state if true
    internal bool fulminatePresent; //Weapon with Fulminate is present if true
    internal bool fulminateFated; //Rarity 5 Weapon with Fulminate is present if true
    internal int fulminateBuff; //damage value used to increase Destruct Grenade damage by Fulminate
    internal bool enshroudPresent; //Weapon with Enshroud is present if true

    internal List<string> readdedWeps = new List<string>(10); //List of Weapons (in string form)
    
    // Start is called before the first frame update
    void Start()
    {      

        selection = -1;
        grenadeSelection = 0;

        weaponPage = GameObject.Find("weaponPagePanel").GetComponent<Image>();
        wepName = GameObject.Find("weaponName").GetComponent<Text>();
        wepStats = GameObject.Find("weaponStats").GetComponent<Text>();
        flavor = GameObject.Find("flavorText").GetComponent<Text>();
        cheatOne = GameObject.Find("weaponCheat (1)").GetComponent<Text>();
        cheatTwo = GameObject.Find("weaponCheat (2)").GetComponent<Text>();
        cheatThree = GameObject.Find("weaponCheat (3)").GetComponent<Text>();
        cheatFour = GameObject.Find("weaponCheat (4)").GetComponent<Text>();
        cheatTraitOne = GameObject.Find("weaponCheat (5)").GetComponent<Text>();
        cheatTraitTwo = GameObject.Find("weaponCheat (6)").GetComponent<Text>();
        invMonitor = GameObject.Find("invMonitor").GetComponent<Text>();
        rarityCheck = GameObject.Find("weaponRarity").GetComponent<Text>();
        dismantleText = GameObject.Find("deletingText").GetComponent<Text>();
        reticleSprite = GameObject.Find("reticleImage").GetComponent<Image>();

        dismantleText.text = " ";
        weaponPage.gameObject.SetActive(false);
        reticleSprite.gameObject.SetActive(false);
        dismantleTimerReset = dismantleTimer;
        weaponLoad.value = 0;
        grenadeThrow.value = 0;

        lucentFunds = PlayerPrefs.GetInt("lucentBalance");
        if(lucentFunds >= 100000)
        {
            lucentFunds = 100000;
        }

        weaponAmmoPage.gameObject.SetActive(false);
        //weaponLoad.gameObject.SetActive(false);
        lucentText.gameObject.SetActive(false);
        grenadeText.gameObject.SetActive(false);
        wepStateTimerReset = wepStateTimer;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(selection + " || " + inventory.Count);
        //Debug.Log(grenadeSelection + " || " + grenades.Count);

        //Enables use of Weapon switching, destruction, Grenade switching and throwing while game is active
        if(Time.timeScale == 1)
        {
            SwitchInv();
            DismantleInv();
            SwitchGrenades();
            ThrowGrenade();
        }

        //Hides Weapon ammo page after timer reaches zero
        wepStateTimer -= Time.deltaTime;
        if (wepStateTimer <= 0f)
        {
            weaponAmmoPage.gameObject.SetActive(false);
            //weaponLoad.gameObject.SetActive(false);
            lucentText.gameObject.SetActive(false);
            grenadeText.gameObject.SetActive(false);
        }

        //WriteOnReset();
        //WriteInventory();

        //Reveals Inventory page if game is running and at least one Weapon is in inventory
        if (Input.GetKeyDown(KeyCode.UpArrow) && Time.timeScale != 0)
        {
            if(inventory.Count <= 0)
            {
                //Debug.Log("Cannot open weapon page; no weapon in inventory");
            }

            else
            {
                weaponPage.gameObject.SetActive(true);              
            }         
        }

        //Hides Inventory page if game is running
        if(Input.GetKeyDown(KeyCode.DownArrow) && Time.timeScale != 0)
        {
            weaponPage.gameObject.SetActive(false);
        }

        //Displays Weapon ammo count, statistics, reload progress, Lucent balance, and selected Grenade
        if(inventory.Count > 0)
        {
            weaponCurAmmo.text = inventory[selection].GetComponent<FirearmScript>().currentAmmo.ToString();
            weaponResAmmo.text = inventory[selection].GetComponent<FirearmScript>().reserveAmmo.ToString();
            weaponLoad.maxValue = inventory[selection].GetComponent<FirearmScript>().reloadSpeed;

            if (inventory[selection].GetComponent<FirearmScript>().isReloading == true)
            {
                weaponLoad.value += Time.deltaTime;
            }

            if(inventory[selection].GetComponent<FirearmScript>().isReloading == false)
            {
                weaponLoad.value = 0;
            }

            wepName.text = inventory[selection].name;

            //Stats for Single Fire or Semi Fire Firearms
            if (inventory[selection].GetComponent<SingleFireFirearm>() != null || inventory[selection].GetComponent<SemiFireFirearm>() != null)
            {
                wepStats.text = "Damage: " + inventory[selection].GetComponent<FirearmScript>().damage.ToString() + "\n" +
                          "Reload Speed: " + inventory[selection].GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                          "Effective Range " + inventory[selection].GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                          "Total Range: " + inventory[selection].GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                          "Magazine: " + inventory[selection].GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                          "Max Reserves: " + inventory[selection].GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                          "Rate of Fire: " + Mathf.Round(inventory[selection].GetComponent<FirearmScript>().fireRate * 1000).ToString() + " RPM";
            }

            //Stats for Shotguns
            else if(inventory[selection].GetComponent<ShotgunFirearm>() != null)
            {
                wepStats.text = "Damage: " + inventory[selection].GetComponent<FirearmScript>().damage.ToString() + "\n" +
                          "Reload Speed: " + inventory[selection].GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                          "Effective Range " + inventory[selection].GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                          "Total Range: " + inventory[selection].GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                          "Magazine: " + inventory[selection].GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                          "Max Reserves: " + inventory[selection].GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                          "Rate of Fire: " + Mathf.Round(inventory[selection].GetComponent<FirearmScript>().fireRate * 100).ToString() + " RPM";
            }

            //Stats for all other Firearms
            else
            {
                wepStats.text = "Damage: " + inventory[selection].GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + inventory[selection].GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + inventory[selection].GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + inventory[selection].GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + inventory[selection].GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + inventory[selection].GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(inventory[selection].GetComponent<FirearmScript>().fireRate * 10000).ToString() + " RPM";
            }

            flavor.text = inventory[selection].GetComponent<FirearmScript>().flavorText;
            
            //Text for Rarities
            if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 1)
            {
                rarityCheck.text = "Usual";
                weaponPage.color = Color.grey;
            }

            if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 2)
            {
                rarityCheck.text = "Sought";
                weaponPage.color = Color.green;
            }

            if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 3)
            {
                rarityCheck.text = "Coveted";
                weaponPage.color = Color.red;
            }

            if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 4)
            {
                rarityCheck.text = "Treasured";
                weaponPage.color = Color.yellow;
            }

            if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
            {
                rarityCheck.text = "Fated";
                weaponPage.color = Color.cyan;

                if(inventory[selection].GetComponent<FirearmScript>().isExotic == true)
                {
                    rarityCheck.text = "Exotic";
                    weaponPage.color = Color.Lerp(Color.cyan, Color.white, Mathf.PingPong(Time.time, 0.9f));
                }
            }

            //Text for [active item / inventory size]
            invMonitor.text = (selection + 1) + " / " + inventory.Count;

            DisplayCheats();
        }

        if(grenadeSelection == 0)
        {
            grenadeText.text = "Fog: " + fogGrenadeCharges;
        }

        if (grenadeSelection == 1)
        {
            grenadeText.text = "Sol: " + solGrenadeCharges;
        }

        if (grenadeSelection == 2)
        {
            grenadeText.text = "Des: " + desGrenadeCharges;
        }

        lucentText.text = lucentFunds.ToString("N0");

        if(fogGrenadeCharges >= 3)
        {
            fogGrenadeCharges = 3;
        }

        if(solGrenadeCharges >= 3)
        {
            solGrenadeCharges = 3;
        }

        if (desGrenadeCharges >= 3)
        {
            desGrenadeCharges = 3;
        }
    }

    /// <summary>
    /// Adds and enables Weapon to Inventory if previously empty. 
    /// Adds and disables newly added Weapons if Inventory has at least one Weapon
    /// </summary>
    /// <param name="g">Weapon being added to Inventory</param>
    public void AddInv(GameObject g)
    {
        if(inventory.Count <= 0)
        {
            inventory.Add(g);            

            g.transform.SetParent(gunPlace, true);
            g.transform.position = gunPlace.transform.position;
            g.transform.rotation = gunPlace.transform.rotation;
            g.GetComponent<FirearmScript>().enabled = true;
            reticleSprite.gameObject.SetActive(true);
            reticleSprite.sprite = g.GetComponent<FirearmScript>().reticle;
            g.GetComponent<BoxCollider>().isTrigger = false;
            g.GetComponent<BoxCollider>().enabled = false;
            selection++;          

            return;
        }

        if(inventory.Count >= 1)
        {          
            inventory.Add(g);

            g.transform.SetParent(gunPlace, true);
            g.transform.position = gunPlace.transform.position;
            g.transform.rotation = gunPlace.transform.rotation;
            g.GetComponent<FirearmScript>().enabled = false;
            g.GetComponent<BoxCollider>().isTrigger = false;
            g.GetComponent<BoxCollider>().enabled = false;
            g.gameObject.SetActive(false);

            return;
        }          
    }

    /// <summary>
    /// Switches Weapons dependent on Left, Right arrow inputs
    /// </summary>
    private void SwitchInv()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Prevents switching when inventory is empty
            if(selection <= -1)
            {
                //Debug.Log("Cannot switch weapons");
                return;
            }
           
            //Prevents switching when only one item in inventory
            if(selection <= 0 && inventory.Count <= 1)
            {
                //Debug.Log("Cannot switch weapons");
                return;
            }

            //selection--;

            //Activates the Weapon at the end of the Inventory if you switch at the front. Otherwise, index goes left.
            if (selection <= 0 && inventory.Count >= 1)
            {
                selection = inventory.Count - 1;

                inventory[selection].GetComponent<FirearmScript>().enabled = true;
                reticleSprite.sprite = inventory[selection].GetComponent<FirearmScript>().reticle;
                inventory[selection].gameObject.SetActive(true);

                inventory[0].GetComponent<FirearmScript>().enabled = false;
                inventory[0].gameObject.SetActive(false);

                weaponAmmoPage.gameObject.SetActive(true);
                lucentText.gameObject.SetActive(true);
                wepStateTimer = wepStateTimerReset;
                return;

            }

            else
            {
                selection--;

                //Prevents running off the inventory backwards
                //if (selection <= -1 && inventory.Count >= 1)
                //{
                //    selection = 0;
                //}

                inventory[selection].GetComponent<FirearmScript>().enabled = true;
                reticleSprite.sprite = inventory[selection].GetComponent<FirearmScript>().reticle;
                inventory[selection].gameObject.SetActive(true);

                inventory[selection + 1].GetComponent<FirearmScript>().enabled = false;
                inventory[selection + 1].gameObject.SetActive(false);

                weaponAmmoPage.gameObject.SetActive(true);
                lucentText.gameObject.SetActive(true);
                wepStateTimer = wepStateTimerReset;
                return;
            }

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Prevents switching when inventory is empty
            if (selection <= -1)
            {
                //Debug.Log("Cannot switch weapons");
                return;
            }

            //Prevents switching when only one item in inventory
            if (selection <= 0 && inventory.Count <= 1)
            {
                //Debug.Log("Cannot switch weapons");
                return;
            }

            //Activates the Weapon at the front of the Inventory if you switch at the end. Otherwise, index goes right.
            if (selection >= inventory.Count - 1 && inventory.Count >= 1)
            {
                selection = 0;

                inventory[selection].GetComponent<FirearmScript>().enabled = true;
                reticleSprite.sprite = inventory[selection].GetComponent<FirearmScript>().reticle;
                inventory[selection].gameObject.SetActive(true);

                inventory[inventory.Count - 1].GetComponent<FirearmScript>().enabled = false;
                inventory[inventory.Count - 1].gameObject.SetActive(false);

                weaponAmmoPage.gameObject.SetActive(true);
                lucentText.gameObject.SetActive(true);
                wepStateTimer = wepStateTimerReset;

                return;
            }

            else
            {
                selection++;

                //Prevents running off the inventory forwards
                //if (selection >= inventory.Count)
                //{
                //    selection = inventory.Count - 1;
                //}

                inventory[selection].GetComponent<FirearmScript>().enabled = true;
                reticleSprite.sprite = inventory[selection].GetComponent<FirearmScript>().reticle;
                inventory[selection].gameObject.SetActive(true);

                inventory[selection - 1].GetComponent<FirearmScript>().enabled = false;
                inventory[selection - 1].gameObject.SetActive(false);

                weaponAmmoPage.gameObject.SetActive(true);
                lucentText.gameObject.SetActive(true);
                wepStateTimer = wepStateTimerReset;

                return;
            }          
        }
    }

    /// <summary>
    /// Removes Weapon at index position after short period
    /// </summary>
    private void DismantleInv()
    {
        if (Input.GetKey(KeyCode.X))
        {
            dismantleTimer -= Time.deltaTime;
            dismantleText.text = "Dismantling...";
            dismantleText.color = Color.Lerp(Color.red, Color.black, dismantleTimer);
         
            if(dismantleTimer <= 0.0f)
            {
                if (selection <= -1 && inventory.Count <= 0)
                {
                    //Debug.Log("Cannot dismantle; no items in inventory");
                    return;
                }

                //Creats, assigns gameObject "rid" to disposed Weapon
                //Inventory removes selected Weapon, "rid" is destroyed
                GameObject rid = inventory[selection];
                inventory.RemoveAt(selection);
                Destroy(rid);

                //selection decrements if equal to or higher than Inventory size
                if (selection >= inventory.Count)
                {
                    selection--;

                    //selection returns to -1 if Inventory size is zero
                    //Otherwise, activates next Weapon
                    if (selection <= -1 && inventory.Count <= 0)
                    {
                        selection = -1;
                        return;
                    }

                    else
                    {
                        inventory[selection].GetComponent<FirearmScript>().enabled = true;
                        inventory[selection].gameObject.SetActive(true);
                        reticleSprite.sprite = inventory[selection].GetComponent<FirearmScript>().reticle;

                    }
                }

                //selection activates first Weapon in inventory
                if (selection <= -1 && inventory.Count >= 1)
                {
                    selection = 0;
                    inventory[selection].GetComponent<FirearmScript>().enabled = true;
                    inventory[selection].gameObject.SetActive(true);
                    reticleSprite.sprite = inventory[selection].GetComponent<FirearmScript>().reticle;
                }

                //selection activates current Weapon in inventory
                else
                {
                    inventory[selection].GetComponent<FirearmScript>().enabled = true;
                    inventory[selection].gameObject.SetActive(true);
                    reticleSprite.sprite = inventory[selection].GetComponent<FirearmScript>().reticle;
                }

                dismantleTimer = dismantleTimerReset;

                weaponAmmoPage.gameObject.SetActive(true);
                lucentText.gameObject.SetActive(true);
                wepStateTimer = wepStateTimerReset;

            }
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            dismantleTimer = dismantleTimerReset;
            dismantleText.text = " ";
            dismantleText.color = Color.black;
            return;
        }
    }

    /// <summary>
    /// Displays Stat, Functional Cheats dependent on RNG numbers rolled
    /// </summary>
    void DisplayCheats()
    {
        //Yields -- Cheat 1
        if(inventory[selection].GetComponent<DeepYield>())
        {
            cheatOne.text = "Deep Yield (+12% MAG)";
        }

        if (inventory[selection].GetComponent<DeeperYield>())
        {
            cheatOne.text = "Deeper Yield (+24% MAG)";
        }

        //Stores -- Cheat 2
        if (inventory[selection].GetComponent<DeepStores>())
        {
            cheatTwo.text = "Deep Stores (+15% RES)";
        }

        if (inventory[selection].GetComponent<DeeperStores>())
        {
            cheatTwo.text = "Deeper Stores (+30% RES)";
        }

        //Sights -- Cheat 3
        if (inventory[selection].GetComponent<FarSight>())
        {
            cheatThree.text = "Far Sight (+10% EFR)";
        }

        if (inventory[selection].GetComponent<FartherSight>())
        {
            cheatThree.text = "Farther Sight (+20% EFR)";
        }

        //Hands -- Cheat 4
        if (inventory[selection].GetComponent<HastyHands>())
        {
            cheatFour.text = "Hasty Hands (+15% RLD)";
        }

        if (inventory[selection].GetComponent<HastierHands>())
        {
            cheatFour.text = "Hastier Hands (+25% RLD)";
        }

        //Extended Functions -- Cheat 5
        if (inventory[selection].GetComponent<FirearmScript>().weaponRarity <= 1)
        {
            cheatTraitOne.text = " ";
            cheatTraitTwo.text = " ";
        }
        
        else if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 2 || inventory[selection].GetComponent<FirearmScript>().weaponRarity == 3)
        {
            //Wait! Now I'm Ready!
            if (inventory[selection].GetComponent<WaitNowImReady>())
            {
                cheatTraitOne.text = "Wait! Now I'm Ready!" + '\n' +
                    "Kills with this Weapon restore 10% of Shield strength.";               
            }

            //Efficacy
            if (inventory[selection].GetComponent<Efficacy>())
            {
                cheatTraitOne.text = "Efficacy" + '\n' +
                    "Enemy hits increases this Weapon's base damage by 1%.";
            }

            //Inoculated
            if (inventory[selection].GetComponent<Inoculated>())
            {
                cheatTraitOne.text = "Inoculated" + '\n' +
                    "Kills with this Weapon restore 5% of Health.";
            }

            //Rude Awakening
            if (inventory[selection].GetComponent<RudeAwakening>())
            {
                cheatTraitOne.text = "Rude Awakening" + '\n' +
                    "[E] - Cast a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x.";
            }

            //Not with a Stick
            if (inventory[selection].GetComponent<NotWithAStick>())
            {
                cheatTraitOne.text = "Not with a Stick" + '\n' +
                    "Kills with this Weapon increase Effective Range by 30% of max Range.";
            }

            //Malicious Wind-Up
            if (inventory[selection].GetComponent<MaliciousWindUp>())
            {
                cheatTraitOne.text = "Malicious Wind-Up" + '\n' +
                    "Inflicting Damage increases Reload Speed by 0.75%.";
            }

            //Positive-Negative
            if (inventory[selection].GetComponent<PositiveNegative>())
            {
                cheatTraitOne.text = "Positive-Negative" + '\n' +
                    "Moving generates a charge. While halfway charged, Enemy hits apply damage-over-time.";
            }

            //Cadence
            if (inventory[selection].GetComponent<Cadence>())
            {
                cheatTraitOne.text = "Cadence" + '\n' +
                    "Every third Enemy kill spawns a Lucent cluster.";
            }

            //Good Things Come
            if (inventory[selection].GetComponent<GoodThingsCome>())
            {
                cheatTraitOne.text = "Good Things Come" + '\n' +
                "Being in combat grants increased movement and reduces recoil and damage taken.";
            }

            //All Else Fails
            if (inventory[selection].GetComponent<AllElseFails>())
            {
                cheatTraitOne.text = "All Else Fails" + '\n' +
                    "When Shield depletes, all incoming Enemy damage is nullified for three seconds.";
            }

            //The Most Resplendent
            if (inventory[selection].GetComponent<TheMostResplendent>())
            {
                cheatTraitOne.text = "The Most Resplendent" + '\n' +
                    "[E] - Create a Hard Lucent crystal that produces Lucent clusters passively or when shot. Stacks 1x.";
            }

            //Fulminate
            if (inventory[selection].GetComponent<Fulminate>())
            {
                cheatTraitOne.text = "Fulminate" + '\n' +
                    "Enemy hits increase Destruct Grenade damage by 2%, up to 70%. Melee kills cast a Destruct Grenade.";
            }

            //Forager
            if (inventory[selection].GetComponent<Forager>())
            {
                cheatTraitOne.text = "Forager" + '\n' +
                    "Weapon or Melee kills produce a burst of Lucent clusters, Health, Shield, and Ammo pickups.";
            }

            //Counterplay
            if (inventory[selection].GetComponent<Counterplay>())
            {
                cheatTraitOne.text = "Counterplay" + '\n' +
                    "Hits taken during Evasions casts two Lucent clusters and increases Weapon damage by 10%. Stacks 3x.";
            }

            //Enshroud
            if (inventory[selection].GetComponent<Enshroud>())
            {
                cheatTraitOne.text = "Enshroud" + '\n' +
                    "Enemy hits increase Melee range by 15%, up to 200%. Melee kills cast a Fogger Grenade.";
            }

            //Gale Force Winds
            if (inventory[selection].GetComponent<GaleForceWinds>())
            {
                cheatTraitOne.text = "Gale Force Winds" + '\n' +
                    "[E] - Cast traveling winds that applies Health and Slowed debuffs to in-range Enemies. Lasts 45s.";
            }

            cheatTraitTwo.text = " ";
        }

        else if (inventory[selection].GetComponent<FirearmScript>().weaponRarity >= 4)
        {
            //Pool 1--------------------------------------------

            //All Else Fails
            if (inventory[selection].GetComponent<AllElseFails>())
            {
                cheatTraitOne.text = "All Else Fails" + '\n' +
                    "When Shield depletes, all incoming Enemy damage is nullified for three seconds.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitOne.text = "All Else Fails" + " (Fated)" + '\n' +
                    "When Shield depletes, all incoming Enemy damage is nullified for five seconds.";
                }
            }

            //Not with a Stick
            if (inventory[selection].GetComponent<NotWithAStick>())
            {
                cheatTraitOne.text = "Not with a Stick" + '\n' +
                    "Kills with this Weapon increase Effective Range by 30% of max Range.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitOne.text = "Not with a Stick" + " (Fated)" + '\n' +
                    "Maximizing Effective Range through kills increases Aim Assist by 50% for 20 seconds.";
                }
            }

            //Malicious Wind-Up
            if (inventory[selection].GetComponent<MaliciousWindUp>())
            {
                cheatTraitOne.text = "Malicious Wind-Up" + '\n' +
                    "Inflicting Damage increases Reload Speed by 0.75%.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitOne.text = "Malicious Wind-Up" + " (Fated)" + '\n' +
                    "Inflicting Damage increases Reload Speed by 1.5%. Kills restore 5% of Max Reserves.";
                }
            }

            //Positive-Negative
            if (inventory[selection].GetComponent<PositiveNegative>())
            {
                cheatTraitOne.text = "Positive-Negative" + '\n' +
                    "Moving generates a charge. While halfway charged, Enemy hits apply damage-over-time.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitOne.text = "Positive-Negative" + " (Fated)" + '\n' +
                    "While halfway charged, Enemy hits apply stronger damage-over-time that triggers twice as fast.";
                }
            }

            //Good Things Come
            if (inventory[selection].GetComponent<GoodThingsCome>())
            {
                cheatTraitOne.text = "Good Things Come" + '\n' +
                "Being in combat grants increased movement and reduces recoil and damage taken.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {

                    cheatTraitOne.text = "Good Things Come" + " (Fated)" + '\n' +
                        "Being in combat grants Infinite Ammo, and doubles increased movement, recoil and damage reduction.";
                }
            }

            //The Most Resplendent
            if (inventory[selection].GetComponent<TheMostResplendent>())
            {
                cheatTraitOne.text = "The Most Resplendent" + '\n' +
                    "[E] - Create a Hard Lucent crystal that produces Lucent clusters passively or when shot. Stacks 1x.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {

                    cheatTraitOne.text = "The Most Resplendent" + " (Fated)" + '\n' +
                    "[E] - Create a Hard Lucent crystal. Stacks 2x. Physically shattering the crystal restores 35% of Health.";
                }
            }

            //Fulminate
            if (inventory[selection].GetComponent<Fulminate>())
            {
                cheatTraitOne.text = "Fulminate" + '\n' +
                    "Enemy hits increase Destruct Grenade damage by 2%, up to 70%. Melee kills cast a Destruct Grenade.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitOne.text = "Fulminate" + " (Fated)" + '\n' +
                    "Enemy hits increase Destruct Grenade damage by 2%, up to 70%. Passively throw two Destruct Grenades.";
                }
            }

            //Forager
            if (inventory[selection].GetComponent<Forager>())
            {
                cheatTraitOne.text = "Forager" + '\n' +
                    "Weapon or Melee kills produce a burst of Lucent clusters, Health, Shield, and Ammo pickups.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitOne.text = "Forager" + " (Fated)" + '\n' +
                    "Kills produce stronger bursts of Lucent clusters, Health, Shield, and Ammo pickups. Every tenth hit on Bosses produce a burst.";
                }
            }

            //Pool 2--------------------------------------------

            //Wait! Now I'm Ready
            if (inventory[selection].GetComponent<WaitNowImReady>())
            {
                cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                    "Kills with this Weapon restore 10% of Shield strength.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitTwo.text = "Wait! Now I'm Ready!" + " (Fated)" + '\n' +
                    "Kills with this Weapon restore 20% of Shield strength.";
                }
            }

            //Efficacy
            if (inventory[selection].GetComponent<Efficacy>())
            {
                cheatTraitTwo.text = "Efficacy" + '\n' +
                    "Enemy hits increases this Weapon's base damage by 1%.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitTwo.text = "Efficacy" + " (Fated)" + '\n' +
                    "Enemy hits increases this Weapon's base damage by 2%, up to 125%, and cannot reset.";
                }
            }

            //Inoculated
            if (inventory[selection].GetComponent<Inoculated>())
            {
                cheatTraitTwo.text = "Inoculated" + '\n' +
                    "Kills with this Weapon restore 5% of Health.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitTwo.text = "Inoculated" + " (Fated)" + '\n' +
                    "Kills with this Weapon restore 10% of Health.";
                }
            }

            //Cadence
            if (inventory[selection].GetComponent<Cadence>())
            {
                cheatTraitTwo.text = "Cadence" + '\n' +
                    "Every third Enemy kill spawns a Lucent cluster.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitTwo.text = "Cadence" + " (Fated)" + '\n' +
                    "Every third shot spawns a Lucent cluster.";
                }
            }

            //Rude Awakening
            if (inventory[selection].GetComponent<RudeAwakening>())
            {
                cheatTraitTwo.text = "Rude Awakening" + '\n' +
                    "[E] - Cast a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitTwo.text = "Rude Awakening" + " (Fated)" + '\n' +
                    "[E] - Cast a lethal AOE blast. Stacks 6x. Having stacks increases base damage by 20%.";
                }
            } 

            //Counterplay
            if (inventory[selection].GetComponent<Counterplay>())
            {
                cheatTraitTwo.text = "Counterplay" + '\n' +
                    "Hits taken during Evasions casts two Lucent clusters and increases Weapon damage by 10%. Stacks 3x.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitTwo.text = "Counterplay" + " (Fated)" + '\n' +
                    "Hits taken during Evasions casts Lucent clusters, a Solution Grenade, and increases Weapon damage by 10%. Stacks 10x.";
                }
            }

            //Enshroud
            if (inventory[selection].GetComponent<Enshroud>())
            {
                cheatTraitTwo.text = "Enshroud" + '\n' +
                    "Enemy hits increase Melee range by 15%, up to 200%. Melee kills cast a Fogger Grenade.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitTwo.text = "Enshroud" + " (Fated)" + '\n' +
                    "Enemy hits increase Melee range by 15%, up to 200%. All Fogger Grenades apply low damage-over-time.";
                }
            }

            //Gale Force Winds
            if (inventory[selection].GetComponent<GaleForceWinds>())
            {
                cheatTraitTwo.text = "Gale Force Winds" + '\n' +
                    "[E] - Cast traveling winds that applies Health and Slowed debuffs to in-range Enemies. Lasts 45s.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitTwo.text = "Gale Force Winds" + " (Fated)" + '\n' +
                    "[E] - Cast faster traveling winds that applies stronger Health debuffs. Applies low damage-over-time to tracked Enemies.";
                }
            }
        }


        //Extended Functions for Exotics -- Cheat 5
        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -1)
        {
            cheatTraitOne.text = "Equivalent Exchange" + '\n' +
                "Taking Enemy damage adds 35% of the hit to this Weapon's base damage and as Health.";

            cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                   "Kills with this weapon restore 10% of Shield strength.";
        } //Equivalent Exchange + Wait! Now I'm Ready!

        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -2)
        {
            cheatTraitOne.text = "Absolutely No Stops" + '\n' +
                "Expending your magazine instantly reloads, and increases damage and Rate of Fire.";

            cheatTraitTwo.text = "Forager" + '\n' +
                "Weapon or Melee kills produce a burst of Lucent clusters, Health, Shield, and Ammo pickups.";
        } //Absolutely No Stops + Forager

        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -3)
        {
            cheatTraitOne.text = "Shelter in Place" + '\n' +
                "Refraining from moving amplifies Weapon damage by 100% and provides 80% damage reduction.";

            cheatTraitTwo.text = "Positive-Negative" + '\n' +
                    "Moving generates a charge. While halfway charged, Enemy hits apply damage-over-time.";
        } //Shelter in Place + Positive-Negative

        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -4)
        {
            cheatTraitOne.text = "Social Distance, please!" + '\n' +
                "Enemy hits apply a debuff and increases base damage by 30%. Kills spread 400% of damage.";

            cheatTraitTwo.text = "Not with a Stick" + '\n' +
                    "Kills with this Weapon increase Effective Range by 30% of max Range.";
        } //Social Distance, Please! + Not with a Stick

        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -5)
        {
            cheatTraitOne.text = "The Early Berth gets the Hearst" + '\n' +
                "Every other Enemy hit triggers a Berth detonation, inflicting 200% of Weapon damage.";

            cheatTraitTwo.text = "Efficacy" + '\n' +
                    "Enemy hits increases this Weapon's base damage by 1%.";
        } //Early Berth gets the Hearst + Efficacy

        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -6)
        {
            cheatTraitOne.text = "Off Your Own Supply" + '\n' +
                "[E] - Sacrificing your Shield grants increased movement, Reload Speed, Weapon damage and zero Recoil.";

            cheatTraitTwo.text = "Inoculated" + '\n' +
                    "Kills with this Weapon restore 5% of Health.";
        } //Off your own Supply + Inoculated

        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -7)
        {
            cheatTraitOne.text = "Pay to Win" + '\n' +
                "[E] - Consume 5,280 Lucent for a 50% Weapon damage increase. Stacks 150x.";

            cheatTraitTwo.text = "The Most Resplendent" + '\n' +
                    "[E] - Create a Hard Lucent crystal that produces Lucent clusters passively or when shot. Stacks 1x.";
        } //Pay to Win + The Most Resplendent
    }

    /// <summary>
    /// Switches grenades upon input
    /// </summary>
    private void SwitchGrenades()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(grenadeSelection >= grenades.Count - 1)
            {
                grenadeSelection = 0;
                grenadeText.gameObject.SetActive(true);
                wepStateTimer = wepStateTimerReset;

            }

            else
            {
                grenadeSelection++;
                grenadeText.gameObject.SetActive(true);
                wepStateTimer = wepStateTimerReset;
            }
        }
    }

    /// <summary>
    /// Charges, throws selected grenade
    /// </summary>
    private void ThrowGrenade()
    {
        if(Input.GetKey(KeyCode.G))
        {
            if(grenadeSelection == 0)
            {
                if (fogGrenadeCharges <= 0)
                {
                    Debug.Log("Cannot throw Fogger Grenade; No charges available");
                }

                else
                {
                    throwing = true;
                    grenadeThrow.maxValue = throwMax;
                    grenadeThrow.minValue = throwMin;

                    throwStrength = Mathf.PingPong(Time.time * 50f, throwMax - throwMin) + throwMin;
                    grenadeThrow.value = Mathf.PingPong(Time.time * 50f, throwMax - throwMin) + throwMin;

                }
            } //Fogger Grenade

            if (grenadeSelection == 1)
            {
                if (solGrenadeCharges <= 0)
                {
                    Debug.Log("Cannot throw Solution Grenade; No charges available");
                }

                else
                {
                    throwing = true;
                    grenadeThrow.maxValue = throwMax;
                    grenadeThrow.minValue = throwMin;

                    throwStrength = Mathf.PingPong(Time.time * 50f, throwMax - throwMin) + throwMin;
                    grenadeThrow.value = Mathf.PingPong(Time.time * 50f, throwMax - throwMin) + throwMin;

                }
            } //Solution Grenade

            if (grenadeSelection == 2)
            {
                if (desGrenadeCharges <= 0)
                {
                    Debug.Log("Cannot throw Destruct Grenade; No charges available");
                }

                else
                {
                    throwing = true;
                    grenadeThrow.maxValue = throwMax;
                    grenadeThrow.minValue = throwMin;

                    throwStrength = Mathf.PingPong(Time.time * 50f, throwMax - throwMin) + throwMin;
                    grenadeThrow.value = Mathf.PingPong(Time.time * 50f, throwMax - throwMin) + throwMin;

                }
            } //Destruct Grenade
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            if(grenadeSelection == 0)
            {
                if (fogGrenadeCharges <= 0)
                {
                    Debug.Log("Cannot throw Fogger Grenade; No charges available");
                }

                else
                {
                    fogGrenadeCharges--;
                    GameObject brand = Instantiate(grenades[grenadeSelection], transform.position + transform.forward, transform.rotation);
                    brand.name = grenades[grenadeSelection].name;
                    brand.GetComponent<Rigidbody>().AddForce(transform.forward * throwStrength, ForceMode.Impulse);

                    if(enshroudPresent)
                    {
                        brand.GetComponent<FoggerGrenadeScript>().enshroudFlag = true;
                    }

                    throwing = false;

                    throwStrength = 0;
                    grenadeThrow.value = throwMin;

                    grenadeText.gameObject.SetActive(true);
                    wepStateTimer = wepStateTimerReset;
                }
            } //Fogger Grenade

            if (grenadeSelection == 1)
            {
                if (solGrenadeCharges <= 0)
                {
                    Debug.Log("Cannot throw Solution Grenade; No charges available");
                }

                else
                {
                    solGrenadeCharges--;
                    GameObject brand = Instantiate(grenades[grenadeSelection], transform.position + transform.forward, transform.rotation);
                    brand.name = grenades[grenadeSelection].name;
                    brand.GetComponent<Rigidbody>().AddForce(transform.forward * throwStrength, ForceMode.Impulse);
                    throwing = false;

                    throwStrength = 0;
                    grenadeThrow.value = throwMin;

                    grenadeText.gameObject.SetActive(true);
                    wepStateTimer = wepStateTimerReset;
                }
            } //Solution Grenade

            if (grenadeSelection == 2)
            {
                if (desGrenadeCharges <= 0)
                {
                    Debug.Log("Cannot throw Destruct Grenade; No charges available");
                }

                else
                {
                    desGrenadeCharges--;
                    GameObject brand = Instantiate(grenades[grenadeSelection], transform.position + transform.forward, transform.rotation);
                    brand.name = grenades[grenadeSelection].name;
                    brand.GetComponent<Rigidbody>().AddForce(transform.forward * throwStrength, ForceMode.Impulse);

                    if(fulminatePresent)
                    {
                        if(fulminateBuff == brand.GetComponent<DestructGrenadeScript>().explosiveDamage)
                        {
                            //Debug.Log("No buff was applied; Damage has not changed");
                        }

                        else
                        {
                            brand.GetComponent<DestructGrenadeScript>().explosiveDamage = fulminateBuff;
                            //Debug.Log(fulminateBuff);
                        }

                        if(fulminateFated)
                        {
                            StartCoroutine(FatedFulminateFreeGrenade(throwStrength, fulminateBuff));
                        }

                    }

                    throwing = false;

                    throwStrength = 0;
                    grenadeThrow.value = throwMin;

                    grenadeText.gameObject.SetActive(true);
                    wepStateTimer = wepStateTimerReset;
                }
            } //Destruct Grenade
        }
    }

    /// <summary>
    /// Creates Inventory file if filename was not found
    /// Overwrites existing Inventory file if it was found
    /// </summary>
    private void CreateInventoryFile()
    {
        //inv.txt - relative (adds file where game is running from)
        //C:\Users\Darius\inv.txt - fully qualified (adds file in specific location)
        if (!File.Exists(filepath))
        {
            File.Create(filepath).Close();

        }

        else //overwrite the file anyway
        {
            File.Create(filepath).Close();
        }
    }

    /// <summary>
    /// Reads Inventory file and outputs it to Console
    /// </summary>
    private void ReadInventoryFile()
    {
        if(inventory.Count <= 0)
        {
            Debug.Log("Nothing to read due to empty inventory!");
        }

        else
        {
            using (StreamReader read = new StreamReader(filepath))
            {
                while (!read.EndOfStream) //while you have yet to reach the end of the file
                {
                    Debug.Log(read.ReadLine());
                }
            }
        }      
    }   

    /// <summary>
    /// Records information of Weapon type, rarity, exotic property, cheats 
    /// </summary>
    public void WriteOnReset()
    {
        CreateInventoryFile();

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

                    if (inventory[i].name == "Machine Gun" || inventory[i].name == "Bulleted Clause")
                    {
                        write.Write("2");                      
                    }

                    if (inventory[i].name == "Pistol" || inventory[i].name == "Apathetic")
                    {
                        write.Write("3");                    
                    }

                    if (inventory[i].name == "Semi Fire Rifle" || inventory[i].name == "The Fatal Cardio")
                    {
                        write.Write("4");                       
                    }

                    if (inventory[i].name == "Shotgun" || inventory[i].name == "Viral Shadow")
                    {
                        write.Write("5");                      
                    }

                    if (inventory[i].name == "Single Fire Rifle" || inventory[i].name == "Every Second Sun")
                    {
                        write.Write("6");                      
                    }

                    if (inventory[i].name == "SMG" || inventory[i].name == "Underfoot")
                    {
                        write.Write("7");                      
                    }

                    if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
                    {
                        write.Write("1");
                    }

                    if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 2)
                    {
                        write.Write("2");
                    }

                    if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 3)
                    {
                        write.Write("3");
                    }

                    if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 4)
                    {
                        write.Write("4");
                    }

                    if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 5)
                    {
                        write.Write("5");
                    }

                    if (inventory[i].GetComponent<FirearmScript>().isExotic == true)
                    {
                        write.Write("1");
                    }

                    else
                    {
                        write.Write("0");
                    }

                    if (inventory[i].GetComponent<DeepYield>())
                    {
                        write.Write("1");
                    }

                    if (inventory[i].GetComponent<DeeperYield>())
                    {
                        write.Write("2");
                    }

                    if (inventory[i].GetComponent<DeepStores>())
                    {
                        write.Write("3");
                    }

                    if (inventory[i].GetComponent<DeeperStores>())
                    {
                        write.Write("4");
                    }

                    if (inventory[i].GetComponent<FarSight>())
                    {
                        write.Write("5");
                    }

                    if (inventory[i].GetComponent<FartherSight>())
                    {
                        write.Write("6");
                    }

                    if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
                    {
                        if (inventory[i].GetComponent<HastyHands>())
                        {
                            write.WriteLine("7");
                        }

                        if (inventory[i].GetComponent<HastierHands>())
                        {
                            write.WriteLine("8");
                        }
                    }

                    if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 2 || inventory[i].GetComponent<FirearmScript>().weaponRarity == 3)
                    {                      

                        if (inventory[i].GetComponent<HastyHands>())
                        {
                            write.Write("7");
                        }

                        if (inventory[i].GetComponent<HastierHands>())
                        {
                            write.Write("8");
                        }

                        if (inventory[i].GetComponent<WaitNowImReady>())
                        {
                            write.WriteLine("0");
                        }

                        if (inventory[i].GetComponent<Efficacy>())
                        {
                            write.WriteLine("1");
                        }

                        if (inventory[i].GetComponent<Inoculated>())
                        {
                            write.WriteLine("2");
                        }

                        if (inventory[i].GetComponent<RudeAwakening>())
                        {
                            write.WriteLine("3");
                        }

                        if (inventory[i].GetComponent<NotWithAStick>())
                        {
                            write.WriteLine("4");
                        }

                        if (inventory[i].GetComponent<MaliciousWindUp>())
                        {
                            write.WriteLine("5");
                        }

                        if (inventory[i].GetComponent<PositiveNegative>())
                        {
                            write.WriteLine("6");
                        }

                        if (inventory[i].GetComponent<Cadence>())
                        {
                            write.WriteLine("7");
                        }

                        if (inventory[i].GetComponent<GoodThingsCome>())
                        {
                            write.WriteLine("8");
                        }

                        if (inventory[i].GetComponent<AllElseFails>())
                        {
                            write.WriteLine("9");
                        }

                        if (inventory[i].GetComponent<TheMostResplendent>())
                        {
                            write.WriteLine("!");
                        }

                        if (inventory[i].GetComponent<Fulminate>())
                        {
                            write.WriteLine("@");
                        }

                        if (inventory[i].GetComponent<Forager>())
                        {
                            write.WriteLine("#");
                        }

                        if (inventory[i].GetComponent<Counterplay>())
                        {
                            write.WriteLine("$");
                        }

                        if (inventory[i].GetComponent<Enshroud>())
                        {
                            write.WriteLine("%");
                        }

                        if (inventory[i].GetComponent<GaleForceWinds>())
                        {
                            write.WriteLine("^");
                        }
                    }

                    if (inventory[i].GetComponent<FirearmScript>().weaponRarity >= 4)
                    {

                        if (inventory[i].GetComponent<HastyHands>())
                        {
                            write.Write("7");
                        }

                        if (inventory[i].GetComponent<HastierHands>())
                        {
                            write.Write("8");
                        }

                        if (inventory[i].GetComponent<FirearmScript>().isExotic == true)
                        {
                            //Exotic Full Fire Rifle
                            if (inventory[i].GetComponent<EquivalentExchange>())
                            {
                                write.Write("A");
                            }

                            if (inventory[i].GetComponent<WaitNowImReady>())
                            {
                                write.WriteLine("0");
                            }

                            //Exotic Machine Gun
                            if (inventory[i].GetComponent<PayToWin>())
                            {
                                write.Write("G");
                            }

                            if (inventory[i].GetComponent<TheMostResplendent>())
                            {
                                write.WriteLine("!");
                            }

                            //Exotic Pistol
                            if (inventory[i].GetComponent<ShelterInPlace>())
                            {
                                write.Write("C");
                            }

                            if (inventory[i].GetComponent<PositiveNegative>())
                            {
                                write.WriteLine("6");
                            }

                            //Exotic Semi Fire Rifle
                            if (inventory[i].GetComponent<OffYourOwnSupply>())
                            {
                                write.Write("F");
                            }

                            if (inventory[i].GetComponent<Inoculated>())
                            {
                                write.WriteLine("2");
                            }

                            //Exotic Shotgun
                            if (inventory[i].GetComponent<SocialDistancePlease>())
                            {
                                write.Write("D");
                            }

                            if (inventory[i].GetComponent<NotWithAStick>())
                            {
                                write.WriteLine("4");
                            }

                            //Exotic Single Fire Rifle
                            if (inventory[i].GetComponent<EarlyBerthGetsTheHearst>())
                            {
                                write.Write("E");
                            }

                            if (inventory[i].GetComponent<Efficacy>())
                            {
                                write.WriteLine("1");
                            }

                            //Exotic SMG
                            if (inventory[i].GetComponent<AbsolutelyNoStops>())
                            {
                                write.Write("B");
                            }

                            if (inventory[i].GetComponent<Forager>())
                            {
                                write.WriteLine("#");
                            }
                        }

                        else
                        {
                            if (inventory[i].GetComponent<AllElseFails>())
                            {
                                write.Write("9");
                            }

                            if (inventory[i].GetComponent<NotWithAStick>())
                            {
                                write.Write("4");
                            }

                            if (inventory[i].GetComponent<MaliciousWindUp>())
                            {
                                write.Write("5");
                            }

                            if (inventory[i].GetComponent<PositiveNegative>())
                            {
                                write.Write("6");
                            }

                            if (inventory[i].GetComponent<GoodThingsCome>())
                            {
                                write.Write("8");
                            }

                            if (inventory[i].GetComponent<TheMostResplendent>())
                            {
                                write.Write("!");
                            }

                            if (inventory[i].GetComponent<Fulminate>())
                            {
                                write.Write("@");
                            }

                            if (inventory[i].GetComponent<Forager>())
                            {
                                write.Write("#");
                            }

                            if (inventory[i].GetComponent<WaitNowImReady>())
                            {
                                write.WriteLine("0");
                            }

                            if (inventory[i].GetComponent<Efficacy>())
                            {
                                write.WriteLine("1");
                            }

                            if (inventory[i].GetComponent<Inoculated>())
                            {
                                write.WriteLine("2");
                            }

                            if (inventory[i].GetComponent<Cadence>())
                            {
                                write.WriteLine("7");
                            }

                            if (inventory[i].GetComponent<RudeAwakening>())
                            {
                                write.WriteLine("3");
                            }

                            if (inventory[i].GetComponent<Counterplay>())
                            {
                                write.WriteLine("$");
                            }

                            if (inventory[i].GetComponent<Enshroud>())
                            {
                                write.WriteLine("%");
                            }

                            if (inventory[i].GetComponent<GaleForceWinds>())
                            {
                                write.WriteLine("^");
                            }
                        }
                    }
                }
            }

            else
            {
                write.WriteLine("Inventory empty upon reset -- nothing saved!");
            }
        }
    }

    /// <summary>
    /// Records Inventory on input
    /// </summary>
    private void WriteInventory()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            WriteOnReset();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Adds Weapon to inventory upon trigger activation
        if(other.gameObject.tag == "Weapon")
        {
            AddInv(other.gameObject);
        }
    }

    /// <summary>
    /// Throws another Destruct Grenade with no cost, on a timed delay
    /// </summary>
    /// <param name="strength">represents grenade throw strength</param>
    /// <param name="buff">represents new Destruct Grenade damage value</param>
    /// <returns></returns>
    public IEnumerator FatedFulminateFreeGrenade(float strength, int buff)
    {
        yield return new WaitForSeconds(0.15f);
        GameObject another = Instantiate(grenades[grenadeSelection], transform.position + transform.forward, transform.rotation);
        another.name = grenades[grenadeSelection].name;
        another.GetComponent<Rigidbody>().AddForce(transform.forward * strength, ForceMode.Impulse);

        another.GetComponent<DestructGrenadeScript>().explosiveDamage = fulminateBuff;

    }
}
