using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counterplay : MonoBehaviour
{
    private float dmgIncreasePercent = 10f;
    private int dmgAdd;
    private int stackCountActive, stackCountCap;
    private bool done = false;
    private FirearmScript firearm;
    private PlayerStatusScript status;
    private PlayerInventoryScript inventory;
    private GameObject lucentCluster;
    internal GameObject proc;
    internal bool counterplayFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        status = FindObjectOfType<PlayerStatusScript>();
        inventory = FindObjectOfType<PlayerInventoryScript>();
        lucentCluster = Resources.Load<GameObject>("Game Items/testLucent");
        proc.GetComponent<Text>().text = " ";

        dmgIncreasePercent /= 100;
        dmgIncreasePercent *= firearm.damage;
        dmgAdd = (int)dmgIncreasePercent;

        if(firearm.weaponRarity == 5)
        {
            stackCountCap = 10;
        }

        else
        {
            stackCountCap = 3;
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

        status.counterplayCheat = gameObject;

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

            if(firearm.weaponRarity == 5)
            {
                GameObject solution = Instantiate(inventory.grenades[1], transform.position + Vector3.down, Quaternion.Euler(new Vector3(90f, 0f, 0f)));
                solution.GetComponent<SolutionGrenadeScript>().armingTime = 0.0f;
                solution.GetComponent<SolutionGrenadeScript>().StartCoroutine(solution.GetComponent<SolutionGrenadeScript>().SetupGrenade());
            }

            if(stackCountActive >= stackCountCap)
            {
                //Do nothing
            }

            else
            {
                stackCountActive++;
                firearm.damage += dmgAdd;
            }

            status.counterplayFlag = false;
        }
    }

    //IEnumerator ClearText()
    //{
    //    yield return new WaitForSeconds(1f);
    //    proc.GetComponent<Text>().text = " ";
    //}

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = " ";
            status.counterplayCheat = null;
        }
    }
}
