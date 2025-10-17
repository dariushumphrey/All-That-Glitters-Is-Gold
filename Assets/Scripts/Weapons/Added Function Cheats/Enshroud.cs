using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enshroud : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerInventoryScript player;
    private PlayerMeleeScript melee;
    internal GameObject proc;

    private float meleePct = 15f;
    private float percentCap = 200f;
    private float buffTimer = 7f;
    private float buffTimerReset;
    private float cooldownTimer = 12f;
    private float cooldownReset;
    internal bool cooldown = false;

    private float meleeReset;
    private int meleeExtend;
    private int meleeCap;

    private int destructReset;
    internal bool hitConfirmed = false;
    internal bool killConfirmed = false;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerInventoryScript>();
        melee = FindObjectOfType<PlayerMeleeScript>();

        buffTimerReset = buffTimer;
        cooldownReset = cooldownTimer;
        meleeReset = melee.meleeRange;

        meleePct /= 100;
        meleePct *= melee.meleeRange;
        meleeExtend = (int)meleePct;

        percentCap /= 100;
        percentCap *= melee.meleeRange;
        meleeCap = (int)percentCap + (int)melee.meleeRange;

        if(firearm.weaponRarity == 5)
        {
            cooldownTimer = 6f;
        }

        proc.GetComponent<Text>().text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        //Enshroud
        //___.text = Enemy hits increase Melee attack range by 15%, up to 200% for five seconds. While active, melee kills cast a free, auto-detonating Fogger Grenade.

        melee.enshroudCheat = gameObject;

        if(firearm.weaponRarity == 5)
        {
            player.enshroudPresent = true;
        }
        
        if (melee.meleeRange != meleeReset)
        {
            proc.GetComponent<Text>().text = "Enshroud: " + buffTimer.ToString("F0") + "s";
        }

        else
        {
            proc.GetComponent<Text>().text = "";
        }

        if (hitConfirmed == true)
        {

            melee.meleeRange += meleeExtend;
            if(melee.meleeRange >= meleeCap)
            {
                melee.meleeRange = meleeCap;
            }

            buffTimer = buffTimerReset;
            hitConfirmed = false;
        }

        buffTimer -= Time.deltaTime;
        if(buffTimer <= 0f)
        {
            buffTimer = 0f;
            melee.meleeRange = meleeReset;

        }

        if(cooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if(cooldownTimer <= 0f)
            {
                cooldownTimer = cooldownReset;
                cooldown = false;
            }
        }
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = " ";
            melee.meleeRange = meleeReset;
            melee.enshroudCheat = null;
            player.enshroudPresent = false;
        }
    }
}
