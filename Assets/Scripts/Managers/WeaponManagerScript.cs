using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class WeaponManagerScript : MonoBehaviour
{
    public List<GameObject> weapons = new List<GameObject>();

    private PlayerInventoryScript player;
    string wepStr, rarStr, exoStr, cOneStr, cTwoStr, cThreeStr, cFourStr, cFiveStr, cSixStr;
    char wep, rar, exo, cOne, cTwo, cThree, cFour, cFive, cSix;

    void Awake()
    {
        player = FindObjectOfType<PlayerInventoryScript>();
        ReadOnReload();
        RespawnWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void RespawnWeapons()
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
                Debug.Log("Respawning Full Fire Rifle");
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
                        item.GetComponent<FirearmScript>().cheatRNG = 851;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }
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
                        item.GetComponent<FirearmScript>().fcnChtOne = 441;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    //Equivalent Exchange pairing
                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 460;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 491;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }
            }

            if (wepStr == "2")
            {
                Debug.Log("Respawning Machine Gun");
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
                        item.GetComponent<FirearmScript>().cheatRNG = 851;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }
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
                        item.GetComponent<FirearmScript>().fcnChtOne = 441;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    //Pay to Win pairing
                    if (cSixStr == "5")
                    {
                        //item.GetComponent<FirearmScript>().fcnChtOne = 425;
                        item.AddComponent<MaliciousWindUp>();
                        item.GetComponent<MaliciousWindUp>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 460;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 491;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }
            }

            if (wepStr == "3")
            {
                Debug.Log("Respawning Pistol");
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
                        item.GetComponent<FirearmScript>().cheatRNG = 851;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }
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
                        item.GetComponent<FirearmScript>().fcnChtOne = 441;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    //Shelter in Place pairing
                    if (cSixStr == "6")
                    {
                        //item.GetComponent<FirearmScript>().fcnChtOne = 425;
                        item.AddComponent<PositiveNegative>();
                        item.GetComponent<PositiveNegative>().proc = item.GetComponent<FirearmScript>().procTwo;
                    }

                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 460;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 491;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }
            }

            if (wepStr == "4")
            {
                Debug.Log("Respawning Semi Fire Rifle");
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
                        item.GetComponent<FirearmScript>().cheatRNG = 851;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }
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
                        item.GetComponent<FirearmScript>().fcnChtOne = 441;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 460;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    //Off your own Supply pairing
                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 491;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }
            }

            if (wepStr == "5")
            {
                Debug.Log("Respawning Shotgun");
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
                        item.GetComponent<FirearmScript>().cheatRNG = 851;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }
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
                        item.GetComponent<FirearmScript>().fcnChtOne = 441;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    //Social Distance, Please! pairing
                    if (cSixStr == "4")
                    {
                        //item.GetComponent<FirearmScript>().fcnChtOne = 415;
                        item.AddComponent<NotWithAStick>();
                        item.GetComponent<NotWithAStick>().proc = item.GetComponent<FirearmScript>().procTwo;
                    }

                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 460;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 491;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }
            }

            if (wepStr == "6")
            {
                Debug.Log("Respawning Single Fire Rifle");
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
                        item.GetComponent<FirearmScript>().flavorText = "The Resplendence, for all its igneous light, could never have thwarted the terrible plot. Civilians. Servicemembers. Even the Replevin. These victims are owed retribution of a thermobaric kind.";
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
                        item.GetComponent<FirearmScript>().cheatRNG = 851;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }
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
                        item.GetComponent<FirearmScript>().fcnChtOne = 441;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 460;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    //Early Berth gets the Hearst pairing
                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 491;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }
            }

            if (wepStr == "7")
            {
                Debug.Log("Respawning SMG");
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
                        item.GetComponent<FirearmScript>().cheatRNG = 851;
                        item.AddComponent<AllElseFails>();

                        item.GetComponent<AllElseFails>().proc = item.GetComponent<FirearmScript>().procOne;
                        item.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                    }
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
                        item.GetComponent<FirearmScript>().fcnChtOne = 441;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procOne;

                    }

                    //Absolutely no breaks! Pairing
                    if (cSixStr == "8")
                    {
                        item.GetComponent<FirearmScript>().fcnChtOne = 441;
                        item.AddComponent<GoodThingsCome>();
                        item.GetComponent<GoodThingsCome>().proc = item.GetComponent<FirearmScript>().procTwo;
                    }

                    if (cSixStr == "0")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 460;
                        item.AddComponent<WaitNowImReady>();
                        item.GetComponent<WaitNowImReady>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "1")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 465;
                        item.AddComponent<Efficacy>();
                        item.GetComponent<Efficacy>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "2")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 475;
                        item.AddComponent<Inoculated>();
                        item.GetComponent<Inoculated>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "7")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 485;
                        item.AddComponent<Cadence>();
                        item.GetComponent<Cadence>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }

                    if (cSixStr == "3")
                    {
                        item.GetComponent<FirearmScript>().fcnChtTwo = 491;
                        item.AddComponent<RudeAwakening>();
                        item.GetComponent<RudeAwakening>().proc = item.GetComponent<FirearmScript>().procTwo;

                    }
                }
            }
        }
    }
}
