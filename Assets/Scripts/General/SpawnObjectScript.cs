using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectScript : MonoBehaviour
{
    public GameObject item;
    public Transform place;
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
            GameObject spawned = Instantiate(item, place.position, Quaternion.identity);
            spawned.name = item.name;
            spawned.AddComponent<DestroyScript>();
            spawned.GetComponent<DestroyScript>().destroyTimer = 10f;

            if (item.GetComponent<AmmoScript>())
            {
                item.GetComponent<AmmoScript>().ammoPercent = 100f;
            }
        }
    }
}
