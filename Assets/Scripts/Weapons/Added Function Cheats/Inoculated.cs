using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inoculated : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerStatusScript player;
    private EnemyManagerScript enemy;
    private ParticleSystem activation; //VFX used to convey activity
    private Color color = Color.green;
    internal GameObject proc; //Text UI that records Cheat activity
    private float healthPercent = 5f; //% of Player Max Health
    private int healthGain; //Number used to add onto current Health
    internal bool killConfirmed = false; //Affirms achieved kill if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        enemy = FindObjectOfType<EnemyManagerScript>();
        proc.GetComponent<Text>().text = " ";
        activation = Resources.Load<ParticleSystem>("Particles/cheatProcEffect");
        player = FindObjectOfType<PlayerStatusScript>();

        //Non-exotic Rarity 5 Weapons increase amount of Health returned
        if (firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            healthPercent = 10f;
        }

        healthPercent /= 100;
        healthPercent *= player.playerHealthMax;
        healthGain = (int)healthPercent;

        proc.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(firearm.enabled == true)
        {
            //Confirmed kills adds % of max Health onto current health
            if (killConfirmed == true)
            {
                if (player.playerHealth >= player.playerHealthMax)
                {
                    player.playerHealth = player.playerHealthMax;
                }

                else
                {
                    player.playerHealth += healthGain;
                    if (player.playerHealth >= player.playerHealthMax)
                    {
                        player.playerHealth = player.playerHealthMax;
                    }

                    proc.GetComponent<Text>().text = "Inoculated";
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
