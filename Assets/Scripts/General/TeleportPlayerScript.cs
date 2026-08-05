using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerScript : MonoBehaviour
{
    public Transform teleportPosition;

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
            other.gameObject.transform.position = teleportPosition.position;
            other.gameObject.transform.rotation = teleportPosition.rotation;
        }
    }
}
