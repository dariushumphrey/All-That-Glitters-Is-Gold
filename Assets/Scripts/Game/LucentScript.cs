using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucentScript : MonoBehaviour
{
    public int lucentGift;
    public bool threat = false;
    public GameObject shatterEffect;

    private float shatterPercent = 150f;
    private int shatterDamage;
    private bool cascade = false;
    internal bool shot = false;
    internal float shatterDelayTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shot)
        {
            Vector3 epicenter = transform.position;
            Collider[] affected = Physics.OverlapSphere(transform.position, 5f);
            foreach (Collider hit in affected)
            {
                //Debug.Log(affected.Length);

                if(hit.gameObject.CompareTag("Enemy"))
                {
                    if(gameObject.transform.parent != null && gameObject.transform.parent.GetComponent<ReplevinScript>().amBoss)
                    {
                        if(gameObject.GetComponent<Rigidbody>() == null)
                        {
                            gameObject.AddComponent<Rigidbody>();
                        }

                        gameObject.transform.parent.GetComponent<ReplevinScript>().stunMechanic = null;
                        //gameObject.transform.parent.GetComponent<ReplevinScript>().stunMechanic.transform.parent = null;
                        gameObject.transform.parent.GetComponent<ReplevinScript>().enemy.isImmune = false;

                        if (gameObject.transform.parent.GetComponent<ReplevinScript>().interrupted == false)
                        {
                            gameObject.transform.parent.GetComponent<ReplevinScript>().interrupted = true;

                        }
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

                if(hit.gameObject.CompareTag("Lucent"))
                {
                    //hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
                    hit.gameObject.GetComponent<LucentScript>().StartCoroutine(hit.gameObject.GetComponent<LucentScript>().Shatter());
                }

                //Rigidbody inflict = hit.GetComponent<Rigidbody>();
                //if (inflict != null)
                //{
                //    if (inflict.GetComponent<EnemyHealthScript>() != null)
                //    {
                //        inflict.GetComponent<EnemyHealthScript>().inflictDamage(shatterDamage);
                //    }
                //}
            }

            GameObject effect = Instantiate(shatterEffect, transform.position, Quaternion.identity);
            effect.name = "Shatter VFX";

            Destroy(gameObject);
        }      
    }
    
    public void ShatterCalculation()
    {
        shatterPercent /= 100;
        shatterPercent *= lucentGift;
        shatterDamage = (int)shatterPercent;
    }

    public IEnumerator Shatter()
    {
        if(gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObject.GetComponent<Rigidbody>().AddExplosionForce(10f, transform.position, 10f, 50f);
        }

        yield return new WaitForSeconds(shatterDelayTime);
        Vector3 epicenter = transform.position;
        Collider[] affected = Physics.OverlapSphere(transform.position, 5f);
        foreach (Collider hit in affected)
        {
            if(hit.gameObject.CompareTag("Player") && threat)
            {
                ShatterCalculation();
                hit.gameObject.GetComponent<PlayerStatusScript>().InflictDamage(shatterDamage);
                hit.gameObject.GetComponent<PlayerStatusScript>().playerHit = true;
            }

            if (hit.gameObject.CompareTag("Enemy"))
            {
                if (gameObject.transform.parent != null && gameObject.transform.parent.GetComponent<ReplevinScript>().amBoss)
                {
                    if (gameObject.GetComponent<Rigidbody>() == null)
                    {
                        gameObject.AddComponent<Rigidbody>();
                    }

                    gameObject.transform.parent.GetComponent<ReplevinScript>().stunMechanic = null;
                    //gameObject.transform.parent.GetComponent<ReplevinScript>().stunMechanic.transform.parent = null;
                    gameObject.transform.parent.GetComponent<ReplevinScript>().enemy.isImmune = false;
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
                hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(100f, transform.position, 10f, 100f);
                hit.gameObject.GetComponent<LucentScript>().StartCoroutine(hit.gameObject.GetComponent<LucentScript>().Shatter());

            }
        }

        GameObject effect = Instantiate(shatterEffect, transform.position, Quaternion.identity);
        effect.name = "Shatter VFX";
        Destroy(gameObject);
    }
}
