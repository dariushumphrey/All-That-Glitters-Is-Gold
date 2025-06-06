using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : EnemyBehaviorScript
{
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        self.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        self.SetDestination(player.transform.position);

    }
}
