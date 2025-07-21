using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructGrenadeScript : MonoBehaviour
{
    public int explosiveDamage;
    public float explosiveRange = 8f;
    public float armingTime;
    public GameObject destructFlash, destructSmoke, destructSparks;
    public LayerMask contactOnly;
    private bool hitOnce = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SetupGrenade()
    {
        yield return new WaitForSeconds(armingTime);
        //destructFlash.GetComponent<ParticleSystem>().Play();

        GameObject flash = Instantiate(destructFlash, transform.position, Quaternion.identity);
        flash.name = destructFlash.name;
        flash.GetComponent<ParticleSystem>().Play();
        flash.AddComponent<DestroyScript>();
        flash.GetComponent<DestroyScript>().destroyTimer = 2f;

        GameObject smoke = Instantiate(destructSmoke, transform.position, Quaternion.identity);
        smoke.name = destructSmoke.name;
        smoke.GetComponent<ParticleSystem>().Play();
        smoke.AddComponent<DestroyScript>();
        smoke.GetComponent<DestroyScript>().destroyTimer = 2f;

        GameObject sparks = Instantiate(destructSparks, transform.position, Quaternion.identity);
        sparks.name = destructSparks.name;
        sparks.GetComponent<ParticleSystem>().Play();
        sparks.AddComponent<DestroyScript>();
        sparks.GetComponent<DestroyScript>().destroyTimer = 2f;

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject.GetComponent<CapsuleCollider>());

        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().radius = explosiveRange;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;

        Destroy(gameObject, 0.1f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!hitOnce)
        {
            StartCoroutine(SetupGrenade());
            hitOnce = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        RaycastHit hit;
        Vector3 epicenter = transform.position;
        Collider[] affected = Physics.OverlapSphere(transform.position, explosiveRange);
        foreach (Collider contact in affected)
        {
            if(!Physics.Raycast(transform.position, (contact.transform.position - transform.position).normalized, out hit, explosiveRange, contactOnly))
            {
                if (contact.gameObject.CompareTag("Enemy"))
                {
                    if (contact.GetComponent<EnemyHealthScript>() != null)
                    {
                        contact.GetComponent<EnemyHealthScript>().inflictDamage(explosiveDamage);

                    }
                }
            }                
        }

    }
}
