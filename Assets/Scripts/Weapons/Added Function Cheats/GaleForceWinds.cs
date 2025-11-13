using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaleForceWinds : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerMoveScript move;
    internal GameObject proc; //Text UI that records Cheat activity
    internal GameObject applicator;

    private int chargeAccelerant = 2; //Multipler that increases charge through moving
    private int sprintAccelerant = 5; //Multipler that increases charge through Sprinting
    internal int chargeCount = 0; //Number of current uses
    internal float chargePercentage = 0f; //% of current Charge
    internal bool done = false; //Permits one operation if true
    internal bool toggle = false; //Enables/Disables effect if true/false

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        move = FindObjectOfType<PlayerMoveScript>();

        proc.GetComponent<Text>().text = " ";
        applicator = Resources.Load<GameObject>("Game Items/GaleForceWindsApply");

        //Non-exotic Rarity 5 Weapons double the charge multiplier from Sprinting
        if(firearm.weaponRarity == 5)
        {
            sprintAccelerant *= 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Gale Force Winds
        //___.text = Sprinting or moving generates a charge. The next charged shot casts severe winds that applies Health and Slowed debuffs to in-range Enemies.

        //Moving or Sprinting generates a charge, granting a use when reaching 100%
        if(move.horizInput != 0 || move.vertInput != 0)
        {
            if(move.sprinting)
            {
                chargePercentage += Time.deltaTime * (chargeAccelerant * sprintAccelerant);
            }

            else
            {
                chargePercentage += Time.deltaTime * chargeAccelerant;
            }

            if (chargePercentage >= 100f)
            {
                chargePercentage = 100f;
                if(!done)
                {
                    chargeCount++;
                    done = true;
                }
            }
        }

        if(chargePercentage > 0f && chargePercentage < 100f)
        {
            proc.GetComponent<Text>().text = "Strength: " + chargePercentage.ToString("F0") + "%";
        }
        
        else if(chargePercentage >= 100f)
        {
            if(toggle)
            {
                proc.GetComponent<Text>().text = "Deployment Ready";

            }

            else
            {
                proc.GetComponent<Text>().text = "Gale Force Winds " + chargePercentage.ToString("F0") + "%";
            }

        }

        else
        {
            proc.GetComponent<Text>().text = "";
        }

        //Toggles Cheat effects upon input at full charge
        if (Input.GetKeyDown(KeyCode.E) && chargePercentage >= 100f)
        {
            if (!toggle)
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
