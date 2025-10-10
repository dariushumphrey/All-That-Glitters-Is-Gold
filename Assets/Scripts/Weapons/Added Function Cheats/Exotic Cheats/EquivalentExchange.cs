using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquivalentExchange : MonoBehaviour
{
    private float damageReturnPercent = 35f;
    private float dmgRtnPctReset;
    private float healthReturnPercent = 35f;
    private float hthRtnPctReset;
    private float percentCap = 150f;
    private int damageSteal;
    private int healthSteal;
    private int damageRoof;

    private FirearmScript firearm;
    internal GameObject proc;
    private PlayerStatusScript player;
    private GameObject activation;
    private Color color = Color.red;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        proc.GetComponent<Text>().text = " ";
        player = FindObjectOfType<PlayerStatusScript>();
        activation = Resources.Load<GameObject>("Particles/cheatProcEffect");

        percentCap /= 100;
        percentCap *= firearm.damage;
        damageRoof = (int)percentCap + firearm.damage;

        dmgRtnPctReset = damageReturnPercent;
        hthRtnPctReset = healthReturnPercent;

    }

    // Update is called once per frame
    void Update()
    {
        //Equivalent Exchange
        //___.text = If you are attacked, 35% of damage received is added to this weapon's base damage. Base damage can increase up to 150%. 35% of damage received is returned as health. 

        if (firearm.damage >= damageRoof)
        {
            firearm.damage = damageRoof;
        }

        if (player.playerHit == true && firearm.enabled == true)
        {
            //Debug.Log(healthSteal);
            DamageConversion();
            HealthConversion();            
 
            if (firearm.damage >= damageRoof)
            {
                firearm.damage = damageRoof;
            }

            else
            {
                firearm.damage += damageSteal;
            }

            if (player.playerHealth >= player.playerHealthMax)
            {
                player.playerHealth = player.playerHealthMax;
            }

            else
            {
                player.playerHealth += healthSteal;
            }

            proc.GetComponent<Text>().text = "Equivalent Exchange";
            StartCoroutine(DeconfirmProc());
            player.playerHit = false;

            activation.GetComponent<ParticleSystem>().startColor = color;
            Instantiate(activation, gameObject.transform.root.gameObject.transform.position, transform.rotation);
        }
    }

    void DamageConversion()
    {
        damageReturnPercent /= 100;
        damageReturnPercent *= player.dmgReceived;
        damageSteal = (int)damageReturnPercent;
        damageReturnPercent = dmgRtnPctReset;
    }

    void HealthConversion()
    {
        healthReturnPercent /= 100;
        healthReturnPercent *= player.dmgReceived;
        healthSteal = (int)healthReturnPercent;
        healthReturnPercent = hthRtnPctReset;
    }

    IEnumerator DeconfirmProc()
    {
        yield return new WaitForSeconds(1f);
        proc.GetComponent<Text>().text = " ";
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = " ";
        }
    }
}
