using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceScript : MonoBehaviour
{
    public int levelIndex = 0;
    public bool incomingMenu;
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
            if(incomingMenu)
            {
                level.ReturnToMainMenu();
            }

            else
            {
                level.level = levelIndex;
                level.SaveInventory();
                level.LoadScene();
            }           
        }
    }
}
