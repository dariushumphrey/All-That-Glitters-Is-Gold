using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Defiance : MonoBehaviour
{
    private MultiWeaponFirearm firearm;
    internal GameObject proc; //Text UI that records Cheat activity

    private float damagePercent = 200f;
    private int damageAssign;
    private float rangePercent = 200f;
    private int rangeAssign;
    private int startingDamage;
    private float startingRange;

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<MultiWeaponFirearm>();
        startingDamage = firearm.inv.GetComponent<PlayerMeleeScript>().meleeDamage;
        startingRange = firearm.inv.GetComponent<PlayerMeleeScript>().meleeRange;

        damagePercent /= 100;
        damagePercent *= firearm.inv.GetComponent<PlayerMeleeScript>().meleeDamage;
        damageAssign = (int)damagePercent;

        rangePercent /= 100;
        rangePercent *= firearm.inv.GetComponent<PlayerMeleeScript>().meleeRange;
        rangeAssign = (int)rangePercent;

        if (proc)
        {
            proc.GetComponent<Text>().text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        firearm.inv.GetComponent<PlayerMeleeScript>().defiancePresent = true;
        firearm.inv.GetComponent<PlayerMeleeScript>().meleeDamage = damageAssign;
        firearm.inv.GetComponent<PlayerMeleeScript>().meleeRange = rangeAssign;

        if(firearm.inv.GetComponent<PlayerMeleeScript>().effectSignal)
        {
            EffectTriggerSignal();
            firearm.inv.GetComponent<PlayerMeleeScript>().effectSignal = false;
        }
    }

    public void EffectTriggerSignal()
    {
        proc.GetComponent<Text>().text = "Defiance";
        StartCoroutine(RemoveTriggerSignal());
    }

    private IEnumerator RemoveTriggerSignal()
    {
        yield return new WaitForSeconds(1f);
        proc.GetComponent<Text>().text = "";
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";

            firearm.inv.GetComponent<PlayerMeleeScript>().defiancePresent = false;
            firearm.inv.GetComponent<PlayerMeleeScript>().meleeDamage = startingDamage;
            firearm.inv.GetComponent<PlayerMeleeScript>().meleeRange = startingRange;
        }
    }
}
