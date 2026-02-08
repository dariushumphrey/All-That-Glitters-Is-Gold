using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombustibleLucentScript : MonoBehaviour
{
    public Light lucentLight;
    public GameObject shatterEffect; //VFX that plays on condition
    public int lightIntensity = 1;
    public int primedIntensity = 4;
    public int totalUses = 2;
    public bool limitedUse = false;
    internal bool primed = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(limitedUse)
        {
            if (totalUses <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void IlluminateOnHit()
    {
        lucentLight.intensity = primedIntensity;
        primed = true;

        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().radius = 4f;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("Enemy"))
        //{
        //    if (other.gameObject.GetComponent<ReplevinScript>().amBoss)
        //    {

        //    }
        //}

        //if (other.gameObject.CompareTag("Player"))
        //{
        //    StartCoroutine(CombustOnDelay());
        //}
    }

    public void CombustOnDelay()
    {
        GameObject effect = Instantiate(shatterEffect, transform.position, Quaternion.identity);
        effect.name = "Shatter VFX";

        primed = false;
        lucentLight.intensity = lightIntensity;
        Destroy(gameObject.GetComponent<SphereCollider>());

        if(limitedUse)
        {
            totalUses--;
        }
    }
}
