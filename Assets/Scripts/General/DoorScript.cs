using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject door;
    public List<GameObject> console = new List<GameObject>();
    public float openConstraint = 4f;
    public float closedConstraint = 0f;
    public int openSpeed = 5;
    public bool proximity = false;
    public bool locked = false;
    public bool elevator = false;
    public bool invert = false;

    private float state = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!proximity)
        {
            if(invert)
            {
                state += openSpeed * Time.deltaTime;
            }

            else
            {
                state -= openSpeed * Time.deltaTime;
            }

            state = Mathf.Clamp(state, closedConstraint, openConstraint);

            door.transform.position = new Vector3(door.transform.position.x, state, door.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && elevator)
        {
            other.gameObject.transform.parent = door.gameObject.transform;
        }

        if (other.gameObject.tag == "Player" && locked)
        {
            for (int i = 0; i < console.Count; i++)
            {
                if (console[i].GetComponent<DoorConsoleScript>().accepted == true)
                {
                    console.Remove(console[i]);                  
                }
            }

            if(console.Count <= 0)
            {
                locked = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {     
        if (other.gameObject.tag == "Player" && !locked || other.gameObject.tag == "Enemy" && !locked)
        {
            proximity = true;

            if(invert)
            {
                state -= openSpeed * Time.deltaTime;
            }

            else
            {
                state += openSpeed * Time.deltaTime;
            }

            state = Mathf.Clamp(state, closedConstraint, openConstraint);

            door.transform.position = new Vector3(door.transform.position.x, state, door.transform.position.z);           
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && elevator)
        {
            other.gameObject.transform.parent = null;
        }

        if (other.gameObject.tag == "Player" && !locked || other.gameObject.tag == "Enemy" && !locked)
        {
            proximity = false;
        }
    }
}
