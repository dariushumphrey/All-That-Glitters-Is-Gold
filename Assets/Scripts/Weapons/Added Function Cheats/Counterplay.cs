using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counterplay : MonoBehaviour
{
    private float dmgIncreasePercent = 10f; //% to increase Weapon damage
    private int dmgAdd; //Number used to increase Weapon damage
    private int stackCountActive; //Current number of stacks
    private int stackCountCap = 3; //Maximum allowed stacks
    private FirearmScript firearm;
    private PlayerStatusScript status;
    private PlayerInventoryScript inventory;
    private PlayerMoveScript move;
    private GameObject lucentCluster;
    internal GameObject proc; //Text UI that records Cheat activity

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        status = FindObjectOfType<PlayerStatusScript>();
        move = FindObjectOfType<PlayerMoveScript>();
        inventory = FindObjectOfType<PlayerInventoryScript>();
        lucentCluster = Resources.Load<GameObject>("Game Items/testLucent");
        proc.GetComponent<Text>().text = " ";

        dmgIncreasePercent /= 100;
        dmgIncreasePercent *= firearm.damage;
        dmgAdd = (int)dmgIncreasePercent;

        //Non-exotic Rarity 5 Weapons increase total stack maximum
        if(firearm.weaponRarity == 5)
        {
            stackCountCap = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Counterplay
        //___.text = Being hit while immune during Evasions casts two Lucent clusters and increase Weapon damage by 10%. Stacks 3x.

        if(stackCountActive > 0)
        {
            proc.GetComponent<Text>().text = "Counterplay x" + stackCountActive;
        }

        //Allows Counterplay's effect to be triggered
        status.counterplayCheat = gameObject;

        //Being damaged with this Weapon active spawns two Lucent clusters
        if (status.counterplayFlag)
        {
            GameObject payoff = Instantiate(lucentCluster, status.transform.position + (status.transform.right * 2f), Quaternion.identity);
            payoff.name = lucentCluster.name;
            payoff.GetComponent<LucentScript>().shatterDelayTime = 0.25f;
            payoff.GetComponent<LucentScript>().ShatterCalculation();
            payoff.GetComponent<LucentScript>().StartCoroutine(payoff.GetComponent<LucentScript>().Shatter());

            GameObject payoffTheSequel = Instantiate(lucentCluster, status.transform.position + (-status.transform.right * 2f), Quaternion.identity);
            payoffTheSequel.name = lucentCluster.name;
            payoffTheSequel.GetComponent<LucentScript>().shatterDelayTime = 0.25f;
            payoffTheSequel.GetComponent<LucentScript>().ShatterCalculation();
            payoffTheSequel.GetComponent<LucentScript>().StartCoroutine(payoffTheSequel.GetComponent<LucentScript>().Shatter());

            //Rarity 5 Weapons spawn a Solution Grenade
            if (firearm.weaponRarity == 5)
            {
                GameObject solution = Instantiate(inventory.grenades[1], transform.position + Vector3.down, Quaternion.Euler(new Vector3(90f, 0f, 0f)));
                solution.GetComponent<SolutionGrenadeScript>().armingTime = 0.0f;
                solution.GetComponent<SolutionGrenadeScript>().StartCoroutine(solution.GetComponent<SolutionGrenadeScript>().SetupGrenade());
            }

            //Increases damage if maximum stacks haven't been reached
            if (stackCountActive != stackCountCap)
            {
                stackCountActive++;
                firearm.damage += dmgAdd;
            }

            status.counterplayFlag = false;
        }
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
            status.counterplayCheat = null;
        }
    }
}