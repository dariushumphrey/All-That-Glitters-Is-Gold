using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wilberforce;

public class PlayerPrefsManagerScript : MonoBehaviour
{
    private KioskScript kiosk;
    private MenuManagerScript menu;
    private GameObject camToUse;

    void Awake()
    {
        kiosk = FindObjectOfType<KioskScript>();
        menu = FindObjectOfType<MenuManagerScript>();
        camToUse = GameObject.FindGameObjectWithTag("UICamera");

        LevelEntitlement();
        DifficultyEntitlement();
        ViricideEntitlement();
        LucentEntitlement();

        StartCoroutine(AccessibilityAssignment());
        //Debug.Log(PlayerPrefs.GetInt("unlockLevel02"));
        //Debug.Log(PlayerPrefs.GetInt("unlockLevel03"));

        //PlayerPrefs.SetInt("unlockLevel05", 1);
        //PlayerPrefs.SetInt("unlockLevel04", 1);
        //PlayerPrefs.SetInt("unlockLevel03", 1);
        //PlayerPrefs.SetInt("unlockLevel02", 1);
        //PlayerPrefs.SetInt("unlockDifficulty5", 1);
        //PlayerPrefs.SetInt("unlockViricide", 1);
        //PlayerPrefs.SetInt("firstViricideClear", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// //Checks Player ownership of Levels
    /// </summary>
    void LevelEntitlement()
    {
        if (PlayerPrefs.GetInt("unlockLevel02") == 1)
        {
            //Do nothing
        }

        else
        {
            PlayerPrefs.SetInt("unlockLevel02", 0);
        }

        if (PlayerPrefs.GetInt("unlockLevel03") == 1)
        {
            //Do nothing
        }

        else
        {
            PlayerPrefs.SetInt("unlockLevel03", 0);
        }

        if (PlayerPrefs.GetInt("unlockLevel04") == 1)
        {
            //Do nothing
        }

        else
        {
            PlayerPrefs.SetInt("unlockLevel04", 0);
        }

        if (PlayerPrefs.GetInt("unlockLevel05") == 1)
        {
            //Do nothing
        }

        else
        {
            PlayerPrefs.SetInt("unlockLevel05", 0);
        }
    }

    /// <summary>
    /// //Checks Player ownership of Difficulty 5
    /// </summary>
    void DifficultyEntitlement()
    {
        if (PlayerPrefs.GetInt("unlockDifficulty5") == 1)
        {
            return;
        }

        else
        {
            PlayerPrefs.SetInt("unlockDifficulty5", 0);
        }
    }

    /// <summary>
    /// //Checks Player ownership of Viricide access
    /// </summary>
    void ViricideEntitlement()
    {
        if (PlayerPrefs.GetInt("unlockViricide") == 1)
        {
            return;
        }

        else
        {
            PlayerPrefs.SetInt("unlockViricide", 0);
            PlayerPrefs.SetInt("firstViricideClear", 0);
        }
    }

    /// <summary>
    /// //Checks Player Lucent balance
    /// </summary>
    void LucentEntitlement()
    {
        kiosk.lucentFunds = PlayerPrefs.GetInt("lucentBalance");
    }

    IEnumerator AccessibilityAssignment()
    {
        yield return null;
        camToUse.GetComponent<Colorblind>().Type = PlayerPrefs.GetInt("colorblindSetting");
    }

    /// <summary>
    /// Resets Player progess (all PlayerPrefs, zeroes Lucent balance)
    /// </summary>
    public void FormatProgression()
    {
        PlayerPrefs.SetInt("unlockLevel02", 0);
        PlayerPrefs.SetInt("unlockLevel03", 0);
        PlayerPrefs.SetInt("unlockLevel04", 0);
        PlayerPrefs.SetInt("unlockLevel05", 0);
        PlayerPrefs.SetInt("lvl01Checkpoint", 0);
        PlayerPrefs.SetInt("lvl02Checkpoint", 0);
        PlayerPrefs.SetInt("lvl03Checkpoint", 0);
        PlayerPrefs.SetInt("lvl04Checkpoint", 0);
        PlayerPrefs.SetInt("lvl05Checkpoint", 0);
        PlayerPrefs.SetInt("unlockDifficulty5", 0);
        PlayerPrefs.SetInt("unlockViricide", 0);
        PlayerPrefs.SetInt("lucentBalance", 0);
        PlayerPrefs.SetInt("firstViricideClear", 0);
        PlayerPrefs.SetInt("toggleAim", 0);
        PlayerPrefs.SetFloat("aimControllerX", 5f);
        PlayerPrefs.SetFloat("aimControllerY", 5f);
        PlayerPrefs.SetFloat("aimMouseX", 10f);
        PlayerPrefs.SetFloat("aimMouseY", 10f);

        LucentEntitlement();
        menu.Progression();
    }
}
