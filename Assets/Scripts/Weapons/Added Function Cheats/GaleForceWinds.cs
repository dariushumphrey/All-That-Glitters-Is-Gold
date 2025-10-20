using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaleForceWinds : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerMoveScript move;
    internal GameObject proc;
    internal GameObject applicator;

    private float sprintConfirm = 5f;
    private float sprintReset;
    private int chargeAccelerant = 2;
    private int sprintAccelerant = 5;
    internal int chargeCount = 0;
    internal float chargePercentage = 0f;
    internal bool done = false;
    internal bool toggle = false;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        move = FindObjectOfType<PlayerMoveScript>();

        sprintReset = sprintConfirm;
        proc.GetComponent<Text>().text = " ";
        applicator = Resources.Load<GameObject>("Game Items/GaleForceWindsApply");

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
