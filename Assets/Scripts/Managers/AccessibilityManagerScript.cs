using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wilberforce;

public class AccessibilityManagerScript : MonoBehaviour
{
    public Camera aperture;
    public int colorblindPreference = 0;

    // Start is called before the first frame update
    void Start()
    {
        //aperture = Camera.main;

        if(!aperture)
        {
            if (!aperture.GetComponent<Colorblind>())
            {
                aperture.gameObject.AddComponent<Colorblind>();
            }
        }       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AssignNormalColor()
    {
        aperture.GetComponent<Colorblind>().Type = 0;
    }

    public void AssignProtanopiaColor()
    {
        aperture.GetComponent<Colorblind>().Type = 1;
    }

    public void AssignDeuteranopiaColor()
    {
        aperture.GetComponent<Colorblind>().Type = 2;
    }

    public void AssignTritanopiaColor()
    {
        aperture.GetComponent<Colorblind>().Type = 3;
    }

    public void AccessibilitySaving()
    {
        colorblindPreference = aperture.GetComponent<Colorblind>().Type;
        PlayerPrefs.SetInt("colorblindSetting", colorblindPreference);
    }
}
