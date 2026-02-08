using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointTriggerScript : MonoBehaviour
{
    public bool requiresRed, requiresBlue, requiresBoth;
    private CheckpointManagerScript checkpoint;
    private LevelManagerScript level;

    // Start is called before the first frame update
    void Start()
    {
        checkpoint = FindObjectOfType<CheckpointManagerScript>();
        level = FindObjectOfType<LevelManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(requiresRed)
            {
                if(other.gameObject.GetComponent<PlayerInventoryScript>().redKey)
                {
                    level.lvlProgressSaved = true;

                    checkpoint.checkpointNotice.GetComponent<Text>().text = "Checkpoint Reached";
                    checkpoint.StartCoroutine(checkpoint.ClearNotice());
                    gameObject.SetActive(false);
                }
            }

            if (requiresBlue)
            {
                if (other.gameObject.GetComponent<PlayerInventoryScript>().blueKey)
                {
                    level.lvlProgressSaved = true;

                    checkpoint.checkpointNotice.GetComponent<Text>().text = "Checkpoint Reached";
                    checkpoint.StartCoroutine(checkpoint.ClearNotice());
                    gameObject.SetActive(false);
                }
            }

            if (requiresBoth)
            {
                if (other.gameObject.GetComponent<PlayerInventoryScript>().redKey && other.gameObject.GetComponent<PlayerInventoryScript>().blueKey)
                {
                    level.lvlProgressSaved = true;

                    checkpoint.checkpointNotice.GetComponent<Text>().text = "Checkpoint Reached";
                    checkpoint.StartCoroutine(checkpoint.ClearNotice());
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
