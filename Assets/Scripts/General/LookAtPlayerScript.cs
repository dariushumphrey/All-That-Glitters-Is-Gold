using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerScript : MonoBehaviour
{
    private Transform you;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        you = gameObject.transform;
        player = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        you.LookAt(player.transform.position);
    }
}
