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
    //-Inversely, turning this value negative decreases dropThreshold, spawning Lucent drops less frequently
    public float lucentPercent = -4.25f;

    public GameObject loot, exoticLoot, lucent;
    public GameObject[] enemies;
    public bool enemyDied = false;

    private float dropThreshReset;
    private int deathRewardChance;
    internal int cadenceDeadCount = 0;
    internal Vector3 slainPosition, woundedPosition;
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

        if (enemyDied == true)
        {
            //cadenceDeadCount++;
            enemyDied = false;
            //StartCoroutine(EnemyDiedReset());
        }

        for (int e = 0; e < enemies.Length; e++)
        {
            if (enemies[e].GetComponent<EnemyHealthScript>().healthCurrent <= 0)
            {
                enemyDied = true;
                //if (enemies[e].GetComponent<EnemyHealthScript>().isDummy == true)
                //{
                //    //return;
                //}

                //enemies[e] = null;            
                StartCoroutine(EnemyDiedReset());
                //DeathReward();              
            }           
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

    public IEnumerator EnemyDiedReset()
    {
        yield return new WaitForSeconds(0.001f);
        enemyDied = false;
    }
}
