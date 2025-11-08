using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowerScript : MonoBehaviour
{
    public float accuracy = 1.0f;
    public int alertDist = 10;
    public GameObject target;
    public GameObject player;
    public EnemyLeaderScript leader;

    private NavMeshAgent self;
    private float distance = 0.0f;
    private EnemyHealthScript enemy;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponent<EnemyHealthScript>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < alertDist || enemy.healthCurrent < enemy.healthMax)
        {
            if(leader != null && leader.GetComponent<EnemyHealthScript>().healthCurrent > 0)
            {
                leader.Pursuit();
            }

            ChasePlayer();
        }

        //self.SetDestination(target.transform.position);

    }

    public void ChasePlayer()
    {
        self.SetDestination(player.transform.position);

    }

    public void SetDest(GameObject chase)
    {
        self.SetDestination(chase.transform.position);
    }

    public void ScoutPerimeter(Vector3 spot)
    {
        self.SetDestination(spot);
    }
}
