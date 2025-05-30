using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{

    //This value buffs reserve ammo gains:
    //-Increasing this number returns more ammunition to the held Weapon's reserves
    public float ammoPercent = 40f;

    private FirearmScript firearm;
    private int ammoStore = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(other.gameObject.GetComponentInChildren<FirearmScript>() == null)
            {
                return;
            }

            else
            {
                firearm = other.gameObject.GetComponentInChildren<FirearmScript>();
                ammoStore = firearm.reserveSize;

                ammoPercent /= 100;
                ammoPercent *= ammoStore;
                ammoStore = (int)ammoPercent;

                firearm.reserveAmmo += ammoStore;

                if (firearm.reserveAmmo >= firearm.reserveSize)
                {
                    firearm.reserveAmmo = firearm.reserveSize;
                }

                Destroy(gameObject);
            }         
        }
    }
}
