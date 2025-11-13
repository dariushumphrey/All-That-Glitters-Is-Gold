using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cadence : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerMoveScript move;
    internal GameObject proc; //Text UI that records Cheat activity
    private GameObject cluster;

    private int deadCount = 0; //total count of Enemies defeated
    private int triggerCount = 3; //Goal number to trigger effect
    private int shotCount; //total number of confirmed hits
    internal bool hitConfirmed, killConfirmed; //Affirms achieved hits/kills if true
    internal Vector3 clusterPosition; //Lucent Cluster spawn position

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        move = FindObjectOfType<PlayerMoveScript>();
        cluster = Resources.Load<GameObject>("Game Items/testLucent");
        proc.GetComponent<Text>().text = "";      
    }

    // Update is called once per frame
    void Update()
    {
        //Cadence
        //___.text = Every third Kill spawns a Lucent cluster.

        //Increments kill counter
        if(killConfirmed && firearm.weaponRarity != 5)
        {
            deadCount++;
            killConfirmed = false;
        }

        //Produces a Lucent Cluster at defeated Enemy position
        if (deadCount >= triggerCount)
        {
            deadCount = 0;
            GameObject lucent = Instantiate(cluster, clusterPosition + (Vector3.up * 1.5f), transform.rotation);
            lucent.name = cluster.name;
            lucent.GetComponent<LucentScript>().lucentGift *= firearm.weaponRarity;
            lucent.GetComponent<LucentScript>().ShatterCalculation();

            if (move.zeroGravity)
            {
                lucent.GetComponent<Rigidbody>().useGravity = false;
            }

            proc.GetComponent<Text>().text = "Cadence";
            StartCoroutine(ClearText());

        }

        //Increments hit counter
        if (hitConfirmed && firearm.weaponRarity == 5)
        {
            shotCount++;
            hitConfirmed = false;         
        }

        //Produces a Lucent Cluster at damaged Enemy position
        if (shotCount >= triggerCount)
        {
            shotCount = 0;
            GameObject lucent = Instantiate(cluster, clusterPosition, transform.rotation);
            lucent.name = cluster.name;
            lucent.GetComponent<LucentScript>().lucentGift *= firearm.weaponRarity;
            lucent.GetComponent<LucentScript>().ShatterCalculation();

            if (move.zeroGravity)
            {
                lucent.GetComponent<Rigidbody>().useGravity = false;
            }

            proc.GetComponent<Text>().text = "Cadence";
            StartCoroutine(ClearText());
        }
    }

    IEnumerator ClearText()
    {
        yield return new WaitForSeconds(0.5f);
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