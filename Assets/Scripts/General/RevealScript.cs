using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealScript : MonoBehaviour
{
    public List<GameObject> unhide = new List<GameObject>(); //List to hold items meant to be unhidden
    public List<GameObject> hide = new List<GameObject>(); //List to hold items meant to be hidden

    public int startConcealOn = 2; //Hides items starting from provided number (corresponds with Difficulty number)
    public bool hideOnTrigger; //Hides items on a trigger if true
    private LevelManagerScript level;

    private void Awake()
    {
        level = FindObjectOfType<LevelManagerScript>();

        if(hideOnTrigger)
        {
            return;
        }

        //Conceals objects beginning from specified Difficulty
        else
        {
            if (hide.Count > 0)
            {
                if (level.gameSettingState >= startConcealOn)
                {
                    for (int v = 0; v < hide.Count; v++)
                    {
                        hide[v].gameObject.SetActive(false);
                    }
                }
            }

            if (hide.Count > 0)
            {
                if (level.gameSettingState >= startConcealOn)
                {
                    for (int v = 0; v < hide.Count; v++)
                    {
                        hide[v].gameObject.SetActive(false);
                    }
                }
            }
        }      
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //Hides all items when Player enters trigger
            if(hideOnTrigger)
            {
                if (hide.Count > 0)
                {
                    for (int v = 0; v < hide.Count; v++)
                    {
                        hide[v].gameObject.SetActive(false);
                    }
                }
            }
            
            //Unhides all items when Player enters trigger, if any items are meant to be unhidden
            if(unhide.Count > 0)
            {
                for (int u = 0; u < unhide.Count; u++)
                {
                    unhide[u].gameObject.SetActive(true);
                    unhide[u].transform.SetParent(null);
                }
            }

            gameObject.SetActive(false);
        }
    }
}
