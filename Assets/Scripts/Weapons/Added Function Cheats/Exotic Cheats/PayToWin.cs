using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayToWin : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc; //Text UI that records Cheat activity
    private PlayerInventoryScript player;
    private GameObject activation; //VFX used to convey activity
    private Color color = Color.cyan;

    private float damageBuff = 50f; //% of Weapon damage
    private int damageAdd; //Number used to increase Weapon damage
    private int stackNum; //Number of current Stacks
    private int dmgIncrease; //Fixed Weapon damage number
    private int dmgReset; //Holds starting Weapon damage
    internal bool hitConfirmed = false; //Affirms achieved hit if true

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        proc.GetComponent<Text>().text = " ";
        player = FindObjectOfType<PlayerInventoryScript>();
        activation = Resources.Load<GameObject>("Particles/cheatProcEffect");

        damageBuff /= 100;
        damageBuff *= firearm.damage;
        damageAdd = (int)damageBuff;

        stackNum = 0;
        dmgReset = firearm.damage;
        dmgIncrease = firearm.damage + damageAdd;
    }

    // Update is called once per frame
    void Update()
    {
        //Pay to Win
        //___.text = 'Space' - Consume 10,000 Lucent currency to create 150 stacks of a 50% base damage increase. Enemy hits removes three stacks.

        if(stackNum >= 1 && firearm.enabled == true)
        {
            proc.GetComponent<Text>().text = "Pay to Win x" + stackNum;
        }

        else
        {
            proc.GetComponent<Text>().text = "";
        }

        if(stackNum <= 0)
        {
            firearm.damage = dmgReset;
        }

        else
        {
            firearm.damage = dmgIncrease;
        }

        StackGranter();

        //Confirmed hits subtract three stacks
        if(hitConfirmed == true)
        {      
            if (stackNum >= 1)
            {
                stackNum -= 3;
                hitConfirmed = false;            
            }
        }
    }

    /// <summary>
    /// Controls stack acquisition
    /// </summary>
    void StackGranter()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (stackNum >= 1)
            {
                //Having any stacks prevents player from adding more. Consume all stacks to use again.
                return;
            }

            if (stackNum >= 150)
            {
                //If somehow stackNum goes beyond 150, assign it 150 exactly. 
                stackNum = 150;
                return;
            }

            else if (player.lucentFunds < 5280)
            {
                //If the Player has less than 5,280 Lucent funds, they cannot create stacks. 
                return;
            }

            else
            {
                player.lucentFunds -= 5280;              
                stackNum = 150;

                var main = activation.GetComponent<ParticleSystem>().main;
                main.startColor = color;

                Instantiate(activation, gameObject.transform.root.gameObject.transform.position, transform.rotation);

            }
        }
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }
    }
}
