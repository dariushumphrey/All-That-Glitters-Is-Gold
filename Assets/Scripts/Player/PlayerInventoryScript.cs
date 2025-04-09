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
    public List<GameObject> inventory = new List<GameObject>(10);
    public Transform gunPlace;
    private Text weaponStatus, weaponLoad;
    private Image weaponPage;
    private Image reticleSprite;
    private Text wepName, wepStats, flavor;
    private Text cheatOne, cheatTwo, cheatThree, cheatFour, cheatTraitOne, cheatTraitTwo;
    private Text invMonitor, rarityCheck, dismantleText;
    internal Text lucentText, kioskText;
    internal int selection;
    private float dismantleTimer = 1f;
    private float dismantleTimerReset;

    internal List<string> readdedWeps = new List<string>(10);
    
    // Start is called before the first frame update
    void Start()
    {      

        selection = -1;

        weaponStatus = GameObject.Find("weaponAmmoText").GetComponent<Text>();
        weaponLoad = GameObject.Find("weaponReloadText").GetComponent<Text>();
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
        lucentText = GameObject.Find("lucentText").GetComponent<Text>();
        kioskText = GameObject.Find("kioskText").GetComponent<Text>();
        reticleSprite = GameObject.Find("reticleImage").GetComponent<Image>();

        weaponLoad.text = " ";
        weaponStatus.text = " ";
        dismantleText.text = " ";
        kioskText.text = " ";
        weaponPage.gameObject.SetActive(false);
        reticleSprite.gameObject.SetActive(false);
        dismantleTimerReset = dismantleTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(selection + " || " + inventory.Count);

        SwitchInv();
        DismantleInv();
        //WriteOnReset();
        //WriteInventory();

        if(Input.GetKeyDown(KeyCode.UpArrow))
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

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            weaponPage.gameObject.SetActive(false);
        }

        if(inventory.Count > 0)
        {
            weaponStatus.text = inventory[selection].GetComponent<FirearmScript>().currentAmmo.ToString() + " | " +
                                inventory[selection].GetComponent<FirearmScript>().reserveAmmo.ToString();

            if(inventory[selection].GetComponent<FirearmScript>().isReloading == true)
            {
                weaponLoad.text = "Reloading";
            }

            if(inventory[selection].GetComponent<FirearmScript>().isReloading == false)
            {
                weaponLoad.text = " ";
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

        lucentText.text = "Lucent: " + lucentFunds;
    }

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
            g.GetComponent<LineRenderer>().enabled = true;
            //g.GetComponent<ReloadSpeedScript>().enabled = true;
            //g.GetComponent<RangeScript>().enabled = true;
            //g.GetComponent<MagazineScript>().enabled = true;
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
            g.GetComponent<LineRenderer>().enabled = false;
            //g.GetComponent<ReloadSpeedScript>().enabled = false;
            //g.GetComponent<RangeScript>().enabled = false;
            //g.GetComponent<MagazineScript>().enabled = false;
            g.GetComponent<BoxCollider>().isTrigger = false;
            g.GetComponent<BoxCollider>().enabled = false;
            g.gameObject.SetActive(false);

            return;
        }          
    }

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

            else
            {
                selection--;               

                //Prevents running off the inventory backwards
                if(selection <= -1 && inventory.Count >= 1)
                {
                    selection = 0;
                }

                inventory[selection].GetComponent<FirearmScript>().enabled = true;
                reticleSprite.sprite = inventory[selection].GetComponent<FirearmScript>().reticle;
                inventory[selection].GetComponent<LineRenderer>().enabled = true;
                inventory[selection].GetComponent<ReloadSpeedScript>().enabled = true;
                inventory[selection].GetComponent<RangeScript>().enabled = true;
                inventory[selection].GetComponent<MagazineScript>().enabled = true;
                inventory[selection].gameObject.SetActive(true);

                inventory[selection + 1].GetComponent<FirearmScript>().enabled = false;
                inventory[selection + 1].GetComponent<LineRenderer>().enabled = false;
                //inventory[selection + 1].GetComponent<ReloadSpeedScript>().enabled = false;
                //inventory[selection + 1].GetComponent<RangeScript>().enabled = false;
                //inventory[selection + 1].GetComponent<MagazineScript>().enabled = false;
                inventory[selection + 1].gameObject.SetActive(false);
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

            else
            {
                selection++;

                //Prevents running off the inventory forwards
                if(selection >= inventory.Count)
                {
                    selection = inventory.Count - 1;
                }

                inventory[selection].GetComponent<FirearmScript>().enabled = true;
                reticleSprite.sprite = inventory[selection].GetComponent<FirearmScript>().reticle;
                inventory[selection].GetComponent<LineRenderer>().enabled = true;
                inventory[selection].GetComponent<ReloadSpeedScript>().enabled = true;
                inventory[selection].GetComponent<RangeScript>().enabled = true;
                inventory[selection].GetComponent<MagazineScript>().enabled = true;
                inventory[selection].gameObject.SetActive(true);

                inventory[selection - 1].GetComponent<FirearmScript>().enabled = false;
                inventory[selection - 1].GetComponent<LineRenderer>().enabled = false;
                //inventory[selection - 1].GetComponent<ReloadSpeedScript>().enabled = false;
                //inventory[selection - 1].GetComponent<RangeScript>().enabled = false;
                //inventory[selection - 1].GetComponent<MagazineScript>().enabled = false;
                inventory[selection - 1].gameObject.SetActive(false);
                return;
            }          
        }
    }

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
                    Debug.Log("Cannot dismantle; no items in inventory");
                    return;
                }

                GameObject rid = inventory[selection];
                inventory.RemoveAt(selection);
                Destroy(rid);

                if (selection >= inventory.Count)
                {
                    selection--;

                    if (selection <= -1 && inventory.Count <= 0)
                    {
                        selection = -1;
                        return;
                    }

                    else
                    {
                        inventory[selection].GetComponent<FirearmScript>().enabled = true;
                        inventory[selection].GetComponent<LineRenderer>().enabled = true;
                        inventory[selection].gameObject.SetActive(true);
                        reticleSprite.sprite = inventory[selection].GetComponent<FirearmScript>().reticle;

                    }
                }

                if (selection <= -1 && inventory.Count >= 1)
                {
                    selection = 0;
                    inventory[selection].GetComponent<FirearmScript>().enabled = true;
                    inventory[selection].GetComponent<LineRenderer>().enabled = true;
                    inventory[selection].gameObject.SetActive(true);
                    reticleSprite.sprite = inventory[selection].GetComponent<FirearmScript>().reticle;
                }

                else
                {
                    inventory[selection].GetComponent<FirearmScript>().enabled = true;
                    inventory[selection].GetComponent<LineRenderer>().enabled = true;
                    inventory[selection].gameObject.SetActive(true);
                    reticleSprite.sprite = inventory[selection].GetComponent<FirearmScript>().reticle;
                }

                dismantleTimer = dismantleTimerReset;

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

    void DisplayCheats()
    {
        //Yields -- Cheat 1
        if(inventory[selection].GetComponent<FirearmScript>().ammoCheatOne <= 50)
        {
            cheatOne.text = "Deep Yield (+12% MAG)";
        }

        if (inventory[selection].GetComponent<FirearmScript>().ammoCheatOne > 50 || inventory[selection].GetComponent<FirearmScript>().isExotic == true)
        {
            cheatOne.text = "Deeper Yield (+24% MAG)";
        }

        //Stores -- Cheat 2
        if (inventory[selection].GetComponent<FirearmScript>().ammoCheatTwo <= 150)
        {
            cheatTwo.text = "Deep Stores (+15% RVS)";
        }

        if (inventory[selection].GetComponent<FirearmScript>().ammoCheatTwo > 150 || inventory[selection].GetComponent<FirearmScript>().isExotic == true)
        {
            cheatTwo.text = "Deeper Stores (+30% RVS)";
        }

        //Sights -- Cheat 3
        if (inventory[selection].GetComponent<FirearmScript>().rangeCheatOne <= 250)
        {
            cheatThree.text = "Far Sight (+10% EFR)";
        }

        if (inventory[selection].GetComponent<FirearmScript>().rangeCheatOne > 250 || inventory[selection].GetComponent<FirearmScript>().isExotic == true)
        {
            cheatThree.text = "Farther Sight (+20% EFR)";
        }

        //Hands -- Cheat 4
        if (inventory[selection].GetComponent<FirearmScript>().reloadCheatOne <= 350)
        {
            cheatFour.text = "Hasty Hands (+15% RLD)";
        }

        if (inventory[selection].GetComponent<FirearmScript>().reloadCheatOne > 350 || inventory[selection].GetComponent<FirearmScript>().isExotic == true)
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
            if (inventory[selection].GetComponent<FirearmScript>().cheatRNG <= 450)
            {
                cheatTraitOne.text = "Wait! Now I'm Ready!" + '\n' +
                    "Kills with this weapon restore 10% of Shield strength.";               
            }

            //Efficacy
            if (inventory[selection].GetComponent<FirearmScript>().cheatRNG > 450 && inventory[selection].GetComponent<FirearmScript>().cheatRNG <= 500)
            {
                cheatTraitOne.text = "Efficacy" + '\n' +
                    "Enemy hits increases this weapon's base damage by 1%. Reloading this weapon resets its base damage.";
            }

            //Inoculated
            if (inventory[selection].GetComponent<FirearmScript>().cheatRNG > 500 && inventory[selection].GetComponent<FirearmScript>().cheatRNG <= 550)
            {
                cheatTraitOne.text = "Inoculated" + '\n' +
                    "Kills with this weapon restore 5% of Health.";
            }

            //Rude Awakening
            if (inventory[selection].GetComponent<FirearmScript>().cheatRNG > 550 && inventory[selection].GetComponent<FirearmScript>().cheatRNG <= 600)
            {
                cheatTraitOne.text = "Rude Awakening" + '\n' +
                    "Kills grant up to three stacks of a lethal AOE blast that inflicts 1,000% of Weapon damage." + '\n' +
                    "'Space' - Cast Blast";
            }

            //Not with a Stick
            if (inventory[selection].GetComponent<FirearmScript>().cheatRNG > 600 && inventory[selection].GetComponent<FirearmScript>().cheatRNG <= 650)
            {
                cheatTraitOne.text = "Not with a Stick" + '\n' +
                    "Kills with this weapon increase Effective Range by 30% of max Range until your next reload.";
            }

            //Malicious Wind-Up
            if (inventory[selection].GetComponent<FirearmScript>().cheatRNG > 650 && inventory[selection].GetComponent<FirearmScript>().cheatRNG <= 700)
            {
                cheatTraitOne.text = "Malicious Wind-Up" + '\n' +
                    "Inflicting Damage increases Reload Speed by 0.75%. This bonus activates on your next reload.";
            }

            //Positive-Negative
            if (inventory[selection].GetComponent<FirearmScript>().cheatRNG > 700 && inventory[selection].GetComponent<FirearmScript>().cheatRNG <= 750)
            {
                cheatTraitOne.text = "Positive-Negative" + '\n' +
                    "Moving generates a charge. When charged at least halfway, hitting an Enemy applies damage-over-time for ten seconds, inflicting 100% of Weapon damage once every second.";
            }

            //Cadence
            if (inventory[selection].GetComponent<FirearmScript>().cheatRNG > 750 && inventory[selection].GetComponent<FirearmScript>().cheatRNG <= 800)
            {
                cheatTraitOne.text = "Cadence" + '\n' +
                    "Every third Enemy kill spawns a Lucent cluster.";
            }

            //Good Things Come
            if (inventory[selection].GetComponent<FirearmScript>().cheatRNG > 800 && inventory[selection].GetComponent<FirearmScript>().cheatRNG <= 850)
            {
                cheatTraitOne.text = "Good Things Come" + '\n' +
                "Being in combat for three seconds grants:" + '\n' +
                "25% Movement Speed increase" + '\n' +
                "20% Damage reduction" + '\n' +
                "45% Recoil reduction" + '\n' +
                "Leaving combat for five seconds ends the bonus.";
            }

            //All Else Fails
            if (inventory[selection].GetComponent<FirearmScript>().cheatRNG > 850)
            {
                cheatTraitOne.text = "All Else Fails" + '\n' +
                    "When Shield is depleted, all incoming Enemy damage is nullified for three seconds. Cooldown: 20 Seconds.";
            }

            cheatTraitTwo.text = " ";
        }

        else if (inventory[selection].GetComponent<FirearmScript>().weaponRarity >= 4)
        {
            //All Else Fails
            if(inventory[selection].GetComponent<FirearmScript>().fcnChtOne <= 410)
            {
                cheatTraitOne.text = "All Else Fails" + '\n' +
                    "When Shield is depleted, all incoming Enemy damage is nullified for three seconds. Cooldown: 20 Seconds.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitOne.text = "All Else Fails" + " (Fated)" + '\n' +
                    "When Shield is depleted, all incoming Enemy damage is nullified for five seconds. Cooldown: 10 Seconds.";
                }
            }

            //Not with a Stick
            if (inventory[selection].GetComponent<FirearmScript>().fcnChtOne > 410 && inventory[selection].GetComponent<FirearmScript>().fcnChtOne <= 420)
            {
                cheatTraitOne.text = "Not with a Stick" + '\n' +
                    "Kills with this weapon increase Effective Range by 30% of max Range until your next reload.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitOne.text = "Not with a Stick" + " (Fated)" + '\n' +
                    "Kills with this weapon increase Effective Range by 30% of max Range. Maximizing Effective Range increases Aim Assist halfway to full strength. Lasts 20 seconds.";
                }
            }

            //Malicious Wind-Up
            if (inventory[selection].GetComponent<FirearmScript>().fcnChtOne > 420 && inventory[selection].GetComponent<FirearmScript>().fcnChtOne <= 430)
            {
                cheatTraitOne.text = "Malicious Wind-Up" + '\n' +
                    "Inflicting Damage increases Reload Speed by 0.75%. This bonus activates on your next reload.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitOne.text = "Malicious Wind-Up" + " (Fated)" + '\n' +
                    "Inflicting Damage increases Reload Speed by 1.5%. This bonus activates on your next reload. Kills restore 5% of this weapon's reserves.";
                }
            }

            //Positive-Negative
            if (inventory[selection].GetComponent<FirearmScript>().fcnChtOne > 430 && inventory[selection].GetComponent<FirearmScript>().fcnChtOne <= 440)
            {
                cheatTraitOne.text = "Positive-Negative" + '\n' +
                    "Moving generates a charge. When charged at least halfway, hitting an Enemy applies damage-over-time for ten seconds, inflicting 100% of Weapon damage once every second.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitOne.text = "Positive-Negative" + " (Fated)" + '\n' +
                    "Moving generates a charge. When charged at least halfway, hitting an Enemy applies damage-over-time for ten seconds, inflicting 200% of Weapon damage once every half-second.";
                }
            }

            //Good Things Come
            if (inventory[selection].GetComponent<FirearmScript>().fcnChtOne > 440)
            {
                cheatTraitOne.text = "Good Things Come" + '\n' +
                "Being in combat for three seconds grants:" + '\n' +
                "25% Movement Speed increase" + '\n' +
                "20% Damage reduction" + '\n' +
                "45% Recoil reduction" + '\n' +
                "Leaving combat for five seconds ends the bonus.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitOne.text = "Good Things Come" + " (Fated)" + '\n' +
                    "Being in combat instantly grants:" + '\n' +
                    "50% Movement Speed increase" + '\n' +
                    "40% Damage reduction" + '\n' +
                    "90% Recoil reduction" + '\n' +
                    "Infinite Ammunition" + '\n' +
                    "Leaving combat for five seconds ends the bonus.";
                }
            }

            //Wait! Now I'm Ready
            if (inventory[selection].GetComponent<FirearmScript>().fcnChtTwo <= 460)
            {
                cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                    "Kills with this weapon restore 10% of Shield strength.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitTwo.text = "Wait! Now I'm Ready!" + " (Fated)" + '\n' +
                    "Kills with this weapon restore 20% of Shield strength.";
                }
            }

            //Efficacy
            if (inventory[selection].GetComponent<FirearmScript>().fcnChtTwo > 460 && inventory[selection].GetComponent<FirearmScript>().fcnChtTwo <= 470)
            {
                cheatTraitTwo.text = "Efficacy" + '\n' +
                    "Enemy hits increases this weapon's base damage by 1%. Reloading this weapon resets its base damage.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitTwo.text = "Efficacy" + " (Fated)" + '\n' +
                    "Enemy hits increases this weapon's base damage by 2%. Base damage can increase up to 125%.";
                }
            }

            //Inoculated
            if (inventory[selection].GetComponent<FirearmScript>().fcnChtTwo > 470 && inventory[selection].GetComponent<FirearmScript>().fcnChtTwo <= 480)
            {
                cheatTraitTwo.text = "Inoculated" + '\n' +
                    "Kills with this weapon restore 5% of Health.";

                if (inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitTwo.text = "Inoculated" + " (Fated)" + '\n' +
                    "Kills with this weapon restore 10% of Health.";
                }
            }

            //Cadence
            if (inventory[selection].GetComponent<FirearmScript>().fcnChtTwo > 480 && inventory[selection].GetComponent<FirearmScript>().fcnChtTwo <= 490)
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
            if (inventory[selection].GetComponent<FirearmScript>().fcnChtTwo > 490)
            {
                cheatTraitTwo.text = "Rude Awakening" + '\n' +
                    "Kills grant up to three stacks of a lethal AOE blast that inflicts 1,000% of Weapon damage." + '\n' +
                    "'Space' - Cast Blast";

                if(inventory[selection].GetComponent<FirearmScript>().weaponRarity == 5)
                {
                    cheatTraitTwo.text = "Rude Awakening" + " (Fated)" + '\n' +
                    "Kills grant up to six stacks of a lethal AOE blast that inflicts 1,000% of Weapon damage. One kill counts for two stacks. Having any stack increases Weapon damage by 20%." + '\n' +
                    "'Space' - Cast Blast";
                }
            }          
        }


        //Extended Functions for Exotics -- Cheat 5
        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -1)
        {
            cheatTraitOne.text = "Equivalent Exchange" + '\n' +
                "Taking enemy damage adds 35% of damage received to this weapon's base damage. Base damage can increase up to 150%. 35% of enemy damage received is added back as health.";

            cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                   "Kills with this weapon restore 10% of Shield strength.";
        } //Equivalent Exchange + Wait! Now I'm Ready!

        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -2)
        {
            cheatTraitOne.text = "Absolutely No Stops" + '\n' +
                "Expending your magazine fills it from reserves, amplifies damage by 200%, and increases Rate of Fire by 50%. This bonus ends when ammo reserves are depleted or if you stop firing.";

            cheatTraitTwo.text = "Good Things Come" + '\n' +
                "Being in combat for three seconds grants:" + '\n' +
                "25% Movement Speed increase" + '\n' +
                "20% Damage reduction" + '\n' +
                "45% Recoil reduction" + '\n' +
                "Leaving combat for five seconds ends the bonus.";
        } //Absolutely No Stops + Good Things Come

        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -3)
        {
            cheatTraitOne.text = "Shelter in Place" + '\n' +
                "Refraining from moving amplifies damage by 100% and reduces incoming damage by 80%. Resuming movement ends the bonus. Knockback from enemy attacks cannot remove this bonus.";

            cheatTraitTwo.text = "Positive-Negative" + '\n' +
                    "Moving generates a charge. When charged at least halfway, hitting an Enemy applies damage-over-time for ten seconds, inflicting 100% of Weapon damage once every second.";
        } //Shelter in Place + Positive-Negative

        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -4)
        {
            cheatTraitOne.text = "Social Distance, please!" + '\n' +
                "Hitting an enemy applies a debuff that doubles damage taken by this weapon and increases damage by 30%. Hitting un-debuffed enemies refreshes the timer. Kills spread 400% of damage to nearby enemies.";

            cheatTraitTwo.text = "Not with a Stick" + '\n' +
                    "Kills with this weapon increase Effective Range by 30% of max Range until your next reload.";
        } //Social Distance, Please! + Not with a Stick

        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -5)
        {
            cheatTraitOne.text = "The Early Berth gets the Hearst" + '\n' +
                "Enemy hits have a chance to trigger a Replevy Berth explosion.";

            cheatTraitTwo.text = "Efficacy" + '\n' +
                    "Enemy hits increases this weapon's base damage by 1%. Reloading this weapon resets its base damage.";
        } //Early Berth gets the Hearst + Efficacy

        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -6)
        {
            cheatTraitOne.text = "Off Your Own Supply" + '\n' +
                "'Space' - Sacrifice your shield for:" + '\n' +
                "10% Movement Speed increase" + '\n' +
                "80% Reload Speed increase" + '\n' +
                "Zeroed Recoil" + '\n' + 
                "140% Weapon damage increase" + '\n' +
                "The Shield's regeneration delay is paused until the bonus ends.";

            cheatTraitTwo.text = "Inoculated" + '\n' +
                    "Kills with this weapon restore 5% of Health.";
        } //Off your own Supply + Inoculated

        if (inventory[selection].GetComponent<FirearmScript>().cheatRNG == -7)
        {
            cheatTraitOne.text = "Pay to Win" + '\n' +
                "'Space' - Consume 10,000 Lucent currency to create 150 stacks of a 50% base damage increase. Enemy hits removes three stacks.";

            cheatTraitTwo.text = "Malicious Wind-Up" + '\n' +
                    "Inflicting Damage increases Reload Speed by 0.75%. This bonus activates on your next reload.";
        } //Pay to Win + Malicious Wind-Up
    }

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

    public void WriteOnReset()
    {
        CreateInventoryFile();

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

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
                        {
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

                            if (inventory[i].GetComponent<HastyHands>())
                            {
                                write.WriteLine("7");
                            }

                            if (inventory[i].GetComponent<HastierHands>())
                            {
                                write.WriteLine("8");
                            }
                        }                        

                        if(inventory[i].GetComponent<FirearmScript>().weaponRarity == 2 || inventory[i].GetComponent<FirearmScript>().weaponRarity == 3)
                        {
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
                        }

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity >= 4)
                        {
                           
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
                                if (inventory[i].GetComponent<EquivalentExchange>())
                                {
                                    write.Write("A");
                                }

                                if (inventory[i].GetComponent<WaitNowImReady>())
                                {
                                    write.WriteLine("0");
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
                            }        
                            
                        }
                        
                    }

                    if (inventory[i].name == "testMachineGun" || inventory[i].name == "testMachineGun_Exotic")
                    {
                        write.Write("2");
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

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
                        {
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
                        }

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity >= 4)
                        {

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
                                if (inventory[i].GetComponent<PayToWin>())
                                {
                                    write.Write("G");
                                }

                                if (inventory[i].GetComponent<MaliciousWindUp>())
                                {
                                    write.WriteLine("5");
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
                            }

                        }
                    }

                    if (inventory[i].name == "testPistol" || inventory[i].name == "testPistol_Exotic")
                    {
                        write.Write("3");
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

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
                        {
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
                        }

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity >= 4)
                        {

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
                                if (inventory[i].GetComponent<ShelterInPlace>())
                                {
                                    write.Write("C");
                                }

                                if (inventory[i].GetComponent<PositiveNegative>())
                                {
                                    write.WriteLine("6");
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
                            }

                        }
                    }

                    if (inventory[i].name == "testSemiFireRifle" || inventory[i].name == "testSemiFireRifle_Exotic")
                    {
                        write.Write("4");
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

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
                        {
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
                        }

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity >= 4)
                        {

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
                                if (inventory[i].GetComponent<OffYourOwnSupply>())
                                {
                                    write.Write("F");
                                }

                                if (inventory[i].GetComponent<Inoculated>())
                                {
                                    write.WriteLine("2");
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
                            }
                        }
                    }

                    if (inventory[i].name == "testShotgun" || inventory[i].name == "testShotgun_Exotic")
                    {
                        write.Write("5");
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

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
                        {
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
                        }

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity >= 4)
                        {

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
                                if (inventory[i].GetComponent<SocialDistancePlease>())
                                {
                                    write.Write("D");
                                }

                                if (inventory[i].GetComponent<NotWithAStick>())
                                {
                                    write.WriteLine("4");
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
                            }
                        }
                    }

                    if (inventory[i].name == "testSingleFireRifle" || inventory[i].name == "testSingleFireRifle_Exotic")
                    {
                        write.Write("6");
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

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
                        {
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
                        }

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity >= 4)
                        {

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
                                if (inventory[i].GetComponent<EarlyBerthGetsTheHearst>())
                                {
                                    write.Write("E");
                                }

                                if (inventory[i].GetComponent<Efficacy>())
                                {
                                    write.WriteLine("1");
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
                            }
                        }
                    }

                    if (inventory[i].name == "testSMG" || inventory[i].name == "testSMG_Exotic")
                    {
                        write.Write("7");
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

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity == 1)
                        {
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
                        }

                        if (inventory[i].GetComponent<FirearmScript>().weaponRarity >= 4)
                        {

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
                                if (inventory[i].GetComponent<AbsolutelyNoStops>())
                                {
                                    write.Write("B");
                                }

                                if (inventory[i].GetComponent<GoodThingsCome>())
                                {
                                    write.WriteLine("8");
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

    private void WriteInventory()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            WriteOnReset();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            AddInv(other.gameObject);
        }
    }
}
