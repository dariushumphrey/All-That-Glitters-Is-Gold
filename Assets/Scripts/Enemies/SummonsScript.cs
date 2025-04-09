using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SummonsScript : MonoBehaviour
{
    public float spawnTimer = 4.0f;
    public int spawnEnemyMax = 4;
    public GameObject birth;
    public Transform birthPoint;

    private List<GameObject> fam = new List<GameObject>();
    private EnemyHealthScript health;
    private float spawnTimerReset;
    private NavMeshAgent enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        spawnTimerReset = spawnTimer;
        health = GetComponent<EnemyHealthScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fam.Count >= spawnEnemyMax)
        {
            Debug.Log("Summons can no longer birth children; Summons at max");
            spawnTimer = spawnTimerReset;
        }

        else
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0.0f)
            {
                BirthEnemy();
                spawnTimer = spawnTimerReset;
            }
        }
    }

    void BirthEnemy()
    {        
        StartCoroutine(Deliver());
    }

    IEnumerator Deliver()
    {
        float moveSpeed = enemy.speed;
        enemy.speed = 0f;

        yield return new WaitForSeconds(2f);
        if(birth.GetComponent<EnemyHealthScript>() != null)
        {
            birth.GetComponent<EnemyHealthScript>().difficultyValue = health.difficultyValue;
        }
        GameObject child = Instantiate(birth, birthPoint.transform.position, birthPoint.transform.rotation);
        fam.Add(child);
        child.name = birth.name;
        enemy.speed = moveSpeed;

        //For a good laugh -- spawns continuous enemies until time resets. 
        //spawnTimer = spawnTimerReset;

    }
}
