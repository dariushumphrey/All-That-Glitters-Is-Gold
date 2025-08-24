using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTraversalScript : MonoBehaviour
{
    public List<GameObject> lerps = new List<GameObject>();
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
            if(speedUp)
            {
                for (int l = 0; l < lerps.Count; l++)
                {
                    lerps[l].GetComponent<LerpScript>().targetSpeed = newSpeed;
                    lerps[l].GetComponent<LerpScript>().accelerating = true;
                }
            }

            if(slowDown)
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
