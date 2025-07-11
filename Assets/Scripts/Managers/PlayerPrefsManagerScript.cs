using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManagerScript : MonoBehaviour
{
    private KioskScript kiosk;
    void Awake()
    {
        kiosk = FindObjectOfType<KioskScript>();
        //LevelEntitlement();
        //DifficultyEntitlement();
        //ViricideEntitlement();
        LucentEntitlement();
        //Debug.Log(PlayerPrefs.GetInt("unlockLevel02"));

        PlayerPrefs.SetInt("unlockLevel02", 1);
        PlayerPrefs.SetInt("unlockDifficulty5", 1);
        PlayerPrefs.SetInt("unlockViricide", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LevelEntitlement()
    {
        if (PlayerPrefs.GetInt("unlockLevel02") == 1)
        {
            return;
        }

        else
        {
            PlayerPrefs.SetInt("unlockLevel02", 0);
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
        }
    }

    void LucentEntitlement()
    {
        kiosk.lucentFunds = PlayerPrefs.GetInt("lucentBalance");
    }
}
