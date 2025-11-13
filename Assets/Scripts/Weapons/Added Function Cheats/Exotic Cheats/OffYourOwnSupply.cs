using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffYourOwnSupply : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc; //Text UI that records Cheat activity
    private PlayerStatusScript player;
    private float movementPercent = 10f; //% of Player Movement Speed
    private int moveAdd; //Number used to increase Movement Speed
    private float recoilZero = 0f;
    private float recoilReset; //Holds starting Weapon recoil
    private float reloadPercent = 20f; //% of increased Reload Speed

    //reloadReset, baseReloadReset - Holds starting Weapon Reload Speed
    private float reloadReset;
    private float baseReloadReset;
    private float damagePercent = 20f; //% of Weapon damage
    private int damageAdd; //Number to increase Weapon damage
    private int damageIncrease; //Fixed Weapon damage
    private int dmgReset; //Holds starting Weapon damage
    private int speedReset; //Holds starting Movement Speed

    private float procTimer; //Duration of effects
    private float procTimerReset = 15.0f; //Holds starting effect duration
    private bool procConfirm; //Affirms Cheat activation if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        proc.GetComponent<Text>().text = "";
        player = FindObjectOfType<PlayerStatusScript>();

        movementPercent /= 100;
        movementPercent *= player.GetComponent<PlayerMoveScript>().speed;
        moveAdd = (int)movementPercent;
        speedReset = player.GetComponent<PlayerMoveScript>().speed;

        recoilReset = firearm.wepRecoil;

        reloadPercent /= 100;
        reloadPercent *= firearm.reloadSpeed;
        reloadReset = firearm.reloadSpeed;
        baseReloadReset = firearm.reloadReset;

        dmgReset = firearm.damage;
        damagePercent /= 100;
        damagePercent *= firearm.damage;
        damageAdd = (int)damagePercent;
        damageIncrease = (firearm.damage + damageAdd) * 2;

        procConfirm = false;
        procTimer = procTimerReset;
    }

    // Update is called once per frame
    void Update()
    {
        //Off your own Supply
        //___.text = "Sacrifice your full shield for the following benefits:
        //-10% increase in Movement Speed
        //-Zeroed Recoil
        //-80% decrease in Reload Speed
        //-20% increase in base damage amplified by x2
        //The duration before your Shield begins regenerating will not resume until the bonus ends.

        //Zeroes shield, recoil, increases Movement Speed, and reduces Reload Speed on input
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (procConfirm != true)
            {
                player.playerShield = 0;
                player.GetComponent<PlayerMoveScript>().speed += moveAdd;
                firearm.wepRecoil = recoilZero;
                baseReloadReset = firearm.reloadReset;
                firearm.reloadReset = reloadPercent;
                firearm.reloadSpeed = reloadPercent;
                procConfirm = true;
            }
        }

        //Increases damage for duration of effect
        if(procConfirm == true)
        {
            procTimer -= Time.deltaTime;
            player.regenShieldSeconds = 7.0f;

            proc.GetComponent<Text>().text = "Off your own Supply: " + procTimer.ToString("F0") + "s";

            firearm.damage = damageIncrease;          

            //Restores attributes to default levels when timer expires
            if (procTimer < 0.0f)
            {
                firearm.damage = dmgReset;
                firearm.wepRecoil = recoilReset;
                firearm.reloadReset = baseReloadReset;
                firearm.reloadSpeed = firearm.reloadReset;
                player.GetComponent<PlayerMoveScript>().speed = speedReset;

                proc.GetComponent<Text>().text = "";
                procTimer = procTimerReset;
                procConfirm = false;
                return;
            }
        }
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }
    }
}
