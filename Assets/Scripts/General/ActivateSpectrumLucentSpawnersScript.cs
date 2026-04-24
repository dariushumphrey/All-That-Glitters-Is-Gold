using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSpectrumLucentSpawnersScript : MonoBehaviour
{
    public List<GameObject> spawners = new List<GameObject>();
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
            for(int s = 0; s < spawners.Count; s++)
            {
                spawners[s].GetComponent<SpectrumLucentSpawnerScript>().active = true;
            }

            gameObject.SetActive(false);
        }
    }
}
