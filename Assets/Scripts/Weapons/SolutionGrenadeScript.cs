using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionGrenadeScript : MonoBehaviour
{
    public float armingTime;
    public GameObject acidPool, acidSteam, acidReaction;
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
        //acidPool.GetComponent<ParticleSystem>().Play();

        GameObject pool = Instantiate(acidPool, transform.position, Quaternion.identity);
        pool.name = acidPool.name;
        pool.GetComponent<ParticleSystem>().Play();
        pool.AddComponent<DestroyScript>();
        pool.GetComponent<DestroyScript>().destroyTimer = 24f;

        GameObject steam = Instantiate(acidSteam, transform.position, Quaternion.identity);
        steam.name = acidSteam.name;
        steam.GetComponent<ParticleSystem>().Play();
        steam.AddComponent<DestroyScript>();
        steam.GetComponent<DestroyScript>().destroyTimer = 22f;

        GameObject reaction = Instantiate(acidReaction, transform.position, Quaternion.identity);
        reaction.name = acidReaction.name;
        reaction.GetComponent<ParticleSystem>().Play();
        reaction.AddComponent<DestroyScript>();
        reaction.GetComponent<DestroyScript>().destroyTimer = 22f;

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject.GetComponent<CapsuleCollider>());

        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().radius = 7f;
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
            other.gameObject.AddComponent<DamageOverTimeScript>();
            other.gameObject.GetComponent<DamageOverTimeScript>().dotDamage = 1750;
        }

        if(other.gameObject.tag == "Lucent")
        {
            other.gameObject.GetComponent<LucentScript>().shatterDelayTime = 1f;
            other.gameObject.GetComponent<LucentScript>().StartCoroutine(other.gameObject.GetComponent<LucentScript>().Shatter());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if(other.gameObject.GetComponent<DamageOverTimeScript>())
            {
                Destroy(other.gameObject.GetComponent<DamageOverTimeScript>());
            }
        }
    }
}
