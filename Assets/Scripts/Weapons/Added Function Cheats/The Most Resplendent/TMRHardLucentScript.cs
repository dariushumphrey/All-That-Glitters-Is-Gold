using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class TMRHardLucentScript : MonoBehaviour
{
    public int shockwaveBuildup = 0; //current buildup strength
    public int shockwaveGoal = 2000; //Goal number of buildup strength
    public int shatterDamage; //Damage inflicted
    public GameObject lucentCluster; //Lucent game object
    public Collider field; //Zone for cluster spawning
    public bool fatedCrystal = false; //Enables Rarity 5 behavior if true
    private GameObject miniLucent; //Lucent game object
    private GameObject shatterEffect; //VFX used to convey activity
    private float shatterPercent = 150f; //% of shatter damage
    private float effectTimeTotal = 5.5f; //Duration of Crystal effect
    private float lucentProduceTimer = 0.5f; //Goal time to spawn Lucent Clusters
    private int pMaxHealth; //Holds Player max Health
    private float restorePercent = 35f; //% to restore Player Health by
    private int restoreAdd; //Number used to add onto Player Health
    private float produceReset; //Holds starting Lucent Cluster timer
    private int shatterDamageAdd; //Number used to add onto crystal/cluster damage
    private Bounds spawnField; //Bounds that clusters spawn within

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

        produceReset = lucentProduceTimer;
        shatterEffect = Resources.Load<GameObject>("Particles/LucentHardShatterEffect");
        ShatterCalculation();     

        StartCoroutine(KillHardLucent());
    }

    // Update is called once per frame
    void Update()
    {
        LucentPassive();

        //Triggers a damage shockwave when buildup reaches goal
        if(shockwaveBuildup >= shockwaveGoal)
        {
            HardLucentShockwave();
        }
    }

    /// <summary>
    /// Adjusts Hard Lucent crystal shockwave damage
    /// </summary>
    public void ShatterCalculation()
    {
        shatterPercent /= 100;
        shatterPercent *= shatterDamage;
        shatterDamageAdd = (int)shatterPercent;
        shatterDamage += shatterDamageAdd;
    }

    /// <summary>
    /// Produces Lucent Clusters when timer reaches zero
    /// </summary>
    public void LucentPassive()
    {
        lucentProduceTimer -= Time.deltaTime;
        if(lucentProduceTimer <= 0f)
        {
            lucentProduceTimer = produceReset;

            spawnField = field.bounds;
            Vector3 spawnSite = spawnField.center + new Vector3(Random.Range(-spawnField.extents.x, spawnField.extents.x),
                                                                Random.Range(-spawnField.extents.y, spawnField.extents.y),
                                                                Random.Range(-spawnField.extents.z, spawnField.extents.z));

            miniLucent = Instantiate(lucentCluster, spawnSite, transform.rotation);
            miniLucent.GetComponent<LucentScript>().lucentGift += shatterDamageAdd;
            miniLucent.GetComponent<LucentScript>().ShatterCalculation();
            miniLucent.name = lucentCluster.name;

            miniLucent.GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Shatters Hard Lucent crystal at end of duration
    /// </summary>
    public void HardLucentShatter()
    {
        Vector3 epicenter = transform.position;
        Collider[] affected = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider hit in affected)
        {

            if (hit.gameObject.CompareTag("Enemy"))
            {
                if (gameObject.transform.parent != null && gameObject.transform.parent.GetComponent<ReplevinScript>().amBoss)
                {
                    if (gameObject.GetComponent<Rigidbody>() == null)
                    {
                        gameObject.AddComponent<Rigidbody>();
                    }
                }

                if (hit.GetComponent<EnemyHealthScript>() != null)
                {
                    indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                    currentIteration = Regex.Replace(dpsText.GetComponent<Text>().text, "<.*?>", string.Empty);

                    newDPSLine = "<size=36><color=cyan>" + indent + shatterDamage.ToString() + "</color></size>";
                    currentDPSLine = newDPSLine + "\n" + "<size=24><color=silver>" + currentIteration + "</color></size>";

                    hit.GetComponent<EnemyHealthScript>().inflictDamage(shatterDamage);
                    if (hit.GetComponent<EnemyHealthScript>().healthCurrent <= 0 && hit.GetComponent<Rigidbody>() == null)
                    {
                        hit.gameObject.AddComponent<Rigidbody>();
                        hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
                    }

                    dpsText.GetComponent<Text>().text = currentDPSLine;
                    dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                    dpsLinesClear = dpsLinesReset;
                }
            }

            if (hit.gameObject.CompareTag("Lucent"))
            {
                hit.gameObject.GetComponent<LucentScript>().shatterDelayTime = 0.5f;
                hit.gameObject.GetComponent<LucentScript>().StartCoroutine(hit.gameObject.GetComponent<LucentScript>().Shatter());
            }
        }

        GameObject vfx = Instantiate(shatterEffect, transform.position + (Vector3.up * 0.01f), Quaternion.identity);
        vfx.name = shatterEffect.name;

        Destroy(gameObject);
    }

    /// <summary>
    /// Triggers 10m damage shockwave and resets buildup progress
    /// </summary>
    public void HardLucentShockwave()
    {
        Vector3 epicenter = transform.position;
        Collider[] affected = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider hit in affected)
        {

            if (hit.gameObject.CompareTag("Enemy"))
            {
                indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                currentIteration = Regex.Replace(dpsText.GetComponent<Text>().text, "<.*?>", string.Empty);

                if (gameObject.transform.parent != null && gameObject.transform.parent.GetComponent<ReplevinScript>().amBoss)
                {
                    if (gameObject.GetComponent<Rigidbody>() == null)
                    {
                        gameObject.AddComponent<Rigidbody>();
                    }
                }

                if (hit.GetComponent<EnemyHealthScript>() != null)
                {
                    indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                    currentIteration = Regex.Replace(dpsText.GetComponent<Text>().text, "<.*?>", string.Empty);

                    newDPSLine = "<size=36><color=cyan>" + indent + shatterDamage.ToString() + "</color></size>";
                    currentDPSLine = newDPSLine + "\n" + "<size=24><color=silver>" + currentIteration + "</color></size>";

                    hit.GetComponent<EnemyHealthScript>().inflictDamage(shatterDamage);
                    if (hit.GetComponent<EnemyHealthScript>().healthCurrent <= 0 && hit.GetComponent<Rigidbody>() == null)
                    {
                        hit.gameObject.AddComponent<Rigidbody>();
                        hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
                    }

                    dpsText.GetComponent<Text>().text = currentDPSLine;
                    dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                    dpsLinesClear = dpsLinesReset;
                }
            }

            if (hit.gameObject.CompareTag("Lucent"))
            {
                hit.gameObject.GetComponent<LucentScript>().shatterDelayTime = 0.5f;
                hit.gameObject.GetComponent<LucentScript>().StartCoroutine(hit.gameObject.GetComponent<LucentScript>().Shatter());
            }
        }

        GameObject vfx = Instantiate(shatterEffect, transform.position + (Vector3.up * 0.01f), Quaternion.identity);
        vfx.name = shatterEffect.name;

        shockwaveBuildup = 0;
    }

    /// <summary>
    /// Detonates Hard Lucent crystal after delay
    /// </summary>
    public IEnumerator KillHardLucent()
    {
        yield return new WaitForSeconds(effectTimeTotal);
        HardLucentShatter();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(fatedCrystal)
        {
            //Increases Player Health if Player collides into crystal
            if (collision.gameObject.tag == "Player")
            {
                pMaxHealth = collision.gameObject.GetComponent<PlayerStatusScript>().playerHealthMax;
                restorePercent /= 100;
                restorePercent *= pMaxHealth;
                restoreAdd = (int)restorePercent;

                collision.gameObject.GetComponent<PlayerStatusScript>().playerHealth += restoreAdd;
                if(collision.gameObject.GetComponent<PlayerStatusScript>().playerHealth == collision.gameObject.GetComponent<PlayerStatusScript>().playerHealthMax)
                {
                    collision.gameObject.GetComponent<PlayerStatusScript>().playerHealth = collision.gameObject.GetComponent<PlayerStatusScript>().playerHealthMax;
                }

                HardLucentShatter();
            }
        }
    }
}
