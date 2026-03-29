using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MunitionScript : MonoBehaviour
{
    public GameObject hostLauncher;

    public int explosiveDamage;
    public float explosiveRange;
    public float hitDetectionLength = 2f;
    public Transform hitDetection;
    public GameObject detonationEffect; //VFX for munition
    public LayerMask contactOnly; //Ensures Raycast accounts for Surfaces
    public List<GameObject> targets = new List<GameObject>();

    internal bool activatorDroneFlag = false;
    internal bool fatedActivatorDrone = false;
    internal int activatorDroneDamage;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(hitDetection.position, hitDetection.forward, out hit, hitDetectionLength, contactOnly))
        {
            if(activatorDroneFlag)
            {
                ActivatorDroneTriggerMunition();
            }

            else
            {
                TriggerMunition();
            }

            if (hostLauncher.gameObject.GetComponent<MiningPlatform>())
            {
                hostLauncher.gameObject.GetComponent<MiningPlatform>().clusterPosition = hit.point + (hit.normal * 0.01f);
                hostLauncher.gameObject.GetComponent<MiningPlatform>().RemoteProc();
            }

            if (hostLauncher.gameObject.GetComponent<TheMostResplendent>())
            {
                if (hostLauncher.gameObject.GetComponent<TheMostResplendent>().stackCount >= 1 && hostLauncher.gameObject.GetComponent<TheMostResplendent>().toggle)
                {
                    GameObject lucentHard = Instantiate(hostLauncher.gameObject.GetComponent<TheMostResplendent>().hardLucent, hit.point + (hit.normal * 0.01f), 
                        Quaternion.LookRotation(hit.normal));
                    lucentHard.name = hostLauncher.gameObject.GetComponent<TheMostResplendent>().hardLucent.name;

                    if (hostLauncher.GetComponent<FirearmScript>().weaponRarity == 5 && !hostLauncher.GetComponent<FirearmScript>().isExotic)
                    {
                        lucentHard.GetComponent<TMRHardLucentScript>().fatedCrystal = true;
                    }

                    hostLauncher.gameObject.GetComponent<TheMostResplendent>().stackCount--;
                    hostLauncher.gameObject.GetComponent<TheMostResplendent>().toggle = false;
                }
            }

            if (hit.collider.gameObject.GetComponent<TMRHardLucentScript>())
            {
                GameObject miniCluster = Instantiate(hit.collider.gameObject.GetComponent<TMRHardLucentScript>().lucentCluster, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                miniCluster.name = hit.collider.gameObject.GetComponent<TMRHardLucentScript>().lucentCluster.name;
                miniCluster.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                miniCluster.GetComponent<LucentScript>().lucentGift /= 2;
                miniCluster.GetComponent<LucentScript>().lucentGift *= hostLauncher.GetComponent<FirearmScript>().weaponRarity;
                miniCluster.GetComponent<LucentScript>().ShatterCalculation();

                hit.collider.gameObject.GetComponent<TMRHardLucentScript>().shockwaveBuildup += hostLauncher.GetComponent<FirearmScript>().damage;
            }

            if (hostLauncher.gameObject.GetComponent<GaleForceWinds>())
            {
                if (hostLauncher.gameObject.GetComponent<GaleForceWinds>().chargeCount >= 1 && hostLauncher.gameObject.GetComponent<GaleForceWinds>().toggle)
                {
                    GameObject torrent = Instantiate(hostLauncher.gameObject.GetComponent<GaleForceWinds>().applicator, hit.point + (hit.normal * 0.01f), Quaternion.identity);
                    torrent.name = hostLauncher.gameObject.GetComponent<GaleForceWinds>().applicator.name;

                    if (hostLauncher.gameObject.GetComponent<FirearmScript>().weaponRarity == 5)
                    {
                        torrent.GetComponent<GFWStatusApplicator>().fatedFlag = true;
                        torrent.GetComponent<GFWStatusApplicator>().debuffMultiplier *= 1.43f;
                        torrent.GetComponent<GFWStatusApplicator>().travelRadius *= 1.5f;
                        torrent.GetComponent<GFWStatusApplicator>().travelLerpSpeed *= 2f;
                    }

                    hostLauncher.gameObject.GetComponent<GaleForceWinds>().chargeCount--;
                    hostLauncher.gameObject.GetComponent<GaleForceWinds>().chargePercentage = 0f;
                    hostLauncher.gameObject.GetComponent<GaleForceWinds>().done = false;
                    hostLauncher.gameObject.GetComponent<GaleForceWinds>().toggle = false;

                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.layer == 8 || other.gameObject.tag == "Lucent")
        {
            if (activatorDroneFlag)
            {
                ActivatorDroneTriggerMunition();
            }

            else
            {
                TriggerMunition();
            }

            if (hostLauncher.gameObject.GetComponent<MiningPlatform>())
            {
                hostLauncher.gameObject.GetComponent<MiningPlatform>().clusterPosition = transform.position;
                hostLauncher.gameObject.GetComponent<MiningPlatform>().RemoteProc();
            }

            if (hostLauncher.gameObject.GetComponent<GaleForceWinds>())
            {
                if (hostLauncher.gameObject.GetComponent<GaleForceWinds>().chargeCount >= 1 && hostLauncher.gameObject.GetComponent<GaleForceWinds>().toggle)
                {
                    GameObject torrent = Instantiate(hostLauncher.gameObject.GetComponent<GaleForceWinds>().applicator, transform.position, Quaternion.identity);
                    torrent.name = hostLauncher.gameObject.GetComponent<GaleForceWinds>().applicator.name;

                    if (hostLauncher.gameObject.GetComponent<FirearmScript>().weaponRarity == 5)
                    {
                        torrent.GetComponent<GFWStatusApplicator>().fatedFlag = true;
                        torrent.GetComponent<GFWStatusApplicator>().debuffMultiplier *= 1.43f;
                        torrent.GetComponent<GFWStatusApplicator>().travelRadius *= 1.5f;
                        torrent.GetComponent<GFWStatusApplicator>().travelLerpSpeed *= 2f;
                    }

                    hostLauncher.gameObject.GetComponent<GaleForceWinds>().chargeCount--;
                    hostLauncher.gameObject.GetComponent<GaleForceWinds>().chargePercentage = 0f;
                    hostLauncher.gameObject.GetComponent<GaleForceWinds>().done = false;
                    hostLauncher.gameObject.GetComponent<GaleForceWinds>().toggle = false;

                }
            }
        }
    }

    /// <summary>
    /// Collects colliders and applies damage to Enemies, detonates Lucent in its radius
    /// </summary>
    public void TriggerMunition()
    {      
        Vector3 epicenter = transform.position;
        Collider[] affected = Physics.OverlapSphere(transform.position, explosiveRange, contactOnly);
        foreach (Collider contact in affected)
        {
            if(contact.CompareTag("Enemy"))
            {               
                targets.Add(contact.gameObject);             
            }

            if (contact.CompareTag("Lucent"))
            {
                contact.gameObject.GetComponent<LucentScript>().lucentGift = 0;
                contact.gameObject.GetComponent<LucentScript>().shot = true;
            }
        }

        if (targets.Count >= 1)
        {
            for (int t = 0; t < targets.Count; t++)
            {
                if (targets[t].GetComponent<EnemyHealthScript>() != null)
                {
                    if (targets[t].GetComponent<EnemyHealthScript>().isImmune)
                    {
                        string indent = new string(' ', hostLauncher.GetComponent<FirearmScript>().currentDPSLine.Split('\n').Length * hostLauncher.GetComponent<FirearmScript>().indentSpace);
                        hostLauncher.GetComponent<FirearmScript>().newDPSLine = indent + "Immune";
                        hostLauncher.GetComponent<FirearmScript>().currentDPSLine = hostLauncher.GetComponent<FirearmScript>().newDPSLine + "\n" + hostLauncher.GetComponent<FirearmScript>().currentDPSLine;
                        hostLauncher.GetComponent<FirearmScript>().dpsText.GetComponent<Text>().text = hostLauncher.GetComponent<FirearmScript>().currentDPSLine;
                        hostLauncher.GetComponent<FirearmScript>().dpsText.GetComponent<TextClearScript>().clearTimer = hostLauncher.GetComponent<FirearmScript>().dpsText.GetComponent<TextClearScript>().timerReset;
                        hostLauncher.GetComponent<FirearmScript>().dpsLinesClear = hostLauncher.GetComponent<FirearmScript>().dpsLinesReset;

                        hostLauncher.GetComponent<FirearmScript>().DPSNumbers.text = "Immune";
                    }

                    else
                    {
                        string indent = new string(' ', hostLauncher.GetComponent<FirearmScript>().currentDPSLine.Split('\n').Length * hostLauncher.GetComponent<FirearmScript>().indentSpace);

                        if(activatorDroneFlag)
                        {
                            hostLauncher.GetComponent<FirearmScript>().newDPSLine = indent + activatorDroneDamage.ToString();
                        }

                        else
                        {
                            hostLauncher.GetComponent<FirearmScript>().newDPSLine = indent + hostLauncher.GetComponent<FirearmScript>().damage.ToString();
                        }

                        //hostLauncher.GetComponent<FirearmScript>().newDPSLine = indent + hostLauncher.GetComponent<FirearmScript>().damage.ToString();
                        hostLauncher.GetComponent<FirearmScript>().currentDPSLine = hostLauncher.GetComponent<FirearmScript>().newDPSLine + "\n" + hostLauncher.GetComponent<FirearmScript>().currentDPSLine;
                        hostLauncher.GetComponent<FirearmScript>().dpsText.GetComponent<Text>().text = hostLauncher.GetComponent<FirearmScript>().currentDPSLine;
                        hostLauncher.GetComponent<FirearmScript>().dpsText.GetComponent<TextClearScript>().clearTimer = hostLauncher.GetComponent<FirearmScript>().dpsText.GetComponent<TextClearScript>().timerReset;
                        hostLauncher.GetComponent<FirearmScript>().dpsLinesClear = hostLauncher.GetComponent<FirearmScript>().dpsLinesReset;

                        hostLauncher.GetComponent<FirearmScript>().DPSNumbers.text = hostLauncher.GetComponent<FirearmScript>().damage.ToString();
                        //Instantiate(targets[t].GetComponent<EnemyHealthScript>().blood, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                    }

                    targets[t].GetComponent<EnemyHealthScript>().inflictDamage(explosiveDamage);

                    if (hostLauncher.GetComponent<TrenchantPlatform>())
                    {
                        hostLauncher.GetComponent<TrenchantPlatform>().enemy = targets[t].gameObject;
                        hostLauncher.GetComponent<TrenchantPlatform>().RemoteProc();

                    }

                    if (hostLauncher.gameObject.GetComponent<MiningPlatform>())
                    {
                        hostLauncher.gameObject.GetComponent<MiningPlatform>().clusterPosition = targets[t].transform.position;
                        hostLauncher.gameObject.GetComponent<MiningPlatform>().RemoteProc();
                    }

                    if (hostLauncher.gameObject.GetComponent<SiphonicPlatform>())
                    {
                        hostLauncher.gameObject.GetComponent<SiphonicPlatform>().RemoteProc();
                    }

                    if (hostLauncher.GetComponent<Efficacy>() && !targets[t].GetComponent<EnemyHealthScript>().isImmune)
                    {
                        hostLauncher.GetComponent<Efficacy>().RemoteProc();
                    }

                    if (hostLauncher.GetComponent<MaliciousWindUp>() && !targets[t].GetComponent<EnemyHealthScript>().isImmune)
                    {
                        hostLauncher.GetComponent<MaliciousWindUp>().RemoteProc();
                    }

                    if (hostLauncher.GetComponent<PositiveNegative>() && !targets[t].GetComponent<EnemyHealthScript>().isImmune)
                    {
                        hostLauncher.GetComponent<PositiveNegative>().applyOn.Add(targets[t]);
                        hostLauncher.GetComponent<PositiveNegative>().RemoteProc();
                    }

                    if (hostLauncher.GetComponent<Cadence>() && !targets[t].GetComponent<EnemyHealthScript>().isImmune)
                    {
                        hostLauncher.GetComponent<Cadence>().clusterPosition = targets[t].transform.position + (Vector3.up * 0.01f);
                        hostLauncher.GetComponent<Cadence>().RemoteProc();
                    }

                    if (hostLauncher.GetComponent<GoodThingsCome>() && !targets[t].GetComponent<EnemyHealthScript>().isImmune)
                    {
                        hostLauncher.GetComponent<GoodThingsCome>().RemoteProc();
                    }

                    if (hostLauncher.GetComponent<TheMostResplendent>() && !targets[t].GetComponent<EnemyHealthScript>().isImmune)
                    {
                        hostLauncher.GetComponent<TheMostResplendent>().RemoteProc();
                    }

                    if (hostLauncher.GetComponent<Fulminate>() && !targets[t].GetComponent<EnemyHealthScript>().isImmune)
                    {
                        hostLauncher.GetComponent<Fulminate>().RemoteProc();
                    }

                    if (hostLauncher.GetComponent<Enshroud>() && !targets[t].GetComponent<EnemyHealthScript>().isImmune)
                    {
                        hostLauncher.GetComponent<Enshroud>().RemoteProc();
                    }

                    if (targets[t].GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                    {
                        if (hostLauncher.GetComponent<WaitNowImReady>())
                        {
                            hostLauncher.GetComponent<WaitNowImReady>().RemoteProc();
                        }

                        if (hostLauncher.GetComponent<Inoculated>())
                        {
                            hostLauncher.GetComponent<Inoculated>().RemoteProc();
                        }

                        if (hostLauncher.GetComponent<RudeAwakening>())
                        {
                            hostLauncher.GetComponent<RudeAwakening>().RemoteProc();
                        }

                        if (hostLauncher.GetComponent<NotWithAStick>())
                        {
                            hostLauncher.GetComponent<NotWithAStick>().RemoteProc();
                        }

                        if (hostLauncher.GetComponent<Cadence>())
                        {
                            hostLauncher.GetComponent<Cadence>().clusterPosition = targets[t].transform.position + (Vector3.up * 0.01f);
                            hostLauncher.GetComponent<Cadence>().RemoteProc();
                        }

                        if (hostLauncher.gameObject.GetComponent<Forager>())
                        {
                            hostLauncher.gameObject.GetComponent<Forager>().burstPosition = targets[t].transform.position + Vector3.up;
                            hostLauncher.GetComponent<Forager>().RemoteProc();
                        }

                        if (targets[t].GetComponent<Rigidbody>() == null)
                        {
                            targets[t].gameObject.AddComponent<Rigidbody>();
                            targets[t].gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
                        }
                    }


                }
            }

            targets.Clear();
        }

        GameObject effect = Instantiate(detonationEffect, transform.position, Quaternion.identity);
        effect.name = "Munition VFX";

        gameObject.SetActive(false);

    }

    /// <summary>
    /// Collects colliders and applies damage to Enemies, detonates Lucent in its radius
    /// </summary>
    public void ActivatorDroneTriggerMunition()
    {
        Vector3 epicenter = transform.position;
        Collider[] affected = Physics.OverlapSphere(transform.position, explosiveRange, contactOnly);
        foreach (Collider contact in affected)
        {
            if (contact.CompareTag("Enemy"))
            {
                targets.Add(contact.gameObject);
            }

            if (contact.CompareTag("Lucent"))
            {
                contact.gameObject.GetComponent<LucentScript>().lucentGift = 0;
                contact.gameObject.GetComponent<LucentScript>().shot = true;
            }
        }

        if (targets.Count >= 1)
        {
            for (int t = 0; t < targets.Count; t++)
            {
                if (targets[t].GetComponent<EnemyHealthScript>() != null)
                {
                    if (targets[t].GetComponent<EnemyHealthScript>().isImmune)
                    {
                        string indent = new string(' ', hostLauncher.GetComponent<FirearmScript>().currentDPSLine.Split('\n').Length * hostLauncher.GetComponent<FirearmScript>().indentSpace);
                        hostLauncher.GetComponent<FirearmScript>().newDPSLine = indent + "Immune";
                        hostLauncher.GetComponent<FirearmScript>().currentDPSLine = hostLauncher.GetComponent<FirearmScript>().newDPSLine + "\n" + hostLauncher.GetComponent<FirearmScript>().currentDPSLine;
                        hostLauncher.GetComponent<FirearmScript>().dpsText.GetComponent<Text>().text = hostLauncher.GetComponent<FirearmScript>().currentDPSLine;
                        hostLauncher.GetComponent<FirearmScript>().dpsText.GetComponent<TextClearScript>().clearTimer = hostLauncher.GetComponent<FirearmScript>().dpsText.GetComponent<TextClearScript>().timerReset;
                        hostLauncher.GetComponent<FirearmScript>().dpsLinesClear = hostLauncher.GetComponent<FirearmScript>().dpsLinesReset;

                        hostLauncher.GetComponent<FirearmScript>().DPSNumbers.text = "Immune";
                    }

                    else
                    {
                        string indent = new string(' ', hostLauncher.GetComponent<FirearmScript>().currentDPSLine.Split('\n').Length * hostLauncher.GetComponent<FirearmScript>().indentSpace);

                        hostLauncher.GetComponent<FirearmScript>().newDPSLine = indent + activatorDroneDamage.ToString();
                        hostLauncher.GetComponent<FirearmScript>().currentDPSLine = hostLauncher.GetComponent<FirearmScript>().newDPSLine + "\n" + hostLauncher.GetComponent<FirearmScript>().currentDPSLine;
                        hostLauncher.GetComponent<FirearmScript>().dpsText.GetComponent<Text>().text = hostLauncher.GetComponent<FirearmScript>().currentDPSLine;
                        hostLauncher.GetComponent<FirearmScript>().dpsText.GetComponent<TextClearScript>().clearTimer = hostLauncher.GetComponent<FirearmScript>().dpsText.GetComponent<TextClearScript>().timerReset;
                        hostLauncher.GetComponent<FirearmScript>().dpsLinesClear = hostLauncher.GetComponent<FirearmScript>().dpsLinesReset;

                        hostLauncher.GetComponent<FirearmScript>().DPSNumbers.text = activatorDroneDamage.ToString();
                        //Instantiate(targets[t].GetComponent<EnemyHealthScript>().blood, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                    }

                    targets[t].GetComponent<EnemyHealthScript>().inflictDamage(explosiveDamage);

                    if (hostLauncher.GetComponent<TrenchantPlatform>())
                    {
                        hostLauncher.GetComponent<TrenchantPlatform>().enemy = targets[t].gameObject;
                        hostLauncher.GetComponent<TrenchantPlatform>().RemoteProc();

                    }

                    if (hostLauncher.gameObject.GetComponent<MiningPlatform>())
                    {
                        hostLauncher.gameObject.GetComponent<MiningPlatform>().clusterPosition = targets[t].transform.position;
                        hostLauncher.gameObject.GetComponent<MiningPlatform>().RemoteProc();
                    }

                    if (hostLauncher.gameObject.GetComponent<SiphonicPlatform>())
                    {
                        hostLauncher.gameObject.GetComponent<SiphonicPlatform>().RemoteProc();
                    }

                    if (hostLauncher.GetComponent<Efficacy>() && !targets[t].GetComponent<EnemyHealthScript>().isImmune && fatedActivatorDrone)
                    {
                        hostLauncher.GetComponent<Efficacy>().RemoteProc();
                    }

                    if (hostLauncher.GetComponent<Cadence>() && !targets[t].GetComponent<EnemyHealthScript>().isImmune && fatedActivatorDrone)
                    {
                        hostLauncher.GetComponent<Cadence>().clusterPosition = targets[t].transform.position + (Vector3.up * 0.01f);
                        hostLauncher.GetComponent<Cadence>().RemoteProc();
                    }

                    if (hostLauncher.GetComponent<Enshroud>() && !targets[t].GetComponent<EnemyHealthScript>().isImmune && fatedActivatorDrone)
                    {
                        hostLauncher.GetComponent<Enshroud>().RemoteProc();
                    }

                    if (targets[t].GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                    {
                        if (hostLauncher.GetComponent<WaitNowImReady>() && fatedActivatorDrone)
                        {
                            hostLauncher.GetComponent<WaitNowImReady>().RemoteProc();
                        }

                        if (hostLauncher.GetComponent<Inoculated>() && fatedActivatorDrone)
                        {
                            hostLauncher.GetComponent<Inoculated>().RemoteProc();
                        }

                        if (targets[t].GetComponent<Rigidbody>() == null)
                        {
                            targets[t].gameObject.AddComponent<Rigidbody>();
                            targets[t].gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
                        }
                    }              
                }
            }

            targets.Clear();
        }

        GameObject effect = Instantiate(detonationEffect, transform.position, Quaternion.identity);
        effect.name = "Munition VFX";

        gameObject.SetActive(false);

    }
}
