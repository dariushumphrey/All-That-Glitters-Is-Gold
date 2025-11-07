using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteDoorScript : MonoBehaviour
{
    public List<DoorScript> doors = new List<DoorScript>(); //List of doors with intention to be opened
    public float openDelay = 0f;

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
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(OpenDoor());
        }     
    }

    /// <summary>
    /// Turns overrideOpen variable for doors to true after a delay.
    /// </summary>
    private IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(openDelay);
        for(int d = 0; d < doors.Count; d++)
        {
            doors[d].overrideOpen = true;
        }
        gameObject.SetActive(false);
    }
}
