using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorConsoleScript : MonoBehaviour
{
    public bool blueConsole, redConsole = false;
    public bool accepted;
    public Material activeColor;
    public GameObject door;
    public GameObject console;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (blueConsole && other.gameObject.GetComponent<PlayerInventoryScript>().blueKey)
            {
                accepted = true;
                console.GetComponent<MeshRenderer>().material = activeColor;
            }

            if (redConsole && other.gameObject.GetComponent<PlayerInventoryScript>().redKey)
            {
                accepted = true;
                console.GetComponent<MeshRenderer>().material = activeColor;
            }
        }
    }
}
