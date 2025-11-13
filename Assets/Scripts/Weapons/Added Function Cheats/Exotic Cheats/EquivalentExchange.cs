using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquivalentExchange : MonoBehaviour
{
    private float damageReturnPercent = 35f; //% of damage converted to Weapon damage
    private float dmgRtnPctReset; //Holds starting damage % conversion
    private float healthReturnPercent = 35f; //% of damage converted to Player Health
    private float hthRtnPctReset; //Holds starting health % conversion
    private float percentCap = 150f; //% of maximum Weapon damage allowed
    private int damageSteal; //Number used to increase Weapon damage
    private int healthSteal; //Number used to increase Player Health
    private int damageRoof; //Number used to limit Weapon damage

    private FirearmScript firearm;
    internal GameObject proc; //Text UI that records Cheat activity
    private PlayerStatusScript player;
    private GameObject activation; //VFX used to convey activity
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

        if (player.playerHit == true && firearm.enabled == true)
        {   
            //Converts received damage to Weapon damage and Health
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

            var main = activation.GetComponent<ParticleSystem>().main;
            main.startColor = color;

            Instantiate(activation, gameObject.transform.root.gameObject.transform.position, transform.rotation);
        }
    }

    /// <summary>
    /// Converts damaged received to Weapon damage
    /// </summary>
    void DamageConversion()
    {
        damageReturnPercent /= 100;
        damageReturnPercent *= player.dmgReceived;
        damageSteal = (int)damageReturnPercent;
        damageReturnPercent = dmgRtnPctReset;
    }

    /// <summary>
    /// Converts damage received to Health
    /// </summary>
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
        proc.GetComponent<Text>().text = "";
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }
    }
}
