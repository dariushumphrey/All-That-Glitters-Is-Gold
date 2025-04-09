using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeeperYield : MagazineScript
{
    private FirearmScript firearm;
    private float magazinePercent = 24f;
    private int magAdd;

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
        //Deeper Yield - 24% Magazine Size
        
        magazinePercent /= 100;
        magazinePercent *= firearm.ammoSize;
        magAdd = Mathf.RoundToInt(magazinePercent);

        firearm.ammoSize += magAdd;
        firearm.currentAmmo = firearm.ammoSize;
    }
}
