using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootScript : MonoBehaviour
{
    [Header("General")]

    public string lootString;
    public int raritySpawn; //Value used to assign Rarity level to Weapons
    public int focusTarget = -1; //Represent Weapon type focus by number
    public bool exoticDelivery = false;
    public bool debug; //Allows manual trigger of SpawnLoot() if true

    [Header("Loot Delivery Settings")]

    public bool isDrop; //Confirmed as a Delivery item if true
    public List<GameObject> loot = new List<GameObject>(); //List of Weapons to spawn
    public List<GameObject> rarityIdentification = new List<GameObject>();
    public GameObject lootFocusCircle, lootLight, lootModel, lootEffect;
    public ParticleSystem acceptEffect;
    public Canvas collapsedLootInfo;
    public Image collapsedLootImage;
    public Text clpLtType, clpLtRarity, clpLtPlatform,
        clpLtStChtOne, clpLtStChtTwo, clpLtStChtThree, clpLtStChtFour, clpLtFncChtOne, clpLtFncChtTwo;
    public LayerMask contactOnly;

    [Header("Chest Settings")]

    public bool isChest; //Confirmed as a loot chest if true
    public int lootSpamMax = 3; //Repeats spawning behavior up to this value
    public bool spamLoot = false; //Allows repeated Delivery item spawns if true
    public float spawnRate = 0.08f; //Rate of speed for Loot spawns
    //drop - Loot delivery item that spawns standard Weapons
    public GameObject drop;
    public Transform lootSpawn; //Spawn position of Weapons, Delivery items

    
    private float spawnAgain; //Timer used to mediate repeated spawn rate
    private int lootGrant; //Index used to select specific Weapon
    private PlayerMoveScript player;
    private int spawnCount = 0; //Number of times a spawn has occured
    private int determinate; //Number used to randomly select Weapon attributes
    private string wepTypeStr, wepRarStr, wepExoStr, wepFavStr, wepPltStr, stOneStr, stTwoStr, stThreeStr, stFourStr, fcOneStr, fcTwoStr;
    private bool addAsFavorite = false;
    public bool withinVolume = false;
    private GameObject reward;

    // Start is called before the first frame update
    void Start()
    {
        spawnAgain = 0.0f;

        RarityCorrection();

        if (debug)
        {
            if (isChest)
            {
                SpawnLoot();
                return;
            }
        }

        if(isDrop)
        {
            collapsedLootInfo.gameObject.SetActive(false);
            RarityCorrection();
            StartCoroutine(SetupDelivery());       
        }

        player = FindObjectOfType<PlayerMoveScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0) && isChest)
            {
                SpawnLoot();
            }
        }

        if (isChest)
        {
            if (spamLoot)
            {
                //Debug.Log(spawnCount);
                spawnAgain += Time.deltaTime;
                if(spawnAgain >= spawnRate)
                {
                    StartCoroutine(SpawnRepeatedly());
                    spawnCount++;

                    if (spawnCount >= lootSpamMax)
                    {
                        spamLoot = false;
                    }
                }               
            }
        }   

        if(isDrop)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GenerateWeapon();
            }

            if (raritySpawn == 5 && exoticDelivery)
            {
                collapsedLootImage.color = Color.Lerp(Color.cyan, Color.white, Mathf.PingPong(Time.time, 0.9f));
            }

            if (withinVolume)
            {
                if (collapsedLootInfo.gameObject.activeInHierarchy)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        SpawnLoot();
                    }

                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        addAsFavorite = true;
                        SpawnLoot();
                    }
                }
            }
        }        
    }

    /// <summary>
    /// Fixes incorrect rarity assignments
    /// </summary>
    void RarityCorrection()
    {
        if(raritySpawn <= 0)
        {
            raritySpawn = 1;
        }      

        if (raritySpawn >= 6)
        {
            raritySpawn = 5;
        }
    }

    /// <summary>
    /// Spawns Loot delivery items
    /// </summary>
    public void SpawnDrop()
    {
        GameObject reward = Instantiate(drop, lootSpawn.transform.position, lootSpawn.transform.rotation);

        reward.GetComponent<LootScript>().raritySpawn = raritySpawn;
        reward.GetComponent<LootScript>().focusTarget = focusTarget;
        reward.name = drop.name;

        if (player.zeroGravity && player.zgOverride)
        {
            reward.GetComponent<Rigidbody>().useGravity = false;
        }

        //reward.GetComponent<Rigidbody>().AddForce((lootSpawn.transform.forward + lootSpawn.transform.up) * 3f, ForceMode.Impulse);
        reward.GetComponent<Rigidbody>().AddExplosionForce(400f, lootSpawn.transform.position, 10f, 500f);

    }

    /// <summary>
    /// Spawns Loot delivery items
    /// Colors delivery items by rarity
    /// </summary>
    //public void SpawnExotic()
    //{
    //    exoticDrop.GetComponent<LootScript>().raritySpawn = raritySpawn;
    //    GameObject reward = Instantiate(exoticDrop, lootSpawn.transform.position, lootSpawn.transform.rotation);
    //    reward.GetComponent<LootScript>().focusTarget = focusTarget;
    //    reward.name = drop.name;

    //    if (reward.GetComponent<ColorLerpScript>() != null)
    //    {
    //        reward.GetComponent<ColorLerpScript>().colorOne = Color.cyan;
    //        reward.GetComponent<ColorLerpScript>().colorTwo = Color.white;
    //    }

    //    if (player.zeroGravity && player.zgOverride)
    //    {
    //        reward.GetComponent<Rigidbody>().useGravity = false;
    //    }

    //    //reward.GetComponent<Rigidbody>().AddForce((-Vector3.right + Vector3.up) * 3f, ForceMode.Impulse);
    //    reward.GetComponent<Rigidbody>().AddExplosionForce(400f, lootSpawn.transform.position, 10f, 500f);
    //}

    /// <summary>
    /// Spawns Weapon with traits specified by values
    /// </summary>
    public void SpawnLoot()
    {
        if(focusTarget <= -1)
        {
            if(wepTypeStr == "0")
            {
                reward = Instantiate(loot[0], player.transform.position, player.transform.rotation);
                reward.name = loot[0].name;
            }

            else if (wepTypeStr == "1")
            {
                reward = Instantiate(loot[1], player.transform.position, player.transform.rotation);
                reward.name = loot[1].name;
            }

            else if (wepTypeStr == "2")
            {
                reward = Instantiate(loot[2], player.transform.position, player.transform.rotation);
                reward.name = loot[2].name;
            }

            else if (wepTypeStr == "3")
            {
                reward = Instantiate(loot[3], player.transform.position, player.transform.rotation);
                reward.name = loot[3].name;
            }

            else if (wepTypeStr == "4")
            {
                reward = Instantiate(loot[4], player.transform.position, player.transform.rotation);
                reward.name = loot[4].name;
            }

            else if (wepTypeStr == "5")
            {
                reward = Instantiate(loot[5], player.transform.position, player.transform.rotation);
                reward.name = loot[5].name;
            }

            else if (wepTypeStr == "6")
            {
                reward = Instantiate(loot[6], player.transform.position, player.transform.rotation);
                reward.name = loot[6].name;
            }

            else
            {
                reward = Instantiate(loot[7], player.transform.position, player.transform.rotation);
                reward.name = loot[7].name;
            }
                  
        }

        else
        {
            reward = Instantiate(loot[focusTarget], player.transform.position, player.transform.rotation);
            reward.name = loot[focusTarget].name;
        }

        reward.GetComponent<FirearmScript>().weaponRarity = raritySpawn;

        if (exoticDelivery)
        {
            reward.GetComponent<FirearmScript>().isExotic = true;

            if (wepTypeStr == "0")
            {
                reward.name = "Outstanding Warrant";
                reward.GetComponent<FirearmScript>().cheatOverride = -1;
                reward.GetComponent<FirearmScript>().Awake();
                reward.GetComponent<FirearmScript>().flavorText = "''The time has come to collect.'' ";
            }

            else if (wepTypeStr == "1")
            {
                reward.name = "The Dismissal";
                reward.GetComponent<FirearmScript>().cheatOverride = -7;
                reward.GetComponent<FirearmScript>().Awake();
                reward.GetComponent<FirearmScript>().flavorText = "''Wealth is found in the hoard. The Replevin know this; They are territorial, but are otherwise unbothersome.''";
            }

            else if (wepTypeStr == "2")
            {
                reward.name = "Apathetic";
                reward.GetComponent<FirearmScript>().cheatOverride = -3;
                reward.GetComponent<FirearmScript>().Awake();
                reward.GetComponent<FirearmScript>().flavorText = "At what point do the restraints simply fail?";
            }

            else if (wepTypeStr == "3")
            {
                reward.name = "Mercies";
                reward.GetComponent<FirearmScript>().cheatOverride = -6;
                reward.GetComponent<FirearmScript>().Awake();
                reward.GetComponent<FirearmScript>().flavorText = "FORAGER operatives are known for on-site procurement tactics. ''Looting'' is a word the robbed often use.";
            }

            else if (wepTypeStr == "4")
            {
                reward.name = "Viral Shadow";
                reward.GetComponent<FirearmScript>().cheatOverride = -4;
                reward.GetComponent<FirearmScript>().Awake();
                reward.GetComponent<FirearmScript>().flavorText = "''The Resplendent fell victim to a first-of-its-kind case of ''Interstellar agro-terrorism''. This was no mere plot.''";
            }

            else if (wepTypeStr == "5")
            {
                reward.name = "Contempt for Fellows";
                reward.GetComponent<FirearmScript>().cheatOverride = -5;
                reward.GetComponent<FirearmScript>().Awake();
                reward.GetComponent<FirearmScript>().flavorText = "''Force-feeding Replevin Lucent, a resource known to modify behavior on ingestion, defies most known interstellar laws.''";
            }

            else if (wepTypeStr == "6")
            {
                reward.name = "Underfoot";
                reward.GetComponent<FirearmScript>().cheatOverride = -2;
                reward.GetComponent<FirearmScript>().Awake();
                reward.GetComponent<FirearmScript>().flavorText = "Using this Weapon feels like a perpetual Calvary charge. For where you're going, you won't be needing any breaks.";
            }

            else
            {
                reward.name = "Grenade Launcher (Exotic)";
            }

        }

        else
        {
            reward.GetComponent<FirearmScript>().RarityAugment();

            if (wepPltStr == "1")
            {
                reward.AddComponent<DefaultPlatform>();
            }

            else if (wepPltStr == "2")
            {
                reward.AddComponent<EfficientPlatform>();
            }

            else if (wepPltStr == "3")
            {
                reward.AddComponent<ChatterPlatform>();
            }

            else if (wepPltStr == "4")
            {
                reward.AddComponent<TemperedPlatform>();
            }

            else if (wepPltStr == "5")
            {
                reward.AddComponent<SiphonicPlatform>();
            }

            else if (wepPltStr == "6")
            {
                reward.AddComponent<MiningPlatform>();
            }

            else if (wepPltStr == "7")
            {
                reward.AddComponent<TrenchantPlatform>();
            }

            else
            {
                reward.AddComponent<CachePlatform>();
            }

            if (reward.GetComponent<FirearmScript>().weaponRarity >= 2)
            {
                if (stOneStr == "1")
                {
                    reward.AddComponent<DeepYield>();
                }

                else
                {
                    reward.AddComponent<DeeperYield>();
                }

                if (stTwoStr == "3")
                {
                    reward.AddComponent<DeepStores>();

                }

                else
                {
                    reward.AddComponent<DeeperStores>();
                }

                if (stThreeStr == "5")
                {
                    reward.AddComponent<FarSight>();

                }

                else
                {
                    reward.AddComponent<FartherSight>();
                }

                if (stFourStr == "7")
                {
                    reward.AddComponent<HastyHands>();

                }

                else
                {
                    reward.AddComponent<HastierHands>();
                }

                if (reward.GetComponent<FirearmScript>().weaponRarity == 3)
                {
                    if (fcOneStr == "0")
                    {
                        reward.AddComponent<WaitNowImReady>();
                        reward.GetComponent<WaitNowImReady>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "1")
                    {
                        reward.AddComponent<Efficacy>();
                        reward.GetComponent<Efficacy>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "2")
                    {
                        reward.AddComponent<Inoculated>();
                        reward.GetComponent<Inoculated>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "3")
                    {
                        reward.AddComponent<RudeAwakening>();
                        reward.GetComponent<RudeAwakening>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "4")
                    {
                        reward.AddComponent<NotWithAStick>();
                        reward.GetComponent<NotWithAStick>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "5")
                    {
                        reward.AddComponent<MaliciousWindUp>();
                        reward.GetComponent<MaliciousWindUp>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "6")
                    {
                        reward.AddComponent<PositiveNegative>();
                        reward.GetComponent<PositiveNegative>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "7")
                    {
                        reward.AddComponent<Cadence>();
                        reward.GetComponent<Cadence>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "8")
                    {
                        reward.AddComponent<GoodThingsCome>();
                        reward.GetComponent<GoodThingsCome>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "9")
                    {
                        reward.AddComponent<AllElseFails>();
                        reward.GetComponent<AllElseFails>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "!")
                    {
                        reward.AddComponent<TheMostResplendent>();
                        reward.GetComponent<TheMostResplendent>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "@")
                    {
                        reward.AddComponent<Fulminate>();
                        reward.GetComponent<Fulminate>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "#")
                    {
                        reward.AddComponent<Forager>();
                        reward.GetComponent<Forager>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "$")
                    {
                        reward.AddComponent<Counterplay>();
                        reward.GetComponent<Counterplay>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "%")
                    {
                        reward.AddComponent<Enshroud>();
                        reward.GetComponent<Enshroud>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "^")
                    {
                        reward.AddComponent<GaleForceWinds>();
                        reward.GetComponent<GaleForceWinds>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else
                    {
                        reward.AddComponent<ActivatorDrone>();
                        reward.GetComponent<ActivatorDrone>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    reward.GetComponent<FirearmScript>().procTwo.GetComponent<Text>().text = " ";
                }

                if (reward.GetComponent<FirearmScript>().weaponRarity >= 4)
                {
                    if (fcOneStr == "9")
                    {
                        reward.AddComponent<AllElseFails>();
                        reward.GetComponent<AllElseFails>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "4")
                    {
                        reward.AddComponent<NotWithAStick>();
                        reward.GetComponent<NotWithAStick>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "5")
                    {
                        reward.AddComponent<MaliciousWindUp>();
                        reward.GetComponent<MaliciousWindUp>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "6")
                    {
                        reward.AddComponent<PositiveNegative>();
                        reward.GetComponent<PositiveNegative>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "8")
                    {
                        reward.AddComponent<GoodThingsCome>();
                        reward.GetComponent<GoodThingsCome>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "!")
                    {
                        reward.AddComponent<TheMostResplendent>();
                        reward.GetComponent<TheMostResplendent>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "@")
                    {
                        reward.AddComponent<Fulminate>();
                        reward.GetComponent<Fulminate>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else if (fcOneStr == "#")
                    {
                        reward.AddComponent<Forager>();
                        reward.GetComponent<Forager>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }

                    else
                    {
                        reward.AddComponent<ActivatorDrone>();
                        reward.GetComponent<ActivatorDrone>().proc = reward.GetComponent<FirearmScript>().procOne;
                    }


                    if (fcTwoStr == "0")
                    {
                        reward.AddComponent<WaitNowImReady>();
                        reward.GetComponent<WaitNowImReady>().proc = reward.GetComponent<FirearmScript>().procTwo;
                    }

                    else if (fcTwoStr == "1")
                    {
                        reward.AddComponent<Efficacy>();
                        reward.GetComponent<Efficacy>().proc = reward.GetComponent<FirearmScript>().procTwo;
                    }

                    else if (fcTwoStr == "2")
                    {
                        reward.AddComponent<Inoculated>();
                        reward.GetComponent<Inoculated>().proc = reward.GetComponent<FirearmScript>().procTwo;

                    }

                    else if (fcTwoStr == "7")
                    {
                        reward.AddComponent<Cadence>();
                        reward.GetComponent<Cadence>().proc = reward.GetComponent<FirearmScript>().procTwo;
                    }

                    else if (fcTwoStr == "3")
                    {
                        reward.AddComponent<RudeAwakening>();
                        reward.GetComponent<RudeAwakening>().proc = reward.GetComponent<FirearmScript>().procTwo;
                    }

                    else if (fcTwoStr == "$")
                    {
                        reward.AddComponent<Counterplay>();
                        reward.GetComponent<Counterplay>().proc = reward.GetComponent<FirearmScript>().procTwo;
                    }

                    else if (fcTwoStr == "%")
                    {
                        reward.AddComponent<Enshroud>();
                        reward.GetComponent<Enshroud>().proc = reward.GetComponent<FirearmScript>().procTwo;
                    }

                    else
                    {
                        reward.AddComponent<GaleForceWinds>();
                        reward.GetComponent<GaleForceWinds>().proc = reward.GetComponent<FirearmScript>().procTwo;
                    }
                }
            }
        }

        if (addAsFavorite)
        {
            reward.GetComponent<FirearmScript>().favorite = true;
            addAsFavorite = false;
        }                
        
        var main = acceptEffect.GetComponent<ParticleSystem>().main;     
        if (raritySpawn == 1)
        {
            main.startColor = Color.white;
        }

        else if (raritySpawn == 2)
        {
            main.startColor = Color.green;
        }

        else if (raritySpawn == 3)
        {
            main.startColor = Color.red;
        }

        else if (raritySpawn == 4)
        {
            main.startColor = Color.yellow;
        }

        else
        {
            main.startColor = Color.cyan;
        }

        Instantiate(acceptEffect, player.transform.position, player.transform.rotation);

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Triggers SpawnDrop() after a delay
    /// </summary>
    IEnumerator SpawnRepeatedly()
    {
        spawnAgain = 0.0f;
        SpawnDrop();
        yield return new WaitForSeconds(spawnRate);
    }

    //private void OnCollisionEnter(Collision collision)
    //{       
    //    //if(isDrop == true)
    //    //{
    //    //    if(collision.gameObject.CompareTag("Player"))
    //    //    {
    //    //        SpawnLoot();
    //    //        Destroy(gameObject);
    //    //    }
    //    //}
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(isDrop)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!collapsedLootInfo.gameObject.activeInHierarchy)
                {
                    collapsedLootInfo.gameObject.SetActive(true);
                }
            }
        }     
    }

    private void OnTriggerStay(Collider other)
    {
        if(isDrop)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                withinVolume = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(isDrop)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (collapsedLootInfo.gameObject.activeInHierarchy)
                {
                    withinVolume = false;
                    collapsedLootInfo.gameObject.SetActive(false);
                }
            }
        }       
    }

    public void RarityEnforcement()
    {
        if (raritySpawn == 1)
        {
            for (int r = 0; r < rarityIdentification.Count; r++)
            {
                rarityIdentification[r].GetComponent<Renderer>().material.color = Color.white;
            }

            GameObject lightSource = Instantiate(lootLight, transform.position, transform.rotation);
            lightSource.GetComponent<Light>().color = Color.white;
            lightSource.name = lootLight.name;
            lightSource.transform.parent = gameObject.transform;

            GameObject deliveryItemCircle = Instantiate(lootFocusCircle, transform.position + Vector3.down * 1.2f, Quaternion.identity);
            deliveryItemCircle.GetComponent<Renderer>().material.color = Color.white;
            deliveryItemCircle.name = lootFocusCircle.name;
            deliveryItemCircle.transform.parent = gameObject.transform;

        }

        else if (raritySpawn == 2)
        {
            for (int r = 0; r < rarityIdentification.Count; r++)
            {
                rarityIdentification[r].GetComponent<Renderer>().material.color = Color.green;
            }

            GameObject lightSource = Instantiate(lootLight, transform.position, transform.rotation);
            lightSource.GetComponent<Light>().color = Color.green;
            lightSource.name = lootLight.name;
            lightSource.transform.parent = gameObject.transform;

            GameObject deliveryItemCircle = Instantiate(lootFocusCircle, transform.position + Vector3.down * 1.2f, Quaternion.identity);
            deliveryItemCircle.GetComponent<Renderer>().material.color = Color.green;
            deliveryItemCircle.name = lootFocusCircle.name;
            deliveryItemCircle.transform.parent = gameObject.transform;

        }

        else if (raritySpawn == 3)
        {
            for (int r = 0; r < rarityIdentification.Count; r++)
            {
                rarityIdentification[r].GetComponent<Renderer>().material.color = Color.red;
            }

            GameObject lightSource = Instantiate(lootLight, transform.position, transform.rotation);
            lightSource.GetComponent<Light>().color = Color.red;
            lightSource.name = lootLight.name;
            lightSource.transform.parent = gameObject.transform;

            GameObject deliveryItemCircle = Instantiate(lootFocusCircle, transform.position + Vector3.down * 1.2f, Quaternion.identity);
            deliveryItemCircle.GetComponent<Renderer>().material.color = Color.red;
            deliveryItemCircle.name = lootFocusCircle.name;
            deliveryItemCircle.transform.parent = gameObject.transform;

        }

        else if (raritySpawn == 4)
        {
            for (int r = 0; r < rarityIdentification.Count; r++)
            {
                rarityIdentification[r].GetComponent<Renderer>().material.color = Color.yellow;
            }

            GameObject lightSource = Instantiate(lootLight, transform.position, transform.rotation);
            lightSource.GetComponent<Light>().color = Color.yellow;
            lightSource.name = lootLight.name;
            lightSource.transform.parent = gameObject.transform;

            GameObject deliveryItemCircle = Instantiate(lootFocusCircle, transform.position + Vector3.down * 1.2f, Quaternion.identity);
            deliveryItemCircle.GetComponent<Renderer>().material.color = Color.yellow;
            deliveryItemCircle.name = lootFocusCircle.name;
            deliveryItemCircle.transform.parent = gameObject.transform;

            GameObject particles = Instantiate(lootEffect, lootModel.transform.position + Vector3.down * 1.2f, Quaternion.identity);
            particles.name = lootEffect.name;
            particles.transform.parent = gameObject.transform;

            var main = particles.GetComponent<ParticleSystem>().main;
            main.startColor = Color.yellow;

            particles.GetComponent<ParticleSystem>().Play();
        }

        else
        {          
            if (exoticDelivery)
            {
                for (int r = 0; r < rarityIdentification.Count; r++)
                {
                    if(!rarityIdentification[r].GetComponent<ColorLerpScript>())
                    {
                        rarityIdentification[r].AddComponent<ColorLerpScript>();
                        rarityIdentification[r].GetComponent<ColorLerpScript>().colorOne = Color.cyan;
                        rarityIdentification[r].GetComponent<ColorLerpScript>().colorTwo = Color.white;    
                    }
                }

                GameObject lightSource = Instantiate(lootLight, transform.position, transform.rotation);
                lightSource.name = lootLight.name;
                lightSource.transform.parent = gameObject.transform;

                lightSource.gameObject.AddComponent<LightLerpScript>();
                lightSource.gameObject.GetComponent<LightLerpScript>().light = lightSource.gameObject;

                lightSource.gameObject.GetComponent<LightLerpScript>().progress = 1f;
                lightSource.gameObject.GetComponent<LightLerpScript>().loop = true;
                lightSource.gameObject.GetComponent<LightLerpScript>().forColor = true;
                lightSource.gameObject.GetComponent<LightLerpScript>().colorOne = Color.cyan;
                lightSource.gameObject.GetComponent<LightLerpScript>().colorTwo = Color.white;

                GameObject deliveryItemCircle = Instantiate(lootFocusCircle, transform.position + Vector3.down * 1.2f, Quaternion.identity);
                //deliveryItemCircle.GetComponent<Renderer>().material.color = Color.cyan;
                deliveryItemCircle.name = lootFocusCircle.name;
                deliveryItemCircle.transform.parent = gameObject.transform;

                deliveryItemCircle.AddComponent<ColorLerpScript>();
                deliveryItemCircle.GetComponent<ColorLerpScript>().colorOne = Color.cyan;
                deliveryItemCircle.GetComponent<ColorLerpScript>().colorTwo = Color.white;

                GameObject particles = Instantiate(lootEffect, lootModel.transform.position + Vector3.down * 1.2f, Quaternion.identity);
                particles = Instantiate(lootEffect, lootModel.transform.position + Vector3.down * 1.2f, Quaternion.identity);
                particles.name = lootEffect.name;
                particles.transform.parent = gameObject.transform;

                var main = particles.GetComponent<ParticleSystem>().main;
                main.startColor = Color.cyan;

                particles.GetComponent<ParticleSystem>().Play();
            }

            else
            {
                for (int r = 0; r < rarityIdentification.Count; r++)
                {
                    rarityIdentification[r].GetComponent<Renderer>().material.color = Color.cyan;
                }

                GameObject lightSource = Instantiate(lootLight, transform.position, transform.rotation);
                lightSource.GetComponent<Light>().color = Color.cyan;
                lightSource.name = lootLight.name;
                lightSource.transform.parent = gameObject.transform;

                GameObject deliveryItemCircle = Instantiate(lootFocusCircle, transform.position + Vector3.down * 1.2f, Quaternion.identity);
                deliveryItemCircle.GetComponent<Renderer>().material.color = Color.cyan;
                deliveryItemCircle.name = lootFocusCircle.name;
                deliveryItemCircle.transform.parent = gameObject.transform;

                GameObject particles = Instantiate(lootEffect, lootModel.transform.position + Vector3.down * 1.2f, Quaternion.identity);
                particles = Instantiate(lootEffect, lootModel.transform.position + Vector3.down * 1.2f, Quaternion.identity);
                particles.name = lootEffect.name;
                particles.transform.parent = gameObject.transform;

                var main = particles.GetComponent<ParticleSystem>().main;
                main.startColor = Color.cyan;

                particles.GetComponent<ParticleSystem>().Play();

            }          

            //lootEffect.Play();
        }

        lootModel.AddComponent<RotateScript>();
        lootModel.GetComponent<RotateScript>().rotationAccelerant = 1;
        lootModel.GetComponent<RotateScript>().automaticY = 0.1f;
    }

    /// <summary>
    /// Generates a Weapon string for Player inspection
    /// </summary>
    public void GenerateWeapon()
    {
        //Determines Weapon Type

        if(focusTarget <= -1)
        {
            determinate = UnityEngine.Random.Range(0, 8);
        }

        else
        {
            determinate = focusTarget;
        }

        //determinate = 1;
        wepTypeStr = determinate.ToString();

        //Determines Weapon rarity by game difficulty or Exotic drop
        if (exoticDelivery)
        {
            determinate = 5;          
        }

        else
        {
            determinate = raritySpawn;           
        }

        wepRarStr = determinate.ToString();

        //Determines Weapon Exotic property
        if(exoticDelivery)
        {
            wepExoStr = "1";
            clpLtRarity.GetComponent<Text>().text = "Exotic";

            if (wepTypeStr == "0")
            {
                clpLtType.GetComponent<Text>().text = "Outstanding Warrant";
            }

            else if (wepTypeStr == "1")
            {
                clpLtType.GetComponent<Text>().text = "The Dismissal";
            }

            else if (wepTypeStr == "2")
            {
                clpLtType.GetComponent<Text>().text = "Apathetic";
            }

            else if (wepTypeStr == "3")
            {
                clpLtType.GetComponent<Text>().text = "Mercies";
            }

            else if (wepTypeStr == "4")
            {
                clpLtType.GetComponent<Text>().text = "Viral Shadow";
            }

            else if (wepTypeStr == "5")
            {
                clpLtType.GetComponent<Text>().text = "Contempt for Fellows";
            }

            else if (wepTypeStr == "6")
            {
                clpLtType.GetComponent<Text>().text = "Underfoot";
            }

            else
            {
                clpLtType.GetComponent<Text>().text = "Grenade Launcher (Exotic)";
            }

        }

        else
        {
            wepExoStr = "0";

            if (wepTypeStr == "0")
            {
                clpLtType.GetComponent<Text>().text = "Full Fire Rifle";
            }

            else if (wepTypeStr == "1")
            {
                clpLtType.GetComponent<Text>().text = "Machine Gun";
            }

            else if (wepTypeStr == "2")
            {
                clpLtType.GetComponent<Text>().text = "Pistol";
            }

            else if (wepTypeStr == "3")
            {
                clpLtType.GetComponent<Text>().text = "Semi Fire Rifle";
            }

            else if (wepTypeStr == "4")
            {
                clpLtType.GetComponent<Text>().text = "Shotgun";
            }

            else if (wepTypeStr == "5")
            {
                clpLtType.GetComponent<Text>().text = "Single Fire Rifle";
            }

            else if (wepTypeStr == "6")
            {
                clpLtType.GetComponent<Text>().text = "SMG";
            }

            else
            {
                clpLtType.GetComponent<Text>().text = "Grenade Launcher";
            }

            if (wepRarStr == "1")
            {
                clpLtRarity.GetComponent<Text>().text = "Usual";
                collapsedLootImage.GetComponent<Image>().color = Color.white;
            }

            else if (wepRarStr == "2")
            {
                clpLtRarity.GetComponent<Text>().text = "Sought";
                collapsedLootImage.GetComponent<Image>().color = Color.green;
            }

            else if (wepRarStr == "3")
            {
                clpLtRarity.GetComponent<Text>().text = "Coveted";
                collapsedLootImage.GetComponent<Image>().color = Color.red;
            }

            else if (wepRarStr == "4")
            {
                clpLtRarity.GetComponent<Text>().text = "Treasured";
                collapsedLootImage.GetComponent<Image>().color = Color.yellow;
            }

            else
            {
                clpLtRarity.GetComponent<Text>().text = "Fated";
                collapsedLootImage.GetComponent<Image>().color = Color.cyan;
            }
        }

        //wepExoStr = determinate.ToString();

        //Determines Weapon Favorite - Always zero
        determinate = 0;
        wepFavStr = determinate.ToString();

        //Determines Weapon Platform
        determinate = UnityEngine.Random.Range(1, 9);
        wepPltStr = determinate.ToString();

        if (wepPltStr == "1")
        {
            clpLtPlatform.GetComponent<Text>().text = "Default\n" +
                        "<i>Standard performance.</i>";
        }

        else if (wepPltStr == "2")
        {
            clpLtPlatform.GetComponent<Text>().text = "Efficient\n" +
                        "<i>Slow-firing, higher damage.</i>";
        }

        else if (wepPltStr == "3")
        {
            clpLtPlatform.GetComponent<Text>().text = "Chatter\n" +
                        "<i>Fast-firing, lower damage.</i>";
        }

        else if (wepPltStr == "4")
        {
            clpLtPlatform.GetComponent<Text>().text = "Tempered\n" +
                        "<i>Optimal damage, control.</i>";
        }

        else if (wepPltStr == "5")
        {
            clpLtPlatform.GetComponent<Text>().text = "Siphonic\n" +
                        "<i>1% Health, Shield on hits.</i>";
        }

        else if (wepPltStr == "6")
        {
            clpLtPlatform.GetComponent<Text>().text = "Mining\n" +
                        "<i>Fires explosive Lucent rounds.</i>";
        }

        else if (wepPltStr == "7")
        {
            clpLtPlatform.GetComponent<Text>().text = "Trenchant\n" +
                        "<i>Debuffs on hits and evasions.</i>";
        }

        else
        {
            clpLtPlatform.GetComponent<Text>().text = "Cache\n" +
                        "<i>Regenerate all grenades.</i>";
        }

        //If Rarity is 1, complete string assembly
        if (wepRarStr == "1")
        {
            lootString = wepTypeStr + wepRarStr + wepExoStr + wepFavStr + wepPltStr;
            clpLtStChtOne.GetComponent<Text>().text = "";
            clpLtStChtTwo.GetComponent<Text>().text = "";
            clpLtStChtThree.GetComponent<Text>().text = "";
            clpLtStChtFour.GetComponent<Text>().text = "";

            clpLtFncChtOne.GetComponent<Text>().text = "";
            clpLtFncChtTwo.GetComponent<Text>().text = "";
        }

        //If Rarity is 2 and up, generate Cheats and complete string assembly
        if (wepRarStr == "2" || wepRarStr == "3" || wepRarStr == "4" || wepRarStr == "5" && wepExoStr == "0")
        {
            determinate = UnityEngine.Random.Range(1, 3);
            stOneStr = determinate.ToString();

            if(stOneStr == "1")
            {
                clpLtStChtOne.GetComponent<Text>().text = "+MAG";

            }

            else
            {
                clpLtStChtOne.GetComponent<Text>().text = "++MAG";
            }

            determinate = UnityEngine.Random.Range(3, 5);
            stTwoStr = determinate.ToString();

            if (stTwoStr == "3")
            {
                clpLtStChtTwo.GetComponent<Text>().text = "+RES";

            }

            else
            {
                clpLtStChtTwo.GetComponent<Text>().text = "++RES";
            }

            determinate = UnityEngine.Random.Range(5, 7);
            stThreeStr = determinate.ToString();

            if (stThreeStr == "5")
            {
                clpLtStChtThree.GetComponent<Text>().text = "+EFR";

            }

            else
            {
                clpLtStChtThree.GetComponent<Text>().text = "++EFR";
            }

            determinate = UnityEngine.Random.Range(7, 9);
            stFourStr = determinate.ToString();

            if (stFourStr == "7")
            {
                clpLtStChtFour.GetComponent<Text>().text = "+RLD";

            }

            else
            {
                clpLtStChtFour.GetComponent<Text>().text = "++RLD";
            }

            if (wepRarStr == "2")
            {
                lootString = wepTypeStr + wepRarStr + wepExoStr + wepFavStr + wepPltStr +
                stOneStr + stTwoStr + stThreeStr + stFourStr;

                clpLtFncChtOne.GetComponent<Text>().text = "";
                clpLtFncChtTwo.GetComponent<Text>().text = "";
            }

            else if(wepRarStr == "3")
            {
                int act = 0;

                char[] newPool = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '@', '#', '$', '%', '^', '&' };
                act = UnityEngine.Random.Range(0, newPool.Length);
                fcOneStr = newPool[act].ToString();

                lootString = wepTypeStr + wepRarStr + wepExoStr + wepFavStr + wepPltStr +
                stOneStr + stTwoStr + stThreeStr + stFourStr +
                fcOneStr;

                if(fcOneStr == "0")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Wait! Now I'm Ready!\n" +
                        "<i>10% Shield on kills.</i>";
                }

                else if (fcOneStr == "1")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Efficacy\n" +
                        "<i>1% Damage on hits.</i>";
                }

                else if (fcOneStr == "2")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Inoculated\n" +
                        "<i>5% Health on kills.</i>";
                }

                else if (fcOneStr == "3")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Rude Awakening\n" +
                        "<i>Cast AOE damage waves.</i>";
                }

                else if (fcOneStr == "4")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Not with a Stick\n" +
                        "<i>35% Effective Range on kills.</i>";
                }

                else if (fcOneStr == "5")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Malicious Wind-Up\n" +
                        "<i>Hits increase Reload Speed.</i>";
                }

                else if (fcOneStr == "6")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Positive-Negative\n" +
                        "<i>Apply damage-over-time.</i>";
                }

                else if (fcOneStr == "7")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Cadence\n" +
                        "<i>Clusters on third kills.</i>";
                }

                else if (fcOneStr == "8")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Good Things Come\n" +
                        "<i>Grants benefits during combat.</i>";
                }

                else if (fcOneStr == "9")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "All Else Fails\n" +
                        "<i>Immmuity on Shield breaks.</i>";
                }

                else if (fcOneStr == "!")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "The Most Resplendent\n" +
                        "<i>Attach crystals to surfaces.</i>";
                }

                else if (fcOneStr == "@")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Fulminate\n" +
                        "<i>2% Des. Grenade damage on hits.</i>";
                }

                else if (fcOneStr == "#")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Forager\n" +
                        "<i>Kills produce pickup bursts.</i>";
                }

                else if (fcOneStr == "$")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Counterplay\n" +
                        "<i>Evading damage create Clusters.</i>";
                }

                else if (fcOneStr == "%")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Enshroud\n" +
                        "<i>15% Melee range on hits.</i>";
                }

                else if(fcOneStr == "^")
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Gale Force Winds\n" +
                        "<i>Cast debuffing winds.</i>";
                }

                else
                {
                    clpLtFncChtOne.GetComponent<Text>().text = "Activator Drone\n" +
                        "<i>Drone attacks, triggers effects.</i>";
                }

                clpLtFncChtTwo.GetComponent<Text>().text = "";
            }

            else
            {
                int choice = 0;

                char[] poolOne = { '9', '4', '5', '6', '8', '!', '@', '#', '&' };
                choice = UnityEngine.Random.Range(0, poolOne.Length);
                fcOneStr = poolOne[choice].ToString();

                char[] poolTwo = { '0', '1', '2', '7', '3', '$', '%', '^' };
                choice = UnityEngine.Random.Range(0, poolTwo.Length);
                fcTwoStr = poolTwo[choice].ToString();

                lootString = wepTypeStr + wepRarStr + wepExoStr + wepFavStr + wepPltStr +
                    stOneStr + stTwoStr + stThreeStr + stFourStr +
                    fcOneStr + fcTwoStr;

                if (fcOneStr == "9")
                {
                    if(wepRarStr == "5")
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "All Else Fails (Fated)\n" +
                        "<i>Immmune longer on Shield breaks.</i>";
                    }

                    else
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "All Else Fails\n" +
                        "<i>Immmuity on Shield breaks.</i>";
                    }
                }

                else if (fcOneStr == "4")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Not with a Stick (Fated)\n" +
                        "<i>Halfway maxes Aim Assist.</i>";
                    }

                    else
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Not with a Stick\n" +
                        "<i>35% Effective Range on kills.</i>";
                    }
                }

                else if (fcOneStr == "5")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Malicious Wind-Up (Fated)\n" +
                        "<i>Kills fill 5% reserve ammo.</i>";
                    }

                    else
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Malicious Wind-Up\n" +
                        "<i>Hits increase Reload Speed.</i>";
                    }
                }

                else if (fcOneStr == "6")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Positive-Negative (Fated)\n" +
                        "<i>Stronger damage-over-time.</i>";
                    }

                    else
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Positive-Negative\n" +
                        "<i>Apply damage-over-time.</i>";
                    }
                }

                else if (fcOneStr == "8")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Good Things Come (Fated)\n" +
                        "<i>Infinite ammo during combat.</i>";
                    }

                    else
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Good Things Come\n" +
                        "<i>Grants benefits during combat.</i>";

                    }
                }

                else if (fcOneStr == "!")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "The Most Resplendent (Fated)\n" +
                        "<i>Crystals add 35% Health.</i>";
                    }

                    else
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "The Most Resplendent\n" +
                        "<i>Attach crystals to surfaces.</i>";

                    }
                }

                else if (fcOneStr == "@")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Fulminate (Fated)\n" +
                        "<i>Throw two Des. Grenades.</i>";
                    }

                    else
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Fulminate\n" +
                        "<i>2% Des. Grenade damage on hits.</i>";

                    }
                }

                else if (fcOneStr == "#")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Forager (Fated)\n" +
                        "<i>Burst on 10th Boss hits.</i>";
                    }

                    else
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Forager\n" +
                        "<i>Kills produce pickup bursts.</i>";

                    }
                }

                else
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Activator Drone (Fated)\n" +
                        "<i>Drone triggers other Cheats.</i>";
                    }

                    else
                    {
                        clpLtFncChtOne.GetComponent<Text>().text = "Activator Drone\n" +
                        "<i>Drone attacks, triggers effects.</i>";

                    }
                }

                if (fcTwoStr == "0")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Wait! Now I'm Ready! (Fated)\n" +
                        "<i>20% Shield on kills.</i>";
                    }

                    else
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Wait! Now I'm Ready!\n" +
                        "<i>10% Shield on kills.</i>";
                    }
                }

                else if (fcTwoStr == "1")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Efficacy (Fated)\n" +
                        "<i>2% Damage on hits.</i>";
                    }

                    else
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Efficacy\n" +
                        "<i>1% Damage on hits.</i>";
                    }
                }

                else if (fcTwoStr == "2")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Inoculated (Fated)\n" +
                        "<i>10% Health on kills.</i>";
                    }

                    else
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Inoculated\n" +
                        "<i>5% Health on kills.</i>";

                    }
                }

                else if (fcTwoStr == "7")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Cadence (Fated)\n" +
                        "<i>Clusters on third hits.</i>";
                    }

                    else
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Cadence\n" +
                        "<i>Clusters on third kills.</i>";
                    }
                }

                else if (fcTwoStr == "3")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Rude Awakening (Fated)\n" +
                        "<i>20% Damage with any stack.</i>";
                    }

                    else
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Rude Awakening\n" +
                        "<i>Cast AOE damage waves.</i>";
                    }
                }

                else if (fcTwoStr == "$")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Counterplay (Fated)\n" +
                        "<i>Evading casts Sol. Grenades.</i>";
                    }

                    else
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Counterplay\n" +
                        "<i>Evading damage create Clusters.</i>";
                    }
                }

                else if (fcTwoStr == "%")
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Enshroud (Fated)\n" +
                        "<i>Throw damaging Fog. Grenades.</i>";
                    }

                    else
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Enshroud\n" +
                        "<i>15% Melee range on hits.</i>";
                    }
                }

                else
                {
                    if (wepRarStr == "5")
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Gale Force Winds (Fated)\n" +
                        "<i>Winds apply all debuffs.</i>";
                    }

                    else
                    {
                        clpLtFncChtTwo.GetComponent<Text>().text = "Gale Force Winds\n" +
                        "<i>Cast debuffing winds.</i>";
                    }
                }
            }          
        }   
        
        if(wepRarStr == "5" && wepExoStr == "1")
        {
            stOneStr = "2";
            stTwoStr = "4";
            stThreeStr = "6";
            stFourStr = "8";

            clpLtStChtOne.GetComponent<Text>().text = "++MAG";
            clpLtStChtTwo.GetComponent<Text>().text = "++RES";
            clpLtStChtThree.GetComponent<Text>().text = "++EFR";
            clpLtStChtFour.GetComponent<Text>().text = "++RLD";

            if (wepTypeStr == "0")
            {
                wepPltStr = "5";
                fcOneStr = "A";
                fcTwoStr = "2";
                clpLtPlatform.GetComponent<Text>().text = "Siphonic\n" +
                        "<i>1% Health, Shield on hits.</i>";
                clpLtFncChtOne.GetComponent<Text>().text = "Equivalent Exchange\n" +
                        "<i>Taking damage increases damage.</i>";
                clpLtFncChtTwo.GetComponent<Text>().text = "Inoculated\n" +
                        "<i>5% Health on kills.</i>";
            }

            if (wepTypeStr == "1")
            {
                wepPltStr = "5";
                fcOneStr = "G";
                fcTwoStr = "!";
                clpLtPlatform.GetComponent<Text>().text = "Mining\n" +
                        "<i>Fires explosive Lucent rounds.</i>";
                clpLtFncChtOne.GetComponent<Text>().text = "Pay to Win\n" +
                        "<i>Spend Lucent for damage.</i>";
                clpLtFncChtTwo.GetComponent<Text>().text = "The Most Resplendent\n" +
                        "<i>Attach crystals to surfaces.</i>";
            }

            if (wepTypeStr == "2")
            {
                wepPltStr = "2";
                fcOneStr = "C";
                fcTwoStr = "$";
                clpLtPlatform.GetComponent<Text>().text = "Efficient\n" +
                        "<i>Slow-firing, higher damage.</i>";
                clpLtFncChtOne.GetComponent<Text>().text = "Superweapon\n" +
                        "<i>Charge extreme-damage shots.</i>";
                clpLtFncChtTwo.GetComponent<Text>().text = "Counterplay\n" +
                        "<i>Evading damage create Clusters.</i>";
            }

            if (wepTypeStr == "3")
            {
                wepPltStr = "7";
                fcOneStr = "F";
                fcTwoStr = "0";
                clpLtPlatform.GetComponent<Text>().text = "Trenchant\n" +
                        "<i>Debuffs on hits and evasions.</i>";
                clpLtFncChtOne.GetComponent<Text>().text = "Volant\n" +
                        "<i>Enable character flight.</i>";
                clpLtFncChtTwo.GetComponent<Text>().text = "Wait! Now I'm Ready!\n" +
                        "<i>10% Shield on kills.</i>";
            }

            if (wepTypeStr == "4")
            {
                wepPltStr = "4";
                fcOneStr = "D";
                fcTwoStr = "4";
                clpLtPlatform.GetComponent<Text>().text = "Tempered\n" +
                        "<i>Optimal damage, control.</i>";
                clpLtFncChtOne.GetComponent<Text>().text = "Social Distance, please!\n" +
                        "<i>Spreads 400% damage on kills.</i>";
                clpLtFncChtTwo.GetComponent<Text>().text = "Not with a Stick\n" +
                        "<i>35% Effective Range on kills.</i>";
            }

            if (wepTypeStr == "5")
            {
                wepPltStr = "8";
                fcOneStr = "E";
                fcTwoStr = "1";
                clpLtPlatform.GetComponent<Text>().text = "Cache\n" +
                        "<i>Regenerate all grenades.</i>";
                clpLtFncChtOne.GetComponent<Text>().text = "Early Berth gets the Hearst\n" +
                        "<i>Every other shot explodes.</i>";
                clpLtFncChtTwo.GetComponent<Text>().text = "Efficacy\n" +
                        "<i>1% Damage on hits.</i>";
            }

            if (wepTypeStr == "6")
            {
                wepPltStr = "3";
                fcOneStr = "B";
                fcTwoStr = "#";
                clpLtPlatform.GetComponent<Text>().text = "Chatter\n" +
                        "<i>Fast-firing, lower damage.</i>";
                clpLtFncChtOne.GetComponent<Text>().text = "Absolutely no Stops\n" +
                        "<i>Firing increases fire rate.</i>";
                clpLtFncChtTwo.GetComponent<Text>().text = "Forager\n" +
                        "<i>Kills produce pickup bursts.</i>";
            }

            lootString = wepTypeStr + wepRarStr + wepExoStr + wepFavStr + wepPltStr +
                stOneStr + stTwoStr + stThreeStr + stFourStr +
                fcOneStr + fcTwoStr;
        }       
    }

    public IEnumerator SetupDelivery()
    {
        yield return new WaitForSeconds(2f);

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, contactOnly))
        {
            if(hit.collider.gameObject.layer == 8)
            {
                transform.position = hit.point + (hit.normal * 1.25f);
            }
        }

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        if(gameObject.GetComponent<Rigidbody>())
        {
            Destroy(gameObject.GetComponent<Rigidbody>());
        }

        if (gameObject.GetComponent<BoxCollider>())
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.GetComponent<BoxCollider>().size = new Vector3(3f, 3f, 3f);
        }

        RarityEnforcement();
        GenerateWeapon();
    }
}