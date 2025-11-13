using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Efficacy : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc; //Text UI that records Cheat activity

    private float dmgPct = 1f; //% of Weapon' base damage
    private int dmgAdd; //Number used to add onto damage

    private float percentCap = 125f; //% of max damage allowed
    private int damageRoof; //Extent of damage increase

    private int dmgReset; //Holds Weapon' starting damage
    internal bool hitConfirmed = false; //Affirms achieved hit if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        proc.GetComponent<Text>().text = "";

        //Non-exotic Rarity 5 Weapons increase damage % added and sets a damage cap
        if (firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            dmgPct = 2f;

            percentCap /= 100;
            percentCap *= firearm.damage;
            damageRoof = (int)percentCap + firearm.damage;
        }

        dmgPct /= 100;
        dmgPct *= firearm.damage;
        dmgAdd = (int)dmgPct;

        dmgReset = firearm.damage;
    }

    // Update is called once per frame
    void Update()
    {

        //Efficacy
        //____.text = "Enemy hits increases this weapon's base damage by 1%. Reloading this weapon resets its base damage."

        //Confirmed kills adds % of damage onto current damage
        if (hitConfirmed == true)
        {
            firearm.damage += dmgAdd;

            if(firearm.weaponRarity == 5 && !firearm.isExotic)
            {
                if (firearm.damage >= damageRoof)
                {
                    firearm.damage = damageRoof;
                }
            }

            proc.GetComponent<Text>().text = "Efficacy";
            hitConfirmed = false;
            StartCoroutine(ClearText());
        }

        //Damage reset occurs while reloading as an Exotic weapon or a non-rarity 5 Weapon
        if (firearm.isReloading == true && firearm.weaponRarity != 5 || firearm.isReloading == true && firearm.isExotic)
        {
            firearm.damage = dmgReset;
            proc.GetComponent<Text>().text = "";
        }
    }

    IEnumerator ClearText()
    {
        yield return new WaitForSeconds(1.5f);
        proc.GetComponent<Text>().text = "";
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }

        firearm.damage = dmgReset;
    }
}