using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{
    public int dropRarity;
    public int lucentYield;
    public float dropThreshold; //A goal number to be at or below in order to spawn Loot
    //This value governs the escalation or de-escalation of Loot drop rates as Rarity increases
    //-Increasing this number increases dropThreshold, spawning Loot drops more frequently
    //-Inversely, turning this value negative will decrease dropThreshold, spawning Loot drops less frequently
    public float dropPercent = 4.25f;

    public float lucentThreshold; //A goal number to be at or below in order to spawn Lucent
    //This value governs the escalation or de-escalation of Lucent drop rates as Difficulty increases (though this Script will increase it through Rarity)
    //-Increasing this number increases lucentThreshold, spawning Lucent drops more frequently
    //-Inversely, turning this value negative decreases lucentThreshold, spawning Lucent drops less frequently
    public float lucentPercent = -4.25f;

    public GameObject loot, exoticLoot, lucent;
    public List<GameObject> combatants = new List<GameObject>();
    private GameObject[] enemies;
    public bool enemyDied = false;

    private float dropThreshReset;
    private int deathRewardChance;
    internal int cadenceDeadCount = 0;
    internal Vector3 slainPosition, woundedPosition;
    internal int killCount = 0;
    internal int damageReceived = 0;
    internal int damageDealt = 0;
    private PlayerInventoryScript player;

    // Start is called before the first frame update
    void Start()
    {
        dropThreshReset = dropThreshold;

        player = FindObjectOfType<PlayerInventoryScript>();
        enemyDied = false;
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        RarityCheck();
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemyDied)
        {
            //cadenceDeadCount++;
            killCount++;
            enemyDied = false;
            //StartCoroutine(EnemyDiedReset());
        }
    }

    void RarityCheck()
    {
        if (dropRarity <= 0)
        {
            dropRarity = 1;
        }

        if(dropRarity >= 2)
        {
            if (dropThreshold >= 100)
            {
                dropThreshold = 100;
            }

            else if (dropThreshold <= 0)
            {
                dropThreshold = 0;
            }

            else
            {
                dropPercent *= dropRarity;
                dropPercent /= 100;
                dropPercent *= dropThreshold;
                dropPercent = Mathf.Round(dropPercent);
                dropThreshold += dropPercent;
            }
            
            if (lucentThreshold >= 100)
            {
                lucentThreshold = 100;
            }

            else if (lucentThreshold <= 0)
            {
                lucentThreshold = 0;
            }

            else
            {
                lucentPercent *= dropRarity;
                lucentPercent /= 100;
                lucentPercent *= lucentThreshold;
                lucentPercent = Mathf.Round(lucentPercent);
                lucentThreshold += lucentPercent;
            }          
        }
        
        
        if (dropRarity >= 6)
        {
            dropRarity = 5;
        }
    }   

    public void DeathReward(Vector3 deathPos)
    {
        //For Loot
        if(loot.GetComponent<LootScript>() != null)
        {
            loot.GetComponent<LootScript>().raritySpawn = dropRarity;
        }

        deathRewardChance = Random.Range(0, 101);
        //Debug.Log(deathRewardChance + "|" + dropThreshold);

        if (deathRewardChance <= dropThreshold)
        {
            GameObject reward = Instantiate(loot, deathPos, loot.transform.rotation);
            if (dropRarity == 1)
            {
                reward.GetComponent<Renderer>().material.color = Color.gray;
            }

            else if (dropRarity == 2)
            {
                reward.GetComponent<Renderer>().material.color = Color.green;
            }

            else if (dropRarity == 3)
            {
                reward.GetComponent<Renderer>().material.color = Color.red;
            }

            else if (dropRarity == 4)
            {
                reward.GetComponent<Renderer>().material.color = Color.yellow;
            }

            else if (dropRarity == 5)
            {
                reward.GetComponent<Renderer>().material.color = Color.cyan;
            }

            //Removes (Clone) from name
            reward.name = loot.name;
        }

        //For Exotic Loot
        if(dropRarity >= 4)
        {
            deathRewardChance = Random.Range(0, 103);
            if (deathRewardChance >= 100)
            {
                GameObject reward = Instantiate(exoticLoot, deathPos, loot.transform.rotation);
                if(reward.GetComponent<ColorLerpScript>() != null)
                {
                    reward.GetComponent<ColorLerpScript>().colorOne = Color.cyan;
                    reward.GetComponent<ColorLerpScript>().colorTwo = Color.white;
                }

                //Removes (Clone) from name
                reward.name = loot.name;
            }
        }
        
        //For Lucent
        deathRewardChance = Random.Range(0, 101);
        //Debug.Log(deathRewardChance + "|" + lucentThreshold);

        if (deathRewardChance <= lucentThreshold)
        {
            GameObject rewardTwo = Instantiate(lucent, deathPos, loot.transform.rotation);
            rewardTwo.GetComponent<LucentScript>().lucentGift *= dropRarity;
            rewardTwo.GetComponent<LucentScript>().ShatterCalculation();

            //Removes (Clone) from name
            rewardTwo.name = loot.name;
        }
    }

    //The Following four methods are to help Cadence determine where to spawn Lucent clusters.
    public void CadenceRewardPosition(Vector3 deathPos)
    {
        slainPosition = deathPos;
    }

    public void CadenceReward()
    {
        GameObject rewardTwo = Instantiate(lucent, slainPosition, loot.transform.rotation);
        rewardTwo.GetComponent<LucentScript>().lucentGift *= dropRarity;
        rewardTwo.GetComponent<LucentScript>().ShatterCalculation();
        rewardTwo.name = loot.name;
    }

    public void FatedCadenceRewardPosition(Vector3 alivePos)
    {
        woundedPosition = alivePos;
    }
    
    public void FatedCadenceReward(Vector3 shotPosition)
    {
        GameObject rewardTwo = Instantiate(lucent, shotPosition, loot.transform.rotation);
        rewardTwo.GetComponent<LucentScript>().lucentGift *= dropRarity;
        rewardTwo.GetComponent<LucentScript>().ShatterCalculation();
        rewardTwo.name = loot.name;
    }
   
    public void CatalogEnemies()
    {
        combatants.Clear();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            combatants.Add(enemy);
        }
    }

    public void RemoveEnemies()
    {       
        for (int e = 0; e < combatants.Count; e++)
        {
            if (combatants[e].GetComponent<EnemyHealthScript>().healthCurrent <= 0)
            {
                combatants.Remove(combatants[e]);
                
            }
        }
    }
}
