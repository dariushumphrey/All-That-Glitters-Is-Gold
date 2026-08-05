using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRaycastScript : MonoBehaviour
{
    public Transform attackStartPoint;
    public int damage;

    private bool attackLock = false;
    private Vector3 rayOrigin;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        attackLock = true;
        rayOrigin = attackStartPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(attackLock)
        {
            if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, 1.25f))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
                    {
                        if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat)
                        {
                            hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;
                        }
                    }

                    else if (hit.collider.GetComponent<PlayerMeleeScript>().guarding)
                    {
                        if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat &&
                            hit.collider.GetComponent<PlayerStatusScript>().cam.multiWeapon)
                        {
                            hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;
                        }
                    }

                    else
                    {
                        hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                        hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                        //This code shoves the Player with particular force in their opposite direction.
                        //This is a melee attack, shoving the player with less force, subtly offsetting the player upwards to distinguish it from a charge.
                        //Vector3 knockbackDir = -hit.collider.transform.forward;
                        //hit.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * 50000);
                    }

                    attackLock = false;
                    StartCoroutine(ResetAttackLock());
                }
            }
        }     
    }

    public IEnumerator ResetAttackLock()
    {
        yield return new WaitForSeconds(1f);
        attackLock = true;
    }
}
