using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodThingsCome : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerMoveScript move;
    private PlayerStatusScript player;
    internal GameObject proc;
    private float movementPercent = 25f;
    private int moveNew;
    private float resistancePercent = 20f;
    private int dmgReduce;
    private int resistanceAdd;
    private float recoilPercent = 45f;
    private float recoilNew;
    private float granterTimer = 0f;
    private float granterStop = 5f;
    private float granterReset;
    private int movementReset;
    private float resistanceReset;
    private float recoilReset;
    internal bool hitConfirmed;
    private bool inFirefight = false;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        move = FindObjectOfType<PlayerMoveScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        proc.GetComponent<Text>().text = " ";

        granterReset = granterStop;

        if(firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            resistancePercent *= 2;
            resistanceReset = resistancePercent;

            movementPercent *= 2;
            movementReset = move.speed;
            movementPercent /= 100;
            movementPercent *= move.speed;
            moveNew = move.speed + (int)movementPercent;

            recoilPercent *= 2;
            recoilReset = firearm.wepRecoil;
            recoilPercent /= 100;
            recoilPercent *= firearm.wepRecoil;
            recoilNew = firearm.wepRecoil - recoilPercent;
        }

        else
        {
            resistanceReset = resistancePercent;

            movementReset = move.speed;
            movementPercent /= 100;
            movementPercent *= move.speed;
            moveNew = move.speed + (int)movementPercent;

            recoilReset = firearm.wepRecoil;
            recoilPercent /= 100;
            recoilPercent *= firearm.wepRecoil;
            recoilNew = firearm.wepRecoil - recoilPercent;
        }         
    }

    // Update is called once per frame
    void Update()
    {
        //Good Things Come
        //___.text = Being in combat for three seconds grants three benefits: 
        //25% Movement Speed Increase
        //20% Damage Resistance
        //45% Recoil Reduction
        //The bonus ends when you exit combat.

        if(hitConfirmed && firearm.enabled == true || player.playerHit == true && firearm.enabled == true) 
        {
            inFirefight = true;
            hitConfirmed = false;
            granterStop = granterReset;
        }

        if(inFirefight)
        {
            if(firearm.weaponRarity == 5 && !firearm.isExotic)
            {
                move.speed = moveNew;

                dmgReduce = player.dmgReceived;
                player.dmgReceived = 0;
                DamageResistConversion();
                player.playerHit = false;

                firearm.wepRecoil = recoilNew;

                firearm.currentAmmo = firearm.ammoSize;
                firearm.ammoSpent = 0;

                proc.GetComponent<Text>().text = "Good Things Come Instantly";
            }

            else
            {
                granterTimer += Time.deltaTime;
                if (granterTimer >= 3f)
                {
                    move.speed = moveNew;

                    dmgReduce = player.dmgReceived;
                    player.dmgReceived = 0;
                    DamageResistConversion();
                    player.playerHit = false;

                    firearm.wepRecoil = recoilNew;

                    proc.GetComponent<Text>().text = "Good Things Come";
                }
            }         

            granterStop -= Time.deltaTime;
            if(granterStop <= 0f)
            {
                granterStop = granterReset;
                granterTimer = 0f;

                move.speed = movementReset;
                firearm.wepRecoil = recoilReset;

                inFirefight = false;
                proc.GetComponent<Text>().text = " ";
            }
        }      
    }

    void DamageResistConversion()
    {
        resistancePercent /= 100;
        resistancePercent *= dmgReduce;
        resistanceAdd = (int)resistancePercent;
        resistancePercent = resistanceReset;

        if (player.playerShield <= 0)
        {
            if (player.playerHealth >= player.playerHealthMax)
            {
                player.playerHealth = player.playerHealthMax;
            }

            else
            {
                player.playerHealth += resistanceAdd;
            }
        }

        else
        {
            player.playerShield += resistanceAdd;
        }
    }

    private void OnDisable()
    {
        if(proc != null)
        {
            proc.GetComponent<Text>().text = " ";
        }

        if(inFirefight)
        {
            granterStop = granterReset;
            granterTimer = 0f;
            move.speed = movementReset;
            firearm.wepRecoil = recoilReset;
            inFirefight = false;
        }
    }
}
