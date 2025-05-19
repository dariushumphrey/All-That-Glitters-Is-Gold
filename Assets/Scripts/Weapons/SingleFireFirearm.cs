using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFireFirearm : FirearmScript
{
    //Variable that holds an Enemy that was hit by this weapon

    public override void FireWeapon()
    {
        fireAgain = fireAgain + Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && currentAmmo >= 1 && fireAgain >= fireRate && !isReloading)
        {
            fireAgain = 0.0f;
            currentAmmo--;
            ammoSpent++;

            //Recoil
            gunCam.transform.eulerAngles = new Vector3(GetComponentInParent<PlayerCameraScript>().pitch -=
                Random.Range(-wepRecoil, wepRecoil), GetComponentInParent<PlayerCameraScript>().yaw +=
                Random.Range(-wepRecoil, wepRecoil), 0.0f);

            if (ammoSpent >= ammoSize)
            {
                ammoSpent = ammoSize;
            }

            Vector3 rayOrigin = gunCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            bulletTrail.SetPosition(0, barrel.position);

            if (Physics.Raycast(rayOrigin, gunCam.transform.forward, out hit, range))
            {
                bulletTrail.SetPosition(1, hit.point);
                //Instantiate(DPSNumbers, hit.point, transform.rotation);

                if (hit.collider.tag == "Enemy")
                {
                    confirmHit = true;
                    if (gameObject.GetComponent<MaliciousWindUp>())
                    {
                        gameObject.GetComponent<MaliciousWindUp>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<Efficacy>())
                    {
                        gameObject.GetComponent<Efficacy>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<Cadence>())
                    {
                        gameObject.GetComponent<Cadence>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<GoodThingsCome>())
                    {
                        gameObject.GetComponent<GoodThingsCome>().hitConfirmed = true;
                    }

                    StartCoroutine(DeconfirmHit());
                    FatedCadenceRewardPosition(hit.collider.transform.position);

                    //Debug.Log(hit.transform.gameObject);
                    targetHit = hit.transform.gameObject;

                    //For damage falloff checks
                    if (hit.distance <= effectiveRange)
                    {
                        DPSNumbers.text = damage.ToString();
                        Instantiate(DPSNumbers, hit.point, transform.rotation);
                        hit.collider.GetComponent<EnemyHealthScript>().inflictDamage(damage);
                        if (hit.collider.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                        {
                            confirmKill = true;
                            if (gameObject.GetComponent<NotWithAStick>())
                            {
                                gameObject.GetComponent<NotWithAStick>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<MaliciousWindUp>())
                            {
                                gameObject.GetComponent<MaliciousWindUp>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<WaitNowImReady>())
                            {
                                gameObject.GetComponent<WaitNowImReady>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<Inoculated>())
                            {
                                gameObject.GetComponent<Inoculated>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<Cadence>())
                            {
                                gameObject.GetComponent<Cadence>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<RudeAwakening>())
                            {
                                gameObject.GetComponent<RudeAwakening>().killConfirmed = true;
                            }

                            //hit.collider.GetComponent<Rigidbody>().AddForce(-hit.collider.transform.forward * 1f, ForceMode.Impulse);
                        }
                    }

                    if (hit.distance > effectiveRange)
                    {
                        DPSNumbers.text = (damage / 2).ToString();
                        Instantiate(DPSNumbers, hit.point, transform.rotation);
                        hit.collider.GetComponent<EnemyHealthScript>().inflictDamage(damage / 2);
                        if (hit.collider.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                        {
                            confirmKill = true;
                            if (gameObject.GetComponent<NotWithAStick>())
                            {
                                gameObject.GetComponent<NotWithAStick>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<MaliciousWindUp>())
                            {
                                gameObject.GetComponent<MaliciousWindUp>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<WaitNowImReady>())
                            {
                                gameObject.GetComponent<WaitNowImReady>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<Inoculated>())
                            {
                                gameObject.GetComponent<Inoculated>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<Cadence>())
                            {
                                gameObject.GetComponent<Cadence>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<RudeAwakening>())
                            {
                                gameObject.GetComponent<RudeAwakening>().killConfirmed = true;
                            }

                            //hit.collider.GetComponent<Rigidbody>().AddForce(-hit.collider.transform.forward * 1f, ForceMode.Impulse);
                        }
                    }

                    if (hit.collider.GetComponent<ReplevinScript>() != null)
                    {
                        hit.collider.GetComponent<ReplevinScript>().playerFound = true;
                    }

                    //For Clusters
                    if (hit.collider.GetComponent<EnemyLeaderScript>() != null)
                    {
                        hit.collider.GetComponent<EnemyLeaderScript>().Pursuit();
                    }

                    if (hit.collider.GetComponent<EnemyFollowerScript>() != null)
                    {
                        if (hit.collider.GetComponent<EnemyFollowerScript>().leader != null && hit.collider.GetComponent<EnemyFollowerScript>().leader.GetComponent<EnemyHealthScript>().healthCurrent > 0)
                        {
                            hit.collider.GetComponent<EnemyFollowerScript>().leader.Pursuit();
                        }
                    }


                }

                if (hit.collider.tag == "Lucent")
                {
                    inv.lucentFunds += hit.collider.GetComponent<LucentScript>().lucentGift;
                    hit.collider.GetComponent<LucentScript>().lucentGift = 0;
                }
            }

            else
            {
                bulletTrail.SetPosition(1, rayOrigin + (gunCam.transform.forward * range));
            }
        }
    }
}
