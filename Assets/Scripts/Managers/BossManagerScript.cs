using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossManagerScript : MonoBehaviour
{
    public int bossDifficulty = 1;
    public List<GameObject> addSpawners = new List<GameObject>();
    public List<GameObject> chestRewards = new List<GameObject>();
    internal bool isAlive = true;
    private EnemyManagerScript enemyManager;
    private LevelManagerScript levelManager;
    private bool done = false;
  
    void Awake()
    {
        enemyManager = FindObjectOfType<EnemyManagerScript>();
        levelManager = FindObjectOfType<LevelManagerScript>();

        for (int c = 0; c < chestRewards.Count; c++)
        {
            chestRewards[c].gameObject.SetActive(false);            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bossDifficulty = enemyManager.dropRarity;
        for (int c = 0; c < chestRewards.Count; c++)
        {
            chestRewards[c].GetComponent<LootScript>().raritySpawn = bossDifficulty;
            chestRewards[c].GetComponent<LootScript>().lootSpamMax = bossDifficulty;          
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive && !done)
        {
            StartCoroutine(RewardOnDelay());
            done = true;

        }
    }

    public void TriggerAdds()
    {
        for(int a = 0; a < addSpawners.Count; a++)
        {
            for(int r = 0; r < addSpawners[a].GetComponent<SpawnerScript>().spawnRepeat; r++)
            {
                addSpawners[a].GetComponent<SpawnerScript>().SpawnObject();
            }
        }
    }
    
    private IEnumerator RewardOnDelay()
    {
        yield return new WaitForSeconds(2f);
        for (int c = 0; c < chestRewards.Count; c++)
        {
            chestRewards[c].gameObject.SetActive(true);
            chestRewards[c].GetComponent<LootScript>().raritySpawn = bossDifficulty;
            if (chestRewards[c].GetComponent<LootScript>().spamLoot != true)
            {
                chestRewards[c].GetComponent<LootScript>().lootSpamMax = bossDifficulty;
                chestRewards[c].GetComponent<LootScript>().spamLoot = true;
            }
        }

        levelManager.gameComplete = true;
        levelManager.CheckForFirstViricideClear();
    }
}
