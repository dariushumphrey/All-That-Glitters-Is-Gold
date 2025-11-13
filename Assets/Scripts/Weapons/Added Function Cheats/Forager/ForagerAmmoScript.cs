using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForagerAmmoScript : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerInventoryScript player;
    private GameObject lerpPoint; //Object to lerp towards
    internal int ammoAdd; //Number used to increase Weapon ammo
    internal int overflowCap; //Number used to limit Weapon magazine

    // Start is called before the first frame update
    void Start()
    {
        lerpPoint = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(AutoCollection());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Fills Weapon ammo up to overflow cap and increase count of equipped grenade
        if(collision.gameObject.CompareTag("Player"))
        {
            firearm = collision.gameObject.GetComponentInChildren<FirearmScript>();
            firearm.currentAmmo += ammoAdd;
            if(firearm.currentAmmo >= overflowCap)
            {
                firearm.currentAmmo = overflowCap;
            }

            player = collision.gameObject.GetComponent<PlayerInventoryScript>();
            if(player.grenadeSelection == 0)
            {
                player.fogGrenadeCharges++;
            }

            else if (player.grenadeSelection == 1)
            {
                player.solGrenadeCharges++;
            }

            else if (player.grenadeSelection == 2)
            {
                player.desGrenadeCharges++;
            }

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Causes pickup to Lerp towards Player after delay
    /// </summary>
    IEnumerator AutoCollection()
    {
        yield return new WaitForSeconds(1f);
        gameObject.AddComponent<LerpScript>();
        gameObject.GetComponent<LerpScript>().positionOne = gameObject.transform;
        gameObject.GetComponent<LerpScript>().positionTwo = lerpPoint.transform;
        gameObject.GetComponent<LerpScript>().thing = gameObject;

        gameObject.GetComponent<LerpScript>().rate = 0.025f;
        gameObject.GetComponent<LerpScript>().automated = true;

    }
}
