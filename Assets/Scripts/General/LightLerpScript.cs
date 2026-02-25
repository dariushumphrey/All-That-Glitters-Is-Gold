using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLerpScript : MonoBehaviour
{
    public Color colorOne = Color.gray;
    public Color colorTwo = Color.gray;
    public GameObject light;
    public bool single, reverse, loop;
    public float accelerant = 1f;
    private float progress = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        progress = Mathf.Clamp(progress, 0f, 1f);

        if (single)
        {
            light.GetComponent<Light>().color = Color.Lerp(colorOne, colorTwo, progress += Time.deltaTime * accelerant);
        }

        else if (reverse)
        {
            light.GetComponent<Light>().color = Color.Lerp(colorOne, colorTwo, progress -= Time.deltaTime * accelerant);
        }

        else if(loop)
        {
            light.GetComponent<Light>().color = Color.Lerp(colorOne, colorTwo, Mathf.PingPong(Time.time, progress * accelerant));
        }
    }
}
