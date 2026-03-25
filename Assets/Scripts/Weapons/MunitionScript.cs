using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            TriggerMunition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.layer == 8 || other.gameObject.tag == "Lucent")
        {
            TriggerMunition();          
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

                if (contact.GetComponent<EnemyHealthScript>() != null)
                {
                    contact.GetComponent<EnemyHealthScript>().inflictDamage(explosiveDamage);
                    if (contact.GetComponent<EnemyHealthScript>().healthCurrent <= 0 && contact.GetComponent<Rigidbody>() == null)
                    {
                        contact.gameObject.AddComponent<Rigidbody>();
                        contact.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
                    }
                }
            }

            if (contact.CompareTag("Lucent"))
            {
                contact.gameObject.GetComponent<LucentScript>().lucentGift = 0;
                contact.gameObject.GetComponent<LucentScript>().shot = true;
            }

            //if (!Physics.Raycast(transform.position, (contact.transform.position - transform.position).normalized, out hit, explosiveRange, contactOnly))
            //{
            //    if (contact.gameObject.CompareTag("Enemy"))
            //    {
            //        if (contact.GetComponent<EnemyHealthScript>() != null)
            //        {
            //            contact.GetComponent<EnemyHealthScript>().inflictDamage(explosiveDamage);
            //            if (contact.GetComponent<EnemyHealthScript>().healthCurrent <= 0 && contact.GetComponent<Rigidbody>() == null)
            //            {
            //                contact.gameObject.AddComponent<Rigidbody>();
            //                contact.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
            //            }
            //        }
            //    }

            //    if (contact.gameObject.CompareTag("Lucent"))
            //    {
            //        contact.gameObject.GetComponent<LucentScript>().lucentGift = 0;
            //        contact.gameObject.GetComponent<LucentScript>().shot = true;
            //    }
            //}


        }

        if(targets.Count >= 1)
        {
            for(int t = 0; t < targets.Count; t++)
            {
                hostLauncher.GetComponent<LauncherFirearm>().detectedTargets.Add(targets[t]);
            }

            hostLauncher.GetComponent<LauncherFirearm>().ExtendedFunctions();
        }

        GameObject effect = Instantiate(detonationEffect, transform.position, Quaternion.identity);
        effect.name = "Munition VFX";
        //Destroy(gameObject);
        gameObject.SetActive(false);

    }
}
