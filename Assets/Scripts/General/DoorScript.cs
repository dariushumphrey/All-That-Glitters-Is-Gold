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
    public bool vertical = false;
    public bool horizontal = false;
    public bool diagonal = false;

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

        if(other.gameObject.tag == "Enemy")
        {
            if(!other.gameObject.GetComponent<Rigidbody>())
            {
                other.gameObject.AddComponent<Rigidbody>();
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                other.gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
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
        if (other.gameObject.tag == "Player" && elevator)
        {
            other.gameObject.transform.parent = null;
        }

        if (other.gameObject.tag == "Player" && !locked || other.gameObject.tag == "Enemy" && !locked)
        {
            proximity = false;
        }

        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<Rigidbody>())
            {
                Destroy(other.gameObject.GetComponent<Rigidbody>());
            }

        }
    }
}
