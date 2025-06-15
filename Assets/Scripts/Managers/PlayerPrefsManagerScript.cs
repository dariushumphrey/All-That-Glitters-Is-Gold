using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManagerScript : MonoBehaviour
{ 
    void Awake()
    {
        LevelEntitlement();
        DifficultyEntitlement();
        ViricideEntitlement();
        //Debug.Log(PlayerPrefs.GetInt("unlockLevel02"));
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
}
