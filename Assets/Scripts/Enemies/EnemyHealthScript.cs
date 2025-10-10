using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    public int difficultyValue;
    public int healthCurrent = 2000;
    public int healthMax = 2000;
    //This value buffs enemy health:
    //-Increasing this number adds a percentage of max health onto current health
    //-Increasing max health allows this number to give more health in return
    public float healthPercent = 20f;

    public int lucentYield = 200;
    //This value buffs Lucent awarded on deaths:
    //-Increasing this number adds a percentage of set Lucent onto itself
    //-Increasing base Lucent yield allows this number to add more Lucent in return
    public float lucentPercent = 20f;

    public bool isImmune;
    public bool isDummy;
    public GameObject corpse;
    public GameObject ammoReward;

    public Canvas visual;
    public Slider currentHealth, healthLost;
    public Image curHealthColor, losHealthColor, debuffNotice, dotNotice, slowNotice;
    public Text enemyName;
    public ParticleSystem blood; //Effect that plays when Enemy has been damaged
    public float ammoRewardThreshold = 80f; //Required number to be at or above in order to spawn Ammunition.
    private float lhUpdateTimer = 1f;
    private float lhUpdateReset;
    internal float canvasTimer = 2f;
    internal float canvasTimerReset;

    private int healthAdd;
    private int damageAdd;
    private int lucentAdd;
    private float fireRateAdd;
    private int ammoRewardChance;
    private EnemyManagerScript manager;
    private ReplevinScript attack;
    private BerthScript berth;
    private PlayerInventoryScript player;
    private LayerMask layer = 7;
    internal bool enemyHit;
    internal int damageHit;
    private bool done = false;

    // Start is called before the first frame update
    public void Start()
    {
        //healthCurrent = healthMax;
        manager = FindObjectOfType<EnemyManagerScript>();
        attack = GetComponent<ReplevinScript>();
        player = FindObjectOfType<PlayerInventoryScript>();

        DifficultyMatch();

        debuffNotice.gameObject.SetActive(false);
        dotNotice.gameObject.SetActive(false);
        slowNotice.gameObject.SetActive(false);
        lhUpdateReset = lhUpdateTimer;
        canvasTimerReset = canvasTimer;

        healthLost.maxValue = healthMax;
        healthLost.value = healthCurrent;
        enemyName.text = gameObject.name;
        visual.gameObject.SetActive(false);

        if (GetComponent<BerthScript>() == null)
        {
            return;
        }

        else
        {
            berth = GetComponent<BerthScript>();
        }

        enemyHit = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth.value = healthCurrent;
        lhUpdateTimer -= Time.deltaTime;
        if(lhUpdateTimer <= 0f)
        {
            lhUpdateTimer = 0f;
            healthLost.value = healthCurrent;
        }

        if(attack.boss && healthCurrent > 0)
        {
            canvasTimer = canvasTimerReset;
            visual.gameObject.SetActive(true);
        }

        canvasTimer -= Time.deltaTime;
        if (canvasTimer <= 0f)
        {
            canvasTimer = 0f;
            if (visual.gameObject.activeInHierarchy == true)
            {
                visual.gameObject.SetActive(false);
            }

            else
            {
                visual.gameObject.SetActive(false);
            }
        }

        if (GetComponent<PosNegDOT>() != null || GetComponent<DamageOverTimeScript>() != null)
        {
            if (dotNotice.gameObject.activeInHierarchy == false)
            {
                dotNotice.gameObject.SetActive(true);
            }
        }

        else
        {
            dotNotice.gameObject.SetActive(false);
        }    

        if (GetComponent<SDPHealthDebuff>() != null || GetComponent<DebuffScript>() != null)
        {
            if (debuffNotice.gameObject.activeInHierarchy == false)
            {
                debuffNotice.gameObject.SetActive(true);
            }          
        }

        else
        {
            debuffNotice.gameObject.SetActive(false);
        }

        if (GetComponent<SlowedScript>() != null)
        {
            if (slowNotice.gameObject.activeInHierarchy == false)
            {
                slowNotice.gameObject.SetActive(true);
            }
         
        }

        else
        {
            slowNotice.gameObject.SetActive(false);
        }
    
        if (isDummy == true)
        {
            return;
        }

        if (healthCurrent <= 0)
        {
            if(isDummy == true)
            {
                return;
            }

            //manager.enemyDied = true;
            EnemyDeath();
        }
    }

    public void DifficultyMatch()
    {
        //If difficulty value 0 or less, auto-corrects to lowest difficulty.
        if (difficultyValue <= 0)
        {
            difficultyValue = 1;
            healthCurrent = healthMax;
            attack.damage *= difficultyValue;

            currentHealth.maxValue = healthMax;
            currentHealth.value = healthCurrent;
            curHealthColor.color = Color.grey;
            losHealthColor.color = Color.red;
        }

        if (difficultyValue == 1)
        {
            //healthMax += healthAdd;
            healthCurrent = healthMax;
            attack.damage *= difficultyValue;

            currentHealth.maxValue = healthMax;
            currentHealth.value = healthCurrent;
            curHealthColor.color = Color.grey;
            losHealthColor.color = Color.red;

        }

        if (difficultyValue == 2)
        {
            healthPercent *= difficultyValue;
            healthPercent /= 100;
            healthPercent *= healthMax;
            healthAdd = (int)healthPercent;

            attack.damagePercent *= difficultyValue;
            attack.damagePercent /= 100;
            attack.damagePercent *= attack.damage;
            damageAdd = (int)attack.damagePercent;

            attack.rangeAttackChange *= difficultyValue;
            attack.rangeAttackChange /= 100;
            attack.rangeAttackChange *= attack.rangeAttackRate;
            attack.rangeAttackRate -= attack.rangeAttackChange;

            lucentPercent *= difficultyValue;
            lucentPercent /= 100;
            lucentPercent *= lucentYield;
            lucentAdd = (int)lucentPercent;
            lucentYield += lucentAdd;

            healthMax += healthAdd;
            healthCurrent = healthMax;
            attack.damage += damageAdd;

            currentHealth.maxValue = healthMax;
            currentHealth.value = healthCurrent;
            curHealthColor.color = Color.green;
            losHealthColor.color = Color.red;

        }

        if (difficultyValue == 3)
        {
            healthPercent *= difficultyValue;
            healthPercent /= 100;
            healthPercent *= healthMax;
            healthAdd = (int)healthPercent;

            attack.damagePercent *= difficultyValue;
            attack.damagePercent /= 100;
            attack.damagePercent *= attack.damage;
            damageAdd = (int)attack.damagePercent;

            attack.rangeAttackChange *= difficultyValue;
            attack.rangeAttackChange /= 100;
            attack.rangeAttackChange *= attack.rangeAttackRate;
            attack.rangeAttackRate -= attack.rangeAttackChange;

            lucentPercent *= difficultyValue;
            lucentPercent /= 100;
            lucentPercent *= lucentYield;
            lucentAdd = (int)lucentPercent;
            lucentYield += lucentAdd;

            healthMax += healthAdd;
            healthCurrent = healthMax;
            attack.damage += damageAdd;

            currentHealth.maxValue = healthMax;
            currentHealth.value = healthCurrent;
            curHealthColor.color = Color.red;
            losHealthColor.color = Color.white;

        }

        if (difficultyValue == 4)
        {
            healthPercent *= difficultyValue;
            healthPercent /= 100;
            healthPercent *= healthMax;
            healthAdd = (int)healthPercent;

            attack.damagePercent *= difficultyValue;
            attack.damagePercent /= 100;
            attack.damagePercent *= attack.damage;
            damageAdd = (int)attack.damagePercent;

            attack.rangeAttackChange *= difficultyValue;
            attack.rangeAttackChange /= 100;
            attack.rangeAttackChange *= attack.rangeAttackRate;
            attack.rangeAttackRate -= attack.rangeAttackChange;

            lucentPercent *= difficultyValue;
            lucentPercent /= 100;
            lucentPercent *= lucentYield;
            lucentAdd = (int)lucentPercent;
            lucentYield += lucentAdd;

            healthMax += healthAdd;
            healthCurrent = healthMax;
            attack.damage += damageAdd;

            currentHealth.maxValue = healthMax;
            currentHealth.value = healthCurrent;
            curHealthColor.color = Color.yellow;
            losHealthColor.color = Color.red;

        }

        if (difficultyValue == 5)
        {
            healthPercent *= difficultyValue;
            healthPercent /= 100;
            healthPercent *= healthMax;
            healthAdd = (int)healthPercent;

            attack.damagePercent *= difficultyValue;
            attack.damagePercent /= 100;
            attack.damagePercent *= attack.damage;
            damageAdd = (int)attack.damagePercent;

            attack.rangeAttackChange *= difficultyValue;
            attack.rangeAttackChange /= 100;
            attack.rangeAttackChange *= attack.rangeAttackRate;
            attack.rangeAttackRate -= attack.rangeAttackChange;

            lucentPercent *= difficultyValue;
            lucentPercent /= 100;
            lucentPercent *= lucentYield;
            lucentAdd = (int)lucentPercent;
            lucentYield += lucentAdd;

            healthMax += healthAdd;
            healthCurrent = healthMax;
            attack.damage += damageAdd;

            currentHealth.maxValue = healthMax;
            currentHealth.value = healthCurrent;
            curHealthColor.color = Color.cyan;
            losHealthColor.color = Color.red;

        }

        //If difficulty value 6 or more, auto-corrects to highest difficulty.
        if (difficultyValue >= 6)
        {
            difficultyValue = 5;
            healthPercent *= difficultyValue;
            healthPercent /= 100;
            healthPercent *= healthMax;
            healthAdd = (int)healthPercent;

            healthMax += healthAdd;
            healthCurrent = healthMax;
            attack.damage *= difficultyValue;

            currentHealth.maxValue = healthMax;
            currentHealth.value = healthCurrent;
            curHealthColor.color = Color.cyan;
            losHealthColor.color = Color.red;

        }
    }

    public void inflictDamage(int damageTaken)
    {
        if(isImmune)
        {
            return;
        }

        enemyHit = true;
        damageHit = damageTaken;
        manager.damageReceived += damageHit;

        //if(GetComponent<PosNegDOT>() != null)
        //{
        //    dotNotice.gameObject.SetActive(true);
        //}

        if (GetComponent<SDPHealthDebuff>() != null)
        {
            healthCurrent -= (damageHit * GetComponent<SDPHealthDebuff>().damageAmp);
            //debuffNotice.gameObject.SetActive(true);
        }

        else if (GetComponent<DebuffScript>() != null)
        {
            healthCurrent -= (damageHit * (int)GetComponent<DebuffScript>().damageAmp);
        }

        else
        {
            healthCurrent -= damageHit;
        }

        if (visual.gameObject.activeInHierarchy != true)
        {
            if(healthCurrent <= 0)
            {
                visual.gameObject.SetActive(false);
            }

            else
            {
                visual.gameObject.SetActive(true);
            }
        }

        //healthCurrent -= damageHit;
        currentHealth.value = healthCurrent;
        lhUpdateTimer = lhUpdateReset;
        canvasTimer = canvasTimerReset;
        //StartCoroutine(HealthLostDelayedUpdate());
    }

    void EnemyDeath()
    {
        //Debug.Log(transform.position);
        //manager.deathPos = transform.position;

        if(!done)
        {
            //Instantiate(corpse, transform.position, transform.rotation);
            gameObject.tag = "Corpse";
            corpse.layer = 7;

            ammoRewardChance = Random.Range(0, 101);
            if (ammoRewardChance >= ammoRewardThreshold)
            {
                Instantiate(ammoReward, transform.position, transform.rotation);
            }

            manager.enemyDied = true;
            manager.RemoveEnemies();
            manager.DeathReward(transform.position);

            manager.CadenceRewardPosition(gameObject.transform.position); //For Cadence positioning only
            manager.FatedCadenceRewardPosition(gameObject.transform.position); //For Cadence positioning only

            if (GetComponent<SDPHealthDebuff>() != null)
            {
                Vector3 epicenter = transform.position;
                Collider[] affected = Physics.OverlapSphere(transform.position, 10f);
                foreach (Collider hit in affected)
                {
                    if (hit.gameObject.CompareTag("Enemy"))
                    {
                        if (hit.GetComponent<EnemyHealthScript>() != null)
                        {
                            hit.GetComponent<EnemyHealthScript>().inflictDamage(GetComponent<SDPHealthDebuff>().dmgShare);
                            if (hit.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                            {
                                Instantiate(GetComponent<SDPHealthDebuff>().activation, hit.transform.position, hit.transform.rotation);
                            }
                        }
                    }

                    //Rigidbody inflict = hit.GetComponent<Rigidbody>();
                    //if (inflict != null)
                    //{
                    //    if (inflict.GetComponent<EnemyHealthScript>() != null)
                    //    {
                    //        inflict.GetComponent<EnemyHealthScript>().inflictDamage(GetComponent<SDPHealthDebuff>().dmgShare);
                    //    }
                    //}
                }

                Instantiate(GetComponent<SDPHealthDebuff>().activation, transform.position, transform.rotation);
            }

            if (berth != null)
            {
                berth.Explode();
            }

            if(attack.amBoss)
            {
                if(attack.boss.isAlive)
                {
                    attack.boss.isAlive = false;
                }
            }

            player.lucentFunds += lucentYield;
            if(player.lucentFunds >= 100000)
            {
                player.lucentFunds = 100000;
            }

            if(gameObject.GetComponent<Rigidbody>() == null)
            {
                gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                gameObject.GetComponent<Rigidbody>().freezeRotation = false;
            }

            else
            {
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                gameObject.GetComponent<Rigidbody>().freezeRotation = false;
            }

            visual.gameObject.SetActive(false);
            done = true;
        }

        Destroy(gameObject, 4f);
    }

    IEnumerator RestoreMovement()
    {
        yield return new WaitForSeconds(0.1f);
        attack.moveSpeed = attack.moveSpeedReset;
        attack.boostSpeed = attack.boostSpeedReset;
        attack.nmaAccel = attack.nmaAccelReset;
    }

    //public IEnumerator HealthLostDelayedUpdate()
    //{
    //    yield return new WaitForSeconds(0.75f);
    //    healthLost.value = healthCurrent;
    //}
}
