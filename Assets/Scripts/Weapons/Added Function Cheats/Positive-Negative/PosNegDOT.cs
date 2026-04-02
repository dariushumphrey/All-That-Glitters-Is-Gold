using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class PosNegDOT : MonoBehaviour
{
    private EnemyHealthScript enemy;
    internal int dotDamage; //Damage applied
    internal int dotRarity = 1; //Rarity level of damage-over-time effect
    private float dotTimer = 0f; //Damage timer
    private float damageOverTimeProc = 1f; //Time to reach before applying damage
    private float damageOverTimeLength = 10f; //Duration of damage-over-time

    internal GameObject dpsText; //Text objects that track Cheat, Damage activity
    internal string indent; //Used to produce new lines
    internal string currentIteration; //Used to capture current state of dpsText
    internal string currentDPSLine = ""; //Records damage history
    internal string newDPSLine; //Records most recent damage
    internal int indentSpace = 0; //Amount of applied indentation
    internal float dpsLinesClear = 2f; //Clears damage history after this time
    internal float dpsLinesReset;
    // Start is called before the first frame update
    void Start()
    {
        dpsText = GameObject.Find("dpsText");

        enemy = gameObject.GetComponent<EnemyHealthScript>();
        if(dotRarity == 5)
        {
            damageOverTimeProc = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        dotTimer += Time.deltaTime;
        if(dotTimer >= damageOverTimeProc)
        {
            if (enemy.healthCurrent <= 0)
            {
                Destroy(this);
            }

            else
            {
                indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                currentIteration = Regex.Replace(dpsText.GetComponent<Text>().text, "<.*?>", string.Empty);

                newDPSLine = "<size=36><color=blue>" + indent + dotDamage.ToString() + "</color></size>";
                currentDPSLine = newDPSLine + "\n" + "<size=24><color=silver>" + currentIteration + "</color></size>";

                enemy.inflictDamage(dotDamage);
                dotTimer = 0f;

                dpsText.GetComponent<Text>().text = currentDPSLine;
                dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                dpsLinesClear = dpsLinesReset;
            }
            
        }

        damageOverTimeLength -= Time.deltaTime;
        if(damageOverTimeLength <= 0)
        {
            Destroy(this);
        }
    }
}
