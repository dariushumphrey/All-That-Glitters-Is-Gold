using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enshroud : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerInventoryScript player;
    private PlayerMeleeScript melee;
    private GameObject activation; //VFX used to convey activity
    internal GameObject proc; //Text UI that records Cheat activity

    private float meleePct = 15f; //% of Melee attack range
    private float percentCap = 200f; //% of maximum Melee attack range
    private float buffTimer = 7f; //Duration of effects
    private float buffTimerReset; //Holds starting duration value
    private float cooldownTimer = 12f; //Duration of effect timeout
    private float cooldownReset; //Holds starting timeout duration
    internal bool cooldown = false; //Affirms effects are inactive if true

    private float meleeReset; //Holds starting Melee attack range
    private int meleeExtend; //Number used to increase Melee attack range
    private int meleeCap; //Number used to limit Melee attack range

    internal bool hitConfirmed = false; //Affirms achieved hit if true
    internal bool killConfirmed = false; //Affirms achieved kill if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerInventoryScript>();
        melee = FindObjectOfType<PlayerMeleeScript>();
        activation = Resources.Load<GameObject>("Particles/EnshroudActive");

        buffTimerReset = buffTimer;
        cooldownReset = cooldownTimer;
        meleeReset = melee.meleeRange;

        meleePct /= 100;
        meleePct *= melee.meleeRange;
        meleeExtend = (int)meleePct;

        percentCap /= 100;
        percentCap *= melee.meleeRange;
        meleeCap = (int)percentCap + (int)melee.meleeRange;

        //Non-exotic Rarity 5 Weapons reduce cooldown to six seconds
        if(firearm.weaponRarity == 5)
        {
            cooldownTimer = 6f;
        }

        proc.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        //Enshroud
        //___.text = Enemy hits increase Melee attack range by 15%, up to 200% for five seconds. While active, melee kills cast a free, auto-detonating Fogger Grenade.

        //Permits use of Enshroud's melee effects while Weapon is active
        melee.enshroudCheat = gameObject;

        //Permits use of Enshroud's Grenade use if Weapon is Rarity 5
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

        //Confirmed hits increase Melee attack range
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

        //Effects expire when timer reaches zero
        buffTimer -= Time.deltaTime;
        if(buffTimer <= 0f)
        {
            buffTimer = 0f;
            melee.meleeRange = meleeReset;
        }

        //Cooldown expires when timer reaches zero
        if(cooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if(cooldownTimer <= 0f)
            {
                cooldownTimer = cooldownReset;
                cooldown = false;
            }
        }

        //Creates VFX to visualize maxed Melee range
        if (melee.meleeRange >= meleeCap && Time.timeScale == 1)
        {
            GameObject effect = Instantiate(activation, gameObject.transform.root.gameObject.transform.position + Vector3.down, transform.rotation);
            effect.name = activation.name;
        }
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
            melee.meleeRange = meleeReset;
            melee.enshroudCheat = null;
            player.enshroudPresent = false;
        }
    }
}
