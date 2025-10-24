using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{
    static LevelManagerScript instance = null;

    public enum Setting
    {
        Navigation = 0, Campaign = 1, Viricide = 2
    }

    public Setting setting;
    //Determines Loot rarity and Enemy difficulty. 
    public int gameSettingState = 1;

    public PlayerStatusScript player;
    public EnemyManagerScript manager;

    public GameObject[] chests;
    public GameObject[] spawners;
    public GameObject kioskAdjust;
    public int weaponFocus = -1;

    public int level = 0;
    public float gameTime = 0f;
    public GameObject eogStatsText;
    public GameObject pauseMenu, resultsMenu, controlsMenu;

    private float gameEndDelay = 10f;
    private bool paused = false;
    private GameObject continueButton, restartButton, quitButton, mainMenuButton;
    private GameObject menuReturnButton;
    internal bool gameComplete = false;

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

            if(setting == Setting.Navigation)
            {
                if(gameComplete)
                {
                    gameComplete = false;
                }

                gameTime = 0f;
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                }
                return;
            }

            else
            {
                GameSet();
            }
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
        manager.lootFocus = weaponFocus;

        chests = GameObject.FindGameObjectsWithTag("Chest");
        for(int c = 0; c < chests.Length; c++)
        {
            chests[c].GetComponent<LootScript>().raritySpawn = gameSettingState;
            chests[c].GetComponent<LootScript>().focusTarget = weaponFocus;
            //chests[c].GetComponent<LootScript>().SpawnDrop();
        }

        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        for(int s = 0; s < spawners.Length; s++)
        {
            spawners[s].GetComponent<SpawnerScript>().difficultySpawn = gameSettingState;
        }

        kioskAdjust = GameObject.FindGameObjectWithTag("Kiosk");
        if(kioskAdjust != null)
        {
            kioskAdjust.GetComponent<LootScript>().raritySpawn = gameSettingState;
            kioskAdjust.GetComponent<LootScript>().PriceAdjust();
        }

        pauseMenu = GameObject.Find("pauseBG");
        continueButton = GameObject.Find("continueButton");
        continueButton.GetComponent<Button>().onClick.AddListener(ResumeGame);

        restartButton = GameObject.Find("restartButton");
        restartButton.GetComponent<Button>().onClick.AddListener(RestartGame);

        quitButton = GameObject.Find("gameQuitButton");
        quitButton.GetComponent<Button>().onClick.AddListener(QuitGame);

        mainMenuButton = GameObject.Find("menuQuitButton");
        mainMenuButton.GetComponent<Button>().onClick.AddListener(ReturnToMainMenu);

        controlsMenu = GameObject.Find("controlsPage");
        controlsMenu.gameObject.SetActive(false);

        pauseMenu.gameObject.SetActive(false);

        resultsMenu = GameObject.Find("completeBG");
        eogStatsText = GameObject.Find("completeText");
        menuReturnButton = GameObject.Find("menuReturnButton");
        menuReturnButton.GetComponent<Button>().onClick.AddListener(ReturnToMainMenu);
        resultsMenu.gameObject.SetActive(false);      

        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        if(setting == Setting.Navigation)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        else
        {
            if (player != null)
            {
                if(player.isDead)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        player.GetComponent<PlayerInventoryScript>().WriteOnReset();
                        LoadScene();
                    }
                }               
            }

            gameTime += Time.deltaTime;
            weaponFocus = Mathf.Clamp(weaponFocus, -1, 6);
            //missionTimerText.gameObject.SetActive(false);
            //DisplayTimer(gametype.gameTimer);
            //missionTimerText.gameObject.SetActive(false);
            //missionTimerText = GameObject.Find("MissionTimer");

            if (Input.GetKeyDown(KeyCode.Escape) && !gameComplete && controlsMenu.activeInHierarchy == false)
            {
                if (player.isDead)
                {
                    return;
                }

                if (!paused)
                {
                    Time.timeScale = 0;
                    if (pauseMenu.gameObject.activeSelf == false)
                    {
                        pauseMenu.gameObject.SetActive(true);
                    }
                    paused = true;
                }

                else if (paused)
                {
                    Time.timeScale = 1;
                    paused = false;
                    if (pauseMenu.gameObject.activeSelf != false)
                    {
                        pauseMenu.gameObject.SetActive(false);
                    }
                }
            }

            if(gameComplete)
            {
                gameEndDelay -= Time.deltaTime;
                if(gameEndDelay <= 0f)
                {
                    gameEndDelay = 10f;
                    Time.timeScale = 0;
                    if (pauseMenu.gameObject.activeSelf != false)
                    {
                        pauseMenu.gameObject.SetActive(false);
                    }

                    Cursor.lockState = CursorLockMode.None;
                    
                    float minutes = Mathf.FloorToInt(gameTime / 60);
                    float seconds = Mathf.FloorToInt(gameTime % 60);
                    //paused = true;
                    resultsMenu.gameObject.SetActive(true);
                    eogStatsText.gameObject.GetComponent<Text>().text = "Viricide Accomplished:" + "\n" +
                        "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds) + "\n" +
                        "Kills: " + manager.killCount + "\n" +
                        "Damage Dealt: " + manager.damageReceived.ToString("N0") + "\n" +
                        "Damage Received: " + manager.damageDealt.ToString("N0");
                    
                }
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

        SceneManager.LoadScene(level);
    }

    private void OnLevelWasLoaded(int level)
    {
        Start();
    }
    
    public void CheckForFirstViricideClear()
    {
        if (PlayerPrefs.GetInt("firstViricideClear") == 1)
        {
            return;
        }

        else
        {
            PlayerPrefs.SetInt("firstViricideClear", 1);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        VanishPauseMenu();
    }

    public void SaveInventory()
    {
        player.GetComponent<PlayerInventoryScript>().WriteOnReset();
    }

    public void RestartGame()
    {
        player.GetComponent<PlayerInventoryScript>().WriteOnReset();
        PlayerPrefs.SetInt("lucentBalance", player.GetComponent<PlayerInventoryScript>().lucentFunds);
        LoadScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        level = 0;
        setting = Setting.Navigation;
        player.GetComponent<PlayerInventoryScript>().WriteOnReset();
        PlayerPrefs.SetInt("lucentBalance", player.GetComponent<PlayerInventoryScript>().lucentFunds);

        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }

        LoadScene();
    }

    private void VanishPauseMenu()
    {
        pauseMenu.gameObject.SetActive(false);
        paused = false;
    }

    public void OpenPage(GameObject page)
    {
        page.gameObject.SetActive(true);
    }

    public void ClosePage(GameObject page)
    {
        page.gameObject.SetActive(false);
    }

    //void DisplayTimer(float timer)
    //{
    //    float minutes = Mathf.FloorToInt(timer / 60);
    //    float seconds = Mathf.FloorToInt(timer % 60);
    //    missionTimerText.GetComponent<Text>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
    //}
}
