using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarSight : RangeScript
{
    private FirearmScript firearm;
    private float rangePercent = 10f;
    private int rangeAdd;

    public override void Awake()
    {
        firearm = GetComponent<FirearmScript>();
        Cheat();
        //GetComponent<RangeScript>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Cheat()
    {
        //Far Sight - 10% Effective Range Increase

        rangePercent /= 100;
        rangePercent *= firearm.range;
        rangeAdd = (int)rangePercent;

        firearm.effectiveRange += rangeAdd;
    }
}
