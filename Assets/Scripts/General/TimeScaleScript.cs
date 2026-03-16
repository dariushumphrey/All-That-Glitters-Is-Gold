using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleScript : MonoBehaviour
{
    public float timeAccelerant = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1 * timeAccelerant;
    }
}
