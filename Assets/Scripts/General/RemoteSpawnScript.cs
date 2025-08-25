using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteSpawnScript : MonoBehaviour
{
    public List<SpawnerScript> spawners = new List<SpawnerScript>();
    public float spawnDelay;
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
            StartCoroutine(DelayedSpawn());
        }
    }

    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        for(int s = 0; s < spawners.Count; s++)
        {
            spawners[s].RemoteSpawn();
        }
        gameObject.SetActive(false);
    }
}
