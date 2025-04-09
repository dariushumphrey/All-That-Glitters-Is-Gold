using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Efficacy : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc;

    private float dmgPct = 1f;
    private float fatedDmgPct = 2f;
    private float percentCap = 125f;

    private int dmgAdd;
    private int fatedDmgAdd;
    private int damageRoof;

    private int dmgReset;
    internal bool hitConfirmed = false;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        proc.GetComponent<Text>().text = " ";

        dmgPct /= 100;
        fatedDmgPct /= 100;
        percentCap /= 100;

        dmgPct *= firearm.damage;
        fatedDmgPct *= firearm.damage;
        percentCap *= firearm.damage;

        dmgAdd = (int)dmgPct;
        fatedDmgAdd = (int)fatedDmgPct;
        damageRoof = (int)percentCap + firearm.damage;

        dmgReset = firearm.damage;
    }

    // Update is called once per frame
    void Update()
    {

        //Efficacy
        //____.text = "Enemy hits increases this weapon's base damage by 1%. Reloading this weapon resets its base damage."

        if (hitConfirmed == true)
        {
            if(firearm.weaponRarity == 5 && !firearm.isExotic)
            {
                firearm.damage += fatedDmgAdd;

                if (firearm.damage >= damageRoof)
                {
                    firearm.damage = damageRoof;
                }
            }

            else
            {
                firearm.damage += dmgAdd;
            }

            proc.GetComponent<Text>().text = "Efficacy";
            hitConfirmed = false;
            StartCoroutine(ClearText());
        }

        if (firearm.isReloading == true && firearm.weaponRarity != 5 || firearm.isReloading == true && firearm.isExotic)
        {
            firearm.damage = dmgReset;
            proc.GetComponent<Text>().text = " ";
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
        }
    }
}
