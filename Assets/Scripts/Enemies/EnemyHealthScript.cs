using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    public int difficultyValue; //Increases toughness of Enemy Health
    public int healthCurrent = 2000;
    public int healthMax = 2000;

    //This value buffs enemy health:
    //-Increasing this number adds a percentage of max health onto current health
    //-Increasing max health allows this number to give more health in return
    public float healthPercent = 20f;
    public int lucentYield = 200; //Amount of Lucent awarded when defeated

    //This value buffs Lucent awarded on deaths:
    //-Increasing this number adds a percentage of set Lucent onto itself
    //-Increasing base Lucent yield allows this number to add more Lucent in return
    public float lucentPercent = 20f;
    public bool isImmune; //Affirms imperviousness to damage when true
    public GameObject ammoReward; //Weapon ammo pickup

    public Canvas visual; //Canvas object that visualizes Enemy health

    //currentHealth - Slider representing existing Health
    //healthLost - Slider representing subtracted Health
    public Slider currentHealth, healthLost;

    //curHealthColor - Image representing existing Health
    //losHealthColor - Image representing lost Health
    //debuffNotice - Image used when debuff is received
    //dotNotice - Image used when damage-over-time is received
    //slowNotice - Image used when slowed effect is received
    public Image curHealthColor, losHealthColor, debuffNotice, dotNotice, slowNotice;
    public Text enemyName;
    public ParticleSystem blood; //VFX played when Enemy is damaged
    public float ammoRewardThreshold = 80f; //Goal number to be at or above to spawn Ammunition.

    private float lhUpdateTimer = 1f; //Duration to wait before updating lost Health slider
    private float lhUpdateReset; //Holds starting lost health update duration
    internal float canvasTimer = 2f; //Duration before Canvas disappears
    internal float canvasTimerReset; //Holds starting Canvas disappear timer
    private int healthAdd; //Number used to increase Enemy Health
    private int lucentAdd; //Number used to increase Lucent awarded on defeats
    private float fireRateAdd; //Number used to increase Ranged attack rate
    private int ammoRewardChance; //Number used to randomly award ammo
    private EnemyManagerScript manager;
    private ReplevinScript attack;
    private BerthScript berth;
    private PlayerInventoryScript player;
    internal bool enemyHit; //Affirms Enemy has been hit if true
    internal int damageHit; //Holds received damage taken
    private bool done = false; //Permits one execution if true

    // Start is called before the first frame update
    public void Start()
    {
        manager = FindObjectOfType<EnemyManagerScript>();
        attack = GetComponent<ReplevinScript>();
        player = FindObjectOfType<PlayerInventoryScript>();

        DifficultyMatch(); //Scales Enemy toughness by Difficulty number

        debuffNotice.gameObject.SetActive(false);
        dotNotice.gameObject.SetActive(false);
        slowNotice.gameObject.SetActive(false);
        lhUpdateReset = lhUpdateTimer;
        canvasTimerReset = canvasTimer;

        healthLost.maxValue = healthMax;
        healthLost.value = healthCurrent;
        enemyName.text = gameObject.name;
        visual.gameObject.SetActive(false);
        enemyHit = false;

        if(GetComponent<BerthScript>())
        {
            berth = GetComponent<BerthScript>();
        }     
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth.value = healthCurrent;

        //Updates lost Health slider when timer expires
        lhUpdateTimer -= Time.deltaTime;
        if(lhUpdateTimer <= 0f)
        {
            lhUpdateTimer = 0f;
            healthLost.value = healthCurrent;
        }

        //For bosses, Canvas is active on-screen
        if(attack.boss && healthCurrent > 0)
        {
            canvasTimer = canvasTimerReset;
            visual.gameObject.SetActive(true);
        }

        //Canvas disappears when timer expires
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

        //Effect images (damage-over-time, Health debuff, slowed) appear, disappear if component is detected
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
    
        if (healthCurrent <= 0)
        {

            EnemyDeath();
        }
    }

    /// <summary>
    /// Modifies Enemy max Health, damage dependent on Difficulty number
    /// </summary>
    public void DifficultyMatch()
    {
        //If difficulty value 0 or less, auto-corrects to lowest difficulty.
        if (difficultyValue <= 0)
        {
            difficultyValue = 1;
            healthCurrent = healthMax;

            currentHealth.maxValue = healthMax;
            currentHealth.value = healthCurrent;
            curHealthColor.color = Color.grey;
            losHealthColor.color = Color.red;
        }

        if (difficultyValue == 1)
        {
            healthCurrent = healthMax;

            currentHealth.maxValue = healthMax;
            currentHealth.value = healthCurrent;
            curHealthColor.color = Color.grey;
            losHealthColor.color = Color.red;

        }

        if (difficultyValue >= 2 && difficultyValue <= 5)
        {
            healthPercent *= difficultyValue;
            healthPercent /= 100;
            healthPercent *= healthMax;
            healthAdd = (int)healthPercent;
            healthMax += healthAdd;
            healthCurrent = healthMax;

            lucentPercent *= difficultyValue;
            lucentPercent /= 100;
            lucentPercent *= lucentYield;
            lucentAdd = (int)lucentPercent;
            lucentYield += lucentAdd;

            currentHealth.maxValue = healthMax;
            currentHealth.value = healthCurrent;

            if(difficultyValue == 2)
            {
                curHealthColor.color = Color.green;
                losHealthColor.color = Color.red;
            }

            else if(difficultyValue == 3)
            {
                curHealthColor.color = Color.red;
                losHealthColor.color = Color.white;
            }

            else if (difficultyValue == 4)
            {
                curHealthColor.color = Color.yellow;
                losHealthColor.color = Color.red;
            }

            else if (difficultyValue == 5)
            {
                curHealthColor.color = Color.cyan;
                losHealthColor.color = Color.red;
            }
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

            lucentPercent *= difficultyValue;
            lucentPercent /= 100;
            lucentPercent *= lucentYield;
            lucentAdd = (int)lucentPercent;
            lucentYield += lucentAdd;

            currentHealth.maxValue = healthMax;
            currentHealth.value = healthCurrent;
            curHealthColor.color = Color.cyan;
            losHealthColor.color = Color.red;

        }
    }

    /// <summary>
    /// Applies damage received to Health
    /// </summary>
    /// <param name="damageTaken">represents damage received</param>
    public void inflictDamage(int damageTaken)
    {
        if(isImmune)
        {
            return;
        }

        enemyHit = true;
        damageHit = damageTaken;
        manager.damageReceived += damageHit;

        if (GetComponent<SDPHealthDebuff>() != null)
        {
            healthCurrent -= (damageHit * GetComponent<SDPHealthDebuff>().damageAmp);
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

        currentHealth.value = healthCurrent;
        lhUpdateTimer = lhUpdateReset;
        canvasTimer = canvasTimerReset;
    }

    /// <summary>
    /// Marks Enemy as a corpse, awards Lucent & Ammo, and unfreezes Rigidbody constraints
    /// </summary>
    void EnemyDeath()
    {
        if(!done)
        {
            gameObject.tag = "Corpse";

            ammoRewardChance = Random.Range(0, 101);
            if (ammoRewardChance >= ammoRewardThreshold)
            {
                Instantiate(ammoReward, transform.position, transform.rotation);
            }

            manager.enemyDied = true;
            manager.RemoveEnemies();
            manager.DeathReward(transform.position);

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
                                Instantiate(GetComponent<SDPHealthDebuff>().activation, hit.transform.position, Quaternion.identity);
                            }
                        }
                    }
                }

                Instantiate(GetComponent<SDPHealthDebuff>().activation, transform.position, Quaternion.identity);
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

        Destroy(gameObject, 5f);
    }
}