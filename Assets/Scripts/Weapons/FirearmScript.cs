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
    public LayerMask contactOnly;
    private int damageAdd; //adds additional damage onto total damage
    private float dmgPctReset;

    //Additional Components
    public Camera gunCam; //Camera used to determine start of Raycast
    public LineRenderer bulletTrail;
    public Transform barrel;
    public TextMesh DPSNumbers; //TextMesh that displays damage in-environment when hitting an Enemy
    public Sprite reticle;
    internal GameObject targetHit;
    internal GameObject procOne, procTwo, dpsText;
    internal PlayerInventoryScript inv;
    internal Vector3 cadencePosition, fatedCadencePosition;

    //Hidden variables
    internal float fireAgain; //Seconds to wait until weapon can fire
    internal int ammoSpent = 0; //Tracks how many times weapon has fired -- used to subtract from reserve ammo
    internal bool isReloading = false;
    internal float reloadReset;
    internal bool confirmHit; //Tracks whether weapon shot is a confirmed hit on Enemy
    internal bool confirmKill;
    internal string currentDPSLine = "";
    internal string newDPSLine;
    internal int indentSpace = 0;
    internal float dpsLinesClear = 2f;
    internal float dpsLinesReset;

    //RNG numbers
    internal int ammoCheatOne; // Determines Current Ammo augments
    internal int ammoCheatTwo; // Determines Resereve Ammo augments
    internal int rangeCheatOne; // Determines Effective Range augments
    internal int reloadCheatOne; // Determines Reload Speed augments
    internal int cheatRNG; // Determines Weapon added Functions augments -- Overridden if weapon is Exotic
    internal int fcnChtOne;
    internal int fcnChtTwo;
    public bool saved; // If checked, weapon will not generate cheats for itself -- savable weapons only
    public bool display; //If checked, Weapon will not do anything -- for Inventory screen only

    void Awake()
    {
        if(display)
        {
            return;
        }

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
            //if (dpsText.GetComponent<Text>() != null)
            //{
            //    dpsText.GetComponent<Text>().text = " ";
            //}
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

        else
        {
            bulletTrail = GetComponent<LineRenderer>();
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
            DeconfirmKill();

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

                //if (isReloading)
                //{
                //    reloadSpeed = reloadReset;
                //    StartCoroutine(ReloadWep());
                //    reloadSpeed -= Time.deltaTime;
                //    if (reloadSpeed <= 0f)
                //    {
                //        reloadSpeed = reloadReset;
                //        ReloadWeapon();
                //    }
                //}

                FireWeapon();
            }
        }       
    }

    public virtual void RarityAugment()
    {
        dmgPctReset = damagePercent;
        //If rarity set 0 or less, auto-corrects to lowest rarity.
        if(weaponRarity <= 0)
        {
            weaponRarity = 1;
            damage *= weaponRarity;
        }

        if(weaponRarity == 1)
        {
            damage *= weaponRarity;
        }

        if (weaponRarity == 2)
        {
            damagePercent *= weaponRarity;
            damagePercent /= 100;
            damagePercent *= damage;
            damageAdd = (int)damagePercent;

            damage += damageAdd;
            damagePercent = dmgPctReset;

            //damage *= weaponRarity;
        }

        if (weaponRarity == 3)
        {
            damagePercent *= weaponRarity;
            damagePercent /= 100;
            damagePercent *= damage;
            damageAdd = (int)damagePercent;

            damage += damageAdd;
            damagePercent = dmgPctReset;
        }

        if (weaponRarity == 4)
        {
            damagePercent *= weaponRarity;
            damagePercent /= 100;
            damagePercent *= damage;
            damageAdd = (int)damagePercent;

            damage += damageAdd;
            damagePercent = dmgPctReset;
        }

        if (weaponRarity == 5 && !isExotic)
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
        if (weaponRarity >= 6 /*|| isExotic == true*/)
        {
            weaponRarity = 5;
            damagePercent *= weaponRarity;
            damagePercent /= 100;
            damagePercent *= damage;
            damageAdd = (int)damagePercent;

            damage += damageAdd;
        }       
    }  

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

        ammoCheatOne = Random.Range(0, 101);
        ammoCheatTwo = Random.Range(100, 201);

        if (ammoCheatOne <= 50)
        {
            gameObject.AddComponent<DeepYield>();
        }

        if (ammoCheatOne > 50)
        {
            gameObject.AddComponent<DeeperYield>();
        }

        if (ammoCheatTwo <= 150)
        {
            gameObject.AddComponent<DeepStores>();
        }

        if (ammoCheatTwo > 150)
        {
            gameObject.AddComponent<DeeperStores>();
        }       

    }

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

        rangeCheatOne = Random.Range(200, 301);

        if(rangeCheatOne <= 250)
        {
            gameObject.AddComponent<FarSight>();
        }

        if (rangeCheatOne > 250)
        {
            gameObject.AddComponent<FartherSight>();          
        }
    }

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

        reloadCheatOne = Random.Range(300, 401);

        if(reloadCheatOne <= 350)
        {
            gameObject.AddComponent<HastyHands>();           
        }

        if (reloadCheatOne > 350)
        {
            gameObject.AddComponent<HastierHands>();           
        }
    }

    public virtual void CheatGenerator()
    {
        if(isExotic == true)
        {
            cheatRNG = cheatOverride;
            if(cheatRNG == -1)
            {
                gameObject.AddComponent<EquivalentExchange>();
                gameObject.AddComponent<WaitNowImReady>();

                gameObject.GetComponent<EquivalentExchange>().proc = procOne;
                gameObject.GetComponent<WaitNowImReady>().proc = procTwo;
            }

            if (cheatRNG == -2)
            {
                gameObject.AddComponent<AbsolutelyNoStops>();
                gameObject.AddComponent<GoodThingsCome>();

                gameObject.GetComponent<AbsolutelyNoStops>().proc = procOne;
                gameObject.GetComponent<GoodThingsCome>().proc = procTwo;
            }

            if(cheatRNG == -3)
            {
                gameObject.AddComponent<ShelterInPlace>();
                gameObject.AddComponent<PositiveNegative>();

                gameObject.GetComponent<ShelterInPlace>().proc = procOne;
                gameObject.GetComponent<PositiveNegative>().proc = procTwo;
            }

            if(cheatRNG == -4)
            {
                gameObject.AddComponent<SocialDistancePlease>();
                gameObject.AddComponent<NotWithAStick>();

                gameObject.GetComponent<SocialDistancePlease>().proc = procOne;
                gameObject.GetComponent<NotWithAStick>().proc = procTwo;
            }

            if(cheatRNG == -5)
            {
                gameObject.AddComponent<EarlyBerthGetsTheHearst>();
                gameObject.AddComponent<Efficacy>();

                gameObject.GetComponent<EarlyBerthGetsTheHearst>().proc = procOne;
                gameObject.GetComponent<Efficacy>().proc = procTwo;

            }

            if(cheatRNG == -6)
            {
                gameObject.AddComponent<OffYourOwnSupply>();
                gameObject.AddComponent<Inoculated>();

                gameObject.GetComponent<OffYourOwnSupply>().proc = procOne;
                gameObject.GetComponent<Inoculated>().proc = procTwo;

            }

            if(cheatRNG == -7)
            {
                gameObject.AddComponent<PayToWin>();
                gameObject.AddComponent<MaliciousWindUp>();

                gameObject.GetComponent<PayToWin>().proc = procOne;
                gameObject.GetComponent<MaliciousWindUp>().proc = procTwo;
            }

            return;
        }

        if (saved == true)
        {
            return;
        }

        if(weaponRarity == 2 || weaponRarity == 3)
        {
            cheatRNG = Random.Range(400, 901);
            //cheatRNG = 851;
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

            if (cheatRNG > 850)
            {
                gameObject.AddComponent<AllElseFails>();
                gameObject.GetComponent<AllElseFails>().proc = procOne;
                procTwo.GetComponent<Text>().text = " ";

            }
        }
        
        if(weaponRarity >= 4)
        {
            fcnChtOne = Random.Range(400, 451);
            if(fcnChtOne <= 410)
            {
                gameObject.AddComponent<AllElseFails>();
                gameObject.GetComponent<AllElseFails>().proc = procOne;
            }

            if (fcnChtOne > 410 && fcnChtOne <= 420)
            {
                gameObject.AddComponent<NotWithAStick>();
                gameObject.GetComponent<NotWithAStick>().proc = procOne;

            }

            if (fcnChtOne > 420 && fcnChtOne <= 430)
            {
                gameObject.AddComponent<MaliciousWindUp>();
                gameObject.GetComponent<MaliciousWindUp>().proc = procOne;

            }

            if (fcnChtOne > 430 && fcnChtOne <= 440)
            {
                gameObject.AddComponent<PositiveNegative>();
                gameObject.GetComponent<PositiveNegative>().proc = procOne;

            }

            if (fcnChtOne > 440)
            {
                gameObject.AddComponent<GoodThingsCome>();
                gameObject.GetComponent<GoodThingsCome>().proc = procOne;

            }

            fcnChtTwo = Random.Range(450, 501);
            if (fcnChtTwo <= 460)
            {
                gameObject.AddComponent<WaitNowImReady>();
                gameObject.GetComponent<WaitNowImReady>().proc = procTwo;

            }

            if (fcnChtTwo > 460 && fcnChtTwo <= 470)
            {
                gameObject.AddComponent<Efficacy>();
                gameObject.GetComponent<Efficacy>().proc = procTwo;

            }

            if (fcnChtTwo > 470 && fcnChtTwo <= 480)
            {
                gameObject.AddComponent<Inoculated>();
                gameObject.GetComponent<Inoculated>().proc = procTwo;

            }

            if (fcnChtTwo > 480 && fcnChtTwo <= 490)
            {
                gameObject.AddComponent<Cadence>();
                gameObject.GetComponent<Cadence>().proc = procTwo;

            }

            if (fcnChtTwo > 490)
            {
                gameObject.AddComponent<RudeAwakening>();
                gameObject.GetComponent<RudeAwakening>().proc = procTwo;

            }
        }
    }

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
                //return;
            }

            else if (currentAmmo <= 0 && reserveAmmo <= 0)
            {
                Debug.Log("Cannot reload; no ammo!");

                inv.weaponAmmoPage.gameObject.SetActive(true);
                //weaponLoad.gameObject.SetActive(true);
                inv.lucentText.gameObject.SetActive(true);
                inv.wepStateTimer = inv.wepStateTimerReset;
                //return;
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

                //return;
            }
        }
    }

    public virtual void FireWeapon()
    {
        fireAgain += Time.deltaTime;

        if (Input.GetButton("Fire1") && currentAmmo >= 1 && fireAgain >= fireRate && !isReloading)
        {
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

            GameObject start = new GameObject();
            GameObject.Destroy(start, 0.1f);

            start.name = "Trail";
            start.AddComponent<LineRenderer>();
            start.GetComponent<LineRenderer>().startWidth = 0.1f;
            start.GetComponent<LineRenderer>().endWidth = 0.1f;
            start.GetComponent<LineRenderer>().material = bulletTrail.GetComponent<LineRenderer>().material;
            start.GetComponent<LineRenderer>().SetPosition(0, barrel.transform.position);

            //bulletTrail.SetPosition(0, barrel.position);

            if (Physics.Raycast(rayOrigin, gunCam.transform.forward, out hit, range, contactOnly))
            {
                start.gameObject.transform.position = hit.point;
                start.GetComponent<LineRenderer>().SetPosition(1, hit.point);

                //bulletTrail.SetPosition(1, hit.point);
                //Instantiate(DPSNumbers, hit.point, transform.rotation);

                if(hit.collider.tag == "Enemy")
                {
                    confirmHit = true;
                    if (gameObject.GetComponent<MaliciousWindUp>())
                    {
                        gameObject.GetComponent<MaliciousWindUp>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<Efficacy>())
                    {
                        gameObject.GetComponent<Efficacy>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<Cadence>())
                    {
                        gameObject.GetComponent<Cadence>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<GoodThingsCome>())
                    {
                        gameObject.GetComponent<GoodThingsCome>().hitConfirmed = true;
                    }

                    StartCoroutine(DeconfirmHit());
                    FatedCadenceRewardPosition(hit.collider.transform.position);
                    targetHit = hit.transform.gameObject;

                    //Debug.Log(hit.transform.gameObject);

                    //For damage falloff checks
                    if (hit.distance <= effectiveRange)
                    {
                        if(hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                        {
                            string indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                            newDPSLine = indent + "Immune";
                            currentDPSLine = newDPSLine + "\n" + currentDPSLine;
                            dpsText.GetComponent<Text>().text = currentDPSLine;
                            dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                            dpsLinesClear = dpsLinesReset;

                            DPSNumbers.text = "Immune";
                            //dpsText.GetComponent<Text>().text += "\n" + "Immune";
                            //dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
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
                            //dpsText.GetComponent<Text>().text += "\n" + damage.ToString();
                            //dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                        }

                        //Instantiate(DPSNumbers, hit.point, transform.rotation);
                        hit.collider.GetComponent<EnemyHealthScript>().inflictDamage(damage);
                        if(hit.collider.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                        {
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
                                CadenceRewardPosition(hit.collider.transform.position);
                            }

                            if (gameObject.GetComponent<RudeAwakening>())
                            {
                                gameObject.GetComponent<RudeAwakening>().killConfirmed = true;
                            }

                            //if(hit.collider.GetComponent<Rigidbody>() != null)
                            //{
                            //    hit.collider.GetComponent<Rigidbody>().AddForce(-hit.collider.transform.forward * 0.5f, ForceMode.Impulse);
                            //}
                        }
                    }

                    if (hit.distance > effectiveRange)
                    {
                        if (hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                        {
                            string indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                            newDPSLine = indent + "Immune";
                            currentDPSLine = newDPSLine + "\n" + currentDPSLine;
                            dpsText.GetComponent<Text>().text = currentDPSLine;
                            dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                            dpsLinesClear = dpsLinesReset;

                            DPSNumbers.text = "Immune";
                            //dpsText.GetComponent<Text>().text += "\n" + "Immune";
                            //dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
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
                            //dpsText.GetComponent<Text>().text += "\n" + (damage / 2).ToString();
                            //dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                        }

                        //Instantiate(DPSNumbers, hit.point, transform.rotation);
                        hit.collider.GetComponent<EnemyHealthScript>().inflictDamage(damage / 2);
                        if (hit.collider.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                        {
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
                                CadenceRewardPosition(hit.collider.transform.position);

                            }

                            if (gameObject.GetComponent<RudeAwakening>())
                            {
                                gameObject.GetComponent<RudeAwakening>().killConfirmed = true;
                            }

                            //if (hit.collider.GetComponent<Rigidbody>() != null)
                            //{
                            //    hit.collider.GetComponent<Rigidbody>().AddForce(-hit.collider.transform.forward * 0.5f, ForceMode.Impulse);
                            //}

                        }
                    }

                    if (hit.collider.GetComponent<ReplevinScript>() != null)
                    {
                        hit.collider.GetComponent<ReplevinScript>().playerFound = true;
                    }

                    //For Clusters
                    if (hit.collider.GetComponent<EnemyLeaderScript>() != null)
                    {
                        hit.collider.GetComponent<EnemyLeaderScript>().Pursuit();
                    }

                    if (hit.collider.GetComponent<EnemyFollowerScript>() != null)
                    {
                        if(hit.collider.GetComponent<EnemyFollowerScript>().leader != null && hit.collider.GetComponent<EnemyFollowerScript>().leader.GetComponent<EnemyHealthScript>().healthCurrent > 0)
                        {
                            hit.collider.GetComponent<EnemyFollowerScript>().leader.Pursuit();
                        }
                    }

                }

                if (hit.collider.tag == "Lucent")
                {
                    inv.lucentFunds += hit.collider.GetComponent<LucentScript>().lucentGift;
                    if(inv.lucentFunds >= 100000)
                    {
                        inv.lucentFunds = 100000;
                    }

                    hit.collider.GetComponent<LucentScript>().lucentGift = 0;
                }
            }

            else
            {
                //bulletTrail.SetPosition(1, rayOrigin + (gunCam.transform.forward * range));

                start.gameObject.transform.position = rayOrigin + (gunCam.transform.forward * range);
                start.GetComponent<LineRenderer>().SetPosition(1, rayOrigin + (gunCam.transform.forward * range));
            }

        }
        
    }

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

    public virtual IEnumerator DeconfirmHit()
    {
        yield return new WaitForSeconds(0.01f);
        confirmHit = false;
    }

    public virtual void DeconfirmKill()
    {
        if(confirmKill)
        {
            confirmKill = false;
        }
    }

    //The Following methods helps Cadence determine where to spawn Lucent clusters.

    public virtual void CadenceRewardPosition(Vector3 killPosition)
    {
        cadencePosition = killPosition;
    }

    public virtual void FatedCadenceRewardPosition(Vector3 shotPosition)
    {
        fatedCadencePosition = shotPosition;
    }  
}
