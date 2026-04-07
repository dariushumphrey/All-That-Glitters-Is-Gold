using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class KioskScript : MonoBehaviour
{
    public int lucentFunds = 0; //Player's Lucent balance
    public float devaluePercent = 20f; //Decreases worth of Player's Weapons by this value
    public float timedResetSeconds = 60f; //Time before Kiosk refreshes items
    public Text lucentText;
    public Text timedResetText;
    public Text purchaseConfirmText;
    public WeaponManagerScript playerInventory;
    public string filepath = "inventory.txt"; //Inventory file name

    public List<string> kioskWares = new List<string>(); //List of Weapons in string form
    public List<float> warePrices = new List<float>(); //List of Weapons' prices from Kiosk
    public List<float> inventoryWorth = new List<float>(); //List of Weapons' prices from Player's Inventory
    public List<Button> wareOptions = new List<Button>(); //List of buttons that hold Weapon info

    public List<Text> prices = new List<Text>(); //List of text that display Weapons' prices
    public List<Text> names = new List<Text>(); //List of text that display Weapons' names
    public List<Text> rarities = new List<Text>(); //List of text that display Weapons' rarities
    public List<Text> platforms = new List<Text>(); //List of text that display Weapons' platforms

    //statOnes, Twos, Threes, Fours - Lists of texts that display Weapons' Stat Cheats
    public List<Text> statOnes = new List<Text>();
    public List<Text> statTwos = new List<Text>();
    public List<Text> statThrees = new List<Text>();
    public List<Text> statFours = new List<Text>();

    //functionOnes, Twos - List of text that display Weapons' Function Cheats
    public List<Text> functionOnes = new List<Text>();
    public List<Text> functionTwos = new List<Text>();

    public Text wepName, flavor, rarityCheck, stats, platform; //Texts that display Weapon stats, 
    public Text cheatOne, cheatTwo, cheatThree, cheatFour, cheatTraitOne, cheatTraitTwo; //Texts that displays Weapon cheats
    public ScrollRect inspectScroll;

    private GameObject item; //GameObject that receives Weapons 
    private string weaponIdentity; //Represents a Weapon in string form
    private float weaponPrice; //Represents a Weapon's price
    private int potentialBuy; //Index that selects a Weapon from the Kiosk's list
    private ColorBlock quality; //Assigns colors to buttons from Weapon rarity
    private int devalue; //Value that subtracts Weapon worth in Player's Inventory
    private float devaluePctReset;
    private float kioskReset;
    private TimeSpan time;
    private string timeFormat; //String that displays Kiosk refresh timer
    private bool paused = false; //Halts Kiosk refresh timer if true

    private int determinate; //Number used to randomly select Weapon attributes
    private string wepTypeStr, wepRarStr, wepExoStr, wepFavStr, wepPlatStr, stOneStr, stTwoStr, stThreeStr, stFourStr, fcOneStr, fcTwoStr;

    private void Awake()
    {
        kioskReset = timedResetSeconds;
        devaluePctReset = devaluePercent;
    }

    // Start is called before the first frame update
    void Start()
    {
        //devaluePctReset = devaluePercent;
        GenerateWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        lucentText.text = "Lucent: " + lucentFunds.ToString("N0");

        //Displays time before Kiosk items are refreshed with new stock
        time = TimeSpan.FromSeconds(timedResetSeconds);
        timeFormat = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
        timedResetText.text = "Refreshes in: " + timeFormat;

        if(!paused)
        {
            timedResetSeconds -= Time.deltaTime;
        }

        if(timedResetSeconds <= 0f)
        {
            timedResetSeconds = kioskReset;
            Start();
        }
    }

    /// <summary>
    /// Produces one set of six Weapons (in string form) for sale
    /// </summary>
    void GenerateWeapons()
    {
        for (int w = 0; w < kioskWares.Count; w++)
        {
            //Determines Weapon Type
            determinate = UnityEngine.Random.Range(0, 8);
            //determinate = 1;
            wepTypeStr = determinate.ToString();

            //Determines Weapon Rarity -- capped to Rarity 3 if Viricide has not been cleared at least once
            //If Viricide has been completed at least once, Weapons starting from Rarity 4 will be sellable
            if (PlayerPrefs.GetInt("firstViricideClear") != 1)
            {
                determinate = UnityEngine.Random.Range(1, 4);
            }

            else
            {
                determinate = UnityEngine.Random.Range(4, 6);
            }

            wepRarStr = determinate.ToString();

            //Determines Weapon Exotic property -- automatically set to 1 if Rarity is 5, set to 0 if not
            if (wepRarStr == "5")
            {
                wepExoStr = "1";
            }

            else
            {
                wepExoStr = "0";
            }

            //Determines Weapon Favorite property -- always zero
            wepFavStr = "0";

            //Determines Weapon Platform property
            determinate = UnityEngine.Random.Range(1, 9);
            wepPlatStr = determinate.ToString();

            //Determines Statistical Cheat values -- automatically set to 2,4,6,8 if Weapon is Exotic
            if (wepRarStr == "5" && wepExoStr == "1")
            {
                stOneStr = "2";
                stTwoStr = "4";
                stThreeStr = "6";
                stFourStr = "8";
            }

            else
            {
                determinate = UnityEngine.Random.Range(1, 3);
                stOneStr = determinate.ToString();

                determinate = UnityEngine.Random.Range(3, 5);
                stTwoStr = determinate.ToString();

                determinate = UnityEngine.Random.Range(5, 7);
                stThreeStr = determinate.ToString();

                determinate = UnityEngine.Random.Range(7, 9);
                stFourStr = determinate.ToString();
            }
            
            //If Rarity is 1, complete string assembly
            if(wepRarStr == "1")
            {
                weaponIdentity = wepTypeStr + wepRarStr + wepExoStr + wepFavStr + wepPlatStr;
            }

            //If Rarity is 2, complete string assembly
            else if (wepRarStr == "2")
            {
                weaponIdentity = wepTypeStr + wepRarStr + wepExoStr + wepFavStr + wepPlatStr + 
                stOneStr + stTwoStr + stThreeStr + stFourStr;
            }

            else if (wepRarStr == "3")
            {
                int act = 0;

                char[] newPool = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '@', '#', '$', '%', '^', '&' };
                act = UnityEngine.Random.Range(0, newPool.Length);
                fcOneStr = newPool[act].ToString();

                weaponIdentity = wepTypeStr + wepRarStr + wepExoStr + wepFavStr + wepPlatStr +
                    stOneStr + stTwoStr + stThreeStr + stFourStr +
                    fcOneStr;
            }

            //If Rarity is 4, complete string assembly
            else if (wepRarStr == "4")
            {
                int choice = 0;

                char[] poolOne = {'9', '4', '5', '6', '8', '!', '@', '#'};
                choice = UnityEngine.Random.Range(0, poolOne.Length);
                fcOneStr = poolOne[choice].ToString();

                char[] poolTwo = {'0', '1', '2', '7', '3', '$', '%', '^'};
                choice = UnityEngine.Random.Range(0, poolOne.Length);
                fcTwoStr = poolTwo[choice].ToString();

                weaponIdentity = wepTypeStr + wepRarStr + wepExoStr + wepFavStr + wepPlatStr +
                    stOneStr + stTwoStr + stThreeStr + stFourStr +
                    fcOneStr + fcTwoStr;
            }

            //If Rarity 5 (Exotic), complete string assembly based on Weapon type
            else if (wepRarStr == "5" && wepExoStr == "1")
            {
                if(wepTypeStr == "0")
                {
                    wepPlatStr = "5";
                    fcOneStr = "A";
                    fcTwoStr = "2";
                }

                if (wepTypeStr == "1")
                {
                    wepPlatStr = "6";
                    fcOneStr = "G";
                    fcTwoStr = "!";
                }

                if (wepTypeStr == "2")
                {
                    wepPlatStr = "2";
                    fcOneStr = "C";
                    fcTwoStr = "$";
                }

                if (wepTypeStr == "3")
                {
                    wepPlatStr = "7";
                    fcOneStr = "F";
                    fcTwoStr = "0";
                }

                if (wepTypeStr == "4")
                {
                    wepPlatStr = "4";
                    fcOneStr = "D";
                    fcTwoStr = "4";
                }

                if (wepTypeStr == "5")
                {
                    wepPlatStr = "8";
                    fcOneStr = "E";
                    fcTwoStr = "1";
                }

                if (wepTypeStr == "6")
                {
                    wepPlatStr = "3";
                    fcOneStr = "B";
                    fcTwoStr = "#";
                }

                if (wepTypeStr == "7")
                {
                    wepPlatStr = "3";
                    fcOneStr = "H";
                    fcTwoStr = "6";
                }

                weaponIdentity = wepTypeStr + wepRarStr + wepExoStr + wepFavStr + wepPlatStr +
                    stOneStr + stTwoStr + stThreeStr + stFourStr +
                    fcOneStr + fcTwoStr;
            }

            kioskWares[w] = weaponIdentity;

        }

        PriceWeapons();

    }

    /// <summary>
    /// Generates prices for the Kiosk's Weapons
    /// </summary>
    void PriceWeapons()
    {
        string s = "Comic Sans";

        for (int p = 0; p < kioskWares.Count; p++)
        {
            quality = wareOptions[p].GetComponent<Button>().colors;
            quality.pressedColor = Color.gray;
            quality.disabledColor = Color.black;

            s = kioskWares[p];

            wepTypeStr = s[0].ToString();
            wepRarStr = s[1].ToString();
            wepExoStr = s[2].ToString();
            wepFavStr = s[3].ToString();
            wepPlatStr = s[4].ToString();
           
            if (kioskWares[p].Length >= 9)
            {
                stOneStr = s[5].ToString();
                stTwoStr = s[6].ToString();
                stThreeStr = s[7].ToString();
                stFourStr = s[8].ToString();

                if (kioskWares[p].Length == 10)
                {
                    fcOneStr = s[9].ToString();
                }

                if (kioskWares[p].Length == 11)
                {
                    fcOneStr = s[9].ToString();
                    fcTwoStr = s[10].ToString();
                }
            }                    

            if (wepTypeStr == "0")
            {
                weaponPrice = 1300f;
                names[p].text = "Full Fire Rifle";               
            }

            if (wepTypeStr == "1")
            {
                weaponPrice = 1600f;
                names[p].text = "Machine Gun";             
            }

            if (wepTypeStr == "2")
            {
                weaponPrice = 1100f;
                names[p].text = "Pistol";                
            }

            if (wepTypeStr == "3")
            {
                weaponPrice = 1500f;
                names[p].text = "Semi Fire Rifle";              
            }

            if (wepTypeStr == "4")
            {
                weaponPrice = 1700f;
                names[p].text = "Shotgun";              
            }

            if (wepTypeStr == "5")
            {
                weaponPrice = 1400f;
                names[p].text = "Single Fire Rifle";               
            }

            if (wepTypeStr == "6")
            {
                weaponPrice = 1200f;
                names[p].text = "Submachine Gun";              
            }

            if (wepTypeStr == "7")
            {
                weaponPrice = 1800f;
                names[p].text = "Grenade Launcher";
            }

            if (wepRarStr == "1")
            {
                rarities[p].text = "Usual";
                statOnes[p].text = "";
                statTwos[p].text = "";
                statThrees[p].text = "";
                statFours[p].text = "";
                functionOnes[p].text = "";
                functionTwos[p].text = "";
                quality.normalColor = Color.white;
                quality.highlightedColor = Color.white;

            }

            if (wepRarStr == "2")
            {
                weaponPrice *= 2;
                rarities[p].text = "Sought";
                functionOnes[p].text = "";
                functionTwos[p].text = "";
                quality.normalColor = Color.green;
                quality.highlightedColor = Color.green;

            }

            if (wepRarStr == "3")
            {
                weaponPrice *= 3;
                rarities[p].text = "Coveted";
                functionTwos[p].text = "";
                quality.normalColor = Color.red;
                quality.highlightedColor = Color.red;
            }

            if (wepRarStr == "4")
            {
                weaponPrice *= 4;
                rarities[p].text = "Treasured";
                quality.normalColor = Color.yellow;
                quality.highlightedColor = Color.yellow;
            }

            if (wepRarStr == "5")
            {
                if (wepExoStr == "1")
                {
                    weaponPrice *= 6;
                    rarities[p].text = "Exotic";
                    quality.normalColor = Color.cyan;
                    quality.highlightedColor = Color.cyan;

                    if (wepTypeStr == "0")
                    {
                        names[p].text = "Outstanding Warrant";
                    }

                    if (wepTypeStr == "1")
                    {
                        names[p].text = "The Dismissal";
                    }

                    if (wepTypeStr == "2")
                    {
                        names[p].text = "Apathetic";
                    }

                    if (wepTypeStr == "3")
                    {
                        names[p].text = "Mercies";
                    }

                    if (wepTypeStr == "4")
                    {
                        names[p].text = "Viral Shadow";
                    }

                    if (wepTypeStr == "5")
                    {
                        names[p].text = "Contempt For Fellows";
                    }

                    if (wepTypeStr == "6")
                    {
                        names[p].text = "Underfoot";
                    }

                    if (wepTypeStr == "7")
                    {
                        names[p].text = "Nebulous At Best";
                    }
                }

                else
                {
                    weaponPrice *= 5;
                    rarities[p].text = "Fated"; //Fated-rarity non-Exotic Weapons shouldn't appear, but the case is caught here anyway.
                }
            }

            wareOptions[p].GetComponent<Button>().colors = quality;

            if (wepPlatStr == "1")
            {
                weaponPrice += 350f;
                platforms[p].text = "Default\n" +
                            "<i>Standard performance.</i>";
            }

            else if (wepPlatStr == "2")
            {
                weaponPrice += 350f;
                platforms[p].text = "Efficient\n" +
                            "<i>Slow-firing, higher damage.</i>";
            }

            else if (wepPlatStr == "3")
            {
                weaponPrice += 350f;
                platforms[p].text = "Chatter\n" +
                            "<i>Fast-firing, lower damage.</i>";
            }

            else if (wepPlatStr == "4")
            {
                weaponPrice += 350f;
                platforms[p].text = "Tempered\n" +
                            "<i>Optimal damage, control.</i>";
            }

            else if (wepPlatStr == "5")
            {
                weaponPrice += 175f;
                platforms[p].text = "Siphonic\n" +
                            "<i>1% Health, Shield on hits.</i>";
            }

            else if (wepPlatStr == "6")
            {
                weaponPrice += 175f;
                platforms[p].text = "Mining\n" +
                            "<i>Fires explosive Lucent rounds.</i>";
            }

            else if (wepPlatStr == "7")
            {
                weaponPrice += 175f;
                platforms[p].text = "Trenchant\n" +
                            "<i>Debuffs on hits and evasions.</i>";
            }

            else
            {
                weaponPrice += 175f;
                platforms[p].text = "Cache\n" +
                            "<i>Regenerate all grenades.</i>";
            }

            if (kioskWares[p].Length >= 9)
            {
                if (stOneStr == "1")
                {
                    weaponPrice += 625f;
                    statOnes[p].text = "+MAG";
                }

                if (stOneStr == "2")
                {
                    weaponPrice += 1250f;
                    statOnes[p].text = "++MAG";
                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 625f;
                    statTwos[p].text = "+RES";
                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 1250f;
                    statTwos[p].text = "++RES";
                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 625f;
                    statThrees[p].text = "+EFR";
                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 1250f;
                    statThrees[p].text = "++EFR";
                }

                if (stFourStr == "7")
                {
                    weaponPrice += 625f;
                    statFours[p].text = "+RLD";
                }

                if (stFourStr == "8")
                {
                    weaponPrice += 1250f;
                    statFours[p].text = "++RLD";
                }

                if (kioskWares[p].Length == 10)
                {
                    weaponPrice += 10000f;

                    if (fcOneStr == "0")
                    {
                        functionOnes[p].text = "Wait! Now I'm Ready!\n" +
                        "<i>10% Shield on kills.</i>";
                    }

                    if (fcOneStr == "1")
                    {
                        functionOnes[p].text = "Efficacy\n" +
                        "<i>1% Damage on hits.</i>";
                    }

                    if (fcOneStr == "2")
                    {
                        functionOnes[p].text = "Inoculated\n" +
                        "<i>5% Health on kills.</i>";
                    }

                    if (fcOneStr == "3")
                    {
                        functionOnes[p].text = "Rude Awakening\n" +
                        "<i>Cast AOE damage waves.</i>";
                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick\n" +
                        "<i>35% Effective Range on kills.</i>";
                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-Up\n" +
                        "<i>Hits increase Reload Speed.</i>";
                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative\n" +
                        "<i>Apply damage-over-time.</i>";
                    }

                    if (fcOneStr == "7")
                    {
                        functionOnes[p].text = "Cadence\n" +
                        "<i>Clusters on third kills.</i>";
                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come\n" +
                        "<i>Grants benefits during combat.</i>";
                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails\n" +
                        "<i>Immmuity on Shield breaks.</i>";
                    }

                    if (fcOneStr == "!")
                    {
                        functionOnes[p].text = "The Most Resplendent\n" +
                        "<i>Attach crystals to surfaces.</i>";
                    }

                    if (fcOneStr == "@")
                    {
                        functionOnes[p].text = "Fulminate\n" +
                        "<i>2% Des. Grenade damage on hits.</i>";
                    }

                    if (fcOneStr == "#")
                    {
                        functionOnes[p].text = "Forager\n" +
                        "<i>Kills produce pickup bursts.</i>";
                    }

                    if (fcOneStr == "$")
                    {
                        functionOnes[p].text = "Counterplay\n" +
                        "<i>Evading damage create Clusters.</i>";
                    }

                    if (fcOneStr == "%")
                    {
                        functionOnes[p].text = "Enshroud\n" +
                        "<i>15% Melee range on hits.</i>";
                    }

                    if (fcOneStr == "^")
                    {
                        functionOnes[p].text = "Gale Force Winds\n" +
                        "<i>Cast debuffing winds.</i>";
                    }

                    if (fcOneStr == "&")
                    {
                        functionOnes[p].text = "Activator Drone\n" +
                        "<i>Drone attacks, triggers effects.</i>";
                    }

                    functionTwos[p].text = " ";
                }

                if (kioskWares[p].Length == 11)
                {
                    weaponPrice += 20000f;

                    //Exotic Functional Cheats
                    if (fcOneStr == "A")
                    {
                        functionOnes[p].text = "Equivalent Exchange\n" +
                        "<i>Taking damage increases damage.</i>";

                    }

                    if (fcOneStr == "G")
                    {
                        functionOnes[p].text = "Pay to Win\n" +
                        "<i>Spend Lucent for damage.</i>";

                    }

                    if (fcOneStr == "C")
                    {
                        functionOnes[p].text = "Superweapon\n" +
                        "<i>Charge extreme-damage shots.</i>";

                    }

                    if (fcOneStr == "F")
                    {
                        functionOnes[p].text = "Volant\n" +
                        "<i>Enable character flight.</i>";

                    }

                    if (fcOneStr == "D")
                    {
                        functionOnes[p].text = "Social Distance, please!\n" +
                        "<i>Spreads 400% damage on kills.</i>";

                    }

                    if (fcOneStr == "E")
                    {
                        functionOnes[p].text = "Early Berth gets the Hearst\n" +
                        "<i>Every other shot explodes.</i>";

                    }

                    if (fcOneStr == "B")
                    {
                        functionOnes[p].text = "Absolutely no Stops\n" +
                        "<i>Firing increases fire rate.</i>";

                    }

                    if (fcOneStr == "H")
                    {
                        functionOnes[p].text = "Flashpoint\n" +
                        "<i>Fire Lucent mines.</i>";

                    }


                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails\n" +
                        "<i>Immmuity on Shield breaks.</i>";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick\n" +
                        "<i>35% Effective Range on kills.</i>";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-Up\n" +
                        "<i>Hits increase Reload Speed.</i>";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative\n" +
                        "<i>Apply damage-over-time.</i>";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come\n" +
                        "<i>Grants benefits during combat.</i>";

                    }

                    if (fcOneStr == "!")
                    {
                        functionOnes[p].text = "The Most Resplendent\n" +
                        "<i>Attach crystals to surfaces.</i>";

                    }

                    if (fcOneStr == "@")
                    {
                        functionOnes[p].text = "Fulminate\n" +
                        "<i>2% Des. Grenade damage on hits.</i>";

                    }

                    if (fcOneStr == "#")
                    {
                        functionOnes[p].text = "Forager\n" +
                        "<i>Kills produce pickup bursts.</i>";

                    }

                    if (fcOneStr == "&")
                    {
                        functionOnes[p].text = "Activator Drone\n" +
                        "<i>Drone attacks, triggers effects.</i>";

                    }


                    if (fcTwoStr == "!")
                    {
                        functionTwos[p].text = "The Most Resplendent\n" +
                        "<i>Attach crystals to surfaces.</i>";
                    } //Pay to Win pairing

                    if (fcTwoStr == "4")
                    {
                        functionTwos[p].text = "Not with a Stick\n" +
                        "<i>35% Effective Range on kills.</i>";

                    } //Social Distance, Please! pairing

                    if (fcTwoStr == "#")
                    {
                        functionTwos[p].text = "Forager\n" +
                        "<i>Kills produce pickup bursts.</i>";

                    } //Absolutely no Stops! pairing

                    if (fcTwoStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative\n" +
                        "<i>Apply damage-over-time.</i>";

                    } //Flashpoint pairing

                    if (fcTwoStr == "0")
                    {
                        functionTwos[p].text = "Wait! Now I'm Ready!\n" +
                        "<i>10% Shield on kills.</i>";
                    } //Volant pairing

                    if (fcTwoStr == "1")
                    {
                        functionTwos[p].text = "Efficacy\n" +
                        "<i>1% Damage on hits.</i>";

                    } //Early Berth gets the Hearst pairing

                    if (fcTwoStr == "2")
                    {
                        functionTwos[p].text = "Inoculated\n" +
                        "<i>5% Health on kills.</i>";

                    } //Equivalent Exchange pairing

                    if (fcTwoStr == "7")
                    {
                        functionTwos[p].text = "Cadence\n" +
                        "<i>Clusters on third kills.</i>";

                    }

                    if (fcTwoStr == "3")
                    {
                        functionTwos[p].text = "Rude Awakening\n" +
                        "<i>Cast AOE damage waves.</i>";

                    }

                    if (fcTwoStr == "$")
                    {
                        functionTwos[p].text = "Counterplay\n" +
                        "<i>Evading damage create Clusters.</i>";

                    } //Superweapon pairing

                    if (fcTwoStr == "%")
                    {
                        functionTwos[p].text = "Enshroud\n" +
                        "<i>15% Melee range on hits.</i>";

                    }

                    if (fcTwoStr == "^")
                    {
                        functionTwos[p].text = "Gale Force Winds\n" +
                        "<i>Cast debuffing winds.</i>";

                    }
                }
            }                   

            warePrices[p] = weaponPrice;
            prices[p].text = weaponPrice.ToString("N0");
        }
    }

    /// <summary>
    /// Generates prices for the Player's Weapons
    /// </summary>
    void PriceInventory()
    {
        if(inventoryWorth.Count > 0)
        {
            inventoryWorth.Clear();
        }

        string s = "Comic Sans";

        for (int p = 0; p < playerInventory.observedWeps.Count; p++)
        {

            s = playerInventory.observedWeps[p];

            wepTypeStr = s[0].ToString();
            wepRarStr = s[1].ToString();
            wepExoStr = s[2].ToString();
            wepFavStr = s[3].ToString();
            wepPlatStr = s[4].ToString();

            if(playerInventory.observedWeps[p].Length >= 9)
            {
                stOneStr = s[5].ToString();
                stTwoStr = s[6].ToString();
                stThreeStr = s[7].ToString();
                stFourStr = s[8].ToString();

                if (playerInventory.observedWeps[p].Length == 10)
                {
                    fcOneStr = s[9].ToString();
                }

                if (playerInventory.observedWeps[p].Length == 11)
                {
                    fcOneStr = s[9].ToString();
                    fcTwoStr = s[10].ToString();
                }
            }

            if (wepTypeStr == "0")
            {
                weaponPrice = 1300f;            
            }

            if (wepTypeStr == "1")
            {
                weaponPrice = 1600f;              
            }

            if (wepTypeStr == "2")
            {
                weaponPrice = 1100f;              
            }

            if (wepTypeStr == "3")
            {
                weaponPrice = 1500f;              
            }

            if (wepTypeStr == "4")
            {
                weaponPrice = 1700f;              
            }

            if (wepTypeStr == "5")
            {
                weaponPrice = 1400f;               
            }

            if (wepTypeStr == "6")
            {
                weaponPrice = 1200f;             
            }

            if (wepTypeStr == "7")
            {
                weaponPrice = 1800f;
            }

            if (wepPlatStr == "1")
            {
                weaponPrice += 350f;
            }

            else if (wepPlatStr == "2")
            {
                weaponPrice += 350f;
            }

            else if (wepPlatStr == "3")
            {
                weaponPrice += 350f;
            }

            else if (wepPlatStr == "4")
            {
                weaponPrice += 350f;
            }

            else if (wepPlatStr == "5")
            {
                weaponPrice += 175f;
            }

            else if (wepPlatStr == "6")
            {
                weaponPrice += 175f;
            }

            else if (wepPlatStr == "7")
            {
                weaponPrice += 175f;
            }

            else
            {
                weaponPrice += 175f;
            }

            if (wepRarStr == "2")
            {
                weaponPrice *= 2;
            }

            if (wepRarStr == "3")
            {
                weaponPrice *= 3;
            }

            if (wepRarStr == "4")
            {
                weaponPrice *= 4;
            }

            if (wepRarStr == "5")
            {
                if (wepExoStr == "1")
                {
                    weaponPrice *= 6;
                }

                else
                {
                    weaponPrice *= 5;
                }
            }

            if (stOneStr == "1")
            {
                weaponPrice += 625f;

            }

            if (stOneStr == "2")
            {
                weaponPrice += 1250f;

            }

            if (stTwoStr == "3")
            {
                weaponPrice += 625f;

            }

            if (stTwoStr == "4")
            {
                weaponPrice += 1250f;

            }

            if (stThreeStr == "5")
            {
                weaponPrice += 625f;

            }

            if (stThreeStr == "6")
            {
                weaponPrice += 1250f;

            }

            if (stFourStr == "7")
            {
                weaponPrice += 625f;

            }

            if (stFourStr == "8")
            {
                weaponPrice += 1250f;

            }

            if (playerInventory.observedWeps[p].Length == 8)
            {
                weaponPrice += 10000f;
            }

            if (playerInventory.observedWeps[p].Length == 9)
            {
                weaponPrice += 20000f;
            }

            devaluePercent /= 100;
            devaluePercent *= weaponPrice;
            devalue = (int)devaluePercent;
            weaponPrice -= devalue;

            devaluePercent = devaluePctReset;

            inventoryWorth.Add(weaponPrice);
        }
    }

    /// <summary>
    /// Displays price, attributes from a Weapon being sold from the Kiosk
    /// </summary>
    public void AssessWeapon(int q)
    {
        if (playerInventory.gameObject.transform.childCount > 0)
        {
            foreach (Transform child in playerInventory.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        string c = "Comic Sans";

        c = kioskWares[q];
        potentialBuy = q;

        wepTypeStr = c[0].ToString();
        wepRarStr = c[1].ToString();
        wepExoStr = c[2].ToString();
        wepFavStr = c[3].ToString();
        wepPlatStr = c[4].ToString();

        if(kioskWares[q].Length >= 9)
        {
            stOneStr = c[5].ToString();
            stTwoStr = c[6].ToString();
            stThreeStr = c[7].ToString();
            stFourStr = c[8].ToString();

            if (kioskWares[q].Length == 10)
            {
                fcOneStr = c[9].ToString();
            }

            if (kioskWares[q].Length == 11)
            {
                fcOneStr = c[9].ToString();
                fcTwoStr = c[10].ToString();
            }
        }

        if (wepTypeStr == "0")
        {
            wepName.text = "Full Fire Rifle";
            item = Instantiate(playerInventory.weapons[0], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[0].name;
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;           
        }

        if (wepTypeStr == "1")
        {
            wepName.text = "Machine Gun";
            item = Instantiate(playerInventory.weapons[1], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[1].name;
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;          
        }

        if (wepTypeStr == "2")
        {
            wepName.text = "Pistol";
            item = Instantiate(playerInventory.weapons[2], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[2].name;
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;          
        }

        if (wepTypeStr == "3")
        {
            wepName.text = "Semi Fire Rifle";
            item = Instantiate(playerInventory.weapons[3], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[3].name;
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;           
        }

        if (wepTypeStr == "4")
        {
            wepName.text = "Shotgun";
            item = Instantiate(playerInventory.weapons[4], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[4].name;
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;          
        }

        if (wepTypeStr == "5")
        {
            wepName.text = "Single Fire Rifle";
            item = Instantiate(playerInventory.weapons[5], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[5].name;
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;         
        }

        if (wepTypeStr == "6")
        {
            wepName.text = "Submachine Gun";
            item = Instantiate(playerInventory.weapons[6], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[6].name;
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;          
        }

        if (wepTypeStr == "7")
        {
            wepName.text = "Grenade Launcher";
            item = Instantiate(playerInventory.weapons[7], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[7].name;
            //flavor.text = item.GetComponent<FirearmScript>().flavorText;          
        }

        if (wepPlatStr == "1")
        {
            item.AddComponent<DefaultPlatform>();
            platform.text = "Default Platform - Standard performance.";
        }

        else if (wepPlatStr == "2")
        {
            item.AddComponent<EfficientPlatform>();
            item.GetComponent<EfficientPlatform>().Start();
            platform.text = "Efficient Platform - Configured for slow-firing, high-damage.";
        }

        else if (wepPlatStr == "3")
        {
            item.AddComponent<ChatterPlatform>();
            item.GetComponent<ChatterPlatform>().Start();
            platform.text = "Chatter Platform - Configured for fast-firing, low-damage.";
        }

        else if (wepPlatStr == "4")
        {
            item.AddComponent<TemperedPlatform>();
            item.GetComponent<TemperedPlatform>().Start();
            platform.text = "Tempered Platform - Tuned for highest damage, improved firing and control.";
        }

        else if (wepPlatStr == "5")
        {
            item.AddComponent<SiphonicPlatform>();
            item.GetComponent<SiphonicPlatform>().Start();
            platform.text = "Siphonic Platform - Weapon hits restore 1% Health & Shield. Melee Kills restore 15% Health & Shield.";
        }

        else if (wepPlatStr == "6")
        {
            item.AddComponent<MiningPlatform>();
            item.GetComponent<MiningPlatform>().Start();
            platform.text = "Mining Platform - Fires Lucent explosive rounds.";
        }

        else if (wepPlatStr == "7")
        {
            item.AddComponent<TrenchantPlatform>();
            item.GetComponent<TrenchantPlatform>().Start();
            platform.text = "Trenchant Platform -" + "\n" +
                "Evasions slow enemies." + "\n" +
                "Melees apply damage-over-time." + "\n" +
                "Weapon hits apply Health debuffs.";
        }

        else
        {
            item.AddComponent<CachePlatform>();
            item.GetComponent<CachePlatform>().Start();
            platform.text = "Cache Platform - Regenerates all grenades every two seconds." + "\n" +
                "Activator Drones fire mini-Rockets.";
        }

        if (wepRarStr == "1")
        {
            rarityCheck.text = "Usual";
            cheatOne.text = "";
            cheatTwo.text = "";
            cheatThree.text = "";
            cheatFour.text = "";
            cheatTraitOne.text = "";
            cheatTraitTwo.text = "";
        }

        if (wepRarStr == "2")
        {
            rarityCheck.text = "Sought";
            cheatTraitOne.text = "";
            cheatTraitTwo.text = "";
            item.GetComponent<FirearmScript>().weaponRarity = 2;
            item.GetComponent<FirearmScript>().RarityAugment();
        }

        if (wepRarStr == "3")
        {
            rarityCheck.text = "Coveted";
            cheatTraitTwo.text = "";
            item.GetComponent<FirearmScript>().weaponRarity = 3;
            item.GetComponent<FirearmScript>().RarityAugment();
        }

        if (wepRarStr == "4")
        {
            rarityCheck.text = "Treasured";
            item.GetComponent<FirearmScript>().weaponRarity = 4;
            item.GetComponent<FirearmScript>().RarityAugment();
        }

        flavor.text = item.GetComponent<FirearmScript>().flavorText;

        if (wepRarStr == "5")
        {
            item.GetComponent<FirearmScript>().weaponRarity = 5;

            if (wepExoStr == "1")
            {
                rarityCheck.text = "Exotic";

                if (wepTypeStr == "0")
                {
                    wepName.text = "Outstanding Warrant";
                    item.GetComponent<FirearmScript>().flavorText = "''The time has come to collect.'' ";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                }

                if (wepTypeStr == "1")
                {
                    wepName.text = "The Dismissal";
                    item.GetComponent<FirearmScript>().flavorText = "''Wealth is found in the hoard. The Replevin know this; They are territorial, but are otherwise unbothersome.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                }

                if (wepTypeStr == "2")
                {
                    wepName.text = "Apathetic";
                    item.GetComponent<FirearmScript>().flavorText = "At what point do the restraints simply fail?'";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                }

                if (wepTypeStr == "3")
                {
                    wepName.text = "Mercies";
                    item.GetComponent<FirearmScript>().flavorText = "FORAGER operatives are known for on-site procurement tactics. ''Looting'' is a word the robbed often use.";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                }

                if (wepTypeStr == "4")
                {
                    wepName.text = "Viral Shadow";
                    item.GetComponent<FirearmScript>().flavorText = "''The Resplendent fell victim to a first-of-its-kind case of ''Interstellar agro-terrorism''. This was no mere plot.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                }

                if (wepTypeStr == "5")
                {
                    wepName.text = "Contempt For Fellows";
                    item.GetComponent<FirearmScript>().flavorText = "''Force-feeding Replevin Lucent, a resource known to modify behavior on ingestion, defies most known interstellar laws.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                }

                if (wepTypeStr == "6")
                {
                    wepName.text = "Underfoot";
                    item.GetComponent<FirearmScript>().flavorText = "Using this Weapon feels like a perpetual Calvary charge. For where you're going, you won't be needing any breaks.";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                }

                if (wepTypeStr == "7")
                {
                    wepName.text = "Nebulous At Best";
                    item.GetComponent<FirearmScript>().flavorText = "Just when you thought you understood how the world works.";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                }

                item.GetComponent<FirearmScript>().RarityAugment();
            }

            else
            {
                rarityCheck.text = "Fated";
                item.GetComponent<FirearmScript>().RarityAugment();
                flavor.text = item.GetComponent<FirearmScript>().flavorText;
            }
        }

        if (wepTypeStr == "0" || wepTypeStr == "1" || wepTypeStr == "6")
        {
            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 10000).ToString() + " RPM";
        }

        else if (wepTypeStr == "2" || wepTypeStr == "3" || wepTypeStr == "5")
        {
            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 1000).ToString() + " RPM";
        }

        else if (wepTypeStr == "4" || wepTypeStr == "7")
        {
            stats.text = "Damage: " + item.GetComponent<FirearmScript>().damage.ToString() + "\n" +
                         "Reload Speed: " + item.GetComponent<FirearmScript>().reloadSpeed.ToString("F2") + "s" + "\n" +
                         "Effective Range " + item.GetComponent<FirearmScript>().effectiveRange.ToString() + "m" + "\n" +
                         "Total Range: " + item.GetComponent<FirearmScript>().range.ToString() + "m" + "\n" +
                         "Magazine: " + item.GetComponent<FirearmScript>().ammoSize.ToString() + "\n" +
                         "Max Reserves: " + item.GetComponent<FirearmScript>().reserveSize.ToString() + "\n" +
                         "Rate of Fire: " + Mathf.Round(item.GetComponent<FirearmScript>().fireRate * 100).ToString() + " RPM";
        }

        if (kioskWares[q].Length >= 9)
        {
            if (stOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "<i>12% Magazine Size</i>";
                item.AddComponent<DeepYield>();
            }

            if (stOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "<i>24% Magazine Size</i>";
                item.AddComponent<DeeperYield>();
            }

            if (stTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "<i>15% Reserves Size</i>";
                item.AddComponent<DeepStores>();
            }

            if (stTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "<i>30% Reserves Size</i>";
                item.AddComponent<DeeperStores>();
            }

            if (stThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "<i>10% Effective Range Increase</i>";
                item.AddComponent<FarSight>();
            }

            if (stThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "<i>20% Effective Range Increase</i>";
                item.AddComponent<FartherSight>();
            }

            if (stFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "<i>15% Reload Speed Increase</i>";
                item.AddComponent<HastyHands>();
            }

            if (stFourStr == "8")
            {
                cheatFour.text = "Hastier Hands" + "\n" + "<i>25% Reload Speed Increase</i>";
                item.AddComponent<HastierHands>();
            }

            if(kioskWares[q].Length == 10)
            {
                if (fcOneStr == "0")
                {
                    cheatTraitOne.text = "Wait! Now I'm Ready!" + "\n" +
                        "Kills with this Weapon restore 10% of Shield strength.";

                }

                if (fcOneStr == "1")
                {
                    cheatTraitOne.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";
                }

                if (fcOneStr == "2")
                {
                    cheatTraitOne.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";
                }

                if (fcOneStr == "3")
                {
                    cheatTraitOne.text = "Rude Awakening" + '\n' +
                        "Kills grant casts of a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x." + '\n' +
                        "'E' - Cast Blast";
                }

                if (fcOneStr == "4")
                {
                    cheatTraitOne.text = "Not with a Stick" + '\n' +
                        "Kills with this Weapon increase Effective Range by 30% of max Range until your next reload.";
                }

                if (fcOneStr == "5")
                {
                    cheatTraitOne.text = "Malicious Wind-Up" + '\n' +
                        "Inflicting damage increases Reload Speed by 0.75%. This bonus activates on your next reload.";
                }

                if (fcOneStr == "6")
                {
                    cheatTraitOne.text = "Positive-Negative" + '\n' +
                        "Moving generates a charge. While halfway charged, Enemy hits applies 100% of Weapon damage as damage-over-time for ten seconds.";
                }

                if (fcOneStr == "7")
                {
                    cheatTraitOne.text = "Cadence" + '\n' +
                        "Every third Enemy kill spawns a Lucent cluster.";
                }

                if (fcOneStr == "8")
                {
                    cheatTraitOne.text = "Good Things Come" + '\n' +
                        "Being in combat for three seconds grants 25% Movement Speed, 20% damage reduction, and 45% Recoil reduction until you leave combat.";
                }

                if (fcOneStr == "9")
                {
                    cheatTraitOne.text = "All Else Fails" + '\n' +
                        "When Shield is depleted, all incoming Enemy damage is nullified for three seconds. Cooldown: 20 Seconds.";
                }

                if (fcOneStr == "!")
                {
                    cheatTraitOne.text = "The Most Resplendent" + '\n' +
                    "Create a Hard Lucent crystal on surfaces or Enemies that produces Lucent clusters passively or when shot." + '\n' +
                    "'[E]' - Toggle cast";
                }

                if (fcOneStr == "@")
                {
                    cheatTraitOne.text = "Fulminate" + '\n' +
                    "Enemy hits increase Destruct Grenade damage by 2%, up to 70%, for seven seconds. Melee kills cast a Destruct Grenade.";
                }

                if (fcOneStr == "#")
                {
                    cheatTraitOne.text = "Forager" + '\n' +
                    "Weapon or Melee kills produce a burst of Lucent clusters, 1% Health, 2% Shield, and 15% Ammo pickups.";
                }

                if (fcOneStr == "$")
                {
                    cheatTraitOne.text = "Counterplay" + '\n' +
                    "Hits taken while immune during Evasions casts two Lucent clusters and permanently increases Weapon damage by 10%. Stacks 3x.";
                }

                if (fcOneStr == "%")
                {
                    cheatTraitOne.text = "Enshroud" + '\n' +
                    "Enemy hits increase Melee range by 15%, up to 200%, for seven seconds. Melee kills cast a Fogger Grenade. Cooldown: 12 seconds.";
                }

                if (fcOneStr == "^")
                {
                    cheatTraitOne.text = "Gale Force Winds" + '\n' +
                    "Cast traveling winds from Sprinting or moving that applies Health and Slowed debuffs to Enemies." + '\n' +
                    "'[E]' - Toggle cast";
                }

                if (fcOneStr == "&")
                {
                    cheatTraitOne.text = "Activator Drone" + '\n' +
                        "Passively attacks enemies, or can receive a target by aiming. Attacks trigger weapon passives.";
                }

                cheatTraitTwo.text = " ";
            }

            if (kioskWares[q].Length == 11)
            {
                //Exotic Functional Cheats
                if (fcOneStr == "A")
                {
                    cheatTraitOne.text = "Equivalent Exchange" + '\n' +
                        "Taking Enemy damage adds 35% of damage received to this Weapon's base damage and to your Health. Base damage can increase up to 150%.";
                }

                if (fcOneStr == "G")
                {
                    cheatTraitOne.text = "Pay to Win" + '\n' +
                        "Consume 30,000 Lucent to grant stacks of a 50% Weapon damage increase. Stacks 150x." + "\n" +
                        "'E' - Consume Lucent";
                }

                if (fcOneStr == "C")
                {
                    cheatTraitOne.text = "Superweapon" + '\n' +
                        "Kills grant stacks of damage resistance. Stacks 8x. [E] - Charge an extreme-damage shot, inflicting 1000% of Weapon damage per stack.";
                }

                if (fcOneStr == "F")
                {
                    cheatTraitOne.text = "Volant" + '\n' +
                        "[E] - Enables character flight until Shield is broken or disenaged.";
                }

                if (fcOneStr == "D")
                {
                    cheatTraitOne.text = "Social Distance, please!" + '\n' +
                        "Weapon hits temporarily increase Weapon damage by 30% and adds a Health debuff. Kills spread 400% of Weapon damage to nearby enemies.";
                }

                if (fcOneStr == "E")
                {
                    cheatTraitOne.text = "The Early Berth gets the Hearst" + '\n' +
                        "Every other Enemy hit triggers a Berth detonation, inflicting 200% of Weapon damage.";
                }

                if (fcOneStr == "B")
                {
                    cheatTraitOne.text = "Absolutely No Stops" + '\n' +
                        "Expending your magazine fills it from reserves, amplifies Weapon damage by 200%, and increases Rate of Fire by 50%.";
                }

                if (fcOneStr == "H")
                {
                    cheatTraitOne.text = "Flashpoint" + '\n' +
                        "Fires floating Lucent mines. [E] - Detonates all active mines.";
                }


                if (fcOneStr == "9")
                {
                    cheatTraitOne.text = "All Else Fails" + '\n' +
                        "When Shield is depleted, all incoming Enemy damage is nullified for three seconds. Cooldown: 20 Seconds.";
                }

                if (fcOneStr == "4")
                {
                    cheatTraitOne.text = "Not with a Stick" + '\n' +
                        "Kills with this Weapon increase Effective Range by 30% of max Range until your next reload.";
                }

                if (fcOneStr == "5")
                {
                    cheatTraitOne.text = "Malicious Wind-Up" + '\n' +
                        "Inflicting Damage increases Reload Speed by 0.75%. This bonus activates on your next reload.";
                }

                if (fcOneStr == "6")
                {
                    cheatTraitOne.text = "Positive-Negative" + '\n' +
                        "Moving generates a charge. While halfway charged, Enemy hits applies 100% of Weapon damage as damage-over-time for ten seconds.";
                }

                if (fcOneStr == "8")
                {
                    cheatTraitOne.text = "Good Things Come" + '\n' +
                        "Being in combat for three seconds grants 25% Movement Speed, 20% damage reduction, and 45% Recoil reduction until you leave combat.";
                }

                if (fcOneStr == "!")
                {
                    cheatTraitOne.text = "The Most Resplendent" + '\n' +
                    "Create a Hard Lucent crystal on surfaces or Enemies that produces Lucent clusters passively or when shot." + '\n' +
                    "'[E]' - Toggle cast";
                }

                if (fcOneStr == "@")
                {
                    cheatTraitOne.text = "Fulminate" + '\n' +
                    "Enemy hits increase Destruct Grenade damage by 2%, up to 70%, for seven seconds. Melee kills cast a Destruct Grenade.";
                }

                if (fcOneStr == "#")
                {
                    cheatTraitOne.text = "Forager" + '\n' +
                    "Weapon or Melee kills produce a burst of Lucent clusters, 1% Health, 2% Shield, and 15% Ammo pickups.";
                }

                if (fcOneStr == "&")
                {
                    cheatTraitOne.text = "Activator Drone" + '\n' +
                        "Passively attacks enemies, or can receive a target by aiming. Attacks trigger weapon passives.";
                }

                if (fcTwoStr == "!")
                {
                    cheatTraitTwo.text = "The Most Resplendent" + '\n' +
                            "[E] - Create a Hard Lucent crystal that produces Lucent clusters passively or when shot. Stacks 1x.";
                } //Pay to Win pairing         

                if (fcTwoStr == "4")
                {
                    cheatTraitTwo.text = "Not with a Stick" + '\n' +
                            "Kills with this Weapon increase Effective Range by 30% of max Range until your next reload.";
                } //Social Distance, Please! pairing

                if (fcTwoStr == "#")
                {
                    cheatTraitTwo.text = "Forager" + '\n' +
                            "Weapon or Melee kills produce a burst of Lucent clusters, Health, Shield, and Ammo pickups.";
                } //Absolutely no breaks! Pairing

                if (fcTwoStr == "6")
                {
                    cheatTraitOne.text = "Positive-Negative" + '\n' +
                        "Moving generates a charge. While halfway charged, Enemy hits applies 100% of Weapon damage as damage-over-time for ten seconds.";
                } //Flashpoint pairing

                if (fcTwoStr == "0")
                {
                    cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                           "Kills with this Weapon restore 10% of Shield strength.";
                } //Volant pairing

                if (fcTwoStr == "1")
                {
                    cheatTraitTwo.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";
                } //Early Berth gets the Hearst pairing

                if (fcTwoStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";
                } //Equivalent Exchange pairing

                if (fcTwoStr == "7")
                {
                    cheatTraitTwo.text = "Cadence" + '\n' +
                        "Every third Enemy kill spawns a Lucent cluster.";
                }

                if (fcTwoStr == "3")
                {
                    cheatTraitTwo.text = "Rude Awakening" + '\n' +
                         "Kills grant casts of a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x." + '\n' +
                        "'E' - Cast Blast";
                }

                if (fcTwoStr == "$")
                {
                    cheatTraitTwo.text = "Counterplay" + '\n' +
                    "Hits taken while immune during Evasions casts two Lucent clusters and permanently increases Weapon damage by 10%. Stacks 3x.";
                } //Superweapon pairing

                if (fcTwoStr == "%")
                {
                    cheatTraitTwo.text = "Enshroud" + '\n' +
                    "Enemy hits increase Melee range by 15%, up to 200%, for seven seconds. Melee kills cast a Fogger Grenade. Cooldown: 12 seconds.";
                }

                if (fcTwoStr == "^")
                {
                    cheatTraitTwo.text = "Gale Force Winds" + '\n' +
                    "Cast traveling winds from Sprinting or moving that applies Health and Slowed debuffs to Enemies." + '\n' +
                    "'[E]' - Toggle cast";
                }
            }
        }

        item.transform.parent = playerInventory.gameObject.transform;

        purchaseConfirmText.text = "Trade your Lucent for this Weapon?" + "\n" +
                                    warePrices[q].ToString("N0") + " [You have: " + lucentFunds.ToString("N0") + "]";
    } 

    public void PurchaseInquiry()
    {
        if(lucentFunds >= warePrices[potentialBuy])
        {
            lucentFunds -= (int)warePrices[potentialBuy];

            if (playerInventory.observedWeps.Count <= 0)
            {
                playerInventory.ObserveOnLoad();
                playerInventory.observedWeps.Add(kioskWares[potentialBuy]);
                playerInventory.WriteInventory();

            }

            else
            {
                playerInventory.observedWeps.Add(kioskWares[potentialBuy]);
                playerInventory.WriteInventory();

            }
        }       
    }

    /// <summary>
    /// Reads, appraises Player's Inventory if they have at least one Weapon
    /// </summary>
    public void AppraiseInquiry()
    {
        if (playerInventory.observedWeps.Count <= 0)
        {
            playerInventory.ObserveOnLoad();
            PriceInventory();
        }

        else
        {
            PriceInventory();
        }
    }

    /// <summary>
    /// Resets Weapon inspection page to top position
    /// </summary>
    public void ResetInspectPage()
    {
        inspectScroll.verticalNormalizedPosition = 1f;
    }

    /// <summary>
    /// Allows/prevents Kiosk items from being inspected based on Player Lucent funds
    /// </summary>
    public void CheckForInsufficientLucent()
    {
        for(int l = 0; l < wareOptions.Count; l++)
        {
            if(lucentFunds < warePrices[l])
            {
                wareOptions[l].GetComponent<Button>().interactable = false;
            }

            else
            {
                if (wareOptions[l].GetComponent<Button>().interactable == false)
                {
                    wareOptions[l].GetComponent<Button>().interactable = true;
                }

                else
                {
                    wareOptions[l].GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    public void SaveLucentBalance()
    {
        PlayerPrefs.SetInt("lucentBalance", lucentFunds);
    }

    /// <summary>
    /// Retrieves Player's current Lucent balance
    /// </summary>
    public void AssignLucentBalance()
    {
        lucentFunds = PlayerPrefs.GetInt("lucentBalance");
        if(lucentFunds >= 100000)
        {
            lucentFunds = 100000;
        }
    }

    public void PauseRefreshTimer()
    {
        paused = true;
    }

    public void ResumeRefreshTimer()
    {
        paused = false;
    }
}