using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHunterScript : MonoBehaviour
{
    public float accuracy = 1.0f;
    public GameObject prey;

    private NavMeshAgent self;
    private float distance = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<NavMeshAgent>();
        prey = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        self.SetDestination(prey.transform.position);
    }
}
