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
    public Slider healthBar;
    public Image barColor;

    private int healthAdd;
    private int damageAdd;
    private int lucentAdd;
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
        healthBar.gameObject.SetActive(false);

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
        healthBar.value = healthCurrent;

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

            healthBar.maxValue = healthMax;
            healthBar.value = healthCurrent;
            barColor.color = Color.grey;
        }

        if (difficultyValue == 1)
        {
            //healthMax += healthAdd;
            healthCurrent = healthMax;
            attack.damage *= difficultyValue;

            healthBar.maxValue = healthMax;
            healthBar.value = healthCurrent;
            barColor.color = Color.grey;
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

            lucentPercent *= difficultyValue;
            lucentPercent /= 100;
            lucentPercent *= lucentYield;
            lucentAdd = (int)lucentPercent;
            lucentYield += lucentAdd;

            healthMax += healthAdd;
            healthCurrent = healthMax;
            attack.damage += damageAdd;

            healthBar.maxValue = healthMax;
            healthBar.value = healthCurrent;
            barColor.color = Color.green;
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

            lucentPercent *= difficultyValue;
            lucentPercent /= 100;
            lucentPercent *= lucentYield;
            lucentAdd = (int)lucentPercent;
            lucentYield += lucentAdd;

            healthMax += healthAdd;
            healthCurrent = healthMax;
            attack.damage += damageAdd;

            healthBar.maxValue = healthMax;
            healthBar.value = healthCurrent;
            barColor.color = Color.red;
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

            lucentPercent *= difficultyValue;
            lucentPercent /= 100;
            lucentPercent *= lucentYield;
            lucentAdd = (int)lucentPercent;
            lucentYield += lucentAdd;

            healthMax += healthAdd;
            healthCurrent = healthMax;
            attack.damage += damageAdd;

            healthBar.maxValue = healthMax;
            healthBar.value = healthCurrent;
            barColor.color = Color.yellow;
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

            lucentPercent *= difficultyValue;
            lucentPercent /= 100;
            lucentPercent *= lucentYield;
            lucentAdd = (int)lucentPercent;
            lucentYield += lucentAdd;

            healthMax += healthAdd;
            healthCurrent = healthMax;
            attack.damage += damageAdd;

            healthBar.maxValue = healthMax;
            healthBar.value = healthCurrent;
            barColor.color = Color.cyan;
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

            healthBar.maxValue = healthMax;
            healthBar.value = healthCurrent;
            barColor.color = Color.cyan;
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

        if(healthBar.gameObject.activeInHierarchy != true)
        {
            healthBar.gameObject.SetActive(true);
        }

        if(GetComponent<SDPHealthDebuff>() != null)
        {
            healthCurrent -= (damageHit * GetComponent<SDPHealthDebuff>().damageAmp);
        }

        else
        {
            healthCurrent -= damageHit;
        }

        //healthCurrent -= damageHit;
        healthBar.value = healthCurrent;
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
            if (ammoRewardChance >= 80)
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
            }

            if (berth != null)
            {
                berth.Explode();
            }

            player.lucentFunds += lucentYield;
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
}
