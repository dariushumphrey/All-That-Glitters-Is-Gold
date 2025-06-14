using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestTriggerScript : MonoBehaviour
{
    public GameObject chest;
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
            chest.GetComponent<LootScript>().SpawnDrop();
            gameObject.SetActive(false);
        }       
    }
}
