using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorConsoleScript : MonoBehaviour
{
    public bool blueConsole, redConsole = false;
    public bool accepted; //Console has received a key and unlocked a door if true
    public Material activeColor;
    public GameObject blueKeyDisplay, redKeyDisplay;
    public GameObject doorObject;

    private MeshRenderer consoleObject;
    private Material[] consoleMaterials;
    private Material[] doorMaterials;

    // Start is called before the first frame update
    void Start()
    {
        consoleObject = GetComponentInParent<MeshRenderer>();
        consoleMaterials = consoleObject.materials;
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
                consoleMaterials[1] = activeColor;
                consoleObject.materials = consoleMaterials;
                blueKeyDisplay.SetActive(true);

                gameObject.SetActive(false);
                //console.GetComponent<MeshRenderer>().material = activeColor;
            }

            if (redConsole && other.gameObject.GetComponent<PlayerInventoryScript>().redKey)
            {
                accepted = true;
                consoleMaterials[1] = activeColor;
                consoleObject.materials = consoleMaterials;
                redKeyDisplay.SetActive(true);

                gameObject.SetActive(false);
                //console.GetComponent<MeshRenderer>().material = activeColor;
            }
        }
    }
}
