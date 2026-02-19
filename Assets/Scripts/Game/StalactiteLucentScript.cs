using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteLucentScript : MonoBehaviour
{
    public int shatterDamage;
    public GameObject lucentCluster; //Lucent game object
    public GameObject raycastPoint;
    public Collider field; //Zone for cluster spawning
    public bool hostCrystal, fragment;
    public LayerMask contactOnly;
    private int shatterDamageAdd;
    private float shatterPercent = 250f;
    private bool cooldown = false;
    private Bounds spawnField; //Bounds that clusters spawn within
    private RaycastHit hit;
    private GameObject lucentShard; //Lucent game object
    private GameObject shatterEffect; //VFX used to convey activity

    // Start is called before the first frame update
    void Start()
    {
        shatterEffect = Resources.Load<GameObject>("Particles/LucentShatterEffect");

        ShatterCalculation();

        if(fragment)
        {
            Destroy(gameObject, 2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fragment)
        {
            if(Physics.Raycast(raycastPoint.transform.position, raycastPoint.transform.forward, out hit, 0.5f, contactOnly))
            {
                Vector3 epicenter = transform.position;
                Collider[] affected = Physics.OverlapSphere(transform.position, 10f);
                foreach (Collider hit in affected)
                {
                    if (hit.gameObject.CompareTag("Enemy"))
                    {
                        //Triggers stun mechanic on Bosses
                        if (hit.gameObject.GetComponent<ReplevinScript>().amBoss)
                        {
                            hit.gameObject.GetComponent<ReplevinScript>().enemy.isImmune = false;
                        }

                        if (hit.GetComponent<EnemyHealthScript>() != null)
                        {
                            hit.GetComponent<EnemyHealthScript>().inflictDamage(shatterDamage);
                            if (hit.GetComponent<EnemyHealthScript>().healthCurrent <= 0 && hit.GetComponent<Rigidbody>() == null)
                            {
                                hit.gameObject.AddComponent<Rigidbody>();
                                hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
                            }
                        }
                    }

                    if (hit.gameObject.CompareTag("Lucent"))
                    {
                        hit.gameObject.GetComponent<LucentScript>().StartCoroutine(hit.gameObject.GetComponent<LucentScript>().Shatter());
                    }
                }

                GameObject vfx = Instantiate(shatterEffect, transform.position + (Vector3.up * 0.01f), Quaternion.identity);
                vfx.name = shatterEffect.name;

                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Produces Lucent Clusters
    /// </summary>
    public void LucentPassive()
    {      
        if(!cooldown)
        {
            spawnField = field.bounds;
            Vector3 spawnSite = spawnField.center + new Vector3(Random.Range(-spawnField.extents.x, spawnField.extents.x),
                                                                Random.Range(-spawnField.extents.y, spawnField.extents.y),
                                                                Random.Range(-spawnField.extents.z, spawnField.extents.z));

            lucentShard = Instantiate(lucentCluster, spawnSite, transform.rotation);
            lucentShard.name = lucentCluster.name;
            lucentShard.GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);

            cooldown = true;
            StartCoroutine(CooldownRemovalDelay());
        }     
    }

    public IEnumerator CooldownRemovalDelay()
    {
        yield return new WaitForSeconds(0.04f);
        cooldown = false;
    }

    /// <summary>
    /// Calculates cluster shatter damage
    /// </summary>
    public void ShatterCalculation()
    {
        shatterPercent /= 100;
        shatterPercent *= shatterDamage;
        shatterDamageAdd = (int)shatterPercent;
        shatterDamage += shatterDamageAdd;
    }
}
