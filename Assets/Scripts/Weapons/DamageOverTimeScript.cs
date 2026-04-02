using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class DamageOverTimeScript : MonoBehaviour
{
    private EnemyHealthScript enemy;
    private PlayerStatusScript player;
    internal int dotDamage; //Damage to apply
    internal bool playerHarm = false; //Activates ability to damage Players if true
    internal float damageOverTimeLength = 20f; //Duration of effect
    private float dotTimer = 0f; //Damage timer 
    internal float damageOverTimeProc = 0.25f; //Time to reach before applying damage

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

        //Damages the Player if available. Otherwise, damages an Enemy
        if (playerHarm)
        {
            player = gameObject.GetComponent<PlayerStatusScript>();
        }

        else
        {
            enemy = gameObject.GetComponent<EnemyHealthScript>();
        }

        StartCoroutine(KillDamageOverTime());
    }

    // Update is called once per frame
    void Update()
    {
        //Applies damage to either a Player or Enemy when goal time has been reached
        dotTimer += Time.deltaTime;
        if(dotTimer >= damageOverTimeProc)
        {         
            if (playerHarm && player != null)
            {
                player.InflictDamage(dotDamage);
            }

            if(enemy != null)
            {
                if (enemy.healthCurrent <= 0)
                {
                    Destroy(this);
                }

                else
                {
                    indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                    currentIteration = Regex.Replace(dpsText.GetComponent<Text>().text, "<.*?>", string.Empty);

                    newDPSLine = "<size=36><color=green>" + indent + dotDamage.ToString() + "</color></size>";
                    currentDPSLine = newDPSLine + "\n" + "<size=24><color=silver>" + currentIteration + "</color></size>";

                    enemy.inflictDamage(dotDamage);

                    dpsText.GetComponent<Text>().text = currentDPSLine;
                    dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                    dpsLinesClear = dpsLinesReset;
                }
            }

            dotTimer = 0f;
        }
    }

    /// <summary>
    /// Destroys itself after delay
    /// </summary>
    public IEnumerator KillDamageOverTime()
    {
        yield return new WaitForSeconds(damageOverTimeLength);
        Destroy(this);
    }
}
