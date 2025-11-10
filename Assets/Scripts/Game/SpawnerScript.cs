using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public int difficultySpawn; //Enemy toughness specified by difficulty number
    public int spawnRepeat = 1; //number of times to repeat spawns
    public int berthChance = 80; //Number to be above in order to apply Berth condition
    public List<GameObject> spawned = new List<GameObject>(); //List of Enemies to spawn
    private EnemyManagerScript enemy;

    public Collider field; //Trigger-marked Collider used to spawn Enemies within
    private int berthCondition; //Number used to randomly apply Berth condition
    private Bounds spawnField;

    // Start is called before the first frame update
    void Start()
    {
        DifficultyCorrection();
        enemy = FindObjectOfType<EnemyManagerScript>();
    }

    /// <summary>
    /// Fixes incorrect difficulty assignment
    /// </summary>
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

    }

    /// <summary>
    /// Spawns a random Enemy type, randomly applies Berth condition
    /// </summary>
    public void SpawnObject()
    {
        int picker = Random.Range(0, spawned.Count);
        spawnField = field.bounds;
        Vector3 spawnSite = spawnField.center + new Vector3(Random.Range(-spawnField.extents.x, spawnField.extents.x), 
                                                            Random.Range(-spawnField.extents.y, spawnField.extents.y), 
                                                            Random.Range(-spawnField.extents.z, spawnField.extents.z));

        if(spawned[picker].GetComponent<EnemyHealthScript>() != null)
        {
            spawned[picker].GetComponent<EnemyHealthScript>().difficultyValue = difficultySpawn;           
        }

        GameObject fresh = Instantiate(spawned[picker], spawnSite, transform.rotation);
        berthCondition = Random.Range(0, 101);
        if (berthCondition > berthChance && !spawned[picker].GetComponent<ReplevinScript>().amBoss)
        {
            fresh.AddComponent<BerthScript>();
            fresh.AddComponent<ColorLerpScript>();

            fresh.GetComponent<ColorLerpScript>().enemyUse = true;
            fresh.GetComponent<ColorLerpScript>().materialIndex = 1;
            fresh.GetComponent<ColorLerpScript>().colorOne = Color.red;
            fresh.GetComponent<ColorLerpScript>().colorTwo = Color.yellow;
        }

        fresh.name = spawned[picker].name;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            for (int s = 0; s < spawnRepeat; s++)
            {
                SpawnObject();
            }

            enemy.CatalogEnemies();
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Allows other sources to invoke SpawnObject() method
    /// </summary>
    public void RemoteSpawn()
    {
        for (int s = 0; s < spawnRepeat; s++)
        {
            SpawnObject();
        }

        enemy.CatalogEnemies();
        gameObject.SetActive(false);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, field.bounds.size);
    //}
}
