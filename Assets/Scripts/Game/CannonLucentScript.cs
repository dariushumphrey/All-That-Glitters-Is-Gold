using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonLucentScript : MonoBehaviour
{
    public int damage;
    public float damagePercent = 2000f;
    public int mechanicDamage;

    public bool active = false;
    public GameObject player;
    public GameObject boss;
    public int spectrumThreatCount = 1;
    public int spectrumLucentCount = 0;
    public int allowableLucent = 10;
    public Transform raycastPoint;
    public LayerMask contactOnly;
    public RaycastHit hit;

    public GameObject threatActivation; //VFX used to convey activity
    private GameObject threatActivationAOE; //VFX used around Player
    private Material spectrumThreat; //LineRenderer Material for bullet visual
    private GameObject threatLight; //Light object used to visualize charge

    public Material spectrumNormal; //LineRenderer Material for bullet visual
    private GameObject normalActivation; //VFX used to convey activity
    private GameObject normalAOE; //VFX used around Player
    private GameObject spectrumLight; //Light object used to visualize charge

    private GameObject light; //Holds reference to spawned light
    private float intensityMax = 50f; //Maximum light intensity for Superweapon charge
    private GameObject shatterEffect; //VFX that plays on condition


    private Vector3 distance, bossDistance;
    private float superweaponCharge = 0f; //Current charge of Superweapon attack
    private float superweaponReset; //Holds starting Superweapon damage
    private float rotationStrength = 2f; //Governs turning speed of Enemy while Player is in range
    internal int clusterCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        superweaponReset = superweaponCharge;

        spectrumThreat = Resources.Load<Material>("Materials/Weapons/SuperweaponShotMaterial");
        threatActivation = Resources.Load<GameObject>("Particles/CannonLucentLethalActive");
        threatActivationAOE = Resources.Load<GameObject>("Particles/CannonLucentLethalAOE");
        threatLight = Resources.Load<GameObject>("Particles/Lights/lucentShatterThreatIntensityLight");

        spectrumNormal = Resources.Load<Material>("Materials/Weapons/CannonLucentShotMaterial");
        normalActivation = Resources.Load<GameObject>("Particles/CannonLucentActive");
        normalAOE = Resources.Load<GameObject>("Particles/CannonLucentAOE");
        spectrumLight = Resources.Load<GameObject>("Particles/Lights/lucentShatterIntensityLight");

        shatterEffect = Resources.Load<GameObject>("Particles/LucentShatterEffect");
    }

    // Update is called once per frame
    void Update()
    {

        if(active)
        {
            if (spectrumThreatCount > spectrumLucentCount)
            {
                distance = player.transform.position - raycastPoint.position;
                raycastPoint.rotation = Quaternion.Lerp(raycastPoint.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

                superweaponCharge += Time.deltaTime * 30;

                GameObject effect = Instantiate(threatActivation, raycastPoint.position, raycastPoint.rotation);
                effect.name = threatActivation.name;

                if (!light)
                {
                    GameObject shiner = Instantiate(threatLight, raycastPoint.position, transform.rotation, raycastPoint.transform);
                    shiner.name = threatLight.name;
                    light = shiner;

                    light.GetComponent<Light>().intensity = 1;
                }

                else
                {
                    float newIntensity = (superweaponCharge / 100) * intensityMax;
                    light.GetComponent<Light>().intensity = newIntensity;
                }

                if (superweaponCharge >= 100f)
                {
                    GameObject start = new GameObject();
                    GameObject.Destroy(start, 0.3f);

                    start.name = "Trail";
                    start.AddComponent<LineRenderer>();
                    start.GetComponent<LineRenderer>().startWidth = 1f;
                    start.GetComponent<LineRenderer>().endWidth = 1f;
                    start.GetComponent<LineRenderer>().widthMultiplier = 10f;
                    start.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    start.GetComponent<LineRenderer>().material = spectrumThreat;
                    start.GetComponent<LineRenderer>().SetPosition(0, raycastPoint.position);

                    if (Physics.Raycast(raycastPoint.position, raycastPoint.forward, out hit, Mathf.Infinity, contactOnly))
                    {
                        if(hit.collider.CompareTag("Player"))
                        {
                            hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                        }

                        if (hit.collider.CompareTag("Hard Lucent"))
                        {
                            GameObject obliterate = Instantiate(shatterEffect, hit.collider.transform.position, Quaternion.identity);
                            hit.collider.gameObject.SetActive(false);
                        }
                    }

                    GameObject shot = Instantiate(threatActivationAOE, raycastPoint.position, raycastPoint.rotation);
                    shot.name = threatActivationAOE.name;

                    start.gameObject.transform.position = raycastPoint.position;
                    start.GetComponent<LineRenderer>().SetPosition(1, hit.point);

                    superweaponCharge = 0f;
                    spectrumThreatCount = 0;
                    spectrumLucentCount = 0;
                    clusterCount = 0;

                    boss.GetComponent<ReplevinScript>().collectionTimer = boss.GetComponent<ReplevinScript>().collectionTimerReset;
                    boss.GetComponent<ReplevinScript>().RestartSpectrumSpawners();

                    if (light)
                    {
                        Destroy(light, 0.1f);
                    }

                    active = false;
                }
            }

            else
            {
                bossDistance = boss.transform.position - raycastPoint.position;
                raycastPoint.rotation = Quaternion.Lerp(raycastPoint.rotation, Quaternion.LookRotation(bossDistance, Vector3.up), rotationStrength);

                superweaponCharge += Time.deltaTime * 90;

                GameObject effect = Instantiate(normalActivation, raycastPoint.position, raycastPoint.rotation);
                effect.name = normalActivation.name;

                if (!light)
                {
                    GameObject shiner = Instantiate(spectrumLight, raycastPoint.position, transform.rotation, raycastPoint.transform);
                    shiner.name = spectrumLight.name;
                    light = shiner;

                    light.GetComponent<Light>().intensity = 1;
                }

                else
                {
                    float newIntensity = (superweaponCharge / 100) * intensityMax;
                    light.GetComponent<Light>().intensity = newIntensity;
                }

                if (superweaponCharge >= 100f)
                {
                    GameObject start = new GameObject();
                    GameObject.Destroy(start, 0.3f);

                    start.name = "Trail";
                    start.AddComponent<LineRenderer>();
                    start.GetComponent<LineRenderer>().startWidth = 1f;
                    start.GetComponent<LineRenderer>().endWidth = 1f;
                    start.GetComponent<LineRenderer>().widthMultiplier = 10f;
                    start.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    start.GetComponent<LineRenderer>().material = spectrumNormal;
                    start.GetComponent<LineRenderer>().SetPosition(0, raycastPoint.position);

                    if (Physics.Raycast(raycastPoint.position, raycastPoint.forward, out hit, Mathf.Infinity, contactOnly))
                    {
                        if (hit.collider.CompareTag("Enemy"))
                        {
                            hit.collider.GetComponent<ReplevinScript>().interrupted = true;
                            hit.collider.GetComponent<EnemyHealthScript>().isImmune = false;
                            hit.collider.GetComponent<EnemyHealthScript>().inflictDamage(mechanicDamage);
                        }
                    }

                    GameObject shot = Instantiate(normalAOE, raycastPoint.position, raycastPoint.rotation);
                    shot.name = normalAOE.name;

                    start.gameObject.transform.position = raycastPoint.position;
                    start.GetComponent<LineRenderer>().SetPosition(1, hit.point);

                    if (light)
                    {
                        Destroy(light, 0.1f);
                    }

                    superweaponCharge = 0f;
                    spectrumThreatCount = 0;
                    spectrumLucentCount = 0;
                    clusterCount = 0;

                    active = false;
                }
            }
        }
        
        else
        {
            if(clusterCount >= allowableLucent)
            {
                clusterCount = allowableLucent;
                active = true;
            }
        }
    }

    public void DamageCalculation()
    {
        damagePercent /= 100;
        damagePercent *= damage;
        mechanicDamage = (int)damagePercent;
    }
}
