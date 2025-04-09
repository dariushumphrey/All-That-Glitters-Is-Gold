using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllElseFails : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc;
    private PlayerStatusScript player;
    private int dmgNullify = 0;
    private float nullifyTimer = 3f;
    private float cooldownTimer = 20f;
    private float nullifyReset, cooldownReset;
    private bool invulnerable, cooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        proc.GetComponent<Text>().text = " ";

        if(firearm.weaponRarity == 5)
        {
            nullifyTimer = 5f;
            nullifyReset = nullifyTimer;

            cooldownTimer = 10f;
            cooldownReset = cooldownTimer;
        }

        else
        {
            nullifyReset = nullifyTimer;
            cooldownReset = cooldownTimer;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        //All Else Fails
        //___.text = When Shield is depleted, all incoming Enemy damage is nullified for three seconds. Cooldown: 20 Seconds. 

        if(player.playerShield <= 0 && !cooldown)
        {
            invulnerable = true;

            if(invulnerable)
            {
                if (player.playerHit == true)
                {
                    dmgNullify = player.dmgReceived;
                    player.dmgReceived = 0;

                    if (player.playerShield <= 0)
                    {
                        if (player.playerHealth >= player.playerHealthMax)
                        {
                            player.playerHealth = player.playerHealthMax;
                        }

                        else
                        {
                            player.playerHealth += dmgNullify;
                        }
                    }

                    else
                    {
                        player.playerShield += dmgNullify;
                    }

                    player.playerHit = false;
                }


                nullifyTimer -= Time.deltaTime;
                proc.GetComponent<Text>().text = "All Else Fails: " + nullifyTimer.ToString("F0") + "s";

                if (nullifyTimer <= 0f)
                {
                    nullifyTimer = nullifyReset;
                    invulnerable = false;
                    cooldown = true;                                    
                }
            }
        }

        if(cooldown)
        {
            cooldownTimer -= Time.deltaTime;
            proc.GetComponent<Text>().text = "Cooldown: " + cooldownTimer.ToString("F0") + "s";

            if (cooldownTimer <= 0f)
            {
                cooldownTimer = cooldownReset;
                cooldown = false;
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
