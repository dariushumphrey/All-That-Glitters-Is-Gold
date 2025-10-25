using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbsolutelyNoStops : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc;
    private GameObject activation, secondTry;

    private bool tick;
    private int dmgIncrease;
    private float rofPercent = 50f;
    private float rofReset;
    private int dmgReset;
    private float rldReset;
    private bool tickedAlready;
    private bool capOne, capTwo;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        proc.GetComponent<Text>().text = " ";
        activation = Resources.Load<GameObject>("Particles/AbsolutelyNoBreaksActive");

        tick = false;

        dmgReset = firearm.damage;
        dmgIncrease = firearm.damage * 3;

        rofReset = firearm.fireRate;
        rofPercent /= 100;
        rofPercent *= firearm.fireRate;

        rldReset = firearm.reloadSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        //Absolutely No Stops
        //___.text = Expending your magazine automatically fills it from reserves, amplifies damage by 200%, and reduces Recoil and increases Rate of Fire by 50%. 
        //This bonus ends when ammo reserves are depleted or if you stop firing.

        if(tick && Time.timeScale == 1)
        {
            GameObject secondTry = Instantiate(activation, gameObject.transform.position, transform.rotation, gameObject.transform);
        }

        if (firearm.currentAmmo <= 0 && firearm.reserveAmmo > 0)
        {
            if (tick)
            {
                firearm.ReloadWeapon();
                //return;
            }

            else
            {
                tick = true;
                firearm.damage = dmgIncrease;
                firearm.reloadSpeed = 0.0f;
                firearm.ReloadWeapon();
                firearm.fireRate = rofPercent;
                proc.GetComponent<Text>().text = "Absolutely No Stops";

            }
        }

        if (firearm.reserveAmmo <= 0 || Input.GetButtonUp("Fire1") && tick)
        {
            tick = false;
            firearm.damage = dmgReset;
            firearm.reloadSpeed = rldReset;
            firearm.fireRate = rofReset;
            proc.GetComponent<Text>().text = " ";
        }

        if (tick && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reload canceled");
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
