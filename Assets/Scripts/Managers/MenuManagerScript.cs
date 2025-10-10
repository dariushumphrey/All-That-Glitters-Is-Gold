using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerScript : MonoBehaviour
{
    public enum Setting
    {
        Menu = 0, Game = 1
    }

    public Setting setting;

    public Image caThumbnail, vcThumbnail;
    public Sprite caLevelOne, caLevelTwo, caLevelThree, vcLevelOne, vcLevelTwo;
    public Text caDiffText, vcDiffText, caLevelText, vcLevelText;
    public Button vcButton;
    public Slider vcDifficulty, vcLevel, caDifficulty, caLevel;
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

            caDiffText.text = "Difficulty: " + caDifficulty.value;
            vcDiffText.text = "Difficulty: " + vcDifficulty.value;

            caLevelText.text = "Level " + caLevel.value;
            vcLevelText.text = "Viricide: " + (vcLevel.value - 3);
        }
        
    }

    public void InitializeViricideGame()
    {
        levelManager.setting = LevelManagerScript.Setting.Viricide;
        levelManager.gameSettingState = (int)vcDifficulty.value;
        levelManager.level = (int)vcLevel.value;

        levelManager.LoadScene();
    }

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
