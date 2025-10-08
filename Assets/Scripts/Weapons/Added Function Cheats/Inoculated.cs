using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inoculated : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerStatusScript player;
    private EnemyManagerScript enemy;
    private ParticleSystem activation;
    private Color color = Color.green;
    internal GameObject proc;
    private float healthPercent = 5f;
    private int healthGain;
    internal bool killConfirmed = false;

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        enemy = FindObjectOfType<EnemyManagerScript>();
        proc.GetComponent<Text>().text = " ";
        activation = Resources.Load<ParticleSystem>("Particles/cheatProcEffect");

        player = FindObjectOfType<PlayerStatusScript>();

        if (firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            healthPercent = 10f;
            healthPercent /= 100;
            healthPercent *= player.playerHealthMax;
            healthGain = (int)healthPercent;
        }

        else
        {
            healthPercent /= 100;
            healthPercent *= player.playerHealthMax;
            healthGain = (int)healthPercent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(firearm.enabled == true)
        {
            if (killConfirmed == true)
            {
                if (player.playerHealth >= player.playerHealthMax)
                {
                    player.playerHealth = player.playerHealthMax;
                    //return;
                }

                else
                {
                    player.playerHealth += healthGain;
                    if (player.playerHealth >= player.playerHealthMax)
                    {
                        player.playerHealth = player.playerHealthMax;
                        //return;
                    }

                    proc.GetComponent<Text>().text = "Inoculated";
                    StartCoroutine(ClearText());
                    killConfirmed = false;

                    activation.GetComponent<ParticleSystem>().startColor = color;
                    Instantiate(activation, gameObject.transform.root.gameObject.transform.position, transform.rotation);
                    //enemy.StartCoroutine(enemy.EnemyDiedReset());
                    return;
                }
            }
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
        }
    }
}
