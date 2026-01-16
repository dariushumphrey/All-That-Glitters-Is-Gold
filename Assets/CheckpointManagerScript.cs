using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointManagerScript : MonoBehaviour
{
    public bool checkpointReached;

    public Transform checkpointStartPosition;
    public GameObject checkpointTrigger;
    public GameObject player;
    public GameObject weaponManager;
    public GameObject redKey, blueKey;
    public List<GameObject> spawnerGroups = new List<GameObject>();
    public List<GameObject> doorConsoles = new List<GameObject>();
    public List<GameObject> doorTriggers = new List<GameObject>();
    public List<GameObject> events = new List<GameObject>();

    public GameObject checkpointNotice;

    // Start is called before the first frame update
    void Start()
    {
        checkpointNotice = GameObject.Find("checkpointNotice");
        checkpointNotice.GetComponent<Text>().text = "";

        if(checkpointReached)
        {
            player.transform.position = checkpointStartPosition.position;
            player.transform.rotation = checkpointStartPosition.rotation;

            weaponManager.transform.position = checkpointStartPosition.position;

            if(redKey != null)
            {
                redKey.transform.position = player.transform.position;
            }

            if(blueKey != null)
            {
                blueKey.transform.position = player.transform.position;
            }

            if (spawnerGroups.Count >= 1)
            {
                for (int s = 0; s < spawnerGroups.Count; s++)
                {
                    spawnerGroups[s].SetActive(false);
                }
            }

            if (doorConsoles.Count >= 1)
            {
                for (int d = 0; d < doorConsoles.Count; d++)
                {
                    doorConsoles[d].GetComponent<DoorConsoleScript>().accepted = true;
                }
            }

            if (doorTriggers.Count >= 1)
            {
                for (int d = 0; d < doorTriggers.Count; d++)
                {
                    doorTriggers[d].SetActive(false);
                }
            }

            if(events.Count >= 1)
            {
                for (int e = 0; e < events.Count; e++)
                {
                    events[e].transform.position = player.transform.position;
                }
            }

            checkpointTrigger.gameObject.SetActive(false);
        }     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ClearNotice()
    {
        yield return new WaitForSeconds(4f);
        checkpointNotice.GetComponent<Text>().text = "";

    }
}
