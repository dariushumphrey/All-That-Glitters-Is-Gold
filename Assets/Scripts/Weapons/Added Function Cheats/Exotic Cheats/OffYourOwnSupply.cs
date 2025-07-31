using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffYourOwnSupply : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc;
    private PlayerStatusScript player;
    private float movementPercent = 10f;
    private int moveAdd;
    private float recoilZero;
    private float recoilReset;
    private float reloadPercent = 20f;
    private float reloadReset;
    private float baseReloadReset;
    private float damagePercent = 20f;
    private int damageAdd;
    private int damageIncrease;
    private int dmgIncOne, dmgIncTwo, dmgIncThree;
    private int dmgReset;
    private int speedReset;

    private float procTimer;
    private float procTimerReset = 15.0f;
    private bool procConfirm;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        proc.GetComponent<Text>().text = " ";
        player = FindObjectOfType<PlayerStatusScript>();

        movementPercent /= 100;
        movementPercent *= player.GetComponent<PlayerMoveScript>().speed;
        moveAdd = (int)movementPercent;
        speedReset = player.GetComponent<PlayerMoveScript>().speed;

        recoilZero = 0.0f;
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

        if(procConfirm == true)
        {
            procTimer -= Time.deltaTime;
            player.regenShieldSeconds = 7.0f;

            proc.GetComponent<Text>().text = "Off your own Supply: " + procTimer.ToString("F0") + "s";

            firearm.damage = damageIncrease;          

            if (procTimer < 0.0f)
            {
                firearm.damage = dmgReset;
                firearm.wepRecoil = recoilReset;
                firearm.reloadReset = baseReloadReset;
                firearm.reloadSpeed = firearm.reloadReset;
                player.GetComponent<PlayerMoveScript>().speed = speedReset;

                //player.StartCoroutine(player.RechargeShield());
                proc.GetComponent<Text>().text = " ";
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
            proc.GetComponent<Text>().text = " ";
        }
    }
}
