using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject door;
    public List<GameObject> console = new List<GameObject>();
    public float openConstraint = 4f; //Value to stop door when opening
    public float closedConstraint = 0f; //Value to stop door when closing
    public int openSpeed = 5;
    public bool proximity = false; //Opens door if true/closes door if false
    public bool locked = false; //Prevents door from opening if true
    public bool elevator = false; //Makes Player a child of Elevator when moving if true
    public bool invert = false; //Reverses effects of constraints if true
    public bool vertical = false; //Opens door on Y-axix if true
    public bool horizontal = false; //Opens door on X-axis if true
    public bool diagonal = false; //Opens door on Z-axis if true
    public bool overrideOpen = false; //Forces Door to open if true

    private float state = 0f; //Float to manipulate X,Y, or Z position of door

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Closes door when Player/Enemy is not nearby
        if (!proximity)
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

            if(horizontal)
            {
                door.transform.position = new Vector3(state, door.transform.position.y, door.transform.position.z);
            }

            if(vertical)
            {
                door.transform.position = new Vector3(door.transform.position.x, state, door.transform.position.z);
            }

            if(diagonal)
            {
                door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y, state);
            }           
        }

        if(overrideOpen)
        {
            proximity = true;
            ForceOpen();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Makes gameObject Parent of Player if set as Elevator
        if (other.gameObject.tag == "Player" && elevator)
        {
            other.gameObject.transform.parent = door.gameObject.transform;
        }

        //Checks for consoles with accepted keys and unlocks doors when Player enters trigger
        if (other.gameObject.tag == "Player" && locked)
        {
            for (int i = 0; i < console.Count; i++)
            {
                if (console[i].GetComponent<DoorConsoleScript>().accepted == true)
                {
                    console.Remove(console[i]);
                    if (console.Count <= 0)
                    {
                        locked = false;
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Checks for consoles with accepted keys and unlocks doors when Player remains in trigger
        if (other.gameObject.tag == "Player" && locked && console.Count >= 1)
        {
            for (int i = 0; i < console.Count; i++)
            {
                if (console[i].GetComponent<DoorConsoleScript>().accepted == true)
                {
                    console.Remove(console[i]);                   
                }

                if (console.Count <= 0)
                {
                    locked = false;
                }
            }
        }

        //Opens door for Players/Enemies when present within trigger
        if (other.gameObject.tag == "Player" && !locked && !overrideOpen || other.gameObject.tag == "Enemy" && !locked && !overrideOpen)
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

            if(horizontal)
            {
                door.transform.position = new Vector3(state, door.transform.position.y, door.transform.position.z);

            }

            if(vertical)
            {
                door.transform.position = new Vector3(door.transform.position.x, state, door.transform.position.z);
            }

            if (diagonal)
            {
                door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y, state);
            }
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        //Removes gameObject as Parent of Player if set as Elevator
        if (other.gameObject.tag == "Player" && elevator)
        {
            other.gameObject.transform.parent = null;
        }

        //Turns proximity flag to false when Player/Enemy leaves trigger
        if (other.gameObject.tag == "Player" && !locked || other.gameObject.tag == "Enemy" && !locked)
        {
            proximity = false;
        }
    }

    /// <summary>
    /// Forces Door to permanently open
    /// </summary>
    private void ForceOpen()
    {
        if (invert)
        {
            state -= openSpeed * Time.deltaTime;
        }

        else
        {
            state += openSpeed * Time.deltaTime;
        }

        state = Mathf.Clamp(state, closedConstraint, openConstraint);

        if (horizontal)
        {
            door.transform.position = new Vector3(state, door.transform.position.y, door.transform.position.z);

        }

        if (vertical)
        {
            door.transform.position = new Vector3(door.transform.position.x, state, door.transform.position.z);
        }

        if (diagonal)
        {
            door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y, state);
        }
    }
}
