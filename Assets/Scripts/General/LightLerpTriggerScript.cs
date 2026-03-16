using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLerpTriggerScript : MonoBehaviour
{
    public GameObject lightToAffect;
    public bool onSingle, onReverse, onLoop, changeColor, changeIntensity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(!lightToAffect.GetComponent<LightLerpScript>())
            {
                lightToAffect.AddComponent<LightLerpScript>();
            }

            if(onSingle)
            {
                lightToAffect.GetComponent<LightLerpScript>().single = true;

                if(changeColor)
                {
                    lightToAffect.GetComponent<LightLerpScript>().forColor = true;
                }

                else if(changeIntensity)
                {
                    lightToAffect.GetComponent<LightLerpScript>().forIntensity = true;
                }
            }

            else if (onReverse)
            {
                lightToAffect.GetComponent<LightLerpScript>().reverse = true;

                if (changeColor)
                {
                    lightToAffect.GetComponent<LightLerpScript>().forColor = true;
                }

                else if (changeIntensity)
                {
                    lightToAffect.GetComponent<LightLerpScript>().forIntensity = true;
                }
            }

            else if (onLoop)
            {
                lightToAffect.GetComponent<LightLerpScript>().loop = true;

                if (changeColor)
                {
                    lightToAffect.GetComponent<LightLerpScript>().forColor = true;
                }
            }

            lightToAffect.GetComponent<LightLerpScript>().StartCoroutine(lightToAffect.GetComponent<LightLerpScript>().ClearSettings());
            gameObject.SetActive(false);
        }
    }
}
