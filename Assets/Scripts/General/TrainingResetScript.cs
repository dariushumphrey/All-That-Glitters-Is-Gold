using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingResetScript : MonoBehaviour
{
    public Transform resetPoint;
    public PlayerStatusScript player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStatusScript>();

        player.transform.position = resetPoint.transform.position;
        player.transform.rotation = resetPoint.transform.rotation;
        //Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isDead == true)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                Spawn();             
            }
        }
    }

    void Spawn()
    {
        player.transform.position = resetPoint.transform.position;
        player.transform.rotation = resetPoint.transform.rotation;
        player.GetComponent<Rigidbody>().freezeRotation = true;

        player.move.enabled = true;
        player.cam.enabled = true;
        player.inv.enabled = true;

        player.playerHealth = player.playerHealthMax;
        player.isDead = false;

        if (player.inv.inventory.Count > 0)
        {
            player.inv.GetComponentInChildren<FirearmScript>().enabled = true;
        }
    }
}
