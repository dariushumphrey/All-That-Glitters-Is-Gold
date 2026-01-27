using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volant : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc; //Text UI that records Cheat activity
    private PlayerMoveScript move;
    private PlayerStatusScript status;
    private EnemyManagerScript enemy;

    internal bool toggle = false; //Enables/Disables effect if true/false
    internal bool voluntary = false; //Affirms manual Cheat activation if true
    internal bool cheatOverride = false; //Prevents Cheat deactivation if true -- Zero Gravity sections only

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        proc.GetComponent<Text>().text = "";
        move = FindObjectOfType<PlayerMoveScript>();
        status = FindObjectOfType<PlayerStatusScript>();
        enemy = FindObjectOfType<EnemyManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //Volant
        //___.text = "[E] - Enables character flight until Shield is broken or disenaged."

        if(move.zgOverride)
        {
            move.volant = false;
            proc.GetComponent<Text>().text = "";
        }

        if (status.playerShield > 0 && !move.zgOverride)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!toggle)
                {
                    voluntary = true;
                    enemy.volant = true;
                    move.volant = true;

                    proc.GetComponent<Text>().text = "Volant";
                    move.zeroGravity = true;
                    toggle = true;
                }

                else
                {
                    voluntary = false;
                    enemy.volant = false;
                    move.volant = false;

                    proc.GetComponent<Text>().text = "";
                    move.zeroGravity = false;
                    toggle = false;
                }
            }
        }

        if(status.playerShield <= 0 && !move.zgOverride)
        {
            if(toggle)
            {
                voluntary = false;
                enemy.volant = false;
                move.volant = false;

                proc.GetComponent<Text>().text = "";
                move.zeroGravity = false;
                toggle = false;
            }
        }
    }

    private void OnDisable()
    {
        if (toggle)
        {
            if(voluntary)
            {
                voluntary = false;
            }

            enemy.volant = false;
            move.volant = false;

            proc.GetComponent<Text>().text = "";
            move.zeroGravity = false;
            toggle = false;
        }

        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }
    }
}
