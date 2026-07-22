using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllElseFails : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc; //Text UI that records Cheat activity
    private PlayerStatusScript player;
    private GameObject activation; //VFX used to convey activity
    private int dmgNullify = 0; //Receives damage taken
    private float nullifyTimer = 5f; //Timer used to keep effects active
    private float cooldownTimer = 10f; //Duration of effect cooldown

    //nullifyReset - holds starting effect duration
    //cooldownReset - holds starting cooldown duration
    private float nullifyReset, cooldownReset;

    //invulnerable - Affirms Player immunity if true
    //cooldown - Affirms effect timeout if true
    private bool cooldown = false;
    private bool done = false; //Allows an effect once if true;

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        activation = Resources.Load<GameObject>("Particles/AllElseFailsActive");
        proc.GetComponent<Text>().text = "";

        var main = activation.GetComponent<ParticleSystem>().main;
        main.startLifetime = 5f;

        //Rarity 5 Weapons increase immunity duration and decrease cooldown
        //VFX duration is increased to match effect duration
        //if (!firearm.isExotic && firearm.weaponRarity == 5)
        //{
        //    nullifyTimer = 5f;
        //    cooldownTimer = 10f;
            
        //}

        nullifyReset = nullifyTimer;
        cooldownReset = cooldownTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //All Else Fails
        //___.text = When Shield is depleted, all incoming Enemy damage is nullified for three seconds. Cooldown: 20 Seconds. 

        if (player.playerShield <= 0 && !cooldown)
        {
            player.allElseFailsFlag = true;       
        }

        if (player.allElseFailsFlag)
        {
            //Produces VFX and childs effect to Player
            if (!done)
            {
                GameObject effect = Instantiate(activation, gameObject.transform.root.gameObject.transform.position + (Vector3.up * 1.5f), Quaternion.identity, gameObject.transform.root.gameObject.transform);
                if (!firearm.isExotic && firearm.weaponRarity == 5)
                {
                    effect.AddComponent<DestroyScript>();
                    effect.GetComponent<DestroyScript>().destroyTimer = 6f;
                }

                else
                {
                    effect.AddComponent<DestroyScript>();
                    effect.GetComponent<DestroyScript>().destroyTimer = 4f;
                }

                done = true;
            }

            nullifyTimer -= Time.deltaTime;
            proc.GetComponent<Text>().text = "All Else Fails: " + nullifyTimer.ToString("F0") + "s";

            //Effect expires and enters cooldown
            if (nullifyTimer <= 0f)
            {
                nullifyTimer = nullifyReset;
                player.allElseFailsFlag = false;
                //player.StartCoroutine(player.CancelInvulnerable());
                cooldown = true;
            }
        }     

        if (cooldown)
        {
            if (!firearm.isExotic && firearm.weaponRarity == 5)
            {
                done = false;
                cooldown = false;
                proc.GetComponent<Text>().text = "";
            }

            else
            {
                cooldownTimer -= Time.deltaTime;
                proc.GetComponent<Text>().text = "Cooldown: " + cooldownTimer.ToString("F0") + "s";

                if (cooldownTimer <= 0f)
                {
                    done = false;
                    cooldownTimer = cooldownReset;
                    cooldown = false;
                    proc.GetComponent<Text>().text = "";
                }
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
