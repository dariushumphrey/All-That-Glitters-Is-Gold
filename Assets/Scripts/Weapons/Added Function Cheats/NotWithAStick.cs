using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotWithAStick : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc;
    private float efIncrease = 20f;
    private float efReset;
    private float aaReset;
    private float benefitTimer = 20f;
    private float benefitTimerReset;
    private bool maxed;
    internal bool killConfirmed = false;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        proc.GetComponent<Text>().text = " ";

        benefitTimerReset = benefitTimer;
        efReset = firearm.effectiveRange;
        aaReset = firearm.aimAssistStrength;
        maxed = false;

        efIncrease /= 100;
        efIncrease *= firearm.range;

    }

    // Update is called once per frame
    void Update()
    {
     
        if (killConfirmed && firearm.enabled == true)
        {
            firearm.effectiveRange += efIncrease;
            proc.GetComponent<Text>().text = "Not with a Stick";
            killConfirmed = false;

            if (firearm.effectiveRange >= firearm.range)
            {
                firearm.effectiveRange = firearm.range;
                if (firearm.weaponRarity == 5 && !firearm.isExotic)
                {
                    maxed = true;
                    firearm.aimAssistStrength = 0.5f;
                }
            }
        }

        if (firearm.isReloading == true && firearm.enabled == true)
        {
            if (firearm.weaponRarity == 5 && !firearm.isExotic)
            {
                //Do nothing
            }

            else
            {
                firearm.effectiveRange = efReset;
                proc.GetComponent<Text>().text = " ";
            }
        }

        if (maxed)
        {
            benefitTimer -= Time.deltaTime;
            proc.GetComponent<Text>().text = "Longest Stick " + benefitTimer.ToString("F0") + "s";
            if (benefitTimer <= 0f)
            {
                maxed = false;
                firearm.effectiveRange = efReset;
                firearm.aimAssistStrength = aaReset;
                benefitTimer = benefitTimerReset;
                proc.GetComponent<Text>().text = " ";
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
