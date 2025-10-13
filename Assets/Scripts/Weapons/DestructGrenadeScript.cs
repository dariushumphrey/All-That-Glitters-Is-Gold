using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructGrenadeScript : MonoBehaviour
{
    public int explosiveDamage;
    public float explosiveRange = 8f;
    public float armingTime;
    public GameObject destructFlash, destructSmoke, destructSparks;
    public GameObject detonationEffect;
    public LayerMask contactOnly;
    private bool hitOnce = false;
    internal int explosiveDamageReset;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator SetupGrenade()
    {
        yield return new WaitForSeconds(armingTime);
        //destructFlash.GetComponent<ParticleSystem>().Play();

        //GameObject flash = Instantiate(destructFlash, transform.position, Quaternion.identity);
        //flash.name = destructFlash.name;
        //flash.GetComponent<ParticleSystem>().Play();
        //flash.AddComponent<DestroyScript>();
        //flash.GetComponent<DestroyScript>().destroyTimer = 2f;

        //GameObject smoke = Instantiate(destructSmoke, transform.position, Quaternion.identity);
        //smoke.name = destructSmoke.name;
        //smoke.GetComponent<ParticleSystem>().Play();
        //smoke.AddComponent<DestroyScript>();
        //smoke.GetComponent<DestroyScript>().destroyTimer = 2f;

        //GameObject sparks = Instantiate(destructSparks, transform.position, Quaternion.identity);
        //sparks.name = destructSparks.name;
        //sparks.GetComponent<ParticleSystem>().Play();
        //sparks.AddComponent<DestroyScript>();
        //sparks.GetComponent<DestroyScript>().destroyTimer = 2f;

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject.GetComponent<CapsuleCollider>());

        //gameObject.AddComponent<SphereCollider>();
        //gameObject.GetComponent<SphereCollider>().radius = explosiveRange;
        //gameObject.GetComponent<SphereCollider>().isTrigger = true;

        RaycastHit hit;
        Vector3 epicenter = transform.position;
        Collider[] affected = Physics.OverlapSphere(transform.position, explosiveRange);
        foreach (Collider contact in affected)
        {
            if (!Physics.Raycast(transform.position, (contact.transform.position - transform.position).normalized, out hit, explosiveRange, contactOnly))
            {
                if (contact.gameObject.CompareTag("Enemy"))
                {
                    if (contact.GetComponent<EnemyHealthScript>() != null)
                    {
                        contact.GetComponent<EnemyHealthScript>().inflictDamage(explosiveDamage);
                        if(contact.GetComponent<EnemyHealthScript>().healthCurrent <= 0 && contact.GetComponent<Rigidbody>() == null)                    
                        {
                            contact.gameObject.AddComponent<Rigidbody>();
                            contact.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
                        }
                    }
                }

                if(contact.gameObject.CompareTag("Lucent"))
                {
                    contact.gameObject.GetComponent<LucentScript>().lucentGift = 0;
                    contact.gameObject.GetComponent<LucentScript>().shot = true;
                }
            }
        }

        GameObject effect = Instantiate(detonationEffect, transform.position, Quaternion.identity);
        effect.name = "Detonation VFX";
        Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!hitOnce)
        {
            StartCoroutine(SetupGrenade());
            hitOnce = true;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    RaycastHit hit;
    //    Vector3 epicenter = transform.position;
    //    Collider[] affected = Physics.OverlapSphere(transform.position, explosiveRange);
    //    foreach (Collider contact in affected)
    //    {
    //        if(!Physics.Raycast(transform.position, (contact.transform.position - transform.position).normalized, out hit, explosiveRange, contactOnly))
    //        {
    //            if (contact.gameObject.CompareTag("Enemy"))
    //            {
    //                if (contact.GetComponent<EnemyHealthScript>() != null)
    //                {
    //                    contact.GetComponent<EnemyHealthScript>().inflictDamage(explosiveDamage);

    //                }
    //            }
    //        }                
    //    }

    //}
}
