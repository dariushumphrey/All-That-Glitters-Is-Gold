using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeeperStores : MagazineScript
{
    private FirearmScript firearm;
    private float reservePercent = 30f;
    private int reserveAdd;

    public override void Awake()
    {
        firearm = GetComponent<FirearmScript>();
        Cheat();
        //GetComponent<MagazineScript>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Cheat()
    {
        //Deeper Stores - 30% Reserve Size

        reservePercent /= 100;
        reservePercent *= firearm.reserveSize;
        reserveAdd = Mathf.RoundToInt(reservePercent);

        firearm.reserveSize += reserveAdd;
        firearm.reserveAmmo = firearm.reserveSize;

    }
}
