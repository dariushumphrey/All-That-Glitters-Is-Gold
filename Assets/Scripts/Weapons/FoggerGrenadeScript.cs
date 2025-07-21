using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoggerGrenadeScript : MonoBehaviour
{
    public float armingTime;
    public GameObject smokePlume, smokeRadius;
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
        smokePlume.GetComponent<ParticleSystem>().Play();

        GameObject radius = Instantiate(smokeRadius, transform.position, Quaternion.identity);
        radius.name = smokeRadius.name;
        radius.GetComponent<ParticleSystem>().Play();
        radius.AddComponent<DestroyScript>();
        radius.GetComponent<DestroyScript>().destroyTimer = 22f;

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
        }
    }
}
