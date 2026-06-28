using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Flashpoint : MonoBehaviour
{
    private LauncherFirearm firearm;
    private GameObject lucentMine;
    public List<GameObject> minesActive = new List<GameObject>();
    internal GameObject proc; //Text UI that records Cheat activity

    private PlayerInput input;
    internal InputAction useCheat;

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<LauncherFirearm>();
        lucentMine = Resources.Load<GameObject>("Game Items/Munition (GL_Exotic)");

        firearm.GetComponent<LauncherFirearm>().munition = lucentMine;

        input = firearm.input;
        useCheat = input.actions["Use Cheat"];

        if (proc)
        {
            proc.GetComponent<Text>().text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
       // minesActive = GameObject.FindGameObjectsWithTag("Mine");

        if(minesActive.Count >= 1)
        {
            proc.GetComponent<Text>().text = "Mines: " + minesActive.Count;
        }

        else
        {
            proc.GetComponent<Text>().text = "";
        }

        if (useCheat.triggered && minesActive.Count >= 1)
        {
            for (int m = 0; m < minesActive.Count; m++)
            {
                minesActive[m].GetComponent<MunitionScript>().TriggerMunition();
                Destroy(minesActive[m], 1f);
            }

            ClearMineList();

            //foreach (GameObject mine in minesActive)
            //{
            //    minesDetonate.Add(mine);
            //}
        }

        if(minesActive.Count >= 11)
        {
            minesActive[0].GetComponent<MunitionScript>().TriggerMunition();
            Destroy(minesActive[0], 1f);
            minesActive.Remove(minesActive[0]);
        }

        //if(minesDetonate.Count >= 1)
        //{
        //    for(int m = 0; m < minesDetonate.Count; m++)
        //    {
        //        minesDetonate[m].GetComponent<MunitionScript>().TriggerMunition();
        //        Destroy(minesDetonate[m], 1f);
        //    }

        //    for (int m = 0; m < minesDetonate.Count; m++)
        //    {
        //        minesDetonate.Remove(minesDetonate[m]);
        //    }

        //}
    }

    public void ClearMineList()
    {
        minesActive.Clear();
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }
    }
}
