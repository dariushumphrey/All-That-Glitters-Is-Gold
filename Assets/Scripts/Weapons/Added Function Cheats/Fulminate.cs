using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fulminate : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerInventoryScript player;
    private PlayerMeleeScript melee;
    internal GameObject proc;

    private int destructDamage;
    private float dmgPct = 2f;
    private float percentCap = 70f;
    private float buffTimer = 7f;
    private float buffTimerReset;
    private float cooldownTimer = 20f;
    private float cooldownReset;
    private bool fatedFulminate = false;
    internal bool cooldown = false;

    private int dmgAdd;
    private int dmgCap;

    private int destructReset;
    internal bool hitConfirmed = false;
    internal bool killConfirmed = false;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerInventoryScript>();
        melee = FindObjectOfType<PlayerMeleeScript>();

        destructDamage = player.grenades[2].GetComponent<DestructGrenadeScript>().explosiveDamage;
        destructReset = destructDamage;
        buffTimerReset = buffTimer;
        cooldownReset = cooldownTimer;

        dmgPct /= 100;
        dmgPct *= destructDamage;
        dmgAdd = (int)dmgPct;

        percentCap /= 100;
        percentCap *= destructDamage;
        dmgCap = (int)percentCap + destructDamage;

        if(firearm.weaponRarity == 5)
        {
            fatedFulminate = true;
        }

        proc.GetComponent<Text>().text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        //Fulminate
        //___.text = Enemy hits increase Destruct Grenade damage by 7%, up to 70% for seven seconds. While active, melee kills cast a free, auto-detonating Destruct Grenade.

        melee.fulminateCheat = gameObject;
        if(fatedFulminate)
        {
            player.fulminateFated = true;
        }

        if (destructDamage != destructReset)
        {
            proc.GetComponent<Text>().text = "Fulminate: " + buffTimer.ToString("F0") + "s";
        }

        else
        {
            proc.GetComponent<Text>().text = "";
        }

        if (hitConfirmed == true)
        {
            player.fulminatePresent = true;

            destructDamage += dmgAdd;
            if(destructDamage >= dmgCap)
            {
                destructDamage = dmgCap;
            }

            player.fulminateBuff = destructDamage;

            buffTimer = buffTimerReset;


            hitConfirmed = false;
        }

        buffTimer -= Time.deltaTime;
        if(buffTimer <= 0f)
        {
            buffTimer = 0f;
            destructDamage = destructReset;
            player.fulminatePresent = false;

        }
    }

    IEnumerator ClearText()
    {
        yield return new WaitForSeconds(1.5f);
        proc.GetComponent<Text>().text = " ";
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = " ";
            player.fulminatePresent = false;
            destructDamage = destructReset;
            melee.fulminateCheat = null;
            player.fulminateFated = false;
        }
    }
}
