using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public int difficultySpawn;
    public int spawnRepeat = 1;
    public bool spawnIndiv, spawnClust;
    public Vector3 spawnArena;
    public List<GameObject> spawned = new List<GameObject>();
    public List<GameObject> spawnedCluster = new List<GameObject>();
    private EnemyManagerScript enemy;

    public Collider field;
    private Bounds spawnField;
    // Start is called before the first frame update
    void Start()
    {
        DifficultyCorrection();
        enemy = FindObjectOfType<EnemyManagerScript>();
    }

    void DifficultyCorrection()
    {
        if(difficultySpawn <= 0)
        {
            difficultySpawn = 1;
        }

        if(difficultySpawn >= 6)
        {
            difficultySpawn = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            SpawnObject();
        }

        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            SpawnCluster();
        }
    }

    public void SpawnObject()
    {
        int picker = Random.Range(0, spawned.Count);
        spawnField = field.bounds;
        Vector3 spawnSite = spawnField.center + new Vector3(Random.Range(-spawnField.extents.x, spawnField.extents.x), 0, Random.Range(-spawnField.extents.z, spawnField.extents.z));

        //Vector3 spawnSite = new Vector3(transform.position.x + Random.Range(-spawnArena.x, spawnArena.x), 0, transform.position.z - Random.Range(-spawnArena.z, spawnArena.z));
        if(spawned[picker].GetComponent<EnemyHealthScript>() != null)
        {
            spawned[picker].GetComponent<EnemyHealthScript>().difficultyValue = difficultySpawn;           
        }

        GameObject fresh = Instantiate(spawned[picker], spawnSite, transform.rotation);
        fresh.name = spawned[picker].name;
    }

    void SpawnCluster()
    {
        int picker2 = Random.Range(0, spawnedCluster.Count);
        Vector3 spawnSite = new Vector3(transform.position.x + Random.Range(-spawnArena.x, spawnArena.x), 0, transform.position.z - Random.Range(-spawnArena.z, spawnArena.z));
        if (spawnedCluster[picker2].GetComponentInChildren<EnemyHealthScript>() != null)
        {
            spawnedCluster[picker2].GetComponentInChildren<EnemyHealthScript>().difficultyValue = difficultySpawn;          
        }
        GameObject fresh = Instantiate(spawnedCluster[picker2], spawnSite, transform.rotation);
        fresh.name = spawnedCluster[picker2].name;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(spawnIndiv == true)
            {
                for (int s = 0; s < spawnRepeat; s++)
                {
                    SpawnObject();
                    //GetComponent<BoxCollider>().enabled = false;
                }

                enemy.CatalogEnemies();
                gameObject.SetActive(false);
            }
            
            if(spawnClust == true)
            {
                for (int s = 0; s < spawnRepeat; s++)
                {
                    SpawnCluster();
                    //GetComponent<BoxCollider>().enabled = false;
                }

                enemy.CatalogEnemies();
                gameObject.SetActive(false);

            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, field.bounds.size);
    //}
}
