using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : EnemyBehaviorScript
{
    private GameObject[] waypoint;
    int waypointNext;

    private void Awake()
    {
        waypoint = GameObject.FindGameObjectsWithTag("Waypoint");

        //if(cluster.Count > 0)
        //{
        //    for(int c = 0; c < cluster.Count; c++)
        //    {
        //        cluster[c].leader = gameSelf;
        //    }
        //}
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        waypointNext = Random.Range(0, waypoint.Length);

        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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
}
