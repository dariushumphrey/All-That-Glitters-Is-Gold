using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaliciousWindUp : MonoBehaviour
{
    private FirearmScript firearm;
    private EnemyManagerScript enemy;
    private ParticleSystem activation; //VFX used to convey activity
    private Color color = Color.white;
    internal GameObject proc; //Text UI that records Cheat activity
    private float decreasePercent = 0.75f; //% of Reload Speed
    private float reserveRestore = 5f; //% of Weapon' max reserves
    private bool done = false; //Affirms completed action if true

    private float reloadReset; //Holds starting Reload Speed
    private int reserveAdd; //Number used to increase Weapon' max reserves
    internal bool killConfirmed = false; //Affirms achieved kill if true
    internal bool hitConfirmed = false; //Affirms achieved hit if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        enemy = FindObjectOfType<EnemyManagerScript>();
        activation = Resources.Load<ParticleSystem>("Particles/cheatProcEffect");

        reloadReset = firearm.reloadSpeed;

        //Non-exotic Rarity 5 Weapons increase % of Reload Speed and calculate % of Reserves
        if (firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            decreasePercent = 1.5f;

            reserveRestore /= 100;
            reserveRestore *= firearm.reserveSize;
            reserveAdd = (int)reserveRestore;
        }

        decreasePercent /= 100;
        decreasePercent *= firearm.reloadSpeed;


        proc.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        //Malicious Wind-Up
        //___.text = "Inflicting Damage increases Reload Speed by 0.75%. This bonus activates on your next reload. 

        //Confirmed hits increase Reload Speed
        if(hitConfirmed == true && firearm.enabled == true)
        {
            firearm.reloadSpeed -= decreasePercent;

            proc.GetComponent<Text>().text = "Malicious Wind-Up Ready";
            hitConfirmed = false;
        }

        //Reloads restore Reload Speed to default
        if (firearm.isReloading == true && firearm.enabled == true)
        {
            if(firearm.reloadSpeed < reloadReset)
            {
                proc.GetComponent<Text>().text = "Malicious Wind Up";      
                
                if(!done)
                {
                    var main = activation.GetComponent<ParticleSystem>().main;
                    main.startColor = color;

                    Instantiate(activation, gameObject.transform.root.gameObject.transform.position, transform.rotation);
                    done = true;
                }

                StartCoroutine(ResetReload());
            }

            else
            {
                proc.GetComponent<Text>().text = "";

            }
        }

        if(firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            //Confirmed kills adds % of max Reserves onto current Reserves
            if(killConfirmed == true)
            {
                firearm.reserveAmmo += reserveAdd;
                if(firearm.reserveAmmo >= firearm.reserveSize)
                {
                    firearm.reserveAmmo = firearm.reserveSize;
                }

                killConfirmed = false;
            }
        }
    }

    IEnumerator ResetReload()
    {
        yield return new WaitForSeconds(firearm.reloadSpeed);
        proc.GetComponent<Text>().text = "";
        done = false;
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }
    }
}
