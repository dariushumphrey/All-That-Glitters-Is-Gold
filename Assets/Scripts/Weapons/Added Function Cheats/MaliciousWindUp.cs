using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaliciousWindUp : MonoBehaviour
{
    private FirearmScript firearm;
    private EnemyManagerScript enemy;
    private ParticleSystem activation;
    private Color color = Color.white;
    internal GameObject proc;
    private float decreasePercent = 0.75f;
    private float fatedDecreasePercent = 1.5f;
    private float reserveRestore = 5f;
    private bool done = false;

    private float reloadReset;
    private int reserveAdd;
    internal bool killConfirmed = false;
    internal bool hitConfirmed = false;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        enemy = FindObjectOfType<EnemyManagerScript>();
        activation = Resources.Load<ParticleSystem>("Particles/cheatProcEffect");

        reloadReset = firearm.reloadSpeed;

        decreasePercent /= 100;
        decreasePercent *= firearm.reloadSpeed;

        fatedDecreasePercent /= 100;
        fatedDecreasePercent *= firearm.reloadSpeed;

        reserveRestore /= 100;
        reserveRestore *= firearm.reserveSize;
        reserveAdd = (int)reserveRestore;

        proc.GetComponent<Text>().text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        //Malicious Wind-Up
        //___.text = "Inflicting Damage increases Reload Speed by 0.25%. This bonus activates on your next reload. 

        if(hitConfirmed == true && firearm.enabled == true)
        {
            if(firearm.weaponRarity == 5 && !firearm.isExotic)
            {
                firearm.reloadSpeed -= fatedDecreasePercent;
            }

            else
            {
                firearm.reloadSpeed -= decreasePercent;
            }

            proc.GetComponent<Text>().text = "Malicious Wind-Up Ready";
            hitConfirmed = false;
        }

        if (firearm.isReloading == true && firearm.enabled == true)
        {
            if(firearm.reloadSpeed < reloadReset)
            {
                proc.GetComponent<Text>().text = "Malicious Wind Up";      
                
                if(!done)
                {
                    activation.GetComponent<ParticleSystem>().startColor = color;
                    Instantiate(activation, gameObject.transform.root.gameObject.transform.position, transform.rotation);
                    done = true;
                }

                StartCoroutine(ResetReload());
            }

            else
            {
                proc.GetComponent<Text>().text = " ";

            }
        }

        if(firearm.weaponRarity == 5 && !firearm.isExotic)
        {
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
        proc.GetComponent<Text>().text = " ";
        done = false;
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = " ";
        }
    }
}
