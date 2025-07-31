using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyScript : MonoBehaviour
{
    public bool isBlueKey, isRedKey = false;
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
        if(other.gameObject.tag == "Player")
        {
            if(other.gameObject.GetComponent<PlayerInventoryScript>())
            {
                if(isBlueKey)
                {
                    other.gameObject.GetComponent<PlayerInventoryScript>().blueKey = true;
                    other.gameObject.GetComponent<PlayerInventoryScript>().bKey.gameObject.SetActive(true);
                }

                if (isRedKey)
                {
                    other.gameObject.GetComponent<PlayerInventoryScript>().redKey = true;
                    other.gameObject.GetComponent<PlayerInventoryScript>().rKey.gameObject.SetActive(true);

                }
            }

            Destroy(gameObject);
        }
    }
}
