using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateZeroGravityScript : MonoBehaviour
{
    //activate - turns on Player's Zero Gravity behavior if true
    //deactivate - turns off Player's Zero Gravity behavior if true
    public bool activate, deactivate;
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
        if(other.gameObject.CompareTag("Player") && activate)
        {
            other.gameObject.GetComponent<PlayerMoveScript>().zeroGravity = true;
        }

        if (other.gameObject.CompareTag("Player") && deactivate)
        {
            other.gameObject.GetComponent<PlayerMoveScript>().zeroGravity = false;
            other.gameObject.GetComponent<PlayerMoveScript>().airbornePull = other.gameObject.GetComponent<PlayerMoveScript>().airbornePullReset;
        }
    }
}
