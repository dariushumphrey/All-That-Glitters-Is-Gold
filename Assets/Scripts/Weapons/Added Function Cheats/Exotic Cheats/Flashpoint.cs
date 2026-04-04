using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashpoint : MonoBehaviour
{
    private LauncherFirearm firearm;
    private GameObject lucentMine;
    private GameObject[] minesActive;
    private List<GameObject> minesDetonate = new List<GameObject>();
    internal GameObject proc; //Text UI that records Cheat activity

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<LauncherFirearm>();
        lucentMine = Resources.Load<GameObject>("Game Items/Munition (GL_Exotic)");

        firearm.GetComponent<LauncherFirearm>().munition = lucentMine;

        if (proc)
        {
            proc.GetComponent<Text>().text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        minesActive = GameObject.FindGameObjectsWithTag("Mine");

        if(minesActive.Length >= 1)
        {
            proc.GetComponent<Text>().text = "Mines: " + minesActive.Length;
        }


        if (Input.GetKeyDown(KeyCode.E) && minesActive.Length >= 1)
        {
            foreach (GameObject mine in minesActive)
            {
                minesDetonate.Add(mine);
            }
        }

        if(minesDetonate.Count >= 1)
        {
            for(int m = 0; m < minesDetonate.Count; m++)
            {
                minesDetonate[m].GetComponent<MunitionScript>().TriggerMunition();
                Destroy(minesDetonate[m], 1f);
            }

            for (int m = 0; m < minesDetonate.Count; m++)
            {
                minesDetonate.Remove(minesDetonate[m]);
            }

            proc.GetComponent<Text>().text = "";
        }
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }
    }
}
