using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarlyBerthGetsTheHearst : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc;
    private int berthChance;
    private float reloadReset;
    // Start is called before the first frame update
    void Start()
    {
        proc.GetComponent<Text>().text = " ";

        if (GetComponent<SingleFireFirearm>() != null)
        {
            firearm = GetComponent<SingleFireFirearm>();
            reloadReset = firearm.reloadSpeed;

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
        //___.text = Enemy hits have a chance to emit a Replevey Berth explosion. Berth hits always trigger their explosion. 

        if (firearm.GetComponent<SingleFireFirearm>().targetHit != null)
        {
            if (firearm.GetComponent<SingleFireFirearm>().targetHit.GetComponent<EnemyHealthScript>().enemyHit == true)
            {
                berthChance = Random.Range(0, 101);

                if (firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>() == null)
                {
                    if (berthChance >= 90)
                    {
                        firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.AddComponent<BerthScript>();
                        firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().explodeForce = 30.0f;
                        firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().Start();
                        firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().Explode();

                        proc.GetComponent<Text>().text = "Early Berth gets the Hearst";
                        StartCoroutine(TextClear());
                    }
                }

                if (firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>() != null)
                {
                    firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().explodeForce = 30.0f;
                    firearm.GetComponent<SingleFireFirearm>().targetHit.gameObject.GetComponent<BerthScript>().Explode();

                    proc.GetComponent<Text>().text = "Early Berth gets the Hearst";
                    StartCoroutine(TextClear());
                }

                firearm.GetComponent<SingleFireFirearm>().targetHit.GetComponent<EnemyHealthScript>().enemyHit = false;
            }
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
