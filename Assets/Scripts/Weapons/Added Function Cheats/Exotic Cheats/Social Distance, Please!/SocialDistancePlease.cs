using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocialDistancePlease : MonoBehaviour
{
    private ShotgunFirearm firearm;
    private PlayerStatusScript player;
    internal GameObject proc; //Text UI that records Cheat activity

    private float cheatDamagePercent = 30f; //% of Weapon damage
    private float damageSpreadPercent = 400f; //% of damage shared amongst Enemies
    private float damageTimer; //Duration of effect
    private int damageAdd; //Number used to increase Weapon damage
    private int damageShare; //Number used to spread damage to Enemies
    private int damageIncrease; //Fixed Weapon damage number
    private int damageReset; //Holds starting Weapon damage

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<ShotgunFirearm>();
        player = FindObjectOfType<PlayerStatusScript>();
        proc.GetComponent<Text>().text = " ";

        damageReset = firearm.damage;
        cheatDamagePercent /= 100;
        cheatDamagePercent *= firearm.damage;
        damageAdd = (int)cheatDamagePercent;
        damageIncrease += firearm.damage + damageAdd;

        damageSpreadPercent /= 100;
        damageSpreadPercent *= damageIncrease;
        damageShare = (int)damageSpreadPercent;

        damageTimer = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        //Social Distance, please!
        //___.text = Hitting an enemy increases weapon base damage by 30% and applies a debuff that amplifies damage taken by this weapon by x2. Hitting other enemies without a debuff refreshes the timer."

        //Debug.Log(damageTimer.ToString("F0") + "s");

        //if (!firearm.targetHit)
        //{
        //    Debug.Log("No one to transmit debuff to yet.");
        //}

        //Hit targets receive a Health debuff
        if (firearm.targetHit)
        {
            if (firearm.targetHit.gameObject.GetComponent<SDPHealthDebuff>() == null)
            {
                firearm.targetHit.gameObject.AddComponent<SDPHealthDebuff>();
                firearm.targetHit.gameObject.GetComponent<SDPHealthDebuff>().dmgShare = damageShare;
                proc.GetComponent<Text>().text = "Social Distance, please!";
                firearm.GetComponent<ShotgunFirearm>().targetHit = null;
                firearm.damage = damageIncrease;
                damageTimer = 0.0f;            
            }

            else
            {               
                proc.GetComponent<Text>().text = "";
                firearm.targetHit = null;
            }

            StartCoroutine(TextClear());

        }

        //Damage is restored to default when time expires
        if (firearm.damage == damageIncrease)
        {
            damageTimer += Time.deltaTime;          
            if (damageTimer >= 10f)
            {
                firearm.damage = damageReset;
                damageTimer = 0.0f;
            }
        }     
    }   

    IEnumerator TextClear()
    {
        yield return new WaitForSeconds(0.7f);
        proc.GetComponent<Text>().text = "";
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }
    }
}
