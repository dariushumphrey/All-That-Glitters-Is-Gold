using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject door;
    public List<GameObject> console = new List<GameObject>();
    public float openConstraint = 4f;
    public int openSpeed = 5;
    public bool proximity = false;
    public bool locked = false;

    private float state = 0f;
    private float closedConstraint = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!proximity)
        {
            state -= openSpeed * Time.deltaTime;
            state = Mathf.Clamp(state, closedConstraint, openConstraint);

            door.transform.position = new Vector3(door.transform.position.x, state, door.transform.position.z);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && locked)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                for(int i = 0; i < console.Count; i++)
                {
                    if(console[i].GetComponent<DoorConsoleScript>().accepted == false)
                    {
                        Debug.Log("Denied");
                    }

                    else
                    {
                        Debug.Log("Accepted");
                        locked = false;
                    }
                }
            }           
        }

        if (other.gameObject.tag == "Player" && !locked)
        {
            proximity = true;

            state += openSpeed * Time.deltaTime;
            state = Mathf.Clamp(state, closedConstraint, openConstraint);

            door.transform.position = new Vector3(door.transform.position.x, state, door.transform.position.z);           
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !locked)
        {
            proximity = false;
        }
    }
}
