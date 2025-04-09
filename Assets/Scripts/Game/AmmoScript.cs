using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{
    public int ammoStore = 100;
    private FirearmScript firearm;
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
