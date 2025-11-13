using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fulminate : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerInventoryScript player;
    private PlayerMeleeScript melee;
    private GameObject activation; //VFX used to convey activity
    internal GameObject proc; //Text UI that records Cheat activity

    private int destructDamage; //Damage to assign Destruct Grenade
    private float dmgPct = 2f; //% to increase Destruct Grenade damage by
    private float percentCap = 70f; //% of allowed Destruct Grenade damage
    private float buffTimer = 7f; //Duration of effect
    private float buffTimerReset; //Holds starting duration
    private bool fatedFulminate = false; //Affirms Rarity 5 behavior if true

    private int dmgAdd; //Number used to increase Destruct Grenade damage
    private int dmgCap; //Number used to limit Destruct Grenade damage to

    private int destructReset; //Holds starting Destruct Grenade damage
    internal bool hitConfirmed = false; //Affirms achieved hit if true
    internal bool killConfirmed = false; //Affirms achieved kill if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerInventoryScript>();
        melee = FindObjectOfType<PlayerMeleeScript>();
        activation = Resources.Load<GameObject>("Particles/FulminateActive");

        destructDamage = player.grenades[2].GetComponent<DestructGrenadeScript>().explosiveDamage;
        destructReset = destructDamage;
        buffTimerReset = buffTimer;

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

        //Grants access to grenade on Melee kills
        melee.fulminateCheat = gameObject;

        //Grants access to double thrown Destruct Grenades
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

        //Increases Destruct Grenade damage on Weapon hits
        //Assigns current damage to new damage
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

        //Restores default damage when timer expires
        buffTimer -= Time.deltaTime;
        if(buffTimer <= 0f)
        {
            buffTimer = 0f;
            destructDamage = destructReset;
            player.fulminatePresent = false;
        }

        //Creates VFX to visualize maxed damage
        if(destructDamage == dmgCap && Time.timeScale == 1)
        {
            GameObject effect = Instantiate(activation, gameObject.transform.root.gameObject.transform.position + Vector3.down, transform.rotation);
            effect.name = activation.name;
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
