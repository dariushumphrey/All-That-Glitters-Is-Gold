using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarlyBerthGetsTheHearst : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc;
    private EnemyManagerScript enemy;

    private float ebgthPercent = 200f;
    private int ebgthDamage;
    private int triggerCount = 2;
    private int shotCount;

    private int berthChance;
    internal bool hitConfirmed;
    // Start is called before the first frame update
    void Start()
    {
        proc.GetComponent<Text>().text = " ";
        enemy = FindObjectOfType<EnemyManagerScript>();

        if (GetComponent<SingleFireFirearm>() != null)
        {
            firearm = GetComponent<SingleFireFirearm>();
            ebgthPercent /= 100;
            ebgthPercent *= firearm.damage;
            ebgthDamage = (int)ebgthPercent;

        }

        else
        {
            //firearm = GetComponent<FirearmScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Early Berth gets the Hearst
        //___.text = Every other Enemy hit triggers a Berth detonation, inflicting 200% of Weapon damage. 

        if (hitConfirmed)
        {
            shotCount++;
            hitConfirmed = false;
        }

        if (shotCount >= triggerCount)
        {
            shotCount = 0;

            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.AddComponent<BerthScript>();
            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().exoticOverride = true;

            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().berthDamage = ebgthDamage;
            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().explodeForce = 30.0f;
            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().Explode();
            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().StartCoroutine(
                firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().RemoveSelf());


            proc.GetComponent<Text>().text = "Early Berth gets the Hearst";
            StartCoroutine(TextClear());
        }

        //if (firearm.GetComponent<SingleFireFirearm>().targetHit != null)
        //{
        //    if (firearm.GetComponent<SingleFireFirearm>().targetHit.GetComponent<EnemyHealthScript>().enemyHit == true)
        //    {
        //        berthChance = Random.Range(0, 101);

        //        if (firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>() == null)
        //        {
        //            if (berthChance >= 85)
        //            {
        //                firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.AddComponent<BerthScript>();
        //                firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().additionalDamage = ebgthDamage;
        //                firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().explodeForce = 30.0f;
        //                //firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().Start();
        //                firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().Explode();

        //                proc.GetComponent<Text>().text = "Early Berth gets the Hearst";
        //                StartCoroutine(TextClear());
        //            }
        //        }

        //        if (firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>() != null)
        //        {
        //            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().explodeForce = 30.0f;
        //            firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().Explode();

        //            proc.GetComponent<Text>().text = "Early Berth gets the Hearst";
        //            StartCoroutine(TextClear());
        //        }

        //        firearm.GetComponent<SingleFireFirearm>().targetHit.GetComponent<EnemyHealthScript>().enemyHit = false;
        //    }
        //}
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
