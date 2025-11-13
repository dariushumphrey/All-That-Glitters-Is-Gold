using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HastierHands : MonoBehaviour
{
    private FirearmScript firearm;
    private float reloadPercent = 25f; //Percent of Reload Speed to increase
    private float reloadAdd; //Used to subtract from Reload Speed

    public void Awake()
    {
        firearm = GetComponent<FirearmScript>();
        Cheat();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Cheat()
    {
        //Hastier Hands - 25% Reload Speed Increase

        //Subtracts a percentage of Reload Speed from itself
        //Allows a Weapon to reload faster
        reloadPercent /= 100;
        reloadPercent *= firearm.reloadSpeed;
        reloadAdd = reloadPercent;      
        firearm.reloadSpeed -= reloadAdd;

    }
}
