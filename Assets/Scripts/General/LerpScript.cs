using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpScript : MonoBehaviour
{
    public Transform positionOne, positionTwo;
    public GameObject thing;
    public float rate = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        thing.transform.position = Vector3.Lerp(positionOne.transform.position, positionTwo.transform.position, rate);
    }
}
