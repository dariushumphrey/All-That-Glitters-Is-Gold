using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelManagerScript : MonoBehaviour
{
    static LevelManagerScript instance = null;

    //Determines Loot rarity and Enemy difficulty. 
    public int gameSettingState = 1;

    public PlayerStatusScript player;
    public EnemyManagerScript manager;

    public GameObject[] chests;
    public GameObject[] spawners;
    public GameObject kioskAdjust;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Duplicated level manager self-destructed!");
        }

        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
            GameSet();
        }
    }

    void GameSet()
    {
        player = FindObjectOfType<PlayerStatusScript>();
        player.playerScaling = gameSettingState;
        player.StatusCorrections();
        player.StatusScaling();

        manager = FindObjectOfType<EnemyManagerScript>();
        manager.dropRarity = gameSettingState;

        chests = GameObject.FindGameObjectsWithTag("Chest");
        for(int c = 0; c < chests.Length; c++)
        {
            chests[c].GetComponent<LootScript>().raritySpawn = gameSettingState;
            chests[c].GetComponent<LootScript>().SpawnDrop();

        }

        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        for(int s = 0; s < spawners.Length; s++)
        {
            spawners[s].GetComponent<SpawnerScript>().difficultySpawn = gameSettingState;
        }

        kioskAdjust = GameObject.FindGameObjectWithTag("Kiosk");
        {
            kioskAdjust.GetComponent<LootScript>().raritySpawn = gameSettingState;
            kioskAdjust.GetComponent<LootScript>().PriceAdjust();

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                player.GetComponent<PlayerInventoryScript>().WriteOnReset();
                LoadScene();
            }
        }

        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    gameSettingState++;
        //    LoadScene();
        //}
    }

    public void LoadScene()
    {
        //Start();

        if (gameSettingState <= 0)
        {
            gameSettingState = 1;
        }

        if(gameSettingState >= 6)
        {
            gameSettingState = 5;
        }

        SceneManager.LoadScene(0);
    }

    private void OnLevelWasLoaded(int level)
    {
        Start();
    }
}
