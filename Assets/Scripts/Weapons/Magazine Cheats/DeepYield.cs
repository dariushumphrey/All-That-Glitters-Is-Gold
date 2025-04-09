using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepYield : MagazineScript
{
    private FirearmScript firearm;
    private float magazinePercent = 12f;
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
        //Deep Yield - 12% Magazine Size
     
        magazinePercent /= 100;
        magazinePercent *= firearm.ammoSize;
        magAdd = Mathf.RoundToInt(magazinePercent);

        firearm.ammoSize += magAdd;
        firearm.currentAmmo = firearm.ammoSize;
    }
}
