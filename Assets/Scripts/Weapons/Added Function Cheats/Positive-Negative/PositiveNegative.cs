using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositiveNegative : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc; //Text UI that records Cheat activity
    private PlayerMoveScript move;
    private GameObject activation; //VFX used to convey activity
    private Material originalBullet, electricBullet; //Materials to change Weapon bullet trail
    private float chargePercentage = 0f; //Current % of charge
    private int chargeAccelerant = 20; //Multipler used to increase charge rate
    private float dotPercent = 100f; //% of Weapon damage used for damage-over-time
    private int dotStrength; //Damage used for damage-over-time

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        move = FindObjectOfType<PlayerMoveScript>();
        activation = Resources.Load<GameObject>("Particles/PositiveNegativeActive");
        electricBullet = Resources.Load<Material>("Materials/Weapons/BulletMaterialPositiveNegative");
        originalBullet = firearm.bulletTrail;

        //Non-exotic Rarity 5 Weapons increase damage-over-time percentage
        if (firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            dotPercent = 200f;
        }

        dotPercent /= 100;
        dotPercent *= firearm.damage;
        dotStrength = (int)dotPercent;

        proc.GetComponent<Text>().text = "";       
        
    }  

    // Update is called once per frame
    void Update()
    {
        //Positive-Negative
        //___.text = Moving generates a charge. When charged at least halfway, hitting an Enemy applies damage-over-time for ten seconds, inflicting 100% of Weapon damage once every second.     

        //Weapon' bullet trail changes based on current charge
        if (chargePercentage < 50f)
        {
            firearm.bulletTrail = originalBullet;
        }

        else
        {
            firearm.bulletTrail = electricBullet;
        }

        if (chargePercentage > 0)
        {
            proc.GetComponent<Text>().text = "+/-: " + chargePercentage.ToString("F0") + "%";
        }

        else
        {
            proc.GetComponent<Text>().text = " ";
        }

        //Movement input generates charge, up to 100%. 
        if(move.horizInput != 0 || move.vertInput != 0)
        {
            chargePercentage += Time.deltaTime * chargeAccelerant;
                      
            if (chargePercentage >= 50f && Time.timeScale == 1)
            {
                GameObject effect = Instantiate(activation, gameObject.transform.root.gameObject.transform.position + Vector3.down, transform.rotation, gameObject.transform.root);
            }

            if (chargePercentage >= 100f)
            {
                chargePercentage = 100f;
            }
        }

        //Lack of movement loses charge, down to 0%
        else
        {
            if(firearm.isExotic == true)
            {
                chargePercentage -= Time.deltaTime * chargeAccelerant / 5;
            }

            else
            {
                chargePercentage -= Time.deltaTime * chargeAccelerant;
            }

            if(chargePercentage <= 0)
            {
                chargePercentage = 0f;
            }
        }

        //Confirmed target hits with half-charge applies damage-over-time
        if(firearm.targetHit)
        {
            if(chargePercentage < 50f)
            {
                firearm.targetHit = null;
            }

            else
            {
                if (firearm.targetHit.gameObject.GetComponent<PosNegDOT>() == null)
                {
                    firearm.targetHit.gameObject.AddComponent<PosNegDOT>();

                    if (firearm.weaponRarity == 5 && !firearm.isExotic)
                    {
                        firearm.targetHit.gameObject.GetComponent<PosNegDOT>().dotRarity = 5;
                    }

                    firearm.targetHit.gameObject.GetComponent<PosNegDOT>().dotDamage = dotStrength;                   
                    firearm.targetHit = null;
                }

                else
                {
                    firearm.targetHit = null;
                }
            }          
        }
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }

        chargePercentage = 0f;
    }  
}
