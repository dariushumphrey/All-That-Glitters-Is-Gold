using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucentScript : MonoBehaviour
{
    public int lucentGift;

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
        gameObject.GetComponent<Rigidbody>().AddExplosionForce(10f, transform.position, 10f, 50f);

        yield return new WaitForSeconds(shatterDelayTime);
        Vector3 epicenter = transform.position;
        Collider[] affected = Physics.OverlapSphere(transform.position, 5f);
        foreach (Collider hit in affected)
        {

            if (hit.gameObject.CompareTag("Enemy"))
            {
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

        Destroy(gameObject);
    }
}
