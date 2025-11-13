using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Forager : MonoBehaviour
{
    private float healthPercent = 1f; //% of Player Health
    private int healthAssign; //Number used to increase Player Health

    private float shieldPercent = 2f; //% of Player Shield
    private int shieldAssign; //Number used to increase Player Shield

    private float ammoPercent = 15f; //% of Weapon magazine size
    private float magOverflowPercent = 150f; //% of Weapon magazine beyond its original size
    private int ammoAssign; //Number used to increase current Weapon magazine
    private int overflowAssign; //Number used to overflow Weapon magazine size

    private int burstCount = 10; //Number of pickups in a burst
    private GameObject healthPickup;
    private GameObject shieldPickup;
    private GameObject ammoPickup;
    private GameObject lucentCluster;
    private List<GameObject> burst = new List<GameObject>(); //List of pickups

    private int shotMaximum = 10; //Goal number of confirmed hits
    private int shots = 0; //Total number of confirmed hits
    private bool done = false; //Allows one operation if true;
    private FirearmScript firearm;
    private PlayerStatusScript status;
    private PlayerMeleeScript melee;
    internal GameObject proc; //Text UI that records Cheat activity
    internal bool killConfirmed = false; //Affirms achieved kill if true
    internal bool hitConfirmed = false; //Affirms achieved hit if true
    internal Vector3 burstPosition; //Spawn position of burst

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        status = FindObjectOfType<PlayerStatusScript>();
        melee = FindObjectOfType<PlayerMeleeScript>();

        //Non-exotic Rarity 5 Weapons double the percentages, number of total pickups
        if (firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            healthPercent *= 2;
            shieldPercent *= 2;
            ammoPercent *= 2;
            burstCount *= 2;
        }

        healthPercent /= 100;
        healthPercent *= status.playerHealthMax;
        healthAssign = (int)healthPercent;

        shieldPercent /= 100;
        shieldPercent *= status.playerShieldMax;
        shieldAssign = (int)shieldPercent;

        ammoPercent /= 100;
        ammoPercent *= firearm.ammoSize;
        ammoAssign = (int)ammoPercent;

        magOverflowPercent /= 100;
        magOverflowPercent *= firearm.ammoSize;
        overflowAssign = (int)magOverflowPercent;


        healthPickup = Resources.Load<GameObject>("Game Items/ForagerHealth");
        shieldPickup = Resources.Load<GameObject>("Game Items/ForagerShield");
        ammoPickup = Resources.Load<GameObject>("Game Items/ForagerAmmo");
        lucentCluster = Resources.Load<GameObject>("Game Items/testLucent");

        burst.Add(healthPickup);
        burst.Add(shieldPickup);
        burst.Add(ammoPickup);
        burst.Add(lucentCluster);

        proc.GetComponent<Text>().text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        //Forager
        //____.text = "Weapon or Melee kills produce a burst of Lucent clusters, Health, Shield, and Ammo pickups."

        //Permits use of Forager's melee effects while Weapon is active
        melee.foragerCheat = gameObject;

        //Creates a pickup burst at the goal number
        if(firearm.weaponRarity == 5)
        {
            if (hitConfirmed == true)
            {
                shots++;
                if (shots >= shotMaximum)
                {
                    if(!done)
                    {
                        ForagerBurst();
                        done = true;
                    }

                    shots = 0;
                }

                hitConfirmed = false;
                done = false;
            }
        }

        //Produces a burst of pickups on Enemy defeats
        if (killConfirmed)
        {
            for(int b = 0; b <= burstCount; b++)
            {
                int index = Random.Range(0, 4);

                GameObject pickup = Instantiate(burst[index], burstPosition, Quaternion.Euler(new Vector3(Random.Range(0f, 360f), 0f, 0f)));
                pickup.name = burst[index].name;

                if (pickup.GetComponent<ForagerHealthScript>())
                {
                    pickup.GetComponent<ForagerHealthScript>().healthAdd = healthAssign;
                }

                if (pickup.GetComponent<ForagerShieldScript>())
                {
                    pickup.GetComponent<ForagerShieldScript>().shieldAdd = shieldAssign;
                }

                if (pickup.GetComponent<ForagerAmmoScript>())
                {
                    pickup.GetComponent<ForagerAmmoScript>().ammoAdd = ammoAssign;
                    pickup.GetComponent<ForagerAmmoScript>().overflowCap = overflowAssign;
                }

                if (pickup.GetComponent<LucentScript>())
                {
                    pickup.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                    pickup.GetComponent<LucentScript>().ShatterCalculation();
                    pickup.GetComponent<LucentScript>().shatterDelayTime = 0.25f;
                    pickup.GetComponent<LucentScript>().StartCoroutine(pickup.GetComponent<LucentScript>().Shatter());
                }

                //pickup.GetComponent<Rigidbody>().AddForce(pickup.transform.forward, ForceMode.Impulse);
            }

            proc.GetComponent<Text>().text = "Forager";
            StartCoroutine(ClearText());

            killConfirmed = false;
        }
    }

    IEnumerator ClearText()
    {
        yield return new WaitForSeconds(1f);
        proc.GetComponent<Text>().text = "";
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
            melee.foragerCheat = null;
        }
    }

    /// <summary>
    /// Creates a pickup burst
    /// </summary>
    public void ForagerBurst()
    {
        for (int b = 0; b <= burstCount; b++)
        {
            int index = Random.Range(0, 4);

            GameObject pickup = Instantiate(burst[index], burstPosition, Quaternion.Euler(new Vector3(Random.Range(0f, 360f), 0f, 0f)));
            pickup.name = burst[index].name;

            if (pickup.GetComponent<ForagerHealthScript>())
            {
                pickup.GetComponent<ForagerHealthScript>().healthAdd = healthAssign;
            }

            if (pickup.GetComponent<ForagerShieldScript>())
            {
                pickup.GetComponent<ForagerShieldScript>().shieldAdd = shieldAssign;
            }

            if (pickup.GetComponent<ForagerAmmoScript>())
            {
                pickup.GetComponent<ForagerAmmoScript>().ammoAdd = ammoAssign;
                pickup.GetComponent<ForagerAmmoScript>().overflowCap = overflowAssign;
            }

            if (pickup.GetComponent<LucentScript>())
            {
                pickup.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                pickup.GetComponent<LucentScript>().ShatterCalculation();
                pickup.GetComponent<LucentScript>().shatterDelayTime = 0.25f;
                pickup.GetComponent<LucentScript>().StartCoroutine(pickup.GetComponent<LucentScript>().Shatter());
            }

            //pickup.GetComponent<Rigidbody>().AddForce(pickup.transform.forward, ForceMode.Impulse);
        }

        proc.GetComponent<Text>().text = "Forager";
        StartCoroutine(ClearText());
    }
}
