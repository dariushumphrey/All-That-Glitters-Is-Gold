using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public int rotationAccelerant;
    public float automaticX, automaticY, automaticZ;

    private Vector3 rotationVector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 1)
        {
            rotationVector = new Vector3(automaticX, automaticY, automaticZ);
            transform.Rotate(rotationVector * rotationAccelerant);
        }
    }
}
