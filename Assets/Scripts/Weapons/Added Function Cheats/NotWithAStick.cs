using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotWithAStick : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc; //Text UI that records Cheat activity
    private float efIncrease = 20f; //% of max Range use to increase Effective Range
    private float efReset; //Holds starting Effective Range
    private float aaReset; //Holds starting Aim Assist
    private float benefitTimer = 20f; //Timer duration for effect
    private float benefitTimerReset; //Holds starting timer duration
    private bool maxed; //Affirms a maximum value has been reached if true
    internal bool killConfirmed = false; //Affirms an achieved kill if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        proc.GetComponent<Text>().text = " ";

        //Non-exotic Rarity 5 Weapons assign timer and Aim Assist reset values
        if(firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            benefitTimerReset = benefitTimer;
            aaReset = firearm.aimAssistStrength;
            maxed = false;
        }

        efReset = firearm.effectiveRange;
        efIncrease /= 100;
        efIncrease *= firearm.range;

    }

    // Update is called once per frame
    void Update()
    {   
        //Confirmed kills increase Effective Range
        //Non-exotic Rarity 5 Weapons increase Aim Assist when Effective Range & total Range match
        if(killConfirmed && firearm.enabled == true)
        {
            firearm.effectiveRange += efIncrease;
            proc.GetComponent<Text>().text = "Not with a Stick";

            if (firearm.effectiveRange >= firearm.range)
            {
                firearm.effectiveRange = firearm.range;
                if (firearm.weaponRarity == 5 && !firearm.isExotic)
                {
                    maxed = true;
                    firearm.aimAssistStrength = 0.5f;
                }
            }

            killConfirmed = false;
        }

        if (firearm.isReloading == true && firearm.enabled == true)
        {
            if (firearm.weaponRarity != 5 || firearm.isExotic)
            {
                firearm.effectiveRange = efReset;
                proc.GetComponent<Text>().text = "";
            }
        }

        //Benefits remain active until timer expires
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
                proc.GetComponent<Text>().text = "";
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
