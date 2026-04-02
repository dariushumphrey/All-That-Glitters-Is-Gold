using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LucentScript : MonoBehaviour
{
    public int lucentGift; //Lucent cluster worth
    public bool threat = false; //Grants ability to damage Player if true
    public GameObject shatterEffect; //VFX that plays on condition

    private float shatterPercent = 150f; //Percent that cluster damage increases by
    private int shatterDamage; //Lucent cluster damage
    internal bool shot = false; //Confirms damage by source if true
    internal float shatterDelayTime = 0.3f; //Time to wait before detonation

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
    }

    // Update is called once per frame
    void Update()
    {
        //Applies shatter damage in radius to Enemies, triggers detonation in other clusters
        if(shot)
        {
            Vector3 epicenter = transform.position;
            Collider[] affected = Physics.OverlapSphere(transform.position, 5f);
            foreach (Collider hit in affected)
            {
                //Debug.Log(affected.Length);

                if(hit.gameObject.CompareTag("Enemy"))
                {
                    //Triggers stun mechanic on Bosses
                    if(gameObject.transform.parent != null && gameObject.transform.parent.GetComponent<ReplevinScript>().amBoss)
                    {
                        if(gameObject.GetComponent<Rigidbody>() == null)
                        {
                            gameObject.AddComponent<Rigidbody>();
                        }

                        gameObject.transform.parent.GetComponent<ReplevinScript>().stunMechanic = null;
                        //gameObject.transform.parent.GetComponent<ReplevinScript>().stunMechanic.transform.parent = null;
                        gameObject.transform.parent.GetComponent<ReplevinScript>().enemy.isImmune = false;

                        if (gameObject.transform.parent.GetComponent<ReplevinScript>().interrupted == false)
                        {
                            gameObject.transform.parent.GetComponent<ReplevinScript>().interrupted = true;

                        }
                    }

                    indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                    currentIteration = Regex.Replace(dpsText.GetComponent<Text>().text, "<.*?>", string.Empty);

                    if (hit.GetComponent<EnemyHealthScript>() != null)
                    {
                        newDPSLine = "<size=36><color=cyan>" + indent + shatterDamage.ToString() + "</color></size>";
                        currentDPSLine = newDPSLine + "\n" + "<size=24><color=silver>" + currentIteration + "</color></size>";

                        hit.GetComponent<EnemyHealthScript>().inflictDamage(shatterDamage);
                        if (hit.GetComponent<EnemyHealthScript>().healthCurrent <= 0 && hit.GetComponent<Rigidbody>() == null)
                        {
                            hit.gameObject.AddComponent<Rigidbody>();
                            hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
                        }
                    }

                    dpsText.GetComponent<Text>().text = currentDPSLine;
                    dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                    dpsLinesClear = dpsLinesReset;
                }

                if(hit.gameObject.CompareTag("Lucent"))
                {
                    hit.gameObject.GetComponent<LucentScript>().StartCoroutine(hit.gameObject.GetComponent<LucentScript>().Shatter());
                }
            }

            GameObject effect = Instantiate(shatterEffect, transform.position, Quaternion.identity);
            effect.name = "Shatter VFX";

            Destroy(gameObject);
        }      
    }
    
    /// <summary>
    /// Calculates cluster shatter damage
    /// </summary>
    public void ShatterCalculation()
    {
        shatterPercent /= 100;
        shatterPercent *= lucentGift;
        shatterDamage = (int)shatterPercent;
    }

    /// <summary>
    ///Applies shatter damage in radius to Enemies, Players (if threat is true), triggers detonation in other clusters after delay
    /// </summary>
    public IEnumerator Shatter()
    {
        if(gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObject.GetComponent<Rigidbody>().AddExplosionForce(10f, transform.position, 10f, 50f);
        }

        yield return new WaitForSeconds(shatterDelayTime);
        Vector3 epicenter = transform.position;
        Collider[] affected = Physics.OverlapSphere(transform.position, 5f);
        foreach (Collider hit in affected)
        {
            if(hit.gameObject.CompareTag("Player") && threat)
            {
                ShatterCalculation();
                hit.gameObject.GetComponent<PlayerStatusScript>().InflictDamage(shatterDamage);
                hit.gameObject.GetComponent<PlayerStatusScript>().playerHit = true;
            }

            if (hit.gameObject.CompareTag("Enemy"))
            {
                if (gameObject.transform.parent != null && gameObject.transform.parent.GetComponent<ReplevinScript>().amBoss)
                {
                    if (gameObject.GetComponent<Rigidbody>() == null)
                    {
                        gameObject.AddComponent<Rigidbody>();
                    }

                    gameObject.transform.parent.GetComponent<ReplevinScript>().stunMechanic = null;
                    //gameObject.transform.parent.GetComponent<ReplevinScript>().stunMechanic.transform.parent = null;
                    gameObject.transform.parent.GetComponent<ReplevinScript>().enemy.isImmune = false;

                    if (gameObject.transform.parent.GetComponent<ReplevinScript>().interrupted == false)
                    {
                        gameObject.transform.parent.GetComponent<ReplevinScript>().interrupted = true;
                    }
                }

                if (hit.GetComponent<EnemyHealthScript>() != null)
                {
                    hit.GetComponent<EnemyHealthScript>().inflictDamage(shatterDamage);
                    if (hit.GetComponent<EnemyHealthScript>().healthCurrent <= 0 && hit.GetComponent<Rigidbody>() == null)
                    {
                        hit.gameObject.AddComponent<Rigidbody>();
                        hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
                    }
                }
            }

            if (hit.gameObject.CompareTag("Lucent"))
            {
                if (hit.gameObject.GetComponent<Rigidbody>() != null)
                {
                    hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(100f, transform.position, 10f, 100f);
                }

                hit.gameObject.GetComponent<LucentScript>().StartCoroutine(hit.gameObject.GetComponent<LucentScript>().Shatter());

            }
        }

        GameObject effect = Instantiate(shatterEffect, transform.position, Quaternion.identity);
        effect.name = "Shatter VFX";
        Destroy(gameObject);
    }
}
