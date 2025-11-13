using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarSight : MonoBehaviour
{
    private FirearmScript firearm;
    private float rangePercent = 10f; //Percent of Effective Range to increase
    private int rangeAdd; //Used to increase Effective Range

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
        //Far Sight - 10% Effective Range Increase

        //Adds a percentage of max Range onto Effective Range
        //Allows Weapon to apply full damage at farther ranges
        rangePercent /= 100;
        rangePercent *= firearm.range;
        rangeAdd = (int)rangePercent;

        firearm.effectiveRange += rangeAdd;
    }
}
