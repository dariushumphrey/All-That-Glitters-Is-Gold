using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotgunFirearm: FirearmScript
{
    public int pellets = 5;
    public float spread;
    //internal GameObject targetHit;

    //public Vector3[] pelletContact;

    //public void Start()
    //{
    //    bulletTrail = GetComponent<LineRenderer>();
    //    gunCam = FindObjectOfType<Camera>();
    //    confirmHit = false;
    //    pelletContact = new Vector3[pellets];    
    //}

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

            for (int a = 0; a < pellets; a++)
            {
                Vector3 rayOrigin = gunCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

                //Spread X
                Vector3 cone = gunCam.transform.right * Random.Range(-spread, spread);

                //Spread Y
                Vector3 cone2 = gunCam.transform.up * Random.Range(-spread, spread);           

                //Spreads out pellets. Multiplying the cones by spread makes pattern more Horizontal. Dividing instead makes pattern more Vertical. 
                rayOrigin += cone + cone2 * spread;              
            
                RaycastHit hit;

                //bulletTrail.SetPosition(0, barrel.position)  

                GameObject start = new GameObject();
                GameObject.Destroy(start, 0.1f);

                start.name = "Trail";

                start.AddComponent<LineRenderer>();

                start.GetComponent<LineRenderer>().startWidth = 0.1f;
                start.GetComponent<LineRenderer>().endWidth = 0.1f;
                
                start.GetComponent<LineRenderer>().material = bulletTrail.GetComponent<LineRenderer>().material;

                start.GetComponent<LineRenderer>().SetPosition(0, barrel.transform.position);
                //start.GetComponent<LineRenderer>().SetPosition(1, rayOrigin + (gunCam.transform.forward * range));

                if (Physics.Raycast(rayOrigin, gunCam.transform.forward, out hit, range, contactOnly))
                {
                    //bulletTrail.SetPosition(1, hit.point);
                    //Instantiate(DPSNumbers, hit.point, transform.rotation);
                    start.gameObject.transform.position = hit.point;
                    start.GetComponent<LineRenderer>().SetPosition(1, hit.point);

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

                        targetHit = hit.transform.gameObject;

                        //For damage falloff checks
                        if (hit.distance <= effectiveRange)
                        {
                            if (hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                            {
                                string indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                                newDPSLine = indent + "Immune";
                                currentDPSLine = newDPSLine + "\n" + currentDPSLine;
                                dpsText.GetComponent<Text>().text = currentDPSLine;
                                dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                                dpsLinesClear = dpsLinesReset;

                                DPSNumbers.text = "Immune";
                                //dpsText.GetComponent<Text>().text += "\n" + "Immune";
                                //dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                            }

                            else
                            {
                                string indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                                newDPSLine = indent + damage.ToString();
                                currentDPSLine = newDPSLine + "\n" + currentDPSLine;
                                dpsText.GetComponent<Text>().text = currentDPSLine;
                                dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                                dpsLinesClear = dpsLinesReset;

                                DPSNumbers.text = damage.ToString();
                                //dpsText.GetComponent<Text>().text += "\n" + damage.ToString();
                                //dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                            }

                            //Instantiate(DPSNumbers, hit.point, transform.rotation);
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
                                    CadenceRewardPosition(hit.collider.transform.position);

                                }

                                if (gameObject.GetComponent<RudeAwakening>())
                                {
                                    gameObject.GetComponent<RudeAwakening>().killConfirmed = true;
                                }

                                if (hit.collider.GetComponent<Rigidbody>() == null)
                                {
                                    hit.collider.gameObject.AddComponent<Rigidbody>();
                                    hit.collider.GetComponent<Rigidbody>().AddForce(-hit.collider.transform.forward * 30f, ForceMode.Impulse);
                                }

                                //if (hit.collider.GetComponent<Rigidbody>() != null)
                                //{
                                //    hit.collider.GetComponent<Rigidbody>().AddForce(-hit.collider.transform.forward * 2f, ForceMode.Impulse);
                                //}

                            }
                        }

                        if (hit.distance > effectiveRange)
                        {
                            if (hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                            {
                                string indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                                newDPSLine = indent + "Immune";
                                currentDPSLine = newDPSLine + "\n" + currentDPSLine;
                                dpsText.GetComponent<Text>().text = currentDPSLine;
                                dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                                dpsLinesClear = dpsLinesReset;

                                DPSNumbers.text = "Immune";
                                //dpsText.GetComponent<Text>().text += "\n" + "Immune";
                                //dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                            }

                            else
                            {
                                string indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                                newDPSLine = indent + (damage / 2).ToString();
                                currentDPSLine = newDPSLine + "\n" + currentDPSLine;
                                dpsText.GetComponent<Text>().text = currentDPSLine;
                                dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                                dpsLinesClear = dpsLinesReset;

                                DPSNumbers.text = (damage / 2).ToString();
                                //dpsText.GetComponent<Text>().text += "\n" + (damage / 2).ToString();
                                //dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                            }

                            //Instantiate(DPSNumbers, hit.point, transform.rotation);
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
                                    CadenceRewardPosition(hit.collider.transform.position);

                                }

                                if (gameObject.GetComponent<RudeAwakening>())
                                {
                                    gameObject.GetComponent<RudeAwakening>().killConfirmed = true;
                                }

                                if (hit.collider.GetComponent<Rigidbody>() == null)
                                {
                                    hit.collider.gameObject.AddComponent<Rigidbody>();
                                    hit.collider.GetComponent<Rigidbody>().AddForce(-hit.collider.transform.forward * 30f, ForceMode.Impulse);
                                }

                                //if (hit.collider.GetComponent<Rigidbody>() != null)
                                //{
                                //    hit.collider.GetComponent<Rigidbody>().AddForce(-hit.collider.transform.forward * 2f, ForceMode.Impulse);
                                //}
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

                    if(hit.collider.tag == "Player")
                    {
                        Debug.Log("Just shot myself");
                    }

                    if (hit.collider.tag == "Lucent")
                    {
                        inv.lucentFunds += hit.collider.GetComponent<LucentScript>().lucentGift;
                        if (inv.lucentFunds >= 100000)
                        {
                            inv.lucentFunds = 100000;
                        }
                        hit.collider.GetComponent<LucentScript>().lucentGift = 0;
                        hit.collider.GetComponent<LucentScript>().shot = true;
                    }
                }

                else
                {
                    //bulletTrail.SetPosition(1, rayOrigin + (gunCam.transform.forward * range));

                    start.gameObject.transform.position = rayOrigin + (gunCam.transform.forward * range);
                    start.GetComponent<LineRenderer>().SetPosition(1, rayOrigin + (gunCam.transform.forward * range));
                }               
            }                
        }
    }  
}
