using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartherSight : RangeScript
{
    private FirearmScript firearm;
    private float rangePercent = 20f;
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
        //Farther Sight - 20% Effective Range Increase

        rangePercent /= 100;
        rangePercent *= firearm.range;
        rangeAdd = (int)rangePercent;

        firearm.effectiveRange += rangeAdd;
    }
}
