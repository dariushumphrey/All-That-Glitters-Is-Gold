using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerScript : MonoBehaviour
{
    private Transform enemy;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.transform;
        player = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        enemy.LookAt(player.transform.position);
    }
}
