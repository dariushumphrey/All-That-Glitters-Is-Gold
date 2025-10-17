using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeScript : MonoBehaviour
{
    public int meleeDamage = 5000;
    public float meleeRange = 4f;
    public float meleeSpeed = 3f;

    private PlayerInventoryScript inv;
    private PlayerMoveScript move;
    internal bool confirmKill;
    internal bool meleeLock;
    internal GameObject meleeTarget;
    internal GameObject fulminateCheat;
    internal GameObject foragerCheat;
    internal GameObject enshroudCheat;
    // Start is called before the first frame update
    void Start()
    {
        inv = gameObject.GetComponent<PlayerInventoryScript>();
        move = gameObject.GetComponent<PlayerMoveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (meleeTarget == null)
            {
                //Do nothing
            }

            else
            {
                meleeLock = true;
            }
        }

        if (meleeLock && meleeTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, meleeTarget.transform.position, meleeSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(meleeTarget.transform.position - transform.position, Vector3.up), 2f);

            MeleeStrike();
        }
    }

    void MeleeStrike()
    {
        RaycastHit hit;        
        if (Physics.Raycast(transform.position, transform.forward, out hit, 3f))
        {
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<EnemyHealthScript>().inflictDamage(meleeDamage);
                if(hit.collider.gameObject.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                {
                    if(fulminateCheat != null)
                    {
                        GameObject free = Instantiate(inv.grenades[2], hit.point, transform.rotation);
                        free.GetComponent<DestructGrenadeScript>().armingTime = 0.0f;
                        free.GetComponent<DestructGrenadeScript>().StartCoroutine(free.GetComponent<DestructGrenadeScript>().SetupGrenade());
                    }

                    if(foragerCheat != null)
                    {
                        foragerCheat.GetComponent<Forager>().burstPosition = hit.collider.transform.position + Vector3.up;
                        foragerCheat.GetComponent<Forager>().ForagerBurst();
                    }

                    if (enshroudCheat != null && !enshroudCheat.GetComponent<Enshroud>().cooldown)
                    {
                        GameObject free = Instantiate(inv.grenades[0], transform.position + Vector3.down, Quaternion.Euler(new Vector3(90f, 0f, 0f)));
                        free.GetComponent<FoggerGrenadeScript>().armingTime = 0.0f;
                        free.GetComponent<FoggerGrenadeScript>().StartCoroutine(free.GetComponent<FoggerGrenadeScript>().SetupGrenade());

                        if(enshroudCheat.GetComponent<FirearmScript>().weaponRarity == 5)
                        {
                            free.GetComponent<FoggerGrenadeScript>().enshroudFlag = true;
                        }

                        enshroudCheat.GetComponent<Enshroud>().cooldown = true;
                    }

                    if (hit.collider.gameObject.GetComponent<Rigidbody>() == null)
                    {
                        hit.collider.gameObject.AddComponent<Rigidbody>();
                        hit.collider.GetComponent<Rigidbody>().AddForce(-hit.collider.transform.forward * 15f, ForceMode.Impulse);
                    }

                }

                meleeLock = false;
            }
        }
    }
}
