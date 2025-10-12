using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheMostResplendent : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc;
    private PlayerStatusScript player;
    private GameObject activation, effect;
    internal GameObject hardLucent;

    private int stackMaximum = 10;
    private int shots = 0;
    internal int stackCount = 0;
    internal int stackMax = 1;
    private bool done = false;
    internal bool toggle = false;
    internal bool hitConfirmed = false;

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        hardLucent = Resources.Load<GameObject>("Game Items/testHardLucent");
        proc.GetComponent<Text>().text = " ";

        if(firearm.weaponRarity == 5)
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


        if (hitConfirmed == true && stackCount != stackMax)
        {
            shots++;
            if(shots >= stackMaximum)
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
            proc.GetComponent<Text>().text = " ";
        }
    }
}
