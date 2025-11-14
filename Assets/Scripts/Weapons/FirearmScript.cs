using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirearmScript : MonoBehaviour
{
    //Basic attributes
    public int damage;
    //This value buffs weapon damage:
    //-Increasing this number adds a percentage of current damage onto itself
    //-Increasing damage allows this number to inflict more damage in return
    public float damagePercent = 10f;

    public int currentAmmo;
    public int ammoSize; //Max magazine size
    public int reserveAmmo;
    public int reserveSize; //Max reserve ammo size
    public int weaponRarity; //Value that determines rarity of firearm
    public int cheatOverride; //Exotics only -- Determines which Exotic cheat to add
    public float fireRate; //Value that determines how fast a weapon can fire
    public float effectiveRange; //Maximum range in which a weapon inflicts full damage
    public float range; //Maximum total weapon range
    public float reloadSpeed;
    public float wepRecoil = 0.4f; //Value that determines experienced weapon feedback to camera
    public float aimAssistStrength = 0.2f; //Value that governs strength of Aim Assist pull towards target
    public bool isExotic; //Exotics only -- Determines whether weapon is Exotic and can take Exotic cheats
    public string flavorText; //Optional Text "Lore" for weapons
    public LayerMask contactOnly; //Ensures Raycast contact with specified layers
    public bool saved; // If checked, weapon will not generate cheats for itself -- savable weapons only
    public bool display; //If checked, Weapon will not do anything -- for Inventory screen only
    private int damageAdd; //adds additional damage onto total damage
    private float dmgPctReset; //Holds starting damage percent

    //Additional Components
    public Camera gunCam; //Camera used for Raycast
    public Material bulletTrail; //LineRenderer Material for bullet visual
    public Transform barrel; //Origin point for Raycast
    public TextMesh DPSNumbers; //TextMesh that displays damage in-environment when hitting an Enemy
    public Sprite reticle;
    public ParticleSystem muzzleFlash; //VFX for firing a Weapon
    public ParticleSystem sparks; //VFX for striking an object with the "Surface" layer;
    internal GameObject targetHit; //Holds reference of damaged Enemy
    internal GameObject procOne, procTwo, dpsText; //Text objects that track Cheat, Damage activity
    internal PlayerInventoryScript inv;
    internal Vector3 cadencePosition, fatedCadencePosition;
    internal Color bulletTrailColor = Color.yellow;

    //Hidden variables
    internal float fireAgain; //Seconds to wait until weapon can fire
    internal int ammoSpent = 0; //Tracks how many times weapon has fired -- used to subtract from reserve ammo
    internal bool isReloading = false; //Confirms an active reload if true
    internal float reloadReset;
    internal bool confirmHit; //Affirms an achieved hit if true
    internal bool confirmKill; //Affirms an achieved defeat if true
    internal string currentDPSLine = ""; //Records damage history
    internal string newDPSLine; //Records most recent damage
    internal int indentSpace = 0; //Amount of applied indentation
    internal float dpsLinesClear = 2f; //Clears damage history after this time
    internal float dpsLinesReset;
    internal int cheatRNG; // Number used to randomly generate Cheats

    void Awake()
    {
        if(display)
        {
            return;
        }

        //Retrieves Text UI for Cheat activity, Damage history
        //Applies Rarity effects and generates Cheats
        else
        {
            procOne = GameObject.Find("weaponCheatText (1)");
            if (procOne.GetComponent<Text>() != null)
            {
                procOne.GetComponent<Text>().text = " ";
            }

            procTwo = GameObject.Find("weaponCheatText (2)");
            if (procTwo.GetComponent<Text>() != null)
            {
                procTwo.GetComponent<Text>().text = " ";
            }

            dpsText = GameObject.Find("dpsText");
            dpsLinesReset = dpsLinesClear;

            RarityAugment();
            AmmoCheats();
            RangeCheats();
            ReloadSpeedCheats();
            CheatGenerator();
            enabled = false;
        }      
    }

    // Start is called before the first frame update
    void Start()
    {
        if(display)
        {
            return;
        }

        //Retrieves Inventory, Main Camera
        //Initializes hit and kill statuses, saves starting Reload Speed
        else
        {
            inv = FindObjectOfType<PlayerInventoryScript>();
            gunCam = Camera.main;
            confirmHit = false;
            confirmKill = false;
            reloadReset = reloadSpeed;
        }      
    }

    // Update is called once per frame
    void Update()
    {
        if(display)
        {
            return; 
        }

        else
        {

            //Allows for updates to damage history, Weapon behaviors if the game isn't paused
            if (Time.timeScale == 0)
            {
                return;
            }

            else
            {
                DPSNumbers.text = damage.ToString();
                if (currentDPSLine.Length != 0)
                {
                    dpsLinesClear -= Time.deltaTime;
                    if (dpsLinesClear <= 0f)
                    {
                        currentDPSLine = "";
                        dpsLinesClear = dpsLinesReset;
                    }
                }

                AmmoReloadCheck();
                FireWeapon();
            }
        }       
    }

    /// <summary>
    /// Applies damage increases based on Weapon rarity
    /// </summary>
    public virtual void RarityAugment()
    {
        dmgPctReset = damagePercent;
        //If rarity set 0 or less, auto-corrects to lowest rarity.
        if(weaponRarity <= 0)
        {
            weaponRarity = 1;
        }

        if (weaponRarity >= 2 && !isExotic)
        {
            damagePercent *= weaponRarity;
            damagePercent /= 100;
            damagePercent *= damage;
            damageAdd = (int)damagePercent;

            damage += damageAdd;
            damagePercent = dmgPctReset;
        }

        if (weaponRarity == 5 && isExotic)
        {
            damagePercent = 60f;
            damagePercent /= 100;
            damagePercent *= damage;
            damageAdd = (int)damagePercent;

            damage += damageAdd;
            damagePercent = dmgPctReset;
        }

        //If rarity set 6 or more, auto-corrects to highest rarity.
        if (weaponRarity >= 6)
        {
            weaponRarity = 5;

            damagePercent *= weaponRarity;
            damagePercent /= 100;
            damagePercent *= damage;
            damageAdd = (int)damagePercent;

            damage += damageAdd;
        }       
    }  

    /// <summary>
    /// Generates Ammo Stat Cheats through RNG
    /// </summary>
    public virtual void AmmoCheats()
    {
        if (isExotic == true)
        {
            gameObject.AddComponent<DeeperYield>();
            gameObject.AddComponent<DeeperStores>();
            return;
        }

        if (saved == true)
        {
            return;
        }

        cheatRNG = Random.Range(0, 101);

        if (cheatRNG <= 50)
        {
            gameObject.AddComponent<DeepYield>();
        }

        else
        {
            gameObject.AddComponent<DeeperYield>();
        }

        cheatRNG = Random.Range(100, 201);

        if (cheatRNG <= 150)
        {
            gameObject.AddComponent<DeepStores>();
        }

        else
        {
            gameObject.AddComponent<DeeperStores>();
        }       

    }

    /// <summary>
    /// Generates Range Stat Cheats through RNG
    /// </summary>
    public virtual void RangeCheats()
    {
        if(isExotic == true)
        {
            gameObject.AddComponent<FartherSight>();
            return;
        }    

        if (saved == true)
        {
            return;
        }

        cheatRNG = Random.Range(200, 301);

        if(cheatRNG <= 250)
        {
            gameObject.AddComponent<FarSight>();
        }

        else
        {
            gameObject.AddComponent<FartherSight>();          
        }
    }

    /// <summary>
    /// Generates Reload Speed Stat Cheats through RNG
    /// </summary>
    public virtual void ReloadSpeedCheats()
    {
        if(isExotic == true)
        {
            gameObject.AddComponent<HastierHands>();
            return;
        }

        if (saved == true)
        {
            return;
        }

        cheatRNG = Random.Range(300, 401);

        if(cheatRNG <= 350)
        {
            gameObject.AddComponent<HastyHands>();           
        }

        else
        {
            gameObject.AddComponent<HastierHands>();           
        }
    }

    /// <summary>
    /// Generates Functional Cheats through RNG and Rarity 
    /// </summary>
    public virtual void CheatGenerator()
    {
        if (isExotic == true)
        {
            cheatRNG = cheatOverride;
            if(cheatRNG == -1)
            {
                gameObject.AddComponent<EquivalentExchange>();
                gameObject.AddComponent<WaitNowImReady>();

                gameObject.GetComponent<EquivalentExchange>().proc = procOne;
                gameObject.GetComponent<WaitNowImReady>().proc = procTwo;
            } //Equivalent Exchange + Wait! Now I'm Ready

            if(cheatRNG == -2)
            {
                gameObject.AddComponent<AbsolutelyNoStops>();
                gameObject.AddComponent<Forager>();

                gameObject.GetComponent<AbsolutelyNoStops>().proc = procOne;
                gameObject.GetComponent<Forager>().proc = procTwo;
            } //Absolutely No Stops + Forager

            if(cheatRNG == -3)
            {
                gameObject.AddComponent<ShelterInPlace>();
                gameObject.AddComponent<PositiveNegative>();

                gameObject.GetComponent<ShelterInPlace>().proc = procOne;
                gameObject.GetComponent<PositiveNegative>().proc = procTwo;
            } //Shelter In Place + Positive-Negative

            if(cheatRNG == -4)
            {
                gameObject.AddComponent<SocialDistancePlease>();
                gameObject.AddComponent<NotWithAStick>();

                gameObject.GetComponent<SocialDistancePlease>().proc = procOne;
                gameObject.GetComponent<NotWithAStick>().proc = procTwo;
            } //Social Distance, Please! + Not with a Stick

            if(cheatRNG == -5)
            {
                gameObject.AddComponent<EarlyBerthGetsTheHearst>();
                gameObject.AddComponent<Efficacy>();

                gameObject.GetComponent<EarlyBerthGetsTheHearst>().proc = procOne;
                gameObject.GetComponent<Efficacy>().proc = procTwo;

            } //Early Berth gets the Hearst + Efficacy

            if(cheatRNG == -6)
            {
                gameObject.AddComponent<OffYourOwnSupply>();
                gameObject.AddComponent<Inoculated>();

                gameObject.GetComponent<OffYourOwnSupply>().proc = procOne;
                gameObject.GetComponent<Inoculated>().proc = procTwo;

            } //Off your Own Supply + Inoculated

            if(cheatRNG == -7)
            {
                gameObject.AddComponent<PayToWin>();
                gameObject.AddComponent<TheMostResplendent>();

                gameObject.GetComponent<PayToWin>().proc = procOne;
                gameObject.GetComponent<TheMostResplendent>().proc = procTwo;
            } //Pay to Win + The Most Resplendent

            return;
        }

        if (saved == true)
        {
            return;
        }

        if(weaponRarity == 2 || weaponRarity == 3)
        {
            cheatRNG = Random.Range(400, 1201);
            //cheatRNG = 1151;
            if (cheatRNG <= 450)
            {
                gameObject.AddComponent<WaitNowImReady>();
                gameObject.GetComponent<WaitNowImReady>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";
            }

            if (cheatRNG > 450 && cheatRNG <= 500)
            {
                gameObject.AddComponent<Efficacy>();
                gameObject.GetComponent<Efficacy>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";

            }

            if (cheatRNG > 500 && cheatRNG <= 550)
            {
                gameObject.AddComponent<Inoculated>();
                gameObject.GetComponent<Inoculated>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";

            }

            if (cheatRNG > 550 && cheatRNG <= 600)
            {
                gameObject.AddComponent<RudeAwakening>();
                gameObject.GetComponent<RudeAwakening>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";

            }

            if (cheatRNG > 600 && cheatRNG <= 650)
            {
                gameObject.AddComponent<NotWithAStick>();
                gameObject.GetComponent<NotWithAStick>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";

            }

            if (cheatRNG > 650 && cheatRNG <= 700)
            {
                gameObject.AddComponent<MaliciousWindUp>();
                gameObject.GetComponent<MaliciousWindUp>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";

            }

            if (cheatRNG > 700 && cheatRNG <= 750)
            {
                gameObject.AddComponent<PositiveNegative>();
                gameObject.GetComponent<PositiveNegative>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";

            }

            if (cheatRNG > 750 && cheatRNG <= 800)
            {
                gameObject.AddComponent<Cadence>();
                gameObject.GetComponent<Cadence>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";

            }

            if (cheatRNG > 800 && cheatRNG <= 850)
            {
                gameObject.AddComponent<GoodThingsCome>();
                gameObject.GetComponent<GoodThingsCome>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";

            }

            if (cheatRNG > 850 && cheatRNG <= 900)
            {
                gameObject.AddComponent<AllElseFails>();
                gameObject.GetComponent<AllElseFails>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";

            }

            if (cheatRNG > 900 && cheatRNG <= 950)
            {
                gameObject.AddComponent<TheMostResplendent>();
                gameObject.GetComponent<TheMostResplendent>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";

            }

            if (cheatRNG > 950 && cheatRNG <= 1000)
            {
                gameObject.AddComponent<Fulminate>();
                gameObject.GetComponent<Fulminate>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";
            }

            if (cheatRNG > 1000 && cheatRNG <= 1050)
            {
                gameObject.AddComponent<Forager>();
                gameObject.GetComponent<Forager>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";
            }

            if (cheatRNG > 1050 && cheatRNG <= 1100)
            {
                gameObject.AddComponent<Counterplay>();
                gameObject.GetComponent<Counterplay>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";
            }

            if (cheatRNG > 1100 && cheatRNG <= 1150)
            {
                gameObject.AddComponent<Enshroud>();
                gameObject.GetComponent<Enshroud>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";
            }

            if (cheatRNG > 1150)
            {
                gameObject.AddComponent<GaleForceWinds>();
                gameObject.GetComponent<GaleForceWinds>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";
            }
        }
        
        if(weaponRarity >= 4)
        {
            cheatRNG = Random.Range(400, 481);
            if(cheatRNG <= 410)
            {
                gameObject.AddComponent<AllElseFails>();
                gameObject.GetComponent<AllElseFails>().proc = procOne;
            }

            if (cheatRNG > 410 && cheatRNG <= 420)
            {
                gameObject.AddComponent<NotWithAStick>();
                gameObject.GetComponent<NotWithAStick>().proc = procOne;

            }

            if (cheatRNG > 420 && cheatRNG <= 430)
            {
                gameObject.AddComponent<MaliciousWindUp>();
                gameObject.GetComponent<MaliciousWindUp>().proc = procOne;

            }

            if (cheatRNG > 430 && cheatRNG <= 440)
            {
                gameObject.AddComponent<PositiveNegative>();
                gameObject.GetComponent<PositiveNegative>().proc = procOne;

            }

            if (cheatRNG > 440 && cheatRNG <= 450)
            {
                gameObject.AddComponent<GoodThingsCome>();
                gameObject.GetComponent<GoodThingsCome>().proc = procOne;

            }

            if (cheatRNG > 450 && cheatRNG <= 460)
            {
                gameObject.AddComponent<TheMostResplendent>();
                gameObject.GetComponent<TheMostResplendent>().proc = procOne;
            }

            if (cheatRNG > 460 && cheatRNG <= 470)
            {
                gameObject.AddComponent<Fulminate>();
                gameObject.GetComponent<Fulminate>().proc = procOne;
            }

            if(cheatRNG > 470)
            {
                gameObject.AddComponent<Forager>();
                gameObject.GetComponent<Forager>().proc = procOne;
            }

            cheatRNG = Random.Range(480, 561);
            if (cheatRNG <= 490)
            {
                gameObject.AddComponent<WaitNowImReady>();
                gameObject.GetComponent<WaitNowImReady>().proc = procTwo;

            }

            if (cheatRNG > 490 && cheatRNG <= 500)
            {
                gameObject.AddComponent<Efficacy>();
                gameObject.GetComponent<Efficacy>().proc = procTwo;

            }

            if (cheatRNG > 500 && cheatRNG <= 510)
            {
                gameObject.AddComponent<Inoculated>();
                gameObject.GetComponent<Inoculated>().proc = procTwo;

            }

            if (cheatRNG > 510 && cheatRNG <= 520)
            {
                gameObject.AddComponent<Cadence>();
                gameObject.GetComponent<Cadence>().proc = procTwo;

            }

            if (cheatRNG > 520 && cheatRNG <= 530)
            {
                gameObject.AddComponent<RudeAwakening>();
                gameObject.GetComponent<RudeAwakening>().proc = procTwo;

            }

            if (cheatRNG > 530 && cheatRNG <= 540)
            {
                gameObject.AddComponent<Counterplay>();
                gameObject.GetComponent<Counterplay>().proc = procTwo;

            }

            if (cheatRNG > 540 && cheatRNG <= 550)
            {
                gameObject.AddComponent<Enshroud>();
                gameObject.GetComponent<Enshroud>().proc = procTwo;

            }

            if (cheatRNG > 550)
            {
                gameObject.AddComponent<GaleForceWinds>();
                gameObject.GetComponent<GaleForceWinds>().proc = procTwo;
            }
        }
    }

    /// <summary>
    /// Prevents reloads during firing, lack of ammo or holding a full magazine
    /// </summary>
    public virtual void AmmoReloadCheck()
    {      
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo != 0 || currentAmmo <= 0 && reserveAmmo != 0)
        {
            if(Input.GetButton("Fire1") || Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Cannot reload; Weapon is being actively fired!");

                inv.weaponAmmoPage.gameObject.SetActive(true);
                //weaponLoad.gameObject.SetActive(true);
                inv.lucentText.gameObject.SetActive(true);
                inv.wepStateTimer = inv.wepStateTimerReset;
                return;
            }

            if (currentAmmo >= 0 && reserveAmmo <= 0)
            {
                Debug.Log("Cannot reload; no spare ammo!");

                inv.weaponAmmoPage.gameObject.SetActive(true);
                //weaponLoad.gameObject.SetActive(true);
                inv.lucentText.gameObject.SetActive(true);
                inv.wepStateTimer = inv.wepStateTimerReset;
            }

            else if (currentAmmo <= 0 && reserveAmmo <= 0)
            {
                Debug.Log("Cannot reload; no ammo!");

                inv.weaponAmmoPage.gameObject.SetActive(true);
                //weaponLoad.gameObject.SetActive(true);
                inv.lucentText.gameObject.SetActive(true);
                inv.wepStateTimer = inv.wepStateTimerReset;
            }

            else if (currentAmmo >= ammoSize)
            {
                Debug.Log("Cannot reload; magazine is full!");

                inv.weaponAmmoPage.gameObject.SetActive(true);
                //weaponLoad.gameObject.SetActive(true);
                inv.lucentText.gameObject.SetActive(true);
                inv.wepStateTimer = inv.wepStateTimerReset;
            }

            else
            {
                isReloading = true;
                StartCoroutine(ReloadWep());
            }
        }
    }

    /// <summary>
    /// Activates Weapon firing behavior
    /// Provides information to Cheats with hit or kill triggers
    /// </summary>
    public virtual void FireWeapon()
    {
        fireAgain += Time.deltaTime;

        if (Input.GetButton("Fire1") && currentAmmo >= 1 && fireAgain >= fireRate && !isReloading)
        {
            //Firing timer resets, Ammo decrements/records number of shots
            fireAgain = 0.0f;
            currentAmmo--;
            ammoSpent++;

            //Recoil
            gunCam.transform.eulerAngles = new Vector3(GetComponentInParent<PlayerCameraScript>().pitch -=
                Random.Range(-wepRecoil, wepRecoil), GetComponentInParent<PlayerCameraScript>().yaw +=
                Random.Range(-wepRecoil, wepRecoil), 0.0f);

            //Prevents value that tracks times weapon has fired to be more than the total magazine size
            if (ammoSpent >= ammoSize)
            {
                ammoSpent = ammoSize;
            }

            Vector3 rayOrigin = gunCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            //Produces the Bullet Trail
            GameObject start = new GameObject();
            GameObject.Destroy(start, 0.1f);

            start.name = "Trail";
            start.AddComponent<LineRenderer>();
            start.GetComponent<LineRenderer>().startWidth = 0.1f;
            start.GetComponent<LineRenderer>().endWidth = 0.1f;
            start.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            start.GetComponent<LineRenderer>().material = bulletTrail;
            start.GetComponent<LineRenderer>().SetPosition(0, barrel.transform.position);

            if (Physics.Raycast(rayOrigin, gunCam.transform.forward, out hit, range, contactOnly))
            {
                start.gameObject.transform.position = hit.point;
                start.GetComponent<LineRenderer>().SetPosition(1, hit.point);

                if(hit.collider.tag == "Enemy")
                {
                    //Affirms confirmed hits for Cheats
                    confirmHit = true;
                    if (gameObject.GetComponent<MaliciousWindUp>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<MaliciousWindUp>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<Efficacy>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<Efficacy>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<Cadence>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<Cadence>().hitConfirmed = true;
                        gameObject.GetComponent<Cadence>().clusterPosition = hit.point + (hit.normal * 0.01f);
                    }

                    if (gameObject.GetComponent<GoodThingsCome>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<GoodThingsCome>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<PayToWin>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<PayToWin>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<TheMostResplendent>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<TheMostResplendent>().hitConfirmed = true;

                        if (gameObject.GetComponent<TheMostResplendent>().stackCount >= 1 && gameObject.GetComponent<TheMostResplendent>().toggle)
                        {
                            GameObject lucentHard = Instantiate(gameObject.GetComponent<TheMostResplendent>().hardLucent, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal), hit.collider.gameObject.transform);
                            lucentHard.name = gameObject.GetComponent<TheMostResplendent>().hardLucent.name;

                            if (weaponRarity == 5 && !isExotic)
                            {
                                lucentHard.GetComponent<TMRHardLucentScript>().fatedCrystal = true;
                            }

                            lucentHard.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                            gameObject.GetComponent<TheMostResplendent>().stackCount--;
                            gameObject.GetComponent<TheMostResplendent>().toggle = false;
                        }
                    }

                    if(gameObject.GetComponent<Fulminate>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<Fulminate>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<Forager>() && weaponRarity == 5 && !hit.collider.GetComponent<EnemyHealthScript>().isImmune && hit.collider.GetComponent<ReplevinScript>().amBoss)
                    {
                        gameObject.GetComponent<Forager>().hitConfirmed = true;
                        gameObject.GetComponent<Forager>().burstPosition = hit.collider.transform.position + (Vector3.up * 2);

                    }

                    if (gameObject.GetComponent<Enshroud>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<Enshroud>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<GaleForceWinds>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        if (gameObject.GetComponent<GaleForceWinds>().chargeCount >= 1 && gameObject.GetComponent<GaleForceWinds>().toggle)
                        {
                            GameObject torrent = Instantiate(gameObject.GetComponent<GaleForceWinds>().applicator, hit.point + (hit.normal * 0.01f), Quaternion.identity);
                            torrent.name = gameObject.GetComponent<GaleForceWinds>().applicator.name;

                            if (weaponRarity == 5)
                            {
                                torrent.GetComponent<GFWStatusApplicator>().fatedFlag = true;
                                torrent.GetComponent<GFWStatusApplicator>().debuffMultiplier *= 1.43f;
                                torrent.GetComponent<GFWStatusApplicator>().travelRadius *= 1.5f;
                                torrent.GetComponent<GFWStatusApplicator>().travelLerpSpeed *= 2f;
                            }

                            gameObject.GetComponent<GaleForceWinds>().chargeCount--;
                            gameObject.GetComponent<GaleForceWinds>().chargePercentage = 0f;
                            gameObject.GetComponent<GaleForceWinds>().done = false;
                            gameObject.GetComponent<GaleForceWinds>().toggle = false;

                        }
                    }

                    StartCoroutine(DeconfirmHit());
                    targetHit = hit.transform.gameObject;

                    if (hit.distance <= effectiveRange)
                    {
                        //Records damage inflicted on non-immune Enemy hit. Records "Immune" on immune Enemy hit
                        if(hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                        {
                            string indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                            newDPSLine = indent + "Immune";
                            currentDPSLine = newDPSLine + "\n" + currentDPSLine;
                            dpsText.GetComponent<Text>().text = currentDPSLine;
                            dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                            dpsLinesClear = dpsLinesReset;

                            DPSNumbers.text = "Immune";
                        }

                        else
                        {
                            string indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                            newDPSLine = indent + damage.ToString();
                            currentDPSLine = newDPSLine + "\n" + currentDPSLine;
                            dpsText.GetComponent<Text>().text = currentDPSLine;
                            dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                            dpsLinesClear = dpsLinesReset;

                            DPSNumbers.text = damage.ToString();
                            Instantiate(hit.collider.GetComponent<EnemyHealthScript>().blood, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                        }

                        hit.collider.GetComponent<EnemyHealthScript>().inflictDamage(damage);
                        if(hit.collider.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                        {
                            //Affirms confirmed kills for Cheats
                            confirmKill = true;
                            if(gameObject.GetComponent<NotWithAStick>())
                            {
                                gameObject.GetComponent<NotWithAStick>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<MaliciousWindUp>())
                            {
                                gameObject.GetComponent<MaliciousWindUp>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<WaitNowImReady>())
                            {
                                gameObject.GetComponent<WaitNowImReady>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<Inoculated>())
                            {
                                gameObject.GetComponent<Inoculated>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<Cadence>())
                            {
                                gameObject.GetComponent<Cadence>().killConfirmed = true;
                                gameObject.GetComponent<Cadence>().clusterPosition = hit.collider.transform.position;
                            }

                            if (gameObject.GetComponent<RudeAwakening>())
                            {
                                gameObject.GetComponent<RudeAwakening>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<Forager>())
                            {
                                gameObject.GetComponent<Forager>().killConfirmed = true;
                                gameObject.GetComponent<Forager>().burstPosition = hit.collider.transform.position + Vector3.up;
                            }

                            if (hit.collider.GetComponent<Rigidbody>() == null)
                            {
                                hit.collider.gameObject.AddComponent<Rigidbody>();

                                Vector3 shotForceDistance = barrel.transform.position - hit.collider.transform.position;
                                hit.collider.GetComponent<Rigidbody>().AddForce(-shotForceDistance.normalized * 20f, ForceMode.Impulse);
                            }

                            else
                            {
                                Vector3 shotForceDistance = barrel.transform.position - hit.collider.transform.position;
                                hit.collider.GetComponent<Rigidbody>().AddForce(-shotForceDistance.normalized * 20f, ForceMode.Impulse);
                            }
                        }
                    } //For damage falloff checks/kill triggers within Effective Range

                    if (hit.distance > effectiveRange)
                    {
                        //Records damage inflicted on non-immune Enemy hit. Records "Immune" on immune Enemy hit
                        if (hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                        {
                            string indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                            newDPSLine = indent + "Immune";
                            currentDPSLine = newDPSLine + "\n" + currentDPSLine;
                            dpsText.GetComponent<Text>().text = currentDPSLine;
                            dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                            dpsLinesClear = dpsLinesReset;

                            DPSNumbers.text = "Immune";
                        }

                        else
                        {
                            string indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                            newDPSLine = indent + (damage / 2).ToString();
                            currentDPSLine = newDPSLine + "\n" + currentDPSLine;
                            dpsText.GetComponent<Text>().text = currentDPSLine;
                            dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                            dpsLinesClear = dpsLinesReset;

                            DPSNumbers.text = (damage / 2).ToString();
                            Instantiate(hit.collider.GetComponent<EnemyHealthScript>().blood, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                        }

                        hit.collider.GetComponent<EnemyHealthScript>().inflictDamage(damage / 2);
                        if (hit.collider.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                        {
                            //Affirms confirmed kills for Cheats
                            confirmKill = true;
                            if (gameObject.GetComponent<NotWithAStick>())
                            {
                                gameObject.GetComponent<NotWithAStick>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<MaliciousWindUp>())
                            {
                                gameObject.GetComponent<MaliciousWindUp>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<WaitNowImReady>())
                            {
                                gameObject.GetComponent<WaitNowImReady>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<Inoculated>())
                            {
                                gameObject.GetComponent<Inoculated>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<Cadence>())
                            {
                                gameObject.GetComponent<Cadence>().killConfirmed = true;
                                gameObject.GetComponent<Cadence>().clusterPosition = hit.collider.transform.position;
                            }

                            if (gameObject.GetComponent<RudeAwakening>())
                            {
                                gameObject.GetComponent<RudeAwakening>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<Forager>())
                            {
                                gameObject.GetComponent<Forager>().killConfirmed = true;
                                gameObject.GetComponent<Forager>().burstPosition = hit.collider.transform.position + Vector3.up;
                            }

                            if (hit.collider.GetComponent<Rigidbody>() == null)
                            {
                                hit.collider.gameObject.AddComponent<Rigidbody>();
                                Vector3 shotForceDistance = barrel.transform.position - hit.collider.transform.position;
                                hit.collider.GetComponent<Rigidbody>().AddForce(-shotForceDistance.normalized * 10f, ForceMode.Impulse);
                            }

                            else
                            {
                                Vector3 shotForceDistance = barrel.transform.position - hit.collider.transform.position;
                                hit.collider.GetComponent<Rigidbody>().AddForce(-shotForceDistance.normalized * 10f, ForceMode.Impulse);
                            }
                        }
                    } //For damage falloff checks/kill triggers while out of Effective Range
                }

                if (hit.collider.tag == "Lucent")
                {
                    inv.lucentFunds += hit.collider.GetComponent<LucentScript>().lucentGift;
                    if(inv.lucentFunds >= 100000)
                    {
                        inv.lucentFunds = 100000;
                    }

                    hit.collider.GetComponent<LucentScript>().lucentGift = 0;
                    hit.collider.GetComponent<LucentScript>().shot = true;
                }

                if (hit.collider.gameObject.layer == 8) //If this Weapon strikes an object with the "Surface" layer
                {
                    if(gameObject.GetComponent<TheMostResplendent>())
                    {
                        if(gameObject.GetComponent<TheMostResplendent>().stackCount >= 1 && gameObject.GetComponent<TheMostResplendent>().toggle)
                        {
                            GameObject lucentHard = Instantiate(gameObject.GetComponent<TheMostResplendent>().hardLucent, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                            lucentHard.name = gameObject.GetComponent<TheMostResplendent>().hardLucent.name;

                            if(weaponRarity == 5 && !isExotic)
                            {
                                lucentHard.GetComponent<TMRHardLucentScript>().fatedCrystal = true;
                            }

                            gameObject.GetComponent<TheMostResplendent>().stackCount--;
                            gameObject.GetComponent<TheMostResplendent>().toggle = false;
                        }
                    }

                    if(hit.collider.gameObject.GetComponent<TMRHardLucentScript>())
                    {
                        GameObject miniCluster = Instantiate(hit.collider.gameObject.GetComponent<TMRHardLucentScript>().lucentCluster, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                        miniCluster.name = hit.collider.gameObject.GetComponent<TMRHardLucentScript>().lucentCluster.name;
                        miniCluster.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                        miniCluster.GetComponent<LucentScript>().lucentGift /= 2;
                        miniCluster.GetComponent<LucentScript>().lucentGift *= weaponRarity;
                        miniCluster.GetComponent<LucentScript>().ShatterCalculation();

                        hit.collider.gameObject.GetComponent<TMRHardLucentScript>().shockwaveBuildup += damage;
                    }

                    if(gameObject.GetComponent<GaleForceWinds>())
                    {
                        if(gameObject.GetComponent<GaleForceWinds>().chargeCount >= 1 && gameObject.GetComponent<GaleForceWinds>().toggle)
                        {
                            GameObject torrent = Instantiate(gameObject.GetComponent<GaleForceWinds>().applicator, hit.point + (hit.normal * 0.01f), Quaternion.identity);
                            torrent.name = gameObject.GetComponent<GaleForceWinds>().applicator.name;

                            if(weaponRarity == 5)
                            {
                                torrent.GetComponent<GFWStatusApplicator>().fatedFlag = true;
                                torrent.GetComponent<GFWStatusApplicator>().debuffMultiplier *= 1.43f;
                                torrent.GetComponent<GFWStatusApplicator>().travelRadius *= 1.5f;
                                torrent.GetComponent<GFWStatusApplicator>().travelLerpSpeed *= 2f;
                            }

                            gameObject.GetComponent<GaleForceWinds>().chargeCount--;
                            gameObject.GetComponent<GaleForceWinds>().chargePercentage = 0f;
                            gameObject.GetComponent<GaleForceWinds>().done = false;
                            gameObject.GetComponent<GaleForceWinds>().toggle = false;

                        }
                    }

                    Instantiate(sparks, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                }
            }

            else
            {
                start.gameObject.transform.position = rayOrigin + (gunCam.transform.forward * range);
                start.GetComponent<LineRenderer>().SetPosition(1, rayOrigin + (gunCam.transform.forward * range));
            }

            muzzleFlash.Play();
        }    
    }

    /// <summary>
    /// Refills a Weapon's magazine
    /// Used when a Cheat requires an instant reload
    /// </summary>
    public virtual void ReloadWeapon()
    {
        if(reserveAmmo <= ammoSpent)
        {
            currentAmmo += reserveAmmo;
        }

        else
        {
            currentAmmo = ammoSize;
        }

        reserveAmmo -= ammoSpent;

        if (reserveAmmo <= 0)
        {
            reserveAmmo = 0;
        }

        ammoSpent = 0;
        isReloading = false;

    }

    /// <summary>
    /// Refills a Weapon's magazine after a delay
    /// </summary>
    public virtual IEnumerator ReloadWep()
    {
        yield return new WaitForSeconds(reloadSpeed);
        if (reserveAmmo <= ammoSpent)
        {
            currentAmmo += reserveAmmo;
        }

        else
        {
            currentAmmo = ammoSize;
        }

        reserveAmmo -= ammoSpent;

        if (reserveAmmo <= 0)
        {
            reserveAmmo = 0;
        }

        ammoSpent = 0;
        isReloading = false;

        reloadSpeed = reloadReset;


    }

    /// <summary>
    /// Reverts affirmation that hit has been achieved after a delay
    /// </summary>
    public virtual IEnumerator DeconfirmHit()
    {
        yield return new WaitForSeconds(0.01f);
        confirmHit = false;
    }
}