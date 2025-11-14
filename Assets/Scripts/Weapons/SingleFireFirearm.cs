using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleFireFirearm : FirearmScript
{

    /// <summary>
    /// Activates Weapon firing behavior
    /// Provides information to Cheats with hit or kill triggers
    /// </summary>
    public override void FireWeapon()
    {
        fireAgain = fireAgain + Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && currentAmmo >= 1 && fireAgain >= fireRate && !isReloading)
        {
            //Firing timer resets, Ammo decrements/records number of shots
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

            //Produces the Bullet Trail
            GameObject start = new GameObject();
            GameObject.Destroy(start, 0.1f);

            start.name = "Trail";
            start.AddComponent<LineRenderer>();
            start.GetComponent<LineRenderer>().startWidth = 0.1f;
            start.GetComponent<LineRenderer>().endWidth = 0.1f;
            start.GetComponent<LineRenderer>().material = bulletTrail;
            start.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            start.GetComponent<LineRenderer>().SetPosition(0, barrel.transform.position);

            if (Physics.Raycast(rayOrigin, gunCam.transform.forward, out hit, range, contactOnly))
            {
                start.gameObject.transform.position = hit.point;
                start.GetComponent<LineRenderer>().SetPosition(1, hit.point);

                if (hit.collider.tag == "Enemy")
                {
                    //Affirms confirmed hits for Cheats
                    confirmHit = true;
                    if (gameObject.GetComponent<MaliciousWindUp>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<MaliciousWindUp>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<Efficacy>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<Efficacy>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<Cadence>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<Cadence>().hitConfirmed = true;
                        gameObject.GetComponent<Cadence>().clusterPosition = hit.point + (hit.normal * 0.01f);
                    }

                    if (gameObject.GetComponent<GoodThingsCome>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<GoodThingsCome>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<EarlyBerthGetsTheHearst>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<EarlyBerthGetsTheHearst>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<TheMostResplendent>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<TheMostResplendent>().hitConfirmed = true;

                        if (gameObject.GetComponent<TheMostResplendent>().stackCount >= 1 && gameObject.GetComponent<TheMostResplendent>().toggle)
                        {
                            GameObject lucentHard = Instantiate(gameObject.GetComponent<TheMostResplendent>().hardLucent, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal), hit.collider.gameObject.transform);
                            lucentHard.name = gameObject.GetComponent<TheMostResplendent>().hardLucent.name;
                            lucentHard.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                            if (weaponRarity == 5)
                            {
                                lucentHard.GetComponent<TMRHardLucentScript>().fatedCrystal = true;
                            }

                            gameObject.GetComponent<TheMostResplendent>().stackCount--;
                            gameObject.GetComponent<TheMostResplendent>().toggle = false;
                        }
                    }

                    if (gameObject.GetComponent<Fulminate>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<Fulminate>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<Forager>() && weaponRarity == 5 && !hit.collider.GetComponent<EnemyHealthScript>().isImmune && hit.collider.GetComponent<ReplevinScript>().amBoss)
                    {
                        gameObject.GetComponent<Forager>().hitConfirmed = true;
                        gameObject.GetComponent<Forager>().burstPosition = hit.collider.transform.position + (Vector3.up * 2);

                    }

                    if (gameObject.GetComponent<Enshroud>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        gameObject.GetComponent<Enshroud>().hitConfirmed = true;
                    }

                    if (gameObject.GetComponent<GaleForceWinds>() && !hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                    {
                        if (gameObject.GetComponent<GaleForceWinds>().chargeCount >= 1 && gameObject.GetComponent<GaleForceWinds>().toggle)
                        {
                            GameObject torrent = Instantiate(gameObject.GetComponent<GaleForceWinds>().applicator, hit.point + (hit.normal * 0.01f), Quaternion.identity);
                            torrent.name = gameObject.GetComponent<GaleForceWinds>().applicator.name;

                            if (weaponRarity == 5)
                            {
                                torrent.GetComponent<GFWStatusApplicator>().fatedFlag = true;
                                torrent.GetComponent<GFWStatusApplicator>().debuffMultiplier *= 1.43f;
                                torrent.GetComponent<GFWStatusApplicator>().travelRadius *= 1.5f;
                                torrent.GetComponent<GFWStatusApplicator>().travelLerpSpeed *= 2f;
                            }

                            gameObject.GetComponent<GaleForceWinds>().chargeCount--;
                            gameObject.GetComponent<GaleForceWinds>().chargePercentage = 0f;
                            gameObject.GetComponent<GaleForceWinds>().done = false;
                            gameObject.GetComponent<GaleForceWinds>().toggle = false;

                        }
                    }

                    StartCoroutine(DeconfirmHit());
                    targetHit = hit.transform.gameObject;

                    if (hit.distance <= effectiveRange)
                    {
                        //Records damage inflicted on non-immune Enemy hit. Records "Immune" on immune Enemy hit
                        if (hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                        {
                            string indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                            newDPSLine = indent + "Immune";
                            currentDPSLine = newDPSLine + "\n" + currentDPSLine;
                            dpsText.GetComponent<Text>().text = currentDPSLine;
                            dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                            dpsLinesClear = dpsLinesReset;

                            DPSNumbers.text = "Immune";
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
                            Instantiate(hit.collider.GetComponent<EnemyHealthScript>().blood, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                        }

                        hit.collider.GetComponent<EnemyHealthScript>().inflictDamage(damage);
                        if (hit.collider.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                        {
                            //Affirms confirmed kills for Cheats
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
                                gameObject.GetComponent<Cadence>().clusterPosition = hit.collider.transform.position;
                            }

                            if (gameObject.GetComponent<RudeAwakening>())
                            {
                                gameObject.GetComponent<RudeAwakening>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<Forager>())
                            {
                                gameObject.GetComponent<Forager>().killConfirmed = true;
                                gameObject.GetComponent<Forager>().burstPosition = hit.collider.transform.position + Vector3.up;
                            }

                            if (hit.collider.GetComponent<Rigidbody>() == null)
                            {
                                hit.collider.gameObject.AddComponent<Rigidbody>();
                                Vector3 shotForceDistance = barrel.transform.position - hit.collider.transform.position;
                                hit.collider.GetComponent<Rigidbody>().AddForce(-shotForceDistance.normalized * 10f, ForceMode.Impulse);
                            }

                            else
                            {
                                Vector3 shotForceDistance = barrel.transform.position - hit.collider.transform.position;
                                hit.collider.GetComponent<Rigidbody>().AddForce(-shotForceDistance.normalized * 10f, ForceMode.Impulse);
                            }
                        }
                    } //For damage falloff checks/kill triggers within Effective Range

                    if (hit.distance > effectiveRange)
                    {
                        //Records damage inflicted on non-immune Enemy hit. Records "Immune" on immune Enemy hit
                        if (hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                        {
                            string indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                            newDPSLine = indent + "Immune";
                            currentDPSLine = newDPSLine + "\n" + currentDPSLine;
                            dpsText.GetComponent<Text>().text = currentDPSLine;
                            dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                            dpsLinesClear = dpsLinesReset;

                            DPSNumbers.text = "Immune";
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
                            Instantiate(hit.collider.GetComponent<EnemyHealthScript>().blood, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                        }

                        hit.collider.GetComponent<EnemyHealthScript>().inflictDamage(damage / 2);
                        if (hit.collider.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                        {
                            //Affirms confirmed kills for Cheats
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
                                gameObject.GetComponent<Cadence>().clusterPosition = hit.collider.transform.position;
                            }

                            if (gameObject.GetComponent<RudeAwakening>())
                            {
                                gameObject.GetComponent<RudeAwakening>().killConfirmed = true;
                            }

                            if (gameObject.GetComponent<Forager>())
                            {
                                gameObject.GetComponent<Forager>().killConfirmed = true;
                                gameObject.GetComponent<Forager>().burstPosition = hit.collider.transform.position + Vector3.up;
                            }

                            if (hit.collider.GetComponent<Rigidbody>() == null)
                            {
                                hit.collider.gameObject.AddComponent<Rigidbody>();
                                Vector3 shotForceDistance = barrel.transform.position - hit.collider.transform.position;
                                hit.collider.GetComponent<Rigidbody>().AddForce(-shotForceDistance.normalized * 5f, ForceMode.Impulse);
                            }

                            else
                            {
                                Vector3 shotForceDistance = barrel.transform.position - hit.collider.transform.position;
                                hit.collider.GetComponent<Rigidbody>().AddForce(-shotForceDistance.normalized * 5f, ForceMode.Impulse);
                            }
                        }
                    } //For damage falloff checks/kill triggers while out of Effective Range
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

                if (hit.collider.gameObject.layer == 8) //If this Weapon strikes an object with the "Surface" layer
                {
                    if (gameObject.GetComponent<TheMostResplendent>())
                    {
                        if (gameObject.GetComponent<TheMostResplendent>().stackCount >= 1 && gameObject.GetComponent<TheMostResplendent>().toggle)
                        {
                            GameObject lucentHard = Instantiate(gameObject.GetComponent<TheMostResplendent>().hardLucent, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                            lucentHard.name = gameObject.GetComponent<TheMostResplendent>().hardLucent.name;

                            if (weaponRarity == 5)
                            {
                                lucentHard.GetComponent<TMRHardLucentScript>().fatedCrystal = true;
                            }

                            gameObject.GetComponent<TheMostResplendent>().stackCount--;
                            gameObject.GetComponent<TheMostResplendent>().toggle = false;
                        }
                    }

                    if (hit.collider.gameObject.GetComponent<TMRHardLucentScript>())
                    {
                        GameObject miniCluster = Instantiate(hit.collider.gameObject.GetComponent<TMRHardLucentScript>().lucentCluster, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                        miniCluster.name = hit.collider.gameObject.GetComponent<TMRHardLucentScript>().lucentCluster.name;
                        miniCluster.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                        miniCluster.GetComponent<LucentScript>().lucentGift /= 2;
                        miniCluster.GetComponent<LucentScript>().lucentGift *= weaponRarity;
                        miniCluster.GetComponent<LucentScript>().ShatterCalculation();

                        hit.collider.gameObject.GetComponent<TMRHardLucentScript>().shockwaveBuildup += damage;
                    }

                    if (gameObject.GetComponent<GaleForceWinds>())
                    {
                        if (gameObject.GetComponent<GaleForceWinds>().chargeCount >= 1 && gameObject.GetComponent<GaleForceWinds>().toggle)
                        {
                            GameObject torrent = Instantiate(gameObject.GetComponent<GaleForceWinds>().applicator, hit.point + (hit.normal * 0.01f), Quaternion.identity);
                            torrent.name = gameObject.GetComponent<GaleForceWinds>().applicator.name;

                            if (weaponRarity == 5)
                            {
                                torrent.GetComponent<GFWStatusApplicator>().fatedFlag = true;
                                torrent.GetComponent<GFWStatusApplicator>().debuffMultiplier *= 1.43f;
                                torrent.GetComponent<GFWStatusApplicator>().travelRadius *= 1.5f;
                                torrent.GetComponent<GFWStatusApplicator>().travelLerpSpeed *= 2f;
                            }

                            gameObject.GetComponent<GaleForceWinds>().chargeCount--;
                            gameObject.GetComponent<GaleForceWinds>().chargePercentage = 0f;
                            gameObject.GetComponent<GaleForceWinds>().done = false;
                            gameObject.GetComponent<GaleForceWinds>().toggle = false;

                        }
                    }

                    Instantiate(sparks, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                }
            }

            else
            {
                start.gameObject.transform.position = rayOrigin + (gunCam.transform.forward * range);
                start.GetComponent<LineRenderer>().SetPosition(1, rayOrigin + (gunCam.transform.forward * range));
            }

            muzzleFlash.Play();
        }
    }
}