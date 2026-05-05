using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBoundaryScript : MonoBehaviour
{
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
            other.gameObject.GetComponent<PlayerStatusScript>().playerShield = 0;
            other.gameObject.GetComponent<PlayerStatusScript>().InflictDamage(9999);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyHealthScript>().healthCurrent != 0)
            {
                other.gameObject.GetComponent<EnemyHealthScript>().healthCurrent = 0;
            }           
        }

        if (other.gameObject.CompareTag("Lucent"))
        {
            other.gameObject.GetComponent<LucentScript>().shot = true;
        }
    }
}
