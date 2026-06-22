using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ossify : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerStatusScript player;
    private GameObject activation; //VFX used to convey activity
    internal GameObject proc; //Text UI that records Cheat activity

    private float drPct = 3f; //Current damage resistance %
    private float drPctReset; //Holds starting damage resistance %
    private float percentCap = 30f; //% of allowed Destruct Grenade damage
    private int destructReset; //Holds starting Destruct Grenade damage
    private float buffTimer = 10f; //Duration of effect
    private float buffTimerReset; //Holds starting duration
    private int drAdd;
    private int damageNegate = 0; //Receives damage taken
    internal bool hitConfirmed = false; //Affirms achieved hit if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerStatusScript>();

        proc.GetComponent<Text>().text = " ";
        activation = Resources.Load<GameObject>("Particles/OssifyMaxPercentActive");

        if (!firearm.isExotic && firearm.weaponRarity == 5)
        {
            drPct = 5f;
            percentCap = 50f;
            buffTimer = 20f;
        }

        drPctReset = drPct;
        buffTimerReset = buffTimer;

    }

    // Update is called once per frame
    void Update()
    {
        if (hitConfirmed == true)
        {
            drPct += drPctReset;
            if (drPct >= percentCap)
            {
                drPct = percentCap;
            }

            buffTimer = buffTimerReset;
            hitConfirmed = false;
        }

        if (drPct != drPctReset)
        {
            proc.GetComponent<Text>().text = "Ossify: " + buffTimer.ToString("F0") + "s";
        }

        else
        {
            proc.GetComponent<Text>().text = "";
        }

        //Taking damage triggers damage resistance if Cheat is active
        if (player.playerHit == true && drPct != drPctReset)
        {
            damageNegate = player.dmgReceived;
            player.dmgReceived = 0;
            DamageResistConversion();
            player.playerHit = false;
        }

        //Restores default damage when timer expires
        buffTimer -= Time.deltaTime;
        if (buffTimer <= 0f)
        {
            buffTimer = 0f;
            drPct = drPctReset;
        }

        if (drPct >= percentCap && Time.timeScale == 1)
        {
            GameObject effect = Instantiate(activation, gameObject.transform.root.gameObject.transform.position, Quaternion.identity);
            effect.name = activation.name;
        }
    }

    void DamageResistConversion()
    {
        float num = drPct;

        num /= 100;
        num *= damageNegate;
        drAdd = (int)num;

        //drPct = drPctReset;

        if (player.playerShield <= 0)
        {
            if (player.playerHealth >= player.playerHealthMax)
            {
                player.playerHealth = player.playerHealthMax;
            }

            else
            {
                player.playerHealth += drAdd;
            }
        }

        else
        {
            player.playerShield += drAdd;
        }
    }

    public void RemoteProc()
    {
        drPct += drPctReset;
        if (drPct >= percentCap)
        {
            drPct = percentCap;
        }

        buffTimer = buffTimerReset;
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = " ";
        }

        drPctReset = drPct;
        buffTimerReset = buffTimer;
    }
}
