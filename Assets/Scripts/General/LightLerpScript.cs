using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLerpScript : MonoBehaviour
{
    public Color colorOne = Color.gray;
    public Color colorTwo = Color.gray;
    public GameObject light;
    public bool single, reverse, loop;
    public bool forColor, forIntensity;
    public float accelerant = 1f;
    public float newIntensity, originalIntensity;
    public float progress = 0f;
    // Start is called before the first frame update
    void Start()
    {
        originalIntensity = light.GetComponent<Light>().intensity;
    }

    // Update is called once per frame
    void Update()
    {
        progress = Mathf.Clamp(progress, 0f, 1f);

        if (single)
        {
            if(forColor)
            {
                light.GetComponent<Light>().color = Color.Lerp(colorOne, colorTwo, progress += Time.deltaTime * accelerant);
            }
            
            if(forIntensity)
            {
                light.GetComponent<Light>().intensity = Mathf.Lerp(originalIntensity, newIntensity, progress += Time.deltaTime * accelerant);
            }
        }

        else if (reverse)
        {
            if(forColor)
            {
                light.GetComponent<Light>().color = Color.Lerp(colorOne, colorTwo, progress -= Time.deltaTime * accelerant);
            }

            if(forIntensity)
            {
                light.GetComponent<Light>().intensity = Mathf.Lerp(originalIntensity, newIntensity, progress -= Time.deltaTime * accelerant);
            }
        }

        else if(loop)
        {
            if(forColor)
            {
                light.GetComponent<Light>().color = Color.Lerp(colorOne, colorTwo, Mathf.PingPong(Time.time, progress * accelerant));
            }           
        }
    }

    public IEnumerator ClearSettings()
    {
        yield return new WaitForSeconds(1f);
        single = false;
        reverse = false;
        loop = false;
        forColor = false;
        forIntensity = false;
    }
}
