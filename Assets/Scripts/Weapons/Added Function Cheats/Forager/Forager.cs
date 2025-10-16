using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Forager : MonoBehaviour
{
    private float healthPercent = 1f;
    private int healthAssign;

    private float shieldPercent = 2f;
    private int shieldAssign;

    private float ammoPercent = 15f;
    private float magOverflowPercent = 150f;
    private int ammoAssign;
    private int overflowAssign;

    private int burstCount = 10;
    private GameObject healthPickup;
    private GameObject shieldPickup;
    private GameObject ammoPickup;
    private GameObject lucentCluster;
    private List<GameObject> burst = new List<GameObject>();

    private int shotMaximum = 10;
    private int shots = 0;
    private bool done = false;
    private FirearmScript firearm;
    private PlayerStatusScript status;
    private PlayerMeleeScript melee;
    internal GameObject proc;
    internal bool killConfirmed = false;
    internal bool hitConfirmed = false;
    internal Vector3 burstPosition;

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        status = FindObjectOfType<PlayerStatusScript>();
        melee = FindObjectOfType<PlayerMeleeScript>();

        if (firearm.weaponRarity == 5)
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

        melee.foragerCheat = gameObject;

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
        proc.GetComponent<Text>().text = " ";
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = " ";
            melee.foragerCheat = null;
        }
    }

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
