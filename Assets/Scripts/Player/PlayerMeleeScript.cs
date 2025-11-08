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
    internal bool confirmKill; //verifies that a Kill has been achieved if true
    internal bool meleeLock; //locks Player into Melee attack if true
    internal GameObject meleeTarget;
    internal GameObject fulminateCheat; //Confirms presence of Weapon with Fulminate if not null
    internal GameObject foragerCheat; //Confirms presence of Weapon with Forager if not null
    internal GameObject enshroudCheat; //Confirms presence of Weapon with Enshroud if not null

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
            //Melee attacks cannot occur if a target is not found
            //Otherwise, Player is locked into attack
            if (meleeTarget == null)
            {
                //Do nothing
            }

            else
            {
                meleeLock = true;
            }
        }

        //Player-character travels and rotates towards Melee target
        if (meleeLock && meleeTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, meleeTarget.transform.position, meleeSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(meleeTarget.transform.position - transform.position, Vector3.up), 2f);

            MeleeStrike();
        }
    }

    /// <summary>
    /// Casts a ray that applies Melee damage
    /// Produces effects dependent on Functional Cheat presence on Melee kills
    /// </summary>
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
                    //Produces a Destruct Grenade that detonates instantly
                    if(fulminateCheat != null)
                    {
                        GameObject free = Instantiate(inv.grenades[2], hit.point, transform.rotation);
                        free.GetComponent<DestructGrenadeScript>().armingTime = 0.0f;
                        free.GetComponent<DestructGrenadeScript>().StartCoroutine(free.GetComponent<DestructGrenadeScript>().SetupGrenade());
                    }

                    //Triggers Forager's pickup burst
                    if(foragerCheat != null)
                    {
                        foragerCheat.GetComponent<Forager>().burstPosition = hit.collider.transform.position + Vector3.up;
                        foragerCheat.GetComponent<Forager>().ForagerBurst();
                    }

                    //Produces a Fogger Grenade that triggers instantly and begins cooldown
                    //Activates Fogger Grenade' damage-over-time ability if Weapon is Rarity 5
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

                    //Applies Rigidbody force on Enemy defeats
                    if (hit.collider.gameObject.GetComponent<Rigidbody>() == null)
                    {
                        hit.collider.gameObject.AddComponent<Rigidbody>();
                        Vector3 meleeForceDistance = transform.position - hit.collider.transform.position;
                        hit.collider.GetComponent<Rigidbody>().AddForce(-meleeForceDistance.normalized * 20f, ForceMode.Impulse);
                    }
                }

                meleeLock = false;
            }
        }
    }
}
