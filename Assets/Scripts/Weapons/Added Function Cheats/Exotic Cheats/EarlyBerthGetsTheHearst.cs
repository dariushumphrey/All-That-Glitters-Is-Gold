using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarlyBerthGetsTheHearst : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc; //Text UI that records Cheat activity
    private EnemyManagerScript enemy;

    private float ebgthPercent = 200f; //% of Berth damage applied from Weapon damage
    private int ebgthDamage; //Number used to apply Berth damage
    private int triggerCount = 2; //Goal number for effect trigger
    private int shotCount; //Total number of confirmed hits
    internal bool hitConfirmed; //Affirms achieved hit if true

    // Start is called before the first frame update
    void Start()
    {
        proc.GetComponent<Text>().text = " ";
        enemy = FindObjectOfType<EnemyManagerScript>();

        if (GetComponent<SingleFireFirearm>() != null)
        {
            firearm = GetComponent<SingleFireFirearm>();
        }

        ebgthPercent /= 100;
        ebgthPercent *= firearm.damage;
        ebgthDamage = (int)ebgthPercent;
    }

    // Update is called once per frame
    void Update()
    {
        //Early Berth gets the Hearst
        //___.text = Every other Enemy hit triggers a Berth detonation, inflicting 200% of Weapon damage. 

        //Confirmed hits increases hit counter
        if (hitConfirmed)
        {
            shotCount++;
            hitConfirmed = false;
        }

        //Applies and invokes Berth detonation on Enemies when goal hits are reached
        if (shotCount >= triggerCount)
        {
            shotCount = 0;

            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.AddComponent<BerthScript>();
            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().exoticOverride = true; //Deactivates Player damage

            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().berthDamage = ebgthDamage;
            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().explodeForce = 30.0f;
            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().Explode();
            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().StartCoroutine(
                firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().RemoveSelf());


            proc.GetComponent<Text>().text = "Early Berth gets the Hearst";
            StartCoroutine(TextClear());
        }
    }

    IEnumerator TextClear()
    {
        yield return new WaitForSeconds(1f);
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
