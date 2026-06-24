using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerfectRecord : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerStatusScript player;
    private int startingWeaponDamage;
    private int startingMeleeDamage;
    private float weaponBuffPercent = 40f;
    private int weaponBuffAssign;
    private int fixedMeleeBuff = 5600;
    private float buffTimeGoal = 3f;
    private float elapsedTime = 0f;
    private bool timeout = false;
    internal GameObject proc; //Text UI that records Cheat activity

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        proc.GetComponent<Text>().text = "";

        startingWeaponDamage = firearm.damage;
        startingMeleeDamage = firearm.inv.GetComponent<PlayerMeleeScript>().meleeDamage;

        if (!firearm.isExotic && firearm.weaponRarity == 5)
        {
            fixedMeleeBuff = 7200;
            weaponBuffPercent = 80f;
        }

        weaponBuffPercent /= 100;
        weaponBuffPercent *= firearm.damage;
        weaponBuffAssign = firearm.damage + (int)weaponBuffPercent;
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.playerHit && !timeout)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= buffTimeGoal)
            {
                elapsedTime = buffTimeGoal;
                firearm.damage = weaponBuffAssign;
                firearm.inv.GetComponent<PlayerMeleeScript>().meleeDamage = fixedMeleeBuff;

                proc.GetComponent<Text>().text = "Perfect Record";
            }
        }

        else
        {
            if(firearm.weaponRarity != 5)
            {
                elapsedTime = 0f;
                firearm.damage = startingWeaponDamage;
                firearm.inv.GetComponent<PlayerMeleeScript>().meleeDamage = startingMeleeDamage;
                proc.GetComponent<Text>().text = "";

                player.playerHit = false;
                timeout = true;
                StartCoroutine(DisableTimeout());
            }         
        }
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }

        elapsedTime = 0f;
        player.playerHit = false;
        timeout = false;
        firearm.damage = startingWeaponDamage;
        firearm.inv.GetComponent<PlayerMeleeScript>().meleeDamage = startingMeleeDamage;
    }

    public IEnumerator DisableTimeout()
    {
        yield return new WaitForSeconds(5f);
        timeout = false;
    }
}
