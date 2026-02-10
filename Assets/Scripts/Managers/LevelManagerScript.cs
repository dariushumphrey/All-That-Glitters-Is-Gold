using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{
    static LevelManagerScript instance = null;

    //Configuration for Main Menu, Campaign, or Viricide behavior
    public enum Setting
    {
        Navigation = 0, Campaign = 1, Viricide = 2
    }

    public Setting setting;
    public int gameSettingState = 1; //Determines Loot rarity and Enemy difficulty. 

    public PlayerStatusScript player;
    public EnemyManagerScript manager;

    public GameObject[] chests; //Array of Chest items
    public GameObject[] spawners; //Array of Enemy spawners
    public int weaponFocus = -1; //Value that forces Loot items to spawn specific Weapon types

    public int level = 0; //Current level
    public float gameTime = 0f;
    public GameObject eogStatsText; //Displays game end statistics
    public GameObject resultsNotice; //Informs player of imminent results display

    //resultsMenu - Menu UI that houses game end statistics
    public GameObject pauseMenu, resultsMenu, controlsMenu;

    private TransitionManagerScript transition;
    private CheckpointManagerScript checkpoint;
    private float gameEndDelay = 10f;
    private float gameRetryDelay = 5f;
    private bool paused = false; //Zeroes game time if true
    private GameObject continueButton, restartButton, quitButton, mainMenuButton;
    private GameObject menuReturnButton;
    internal bool gameComplete = false; //Certifies game completion if true
    internal AsyncOperation async;
    internal Text levelLoadText; //Text that displays Async level load progress
    internal bool lvlProgressSaved = false;

    // Start is called before the first frame update
    void Start()
    {
        //Destroys duplicate Level Managers when navigating scenes
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Duplicated level manager self-destructed!");
        }

        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);

            //Resets game settings when on Main Menu
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

                levelLoadText = GameObject.Find("asyncLoadText").GetComponent<Text>();
                levelLoadText.text = "";

                return;
            }

            else
            {
                GameSet();
            }
        }
    }

    /// <summary>
    /// Initializes Player, Enemy, Loot quality, and finds UI elements
    /// </summary>
    void GameSet()
    {
        player = FindObjectOfType<PlayerStatusScript>();
        player.playerScaling = gameSettingState;
        player.StatusCorrections();
        player.StatusScaling();

        manager = FindObjectOfType<EnemyManagerScript>();
        manager.dropRarity = gameSettingState;
        manager.lootFocus = weaponFocus;

        transition = FindObjectOfType<TransitionManagerScript>();
        levelLoadText = GameObject.Find("asyncLoadText").GetComponent<Text>();
        levelLoadText.text = "";

        checkpoint = FindObjectOfType<CheckpointManagerScript>();

        if(lvlProgressSaved)
        {
            checkpoint.checkpointReached = true;
        }

        chests = GameObject.FindGameObjectsWithTag("Chest");
        for(int c = 0; c < chests.Length; c++)
        {
            if(gameSettingState >= 4)
            {
                chests[c].GetComponent<LootScript>().raritySpawn = 4;
            }

            else
            {
                chests[c].GetComponent<LootScript>().raritySpawn = gameSettingState;
            }

            chests[c].GetComponent<LootScript>().focusTarget = weaponFocus;
            //chests[c].GetComponent<LootScript>().SpawnDrop();
        }

        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        for(int s = 0; s < spawners.Length; s++)
        {
            spawners[s].GetComponent<SpawnerScript>().difficultySpawn = gameSettingState;
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

        if(setting == Setting.Viricide)
        {
            resultsNotice = GameObject.Find("resultsNotice");
            resultsNotice.gameObject.GetComponent<Text>().text = "";
        }

        menuReturnButton = GameObject.Find("menuReturnButton");
        menuReturnButton.GetComponent<Button>().onClick.AddListener(ReturnToMainMenu);
        resultsMenu.gameObject.SetActive(false);      

        //Resumes game if game was paused
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        //Permanently unlocks Cursor when on Main Menu
        if(setting == Setting.Navigation)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        else
        {
            if (player != null)
            {
                //Saves Inventory and reloads scene when Player is defeated
                if(player.isDead)
                {
                    gameRetryDelay -= Time.deltaTime;
                    if(gameRetryDelay <= 0f)
                    {
                        gameRetryDelay = 5f;
                        player.GetComponent<PlayerInventoryScript>().WriteOnReset();
                        PlayerPrefs.SetInt("lucentBalance", player.GetComponent<PlayerInventoryScript>().lucentFunds);

                        LoadScene();
                    }

                    //if (Input.GetKeyDown(KeyCode.F))
                    //{
                    //    player.GetComponent<PlayerInventoryScript>().WriteOnReset();
                    //    LoadScene();
                    //}
                }               
            }

            if(!gameComplete)
            {
                gameTime += Time.deltaTime;
            }

            weaponFocus = Mathf.Clamp(weaponFocus, -1, 6); //Locks weaponFocus within a range to prevent incorrect focusing

            //Pauses game if game is incomplete & controls page is hidden
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

            //Produces game results on a delay (Viricide)
            if(gameComplete && setting == Setting.Viricide)
            {
                gameEndDelay -= Time.deltaTime;
                resultsNotice.gameObject.GetComponent<Text>().text = "Results in " + gameEndDelay.ToString("F0") + "s or [Tab]";

                if(gameEndDelay <= 0f || Input.GetKeyDown(KeyCode.Tab))
                {
                    gameEndDelay = 0f;
                    //Time.timeScale = 0;
                    if (pauseMenu.gameObject.activeSelf != false)
                    {
                        pauseMenu.gameObject.SetActive(false);
                    }

                    //Cursor.lockState = CursorLockMode.None;
                    
                    float minutes = Mathf.FloorToInt(gameTime / 60);
                    float seconds = Mathf.FloorToInt(gameTime % 60);
                    //paused = true;
                    resultsMenu.gameObject.SetActive(true);
                    menuReturnButton.gameObject.SetActive(false);
                    eogStatsText.gameObject.GetComponent<Text>().text = "Viricide Accomplished:" + "\n" +
                        "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds) + "\n" +
                        "Kills: " + manager.killCount + "\n" +
                        "Damage Dealt: " + manager.damageReceived.ToString("N0") + "\n" +
                        "Damage Received: " + manager.damageDealt.ToString("N0");

                    resultsNotice.gameObject.GetComponent<Text>().text = "";

                }
            }

            else if (gameComplete && setting == Setting.Campaign)
            {
                //Time.timeScale = 0;
                if (pauseMenu.gameObject.activeSelf != false)
                {
                    pauseMenu.gameObject.SetActive(false);
                }

                //Cursor.lockState = CursorLockMode.None;

                float minutes = Mathf.FloorToInt(gameTime / 60);
                float seconds = Mathf.FloorToInt(gameTime % 60);
                //paused = true;
                resultsMenu.gameObject.SetActive(true);
                menuReturnButton.gameObject.SetActive(false);
                eogStatsText.gameObject.GetComponent<Text>().text = "Level 0" + (level - 1) + " Cleared:" + "\n" +
                    "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds) + "\n" +
                    "Kills: " + manager.killCount + "\n" +
                    "Damage Dealt: " + manager.damageReceived.ToString("N0") + "\n" +
                    "Damage Received: " + manager.damageDealt.ToString("N0");
            }
        }     
    }

    /// <summary>
    /// Sets game state and loads specified scene
    /// </summary>
    public void LoadScene()
    {
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

    IEnumerator LoadSceneAsync()
    {
        async = SceneManager.LoadSceneAsync(level);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            levelLoadText.text = "Loading: " + async.progress * 100 + "%";

            if (async.progress >= 0.9f)
            {
                StartCoroutine(LoadAsyncedScene());
            }

            yield return null;

        }
    }

    IEnumerator LoadAsyncedScene()
    {
        yield return new WaitForSeconds(3f);

        if(gameComplete)
        {
            gameComplete = false;
            lvlProgressSaved = false;
            if(level == 0)
            {
                setting = Setting.Navigation;
            }
        }

        async.allowSceneActivation = true;
    }

    public IEnumerator LoadAsyncedSceneDelay()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(LoadSceneAsync());
    }

    public void StartAsyncSceneLoad()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private void OnLevelWasLoaded(int level)
    {
        Start();
    }
    
    /// <summary>
    /// Grants Viricide access if previously inaccessible
    /// </summary>
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

    /// <summary>
    /// Saves Player Inventory
    /// </summary>
    public void SaveInventory()
    {
        player.GetComponent<PlayerInventoryScript>().WriteOnReset();
    }

    /// <summary>
    /// Saves Player Inventory, Lucent balance and loads scene
    /// </summary>
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

    /// <summary>
    /// Loads Main menu after saving Player Inventory, Lucent balance
    /// Configures Level Manager for use on Main Menu
    /// </summary>
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

        if (pauseMenu.gameObject.activeSelf != false)
        {
            pauseMenu.gameObject.SetActive(false);
        }

        transition.fadeToBlack = true;
        StartAsyncSceneLoad();
        //LoadScene();
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
}
