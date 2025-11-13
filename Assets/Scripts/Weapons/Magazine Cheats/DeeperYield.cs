using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeeperYield : MonoBehaviour
{
    private FirearmScript firearm;
    private float magazinePercent = 24f; //Percent of ammo capacity to increase
    private int magAdd; //Used to increase max magazine capacity

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
        //Deeper Yield - 24% Magazine Size

        //Adds a percentage of max magazine capacity onto itself, increasing usable ammo
        magazinePercent /= 100;
        magazinePercent *= firearm.ammoSize;
        magAdd = Mathf.RoundToInt(magazinePercent);

        firearm.ammoSize += magAdd;
        firearm.currentAmmo = firearm.ammoSize;
    }
}
