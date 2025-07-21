using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cadence : MonoBehaviour
{
    private FirearmScript firearm;
    private EnemyManagerScript enemy;
    internal GameObject proc;

    private int deadCount = 0;
    private int triggerCount = 3;
    private int shotCount;
    internal bool hitConfirmed, killConfirmed;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        enemy = FindObjectOfType<EnemyManagerScript>();
        proc.GetComponent<Text>().text = " ";      

    }

    // Update is called once per frame
    void Update()
    {
        //Cadence
        //___.text = Every third Kill spawns a Lucent cluster.

        if(killConfirmed && firearm.weaponRarity != 5)
        {
            deadCount++;
            killConfirmed = false;
        }

        if (deadCount >= triggerCount)
        {
            deadCount = 0;
            enemy.CadenceRewardPosition(firearm.cadencePosition);
            enemy.CadenceReward();
            proc.GetComponent<Text>().text = "Cadence";
            StartCoroutine(ClearText());

        }

        if (hitConfirmed && firearm.weaponRarity == 5)
        {
            shotCount++;
            hitConfirmed = false;         
        }

        if (shotCount >= triggerCount)
        {
            shotCount = 0;
            enemy.FatedCadenceReward(firearm.fatedCadencePosition);
            proc.GetComponent<Text>().text = "Cadence";
            StartCoroutine(ClearText());
        }
    }

    IEnumerator ClearText()
    {
        yield return new WaitForSeconds(0.5f);
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
