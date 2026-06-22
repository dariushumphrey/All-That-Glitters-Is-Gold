using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bolster : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerStatusScript player;
    private GameObject activation; //VFX used to convey activity
    internal GameObject proc; //Text UI that records Cheat activity

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerStatusScript>();

        proc.GetComponent<Text>().text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = " ";
        }
    }
}
