using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HastyHands : ReloadSpeedScript
{
    private FirearmScript firearm;
    private float reloadPercent = 15f;
    private float reloadAdd;

    public override void Awake()
    {
        firearm = GetComponent<FirearmScript>();      
        Cheat();
        //GetComponent<ReloadSpeedScript>().enabled = false;
    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    void Cheat()
    {
        //Hasty Hands - 15% Reload Speed Increase

        reloadPercent /= 100;
        reloadPercent *= firearm.reloadSpeed;
        reloadAdd = reloadPercent;      

        firearm.reloadSpeed -= reloadAdd;
    }
}
