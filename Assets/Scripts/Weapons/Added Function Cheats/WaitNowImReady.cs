using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitNowImReady : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerStatusScript player;
    private EnemyManagerScript enemy;
    private ParticleSystem activation;
    private Color color = Color.yellow;
    internal GameObject proc;
    private float shieldPercent = 10f;
    private int shieldGain;
    internal bool killConfirmed = false;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        enemy = FindObjectOfType<EnemyManagerScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        activation = Resources.Load<ParticleSystem>("Particles/cheatProcEffect");

        if(firearm.weaponRarity == 5 && !firearm.isExotic)
        {
            shieldPercent = 20f;
            shieldPercent /= 100;
            shieldPercent *= player.playerShieldMax;
            shieldGain = (int)shieldPercent;
        }

        else
        {
            shieldPercent /= 100;
            shieldPercent *= player.playerShieldMax;
            shieldGain = (int)shieldPercent;
        }

        proc.GetComponent<Text>().text = " ";
    }

    // Update is called once per frame
    void Update()
    {

        if (firearm.enabled == true)
        {
            if (killConfirmed == true)
            {
                if (player.playerShield >= player.playerShieldMax)
                {
                    player.playerShield = player.playerShieldMax;
                    //return;
                }

                else
                {
                    player.playerShield += shieldGain;
                    if (player.playerShield >= player.playerShieldMax)
                    {
                        player.playerShield = player.playerShieldMax;
                        //return;
                    }

                    proc.GetComponent<Text>().text = "Wait! Now, I'm Ready!";
                    StartCoroutine(ClearText());
                    killConfirmed = false;
                    //enemy.StartCoroutine(enemy.EnemyDiedReset());

                    activation.GetComponent<ParticleSystem>().startColor = color;
                    Instantiate(activation, gameObject.transform.root.gameObject.transform.position, transform.rotation);
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
