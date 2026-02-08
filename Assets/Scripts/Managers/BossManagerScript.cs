using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossManagerScript : MonoBehaviour
{
    public int bossDifficulty = 1;
    public List<GameObject> addSpawners = new List<GameObject>(); //List of Enemy spawners
    public List<GameObject> chestRewards = new List<GameObject>(); //List of Chest rewards
    public List<GameObject> lockedDoors = new List<GameObject>();
    internal bool isAlive = true;
    private EnemyManagerScript enemyManager;
    private LevelManagerScript levelManager;
    private bool done = false; //Commits an action once if true
  
    void Awake()
    {
        enemyManager = FindObjectOfType<EnemyManagerScript>();
        levelManager = FindObjectOfType<LevelManagerScript>();

        for (int c = 0; c < chestRewards.Count; c++)
        {
            chestRewards[c].gameObject.SetActive(false);            
        }

        if(lockedDoors.Count >= 1)
        {
            for (int d = 0; d < lockedDoors.Count; d++)
            {
                lockedDoors[d].gameObject.GetComponent<DoorScript>().locked = true;
            }
        }     
    }

    // Start is called before the first frame update
    void Start()
    {
        //Assigns Boss difficulty from Enemy manager, initializes Chest rarity, Weapon focus
        bossDifficulty = enemyManager.dropRarity;
        for (int c = 0; c < chestRewards.Count; c++)
        {
            chestRewards[c].GetComponent<LootScript>().raritySpawn = bossDifficulty;
            chestRewards[c].GetComponent<LootScript>().lootSpamMax = bossDifficulty;
            chestRewards[c].GetComponent<LootScript>().focusTarget = levelManager.weaponFocus;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Reveals Rewards when Boss is defeated
        if(!isAlive && !done)
        {
            StartCoroutine(RewardOnDelay());
            done = true;

        }
    }

    /// <summary>
    /// Invokes Spawners to create Enemies a certain number of times
    /// </summary>
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
    
    /// <summary>
    /// Reveals Chest rewards on a delay
    /// Informs LevelManager that game is complete, grants Viricide access if first clear
    /// </summary>
    private IEnumerator RewardOnDelay()
    {
        yield return new WaitForSeconds(2f);
        for (int c = 0; c < chestRewards.Count; c++)
        {
            chestRewards[c].gameObject.SetActive(true);
            chestRewards[c].GetComponent<LootScript>().raritySpawn = bossDifficulty;
            chestRewards[c].GetComponent<LootScript>().focusTarget = levelManager.weaponFocus;
            if (chestRewards[c].GetComponent<LootScript>().spamLoot != true)
            {
                chestRewards[c].GetComponent<LootScript>().lootSpamMax = bossDifficulty;
                chestRewards[c].GetComponent<LootScript>().spamLoot = true;
            }
        }

        if (lockedDoors.Count >= 1)
        {
            for (int d = 0; d < lockedDoors.Count; d++)
            {
                lockedDoors[d].gameObject.GetComponent<DoorScript>().locked = false;
            }
        }

        if (levelManager.setting == LevelManagerScript.Setting.Viricide)
        {
            levelManager.gameComplete = true;
            levelManager.CheckForFirstViricideClear();

            StartCoroutine(GameEndDelay());
        }
    }

    private IEnumerator GameEndDelay()
    {
        yield return new WaitForSeconds(15f);
        levelManager.ReturnToMainMenu();
    }
}
