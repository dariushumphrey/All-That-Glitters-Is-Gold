using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLeaderScript : MonoBehaviour
{
    public float accuracy = 1.0f;
    public int alertDist = 10;
    public Vector3 perimeter;
    public float scoutTimer;
    public float scoutTimerReset = 5.0f;
    public List<GameObject> cluster = new List<GameObject>();

    private GameObject player;
    private NavMeshAgent self;
    private GameObject myself;
    private float distance = 0.0f;
    private int scoutPick;
    internal bool playerFound = false;
    private GameObject[] waypoint;
    private int waypointNext;
    private EnemyHealthScript health;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<NavMeshAgent>();
        myself = gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        scoutTimer = scoutTimerReset;
        playerFound = false;
        health = GetComponent<EnemyHealthScript>();

        waypoint = GameObject.FindGameObjectsWithTag("Waypoint");
        waypointNext = Random.Range(0, waypoint.Length);

        if (cluster.Count > 0)
        {
            for (int f = 0; f < cluster.Count; f++)
            {
                cluster[f].GetComponent<EnemyHealthScript>().difficultyValue = health.difficultyValue;
                cluster[f].GetComponent<EnemyFollowerScript>().target = myself;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        //perimeter = new Vector3(transform.position.x - Random.Range(-perimeter.x, perimeter.x), 0, transform.position.z - Random.Range(-perimeter.z, perimeter.z));
        //perimeter = new Vector3(Random.Range(-transform.position.x, transform.position.x), 0,  Random.Range(-transform.position.z, transform.position.z));
        //perimeter = new Vector3(perimeter.x, 0, perimeter.z);
        //perimeter = new Vector3(perimeter.x - Random.Range(-transform.position.x, transform.position.x), 0, perimeter.z - Random.Range(-transform.position.z, transform.position.z));
        perimeter = transform.position;

        for(int z = 0; z < cluster.Count; z++)
        {
            if(cluster[z].GetComponent<EnemyHealthScript>().healthCurrent <= 0)
            {
                cluster.RemoveAt(z);
            }
        }

        if(cluster.Count > 0)
        {
            scoutTimer -= Time.deltaTime;
            if (scoutTimer <= 0f && playerFound == false)
            {
                Scout();
                scoutTimer = scoutTimerReset;
            }
        }        

        if (distance < alertDist || playerFound == true)
        {
            Pursuit();
        }

        else
        {
            Patrol();
        }

    }

    void Patrol()
    {
        if (waypoint.Length == 0)
        {
            return;
        }

        if (Vector3.Distance(waypoint[waypointNext].transform.position, self.transform.position) < accuracy)
        {
            waypointNext = Random.Range(0, waypoint.Length);
        }

        self.SetDestination(waypoint[waypointNext].transform.position);
    }

    public void Pursuit()
    {
        playerFound = true;
        self.SetDestination(player.transform.position);

        if (cluster.Count > 0)
        {
            for (int f = 0; f < cluster.Count; f++)
            {
                if(cluster[f].GetComponent<EnemyFollowerScript>().GetComponent<EnemyHealthScript>().healthCurrent > 0)
                {
                    cluster[f].GetComponent<EnemyFollowerScript>().SetDest(player);
                }
            }
        }
    }

    void Scout()
    {
        scoutPick = Random.Range(0, cluster.Count);
        cluster[scoutPick].GetComponent<EnemyFollowerScript>().ScoutPerimeter(perimeter);
        StartCoroutine(Recall(cluster[scoutPick]));
    }

    IEnumerator Recall(GameObject g)
    {
        yield return new WaitForSeconds(scoutTimerReset);
        if(g.GetComponent<EnemyFollowerScript>().GetComponent<EnemyHealthScript>().healthCurrent <= 0)
        {
            yield break;
        }

        else
        {
            g.GetComponent<EnemyFollowerScript>().SetDest(myself);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, perimeter);
    }
}
