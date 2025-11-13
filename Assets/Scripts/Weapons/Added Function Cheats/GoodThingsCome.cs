using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodThingsCome : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerMoveScript move;
    private PlayerStatusScript player;
    internal GameObject proc; //Text UI that records Cheat activity
    private float movementPercent = 25f; //% of Movement Speed to increase
    private int moveNew; //New Player speed 
    private float resistancePercent = 20f; //% of Damage received to resist
    private int dmgReduce; //Receives damage taken
    private int resistanceAdd; //Number used to restore Player Health/Shield
    private float recoilPercent = 45f; //% of Recoil to reduce
    private float recoilNew; //New Weapon recoil value
    private float granterTimer = 0f; //Timer used to activate effects
    private float granterStop = 5f; //Timer used to revert effects
    private float granterReset; //Holds starting timer value (granterStop)
    private int movementReset; //Holds starting Player Movement Speed
    private int reserveReset; //Holds current Weapon reserve ammo
    private float resistanceReset; //Holds % of Damage resistance
    private float recoilReset; //Holds starting Weapon recoil
    internal bool hitConfirmed; //Affirms achieved hit if true
    private bool inFirefight = false; //Affirms active Player combat if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        move = FindObjectOfType<PlayerMoveScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        proc.GetComponent<Text>().text = "";

        granterReset = granterStop;

        //Non-exotic Rarity 5 Weapons double the percentages of 
        //Damage resistance, Movement Speed, and Recoil reduction
        if(firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            resistancePercent *= 2;
            movementPercent *= 2;
            recoilPercent *= 2;
        }

        movementReset = move.speed;
        resistanceReset = resistancePercent;
        recoilReset = firearm.wepRecoil;

        movementPercent /= 100;
        movementPercent *= move.speed;
        moveNew = move.speed + (int)movementPercent;

        resistancePercent /= 100;
        resistancePercent *= dmgReduce;
        resistanceAdd = (int)resistancePercent;

        recoilPercent /= 100;
        recoilPercent *= firearm.wepRecoil;
        recoilNew = firearm.wepRecoil - recoilPercent;
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

        //Inflicting or receiving damage places Player in a firefight state
        if(hitConfirmed && firearm.enabled == true || player.playerHit == true && firearm.enabled == true) 
        {
            inFirefight = true;
            hitConfirmed = false;
            granterStop = granterReset;
        }

        if(inFirefight)
        {
            //Being in a firefight grants benefits immediately
            if(firearm.weaponRarity == 5 && !firearm.isExotic)
            {
                move.speed = moveNew;

                dmgReduce = player.dmgReceived;
                player.dmgReceived = 0;
                DamageResistConversion();
                player.playerHit = false;

                firearm.wepRecoil = recoilNew;

                reserveReset = firearm.reserveAmmo;
                firearm.reserveAmmo = reserveReset;

                firearm.ammoSpent = 0;

                proc.GetComponent<Text>().text = "Good Things Come Instantly";
            }

            else
            {
                //Being in a firefight for a short time grants benefits
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

            //Deactivates benefits when Player disengages from a firefight
            granterStop -= Time.deltaTime;
            if(granterStop <= 0f)
            {
                granterStop = granterReset;
                granterTimer = 0f;

                move.speed = movementReset;
                firearm.wepRecoil = recoilReset;

                inFirefight = false;
                proc.GetComponent<Text>().text = "";
            }
        }      
    }

    /// <summary>
    /// Returns modified damage received back to the Player as Health/Shield
    /// </summary>
    void DamageResistConversion()
    {
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
