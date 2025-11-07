using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTraversalScript : MonoBehaviour
{
    public List<GameObject> lerps = new List<GameObject>(); //List of gameObjects intended to Lerp

    //speedUp - causes lerping object to speed up if true
    //slowDown - causes lerping object to slow down if true
    public bool speedUp, slowDown = false;
    public float newSpeed = 0f;
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
            //Makes objects from lerps list speed up
            if(speedUp)
            {
                for (int l = 0; l < lerps.Count; l++)
                {
                    lerps[l].GetComponent<LerpScript>().targetSpeed = newSpeed;
                    lerps[l].GetComponent<LerpScript>().accelerating = true;
                }
            }

            //Makes objects from lerps list slow down
            if (slowDown)
            {
                for (int l = 0; l < lerps.Count; l++)
                {
                    lerps[l].GetComponent<LerpScript>().targetSpeed = newSpeed;
                    lerps[l].GetComponent<LerpScript>().decelerating = true;
                }
            }
        }
    }
}
