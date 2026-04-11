using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{
    public LayerMask contactOnly;
    //This value buffs reserve ammo gains:
    //-Increasing this number returns more ammunition to the held Weapon's reserves
    public float ammoPercent = 40f;
    public GameObject lootLight, lootModel, lootFocusCircle;
    public ParticleSystem acceptEffect;

    private FirearmScript firearm;
    private PlayerInventoryScript player;
    private int ammoStore = 0; //Receives Weapon's reserves size

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupDelivery());
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

            else if(other.gameObject.GetComponentInChildren<FirearmScript>().reserveAmmo >= other.gameObject.GetComponentInChildren<FirearmScript>().reserveSize)
            {
                return;
            }

            else
            {
                //Increases Weapon reserves size by specified percent
                //Increases all Player Grenade charges by one
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

                if (other.gameObject.GetComponent<PlayerInventoryScript>() == null)
                {
                    return;
                }

                else
                {
                    player = other.gameObject.GetComponent<PlayerInventoryScript>();
                    player.fogGrenadeCharges++;
                    player.solGrenadeCharges++;
                    player.desGrenadeCharges++;
                }

                other.gameObject.GetComponent<PlayerInventoryScript>().weaponAmmoPage.gameObject.SetActive(true);
                other.gameObject.GetComponent<PlayerInventoryScript>().lucentText.gameObject.SetActive(true);
                other.gameObject.GetComponent<PlayerInventoryScript>().wepStateTimer = other.gameObject.GetComponent<PlayerInventoryScript>().wepStateTimerReset;

                var main = acceptEffect.GetComponent<ParticleSystem>().main;
                main.startColor = Color.white;
                Instantiate(acceptEffect, other.gameObject.transform.position, other.gameObject.transform.rotation);

                gameObject.SetActive(false);
            }        
        }
    }

    public IEnumerator SetupDelivery()
    {
        yield return new WaitForSeconds(2f);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, contactOnly))
        {
            if (hit.collider.gameObject.layer == 8)
            {
                transform.position = hit.point + (hit.normal * 1.25f);
            }
        }

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        if (gameObject.GetComponent<Rigidbody>())
        {
            Destroy(gameObject.GetComponent<Rigidbody>());
        }

        if (gameObject.GetComponent<BoxCollider>())
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            //gameObject.GetComponent<BoxCollider>().size = new Vector3(3f, 3f, 3f);
        }

        //GameObject lightSource = Instantiate(lootLight, transform.position, transform.rotation);
        //lightSource.GetComponent<Light>().color = Color.white;
        //lightSource.name = lootLight.name;
        //lightSource.transform.parent = gameObject.transform;

        GameObject deliveryItemCircle = Instantiate(lootFocusCircle, transform.position + Vector3.down * 1.2f, Quaternion.identity);
        deliveryItemCircle.GetComponent<Renderer>().material.color = Color.white;
        deliveryItemCircle.name = lootFocusCircle.name;
        deliveryItemCircle.transform.parent = gameObject.transform;

        lootModel.AddComponent<RotateScript>();
        lootModel.GetComponent<RotateScript>().rotationAccelerant = 1;
        lootModel.GetComponent<RotateScript>().automaticY = 0.2f;
    }
}
