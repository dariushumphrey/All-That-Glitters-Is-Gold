using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteDoorScript : MonoBehaviour
{
    public List<DoorScript> doors = new List<DoorScript>();
    public float openDelay = 0f;
    private bool tripped = false;
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
            tripped = true;
            StartCoroutine(OpenDoor());
        }     
    }

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
