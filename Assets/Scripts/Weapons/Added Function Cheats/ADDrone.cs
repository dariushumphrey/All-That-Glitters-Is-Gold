using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADDrone : MonoBehaviour
{
    public GameObject hostWeapon;
    public GameObject munition;
    public int damage = 2;
    public LayerMask contactOnly; //Ensures Raycast accounts for Surfaces
    public List<GameObject> targets = new List<GameObject>();
    public Material bulletTrail;
    public Material targetingTrail;
    public ParticleSystem muzzleFlash;
    public ParticleSystem launcherFlash;
    public GameObject engagedTarget;
    public Transform attackPoint;
    public float shotForceForward = 50f;
    public float fireRate = 0.1f;
    private float range = 40f;
    public bool siphonicFlag, miningFlag, trenchantFlag, cacheFlag, fatedFlag;
    private float fireAgain = 0.0f;
    private RaycastHit hit;
    internal Vector3 targetVector;

    private int targetChoice;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyHealthAssessment();
        fireAgain += Time.deltaTime;

        if(Time.timeScale == 1)
        {
            if (Input.GetButton("Fire2"))
            {
                if (Physics.Raycast(attackPoint.transform.position, attackPoint.transform.forward, out hit, range, contactOnly))
                {
                    //Produces the Bullet Trail
                    GameObject start = new GameObject();
                    GameObject.Destroy(start, 0.02f);

                    start.name = "Trail";
                    start.AddComponent<LineRenderer>();
                    start.GetComponent<LineRenderer>().startWidth = 0.02f;
                    start.GetComponent<LineRenderer>().endWidth = 0.02f;
                    start.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    start.GetComponent<LineRenderer>().material = targetingTrail;
                    start.GetComponent<LineRenderer>().SetPosition(0, attackPoint.transform.position);

                    start.gameObject.transform.position = hit.point;
                    start.GetComponent<LineRenderer>().SetPosition(1, hit.point);

                    if (hit.collider.CompareTag("Enemy"))
                    {
                        engagedTarget = hit.collider.gameObject;
                    }

                    else
                    {
                        engagedTarget = null;
                    }
                }            
            }

            else
            {
                AutomatedPhase();
            }
        }
    }

    public bool CanSeeEnemy()
    {
        if(engagedTarget)
        {
            targetVector = engagedTarget.transform.position - transform.position;

            if (Physics.Raycast(transform.position, targetVector, out hit, Mathf.Infinity, contactOnly))
            {
                if(hit.collider.gameObject == engagedTarget)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void AutomatedPhase()
    {
        if(CanSeeEnemy())
        {
            if (Physics.Raycast(attackPoint.transform.position, attackPoint.transform.forward, out hit, range, contactOnly))
            {             
                if (hit.collider.CompareTag("Enemy") && fireAgain >= fireRate)
                {
                    if(cacheFlag)
                    {
                        fireAgain = 0f;

                        GameObject launched = Instantiate(munition, attackPoint.transform.position, attackPoint.transform.rotation * Quaternion.Euler(90f, 0f, 0f));
                        launched.name = munition.name;

                        launched.GetComponent<MunitionScript>().hostLauncher = hostWeapon;
                        launched.GetComponent<MunitionScript>().explosiveDamage = damage;

                        launched.GetComponent<MunitionScript>().activatorDroneFlag = true;

                        if(fatedFlag)
                        {
                            launched.GetComponent<MunitionScript>().fatedActivatorDrone = true;
                        }

                        launched.GetComponent<MunitionScript>().activatorDroneDamage = damage;

                        launched.GetComponent<Rigidbody>().AddForce(attackPoint.transform.forward * shotForceForward, ForceMode.VelocityChange);


                        launcherFlash.Play();
                    }

                    else
                    {
                        fireAgain = 0f;

                        //Produces the Bullet Trail
                        GameObject start = new GameObject();
                        GameObject.Destroy(start, 0.1f);

                        start.name = "Trail";
                        start.AddComponent<LineRenderer>();
                        start.GetComponent<LineRenderer>().startWidth = 0.1f;
                        start.GetComponent<LineRenderer>().endWidth = 0.1f;
                        start.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                        start.GetComponent<LineRenderer>().material = bulletTrail;
                        start.GetComponent<LineRenderer>().SetPosition(0, attackPoint.transform.position);

                        start.gameObject.transform.position = hit.point;
                        start.GetComponent<LineRenderer>().SetPosition(1, hit.point);

                        if (hit.collider.GetComponent<EnemyHealthScript>() != null)
                        {
                            if (hit.collider.GetComponent<EnemyHealthScript>().isImmune)
                            {
                                string indent = new string(' ', hostWeapon.GetComponent<FirearmScript>().currentDPSLine.Split('\n').Length * hostWeapon.GetComponent<FirearmScript>().indentSpace);
                                hostWeapon.GetComponent<FirearmScript>().newDPSLine = indent + "Immune";
                                hostWeapon.GetComponent<FirearmScript>().currentDPSLine = hostWeapon.GetComponent<FirearmScript>().newDPSLine + "\n" + hostWeapon.GetComponent<FirearmScript>().currentDPSLine;
                                hostWeapon.GetComponent<FirearmScript>().dpsText.GetComponent<Text>().text = hostWeapon.GetComponent<FirearmScript>().currentDPSLine;
                                hostWeapon.GetComponent<FirearmScript>().dpsText.GetComponent<TextClearScript>().clearTimer = hostWeapon.GetComponent<FirearmScript>().dpsText.GetComponent<TextClearScript>().timerReset;
                                hostWeapon.GetComponent<FirearmScript>().dpsLinesClear = hostWeapon.GetComponent<FirearmScript>().dpsLinesReset;

                                hostWeapon.GetComponent<FirearmScript>().DPSNumbers.text = "Immune";
                            }

                            else
                            {
                                string indent = new string(' ', hostWeapon.GetComponent<FirearmScript>().currentDPSLine.Split('\n').Length * hostWeapon.GetComponent<FirearmScript>().indentSpace);
                                hostWeapon.GetComponent<FirearmScript>().newDPSLine = indent + damage.ToString();
                                hostWeapon.GetComponent<FirearmScript>().currentDPSLine = hostWeapon.GetComponent<FirearmScript>().newDPSLine + "\n" + hostWeapon.GetComponent<FirearmScript>().currentDPSLine;
                                hostWeapon.GetComponent<FirearmScript>().dpsText.GetComponent<Text>().text = hostWeapon.GetComponent<FirearmScript>().currentDPSLine;
                                hostWeapon.GetComponent<FirearmScript>().dpsText.GetComponent<TextClearScript>().clearTimer = hostWeapon.GetComponent<FirearmScript>().dpsText.GetComponent<TextClearScript>().timerReset;
                                hostWeapon.GetComponent<FirearmScript>().dpsLinesClear = hostWeapon.GetComponent<FirearmScript>().dpsLinesReset;

                                hostWeapon.GetComponent<FirearmScript>().DPSNumbers.text = damage.ToString();
                                //Instantiate(targets[t].GetComponent<EnemyHealthScript>().blood, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
                            }

                            if (siphonicFlag)
                            {
                                hostWeapon.GetComponent<ActivatorDrone>().ActivateSiphonicPlatform();
                            }

                            if (miningFlag)
                            {
                                hostWeapon.gameObject.GetComponent<MiningPlatform>().clusterPosition = hit.point + (hit.normal * 0.01f);
                                hostWeapon.GetComponent<ActivatorDrone>().ActivateMiningPlatform();
                            }

                            if (trenchantFlag)
                            {
                                hostWeapon.gameObject.GetComponent<TrenchantPlatform>().enemy = hit.collider.gameObject;
                                hostWeapon.GetComponent<ActivatorDrone>().ActivateTrenchantPlatform();
                            }

                            if (fatedFlag && hostWeapon.GetComponent<Efficacy>())
                            {
                                hostWeapon.GetComponent<Efficacy>().RemoteProc();
                            }

                            if (fatedFlag && hostWeapon.GetComponent<Cadence>())
                            {
                                hostWeapon.GetComponent<Cadence>().clusterPosition = hit.collider.transform.position + (Vector3.up * 0.01f);
                                hostWeapon.GetComponent<Cadence>().RemoteProc();
                            }

                            if (fatedFlag && hostWeapon.GetComponent<Enshroud>())
                            {
                                hostWeapon.GetComponent<Enshroud>().RemoteProc();
                            }

                            hit.collider.GetComponent<EnemyHealthScript>().inflictDamage(damage);
                            if(hit.collider.GetComponent<EnemyHealthScript>().healthCurrent <= 0f)
                            {
                                if(fatedFlag && hostWeapon.GetComponent<WaitNowImReady>())
                                {
                                    hostWeapon.GetComponent<WaitNowImReady>().RemoteProc();
                                }                             

                                if (fatedFlag && hostWeapon.GetComponent<Inoculated>())
                                {
                                    hostWeapon.GetComponent<Inoculated>().RemoteProc();
                                }
                               
                            }
                        }

                        muzzleFlash.Play();
                    }                  
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            if(!targets.Contains(other.gameObject))
            {
                targets.Add(other.gameObject);

                if(!engagedTarget)
                {
                    engagedTarget = other.gameObject;
                }
            }
        }
    }

    private void EnemyHealthAssessment()
    {
        if(targets.Count >= 1)
        {
            for(int c = 0; c < targets.Count; c++)
            {
                if(targets[c].GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                {
                    targets.Remove(targets[c]);                  
                }
            }          
        }

        if (engagedTarget != null)
        {
            if(engagedTarget.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
            {
                engagedTarget = null;
                EnemyTargetAcquisiion();
            }         
        }
    }

    public void EnemyTargetAcquisiion()
    {
        if (!engagedTarget && targets.Count >= 1)
        {
            targetChoice = Random.Range(0, targets.Count);
            engagedTarget = targets[targetChoice];
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (targets.Contains(other.gameObject))
            {
                if (other.gameObject == engagedTarget)
                {
                    engagedTarget = null;
                    EnemyTargetAcquisiion();
                }

                targets.Remove(other.gameObject);
                
            }
        }
    }
}
