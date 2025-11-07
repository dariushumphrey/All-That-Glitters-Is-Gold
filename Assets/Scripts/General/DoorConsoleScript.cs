using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorConsoleScript : MonoBehaviour
{
    public bool blueConsole, redConsole = false;
    public bool accepted; //Console has received a key and unlocked a door if true
    public Material activeColor;
    public GameObject door; //Door to open

    private GameObject console; //Physical console to change the color of
    // Start is called before the first frame update
    void Start()
    {
        console = gameObject.transform.parent.gameObject;
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
