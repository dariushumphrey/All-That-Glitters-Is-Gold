using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManagerScript : MonoBehaviour
{
    private KioskScript kiosk;
    private MenuManagerScript menu;
    void Awake()
    {
        kiosk = FindObjectOfType<KioskScript>();
        menu = FindObjectOfType<MenuManagerScript>();
        LevelEntitlement();
        DifficultyEntitlement();
        ViricideEntitlement();
        LucentEntitlement();
        //Debug.Log(PlayerPrefs.GetInt("unlockLevel02"));
        Debug.Log(PlayerPrefs.GetInt("unlockLevel03"));

        //PlayerPrefs.SetInt("unlockLevel02", 1);
        //PlayerPrefs.SetInt("unlockDifficulty5", 1);
        //PlayerPrefs.SetInt("unlockViricide", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
    }

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

    void LucentEntitlement()
    {
        kiosk.lucentFunds = PlayerPrefs.GetInt("lucentBalance");
    }

    public void FormatProgression()
    {
        PlayerPrefs.SetInt("unlockLevel02", 0);
        PlayerPrefs.SetInt("unlockLevel03", 0);
        PlayerPrefs.SetInt("unlockDifficulty5", 0);
        PlayerPrefs.SetInt("unlockViricide", 0);
        PlayerPrefs.SetInt("lucentBalance", 0);
        PlayerPrefs.SetInt("firstViricideClear", 0);

        LucentEntitlement();
        menu.Progression();
    }
}
