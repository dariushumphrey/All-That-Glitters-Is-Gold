using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceScript : MonoBehaviour
{
    public int levelIndex = 0; //Represents Scene to load
    public bool incomingMenu; //Returns the Player to Main Menu if true
    public TransitionManagerScript transition;

    private LevelManagerScript level;
    private PlayerInventoryScript player;
    // Start is called before the first frame update
    void Start()
    {
        level = FindObjectOfType<LevelManagerScript>();
        player = FindObjectOfType<PlayerInventoryScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {      
        if(other.gameObject.tag == "Player")
        {
            if(incomingMenu)
            {
                if (level.setting == LevelManagerScript.Setting.Campaign)
                {
                    CheckForLevelEntitlement();
                    CheckForDifficultyEntitlement();
                    CheckForViricideEntitlement();
                }

                PlayerPrefs.SetInt("lucentBalance", player.lucentFunds);

                level.lvlProgressSaved = false;

                level.gameComplete = true;
                level.ReturnToMainMenu();
            }

            else
            {
                if(level.setting == LevelManagerScript.Setting.Campaign)
                {
                    CheckForLevelEntitlement();
                }

                PlayerPrefs.SetInt("lucentBalance", player.lucentFunds);

                level.level = levelIndex;
                level.SaveInventory();

                level.lvlProgressSaved = false;

                level.gameComplete = true;
                level.StartCoroutine(level.LoadAsyncedSceneDelay());
                //level.LoadScene();
            }

            transition.StartCoroutine(transition.GameToBlackFadeDelay());
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Unlocks access to Level 02, Level 03 from the Main Menu upon first completion of respective levels.
    /// </summary>
    void CheckForLevelEntitlement()
    {
        if(levelIndex == 2)
        {
            if (PlayerPrefs.GetInt("unlockLevel02") == 1)
            {
                //Do nothing
            }

            else
            {
                PlayerPrefs.SetInt("unlockLevel02", 1);
            }
        }

        if (levelIndex == 3)
        {
            if (PlayerPrefs.GetInt("unlockLevel03") == 1)
            {
                //Do nothing
            }

            else
            {
                PlayerPrefs.SetInt("unlockLevel03", 1);
            }
        }
    }

    /// <summary>
    /// Unlocks Difficulty 5 if not previously unlocked
    /// </summary>
    void CheckForDifficultyEntitlement()
    {
        if (PlayerPrefs.GetInt("unlockDifficulty5") == 1)
        {
            return;
        }

        else
        {
            PlayerPrefs.SetInt("unlockDifficulty5", 1);
        }
    }

    /// <summary>
    /// Unlocks Viricide if not previously unlocked
    /// </summary>
    void CheckForViricideEntitlement()
    {
        if (PlayerPrefs.GetInt("unlockViricide") == 1)
        {
            return;
        }

        else
        {
            PlayerPrefs.SetInt("unlockViricide", 1);
        }
    }
}
