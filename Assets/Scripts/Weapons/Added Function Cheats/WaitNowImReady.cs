using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitNowImReady : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerStatusScript player;
    private EnemyManagerScript enemy;
    private ParticleSystem activation; //VFX used to convey activity
    private Color color = Color.yellow;
    internal GameObject proc; //Text UI that records Cheat activity
    private float shieldPercent = 10f; //Percent of Shield
    private int shieldGain; //Used to add onto current Shield
    internal bool killConfirmed = false; //Affirms achieved kill if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        enemy = FindObjectOfType<EnemyManagerScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        activation = Resources.Load<ParticleSystem>("Particles/cheatProcEffect");

        //Non-exotic Rarity 5 Weapons increase strength of Shield returned
        if(firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            shieldPercent = 20f;
        }

        shieldPercent /= 100;
        shieldPercent *= player.playerShieldMax;
        shieldGain = (int)shieldPercent;

        proc.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (firearm.enabled == true)
        {
            //Confirmed kills adds % of max Shield onto current Shield
            if (killConfirmed == true)
            {
                if (player.playerShield >= player.playerShieldMax)
                {
                    player.playerShield = player.playerShieldMax;
                }

                else
                {
                    player.playerShield += shieldGain;
                    if (player.playerShield >= player.playerShieldMax)
                    {
                        player.playerShield = player.playerShieldMax;
                    }

                    proc.GetComponent<Text>().text = "Wait! Now, I'm Ready!";
                    StartCoroutine(ClearText());

                    var main = activation.GetComponent<ParticleSystem>().main;
                    main.startColor = color;

                    Instantiate(activation, gameObject.transform.root.gameObject.transform.position, transform.rotation);
                }

                killConfirmed = false;
            }
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
        }
    }
}