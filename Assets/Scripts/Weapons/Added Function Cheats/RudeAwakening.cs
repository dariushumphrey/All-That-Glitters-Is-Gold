using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RudeAwakening : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerStatusScript player;
    internal GameObject proc;

    private int waveStacks = 0;
    private int waveStacksMax;

    private float dmgPct = 1000f;
    private float dmgIncrease = 20f;
    private int waveDamage;
    private int dmgNew;
    private int dmgReset;
    internal bool killConfirmed;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = firearm.GetComponentInParent<PlayerStatusScript>();
        proc.GetComponent<Text>().text = " ";

        if(firearm.weaponRarity == 5)
        {
            waveStacksMax = 6;
        }

        else
        {
            waveStacksMax = 3;
        }

        dmgPct /= 100;
        dmgPct *= firearm.damage;
        waveDamage = (int)dmgPct;

        if(firearm.weaponRarity == 5)
        {
            dmgReset = firearm.damage;
            dmgIncrease /= 100;
            dmgIncrease *= firearm.damage;
            dmgNew = firearm.damage + (int)dmgIncrease;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Rude Awakening
        //___.text = "Kills build stacks of a heavy-damage AOE blast. [Space] - Cast Blast"  

        if(killConfirmed)
        {
            if(firearm.weaponRarity == 5)
            {
                waveStacks += 2;
            }

            else
            {
                waveStacks++;
            }

            if(waveStacks >= waveStacksMax)
            {
                waveStacks = waveStacksMax;
            }

            killConfirmed = false;
        }

        if(firearm.weaponRarity == 5)
        {
            if(waveStacks != 0)
            {
                firearm.damage = dmgNew;
            }

            else
            {
                firearm.damage = dmgReset;
            }
        }

        if (waveStacks != 0)
        {
            proc.GetComponent<Text>().text = "Rude Awakening x" + waveStacks;
        }

        else
        {
            proc.GetComponent<Text>().text = " ";
        }       

        if(Input.GetKeyDown(KeyCode.Space) && waveStacks != 0)
        {
            waveStacks--;
            if(waveStacks <= 0)
            {
                waveStacks = 0;
            }

            Vector3 epicenter = transform.position;
            Collider[] affected = Physics.OverlapSphere(transform.position, 7.5f);
            foreach (Collider hit in affected)
            {
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    if (hit.GetComponent<EnemyHealthScript>() != null)
                    {
                        hit.GetComponent<EnemyHealthScript>().inflictDamage(waveDamage);
                        //if (hit.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                        //{
                        //    if(hit.GetComponent<Rigidbody>() != null)
                        //    {
                        //        hit.GetComponent<Rigidbody>().AddExplosionForce(30f, epicenter, 7.5f, 40.0f, ForceMode.Impulse);
                        //    }

                        //    else
                        //    {
                        //        hit.gameObject.AddComponent<Rigidbody>();
                        //        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                        //        gameObject.GetComponent<Rigidbody>().freezeRotation = false;

                        //        hit.GetComponent<Rigidbody>().AddExplosionForce(30f, epicenter, 7.5f, 40.0f, ForceMode.Impulse);
                        //    }
                        //}
                    }
                }

                //Rigidbody inflict = hit.GetComponent<Rigidbody>();
                //if (inflict != null)
                //{
                //    if (inflict.GetComponent<EnemyHealthScript>() != null)
                //    {
                //        inflict.GetComponent<EnemyHealthScript>().inflictDamage(waveDamage);
                //        if (inflict.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                //        {
                //            inflict.AddExplosionForce(30f, epicenter, 7.5f, 40.0f, ForceMode.Impulse);
                //        }
                //    }
                //}
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
