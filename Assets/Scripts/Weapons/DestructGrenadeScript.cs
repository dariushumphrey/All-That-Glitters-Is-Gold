using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class DestructGrenadeScript : MonoBehaviour
{
    public int explosiveDamage;
    public float explosiveRange = 8f;
    public float armingTime; //Time delay before detonation
    public GameObject detonationEffect; //VFX for grenade
    public LayerMask contactOnly; //Ensures Raycast accounts for Surfaces
    private bool hitOnce = false; //Confirms at least one surface collision if true

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

    }

    /// <summary>
    /// Collects colliders and applies damage to Enemies, detonates Lucent in its radius
    /// </summary>
    public IEnumerator SetupGrenade()
    {
        yield return new WaitForSeconds(armingTime);

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject.GetComponent<CapsuleCollider>());

        RaycastHit hit;
        Vector3 epicenter = transform.position;
        Collider[] affected = Physics.OverlapSphere(transform.position, explosiveRange);
        foreach (Collider contact in affected)
        {
            if (!Physics.Raycast(transform.position, (contact.transform.position - transform.position).normalized, out hit, explosiveRange, contactOnly))
            {
                if (contact.gameObject.CompareTag("Enemy"))
                {
                   

                    indent = new string(' ', currentDPSLine.Split('\n').Length * indentSpace);
                    currentIteration = Regex.Replace(dpsText.GetComponent<Text>().text, "<.*?>", string.Empty);

                    if (contact.GetComponent<EnemyHealthScript>() != null)
                    {
                        newDPSLine = "<size=36><color=orange>" + indent + explosiveDamage.ToString() + "</color></size>";
                        currentDPSLine = newDPSLine + "\n" + "<size=24><color=silver>" + currentIteration + "</color></size>";

                        contact.GetComponent<EnemyHealthScript>().inflictDamage(explosiveDamage);
                        if(contact.GetComponent<EnemyHealthScript>().healthCurrent <= 0 && contact.GetComponent<Rigidbody>() == null)                    
                        {
                            contact.gameObject.AddComponent<Rigidbody>();
                            contact.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 10f, 500f);
                        }
                    }

                    dpsText.GetComponent<Text>().text = currentDPSLine;
                    dpsText.GetComponent<TextClearScript>().clearTimer = dpsText.GetComponent<TextClearScript>().timerReset;
                    dpsLinesClear = dpsLinesReset;
                }

                if(contact.gameObject.CompareTag("Lucent"))
                {
                    contact.gameObject.GetComponent<LucentScript>().lucentGift = 0;
                    contact.gameObject.GetComponent<LucentScript>().shot = true;
                }
            }
        }

        GameObject effect = Instantiate(detonationEffect, transform.position, Quaternion.identity);
        effect.name = "Detonation VFX";
        Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!hitOnce)
        {
            StartCoroutine(SetupGrenade());
            hitOnce = true;
        }
    }
}