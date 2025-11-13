using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepStores : MonoBehaviour
{
    private FirearmScript firearm;
    private float reservePercent = 15f; //Percent of ammo capacity to increase
    private int reserveAdd; //Used to increase max reserves capacity

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
        //Deep Stores - 15% Reserve Size

        //Adds a percentage of max reserve capacity onto itself, increasing stored ammo
        reservePercent /= 100;
        reservePercent *= firearm.reserveSize;
        reserveAdd = Mathf.RoundToInt(reservePercent);

        firearm.reserveSize += reserveAdd;
        firearm.reserveAmmo = firearm.reserveSize;

    }
}
