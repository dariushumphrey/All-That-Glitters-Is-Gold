using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerScript : MonoBehaviour
{
    public Image caThumbnail, vcThumbnail;
    public Sprite caLevelOne, caLevelTwo, vcLevelOne;
    public Text caDiffText, vcDiffText, caLevelText, vcLevelText;
    public Button vcButton;
    public Slider vcDifficulty, vcLevel, caDifficulty, caLevel;
    private LevelManagerScript levelManager;
    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManagerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Progression();
    }

    // Update is called once per frame
    void Update()
    {
        if(caLevel.value == 1)
        {
            caThumbnail.sprite = caLevelOne;
        }

        else if (caLevel.value == 2)
        {
            caThumbnail.sprite = caLevelTwo;
        }


        if (vcLevel.value == 1)
        {
            vcThumbnail.sprite = vcLevelOne;
        }

        caDiffText.text = "Difficulty: " + caDifficulty.value;
        vcDiffText.text = "Difficulty: " + vcDifficulty.value;

        caLevelText.text = "Level " + caLevel.value;
        vcLevelText.text = "Viricide: " + (vcLevel.value - 2);
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
        if (PlayerPrefs.GetInt("unlockLevel02") == 1)
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
