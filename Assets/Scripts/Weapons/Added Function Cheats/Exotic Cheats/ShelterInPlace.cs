using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShelterInPlace : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc;
    private PlayerStatusScript player;
    private float absorbPercent = 80f;
    private float absPctReset;
    private float refrainTimer = 0.0f;
    private float refrainDamagePercent = 100f;
    private float rfnDmgReset;
    private int refrainDamageAdd;
    private int dmgAbsorbAdd;
    private int damageNegate = 0;
    private int dmgIncrease;
    private int dmgReset;
   
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        proc.GetComponent<Text>().text = " ";

        absPctReset = absorbPercent;
        rfnDmgReset = refrainDamagePercent;
        dmgIncrease = firearm.damage + refrainDamageAdd;

        dmgReset = firearm.damage;
        refrainDamagePercent /= 100;
        refrainDamagePercent *= firearm.damage;
        refrainDamageAdd = (int)refrainDamagePercent;
        refrainDamagePercent = rfnDmgReset;
        dmgIncrease = firearm.damage + refrainDamageAdd;

    }

    // Update is called once per frame
    void Update()
    {
        //Shelter in Place
        //___.text = Refraining from moving increases weapon base damage by 100% and reduces incoming damage by 80%. Resuming movement ends the bonus. Knockback from enemy attacks cannot remove this bonus. 

        refrainTimer += Time.deltaTime;
        //Debug.Log(refrainTimer.ToString("F0") + "s");

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            refrainTimer = 0.0f;
            proc.GetComponent<Text>().text = " ";
            firearm.damage = dmgReset;
        }

        if (refrainTimer >= 2.0f)
        {
            proc.GetComponent<Text>().text = "Shelter in Place";
            firearm.damage = dmgIncrease;
        }

        if (player.playerHit == true && refrainTimer >= 2.0f)
        {
            damageNegate = player.dmgReceived;
            player.dmgReceived = 0;
            DamageResistConversion();
            player.playerHit = false;
        }           
    }

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
            proc.GetComponent<Text>().text = " ";
        }
    }
}
