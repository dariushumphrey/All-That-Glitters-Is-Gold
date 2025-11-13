using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructGrenadeScript : MonoBehaviour
{
    public int explosiveDamage;
    public float explosiveRange = 8f;
    public float armingTime; //Time delay before detonation
    public GameObject detonationEffect; //VFX for grenade
    public LayerMask contactOnly; //Ensures Raycast accounts for Surfaces
    private bool hitOnce = false; //Confirms at least one surface collision if true

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Collects colliders and applies damage to Enemies, detonates Lucent in its radius
    /// </summary>
    public IEnumerator SetupGrenade()
    {
        yield return new WaitForSeconds(armingTime);

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject.GetComponent<CapsuleCollider>());

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
}