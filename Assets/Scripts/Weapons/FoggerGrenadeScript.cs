using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoggerGrenadeScript : MonoBehaviour
{
    public float armingTime; //Time delay before detonation
    public GameObject smokePlume, smokeRadius; //VFX for grenade
    private bool hitOnce = false; //Confirms at least one surface collision if true
    internal bool enshroudFlag = false; //Confirms the Cheat "Enshroud" is present if true

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Produces a Sphere collider with a 10m radius after a delay
    /// The collider is marked as a trigger to apply effects
    /// </summary>
    public IEnumerator SetupGrenade()
    {
        yield return new WaitForSeconds(armingTime);
        smokePlume.GetComponent<ParticleSystem>().Play();

        GameObject radius = Instantiate(smokeRadius, transform.position, Quaternion.identity);
        radius.name = smokeRadius.name;
        radius.GetComponent<ParticleSystem>().Play();

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject.GetComponent<CapsuleCollider>());

        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().radius = 10f;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;

        Destroy(gameObject, 21f);

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
            other.gameObject.AddComponent<SlowedScript>();
            
            //Enshroud allows Fogger Grenades to apply damage-over-time if true
            if(enshroudFlag)
            {
                if(!other.gameObject.GetComponent<DamageOverTimeScript>())
                {
                    other.gameObject.AddComponent<DamageOverTimeScript>();
                    other.gameObject.GetComponent<DamageOverTimeScript>().dotDamage = 150;
                    other.gameObject.GetComponent<DamageOverTimeScript>().damageOverTimeProc = 1f;
                }
            }
        }
    }
}