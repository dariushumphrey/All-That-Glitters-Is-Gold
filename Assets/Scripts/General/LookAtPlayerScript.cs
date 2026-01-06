using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerScript : MonoBehaviour
{
    private GameObject player;
    private GameObject cam;
    internal Vector3 camPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.LookAt(player.transform);
    }
}
