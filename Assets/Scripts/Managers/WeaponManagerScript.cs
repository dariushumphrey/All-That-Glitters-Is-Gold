using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class WeaponManagerScript : MonoBehaviour
{
    //Configuration for Main Menu or Gameplay behavior
    public enum Setting
    {
        Menu = 0, Gameplay = 1
    }

    public Setting setting;

    public List<GameObject> weapons = new List<GameObject>(); //List of Weapon variants used for attribute assignment, spawning

    public List<string> observedWeps = new List<string>(10); //List of Weapons from inventory.txt
    public string filepath = "inventory.txt"; //Inventory file name
    public Text wepName, flavor, rarityCheck, invMonitor, stats, dismantleText, appraisal, favNotice, wepPlatform; //Texts that display Weapon stats, worth, and dismantle notices
    public Text cheatOne, cheatTwo, cheatThree, cheatFour, cheatTraitOne, cheatTraitTwo; //Texts that displays Weapon cheats
    public Button sellButton;

    //lucentText - Displays Player Lucent balance
    //sortingText - Displays active Inventory sorting type
    public Text lucentText, sortingText, favoriteText;
    public GameObject invNavigation; //Collection of on-screen Inventory page navigation buttons
    public float dismantleTimer;

    internal int selection; //Index used to select Inventory weapons
    private GameObject item; //GameObject that receives Weapons 
    private PlayerInventoryScript player;
    private MenuManagerScript menu;
    private KioskScript kiosk;
    private TransitionManagerScript transition;
    private bool sortToggle = false;
    private bool automateLeft, automateRight;
    private bool invInputHold = false;
    private int automateSelection = 0; //Searches for favorite Weapons on behalf of selection (variable)
    private string favCheck;
    private char[] weaponToModify;
    string wepStr, rarStr, exoStr, cOneStr, cTwoStr, cThreeStr, cFourStr, cFiveStr, cSixStr, favStr, pltStr; //Strings that describe Weapon attributes
    float dismantleTimerReset, spawnDelayTimer;
    bool track = true; //WeaponManager position assigned to Player position if true

    void Awake()
    {
        //Turns Weapon variant display setting off
        //Reads inventory.txt and begins Weapon respawns when in Gameplay      
        if(setting == Setting.Gameplay)
        {
            player = FindObjectOfType<PlayerInventoryScript>();
            transition = FindObjectOfType<TransitionManagerScript>();

            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].GetComponent<FirearmScript>().display = false;
            }

            ReadOnReload();

            spawnDelayTimer = 0.02f;
            StartCoroutine(RespawnWeapons());
        }

        //Turns Weapon variant display setting on
        //Reads inventory.txt and adds Weapons for review when on Main Menu
        else
        {
            selection = -1;

            menu = FindObjectOfType<MenuManagerScript>();
            kiosk = FindObjectOfType<KioskScript>();
            if(observedWeps.Count <= 0)
            {
                ObserveOnLoad();
            }

            dismantleTimerReset = dismantleTimer;
            dismantleText.text = " ";

            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].GetComponent<FirearmScript>().display = true;
            }
        }
    }

    private void Start()
    {
        if(setting == Setting.Menu)
        {
            if(observedWeps.Count <= 0)
            {
                wepName.text = "Your Inventory is empty! Acquire weapons in order to inspect them here.";
                flavor.text = "";
                rarityCheck.text = "";
                invMonitor.text = "";
                stats.text = "";
                dismantleText.text = "";
                cheatOne.text = "";
                cheatTwo.text = "";
                cheatThree.text = "";
                cheatFour.text = "";
                cheatTraitOne.text = "";
                cheatTraitTwo.text = "";
                appraisal.text = "";

                invNavigation.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(setting == Setting.Menu)
        {
            if (observedWeps.Count <= 0)
            {
                wepName.text = "Your Inventory is empty! Acquire weapons in order to inspect them here.";
                flavor.text = "";
                rarityCheck.text = "";
                invMonitor.text = "";
                stats.text = "";
                dismantleText.text = "";
                cheatOne.text = "";
                cheatTwo.text = "";
                cheatThree.text = "";
                cheatFour.text = "";
                cheatTraitOne.text = "";
                cheatTraitTwo.text = "";
                appraisal.text = "";

                invNavigation.SetActive(false);

            }

            //Activates Inventory page navigation, dismantle behaviors, tracks Inventory size
            else
            {
                if(!invInputHold)
                {
                    SwitchInv();
                    DismantleInv();
                    ChangeFavoriteState();
                }

                invMonitor.text = (selection + 1) + " / " + observedWeps.Count;

                if(sortToggle)
                {
                    sortingText.text = "Sort by: Favorite";
                }

                else
                {
                    sortingText.text = "Sort by: All";
                }
            }

            lucentText.text = "Lucent: " + kiosk.lucentFunds.ToString("N0");

        }

        if(setting == Setting.Gameplay)
        {
            if(track)
            {
                gameObject.transform.position = player.gameObject.transform.position;
            }
        }
    }

    /// <summary>
    /// Reads inventory.txt and adds contents to observedWeps
    /// </summary>
    public void ObserveOnLoad()
    {
        if (File.Exists(filepath))
        {
            using (StreamReader read = new StreamReader(filepath))
            {
                while (!read.EndOfStream) //while you have yet to reach the end of the file
                {
                    observedWeps.Add(read.ReadLine());
                   
                }
            }
        }

        else
        {
            CreateInventoryFile();
        }
    }

    /// <summary>
    /// Reads inventory.txt and adds contents to readdedWeps (from PlayerInventoryScript)
    /// </summary>
    private void ReadOnReload()
    {
        if (File.Exists(player.filepath))
        {
            using (StreamReader read = new StreamReader(player.filepath))
            {
                while (!read.EndOfStream) //while you have yet to reach the end of the file
                {
                    player.readdedWeps.Add(read.ReadLine());
                    //Debug.Log(read.ReadLine());
                }
            }
        }
    }

    /// <summary>
    /// Displays Weapon attributes based on string information from inventory.txt
    /// </summary>
    private void DisplayWeapons()
    {
        if(gameObject.transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        string c = "Comic Sans";

        c = observedWeps[selection];
        wepStr = c[0].ToString();
        rarStr = c[1].ToString();
        exoStr = c[2].ToString();
        favStr = c[3].ToString();
        pltStr = c[4].ToString();

        if (observedWeps[selection].Length == 9)
        {
            cOneStr = c[5].ToString();
            cTwoStr = c[6].ToString();
            cThreeStr = c[7].ToString();
            cFourStr = c[8].ToString();
        }

        if (observedWeps[selection].Length == 10)
        {
            cOneStr = c[5].ToString();
            cTwoStr = c[6].ToString();
            cThreeStr = c[7].ToString();
            cFourStr = c[8].ToString();
            cFiveStr = c[9].ToString();
        }

        if (observedWeps[selection].Length == 11)
        {
            cOneStr = c[5].ToString();
            cTwoStr = c[6].ToString();
            cThreeStr = c[7].ToString();
            cFourStr = c[8].ToString();
            cFiveStr = c[9].ToString();
            cSixStr = c[10].ToString();
        }

        invMonitor.text = (selection + 1) + " / " + observedWeps.Count;

        if (wepStr == "0")
        {
            //Debug.Log("Displaying Full Fire Rifle");
            item = Instantiate(weapons[0], transform.position, transform.rotation);
            item.name = weapons[0].name;
            wepName.text = "Full Fire Rifle";
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;                       
        }

        if (wepStr == "1")
        {
            //Debug.Log("Displaying Machine Gun");
            item = Instantiate(weapons[1], transform.position, transform.rotation);
            item.name = weapons[1].name;
            wepName.text = "Machine Gun";
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;           
        }

        if (wepStr == "2")
        {
            //Debug.Log("Displaying Pistol");
            item = Instantiate(weapons[2], transform.position, transform.rotation);
            item.name = weapons[2].name;
            wepName.text = "Pistol";
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;          
        }

        if (wepStr == "3")
        {
            //Debug.Log("Displaying Semi Fire Rifle");
            item = Instantiate(weapons[3], transform.position, transform.rotation);
            item.name = weapons[3].name;
            wepName.text = "Semi Fire Rifle";
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;         
        }

        if (wepStr == "4")
        {
            //Debug.Log("Displaying Shotgun");
            item = Instantiate(weapons[4], transform.position, transform.rotation);
            item.name = weapons[4].name;
            wepName.text = "Shotgun";
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;         
        }

        if (wepStr == "5")
        {
            //Debug.Log("Displaying Single Fire Rifle");
            item = Instantiate(weapons[5], transform.position, transform.rotation);
            item.name = weapons[5].name;
            wepName.text = "Single Fire Rifle";
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;          
        }

        if (wepStr == "6")
        {
            //Debug.Log("Displaying Submachine Gun");
            item = Instantiate(weapons[6], transform.position, transform.rotation);
            item.name = weapons[6].name;
            wepName.text = "Submachine Gun";
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;         
        }

        if (wepStr == "7")
        {
            //Debug.Log("Displaying Grenade Launcher");
            item = Instantiate(weapons[7], transform.position, transform.rotation);
            item.name = weapons[7].name;
            wepName.text = "Grenade Launcher";
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;         
        }

        item.transform.parent = gameObject.transform;

        if (pltStr == "1")
        {
            wepPlatform.text = "Default Platform - Standard performance.";
            item.AddComponent<DefaultPlatform>();
        }

        if (pltStr == "2")
        {
            wepPlatform.text = "Efficient Platform - Configured for slow-firing, high-damage.";
            item.AddComponent<EfficientPlatform>();
            item.GetComponent<EfficientPlatform>().Start();
        }

        if (pltStr == "3")
        {
            wepPlatform.text = "Chatter Platform - Configured for fast-firing, low-damage.";
            item.AddComponent<ChatterPlatform>();
            item.GetComponent<ChatterPlatform>().Start();
        }

        if (pltStr == "4")
        {
            wepPlatform.text = "Tempered Platform - Tuned for highest damage, improved firing and control.";
            item.AddComponent<TemperedPlatform>();
            item.GetComponent<TemperedPlatform>().Start();
        }

        if (pltStr == "5")
        {
            wepPlatform.text = "Siphonic Platform - Weapon hits restore 1% Health & Shield. Melee Kills restore 15% Health & Shield.";
            item.AddComponent<SiphonicPlatform>();
            item.GetComponent<SiphonicPlatform>().Start();
        }

        if (pltStr == "6")
        {
            wepPlatform.text = "Mining Platform - Fires Lucent explosive rounds.";
            item.AddComponent<MiningPlatform>();
            item.GetComponent<MiningPlatform>().Start();
        }

        if (pltStr == "7")
        {
            wepPlatform.text = "Trenchant Platform -" + "\n" + 
                "Evasions slow enemies." + "\n" +
                "Melees apply damage-over-time." + "\n" + 
                "Weapon hits apply Health debuffs.";
            item.AddComponent<TrenchantPlatform>();
            item.GetComponent<TrenchantPlatform>().Start();
        }

        if (pltStr == "8")
        {
            wepPlatform.text = "Cache Platform - Regenerates all grenades every two seconds." + "\n" +
                "Activator Drones fire mini-Rockets.";
            item.AddComponent<CachePlatform>();
            item.GetComponent<CachePlatform>().Start();
        }

        if (rarStr == "1")
        {
            rarityCheck.text = "Usual";

            cheatOne.text = "";
            cheatTwo.text = "";
            cheatThree.text = "";
            cheatFour.text = "";
            cheatTraitOne.text = " ";
            cheatTraitTwo.text = " ";
        }

        if (rarStr == "2")
        {
            rarityCheck.text = "Sought";

            cheatTraitOne.text = " ";
            cheatTraitTwo.text = " ";
            item.GetComponent<FirearmScript>().weaponRarity = 2;
            item.GetComponent<FirearmScript>().RarityAugment();
        }

        if (rarStr == "3")
        {
            rarityCheck.text = "Coveted";
            cheatTraitTwo.text = " ";
            item.GetComponent<FirearmScript>().weaponRarity = 3;
            item.GetComponent<FirearmScript>().RarityAugment();
        }

        if (rarStr == "4")
        {
            rarityCheck.text = "Treasured";
            item.GetComponent<FirearmScript>().weaponRarity = 4;
            item.GetComponent<FirearmScript>().RarityAugment();
        }

        flavor.text = item.GetComponent<FirearmScript>().flavorText;

        if (rarStr == "5")
        {
            rarityCheck.text = "Fated";
            item.GetComponent<FirearmScript>().weaponRarity = 5;

            if (exoStr == "1")
            {
                if (wepStr == "0")
                {
                    wepName.text = "Outstanding Warrant";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "''The time has come to collect.'' ";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[0].name + "_Exotic";
                }

                if (wepStr == "1")
                {
                    wepName.text = "The Dismissal";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "''Wealth is found in the hoard. The Replevin know this; They are territorial, but are otherwise unbothersome.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[1].name + "_Exotic";
                }

                if (wepStr == "2")
                {
                    wepName.text = "Apathetic";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "At what point do the restraints simply fail?";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[2].name + "_Exotic";
                }

                if (wepStr == "3")
                {
                    wepName.text = "Mercies";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "FORAGER operatives are known for on-site procurement tactics. ''Looting'' is a word the robbed often use.";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[3].name + "_Exotic";
                }

                if (wepStr == "4")
                {
                    wepName.text = "Viral Shadow";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "''The Resplendent fell victim to a first-of-its-kind case of ''Interstellar agro-terrorism''. This was no mere plot.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[4].name + "_Exotic";
                }

                if (wepStr == "5")
                {
                    wepName.text = "Contempt For Fellows";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "''Force-feeding Replevin Lucent, a resource known to modify behavior on ingestion, defies most known interstellar laws.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[5].name + "_Exotic";
                }

                if (wepStr == "6")
                {
                    wepName.text = "Underfoot";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "Using this Weapon feels like a perpetual Calvary charge. For where you're going, you won't be needing any breaks.";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[6].name + "_Exotic";
                }

                if (wepStr == "7")
                {
                    wepName.text = "Nebulous At Best";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "Just when you thought you understood how the world works.";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[7].name + "_Exotic";
                }
            }

            else
            {
                item.GetComponent<FirearmScript>().isExotic = false;
                item.GetComponent<FirearmScript>().RarityAugment();

                flavor.text = item.GetComponent<FirearmScript>().flavorText;
            }
        }

        if (favStr == "1")
        {
            favNotice.text = "*";
            sellButton.interactable = false;
            favoriteText.text = "Unmark as Favorite";
            //item.GetComponent<FirearmScript>().favorite = true;
        }

        else
        {
            favNotice.text = "";
            sellButton.interactable = true;
            favoriteText.text = "Mark as Favorite";
            //item.GetComponent<FirearmScript>().favorite = false;
        }

        //Changes Weapon statistics by Weapon type (Rate of Fire primarily changes)
        if (wepStr == "0" || wepStr == "1" || wepStr == "6")
        {
            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 10000).ToString() + " RPM";
        }

        else if (wepStr == "2" || wepStr == "3" || wepStr == "5")
        {
            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 1000).ToString() + " RPM";
        }

        else if (wepStr == "4" || wepStr == "7")
        {
            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 100).ToString() + " RPM";
        }

        if (observedWeps[selection].Length >= 9)
        {
            if (cOneStr == "1")
            {
                cheatOne.text = "Deep Yield" + "\n" + "<i>12% Magazine Size</i>";
                item.AddComponent<DeepYield>();
            }

            if (cOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "<i>24% Magazine Size</i>";
                item.AddComponent<DeeperYield>();
            }

            if (cTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "<i>15% Reserves Size</i>";
                item.AddComponent<DeepStores>();
            }

            if (cTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "<i>30% Reserves Size</i>";
                item.AddComponent<DeeperStores>();
            }

            if (cThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "<i>10% Effective Range Increase</i>";
                item.AddComponent<FarSight>();
            }

            if (cThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "<i>20% Effective Range Increase</i>";
                item.AddComponent<FartherSight>();
            }

            if (cFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "<i>15% Reload Speed Increase</i>";
                item.AddComponent<HastyHands>();
            }

            if (cFourStr == "8")
            {
                cheatFour.text = "Hastier Hands" + "\n" + "<i>25% Reload Speed Increase</i>";
                item.AddComponent<HastierHands>();
            }

            if (observedWeps[selection].Length == 10)
            {
                if (cFiveStr == "0")
                {
                    cheatTraitOne.text = "Wait! Now I'm Ready!" + "\n" +
                        "Kills with this Weapon restore 10% of Shield strength.";

                }

                if (cFiveStr == "1")
                {
                    cheatTraitOne.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";
                }

                if (cFiveStr == "2")
                {
                    cheatTraitOne.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";
                }

                if (cFiveStr == "3")
                {
                    cheatTraitOne.text = "Rude Awakening" + '\n' +
                        "Kills grant casts of a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x." + '\n' +
                        "'E' - Cast Blast";
                }

                if (cFiveStr == "4")
                {
                    cheatTraitOne.text = "Not with a Stick" + '\n' +
                        "Kills with this Weapon increase Effective Range by 30% of max Range until your next reload.";
                }

                if (cFiveStr == "5")
                {
                    cheatTraitOne.text = "Malicious Wind-Up" + '\n' +
                        "Inflicting damage increases Reload Speed by 0.75%. This bonus activates on your next reload.";
                }

                if (cFiveStr == "6")
                {
                    cheatTraitOne.text = "Positive-Negative" + '\n' +
                        "Moving generates a charge. While halfway charged, Enemy hits applies 100% of Weapon damage as damage-over-time for ten seconds.";
                }

                if (cFiveStr == "7")
                {
                    cheatTraitOne.text = "Cadence" + '\n' +
                        "Every third Enemy kill spawns a Lucent cluster.";
                }

                if (cFiveStr == "8")
                {
                    cheatTraitOne.text = "Good Things Come" + '\n' +
                        "Being in combat for three seconds grants 25% Movement Speed, 20% damage reduction, and 45% Recoil reduction until you leave combat.";
                }

                if (cFiveStr == "9")
                {
                    cheatTraitOne.text = "All Else Fails" + '\n' +
                        "When Shield is depleted, all incoming Enemy damage is nullified for three seconds. Cooldown: 20 Seconds.";
                }

                if (cFiveStr == "!")
                {
                    cheatTraitOne.text = "The Most Resplendent" + '\n' +
                    "Create a Hard Lucent crystal on surfaces or Enemies that produces Lucent clusters passively or when shot." + '\n' +
                    "'[E]' - Toggle cast";
                }

                if (cFiveStr == "@")
                {
                    cheatTraitOne.text = "Fulminate" + '\n' +
                    "Enemy hits increase Destruct Grenade damage by 2%, up to 70%, for seven seconds. Melee kills cast a Destruct Grenade.";
                }

                if (cFiveStr == "#")
                {
                    cheatTraitOne.text = "Forager" + '\n' +
                    "Weapon or Melee kills produce a burst of Lucent clusters, 1% Health, 2% Shield, and 15% Ammo pickups.";
                }

                if (cFiveStr == "$")
                {
                    cheatTraitOne.text = "Counterplay" + '\n' +
                    "Hits taken while immune during Evasions casts two Lucent clusters and permanently increases Weapon damage by 10%. Stacks 3x.";
                }

                if (cFiveStr == "%")
                {
                    cheatTraitOne.text = "Enshroud" + '\n' +
                    "Enemy hits increase Melee range by 15%, up to 200%, for seven seconds. Melee kills cast a Fogger Grenade. Cooldown: 12 seconds.";
                }

                if (cFiveStr == "^")
                {
                    cheatTraitOne.text = "Gale Force Winds" + '\n' +
                    "Cast traveling winds from Sprinting or moving that applies Health and Slowed debuffs to Enemies." + '\n' +
                    "'[E]' - Toggle cast";
                }

                if (cFiveStr == "&")
                {
                    cheatTraitOne.text = "Activator Drone" + '\n' +
                        "Passively attacks enemies, or can receive a target by aiming. Attacks trigger weapon passives.";
                }

                cheatTraitTwo.text = " ";
            }

            if (observedWeps[selection].Length == 11)
            {
                //Exotic Functional Cheats
                if (cFiveStr == "A")
                {
                    cheatTraitOne.text = "Equivalent Exchange" + '\n' +
                        "Taking Enemy damage adds 35% of damage received to this Weapon's base damage and to your Health. Base damage can increase up to 150%.";
                }

                if (cFiveStr == "G")
                {
                    cheatTraitOne.text = "Pay to Win" + '\n' +
                        "Consume 30,000 Lucent to grant stacks of a 50% Weapon damage increase. Stacks 150x." + "\n" +
                        "'E' - Consume Lucent";
                }

                if (cFiveStr == "C")
                {
                    cheatTraitOne.text = "Superweapon" + '\n' +
                        "Kills grant stacks of damage resistance. Stacks 8x. [E] - Charge an extreme-damage shot, inflicting 1000% of Weapon damage per stack.";
                }

                if (cFiveStr == "F")
                {
                    cheatTraitOne.text = "Volant" + '\n' +
                        "[E] - Enables character flight until Shield is broken or disenaged.";
                }

                if (cFiveStr == "D")
                {
                    cheatTraitOne.text = "Social Distance, please!" + '\n' +
                        "Weapon hits temporarily increase Weapon damage by 30% and adds a Health debuff. Kills spread 400% of Weapon damage to nearby enemies.";
                }

                if (cFiveStr == "E")
                {
                    cheatTraitOne.text = "The Early Berth gets the Hearst" + '\n' +
                        "Every other Enemy hit triggers a Berth detonation, inflicting 200% of Weapon damage.";
                }

                if (cFiveStr == "B")
                {
                    cheatTraitOne.text = "Absolutely No Stops" + '\n' +
                        "Expending your magazine fills it from reserves, amplifies Weapon damage by 200%, and increases Rate of Fire by 50%.";
                }

                if (cFiveStr == "H")
                {
                    cheatTraitOne.text = "Flashpoint" + '\n' +
                        "Fires floating Lucent mines. [E] - Detonates all active mines.";
                }


                if (cFiveStr == "9")
                {
                    cheatTraitOne.text = "All Else Fails" + '\n' +
                        "When Shield is depleted, all incoming Enemy damage is nullified for three seconds. Cooldown: 20 Seconds.";

                    if (rarStr == "5")
                    {
                        cheatTraitOne.text = "All Else Fails" + " (Fated)" + '\n' +
                        "When Shield is depleted, all incoming Enemy damage is nullified for five seconds. Cooldown: 10 Seconds.";
                    }
                }

                if (cFiveStr == "4")
                {
                    cheatTraitOne.text = "Not with a Stick" + '\n' +
                        "Kills with this Weapon increase Effective Range by 30% of max Range until your next reload.";

                    if (rarStr == "5")
                    {
                        cheatTraitOne.text = "Not with a Stick" + " (Fated)" + '\n' +
                        "Kills increase Effective Range by 30% of max Range. Maximizing Effective Range increases Aim Assist halfway to full strength for 20 seconds.";
                    }
                }

                if (cFiveStr == "5")
                {
                    cheatTraitOne.text = "Malicious Wind-Up" + '\n' +
                        "Inflicting Damage increases Reload Speed by 0.75%. This bonus activates on your next reload.";

                    if (rarStr == "5")
                    {
                        cheatTraitOne.text = "Malicious Wind-Up" + " (Fated)" + '\n' +
                        "Inflicting Damage increases Reload Speed by 1.5%. Kills restore 5% of this weapon's reserves.";
                    }
                }

                if (cFiveStr == "6")
                {
                    cheatTraitOne.text = "Positive-Negative" + '\n' +
                        "Moving generates a charge. While halfway charged, Enemy hits applies 100% of Weapon damage as damage-over-time for ten seconds.";

                    if (rarStr == "5")
                    {
                        cheatTraitOne.text = "Positive-Negative" + " (Fated)" + '\n' +
                        "Moving generates a charge. While halfway charged, Enemy hits applies 200% of Weapon damage as damage-over-time for ten seconds.";
                    }
                }

                if (cFiveStr == "8")
                {
                    cheatTraitOne.text = "Good Things Come" + '\n' +
                        "Being in combat for three seconds grants 25% Movement Speed, 20% damage reduction, and 45% Recoil reduction until you leave combat.";

                    if (rarStr == "5")
                    {
                        cheatTraitOne.text = "Good Things Come" + " (Fated)" + '\n' +
                            "Being in combat instantly grants 50% Movement Speed, 40% damage reduction, 90% Recoil reduction, and Infinite Ammo until you leave combat.";

                    }

                }

                if (cFiveStr == "!")
                {
                    cheatTraitOne.text = "The Most Resplendent" + '\n' +
                    "Create a Hard Lucent crystal on surfaces or Enemies that produces Lucent clusters passively or when shot." + '\n' +
                    "'[E]' - Toggle cast";

                    if (rarStr == "5")
                    {
                        cheatTraitOne.text = "The Most Resplendent" + " (Fated)" + '\n' +
                            "Create a Hard Lucent crystal that produces Lucent clusters. " +
                            "Stacks 2x. Physically colliding with the crystal shatters it, restoring 35% of Health.";
                    }
                }

                if (cFiveStr == "@")
                {
                    cheatTraitOne.text = "Fulminate" + '\n' +
                    "Enemy hits increase Destruct Grenade damage by 2%, up to 70%, for seven seconds. Melee kills cast a Destruct Grenade.";

                    if (rarStr == "5")
                    {
                        cheatTraitOne.text = "Fulminate" + " (Fated)" + '\n' +
                            "Enemy hits increase Destruct Grenade damage by 2%, up to 70%. Melee kills cast a Destruct Grenade. " +
                            "Passively throw two Destruct Grenades.";
                    }
                }

                if (cFiveStr == "#")
                {
                    cheatTraitOne.text = "Forager" + '\n' +
                    "Weapon or Melee kills produce a burst of Lucent clusters, 1% Health, 2% Shield, and 15% Ammo pickups.";

                    if (rarStr == "5")
                    {
                        cheatTraitOne.text = "Forager" + " (Fated)" + '\n' +
                            "Weapon or Melee kills produce bursts of Lucent clusters, 2% Health, 4% Shield, and 30% Ammo pickups. " +
                            "Every 10th Boss hit produces one burst.";
                    }
                }

                if (cFiveStr == "&")
                {
                    cheatTraitOne.text = "Activator Drone" + '\n' +
                        "Passively attacks enemies, or can receive a target by aiming. Attacks trigger weapon passives.";

                    if (rarStr == "5")
                    {
                        cheatTraitOne.text = "Activator Drone" + " (Fated)" + '\n' +
                        "The drone can activate Cheats ''Wait! Now I'm Ready!'', Efficacy, Inoculated, Cadence, and Enshroud.";
                    }
                }


                //Pay to Win pairing
                if (cSixStr == "!")
                {
                    cheatTraitTwo.text = "The Most Resplendent" + '\n' +
                            "[E] - Create a Hard Lucent crystal that produces Lucent clusters passively or when shot. Stacks 1x.";
                }

                //Social Distance, Please! pairing
                if (cSixStr == "4")
                {
                    cheatTraitTwo.text = "Not with a Stick" + '\n' +
                            "Kills with this Weapon increase Effective Range by 30% of max Range until your next reload.";
                }

                //Absolutely no breaks! Pairing
                if (cSixStr == "#")
                {
                    cheatTraitTwo.text = "Forager" + '\n' +
                            "Weapon or Melee kills produce a burst of Lucent clusters, Health, Shield, and Ammo pickups.";
                }

                //Flashpoint Pairing
                if (cSixStr == "6")
                {
                    cheatTraitTwo.text = "Positive-Negative" + '\n' +
                        "Moving generates a charge. While halfway charged, Enemy hits applies 100% of Weapon damage as damage-over-time for ten seconds.";
                }

                //Volant pairing
                if (cSixStr == "0")
                {
                    cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                           "Kills with this Weapon restore 10% of Shield strength.";

                    if (rarStr == "5" && exoStr != "1")
                    {
                        cheatTraitTwo.text = "Wait! Now I'm Ready!" + " (Fated)" + '\n' +
                        "Kills with this Weapon restore 20% of Shield strength.";
                    }

                }

                //Early Berth gets the Hearst pairing
                if (cSixStr == "1")
                {
                    cheatTraitTwo.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";

                    if (rarStr == "5")
                    {
                        cheatTraitTwo.text = "Efficacy" + " (Fated)" + '\n' +
                        "Enemy hits increases this Weapon's base damage by 2%. Base damage can increase up to 125%, and cannot be reset on reloads.";
                    }

                }

                //Equivalent Exchange pairing
                if (cSixStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";

                    if (rarStr == "5")
                    {
                        cheatTraitTwo.text = "Inoculated" + " (Fated)" + '\n' +
                        "Kills with this Weapon restore 10% of Health.";
                    }

                }

                if (cSixStr == "7")
                {
                    cheatTraitTwo.text = "Cadence" + '\n' +
                        "Every third Enemy kill spawns a Lucent cluster.";

                    if (rarStr == "5")
                    {
                        cheatTraitTwo.text = "Cadence" + " (Fated)" + '\n' +
                        "Every third Weapon hit spawns a Lucent cluster.";
                    }

                }

                if (cSixStr == "3")
                {
                    cheatTraitTwo.text = "Rude Awakening" + '\n' +
                         "Kills grant casts of a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x." + '\n' +
                        "'E' - Cast Blast";

                    if (rarStr == "5")
                    {
                        cheatTraitTwo.text = "Rude Awakening" + " (Fated)" + '\n' +
                        "Kills grant casts of a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 6x. Having any stack increases Weapon damage by 20%.";
                    }

                }

                //Superweapon pairing
                if (cSixStr == "$")
                {
                    cheatTraitTwo.text = "Counterplay" + '\n' +
                    "Hits taken while immune during Evasions casts two Lucent clusters and permanently increases Weapon damage by 10%. Stacks 3x.";

                    if (rarStr == "5")
                    {
                        cheatTraitTwo.text = "Counterplay" + " (Fated)" + '\n' +
                        "Hits taken during Evasions casts two Lucent clusters, a Solution Grenade, and permanently increases Weapon damage by 10%. Stacks 10x.";
                    }
                }

                if (cSixStr == "%")
                {
                    cheatTraitTwo.text = "Enshroud" + '\n' +
                    "Enemy hits increase Melee range by 15%, up to 200%, for seven seconds. Melee kills cast a Fogger Grenade. Cooldown: 12 seconds.";

                    if (rarStr == "5")
                    {
                        cheatTraitTwo.text = "Enshroud" + " (Fated)" + '\n' +
                        "Enemy hits increase Melee range by 15%, up to 200%. Melee kills cast a Fogger Grenade. " +
                        "All Fogger Grenades apply low damage-over-time.";
                    }
                }

                if (cSixStr == "^")
                {
                    cheatTraitTwo.text = "Gale Force Winds" + '\n' +
                    "Cast traveling winds from Sprinting or moving that applies Health and Slowed debuffs to Enemies." + '\n' +
                    "'[E]' - Toggle cast";

                    if (rarStr == "5")
                    {
                        cheatTraitTwo.text = "Gale Force Winds" + " (Fated)" + '\n' +
                        "Cast faster traveling winds that applies Slowed and stronger Health debuffs to Enemies. " +
                        "Applies damage-over-time to tracked Enemies.";
                    }
                }
            }
        }
        
        
    }

    /// <summary>
    /// Respawns Weapons based on string information from inventory.txt
    /// </summary>
    private IEnumerator RespawnWeapons()
    {
        yield return new WaitForSeconds(spawnDelayTimer);

        string c = "Comic Sans";

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
                //Debug.Log("Respawning Full Fire Rifle");
                item = Instantiate(weapons[0], transform.position, transform.rotation);
                item.name = weapons[0].name;  
            }

            if (wepStr == "1")
            {
                //Debug.Log("Respawning Machine Gun");
                item = Instantiate(weapons[1], transform.position, transform.rotation);
                item.name = weapons[1].name;           
            }

            if (wepStr == "2")
            {
                //Debug.Log("Respawning Pistol");
                item = Instantiate(weapons[2], transform.position, transform.rotation);
                item.name = weapons[2].name;          
            }

            if (wepStr == "3")
            {
                //Debug.Log("Respawning Semi Fire Rifle");
                item = Instantiate(weapons[3], transform.position, transform.rotation);
                item.name = weapons[3].name;             
            }

            if (wepStr == "4")
            {
                //Debug.Log("Respawning Shotgun");
                item = Instantiate(weapons[4], transform.position, transform.rotation);
                item.name = weapons[4].name;              
            }

            if (wepStr == "5")
            {
                //Debug.Log("Respawning Single Fire Rifle");
                item = Instantiate(weapons[5], transform.position, transform.rotation);
                item.name = weapons[5].name;           
            }

            if (wepStr == "6")
            {
                //Debug.Log("Respawning SMG");
                item = Instantiate(weapons[6], transform.position, transform.rotation);
                item.name = weapons[6].name;            
            }

            if (wepStr == "7")
            {
                //Debug.Log("Respawning Grenade Launcher");
                item = Instantiate(weapons[7], transform.position, transform.rotation);
                item.name = weapons[7].name;
            }

            if (rarStr == "1")
            {
                item.GetComponent<FirearmScript>().weaponRarity = 1;
            }

            if (rarStr == "2")
            {
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "3")
            {
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "4")
            {
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "5")
            {
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (exoStr == "1")
                {
                    if (wepStr == "0")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "''The time has come to collect.'' ";
                        item.name ="Outstanding Warrant";
                    }

                    if (wepStr == "1")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "''Wealth is found in the hoard. The Replevin know this; They are territorial, but are otherwise unbothersome.''";
                        item.name = "The Dismissal";
                    }

                    if (wepStr == "2")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "At what point do the restraints simply fail?";
                        item.name = "Apathetic";
                    }

                    if (wepStr == "3")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "FORAGER operatives are known for on-site procurement tactics. ''Looting'' is a word the robbed often use.";
                        item.name = "Mercies";
                    }

                    if (wepStr == "4")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "''The Resplendent fell victim to a first-of-its-kind case of ''Interstellar agro-terrorism''. This was no mere plot.''";
                        item.name = "Viral Shadow";
                    }

                    if (wepStr == "5")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "''Force-feeding Replevin Lucent, a resource known to modify behavior on ingestion, defies most known interstellar laws.''";
                        item.name = "Contempt For Fellows";
                    }

                    if (wepStr == "6")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "Using this Weapon feels like a perpetual Calvary charge. For where you're going, you won't be needing any breaks.";
                        item.name = "Underfoot";
                    }

                    if (wepStr == "7")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "Just when you thought you understood how the world works.";
                        item.name = "Nebulous At Best";
                    }
                }

                else if (exoStr == "0")
                {
                    item.GetComponent<FirearmScript>().isExotic = false;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }
            }

            if(favStr == "1")
            {
                item.GetComponent<FirearmScript>().favorite = true;
            }

            else
            {
                item.GetComponent<FirearmScript>().favorite = false;
            }

            if(pltStr == "1")
            {
                item.AddComponent<DefaultPlatform>();
            }

            if (pltStr == "2")
            {
                item.AddComponent<EfficientPlatform>();
            }

            if (pltStr == "3")
            {
                item.AddComponent<ChatterPlatform>();
            }

            if (pltStr == "4")
            {
                item.AddComponent<TemperedPlatform>();
            }

            if (pltStr == "5")
            {
                item.AddComponent<SiphonicPlatform>();
            }

            if (pltStr == "6")
            {
                item.AddComponent<MiningPlatform>();
            }

            if (pltStr == "7")
            {
                item.AddComponent<TrenchantPlatform>();
            }

            if (pltStr == "8")
            {
                item.AddComponent<CachePlatform>();
            }

            if(player.readdedWeps[s].Length >= 9)
            {
                if (cOneStr == "1")
                {
                    item.AddComponent<DeepYield>();
                }

                if (cOneStr == "2")
                {
                    item.AddComponent<DeeperYield>();
                }

                if (cTwoStr == "3")
                {
                    item.AddComponent<DeepStores>();
                }

                if (cTwoStr == "4")
                {
                    item.AddComponent<DeeperStores>();
                }

                if (cThreeStr == "5")
                {
                    item.AddComponent<FarSight>();
                }

                if (cThreeStr == "6")
                {
                    item.AddComponent<FartherSight>();
                }

                if (cFourStr == "7")
                {
                    item.AddComponent<HastyHands>();
                }

                if (cFourStr == "8")
                {
                    item.AddComponent<HastierHands>();
                }
            }        

            if (player.readdedWeps[s].Length == 10)
            {
                if (cFiveStr == "0")
                {
                    item.AddComponent<WaitNowImReady>();
                    item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "1")
                {
                    item.AddComponent<Efficacy>();
                    item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "2")
                {
                    item.AddComponent<Inoculated>();
                    item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "3")
                {
                    item.AddComponent<RudeAwakening>();
                    item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "4")
                {
                    item.AddComponent<NotWithAStick>();
                    item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "5")
                {
                    item.AddComponent<MaliciousWindUp>();
                    item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "6")
                {
                    item.AddComponent<PositiveNegative>();
                    item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "7")
                {
                    item.AddComponent<Cadence>();
                    item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "8")
                {
                    item.AddComponent<GoodThingsCome>();
                    item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "9")
                {
                    item.AddComponent<AllElseFails>();
                    item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "!")
                {
                    item.AddComponent<TheMostResplendent>();
                    item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "@")
                {
                    item.AddComponent<Fulminate>();
                    item.GetComponent<Fulminate>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "#")
                {
                    item.AddComponent<Forager>();              
                    item.GetComponent<Forager>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "$")
                {
                    item.AddComponent<Counterplay>();
                    item.GetComponent<Counterplay>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "%")
                {
                    item.AddComponent<Enshroud>();
                    item.GetComponent<Enshroud>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "^")
                {
                    item.AddComponent<GaleForceWinds>();
                    item.GetComponent<GaleForceWinds>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (cFiveStr == "&")
                {
                    item.AddComponent<ActivatorDrone>();
                    item.GetComponent<ActivatorDrone>().proc = item.GetComponent<FirearmScript>().procOne;
                    item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }
            }

            if (player.readdedWeps[s].Length == 11)
            {
                //Exotic Functional Cheats
                if (cFiveStr == "A")
                {
                    item.GetComponent<FirearmScript>().cheatRNG = -1;
                    item.AddComponent<EquivalentExchange>();
                    item.GetComponent<EquivalentExchange>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "G")
                {
                    item.GetComponent<FirearmScript>().cheatRNG = -7;
                    item.AddComponent<PayToWin>();
                    item.GetComponent<PayToWin>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "C")
                {
                    item.GetComponent<FirearmScript>().cheatRNG = -3;
                    item.AddComponent<Superweapon>();
                    item.GetComponent<Superweapon>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "F")
                {
                    item.GetComponent<FirearmScript>().cheatRNG = -6;
                    item.AddComponent<Volant>();
                    item.GetComponent<Volant>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "D")
                {
                    item.GetComponent<FirearmScript>().cheatRNG = -4;
                    item.AddComponent<SocialDistancePlease>();
                    item.GetComponent<SocialDistancePlease>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "E")
                {
                    item.GetComponent<FirearmScript>().cheatRNG = -5;
                    item.AddComponent<EarlyBerthGetsTheHearst>();
                    item.GetComponent<EarlyBerthGetsTheHearst>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "B")
                {
                    item.GetComponent<FirearmScript>().cheatRNG = -2;
                    item.AddComponent<AbsolutelyNoStops>();
                    item.GetComponent<AbsolutelyNoStops>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "H")
                {
                    item.GetComponent<FirearmScript>().cheatRNG = -8;
                    item.AddComponent<Flashpoint>();
                    item.GetComponent<Flashpoint>().proc = item.GetComponent<FirearmScript>().procOne;
                }


                if (cFiveStr == "9")
                {
                    item.AddComponent<AllElseFails>();
                    item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "4")
                {
                    item.AddComponent<NotWithAStick>();
                    item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "5")
                {
                    item.AddComponent<MaliciousWindUp>();
                    item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "6")
                {
                    item.AddComponent<PositiveNegative>();
                    item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "8")
                {
                    item.AddComponent<GoodThingsCome>();
                    item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "!")
                {
                    item.AddComponent<TheMostResplendent>();
                    item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "@")
                {
                    item.AddComponent<Fulminate>();
                    item.GetComponent<Fulminate>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "#")
                {
                    item.AddComponent<Forager>();
                    item.GetComponent<Forager>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                if (cFiveStr == "&")
                {
                    item.AddComponent<ActivatorDrone>();
                    item.GetComponent<ActivatorDrone>().proc = item.GetComponent<FirearmScript>().procOne;
                }

                //Pay to Win pairing
                if (cSixStr == "!")
                {
                    item.AddComponent<TheMostResplendent>();
                    item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procTwo;
                }

                //Social Distance, Please! pairing
                if (cSixStr == "4")
                {
                    item.AddComponent<NotWithAStick>();
                    item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procTwo;
                }

                //Absolutely no breaks! Pairing
                if (cSixStr == "#")
                {
                    item.AddComponent<Forager>();
                    item.GetComponent<Forager>().proc = item.GetComponent<FirearmScript>().procTwo;
                }

                if (cSixStr == "6")
                {
                    item.AddComponent<PositiveNegative>();
                    item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procTwo;
                }

                //Volant pairing
                if (cSixStr == "0")
                {
                    item.AddComponent<WaitNowImReady>();
                    item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;
                }
             
                //Early Berth gets the Hearst pairing
                if (cSixStr == "1")
                {
                    item.AddComponent<Efficacy>();
                    item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;
                }

                //Equivalent Exchange pairing
                if (cSixStr == "2")
                {
                    item.AddComponent<Inoculated>();
                    item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;
                }

                if (cSixStr == "7")
                {
                    item.AddComponent<Cadence>();
                    item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;
                }

                if (cSixStr == "3")
                {
                    item.AddComponent<RudeAwakening>();
                    item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;
                }

                //Superweapon pairing
                if (cSixStr == "$")
                {
                    item.AddComponent<Counterplay>();
                    item.GetComponent<Counterplay>().proc = item.GetComponent<FirearmScript>().procTwo;
                }

                if (cSixStr == "%")
                {
                    item.AddComponent<Enshroud>();
                    item.GetComponent<Enshroud>().proc = item.GetComponent<FirearmScript>().procTwo;
                }

                if (cSixStr == "^")
                {
                    item.AddComponent<GaleForceWinds>();
                    item.GetComponent<GaleForceWinds>().proc = item.GetComponent<FirearmScript>().procTwo;
                }
            }

            yield return new WaitForSeconds(spawnDelayTimer);
        }

        track = false;
        transition.fadeToGame = true;
    }

    /// <summary>
    /// Main Menu Inventory page navigation through keys
    /// </summary>
    private void SwitchInv()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (sortToggle)
            {
                sortToggle = false;
            }

            else
            {
                sortToggle = true;
            }
        }

        if (automateLeft)
        {
            if (automateSelection <= 0 && observedWeps.Count >= 1)
            {
                automateSelection = observedWeps.Count - 1;
            }

            else
            {
                automateSelection--;
            }

            string o = observedWeps[automateSelection];
            favCheck = o[3].ToString();

            if (favCheck == "1")
            {
                selection = automateSelection;


                DisplayWeapons();
                appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");

                automateLeft = false;
            }
            
        } //Automates Inventory navigation left until a favorite Weapon is found

        if (automateRight)
        {
            if (automateSelection >= observedWeps.Count - 1 && observedWeps.Count >= 1)
            {
                automateSelection = 0;
            }

            else
            {
                automateSelection++;
            }

            string o = observedWeps[automateSelection];
            favCheck = o[3].ToString();

            if (favCheck == "1")
            {
                selection = automateSelection;

                DisplayWeapons();
                appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");

                automateRight = false;
            }
        } //Automates Inventory navigation right until a favorite Weapon is found

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Prevents switching when inventory is empty
            if (selection <= -1)
            {
                //Debug.Log("Cannot switch weapons");
                return;
            }

            //Prevents switching when only one item in inventory
            if (selection <= 0 && observedWeps.Count <= 1)
            {
                //Debug.Log("Cannot switch weapons");
                return;
            }

            else
            {
                //selection--;

                //Activates the Weapon at the end of the Inventory if you switch at the front. Otherwise, index goes left.
                if (selection <= 0 && observedWeps.Count >= 1)
                {
                    if (sortToggle)
                    {
                        automateSelection = selection;
                        automateLeft = true;
                    }

                    else
                    {
                        selection = observedWeps.Count - 1;

                        DisplayWeapons();
                        appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");
                    }
                }

                else
                {
                    if (sortToggle)
                    {
                        automateSelection = selection;
                        automateLeft = true;
                    }

                    else
                    {
                        selection--;

                        DisplayWeapons();
                        appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");
                    }
                }
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
            if (selection <= 0 && observedWeps.Count <= 1)
            {
                //Debug.Log("Cannot switch weapons");
                return;
            }

            else
            {
                //selection++;

                //Activates the Weapon at the front of the Inventory if you switch at the end. Otherwise, index goes right.
                if (selection >= observedWeps.Count - 1 && observedWeps.Count >= 1)
                {
                    if (sortToggle)
                    {
                        automateSelection = selection;
                        automateRight = true;
                    }

                    else
                    {
                        selection = 0;

                        DisplayWeapons();
                        appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");
                    }
                }

                else
                {
                    if (sortToggle)
                    {
                        automateSelection = selection;
                        automateRight = true;
                    }

                    else
                    {
                        selection++;

                        DisplayWeapons();
                        appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");
                    }            
                }
            }
        }
    }

    /// <summary>
    /// Main Menu Inventory page dismantling by input
    /// </summary>
    private void DismantleInv()
    {
        if (Input.GetKey(KeyCode.X))
        {
            string d = observedWeps[selection];
            favCheck = d[3].ToString();
            if (favCheck == "1")
            {
                return;
            } //Favorite Weapons cannot be sold

            else
            {
                dismantleTimer -= Time.deltaTime;
                dismantleText.text = "Selling...";
                dismantleText.color = Color.Lerp(Color.cyan, Color.black, dismantleTimer);

                if (dismantleTimer <= 0.0f)
                {
                    if (selection <= -1 && observedWeps.Count <= 0)
                    {
                        Debug.Log("Cannot dismantle; no items in inventory");
                        return;
                    }

                    kiosk.lucentFunds += (int)kiosk.inventoryWorth[selection];
                    if (kiosk.lucentFunds >= 100000)
                    {
                        kiosk.lucentFunds = 100000;
                    }

                    observedWeps.RemoveAt(selection);
                    kiosk.inventoryWorth.RemoveAt(selection);

                    if (gameObject.transform.childCount > 0)
                    {
                        foreach (Transform child in transform)
                        {
                            Destroy(child.gameObject);
                        }
                    }

                    if (selection >= observedWeps.Count)
                    {
                        selection--;
                        dismantleTimer = dismantleTimerReset;

                        if (selection <= -1 && observedWeps.Count <= 0)
                        {
                            selection = -1;
                            invNavigation.SetActive(false);

                            return;
                        }

                    }

                    if (selection <= -1 && observedWeps.Count >= 1)
                    {
                        selection = 0;

                    }

                    DisplayWeapons();
                    SaveInventory();
                    appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");

                    dismantleTimer = dismantleTimerReset;
                }
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

    private void ChangeFavoriteState()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            ModifyFavoriteState();
        }
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

    /// <summary>
    /// Writes Inventory contents to inventory.txt
    /// </summary>
    public void WriteInventory()
    {
        CreateInventoryFile();

        using (StreamWriter write = new StreamWriter(filepath))
        {
            if (observedWeps.Count > 0)
            {
                for (int i = 0; i < observedWeps.Count; i++)
                {
                    write.WriteLine(observedWeps[i]);
                }
            }
        }
    }

    /// <summary>
    /// Main Menu Inventory page navigation that makes index go left
    /// </summary>
    public void SwitchInvLeft()
    {
        if (automateLeft)
        {
            if (automateSelection <= 0 && observedWeps.Count >= 1)
            {
                automateSelection = observedWeps.Count - 1;
            }

            else
            {
                automateSelection--;
            }

            string o = observedWeps[automateSelection];
            favCheck = o[3].ToString();

            if (favCheck == "1")
            {
                selection = automateSelection;


                DisplayWeapons();
                appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");

                automateLeft = false;
            }

        } //Automates Inventory navigation left until a favorite Weapon is found

        //Prevents switching when inventory is empty
        if (selection <= -1)
        {
            //Debug.Log("Cannot switch weapons");
            return;
        }

        //Prevents switching when only one item in inventory
        if (selection <= 0 && observedWeps.Count <= 1)
        {
            //Debug.Log("Cannot switch weapons");
            return;
        }

        else
        {
            //selection--;

            //Activates the Weapon at the end of the Inventory if you switch at the front. Otherwise, index goes left.
            if (selection <= 0 && observedWeps.Count >= 1)
            {
                if (sortToggle)
                {
                    automateSelection = selection;
                    automateLeft = true;
                }

                else
                {
                    selection = observedWeps.Count - 1;

                    DisplayWeapons();
                    appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");
                }
            }

            else
            {
                if (sortToggle)
                {
                    automateSelection = selection;
                    automateLeft = true;
                }

                else
                {
                    selection--;

                    DisplayWeapons();
                    appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");
                }
            }           
        }
    }

    /// <summary>
    /// Main Menu Inventory page navigation that makes index go right
    /// </summary>
    public void SwitchInvRight()
    {
        if (automateRight)
        {
            if (automateSelection >= observedWeps.Count - 1 && observedWeps.Count >= 1)
            {
                automateSelection = 0;
            }

            else
            {
                automateSelection++;
            }

            string o = observedWeps[automateSelection];
            favCheck = o[3].ToString();

            if (favCheck == "1")
            {
                selection = automateSelection;

                DisplayWeapons();
                appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");

                automateRight = false;
            }
        } //Automates Inventory navigation right until a favorite Weapon is found

        //Prevents switching when inventory is empty
        if (selection <= -1)
        {
            //Debug.Log("Cannot switch weapons");
            return;
        }

        //Prevents switching when only one item in inventory
        if (selection <= 0 && observedWeps.Count <= 1)
        {
            //Debug.Log("Cannot switch weapons");
            return;
        }

        else
        {
            //selection++;

            //Activates the Weapon at the front of the Inventory if you switch at the end. Otherwise, index goes right.
            if (selection >= observedWeps.Count - 1 && observedWeps.Count >= 1)
            {
                if (sortToggle)
                {
                    automateSelection = selection;
                    automateRight = true;
                }

                else
                {
                    selection = 0;

                    DisplayWeapons();
                    appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");
                }
            }

            else
            {
                if (sortToggle)
                {
                    automateSelection = selection;
                    automateRight = true;
                }

                else
                {
                    selection++;

                    DisplayWeapons();
                    appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");
                }          
            }
        }
    }

    /// <summary>
    /// Main Menu Inventory page interaction that dismantles Weapon selected by index
    /// </summary>
    public void DismantleButton()
    {
        if (selection <= -1 && observedWeps.Count <= 0)
        {
            Debug.Log("Cannot dismantle; no items in inventory");
            return;
        }

        kiosk.lucentFunds += (int)kiosk.inventoryWorth[selection];
        if(kiosk.lucentFunds >= 100000)
        {
            kiosk.lucentFunds = 100000;
        }

        observedWeps.RemoveAt(selection);
        kiosk.inventoryWorth.RemoveAt(selection);

        if (gameObject.transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (selection >= observedWeps.Count)
        {
            selection--;
            dismantleTimer = dismantleTimerReset;

            if (selection <= -1 && observedWeps.Count <= 0)
            {
                selection = -1;
                invNavigation.SetActive(false);
                appraisal.text = "";

                return;
            }

        }

        if (selection <= -1 && observedWeps.Count >= 1)
        {
            selection = 0;

        }

        DisplayWeapons();
        SaveInventory();
        appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");
    }

    /// <summary>
    /// Main Menu Inventory page interaction that changes sorting type between All and Favorite
    /// </summary>
    public void ChangeSortingType()
    {
        if (sortToggle)
        {
            sortToggle = false;
        }

        else
        {
            sortToggle = true;
        }
    }

    public void ModifyFavoriteState()
    {
        string f = observedWeps[selection];
        weaponToModify = f.ToCharArray();

        if(weaponToModify[3] == '1')
        {
            weaponToModify[3] = '0';
        }

        else
        {
            weaponToModify[3] = '1';
        }

        observedWeps[selection] = new string(weaponToModify);
        //SaveInventory();
        DisplayWeapons();
    }

    public void InventoryInputControlsDeactivate()
    {
        invInputHold = true;
    }

    public void InventoryInputControlsActivate()
    {
        invInputHold = false;
    }

    public void SaveInventory()
    {
        WriteInventory();
    }

    /// <summary>
    /// Triggers the display of Weapon information
    /// </summary>
    public void CheckForInventoryUpdate()
    {
        //Displays information of Weapon at beginning of List
        if (observedWeps.Count > 0 && selection == -1)
        {
            selection++;
            DisplayWeapons();

            invNavigation.SetActive(true);

        }

        //Displays information of Weapon at index position
        if (observedWeps.Count > 0 && selection >= 0)
        {
            DisplayWeapons();

            invNavigation.SetActive(true);

        }

        //Appraises worth of Inventory items
        kiosk.AppraiseInquiry();
        if(observedWeps.Count > 0)
        {
            appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");
        }
    }

    public void ClearChildren()
    {
        if (gameObject.transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
