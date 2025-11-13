using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionGrenadeScript : MonoBehaviour
{
    public float armingTime; //Time delay before detonation
    public GameObject cloudDetonation; //VFX for Grenade
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
    /// Produces a Sphere collider with a 7m radius after a delay
    /// The collider is marked as a trigger to apply effects
    /// </summary>
    public IEnumerator SetupGrenade()
    {
        yield return new WaitForSeconds(armingTime);

        GameObject cloud = Instantiate(cloudDetonation, transform.position, Quaternion.identity);
        cloud.name = cloudDetonation.name;

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject.GetComponent<CapsuleCollider>());

        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().radius = 7f;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;

        Destroy(gameObject, 1f);
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
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.AddComponent<DamageOverTimeScript>();
            other.gameObject.GetComponent<DamageOverTimeScript>().dotDamage = 875;
            other.gameObject.GetComponent<DamageOverTimeScript>().damageOverTimeLength = 2f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 7f);
    }
}