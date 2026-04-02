using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtPlayerScript : MonoBehaviour
{
    private GameObject player;
    private GameObject element;
    internal Vector3 camPos;
    // Start is called before the first frame update
    void Start()
    {
        player = Camera.main.gameObject;
        element = transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        element.transform.LookAt(player.transform);
    }
}
