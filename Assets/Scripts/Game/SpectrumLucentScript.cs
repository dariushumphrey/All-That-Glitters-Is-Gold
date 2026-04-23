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

    // Start is called before the first frame update
    void Start()
    {
        
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

    IEnumerator Shatter()
    {
        yield return new WaitForSeconds(1f);

        GameObject effect = Instantiate(shatterEffect, transform.position, Quaternion.identity);
        effect.name = "Shatter VFX";

        Destroy(gameObject);
    }
}
