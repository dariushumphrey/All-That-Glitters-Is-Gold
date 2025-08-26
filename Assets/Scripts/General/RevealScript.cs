using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealScript : MonoBehaviour
{
    public List<GameObject> unhideWorld = new List<GameObject>(); //For level objects -- boxes, walls, floors, etc.
    public List<GameObject> unhideGame = new List<GameObject>(); //For gameplay items -- chests, keys, weapons, etc.

    public List<GameObject> hideWorld = new List<GameObject>();
    public List<GameObject> hideGame = new List<GameObject>(); 

    public int startConcealOn = 2;
    public bool hideOnTrigger;
    private LevelManagerScript level;

    private void Awake()
    {
        level = FindObjectOfType<LevelManagerScript>();

        //if(unhideWorld.Count > 0)
        //{
        //    for (int v = 0; v < unhideWorld.Count; v++)
        //    {
        //        unhideWorld[v].gameObject.SetActive(false);
        //    }
        //}
       
        //if (unhideGame.Count > 0)
        //{
        //    for (int v = 0; v < unhideGame.Count; v++)
        //    {
        //        unhideGame[v].gameObject.SetActive(false);
        //    }
        //}

        //For concealing objects with no intentions of revealing them at a certain point in Difficulty
        if(hideOnTrigger)
        {
            return;
        }

        else
        {
            if (hideWorld.Count > 0)
            {
                if (level.gameSettingState >= startConcealOn)
                {
                    for (int v = 0; v < hideWorld.Count; v++)
                    {
                        hideWorld[v].gameObject.SetActive(false);
                    }
                }
            }

            if (hideGame.Count > 0)
            {
                if (level.gameSettingState >= startConcealOn)
                {
                    for (int v = 0; v < hideGame.Count; v++)
                    {
                        hideGame[v].gameObject.SetActive(false);
                    }
                }
            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //level = FindObjectOfType<LevelManagerScript>();

        //for (int v = 0; v < unhide.Count; v++)
        //{
        //    unhide[v].gameObject.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(hideOnTrigger)
            {
                if (hideWorld.Count > 0)
                {
                    for (int v = 0; v < hideWorld.Count; v++)
                    {
                        hideWorld[v].gameObject.SetActive(false);
                    }
                }

                if (hideGame.Count > 0)
                {
                    for (int v = 0; v < hideGame.Count; v++)
                    {
                        hideGame[v].gameObject.SetActive(false);
                    }
                }
            }

            if(unhideWorld.Count > 0)
            {
                for (int u = 0; u < unhideWorld.Count; u++)
                {
                    unhideWorld[u].gameObject.SetActive(true);
                    unhideWorld[u].transform.SetParent(null);
                }
            }

            if(unhideGame.Count > 0)
            {
                for (int u = 0; u < unhideGame.Count; u++)
                {
                    if (unhideGame[u].GetComponent<LootScript>() != null)
                    {
                        if (unhideGame[u].GetComponent<LootScript>().raritySpawn >= 5)
                        {
                            Debug.Log("Left Fated/Exotic chest alone");
                            if (level.gameSettingState != 5)
                            {
                                Debug.Log("Did not spawn Exotic chest; game not set to highest difficulty");
                            }

                            else
                            {
                                unhideGame[u].gameObject.SetActive(true);
                                unhideGame[u].transform.SetParent(null);
                                unhideGame[u].GetComponent<LootScript>().SpawnDrop();
                            }
                        }

                        else
                        {
                            unhideGame[u].GetComponent<LootScript>().raritySpawn = level.gameSettingState;
                            unhideGame[u].gameObject.SetActive(true);
                            unhideGame[u].transform.SetParent(null);
                            unhideGame[u].GetComponent<LootScript>().SpawnDrop();
                        }
                    }

                    else
                    {
                        unhideGame[u].gameObject.SetActive(true);
                        unhideGame[u].transform.SetParent(null);
                    }                   
                }
            }

            gameObject.SetActive(false);
        }
    }
}
