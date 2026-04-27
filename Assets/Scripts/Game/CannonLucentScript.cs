using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonLucentScript : MonoBehaviour
{
    public bool active = false;
    public GameObject player;
    public GameObject boss;
    public int spectrumThreatCount = 1;
    public int spectrumLucentCount = 0;
    public int allowableLucent = 10;
    public Transform raycastPoint;
    public LayerMask contactOnly;
    public RaycastHit hit;
    
    private GameObject activation; //VFX used to convey activity
    private GameObject activationAOE; //VFX used around Player
    private Material spectrumThreat; //LineRenderer Material for bullet visual
    private Material spectrumNormal; //LineRenderer Material for bullet visual


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
        activation = Resources.Load<GameObject>("Particles/CannonLucentLethalActive");
        activationAOE = Resources.Load<GameObject>("Particles/CannonLucentLethalAOE");

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

                GameObject effect = Instantiate(activation, raycastPoint.position, raycastPoint.rotation);
                effect.name = activation.name;

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
                            Debug.Log("player hit");
                        }
                    }

                    GameObject shot = Instantiate(activationAOE, raycastPoint.position, raycastPoint.rotation);
                    shot.name = activationAOE.name;

                    start.gameObject.transform.position = raycastPoint.position;
                    start.GetComponent<LineRenderer>().SetPosition(1, hit.point);

                    superweaponCharge = 0f;
                    spectrumThreatCount = 0;
                    spectrumLucentCount = 0;
                    clusterCount = 0;

                    boss.GetComponent<ReplevinScript>().collectionTimer = boss.GetComponent<ReplevinScript>().collectionTimerReset;
                    boss.GetComponent<ReplevinScript>().RestartSpectrumSpawners();

                    active = false;
                }
            }

            else
            {

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
}
