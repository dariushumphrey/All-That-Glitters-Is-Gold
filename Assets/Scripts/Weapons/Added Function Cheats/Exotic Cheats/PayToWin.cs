using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayToWin : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc;
    private PlayerInventoryScript player;
    private GameObject activation;
    private Color color = Color.cyan;

    private float damageBuff = 50f;
    private float reloadReset;
    private int damageAdd;
    private int stackNum;
    private int dmgIncrease;
    private int dmgReset;
    internal bool hitConfirmed = false;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        proc.GetComponent<Text>().text = " ";
        player = FindObjectOfType<PlayerInventoryScript>();
        activation = Resources.Load<GameObject>("Particles/cheatProcEffect");

        reloadReset = firearm.reloadSpeed;

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
        //___.text = 'Space' - Consume 10,000 Lucent currency to create 150 stacks of a 50% base damage increase. Activating the conversion on an empty magazine instantly reloads the weapon. Enemy hits removes three stacks.

        if(stackNum >= 1 && firearm.enabled == true)
        {
            proc.GetComponent<Text>().text = "Pay to Win x" + stackNum;
        }

        else
        {
            proc.GetComponent<Text>().text = " ";
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

        if(hitConfirmed == true)
        {
           
            if (stackNum >= 1)
            {
                stackNum -= 3;
                hitConfirmed = false;            
            }
        }
    }

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
                if(firearm.currentAmmo <= 0)
                {
                    //firearm.reloadSpeed = 0.0f;
                    //firearm.ReloadWeapon();
                }

                activation.GetComponent<ParticleSystem>().startColor = color;
                Instantiate(activation, gameObject.transform.root.gameObject.transform.position, transform.rotation);

                //firearm.reloadSpeed = reloadReset;
            }
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
