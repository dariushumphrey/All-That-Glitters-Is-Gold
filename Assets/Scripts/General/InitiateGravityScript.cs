using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateGravityScript : MonoBehaviour
{
    //activate - turns on Rigidbody gravity if true
    //deactivate - turns off Rigidbody gravity if true
    //affectsPlayer - interacts with Players if true
    //affectsEnemies - interacts with Enemies, Corpses if true
    public bool activate, deactivate, affectsPlayer, affectsEnemies;

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
        //turns on Rigidbody gravity for Players, Enemies, Corpses
        if (activate)
        {
            if(other.gameObject.CompareTag("Enemy") && affectsEnemies)
            {
                other.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }

            else if (other.gameObject.CompareTag("Corpse") && affectsEnemies)
            {
                other.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }

            if (other.gameObject.CompareTag("Player") && affectsPlayer)
            {
                other.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
        }

        //turns off Rigidbody gravity for Players, Enemies, Corpses
        if (deactivate)
        {
            if (other.gameObject.CompareTag("Enemy") && affectsEnemies)
            {
                other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            }

            else if (other.gameObject.CompareTag("Corpse") && affectsEnemies)
            {
                other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            }

            if (other.gameObject.CompareTag("Player") && affectsPlayer)
            {
                other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            }
        }     
    }
}
