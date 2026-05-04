using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumLucentScript : MonoBehaviour
{
    public bool training = false;
    public bool converted = false;
    public GameObject primaryColorChange;
    public GameObject secondaryColorChange;
    public Material convertedPrimary, convertedSecondary;
    public GameObject shatterEffect; //VFX that plays on condition
    public GameObject threatShatterEffect; //VFX that plays on condition

    // Start is called before the first frame update
    void Start()
    {
        if(!training)
        {
            Destroy(gameObject, 10f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Changes cluster state from lethal to normal -- Required for Boss mechanic
    /// </summary>
    public void ConvertCluster()
    {
        converted = true;
        primaryColorChange.GetComponent<MeshRenderer>().material = convertedPrimary;
        secondaryColorChange.GetComponent<MeshRenderer>().material = convertedSecondary;

        if(training)
        {
            StartCoroutine(Shatter());
        }
    }

    public IEnumerator Shatter()
    {
        yield return new WaitForSeconds(1f);

        GameObject effect;

        if(!converted)
        {
            effect = Instantiate(threatShatterEffect, transform.position, Quaternion.identity);
        }

        else
        {
            effect = Instantiate(shatterEffect, transform.position, Quaternion.identity);
        }

        effect.name = "Shatter VFX";

        Destroy(gameObject);
    }

    public void ShatterImmediate()
    {

        GameObject effect;

        if (!converted)
        {
            effect = Instantiate(threatShatterEffect, transform.position, Quaternion.identity);
        }

        else
        {
            effect = Instantiate(shatterEffect, transform.position, Quaternion.identity);
        }

        effect.name = "Shatter VFX";

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<ReplevinScript>())
            {
                if (other.gameObject.GetComponent<ReplevinScript>().amBoss)
                {
                    if (!converted)
                    {
                        other.gameObject.GetComponent<ReplevinScript>().spectrumCannon.GetComponent<CannonLucentScript>().spectrumThreatCount++;
                    }

                    else
                    {
                        other.gameObject.GetComponent<ReplevinScript>().spectrumCannon.GetComponent<CannonLucentScript>().spectrumLucentCount++;
                    }

                    other.gameObject.GetComponent<ReplevinScript>().spectrumCannon.GetComponent<CannonLucentScript>().clusterCount++;
                    ShatterImmediate();
                }               
            }       
        }
    }
}
