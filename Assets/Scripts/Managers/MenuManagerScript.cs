using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerScript : MonoBehaviour
{
    //Configuration for use on Main Menu or Gameplay
    public enum Setting
    {
        Menu = 0, Game = 1
    }

    public Setting setting;

    //ca, vcThumbnail - Image UI that displays Campaign, Viricide level pictures
    public Image caThumbnail, vcThumbnail;
    public Sprite caLevelOne, caLevelTwo, caLevelThree, vcLevelOne, vcLevelTwo; //Images of Level thumbnails
    public Text caDiffText, vcDiffText, caLevelText, vcLevelText, vcWepFocusText; //Texts that displays difficulty number, level name, or Weapon focus
    public Button vcButton; //Viricide navigation button
    public Slider vcDifficulty, vcLevel, vcWepFocus, caDifficulty, caLevel; //Sliders used to select Weapon type, level, or difficulty
    private LevelManagerScript levelManager;
    private WeaponManagerScript weaponManager;

    private void Awake()
    {
        if(setting == Setting.Menu)
        {
            levelManager = FindObjectOfType<LevelManagerScript>();
            weaponManager = FindObjectOfType<WeaponManagerScript>();
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (setting == Setting.Menu)
        {
            Progression();
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (setting == Setting.Menu)
        {
            //Changes Campaign/Viricide thumbnail image by slider value
            if (caLevel.value == 1)
            {
                caThumbnail.sprite = caLevelOne;
            }

            else if (caLevel.value == 2)
            {
                caThumbnail.sprite = caLevelTwo;
            }

            else if (caLevel.value == 3)
            {
                caThumbnail.sprite = caLevelThree;
            }


            if (vcLevel.value == 4)
            {
                vcThumbnail.sprite = vcLevelOne;
            }

            else if (vcLevel.value == 5)
            {
                vcThumbnail.sprite = vcLevelTwo;
            }

            //Changes Campaign/Viricide difficulty, level text by slider value
            caDiffText.text = "Difficulty: " + caDifficulty.value;
            vcDiffText.text = "Difficulty: " + vcDifficulty.value;

            caLevelText.text = "Level " + caLevel.value;
            vcLevelText.text = "Viricide: " + (vcLevel.value - 3);

            //Changes Weapon Focus text by slider value
            if(vcWepFocus.value == -1)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Random";
            }

            else if (vcWepFocus.value == 0)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Full Fire Rifles";
            }

            else if (vcWepFocus.value == 1)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Machine Guns";
            }

            else if (vcWepFocus.value == 2)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Pistols";
            }

            else if (vcWepFocus.value == 3)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Semi Fire Rifles";
            }

            else if (vcWepFocus.value == 4)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Shotguns";
            }

            else if (vcWepFocus.value == 5)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Single Fire Rifles";
            }

            else if (vcWepFocus.value == 6)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Submachine Guns";
            }
        }    
    }

    /// <summary>
    /// Initializes Viricide game dependent on slider values and loads scene
    /// </summary>
    public void InitializeViricideGame()
    {
        levelManager.setting = LevelManagerScript.Setting.Viricide;
        levelManager.gameSettingState = (int)vcDifficulty.value;
        levelManager.level = (int)vcLevel.value;
        levelManager.weaponFocus = (int)vcWepFocus.value;

        levelManager.LoadScene();
    }

    /// <summary>
    /// Initializes Campaign game dependent on slider values and loads scene
    /// </summary>
    public void InitializeCampaignGame()
    {
        levelManager.setting = LevelManagerScript.Setting.Campaign;
        levelManager.gameSettingState = (int)caDifficulty.value;
        levelManager.level = (int)caLevel.value;

        levelManager.LoadScene();

    }

    public void OpenPage(GameObject page)
    {
        page.gameObject.SetActive(true);
    }

    public void ClosePage(GameObject page)
    {
        page.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Configures slider max values by PlayerPref values
    /// Changes Viricide button interactivity by PlayerPref value
    /// </summary>
    public void Progression()
    {
        if (PlayerPrefs.GetInt("unlockLevel03") == 1)
        {
            caLevel.maxValue = 3;
        }

        else if(PlayerPrefs.GetInt("unlockLevel02") == 1)
        {
            caLevel.maxValue = 2;
        }

        else
        {
            caLevel.maxValue = 1;
        }

        if (PlayerPrefs.GetInt("unlockDifficulty5") == 1)
        {
            caDifficulty.maxValue = 5;
            vcDifficulty.maxValue = 5;
        }

        else
        {
            caDifficulty.maxValue = 4;
            vcDifficulty.maxValue = 4;
        }

        if (PlayerPrefs.GetInt("unlockViricide") == 1)
        {
            vcButton.interactable = true;
        }

        else
        {
            vcButton.interactable = false;
        }
    }
}
