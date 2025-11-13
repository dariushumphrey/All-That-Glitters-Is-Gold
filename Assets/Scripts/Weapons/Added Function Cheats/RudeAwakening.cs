using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RudeAwakening : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerStatusScript player;
    private GameObject activation; //VFX used to convey activity
    internal GameObject proc; //Text UI that records Cheat activity

    private int waveStacks = 0; //current count of stacks
    private int waveStacksMax = 3; //maximum stacks allowed

    private float dmgPct = 1000f; //% of Weapon damage used for waveDamage
    private float dmgIncrease = 20f; //% of Weapon damage -- Rarity 5 only
    private int waveDamage; //Damage caused by AOE wave
    private int dmgNew; //Number used to cap Weapon damage -- Rarity 5 only
    private int dmgReset; //Hold starting damage
    internal bool killConfirmed; //Affirms achieved kill if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = firearm.GetComponentInParent<PlayerStatusScript>();
        activation = Resources.Load<GameObject>("Particles/RudeAwakeningActive");
        proc.GetComponent<Text>().text = " ";

        //Rarity 5 Weapons increase max stacks and calculates a fixed Weapon damage amount
        if(firearm.weaponRarity == 5)
        {
            waveStacksMax = 6;

            dmgReset = firearm.damage;
            dmgIncrease /= 100;
            dmgIncrease *= firearm.damage;
            dmgNew = firearm.damage + (int)dmgIncrease;
        }

        dmgPct /= 100;
        dmgPct *= firearm.damage;
        waveDamage = (int)dmgPct;
    }

    // Update is called once per frame
    void Update()
    {
        //Rude Awakening
        //___.text = "Kills build stacks of a heavy-damage AOE blast. [Space] - Cast Blast"  

        //Confirmed kills grant stacks for AOE wave
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

        //Having at least one stack increases Weapon damage
        //Damage is restored to normal otherwise
        if(firearm.weaponRarity == 5)
        {
            if(waveStacks >= 1)
            {
                firearm.damage = dmgNew;
            }

            else
            {
                firearm.damage = dmgReset;
            }
        }

        if (waveStacks >= 1)
        {
            proc.GetComponent<Text>().text = "Rude Awakening x" + waveStacks;
        }

        else
        {
            proc.GetComponent<Text>().text = "";
        }       

        //Pressing 'E' casts an AOE wave if Player has at least one stack
        if(Input.GetKeyDown(KeyCode.E) && waveStacks >= 1)
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
                        if (hit.GetComponent<EnemyHealthScript>().healthCurrent <= 0 && hit.GetComponent<Rigidbody>() == null)
                        {
                            hit.gameObject.AddComponent<Rigidbody>();
                            hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
                        }
                    }
                }

                if (hit.gameObject.CompareTag("Lucent"))
                {
                    hit.gameObject.GetComponent<LucentScript>().lucentGift = 0;
                    hit.gameObject.GetComponent<LucentScript>().shot = true;
                }
            }

            GameObject effect = Instantiate(activation, gameObject.transform.root.gameObject.transform.position, Quaternion.identity);

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
