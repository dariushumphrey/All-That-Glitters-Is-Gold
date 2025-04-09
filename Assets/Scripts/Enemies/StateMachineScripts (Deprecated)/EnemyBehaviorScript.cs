using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class EnemyBehaviorScript : StateMachineBehaviour
{
    public float accuracy = 1.0f;
    public Animator animSelf;
    internal GameObject player;
    internal GameObject gameSelf;
    internal NavMeshAgent self;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gameSelf = animator.gameObject;
        self = gameSelf.GetComponent<NavMeshAgent>();
        animSelf = gameSelf.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

}
