using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheMostResplendent : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc; //Text UI that records Cheat activity
    private PlayerStatusScript player;
    private GameObject activation; //VFX used to convey activity
    internal GameObject hardLucent; //Lucent game object 

    private int shotMaximum = 10; //Goal number of confirmed hits
    private int shots = 0; //Total number of confirmed hits
    internal int stackCount = 0; //Total number of owned stacks
    internal int stackMax = 1; //Maximum number of stacks allowed
    private bool done = false; //Allows one operation if true;
    internal bool toggle = false; //Enables/Disables effect if true/false
    internal bool hitConfirmed = false; //Affirms achieved hit if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        hardLucent = Resources.Load<GameObject>("Game Items/testHardLucent");
        proc.GetComponent<Text>().text = " ";

        //Non-exotic Rarity 5 Weapons increase maximum stacks to 2
        if(firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            stackMax = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //The Most Resplendent
        //___.text = [E] - Activating on a full stack from Enemy hits creates a Hard Lucent crystal at a Weapon’s hit point, producing Lucent clusters for 20 seconds or when shot.

        if(stackCount <= 0)
        {
            proc.GetComponent<Text>().text = "";
        }

        if(stackCount >= 1)
        {
            if(toggle)
            {
                proc.GetComponent<Text>().text = "Deployment Ready";

            }

            else
            {
                proc.GetComponent<Text>().text = "Resplendent x" + stackCount;
            }
            
        }

        //Increments confirmed hits up to goal number
        //Reaching goal number grants one stack
        if (hitConfirmed == true && stackCount != stackMax)
        {
            shots++;
            if(shots >= shotMaximum)
            {
                if(!done)
                {
                    stackCount++;
                    done = true;
                }

                shots = 0;
            }

            hitConfirmed = false;
            done = false;
        }

        //Enables effect upon input with at least one stacks
        if(Input.GetKeyDown(KeyCode.E) && stackCount >= 1)
        {
            if(!toggle)
            {
                toggle = true;
            }

            else
            {
                toggle = false;
            }
        }
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }
    }
}
