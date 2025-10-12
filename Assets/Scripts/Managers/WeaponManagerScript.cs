using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class WeaponManagerScript : MonoBehaviour
{
    public enum Setting
    {
        Menu = 0, Gameplay = 1
    }

    public Setting setting;

    public List<GameObject> weapons = new List<GameObject>();

    public List<string> observedWeps = new List<string>(10);
    public string filepath = "inventory.txt";
    public Text wepName, flavor, rarityCheck, invMonitor, stats, dismantleText, appraisal;
    public Text cheatOne, cheatTwo, cheatThree, cheatFour, cheatTraitOne, cheatTraitTwo;
    public Text lucentText;
    public GameObject invNavigation;
    public float dismantleTimer;

    internal int selection;
    private PlayerInventoryScript player;
    private MenuManagerScript menu;
    private KioskScript kiosk;
    string wepStr, rarStr, exoStr, cOneStr, cTwoStr, cThreeStr, cFourStr, cFiveStr, cSixStr;
    char wep, rar, exo, cOne, cTwo, cThree, cFour, cFive, cSix;
    float dismantleTimerReset, spawnDelayTimer;
    bool track = true;

    void Awake()
    {
        if(setting == Setting.Gameplay)
        {
            player = FindObjectOfType<PlayerInventoryScript>();

            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].GetComponent<FirearmScript>().display = false;
            }

            ReadOnReload();

            spawnDelayTimer = 0.02f;
            StartCoroutine(RespawnWeapons());
        }

        else
        {
            selection = -1;

            menu = FindObjectOfType<MenuManagerScript>();
            kiosk = FindObjectOfType<KioskScript>();
            if(observedWeps.Count <= 0)
            {
                ObserveOnLoad();
            }
            //ObserveOnLoad();
            //foreach (string item in observedWeps)
            //{
            //    menu.invText.text += item + "\n";
            //}

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

            if (observedWeps.Count > 0)
            {
                //selection++;
                //DisplayWeapons();
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

            else
            {
                SwitchInv();
                DismantleInv();
                invMonitor.text = (selection + 1) + " / " + observedWeps.Count;
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

        if (observedWeps[selection].Length == 8)
        {
            cFive = c[7];
            cFiveStr = cFive.ToString();
        }

        if (observedWeps[selection].Length == 9)
        {
            cFive = c[7];
            cFiveStr = cFive.ToString();

            cSix = c[8];
            cSixStr = cSix.ToString();
        }

        invMonitor.text = (selection + 1) + " / " + observedWeps.Count;

        if (wepStr == "1")
        {
            Debug.Log("Displaying Full Fire Rifle");
            GameObject item = Instantiate(weapons[0], transform.position, transform.rotation);
            item.name = weapons[0].name;

            item.transform.parent = gameObject.transform;
            wepName.text = "Full Fire Rifle";
            flavor.text = item.GetComponent<FirearmScript>().flavorText;            

            if (rarStr == "1")
            {
                rarityCheck.text = "Usual";

                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";

            }

            if (rarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "5")
            {
                rarityCheck.text = "Fated";
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (exoStr == "1")
                {
                    wepName.text = "Full Fire Rifle_Exotic";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "''Such is the law.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[0].name + "_Exotic";
                }

                else
                {
                    item.GetComponent<FirearmScript>().isExotic = false;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }
            }           

            if (cOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (cOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (cTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (cTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (cThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (cThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (cFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (cFourStr == "8")
            {
                cheatFour.text = "Hastier Hands" + "\n" + "25% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                item.AddComponent<HastierHands>();

            }

            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 10000).ToString() + " RPM";

            if (observedWeps[selection].Length == 8)
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

                cheatTraitTwo.text = " ";
            }

            if (observedWeps[selection].Length == 9)
            {
                if (cFiveStr == "A")
                {
                    cheatTraitOne.text = "Equivalent Exchange" + '\n' +
                        "Taking Enemy damage adds 35% of damage received to this Weapon's base damage and to your Health. Base damage can increase up to 150%.";
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

                //Equivalent Exchange pairing
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
            }
        }

        if (wepStr == "2")
        {
            Debug.Log("Displaying Machine Gun");
            GameObject item = Instantiate(weapons[1], transform.position, transform.rotation);
            item.name = weapons[1].name;

            item.transform.parent = gameObject.transform;
            wepName.text = "Machine Gun";
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (rarStr == "1")
            {
                rarityCheck.text = "Usual";

                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";

            }

            if (rarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "5")
            {
                rarityCheck.text = "Fated";
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (exoStr == "1")
                {
                    wepName.text = "Machine Gun_Exotic";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "''Only a fool would strike Lucent deals with no exploitable loopholes for themselves. Wealth is found in the hoard.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[0].name + "_Exotic";
                }

                else
                {
                    item.GetComponent<FirearmScript>().isExotic = false;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }
            }

            if (cOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (cOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (cTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (cTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (cThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (cThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (cFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (cFourStr == "8")
            {
                cheatFour.text = "Hastier Hands" + "\n" + "25% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                item.AddComponent<HastierHands>();

            }

            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 10000).ToString() + " RPM";

            if (observedWeps[selection].Length == 8)
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

                cheatTraitTwo.text = " ";
            }

            if (observedWeps[selection].Length == 9)
            {
                if (cFiveStr == "G")
                {
                    cheatTraitOne.text = "Pay to Win" + '\n' +
                        "Consume 5,280 Lucent to grant stacks of a 50% Weapon damage increase. Stacks 150x." + "\n" +
                        "'E' - Consume Lucent";
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

                if (cSixStr == "5")
                {
                    cheatTraitTwo.text = "Malicious Wind-Up" + '\n' +
                        "Inflicting Damage increases Reload Speed by 0.75%. This bonus activates on your next reload.";

                    //if (rarStr == "5")
                    //{
                    //    cheatTraitTwo.text = "Malicious Wind-Up" + " (Fated)" + '\n' +
                    //    "Inflicting Damage increases Reload Speed by 1.5%. Kills restore 5% of this weapon's reserves.";
                    //}
                }

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
            }
        }

        if (wepStr == "3")
        {
            Debug.Log("Displaying Pistol");
            GameObject item = Instantiate(weapons[2], transform.position, transform.rotation);
            item.name = weapons[2].name;

            item.transform.parent = gameObject.transform;
            wepName.text = "Pistol";
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (rarStr == "1")
            {
                rarityCheck.text = "Usual";

                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";

            }

            if (rarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "5")
            {
                rarityCheck.text = "Fated";
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (exoStr == "1")
                {
                    wepName.text = "Pistol_Exotic";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "''I stand firm in the face of Terror. I am a weed in its terrace. It matters not the Human or the Replevin; I cannot be moved.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[0].name + "_Exotic";
                }

                else
                {
                    item.GetComponent<FirearmScript>().isExotic = false;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }
            }

            if (cOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (cOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (cTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (cTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (cThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (cThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (cFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (cFourStr == "8")
            {
                cheatFour.text = "Hastier Hands" + "\n" + "25% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                item.AddComponent<HastierHands>();

            }

            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 1000).ToString() + " RPM";

            if (observedWeps[selection].Length == 8)
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

                cheatTraitTwo.text = " ";
            }

            if (observedWeps[selection].Length == 9)
            {
                if (cFiveStr == "C")
                {
                    cheatTraitOne.text = "Shelter in Place" + '\n' +
                        "Refraining from moving amplifies Weapon damage by 100% and grants 80% damage reduction. Resuming movement ends the bonus.";
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

                if (cSixStr == "6")
                {
                    cheatTraitTwo.text = "Positive-Negative" + '\n' +
                        "Moving generates a charge. While halfway charged, Enemy hits applies 100% of Weapon damage as damage-over-time for ten seconds.";

                    //if (rarStr == "5")
                    //{
                    //    cheatTraitOne.text = "Positive-Negative" + " (Fated)" + '\n' +
                    //    "Moving generates a charge. While halfway charged, Enemy hits applies 200% of Weapon damage as damage-over-time for ten seconds.";
                    //}
                }

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
            }
        }

        if (wepStr == "4")
        {
            Debug.Log("Displaying Semi Fire Rifle");
            GameObject item = Instantiate(weapons[3], transform.position, transform.rotation);
            item.name = weapons[3].name;

            item.transform.parent = gameObject.transform;
            wepName.text = "Semi Fire Rifle";
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (rarStr == "1")
            {
                rarityCheck.text = "Usual";

                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";

            }

            if (rarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "5")
            {
                rarityCheck.text = "Fated";
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (exoStr == "1")
                {
                    wepName.text = "Semi Fire Rifle_Exotic";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "WARNING: Persistent use of the cognitive Supercharger may result in cardiac implosion. Proceed? [Y] or [N]";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[0].name + "_Exotic";
                }

                else
                {
                    item.GetComponent<FirearmScript>().isExotic = false;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }
            }

            if (cOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (cOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (cTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (cTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (cThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (cThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (cFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (cFourStr == "8")
            {
                cheatFour.text = "Hastier Hands" + "\n" + "25% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                item.AddComponent<HastierHands>();

            }

            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 1000).ToString() + " RPM";

            if (observedWeps[selection].Length == 8)
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

                cheatTraitTwo.text = " ";
            }

            if (observedWeps[selection].Length == 9)
            {
                if (cFiveStr == "F")
                {
                    cheatTraitOne.text = "Off Your Own Supply" + '\n' +
                        "Sacrificing your Shield grants 10% Movement Speed, 80% Reload Speed, 140% Weapon damage, and zero Recoil.";
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

                if (cSixStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";

                    if (rarStr == "5" && exoStr != "1")
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
            }
        }

        if (wepStr == "5")
        {
            Debug.Log("Displaying Shotgun");
            GameObject item = Instantiate(weapons[4], transform.position, transform.rotation);
            item.name = weapons[4].name;

            item.transform.parent = gameObject.transform;
            wepName.text = "Shotgun";
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (rarStr == "1")
            {
                rarityCheck.text = "Usual";

                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";

            }

            if (rarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "5")
            {
                rarityCheck.text = "Fated";
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (exoStr == "1")
                {
                    wepName.text = "Shotgun_Exotic";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "''Isn't it wonderful when we all do our part?''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[0].name + "_Exotic";
                }

                else
                {
                    item.GetComponent<FirearmScript>().isExotic = false;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }
            }

            if (cOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (cOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (cTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (cTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (cThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (cThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (cFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (cFourStr == "8")
            {
                cheatFour.text = "Hastier Hands" + "\n" + "25% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                item.AddComponent<HastierHands>();

            }

            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 100).ToString() + " RPM";

            if (observedWeps[selection].Length == 8)
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

                cheatTraitTwo.text = " ";
            }

            if (observedWeps[selection].Length == 9)
            {
                if (cFiveStr == "D")
                {
                    cheatTraitOne.text = "Social Distance, please!" + '\n' +
                        "Weapon hits temporarily increase Weapon damage by 30% and adds a Health debuff. Kills spread 400% of Weapon damage to nearby enemies.";
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

                if (cSixStr == "4")
                {
                    cheatTraitTwo.text = "Not with a Stick" + '\n' +
                        "Kills with this Weapon increase Effective Range by 30% of max Range until your next reload.";

                    if (rarStr == "5" && exoStr != "1")
                    {
                        cheatTraitOne.text = "Not with a Stick" + " (Fated)" + '\n' +
                        "Kills increase Effective Range by 30% of max Range. Maximizing Effective Range increases Aim Assist halfway to full strength for 20 seconds.";
                    }
                }

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

                if (cSixStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";

                    if (rarStr == "5" && exoStr != "1")
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
            }
        }

        if (wepStr == "6")
        {
            Debug.Log("Displaying Single Fire Rifle");
            GameObject item = Instantiate(weapons[5], transform.position, transform.rotation);
            item.name = weapons[5].name;

            item.transform.parent = gameObject.transform;
            wepName.text = "Single Fire Rifle";
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (rarStr == "1")
            {
                rarityCheck.text = "Usual";

                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";

            }

            if (rarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "5")
            {
                rarityCheck.text = "Fated";
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (exoStr == "1")
                {
                    wepName.text = "Single Fire Rifle_Exotic";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "''The Resplendent, for all its igneous light, could never have unveiled the shaded plot. The victims are owed retribution of a thermobaric kind.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[0].name + "_Exotic";
                }

                else
                {
                    item.GetComponent<FirearmScript>().isExotic = false;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }
            }

            if (cOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (cOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (cTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (cTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (cThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (cThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (cFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (cFourStr == "8")
            {
                cheatFour.text = "Hastier Hands" + "\n" + "25% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                item.AddComponent<HastierHands>();

            }

            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 1000).ToString() + " RPM";

            if (observedWeps[selection].Length == 8)
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

                cheatTraitTwo.text = " ";
            }

            if (observedWeps[selection].Length == 9)
            {
                if (cFiveStr == "E")
                {
                    cheatTraitOne.text = "The Early Berth gets the Hearst" + '\n' +
                        "Every other Enemy hit triggers a Berth detonation, inflicting 200% of Weapon damage.";
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

                if (cSixStr == "1")
                {
                    cheatTraitTwo.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";

                    if (rarStr == "5" && exoStr != "1")
                    {
                        cheatTraitTwo.text = "Efficacy" + " (Fated)" + '\n' +
                        "Enemy hits increases this Weapon's base damage by 2%. Base damage can increase up to 125%, and cannot be reset on reloads.";
                    }

                }

                if (cSixStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";

                    if (rarStr == "5" && exoStr != "1")
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
            }
        }

        if (wepStr == "7")
        {
            Debug.Log("Displaying Submachine Gun");
            GameObject item = Instantiate(weapons[6], transform.position, transform.rotation);
            item.name = weapons[6].name;

            item.transform.parent = gameObject.transform;
            wepName.text = "Submachine Gun";
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (rarStr == "1")
            {
                rarityCheck.text = "Usual";

                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";

            }

            if (rarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (rarStr == "5")
            {
                rarityCheck.text = "Fated";
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (exoStr == "1")
                {
                    wepName.text = "Submachine Gun_Exotic";
                    rarityCheck.text = "Exotic";

                    item.GetComponent<FirearmScript>().isExotic = true;
                    //item.GetComponent<FirearmScript>().damagePercent = 60f;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    item.GetComponent<FirearmScript>().flavorText = "Using this Weapon feels like a perpetual Calvary charge. For where you're going, you won't be needing any breaks.";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.name = weapons[0].name + "_Exotic";
                }

                else
                {
                    item.GetComponent<FirearmScript>().isExotic = false;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }
            }

            if (cOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (cOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (cTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (cTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (cThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (cThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (cFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (cFourStr == "8")
            {
                cheatFour.text = "Hastier Hands" + "\n" + "25% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                item.AddComponent<HastierHands>();

            }

            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 10000).ToString() + " RPM";

            if (observedWeps[selection].Length == 8)
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

                cheatTraitTwo.text = " ";
            }

            if (observedWeps[selection].Length == 9)
            {
                if (cFiveStr == "B")
                {
                    cheatTraitOne.text = "Absolutely No Stops" + '\n' +
                        "Expending your magazine fills it from reserves, amplifies Weapon damage by 200%, and increases Rate of Fire by 50%.";
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

                if (cSixStr == "8")
                {
                    cheatTraitTwo.text = "Good Things Come" + '\n' +
                        "Being in combat for three seconds grants 25% Movement Speed, 20% damage reduction, and 45% Recoil reduction until you leave combat.";

                    //if (rarStr == "5")
                    //{
                    //    cheatTraitOne.text = "Good Things Come" + " (Fated)" + '\n' +
                    //        "Being in combat instantly grants 50% Movement Speed, 40% damage reduction, 90% Recoil reduction, and Infinite Ammo until you leave combat.";

                    //}

                }

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

                if (cSixStr == "1")
                {
                    cheatTraitTwo.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";

                    if (rarStr == "5" && exoStr != "1")
                    {
                        cheatTraitTwo.text = "Efficacy" + " (Fated)" + '\n' +
                        "Enemy hits increases this Weapon's base damage by 2%. Base damage can increase up to 125%, and cannot be reset on reloads.";
                    }

                }

                if (cSixStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";

                    if (rarStr == "5" && exoStr != "1")
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
            }
        }
    } //This method displays Weapons from the Player's Inventory for inspetion on the Main Menu.

    private IEnumerator RespawnWeapons()
    {
        string c = "Comic Sans";

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
                //Debug.Log("Respawning Full Fire Rifle");
                GameObject item = Instantiate(weapons[0], transform.position, transform.rotation);
                item.name = weapons[0].name;
                //item.GetComponent<FirearmScript>().damage = 350;
                //item.GetComponent<FirearmScript>().damagePercent = 10f;

                if (rarStr == "1")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 1;
                }

                if (rarStr == "2")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 2;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 210;
                }

                if (rarStr == "3")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 3;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 227;
                    //item.GetComponent<FirearmScript>().damagePercent = 30f;
                }

                if (rarStr == "4")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 4;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 245;
                    //item.GetComponent<FirearmScript>().damagePercent = 40f;
                }

                if (rarStr == "5")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 5;

                    if (exoStr == "1")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        //item.GetComponent<FirearmScript>().damagePercent = 60f;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "Such is the law.";
                        item.name = weapons[0].name + "_Exotic";
                    }

                    else if (exoStr == "0")
                    {
                        item.GetComponent<FirearmScript>().isExotic = false;
                        item.GetComponent<FirearmScript>().RarityAugment();
                    }
                }
               
                if (cOneStr == "1")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                    item.AddComponent<DeepYield>();
                }

                if (cOneStr == "2")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                    item.AddComponent<DeeperYield>();
                }

                if (cTwoStr == "3")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                    item.AddComponent<DeepStores>();
                }

                if (cTwoStr == "4")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                    item.AddComponent<DeeperStores>();
                }

                if (cThreeStr == "5")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                    item.AddComponent<FarSight>();
                }

                if (cThreeStr == "6")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                    item.AddComponent<FartherSight>();
                }

                if (cFourStr == "7")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                    item.AddComponent<HastyHands>();
                }

                if (cFourStr == "8")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                    item.AddComponent<HastierHands>();
                }

                if (player.readdedWeps[s].Length == 8)
                {
                    if (cFiveStr == "0")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 450;
                        item.AddComponent<WaitNowImReady>();

                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "1")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 475;
                        item.AddComponent<Efficacy>();

                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "2")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 525;
                        item.AddComponent<Inoculated>();

                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "3")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 575;
                        item.AddComponent<RudeAwakening>();

                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 625;
                        item.AddComponent<NotWithAStick>();

                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 675;
                        item.AddComponent<MaliciousWindUp>();

                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 725;
                        item.AddComponent<PositiveNegative>();

                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "7")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 775;
                        item.AddComponent<Cadence>();

                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 825;
                        item.AddComponent<GoodThingsCome>();

                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 875;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    } //New

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 925;
                        item.AddComponent<TheMostResplendent>();

                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    } //New
                }

                if (player.readdedWeps[s].Length == 9)
                {
                    if (cFiveStr == "A")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = -1;
                        item.AddComponent<EquivalentExchange>();
                        item.GetComponent<EquivalentExchange>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 410;
                        item.AddComponent<AllElseFails>();
                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 415;
                        item.AddComponent<NotWithAStick>();
                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 425;
                        item.AddComponent<MaliciousWindUp>();
                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 435;
                        item.AddComponent<PositiveNegative>();
                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 445;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    } //New

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 455;
                        item.AddComponent<TheMostResplendent>();
                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;

                    } //New

                    //Equivalent Exchange pairing
                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    } //New

                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    } //New

                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    } //New

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 495;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    } //New

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 505;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    } //New
                }

                yield return new WaitForSeconds(spawnDelayTimer);
            }

            if (wepStr == "2")
            {
                //Debug.Log("Respawning Machine Gun");
                GameObject item = Instantiate(weapons[1], transform.position, transform.rotation);
                item.name = weapons[1].name;
                item.GetComponent<FirearmScript>().damage = 600;
                item.GetComponent<FirearmScript>().damagePercent = 10f;

                if (rarStr == "1")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 1;
                    //item.GetComponent<FirearmScript>().damagePercent = 10f;
                }

                if (rarStr == "2")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 2;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 330;
                    //item.GetComponent<FirearmScript>().damagePercent = 20f;
                }

                if (rarStr == "3")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 3;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 357;
                    //item.GetComponent<FirearmScript>().damagePercent = 30f;
                }

                if (rarStr == "4")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 4;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 385;
                    //item.GetComponent<FirearmScript>().damagePercent = 40f;
                }

                if (rarStr == "5")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 5;

                    if (exoStr == "1")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        //item.GetComponent<FirearmScript>().damagePercent = 60f;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "Only a fool would strike Lucent deals with no exploitable loopholes for themselves. Wealth is found in the hoard.";
                        item.name = weapons[1].name + "_Exotic";
                    }

                    else if (exoStr == "0")
                    {
                        item.GetComponent<FirearmScript>().isExotic = false;
                        item.GetComponent<FirearmScript>().RarityAugment();
                    }
                }                

                if (cOneStr == "1")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                    item.AddComponent<DeepYield>();
                }

                if (cOneStr == "2")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                    item.AddComponent<DeeperYield>();
                }

                if (cTwoStr == "3")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                    item.AddComponent<DeepStores>();
                }

                if (cTwoStr == "4")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                    item.AddComponent<DeeperStores>();
                }

                if (cThreeStr == "5")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                    item.AddComponent<FarSight>();
                }

                if (cThreeStr == "6")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                    item.AddComponent<FartherSight>();
                }

                if (cFourStr == "7")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                    item.AddComponent<HastyHands>();
                }

                if (cFourStr == "8")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                    item.AddComponent<HastierHands>();
                }

                if (player.readdedWeps[s].Length == 8)
                {
                    if (cFiveStr == "0")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 450;
                        item.AddComponent<WaitNowImReady>();

                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "1")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 475;
                        item.AddComponent<Efficacy>();

                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "2")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 525;
                        item.AddComponent<Inoculated>();

                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "3")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 575;
                        item.AddComponent<RudeAwakening>();

                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 625;
                        item.AddComponent<NotWithAStick>();

                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 675;
                        item.AddComponent<MaliciousWindUp>();

                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 725;
                        item.AddComponent<PositiveNegative>();

                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "7")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 775;
                        item.AddComponent<Cadence>();

                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 825;
                        item.AddComponent<GoodThingsCome>();

                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 875;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    } //New

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 925;
                        item.AddComponent<TheMostResplendent>();

                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    } //New
                }

                if (player.readdedWeps[s].Length == 9)
                {
                    if (cFiveStr == "G")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = -7;
                        item.AddComponent<PayToWin>();
                        item.GetComponent<PayToWin>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 410;
                        item.AddComponent<AllElseFails>();
                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 415;
                        item.AddComponent<NotWithAStick>();
                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 425;
                        item.AddComponent<MaliciousWindUp>();
                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 435;
                        item.AddComponent<PositiveNegative>();
                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 445;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    } //New

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 455;
                        item.AddComponent<TheMostResplendent>();
                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;

                    } //New

                    //Pay to Win pairing
                    if (cSixStr == "5")
                    {
                        //item.GetComponent<FirearmScript>().fcnChtOne = 425;
                        item.AddComponent<MaliciousWindUp>();
                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 495;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 505;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }

                yield return new WaitForSeconds(spawnDelayTimer);
            }

            if (wepStr == "3")
            {
                //Debug.Log("Respawning Pistol");
                GameObject item = Instantiate(weapons[2], transform.position, transform.rotation);
                item.name = weapons[2].name;
                item.GetComponent<FirearmScript>().damage = 580;
                item.GetComponent<FirearmScript>().damagePercent = 10f;

                if (rarStr == "1")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 1;
                    //item.GetComponent<FirearmScript>().damagePercent = 10f;
                }

                if (rarStr == "2")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 2;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 348;
                    //item.GetComponent<FirearmScript>().damagePercent = 20f;

                }

                if (rarStr == "3")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 3;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 377;
                    //item.GetComponent<FirearmScript>().damagePercent = 30f;
                }

                if (rarStr == "4")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 4;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 406;
                    //item.GetComponent<FirearmScript>().damagePercent = 40f;
                }

                if (rarStr == "5")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 5;

                    if (exoStr == "1")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        //item.GetComponent<FirearmScript>().damagePercent = 60f;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "I stand firm in the face of Terror. I am a weed in its terrace. It matters not the Human or the Replevin; I cannot be moved.";
                        item.name = weapons[2].name + "_Exotic";
                    }

                    else if (exoStr == "0")
                    {
                        item.GetComponent<FirearmScript>().isExotic = false;
                        item.GetComponent<FirearmScript>().RarityAugment();
                    }
                    
                }              

                if (cOneStr == "1")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                    item.AddComponent<DeepYield>();
                }

                if (cOneStr == "2")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                    item.AddComponent<DeeperYield>();
                }

                if (cTwoStr == "3")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                    item.AddComponent<DeepStores>();
                }

                if (cTwoStr == "4")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                    item.AddComponent<DeeperStores>();
                }

                if (cThreeStr == "5")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                    item.AddComponent<FarSight>();
                }

                if (cThreeStr == "6")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                    item.AddComponent<FartherSight>();
                }

                if (cFourStr == "7")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                    item.AddComponent<HastyHands>();
                }

                if (cFourStr == "8")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                    item.AddComponent<HastierHands>();
                }

                if (player.readdedWeps[s].Length == 8)
                {
                    if (cFiveStr == "0")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 450;
                        item.AddComponent<WaitNowImReady>();

                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "1")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 475;
                        item.AddComponent<Efficacy>();

                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "2")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 525;
                        item.AddComponent<Inoculated>();

                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "3")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 575;
                        item.AddComponent<RudeAwakening>();

                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 625;
                        item.AddComponent<NotWithAStick>();

                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 675;
                        item.AddComponent<MaliciousWindUp>();

                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 725;
                        item.AddComponent<PositiveNegative>();

                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "7")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 775;
                        item.AddComponent<Cadence>();

                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 825;
                        item.AddComponent<GoodThingsCome>();

                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 875;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 925;
                        item.AddComponent<TheMostResplendent>();

                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    } //New
                }

                if (player.readdedWeps[s].Length == 9)
                {
                    if (cFiveStr == "C")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = -3;
                        item.AddComponent<ShelterInPlace>();
                        item.GetComponent<ShelterInPlace>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 410;
                        item.AddComponent<AllElseFails>();
                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 415;
                        item.AddComponent<NotWithAStick>();
                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 425;
                        item.AddComponent<MaliciousWindUp>();
                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 435;
                        item.AddComponent<PositiveNegative>();
                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 445;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 455;
                        item.AddComponent<TheMostResplendent>();
                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;

                    } //New

                    //Shelter in Place pairing
                    if (cSixStr == "6")
                    {
                        //item.GetComponent<FirearmScript>().fcnChtOne = 425;
                        item.AddComponent<PositiveNegative>();
                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procTwo;
                    }

                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 495;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 505;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }

                yield return new WaitForSeconds(spawnDelayTimer);
            }

            if (wepStr == "4")
            {
                //Debug.Log("Respawning Semi Fire Rifle");
                GameObject item = Instantiate(weapons[3], transform.position, transform.rotation);
                item.name = weapons[3].name;
                item.GetComponent<FirearmScript>().damage = 470;
                item.GetComponent<FirearmScript>().damagePercent = 10f;

                if (rarStr == "1")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 1;
                    //item.GetComponent<FirearmScript>().damagePercent = 10f;
                }

                if (rarStr == "2")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 2;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 282;
                    //item.GetComponent<FirearmScript>().damagePercent = 20f;
                }

                if (rarStr == "3")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 3;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 305;
                    //item.GetComponent<FirearmScript>().damagePercent = 30f;
                }

                if (rarStr == "4")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 4;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 329;
                    //item.GetComponent<FirearmScript>().damagePercent = 40f;
                }

                if (rarStr == "5")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 5;

                    if (exoStr == "1")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        //item.GetComponent<FirearmScript>().damagePercent = 60f;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "WARNING: Persistent use of the cognitive Supercharger may result in cardiac implosion. Proceed? [Y] or [N]";
                        item.name = weapons[3].name + "_Exotic";
                    }

                    else if (exoStr == "0")
                    {
                        item.GetComponent<FirearmScript>().isExotic = false;
                        item.GetComponent<FirearmScript>().RarityAugment();
                    }
                }               

                if (cOneStr == "1")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                    item.AddComponent<DeepYield>();
                }

                if (cOneStr == "2")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                    item.AddComponent<DeeperYield>();
                }

                if (cTwoStr == "3")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                    item.AddComponent<DeepStores>();
                }

                if (cTwoStr == "4")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                    item.AddComponent<DeeperStores>();
                }

                if (cThreeStr == "5")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                    item.AddComponent<FarSight>();
                }

                if (cThreeStr == "6")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                    item.AddComponent<FartherSight>();
                }

                if (cFourStr == "7")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                    item.AddComponent<HastyHands>();
                }

                if (cFourStr == "8")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                    item.AddComponent<HastierHands>();
                }

                if (player.readdedWeps[s].Length == 8)
                {
                    if (cFiveStr == "0")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 450;
                        item.AddComponent<WaitNowImReady>();

                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "1")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 475;
                        item.AddComponent<Efficacy>();

                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "2")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 525;
                        item.AddComponent<Inoculated>();

                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "3")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 575;
                        item.AddComponent<RudeAwakening>();

                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 625;
                        item.AddComponent<NotWithAStick>();

                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 675;
                        item.AddComponent<MaliciousWindUp>();

                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 725;
                        item.AddComponent<PositiveNegative>();

                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "7")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 775;
                        item.AddComponent<Cadence>();

                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 825;
                        item.AddComponent<GoodThingsCome>();

                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 875;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 925;
                        item.AddComponent<TheMostResplendent>();

                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    } //New
                }

                if (player.readdedWeps[s].Length == 9)
                {
                    if (cFiveStr == "F")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = -6;
                        item.AddComponent<OffYourOwnSupply>();
                        item.GetComponent<OffYourOwnSupply>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 410;
                        item.AddComponent<AllElseFails>();
                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 415;
                        item.AddComponent<NotWithAStick>();
                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 425;
                        item.AddComponent<MaliciousWindUp>();
                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 435;
                        item.AddComponent<PositiveNegative>();
                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 445;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 455;
                        item.AddComponent<TheMostResplendent>();
                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;

                    } //New

                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    //Off your own Supply pairing
                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 495;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 505;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }

                yield return new WaitForSeconds(spawnDelayTimer);
            }

            if (wepStr == "5")
            {
                //Debug.Log("Respawning Shotgun");
                GameObject item = Instantiate(weapons[4], transform.position, transform.rotation);
                item.name = weapons[4].name;
                item.GetComponent<FirearmScript>().damage = 300;
                item.GetComponent<FirearmScript>().damagePercent = 10f;

                if (rarStr == "1")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 1;
                    //item.GetComponent<FirearmScript>().damagePercent = 10f;
                }

                if (rarStr == "2")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 2;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 360;
                    //item.GetComponent<FirearmScript>().damagePercent = 20f;
                }

                if (rarStr == "3")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 3;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 390;
                    //item.GetComponent<FirearmScript>().damagePercent = 30f;
                }

                if (rarStr == "4")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 4;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 420;
                    //item.GetComponent<FirearmScript>().damagePercent = 40f;
                }

                if (rarStr == "5")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 5;
                    if (exoStr == "1")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        //item.GetComponent<FirearmScript>().damagePercent = 60f;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "Isn't it wonderful when we all do our part?";
                        item.name = weapons[4].name + "_Exotic";
                    }

                    else if (exoStr == "0")
                    {
                        item.GetComponent<FirearmScript>().isExotic = false;
                        item.GetComponent<FirearmScript>().RarityAugment();
                    }
                }               

                if (cOneStr == "1")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                    item.AddComponent<DeepYield>();
                }

                if (cOneStr == "2")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                    item.AddComponent<DeeperYield>();
                }

                if (cTwoStr == "3")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                    item.AddComponent<DeepStores>();
                }

                if (cTwoStr == "4")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                    item.AddComponent<DeeperStores>();
                }

                if (cThreeStr == "5")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                    item.AddComponent<FarSight>();
                }

                if (cThreeStr == "6")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                    item.AddComponent<FartherSight>();
                }

                if (cFourStr == "7")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                    item.AddComponent<HastyHands>();
                }

                if (cFourStr == "8")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                    item.AddComponent<HastierHands>();
                }

                if (player.readdedWeps[s].Length == 8)
                {
                    if (cFiveStr == "0")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 450;
                        item.AddComponent<WaitNowImReady>();

                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "1")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 475;
                        item.AddComponent<Efficacy>();

                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "2")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 525;
                        item.AddComponent<Inoculated>();

                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "3")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 575;
                        item.AddComponent<RudeAwakening>();

                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 625;
                        item.AddComponent<NotWithAStick>();

                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 675;
                        item.AddComponent<MaliciousWindUp>();

                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 725;
                        item.AddComponent<PositiveNegative>();

                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "7")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 775;
                        item.AddComponent<Cadence>();

                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 825;
                        item.AddComponent<GoodThingsCome>();

                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 875;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 925;
                        item.AddComponent<TheMostResplendent>();

                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    } //New
                }

                if (player.readdedWeps[s].Length == 9)
                {
                    if (cFiveStr == "D")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = -4;
                        item.AddComponent<SocialDistancePlease>();
                        item.GetComponent<SocialDistancePlease>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 410;
                        item.AddComponent<AllElseFails>();
                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 415;
                        item.AddComponent<NotWithAStick>();
                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 425;
                        item.AddComponent<MaliciousWindUp>();
                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 435;
                        item.AddComponent<PositiveNegative>();
                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 445;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 455;
                        item.AddComponent<TheMostResplendent>();
                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;

                    } //New

                    //Social Distance, Please! pairing
                    if (cSixStr == "4")
                    {
                        //item.GetComponent<FirearmScript>().fcnChtOne = 415;
                        item.AddComponent<NotWithAStick>();
                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procTwo;
                    }

                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 495;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 505;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }

                yield return new WaitForSeconds(spawnDelayTimer);
            }

            if (wepStr == "6")
            {
                //Debug.Log("Respawning Single Fire Rifle");
                GameObject item = Instantiate(weapons[5], transform.position, transform.rotation);
                item.name = weapons[5].name;
                item.GetComponent<FirearmScript>().damage = 1044;
                item.GetComponent<FirearmScript>().damagePercent = 10f;

                if (rarStr == "1")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 1;
                    //item.GetComponent<FirearmScript>().damagePercent = 10f;
                }

                if (rarStr == "2")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 2;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 402;
                    //item.GetComponent<FirearmScript>().damagePercent = 20f;
                }

                if (rarStr == "3")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 3;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 435;
                    //item.GetComponent<FirearmScript>().damagePercent = 30f;
                }

                if (rarStr == "4")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 4;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 469;
                    //item.GetComponent<FirearmScript>().damagePercent = 40f;
                }

                if (rarStr == "5")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 5;
                    if (exoStr == "1")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        //item.GetComponent<FirearmScript>().damagePercent = 60f;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "The Resplendence, for all its igneous light, could never have thwarted the terrible plot. These victims are owed retribution of a thermobaric kind.";
                        item.name = weapons[5].name + "_Exotic";
                    }

                    else if (exoStr == "0")
                    {
                        item.GetComponent<FirearmScript>().isExotic = false;
                        item.GetComponent<FirearmScript>().RarityAugment();
                    }
                }               

                if (cOneStr == "1")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                    item.AddComponent<DeepYield>();
                }

                if (cOneStr == "2")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                    item.AddComponent<DeeperYield>();
                }

                if (cTwoStr == "3")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                    item.AddComponent<DeepStores>();
                }

                if (cTwoStr == "4")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                    item.AddComponent<DeeperStores>();
                }

                if (cThreeStr == "5")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                    item.AddComponent<FarSight>();
                }

                if (cThreeStr == "6")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                    item.AddComponent<FartherSight>();
                }

                if (cFourStr == "7")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                    item.AddComponent<HastyHands>();
                }

                if (cFourStr == "8")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                    item.AddComponent<HastierHands>();
                }

                if (player.readdedWeps[s].Length == 8)
                {
                    if (cFiveStr == "0")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 450;
                        item.AddComponent<WaitNowImReady>();

                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "1")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 475;
                        item.AddComponent<Efficacy>();

                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "2")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 525;
                        item.AddComponent<Inoculated>();

                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "3")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 575;
                        item.AddComponent<RudeAwakening>();

                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 625;
                        item.AddComponent<NotWithAStick>();

                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 675;
                        item.AddComponent<MaliciousWindUp>();

                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 725;
                        item.AddComponent<PositiveNegative>();

                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "7")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 775;
                        item.AddComponent<Cadence>();

                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 825;
                        item.AddComponent<GoodThingsCome>();

                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 875;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 925;
                        item.AddComponent<TheMostResplendent>();

                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    } //New
                }

                if (player.readdedWeps[s].Length == 9)
                {
                    if (cFiveStr == "E")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = -5;
                        item.AddComponent<EarlyBerthGetsTheHearst>();
                        item.GetComponent<EarlyBerthGetsTheHearst>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 410;
                        item.AddComponent<AllElseFails>();
                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 415;
                        item.AddComponent<NotWithAStick>();
                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 425;
                        item.AddComponent<MaliciousWindUp>();
                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 435;
                        item.AddComponent<PositiveNegative>();
                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 445;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 455;
                        item.AddComponent<TheMostResplendent>();
                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;

                    } //New

                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    //Early Berth gets the Hearst pairing
                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 495;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 505;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }

                yield return new WaitForSeconds(spawnDelayTimer);
            }

            if (wepStr == "7")
            {
                //Debug.Log("Respawning SMG");
                GameObject item = Instantiate(weapons[6], transform.position, transform.rotation);
                item.name = weapons[6].name;
                item.GetComponent<FirearmScript>().damage = 330;
                item.GetComponent<FirearmScript>().damagePercent = 10f;

                if (rarStr == "1")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 1;
                    //item.GetComponent<FirearmScript>().damagePercent = 10f;
                }

                if (rarStr == "2")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 2;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 198;
                    //item.GetComponent<FirearmScript>().damagePercent = 20f;
                }

                if (rarStr == "3")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 3;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 214;
                    //item.GetComponent<FirearmScript>().damagePercent = 30f;
                }

                if (rarStr == "4")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 4;
                    item.GetComponent<FirearmScript>().RarityAugment();
                    //item.GetComponent<FirearmScript>().damage = 231;
                    //item.GetComponent<FirearmScript>().damagePercent = 40f;
                }

                if (rarStr == "5")
                {
                    item.GetComponent<FirearmScript>().weaponRarity = 5;
                    if (exoStr == "1")
                    {
                        item.GetComponent<FirearmScript>().isExotic = true;
                        //item.GetComponent<FirearmScript>().damagePercent = 60f;
                        item.GetComponent<FirearmScript>().RarityAugment();
                        item.GetComponent<FirearmScript>().flavorText = "Using this Weapon feels like a perpetual Calvary charge. For where you're going, you won't be needing any breaks.";
                        item.name = weapons[6].name + "_Exotic";
                    }

                    else if (exoStr == "0")
                    {
                        item.GetComponent<FirearmScript>().isExotic = false;
                        item.GetComponent<FirearmScript>().RarityAugment();
                    }
                }               

                if (cOneStr == "1")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                    item.AddComponent<DeepYield>();
                }

                if (cOneStr == "2")
                {
                    item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                    item.AddComponent<DeeperYield>();
                }

                if (cTwoStr == "3")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                    item.AddComponent<DeepStores>();
                }

                if (cTwoStr == "4")
                {
                    item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                    item.AddComponent<DeeperStores>();
                }

                if (cThreeStr == "5")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                    item.AddComponent<FarSight>();
                }

                if (cThreeStr == "6")
                {
                    item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                    item.AddComponent<FartherSight>();
                }

                if (cFourStr == "7")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                    item.AddComponent<HastyHands>();
                }

                if (cFourStr == "8")
                {
                    item.GetComponent<FirearmScript>().reloadCheatOne = 351;
                    item.AddComponent<HastierHands>();
                }

                if (player.readdedWeps[s].Length == 8)
                {
                    if (cFiveStr == "0")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 450;
                        item.AddComponent<WaitNowImReady>();

                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "1")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 475;
                        item.AddComponent<Efficacy>();

                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "2")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 525;
                        item.AddComponent<Inoculated>();

                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "3")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 575;
                        item.AddComponent<RudeAwakening>();

                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 625;
                        item.AddComponent<NotWithAStick>();

                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 675;
                        item.AddComponent<MaliciousWindUp>();

                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 725;
                        item.AddComponent<PositiveNegative>();

                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "7")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 775;
                        item.AddComponent<Cadence>();

                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 825;
                        item.AddComponent<GoodThingsCome>();

                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 875;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = 925;
                        item.AddComponent<TheMostResplendent>();

                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    } //New
                }

                if (player.readdedWeps[s].Length == 9)
                {
                    if (cFiveStr == "B")
                    {
                        item.GetComponent<FirearmScript>().cheatRNG = -2;
                        item.AddComponent<AbsolutelyNoStops>();
                        item.GetComponent<AbsolutelyNoStops>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "9")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 410;
                        item.AddComponent<AllElseFails>();
                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "4")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 415;
                        item.AddComponent<NotWithAStick>();
                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cFiveStr == "5")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 425;
                        item.AddComponent<MaliciousWindUp>();
                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "6")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 435;
                        item.AddComponent<PositiveNegative>();
                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "8")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 445;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;
                    }

                    if (cFiveStr == "!")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 455;
                        item.AddComponent<TheMostResplendent>();
                        item.GetComponent<TheMostResplendent>().proc = item.GetComponent<FirearmScript>().procOne;

                    } //New

                    //Absolutely no breaks! Pairing
                    if (cSixStr == "8")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 445;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procTwo;
                    }

                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 495;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 505;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }

                yield return new WaitForSeconds(spawnDelayTimer);
            }
        }

        track = false;
    } //This method respawns Weapons previously held in their Inventory.

    private void SwitchInv()
    {
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
                    selection = observedWeps.Count - 1;
                }

                else
                {
                    selection--;

                    //Prevents running off the inventory backwards
                    //if (selection <= -1 && inventory.Count >= 1)
                    //{
                    //    selection = 0;
                    //}

                }

                //Prevents running off the inventory backwards
                //if (selection <= -1 && observedWeps.Count >= 1)
                //{
                //    selection = 0;
                //}

                DisplayWeapons();
                appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");

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
                    selection = 0;
                }

                else
                {
                    selection++;

                    //Prevents running off the inventory forwards
                    //if (selection >= inventory.Count)
                    //{
                    //    selection = inventory.Count - 1;
                    //}             
                }

                //Prevents running off the inventory forwards
                //if (selection >= observedWeps.Count)
                //{
                //    selection = observedWeps.Count - 1;
                //}

                DisplayWeapons();
                appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");

            }
        }
    }

    private void DismantleInv()
    {
        if (Input.GetKey(KeyCode.X))
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

        if (Input.GetKeyUp(KeyCode.X))
        {
            dismantleTimer = dismantleTimerReset;
            dismantleText.text = " ";
            dismantleText.color = Color.black;
            return;
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

    //These six methods are used for button actions when interacting with the Inventory or the WeaponManager itself.
    public void SwitchInvLeft()
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
                selection = observedWeps.Count - 1;            
            }

            else
            {
                selection--;

                //Prevents running off the inventory backwards
                //if (selection <= -1 && inventory.Count >= 1)
                //{
                //    selection = 0;
                //}
                
            }

            //Prevents running off the inventory backwards
            //if (selection <= -1 && observedWeps.Count >= 1)
            //{
            //    selection = 0;
            //}

            DisplayWeapons();
            appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");
        }
    }

    public void SwitchInvRight()
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
                selection = 0;             
            }

            else
            {
                selection++;

                //Prevents running off the inventory forwards
                //if (selection >= inventory.Count)
                //{
                //    selection = inventory.Count - 1;
                //}             
            }

            //Prevents running off the inventory forwards
            //if (selection >= observedWeps.Count)
            //{
            //    selection = observedWeps.Count - 1;
            //}

            DisplayWeapons();
            appraisal.text = "Value: " + kiosk.inventoryWorth[selection].ToString("N0");

        }
    }

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

    public void SaveInventory()
    {
        WriteInventory();
    }

    public void CheckForInventoryUpdate()
    {
        if (observedWeps.Count > 0 && selection == -1)
        {
            selection++;
            DisplayWeapons();

            invNavigation.SetActive(true);

        }

        if (observedWeps.Count > 0 && selection >= 0)
        {
            DisplayWeapons();

            invNavigation.SetActive(true);

        }

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
