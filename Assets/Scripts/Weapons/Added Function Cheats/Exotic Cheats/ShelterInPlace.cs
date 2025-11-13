using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShelterInPlace : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc; //Text UI that records Cheat activity
    private PlayerStatusScript player;
    private float absorbPercent = 80f; //% of damage absorbed
    private float absPctReset; //Holds starting damage % absorbed
    private float refrainTimer = 0.0f; //Timer used to grante effects
    private int dmgAbsorbAdd; //Number used to return Health/Shield
    private int damageNegate = 0; //Receives damage taken
    private int dmgIncrease; //Fixed Weapon damage number
    private int dmgReset; //Holds starting Weapon damage
   
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        proc.GetComponent<Text>().text = " ";

        absPctReset = absorbPercent;
        dmgIncrease = firearm.damage * 2;
        dmgReset = firearm.damage;
    }

    // Update is called once per frame
    void Update()
    {
        //Shelter in Place
        //___.text = Refraining from moving increases weapon base damage by 100% and reduces incoming damage by 80%. Resuming movement ends the bonus. Knockback from enemy attacks cannot remove this bonus. 

        refrainTimer += Time.deltaTime;
        //Debug.Log(refrainTimer.ToString("F0") + "s");

        //Movement resets damage immediately
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) 
        {
            refrainTimer = 0.0f;
            proc.GetComponent<Text>().text = " ";
            firearm.damage = dmgReset;
        }

        //Damage increases after two seconds
        if (refrainTimer >= 2.0f)
        {
            proc.GetComponent<Text>().text = "Shelter in Place";
            firearm.damage = dmgIncrease;
        }

        //Taking damage triggers damage resistance if Cheat is active
        if (player.playerHit == true && refrainTimer >= 2.0f)
        {
            damageNegate = player.dmgReceived;
            player.dmgReceived = 0;
            DamageResistConversion();
            player.playerHit = false;
        }           
    }

    /// <summary>
    /// Converts damaged received into Shield or Health dependent on condition
    /// </summary>
    void DamageResistConversion()
    {
        absorbPercent /= 100;
        absorbPercent *= damageNegate;
        dmgAbsorbAdd = (int)absorbPercent;
        absorbPercent = absPctReset;
        
        if(player.playerShield <= 0)
        {
            if(player.playerHealth >= player.playerHealthMax)
            {
                player.playerHealth = player.playerHealthMax;
            }

            else
            {
                player.playerHealth += dmgAbsorbAdd;
            }
        }

        else
        {
            if (player.playerShield >= player.playerShieldMax)
            {
                player.playerShield = player.playerShieldMax;
            }

            else
            {
                player.playerShield += dmgAbsorbAdd;

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
