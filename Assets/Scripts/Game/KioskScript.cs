using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class KioskScript : MonoBehaviour
{
    public int lucentFunds = 0;
    public float devaluePercent = 20f;
    public Text lucentText;
    public Text purchaseConfirmText;
    public WeaponManagerScript playerInventory;
    public string filepath = "inventory.txt";

    public List<string> kioskWares = new List<string>();
    public List<float> warePrices = new List<float>();
    public List<float> inventoryWorth = new List<float>();
    public List<Button> wareOptions = new List<Button>();

    public List<Text> prices = new List<Text>();
    public List<Text> names = new List<Text>();
    public List<Text> rarities = new List<Text>();
    public List<Text> statOnes = new List<Text>();
    public List<Text> statTwos = new List<Text>();
    public List<Text> statThrees = new List<Text>();
    public List<Text> statFours = new List<Text>();
    public List<Text> functionOnes = new List<Text>();
    public List<Text> functionTwos = new List<Text>();

    public Text wepName, flavor, rarityCheck, stats;
    public Text cheatOne, cheatTwo, cheatThree, cheatFour, cheatTraitOne, cheatTraitTwo;
    public ScrollRect inspectScroll;

    private string weaponIdentity;
    private float weaponPrice;
    private int potentialBuy;
    private ColorBlock quality;
    private int devalue;
    private float devaluePctReset;

    private int determinate;
    private string wepTypeStr, wepRarStr, wepExoStr, stOneStr, stTwoStr, stThreeStr, stFourStr, fcOneStr, fcTwoStr;
    private char wepType, wepRar, wepExo, stOne, stTwo, stThree, stFour, fcOne, fcTwo;
    // Start is called before the first frame update
    void Start()
    {
        devaluePctReset = devaluePercent;
        GenerateWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        lucentText.text = "Lucent: " + lucentFunds.ToString("N0");
    }

    void GenerateWeapons()
    {
        for (int w = 0; w < kioskWares.Count; w++)
        {
            //Determines Weapon Type
            determinate = Random.Range(1, 8);
            wepTypeStr = determinate.ToString();

            //Determines Weapon Rarity -- capped to Rarity 4 if Difficulty 5 is not unlocked
            //When Difficulty 5 unlocks, Weapons starting from Rarity 3 will be sellable (speculative)
            //if(PlayerPrefs.GetInt("unlockDifficulty5") != 1)
            //{
            //    determinate = Random.Range(1, 5);
            //}

            //else
            //{
            //    determinate = Random.Range(1, 6);
            //}

            determinate = Random.Range(1, 6);

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

            //Determines Statistical Cheat values -- automatically set to 2,4,6,8 if Weapon is Exotic
            if(wepRarStr == "5" && wepExoStr == "1")
            {
                stOneStr = "2";
                stTwoStr = "4";
                stThreeStr = "6";
                stFourStr = "8";
            }

            else
            {
                determinate = Random.Range(1, 3);
                stOneStr = determinate.ToString();

                determinate = Random.Range(3, 5);
                stTwoStr = determinate.ToString();

                determinate = Random.Range(5, 7);
                stThreeStr = determinate.ToString();

                determinate = Random.Range(7, 9);
                stFourStr = determinate.ToString();
            }
            
            //If Rarity is 1, complete string assembly
            if(wepRarStr == "1")
            {
                weaponIdentity = wepTypeStr + wepRarStr + wepExoStr +
                    stOneStr + stTwoStr + stThreeStr + stFourStr;
            }

            //If Rarity is 2 or 3, complete string assembly
            else if (wepRarStr == "2" || wepRarStr == "3")
            {
                determinate = Random.Range(0, 10);
                fcOneStr = determinate.ToString();

                weaponIdentity = wepTypeStr + wepRarStr + wepExoStr +
                    stOneStr + stTwoStr + stThreeStr + stFourStr +
                    fcOneStr;
            }

            //If Rarity is 4, complete string assembly
            else if (wepRarStr == "4")
            {
                int choice = 0;

                int[] poolOne = {9, 4, 5, 6, 8};
                choice = Random.Range(0, poolOne.Length);
                determinate = poolOne[choice];                
                fcOneStr = determinate.ToString();

                int[] poolTwo = {0, 1, 2, 7, 3};
                choice = Random.Range(0, poolOne.Length);
                determinate = poolTwo[choice];
                fcTwoStr = determinate.ToString();

                weaponIdentity = wepTypeStr + wepRarStr + wepExoStr +
                    stOneStr + stTwoStr + stThreeStr + stFourStr +
                    fcOneStr + fcTwoStr;
            }

            //If Rarity 5 (Exotic), complete string assembly based on Weapon type
            else if (wepRarStr == "5" && wepExoStr == "1")
            {
                if(wepTypeStr == "1")
                {
                    fcOneStr = "A";
                    fcTwoStr = "0";
                }

                if (wepTypeStr == "2")
                {
                    fcOneStr = "G";
                    fcTwoStr = "5";
                }

                if (wepTypeStr == "3")
                {
                    fcOneStr = "C";
                    fcTwoStr = "6";
                }

                if (wepTypeStr == "4")
                {
                    fcOneStr = "F";
                    fcTwoStr = "2";
                }

                if (wepTypeStr == "5")
                {
                    fcOneStr = "D";
                    fcTwoStr = "4";
                }

                if (wepTypeStr == "6")
                {
                    fcOneStr = "E";
                    fcTwoStr = "1";
                }

                if (wepTypeStr == "7")
                {
                    fcOneStr = "B";
                    fcTwoStr = "8";
                }

                weaponIdentity = wepTypeStr + wepRarStr + wepExoStr +
                    stOneStr + stTwoStr + stThreeStr + stFourStr +
                    fcOneStr + fcTwoStr;
            }

            kioskWares[w] = weaponIdentity;

        }

        PriceWeapons();

    }

    //FFR - 300; MG - 600; Pistol - 100; SeFR - 500; SG - 700; SiFR - 400; SMG - 200
    void PriceWeapons()
    {
        string s = "Comic Sans";

        for (int p = 0; p < kioskWares.Count; p++)
        {
            quality = wareOptions[p].GetComponent<Button>().colors;
            quality.pressedColor = Color.gray;
            quality.disabledColor = Color.black;

            s = kioskWares[p];

            wepType = s[0];
            wepTypeStr = wepType.ToString();

            wepRar = s[1];
            wepRarStr = wepRar.ToString();

            wepExo = s[2];
            wepExoStr = wepExo.ToString();

            stOne = s[3];
            stOneStr = stOne.ToString();

            stTwo = s[4];
            stTwoStr = stTwo.ToString();

            stThree = s[5];
            stThreeStr = stThree.ToString();

            stFour = s[6];
            stFourStr = stFour.ToString();

            if (kioskWares[p].Length == 8)
            {
                fcOne = s[7];
                fcOneStr = fcOne.ToString();
            }

            if (kioskWares[p].Length == 9)
            {
                fcOne = s[7];
                fcOneStr = fcOne.ToString();

                fcTwo = s[8];
                fcTwoStr = fcTwo.ToString();
            }

            if (wepTypeStr == "1")
            {
                weaponPrice = 300f;
                names[p].text = "Full Fire Rifle";

                if (wepRarStr == "1")
                {
                    rarities[p].text = "Usual";
                    functionOnes[p].text = " ";
                    functionTwos[p].text = " ";
                    quality.normalColor = Color.white;
                    quality.highlightedColor = Color.white;

                }

                if (wepRarStr == "2")
                {
                    weaponPrice *= 2;
                    rarities[p].text = "Sought";
                    quality.normalColor = Color.green;
                    quality.highlightedColor = Color.green;

                }

                if (wepRarStr == "3")
                {
                    weaponPrice *= 3;
                    rarities[p].text = "Coveted";
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
                    }

                    else
                    {
                        weaponPrice *= 5;
                        rarities[p].text = "Fated"; //Fated-rarity non-Exotic Weapons shouldn't appear, but the case is caught here anyway.
                    }
                }

                wareOptions[p].GetComponent<Button>().colors = quality;

                if (stOneStr == "1")
                {
                    weaponPrice += 1000f;
                    statOnes[p].text = "+MAG";
                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;
                    statOnes[p].text = "++MAG";
                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;
                    statTwos[p].text = "+RES";
                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;
                    statTwos[p].text = "++RES";
                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;
                    statThrees[p].text = "+EFR";
                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;
                    statThrees[p].text = "++EFR";
                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;
                    statFours[p].text = "+RLD";
                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;
                    statFours[p].text = "++RLD";
                }

                if (kioskWares[p].Length == 8)
                {
                    weaponPrice += 3000f;

                    if(fcOneStr == "0")
                    {
                        functionOnes[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcOneStr == "1")
                    {
                        functionOnes[p].text = "Efficacy";

                    }

                    if (fcOneStr == "2")
                    {
                        functionOnes[p].text = "Inoculated";

                    }

                    if (fcOneStr == "3")
                    {
                        functionOnes[p].text = "Rude Awakening";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "7")
                    {
                        functionOnes[p].text = "Cadence";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    functionTwos[p].text = " ";
                }

                if (kioskWares[p].Length == 9)
                {
                    weaponPrice += 6000f;

                    if (fcOneStr == "A")
                    {
                        functionOnes[p].text = "Equivalent Exchange";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }

                    if (fcTwoStr == "0")
                    {
                        functionTwos[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcTwoStr == "1")
                    {
                        functionTwos[p].text = "Efficacy";

                    }

                    if (fcTwoStr == "2")
                    {
                        functionTwos[p].text = "Inoculated";

                    }

                    if (fcTwoStr == "7")
                    {
                        functionTwos[p].text = "Cadence";

                    }

                    if (fcTwoStr == "3")
                    {
                        functionTwos[p].text = "Rude Awakening";

                    }
                }

                warePrices[p] = weaponPrice;
                prices[p].text = weaponPrice.ToString("N0");
            }

            if (wepTypeStr == "2")
            {
                weaponPrice = 600f;
                names[p].text = "Machine Gun";

                if (wepRarStr == "1")
                {
                    rarities[p].text = "Usual";
                    functionOnes[p].text = " ";
                    functionTwos[p].text = " ";
                    quality.normalColor = Color.white;
                    quality.highlightedColor = Color.white;

                }

                if (wepRarStr == "2")
                {
                    weaponPrice *= 2;
                    rarities[p].text = "Sought";
                    quality.normalColor = Color.green;
                    quality.highlightedColor = Color.green;

                }

                if (wepRarStr == "3")
                {
                    weaponPrice *= 3;
                    rarities[p].text = "Coveted";
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
                    }

                    else
                    {
                        weaponPrice *= 5;
                        rarities[p].text = "Fated"; //Fated-rarity non-Exotic Weapons shouldn't appear, but the case is caught here anyway.
                    }
                }

                wareOptions[p].GetComponent<Button>().colors = quality;

                if (stOneStr == "1")
                {
                    weaponPrice += 1000f;
                    statOnes[p].text = "+MAG";
                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;
                    statOnes[p].text = "++MAG";
                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;
                    statTwos[p].text = "+RES";
                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;
                    statTwos[p].text = "++RES";
                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;
                    statThrees[p].text = "+EFR";
                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;
                    statThrees[p].text = "++EFR";
                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;
                    statFours[p].text = "+RLD";
                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;
                    statFours[p].text = "++RLD";
                }

                if (kioskWares[p].Length == 8)
                {
                    weaponPrice += 3000f;

                    if (fcOneStr == "0")
                    {
                        functionOnes[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcOneStr == "1")
                    {
                        functionOnes[p].text = "Efficacy";

                    }

                    if (fcOneStr == "2")
                    {
                        functionOnes[p].text = "Inoculated";

                    }

                    if (fcOneStr == "3")
                    {
                        functionOnes[p].text = "Rude Awakening";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "7")
                    {
                        functionOnes[p].text = "Cadence";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    functionTwos[p].text = " ";
                }

                if (kioskWares[p].Length == 9)
                {
                    weaponPrice += 6000f;

                    if (fcOneStr == "G")
                    {
                        functionOnes[p].text = "Pay to Win";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }

                    if (fcTwoStr == "5") //Pay to Win pairing
                    {
                        functionTwos[p].text = "Malicious Wind-up";

                    }

                    if (fcTwoStr == "0")
                    {
                        functionTwos[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcTwoStr == "1")
                    {
                        functionTwos[p].text = "Efficacy";

                    }

                    if (fcTwoStr == "2")
                    {
                        functionTwos[p].text = "Inoculated";

                    }

                    if (fcTwoStr == "7")
                    {
                        functionTwos[p].text = "Cadence";

                    }

                    if (fcTwoStr == "3")
                    {
                        functionTwos[p].text = "Rude Awakening";

                    }
                }

                warePrices[p] = weaponPrice;
                prices[p].text = weaponPrice.ToString("N0");
            }

            if (wepTypeStr == "3")
            {
                weaponPrice = 100f;
                names[p].text = "Pistol";

                if (wepRarStr == "1")
                {
                    rarities[p].text = "Usual";
                    functionOnes[p].text = " ";
                    functionTwos[p].text = " ";
                    quality.normalColor = Color.white;
                    quality.highlightedColor = Color.white;

                }

                if (wepRarStr == "2")
                {
                    weaponPrice *= 2;
                    rarities[p].text = "Sought";
                    quality.normalColor = Color.green;
                    quality.highlightedColor = Color.green;

                }

                if (wepRarStr == "3")
                {
                    weaponPrice *= 3;
                    rarities[p].text = "Coveted";
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
                    }

                    else
                    {
                        weaponPrice *= 5;
                        rarities[p].text = "Fated"; //Fated-rarity non-Exotic Weapons shouldn't appear, but the case is caught here anyway.
                    }
                }

                wareOptions[p].GetComponent<Button>().colors = quality;

                if (stOneStr == "1")
                {
                    weaponPrice += 1000f;
                    statOnes[p].text = "+MAG";
                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;
                    statOnes[p].text = "++MAG";
                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;
                    statTwos[p].text = "+RES";
                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;
                    statTwos[p].text = "++RES";
                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;
                    statThrees[p].text = "+EFR";
                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;
                    statThrees[p].text = "++EFR";
                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;
                    statFours[p].text = "+RLD";
                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;
                    statFours[p].text = "++RLD";
                }

                if (kioskWares[p].Length == 8)
                {
                    weaponPrice += 3000f;

                    if (fcOneStr == "0")
                    {
                        functionOnes[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcOneStr == "1")
                    {
                        functionOnes[p].text = "Efficacy";

                    }

                    if (fcOneStr == "2")
                    {
                        functionOnes[p].text = "Inoculated";

                    }

                    if (fcOneStr == "3")
                    {
                        functionOnes[p].text = "Rude Awakening";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "7")
                    {
                        functionOnes[p].text = "Cadence";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    functionTwos[p].text = " ";
                }

                if (kioskWares[p].Length == 9)
                {
                    weaponPrice += 6000f;

                    if (fcOneStr == "C")
                    {
                        functionOnes[p].text = "Shelter in Place";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }

                    if (fcTwoStr == "6") //Shelter in Place pairing
                    {
                        functionTwos[p].text = "Positive-Negative";

                    }

                    if (fcTwoStr == "0")
                    {
                        functionTwos[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcTwoStr == "1")
                    {
                        functionTwos[p].text = "Efficacy";

                    }

                    if (fcTwoStr == "2")
                    {
                        functionTwos[p].text = "Inoculated";

                    }

                    if (fcTwoStr == "7")
                    {
                        functionTwos[p].text = "Cadence";

                    }

                    if (fcTwoStr == "3")
                    {
                        functionTwos[p].text = "Rude Awakening";

                    }
                }

                warePrices[p] = weaponPrice;
                prices[p].text = weaponPrice.ToString("N0");
            }

            if (wepTypeStr == "4")
            {
                weaponPrice = 500f;
                names[p].text = "Semi Fire Rifle";

                if (wepRarStr == "1")
                {
                    rarities[p].text = "Usual";
                    functionOnes[p].text = " ";
                    functionTwos[p].text = " ";
                    quality.normalColor = Color.white;
                    quality.highlightedColor = Color.white;

                }

                if (wepRarStr == "2")
                {
                    weaponPrice *= 2;
                    rarities[p].text = "Sought";
                    quality.normalColor = Color.green;
                    quality.highlightedColor = Color.green;

                }

                if (wepRarStr == "3")
                {
                    weaponPrice *= 3;
                    rarities[p].text = "Coveted";
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
                    }

                    else
                    {
                        weaponPrice *= 5;
                        rarities[p].text = "Fated"; //Fated-rarity non-Exotic Weapons shouldn't appear, but the case is caught here anyway.
                    }
                }

                wareOptions[p].GetComponent<Button>().colors = quality;

                if (stOneStr == "1")
                {
                    weaponPrice += 1000f;
                    statOnes[p].text = "+MAG";
                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;
                    statOnes[p].text = "++MAG";
                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;
                    statTwos[p].text = "+RES";
                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;
                    statTwos[p].text = "++RES";
                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;
                    statThrees[p].text = "+EFR";
                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;
                    statThrees[p].text = "++EFR";
                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;
                    statFours[p].text = "+RLD";
                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;
                    statFours[p].text = "++RLD";
                }

                if (kioskWares[p].Length == 8)
                {
                    weaponPrice += 3000f;

                    if (fcOneStr == "0")
                    {
                        functionOnes[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcOneStr == "1")
                    {
                        functionOnes[p].text = "Efficacy";

                    }

                    if (fcOneStr == "2")
                    {
                        functionOnes[p].text = "Inoculated";

                    }

                    if (fcOneStr == "3")
                    {
                        functionOnes[p].text = "Rude Awakening";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "7")
                    {
                        functionOnes[p].text = "Cadence";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    functionTwos[p].text = " ";
                }

                if (kioskWares[p].Length == 9)
                {
                    weaponPrice += 6000f;

                    if (fcOneStr == "F")
                    {
                        functionOnes[p].text = "Off your own supply";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }                  

                    if (fcTwoStr == "0")
                    {
                        functionTwos[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcTwoStr == "1")
                    {
                        functionTwos[p].text = "Efficacy";

                    }

                    if (fcTwoStr == "2")
                    {
                        functionTwos[p].text = "Inoculated";

                    }

                    if (fcTwoStr == "7")
                    {
                        functionTwos[p].text = "Cadence";

                    }

                    if (fcTwoStr == "3")
                    {
                        functionTwos[p].text = "Rude Awakening";

                    }
                }

                warePrices[p] = weaponPrice;
                prices[p].text = weaponPrice.ToString("N0");
            }

            if (wepTypeStr == "5")
            {
                weaponPrice = 700f;
                names[p].text = "Shotgun";

                if (wepRarStr == "1")
                {
                    rarities[p].text = "Usual";
                    functionOnes[p].text = " ";
                    functionTwos[p].text = " ";
                    quality.normalColor = Color.white;
                    quality.highlightedColor = Color.white;

                }

                if (wepRarStr == "2")
                {
                    weaponPrice *= 2;
                    rarities[p].text = "Sought";
                    quality.normalColor = Color.green;
                    quality.highlightedColor = Color.green;

                }

                if (wepRarStr == "3")
                {
                    weaponPrice *= 3;
                    rarities[p].text = "Coveted";
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
                    }

                    else
                    {
                        weaponPrice *= 5;
                        rarities[p].text = "Fated"; //Fated-rarity non-Exotic Weapons shouldn't appear, but the case is caught here anyway.
                    }
                }

                wareOptions[p].GetComponent<Button>().colors = quality;

                if (stOneStr == "1")
                {
                    weaponPrice += 1000f;
                    statOnes[p].text = "+MAG";
                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;
                    statOnes[p].text = "++MAG";
                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;
                    statTwos[p].text = "+RES";
                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;
                    statTwos[p].text = "++RES";
                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;
                    statThrees[p].text = "+EFR";
                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;
                    statThrees[p].text = "++EFR";
                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;
                    statFours[p].text = "+RLD";
                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;
                    statFours[p].text = "++RLD";
                }

                if (kioskWares[p].Length == 8)
                {
                    weaponPrice += 3000f;

                    if (fcOneStr == "0")
                    {
                        functionOnes[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcOneStr == "1")
                    {
                        functionOnes[p].text = "Efficacy";

                    }

                    if (fcOneStr == "2")
                    {
                        functionOnes[p].text = "Inoculated";

                    }

                    if (fcOneStr == "3")
                    {
                        functionOnes[p].text = "Rude Awakening";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "7")
                    {
                        functionOnes[p].text = "Cadence";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    functionTwos[p].text = " ";
                }

                if (kioskWares[p].Length == 9)
                {
                    weaponPrice += 6000f;

                    if (fcOneStr == "D")
                    {
                        functionOnes[p].text = "Social Distance, Please!";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }

                    if (fcTwoStr == "4") //Social Distance, Please! pairing
                    {
                        functionTwos[p].text = "Not with a Stick";

                    }

                    if (fcTwoStr == "0")
                    {
                        functionTwos[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcTwoStr == "1")
                    {
                        functionTwos[p].text = "Efficacy";

                    }

                    if (fcTwoStr == "2")
                    {
                        functionTwos[p].text = "Inoculated";

                    }

                    if (fcTwoStr == "7")
                    {
                        functionTwos[p].text = "Cadence";

                    }

                    if (fcTwoStr == "3")
                    {
                        functionTwos[p].text = "Rude Awakening";

                    }
                }

                warePrices[p] = weaponPrice;
                prices[p].text = weaponPrice.ToString("N0");
            }

            if (wepTypeStr == "6")
            {
                weaponPrice = 400f;
                names[p].text = "Single Fire Rifle";

                if (wepRarStr == "1")
                {
                    rarities[p].text = "Usual";
                    functionOnes[p].text = " ";
                    functionTwos[p].text = " ";
                    quality.normalColor = Color.white;
                    quality.highlightedColor = Color.white;

                }

                if (wepRarStr == "2")
                {
                    weaponPrice *= 2;
                    rarities[p].text = "Sought";
                    quality.normalColor = Color.green;
                    quality.highlightedColor = Color.green;

                }

                if (wepRarStr == "3")
                {
                    weaponPrice *= 3;
                    rarities[p].text = "Coveted";
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
                    }

                    else
                    {
                        weaponPrice *= 5;
                        rarities[p].text = "Fated"; //Fated-rarity non-Exotic Weapons shouldn't appear, but the case is caught here anyway.
                    }
                }

                wareOptions[p].GetComponent<Button>().colors = quality;

                if (stOneStr == "1")
                {
                    weaponPrice += 1000f;
                    statOnes[p].text = "+MAG";
                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;
                    statOnes[p].text = "++MAG";
                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;
                    statTwos[p].text = "+RES";
                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;
                    statTwos[p].text = "++RES";
                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;
                    statThrees[p].text = "+EFR";
                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;
                    statThrees[p].text = "++EFR";
                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;
                    statFours[p].text = "+RLD";
                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;
                    statFours[p].text = "++RLD";
                }

                if (kioskWares[p].Length == 8)
                {
                    weaponPrice += 3000f;

                    if (fcOneStr == "0")
                    {
                        functionOnes[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcOneStr == "1")
                    {
                        functionOnes[p].text = "Efficacy";

                    }

                    if (fcOneStr == "2")
                    {
                        functionOnes[p].text = "Inoculated";

                    }

                    if (fcOneStr == "3")
                    {
                        functionOnes[p].text = "Rude Awakening";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "7")
                    {
                        functionOnes[p].text = "Cadence";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    functionTwos[p].text = " ";
                }

                if (kioskWares[p].Length == 9)
                {
                    weaponPrice += 6000f;

                    if (fcOneStr == "E")
                    {
                        functionOnes[p].text = "Early Berth gets the Hearst";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }                 

                    if (fcTwoStr == "0")
                    {
                        functionTwos[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcTwoStr == "1")
                    {
                        functionTwos[p].text = "Efficacy";

                    }

                    if (fcTwoStr == "2")
                    {
                        functionTwos[p].text = "Inoculated";

                    }

                    if (fcTwoStr == "7")
                    {
                        functionTwos[p].text = "Cadence";

                    }

                    if (fcTwoStr == "3")
                    {
                        functionTwos[p].text = "Rude Awakening";

                    }
                }

                warePrices[p] = weaponPrice;
                prices[p].text = weaponPrice.ToString("N0");
            }

            if (wepTypeStr == "7")
            {
                weaponPrice = 200f;
                names[p].text = "Submachine Gun";

                if (wepRarStr == "1")
                {
                    rarities[p].text = "Usual";
                    functionOnes[p].text = " ";
                    functionTwos[p].text = " ";
                    quality.normalColor = Color.white;
                    quality.highlightedColor = Color.white
;

                }

                if (wepRarStr == "2")
                {
                    weaponPrice *= 2;
                    rarities[p].text = "Sought";
                    quality.normalColor = Color.green;
                    quality.highlightedColor = Color.green;

                }

                if (wepRarStr == "3")
                {
                    weaponPrice *= 3;
                    rarities[p].text = "Coveted";
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
                    }

                    else
                    {
                        weaponPrice *= 5;
                        rarities[p].text = "Fated"; //Fated-rarity non-Exotic Weapons shouldn't appear, but the case is caught here anyway.
                    }
                }

                wareOptions[p].GetComponent<Button>().colors = quality;

                if (stOneStr == "1")
                {
                    weaponPrice += 1000f;
                    statOnes[p].text = "+MAG";
                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;
                    statOnes[p].text = "++MAG";
                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;
                    statTwos[p].text = "+RES";
                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;
                    statTwos[p].text = "++RES";
                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;
                    statThrees[p].text = "+EFR";
                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;
                    statThrees[p].text = "++EFR";
                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;
                    statFours[p].text = "+RLD";
                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;
                    statFours[p].text = "++RLD";
                }

                if (kioskWares[p].Length == 8)
                {
                    weaponPrice += 3000f;

                    if (fcOneStr == "0")
                    {
                        functionOnes[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcOneStr == "1")
                    {
                        functionOnes[p].text = "Efficacy";

                    }

                    if (fcOneStr == "2")
                    {
                        functionOnes[p].text = "Inoculated";

                    }

                    if (fcOneStr == "3")
                    {
                        functionOnes[p].text = "Rude Awakening";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "7")
                    {
                        functionOnes[p].text = "Cadence";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    functionTwos[p].text = " ";
                }

                if (kioskWares[p].Length == 9)
                {
                    weaponPrice += 6000f;

                    if (fcOneStr == "B")
                    {
                        functionOnes[p].text = "Absolutely no Stops!";

                    }

                    if (fcOneStr == "9")
                    {
                        functionOnes[p].text = "All Else Fails";

                    }

                    if (fcOneStr == "4")
                    {
                        functionOnes[p].text = "Not with a Stick";

                    }

                    if (fcOneStr == "5")
                    {
                        functionOnes[p].text = "Malicious Wind-up";

                    }

                    if (fcOneStr == "6")
                    {
                        functionOnes[p].text = "Positive-Negative";

                    }

                    if (fcOneStr == "8")
                    {
                        functionOnes[p].text = "Good Things Come";

                    }

                    if (fcTwoStr == "8") //Absolutely no Breaks! pairing
                    {
                        functionTwos[p].text = "Good Things Come";

                    }

                    if (fcTwoStr == "0")
                    {
                        functionTwos[p].text = "Wait! Now I'm Ready!";
                    }

                    if (fcTwoStr == "1")
                    {
                        functionTwos[p].text = "Efficacy";

                    }

                    if (fcTwoStr == "2")
                    {
                        functionTwos[p].text = "Inoculated";

                    }

                    if (fcTwoStr == "7")
                    {
                        functionTwos[p].text = "Cadence";

                    }

                    if (fcTwoStr == "3")
                    {
                        functionTwos[p].text = "Rude Awakening";

                    }
                }

                warePrices[p] = weaponPrice;
                prices[p].text = weaponPrice.ToString("N0");
            }

        }
    } //This method gives prices to the Kiosk's personal Weapon selection

    //FFR - 300; MG - 600; Pistol - 100; SeFR - 500; SG - 700; SiFR - 400; SMG - 200
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

            wepType = s[0];
            wepTypeStr = wepType.ToString();

            wepRar = s[1];
            wepRarStr = wepRar.ToString();

            wepExo = s[2];
            wepExoStr = wepExo.ToString();

            stOne = s[3];
            stOneStr = stOne.ToString();

            stTwo = s[4];
            stTwoStr = stTwo.ToString();

            stThree = s[5];
            stThreeStr = stThree.ToString();

            stFour = s[6];
            stFourStr = stFour.ToString();

            if (playerInventory.observedWeps[p].Length == 8)
            {
                fcOne = s[7];
                fcOneStr = fcOne.ToString();
            }

            if (playerInventory.observedWeps[p].Length == 9)
            {
                fcOne = s[7];
                fcOneStr = fcOne.ToString();

                fcTwo = s[8];
                fcTwoStr = fcTwo.ToString();
            }

            if (wepTypeStr == "1")
            {
                weaponPrice = 300f;

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
                    weaponPrice += 1000f;

                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;

                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;

                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;

                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;

                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;

                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;

                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;

                }

                if (playerInventory.observedWeps[p].Length == 8)
                {
                    weaponPrice += 3000f;                 
                }

                if (playerInventory.observedWeps[p].Length == 9)
                {
                    weaponPrice += 6000f;                 
                }

                devaluePercent /= 100;
                devaluePercent *= weaponPrice;
                devalue = (int)devaluePercent;
                weaponPrice -= devalue;

                devaluePercent = devaluePctReset;

                inventoryWorth.Add(weaponPrice);

            }

            if (wepTypeStr == "2")
            {
                weaponPrice = 600f;

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
                    weaponPrice += 1000f;

                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;

                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;

                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;

                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;

                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;

                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;

                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;

                }

                if (playerInventory.observedWeps[p].Length == 8)
                {
                    weaponPrice += 3000f;
                }

                if (playerInventory.observedWeps[p].Length == 9)
                {
                    weaponPrice += 6000f;
                }

                devaluePercent /= 100;
                devaluePercent *= weaponPrice;
                devalue = (int)devaluePercent;
                weaponPrice -= devalue;

                devaluePercent = devaluePctReset;

                inventoryWorth.Add(weaponPrice);

            }

            if (wepTypeStr == "3")
            {
                weaponPrice = 100f;

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
                    weaponPrice += 1000f;

                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;

                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;

                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;

                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;

                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;

                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;

                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;

                }

                if (playerInventory.observedWeps[p].Length == 8)
                {
                    weaponPrice += 3000f;
                }

                if (playerInventory.observedWeps[p].Length == 9)
                {
                    weaponPrice += 6000f;
                }

                devaluePercent /= 100;
                devaluePercent *= weaponPrice;
                devalue = (int)devaluePercent;
                weaponPrice -= devalue;

                devaluePercent = devaluePctReset;

                inventoryWorth.Add(weaponPrice);

            }

            if (wepTypeStr == "4")
            {
                weaponPrice = 500f;

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
                    weaponPrice += 1000f;

                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;

                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;

                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;

                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;

                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;

                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;

                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;

                }

                if (playerInventory.observedWeps[p].Length == 8)
                {
                    weaponPrice += 3000f;
                }

                if (playerInventory.observedWeps[p].Length == 9)
                {
                    weaponPrice += 6000f;
                }

                devaluePercent /= 100;
                devaluePercent *= weaponPrice;
                devalue = (int)devaluePercent;
                weaponPrice -= devalue;

                devaluePercent = devaluePctReset;

                inventoryWorth.Add(weaponPrice);

            }

            if (wepTypeStr == "5")
            {
                weaponPrice = 700f;

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
                    weaponPrice += 1000f;

                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;

                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;

                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;

                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;

                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;

                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;

                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;

                }

                if (playerInventory.observedWeps[p].Length == 8)
                {
                    weaponPrice += 3000f;
                }

                if (playerInventory.observedWeps[p].Length == 9)
                {
                    weaponPrice += 6000f;
                }

                devaluePercent /= 100;
                devaluePercent *= weaponPrice;
                devalue = (int)devaluePercent;
                weaponPrice -= devalue;

                devaluePercent = devaluePctReset;

                inventoryWorth.Add(weaponPrice);

            }

            if (wepTypeStr == "6")
            {
                weaponPrice = 400f;

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
                    weaponPrice += 1000f;

                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;

                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;

                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;

                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;

                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;

                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;

                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;

                }

                if (playerInventory.observedWeps[p].Length == 8)
                {
                    weaponPrice += 3000f;
                }

                if (playerInventory.observedWeps[p].Length == 9)
                {
                    weaponPrice += 6000f;
                }

                devaluePercent /= 100;
                devaluePercent *= weaponPrice;
                devalue = (int)devaluePercent;
                weaponPrice -= devalue;

                devaluePercent = devaluePctReset;

                inventoryWorth.Add(weaponPrice);

            }

            if (wepTypeStr == "7")
            {
                weaponPrice = 200f;

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
                    weaponPrice += 1000f;

                }

                if (stOneStr == "2")
                {
                    weaponPrice += 2000f;

                }

                if (stTwoStr == "3")
                {
                    weaponPrice += 1000f;

                }

                if (stTwoStr == "4")
                {
                    weaponPrice += 2000f;

                }

                if (stThreeStr == "5")
                {
                    weaponPrice += 1000f;

                }

                if (stThreeStr == "6")
                {
                    weaponPrice += 2000f;

                }

                if (stFourStr == "7")
                {
                    weaponPrice += 1000f;

                }

                if (stFourStr == "8")
                {
                    weaponPrice += 2000f;

                }

                if (playerInventory.observedWeps[p].Length == 8)
                {
                    weaponPrice += 3000f;
                }

                if (playerInventory.observedWeps[p].Length == 9)
                {
                    weaponPrice += 6000f;
                }

                devaluePercent /= 100;
                devaluePercent *= weaponPrice;
                devalue = (int)devaluePercent;
                weaponPrice -= devalue;

                devaluePercent = devaluePctReset;

                inventoryWorth.Add(weaponPrice);

            }
        }
    } //This method gives prices to a Player's personal Inventory

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

        wepType = c[0];
        wepTypeStr = wepType.ToString();

        wepRar = c[1];
        wepRarStr = wepRar.ToString();

        wepExo = c[2];
        wepExoStr = wepExo.ToString();

        stOne = c[3];
        stOneStr = stOne.ToString();

        stTwo = c[4];
        stTwoStr = stTwo.ToString();

        stThree = c[5];
        stThreeStr = stThree.ToString();

        stFour = c[6];
        stFourStr = stFour.ToString();

        if (kioskWares[q].Length == 8)
        {
            fcOne = c[7];
            fcOneStr = fcOne.ToString();
        }

        if (kioskWares[q].Length == 9)
        {
            fcOne = c[7];
            fcOneStr = fcOne.ToString();

            fcTwo = c[8];
            fcTwoStr = fcTwo.ToString();
        }

        if (wepTypeStr == "1")
        {
            wepName.text = "Full Fire Rifle";
            GameObject item = Instantiate(playerInventory.weapons[0], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[0].name;

            item.transform.parent = playerInventory.gameObject.transform;
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (wepRarStr == "1")
            {
                rarityCheck.text = "Usual";
                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";
            }

            if (wepRarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "5")
            {
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (wepExoStr == "1")
                {
                    rarityCheck.text = "Exotic";
                    item.GetComponent<FirearmScript>().flavorText = "''Such is the law.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }

                else
                {

                    rarityCheck.text = "Fated";
                    item.GetComponent<FirearmScript>().RarityAugment();

                }
            }

            if (stOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (stOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (stTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (stTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (stThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (stThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (stFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (stFourStr == "8")
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

            if (kioskWares[q].Length == 8)
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
                        "'Space' - Cast Blast";

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

                cheatTraitTwo.text = " ";
            }

            if (kioskWares[q].Length == 9)
            {

                if (fcOneStr == "A")
                {
                    cheatTraitOne.text = "Equivalent Exchange" + '\n' +
                        "Taking Enemy damage adds 35% of damage received to this Weapon's base damage and to your Health. Base damage can increase up to 150%.";

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

                if (fcTwoStr == "0")
                {
                    cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                           "Kills with this Weapon restore 10% of Shield strength.";
                }

                if (fcTwoStr == "1")
                {
                    cheatTraitTwo.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";

                }

                if (fcTwoStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";

                }

                if (fcTwoStr == "7")
                {
                    cheatTraitTwo.text = "Cadence" + '\n' +
                        "Every third Enemy kill spawns a Lucent cluster.";

                }

                if (fcTwoStr == "3")
                {
                    cheatTraitTwo.text = "Rude Awakening" + '\n' +
                         "Kills grant casts of a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x." + '\n' +
                        "'Space' - Cast Blast";

                }
            }
        }

        if (wepTypeStr == "2")
        {
            wepName.text = "Machine Gun";
            GameObject item = Instantiate(playerInventory.weapons[1], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[1].name;

            item.transform.parent = playerInventory.gameObject.transform;
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (wepRarStr == "1")
            {
                rarityCheck.text = "Usual";
                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";
            }

            if (wepRarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "5")
            {
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (wepExoStr == "1")
                {
                    rarityCheck.text = "Exotic";
                    item.GetComponent<FirearmScript>().flavorText = "''Only a fool would strike Lucent deals with no exploitable loopholes for themselves. Wealth is found in the hoard.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }

                else
                {

                    rarityCheck.text = "Fated";
                    item.GetComponent<FirearmScript>().RarityAugment();

                }
            }

            if (stOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (stOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (stTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (stTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (stThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (stThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (stFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (stFourStr == "8")
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

            if (kioskWares[q].Length == 8)
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
                        "'Space' - Cast Blast";

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

                cheatTraitTwo.text = " ";
            }

            if (kioskWares[q].Length == 9)
            {

                if (fcOneStr == "G")
                {
                    cheatTraitOne.text = "Pay to Win" + '\n' +
                        "Consume 5,280 Lucent to grant stacks of a 50% Weapon damage increase. Stacks 150x." + "\n" +
                        "'Space' - Consume Lucent";

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

                if (fcTwoStr == "5")
                {
                    cheatTraitTwo.text = "Malicious Wind-Up" + '\n' +
                        "Inflicting Damage increases Reload Speed by 0.75%. This bonus activates on your next reload.";

                } //Pay to Win pairing

                if (fcTwoStr == "0")
                {
                    cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                           "Kills with this Weapon restore 10% of Shield strength.";
                }

                if (fcTwoStr == "1")
                {
                    cheatTraitTwo.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";

                }

                if (fcTwoStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";

                }

                if (fcTwoStr == "7")
                {
                    cheatTraitTwo.text = "Cadence" + '\n' +
                        "Every third Enemy kill spawns a Lucent cluster.";

                }

                if (fcTwoStr == "3")
                {
                    cheatTraitTwo.text = "Rude Awakening" + '\n' +
                         "Kills grant casts of a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x." + '\n' +
                        "'Space' - Cast Blast";

                }
            }
        }

        if (wepTypeStr == "3")
        {
            wepName.text = "Pistol";
            GameObject item = Instantiate(playerInventory.weapons[2], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[2].name;

            item.transform.parent = playerInventory.gameObject.transform;
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (wepRarStr == "1")
            {
                rarityCheck.text = "Usual";
                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";
            }

            if (wepRarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "5")
            {
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (wepExoStr == "1")
                {
                    rarityCheck.text = "Exotic";
                    item.GetComponent<FirearmScript>().flavorText = "''I stand firm in the face of Terror. I am a weed in its terrace. It matters not the Human or the Replevin; I cannot be moved.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }

                else
                {

                    rarityCheck.text = "Fated";
                    item.GetComponent<FirearmScript>().RarityAugment();

                }
            }

            if (stOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (stOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (stTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (stTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (stThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (stThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (stFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (stFourStr == "8")
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

            if (kioskWares[q].Length == 8)
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
                        "'Space' - Cast Blast";

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

                cheatTraitTwo.text = " ";
            }

            if (kioskWares[q].Length == 9)
            {

                if (fcOneStr == "C")
                {
                    cheatTraitOne.text = "Shelter in Place" + '\n' +
                        "Refraining from moving amplifies Weapon damage by 100% and grants 80% damage reduction. Resuming movement ends the bonus.";

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

                if (fcTwoStr == "6")
                {
                    cheatTraitTwo.text = "Positive-Negative" + '\n' +
                        "Moving generates a charge. While halfway charged, Enemy hits applies 100% of Weapon damage as damage-over-time for ten seconds.";

                } //Shelter in Place pairing

                if (fcTwoStr == "0")
                {
                    cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                           "Kills with this Weapon restore 10% of Shield strength.";
                }

                if (fcTwoStr == "1")
                {
                    cheatTraitTwo.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";

                }

                if (fcTwoStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";

                }

                if (fcTwoStr == "7")
                {
                    cheatTraitTwo.text = "Cadence" + '\n' +
                        "Every third Enemy kill spawns a Lucent cluster.";

                }

                if (fcTwoStr == "3")
                {
                    cheatTraitTwo.text = "Rude Awakening" + '\n' +
                         "Kills grant casts of a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x." + '\n' +
                        "'Space' - Cast Blast";

                }
            }
        }

        if (wepTypeStr == "4")
        {
            wepName.text = "Semi Fire Rifle";
            GameObject item = Instantiate(playerInventory.weapons[3], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[3].name;

            item.transform.parent = playerInventory.gameObject.transform;
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (wepRarStr == "1")
            {
                rarityCheck.text = "Usual";
                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";
            }

            if (wepRarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "5")
            {
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (wepExoStr == "1")
                {
                    rarityCheck.text = "Exotic";
                    item.GetComponent<FirearmScript>().flavorText = "WARNING: Persistent use of the cognitive Supercharger may result in cardiac implosion. Proceed? [Y] or [N]";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }

                else
                {

                    rarityCheck.text = "Fated";
                    item.GetComponent<FirearmScript>().RarityAugment();

                }
            }

            if (stOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (stOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (stTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (stTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (stThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (stThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (stFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (stFourStr == "8")
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

            if (kioskWares[q].Length == 8)
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
                        "'Space' - Cast Blast";

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

                cheatTraitTwo.text = " ";
            }

            if (kioskWares[q].Length == 9)
            {

                if (fcOneStr == "F")
                {
                    cheatTraitOne.text = "Off Your Own Supply" + '\n' +
                        "Sacrificing your Shield grants 10% Movement Speed, 80% Reload Speed, 140% Weapon damage, and zero Recoil.";

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

                if (fcTwoStr == "0")
                {
                    cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                           "Kills with this Weapon restore 10% of Shield strength.";
                }

                if (fcTwoStr == "1")
                {
                    cheatTraitTwo.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";

                }

                if (fcTwoStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";

                }

                if (fcTwoStr == "7")
                {
                    cheatTraitTwo.text = "Cadence" + '\n' +
                        "Every third Enemy kill spawns a Lucent cluster.";

                }

                if (fcTwoStr == "3")
                {
                    cheatTraitTwo.text = "Rude Awakening" + '\n' +
                         "Kills grant casts of a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x." + '\n' +
                        "'Space' - Cast Blast";

                }
            }
        }

        if (wepTypeStr == "5")
        {
            wepName.text = "Shotgun";
            GameObject item = Instantiate(playerInventory.weapons[4], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[4].name;

            item.transform.parent = playerInventory.gameObject.transform;
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (wepRarStr == "1")
            {
                rarityCheck.text = "Usual";
                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";
            }

            if (wepRarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "5")
            {
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (wepExoStr == "1")
                {
                    rarityCheck.text = "Exotic";
                    item.GetComponent<FirearmScript>().flavorText = "''Isn't it wonderful when we all do our part?''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }

                else
                {

                    rarityCheck.text = "Fated";
                    item.GetComponent<FirearmScript>().RarityAugment();

                }
            }

            if (stOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (stOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (stTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (stTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (stThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (stThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (stFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (stFourStr == "8")
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

            if (kioskWares[q].Length == 8)
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
                        "'Space' - Cast Blast";

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

                cheatTraitTwo.text = " ";
            }

            if (kioskWares[q].Length == 9)
            {

                if (fcOneStr == "D")
                {
                    cheatTraitOne.text = "Social Distance, please!" + '\n' +
                        "Weapon hits temporarily increase Weapon damage by 30% and adds a Health debuff. Kills spread 400% of Weapon damage to nearby enemies.";

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

                if (fcTwoStr == "4")
                {
                    cheatTraitTwo.text = "Not with a Stick" + '\n' +
                        "Kills with this Weapon increase Effective Range by 30% of max Range until your next reload.";

                } //Social Distance, please! pairing

                if (fcTwoStr == "0")
                {
                    cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                           "Kills with this Weapon restore 10% of Shield strength.";
                }

                if (fcTwoStr == "1")
                {
                    cheatTraitTwo.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";

                }

                if (fcTwoStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";

                }

                if (fcTwoStr == "7")
                {
                    cheatTraitTwo.text = "Cadence" + '\n' +
                        "Every third Enemy kill spawns a Lucent cluster.";

                }

                if (fcTwoStr == "3")
                {
                    cheatTraitTwo.text = "Rude Awakening" + '\n' +
                         "Kills grant casts of a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x." + '\n' +
                        "'Space' - Cast Blast";

                }
            }
        }

        if (wepTypeStr == "6")
        {
            wepName.text = "Single Fire Rifle";
            GameObject item = Instantiate(playerInventory.weapons[5], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[5].name;

            item.transform.parent = playerInventory.gameObject.transform;
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (wepRarStr == "1")
            {
                rarityCheck.text = "Usual";
                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";
            }

            if (wepRarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "5")
            {
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (wepExoStr == "1")
                {
                    rarityCheck.text = "Exotic";
                    item.GetComponent<FirearmScript>().flavorText = "''The Resplendent, for all its igneous light, could never have unveiled the shaded plot. The victims are owed retribution of a thermobaric kind.''";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }

                else
                {

                    rarityCheck.text = "Fated";
                    item.GetComponent<FirearmScript>().RarityAugment();

                }
            }

            if (stOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (stOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (stTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (stTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (stThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (stThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (stFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (stFourStr == "8")
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

            if (kioskWares[q].Length == 8)
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
                        "'Space' - Cast Blast";

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

                cheatTraitTwo.text = " ";
            }

            if (kioskWares[q].Length == 9)
            {

                if (fcOneStr == "E")
                {
                    cheatTraitOne.text = "The Early Berth gets the Hearst" + '\n' +
                        "Enemy hits have a 10% chance to trigger a Berth explosion.";

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
             
                if (fcTwoStr == "0")
                {
                    cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                           "Kills with this Weapon restore 10% of Shield strength.";
                }

                if (fcTwoStr == "1")
                {
                    cheatTraitTwo.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";

                }

                if (fcTwoStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";

                }

                if (fcTwoStr == "7")
                {
                    cheatTraitTwo.text = "Cadence" + '\n' +
                        "Every third Enemy kill spawns a Lucent cluster.";

                }

                if (fcTwoStr == "3")
                {
                    cheatTraitTwo.text = "Rude Awakening" + '\n' +
                         "Kills grant casts of a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x." + '\n' +
                        "'Space' - Cast Blast";

                }
            }
        }

        if (wepTypeStr == "7")
        {
            wepName.text = "Submachine Gun";
            GameObject item = Instantiate(playerInventory.weapons[6], playerInventory.transform.position, playerInventory.transform.rotation);
            item.name = playerInventory.weapons[6].name;

            item.transform.parent = playerInventory.gameObject.transform;
            flavor.text = item.GetComponent<FirearmScript>().flavorText;

            if (wepRarStr == "1")
            {
                rarityCheck.text = "Usual";
                cheatTraitOne.text = " ";
                cheatTraitTwo.text = " ";
            }

            if (wepRarStr == "2")
            {
                rarityCheck.text = "Sought";
                item.GetComponent<FirearmScript>().weaponRarity = 2;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "3")
            {
                rarityCheck.text = "Coveted";
                item.GetComponent<FirearmScript>().weaponRarity = 3;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "4")
            {
                rarityCheck.text = "Treasured";
                item.GetComponent<FirearmScript>().weaponRarity = 4;
                item.GetComponent<FirearmScript>().RarityAugment();
            }

            if (wepRarStr == "5")
            {
                item.GetComponent<FirearmScript>().weaponRarity = 5;

                if (wepExoStr == "1")
                {
                    rarityCheck.text = "Exotic";
                    item.GetComponent<FirearmScript>().flavorText = "Using this Weapon feels like a perpetual Calvary charge. For where you're going, you won't be needing any breaks.";
                    flavor.text = item.GetComponent<FirearmScript>().flavorText;
                    item.GetComponent<FirearmScript>().RarityAugment();
                }

                else
                {

                    rarityCheck.text = "Fated";
                    item.GetComponent<FirearmScript>().RarityAugment();

                }
            }

            if (stOneStr == "1")
            {
                cheatOne.text = "Deep Yield: " + "\n" + "12% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 50;
                item.AddComponent<DeepYield>();
            }

            if (stOneStr == "2")
            {
                cheatOne.text = "Deeper Yield" + "\n" + "24% Magazine Size";

                item.GetComponent<FirearmScript>().ammoCheatOne = 51;
                item.AddComponent<DeeperYield>();
            }

            if (stTwoStr == "3")
            {
                cheatTwo.text = "Deep Stores" + "\n" + "15% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 150;
                item.AddComponent<DeepStores>();
            }

            if (stTwoStr == "4")
            {
                cheatTwo.text = "Deeper Stores" + "\n" + "30% Reserves Size";

                item.GetComponent<FirearmScript>().ammoCheatTwo = 151;
                item.AddComponent<DeeperStores>();
            }

            if (stThreeStr == "5")
            {
                cheatThree.text = "Far Sight" + "\n" + "10% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 250;
                item.AddComponent<FarSight>();
            }

            if (stThreeStr == "6")
            {
                cheatThree.text = "Farther Sight" + "\n" + "20% Effective Range Increase";

                item.GetComponent<FirearmScript>().rangeCheatOne = 251;
                item.AddComponent<FartherSight>();
            }

            if (stFourStr == "7")
            {
                cheatFour.text = "Hasty Hands" + "\n" + "15% Reload Speed Increase";

                item.GetComponent<FirearmScript>().reloadCheatOne = 350;
                item.AddComponent<HastyHands>();

            }

            if (stFourStr == "8")
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

            if (kioskWares[q].Length == 8)
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
                        "'Space' - Cast Blast";

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

                cheatTraitTwo.text = " ";
            }

            if (kioskWares[q].Length == 9)
            {

                if (fcOneStr == "B")
                {
                    cheatTraitOne.text = "Absolutely No Stops" + '\n' +
                        "Expending your magazine fills it from reserves, amplifies Weapon damage by 200%, and increases Rate of Fire by 50%.";

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

                if (fcTwoStr == "8")
                {
                    cheatTraitTwo.text = "Good Things Come" + '\n' +
                        "Being in combat for three seconds grants 25% Movement Speed, 20% damage reduction, and 45% Recoil reduction until you leave combat.";

                } //Absolutely no Stops! pairing

                if (fcTwoStr == "0")
                {
                    cheatTraitTwo.text = "Wait! Now I'm Ready!" + '\n' +
                           "Kills with this Weapon restore 10% of Shield strength.";
                }

                if (fcTwoStr == "1")
                {
                    cheatTraitTwo.text = "Efficacy" + '\n' +
                         "Enemy hits increases this Weapon's base damage by 1%. Reloading resets its base damage.";

                }

                if (fcTwoStr == "2")
                {
                    cheatTraitTwo.text = "Inoculated" + '\n' +
                        "Kills with this Weapon restore 5% of Health.";

                }

                if (fcTwoStr == "7")
                {
                    cheatTraitTwo.text = "Cadence" + '\n' +
                        "Every third Enemy kill spawns a Lucent cluster.";

                }

                if (fcTwoStr == "3")
                {
                    cheatTraitTwo.text = "Rude Awakening" + '\n' +
                         "Kills grant casts of a lethal AOE blast that inflicts 1,000% of Weapon damage. Stacks 3x." + '\n' +
                        "'Space' - Cast Blast";

                }
            }
        }

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

    public void ResetInspectPage()
    {
        inspectScroll.verticalNormalizedPosition = 1f;
    }

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

    public void AssignLucentBalance()
    {
        lucentFunds = PlayerPrefs.GetInt("lucentBalance");
        if(lucentFunds >= 100000)
        {
            lucentFunds = 100000;
        }
    }
}
