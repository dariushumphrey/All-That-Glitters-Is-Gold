using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceScript : MonoBehaviour
{
    private LevelManagerScript level;
    // Start is called before the first frame update
    void Start()
    {
        level = FindObjectOfType<LevelManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            level.ReturnToMainMenu();
        }
    }
}
