using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMRHardLucentScript : MonoBehaviour
{
    public int shockwaveBuildup = 0;
    public int shockwaveGoal = 2000;
    public int shatterDamage;
    public GameObject lucentCluster;
    public Collider field;
    public bool fatedCrystal = false;
    private GameObject miniLucent;
    private GameObject shatterEffect;
    private float shatterPercent = 150f;
    private float effectTimeTotal = 5.5f;
    private float lucentProduceTimer = 0.5f;
    private int pMaxHealth;
    private float restorePercent = 35f;
    private int restoreAdd;
    private float produceReset;
    private int shatterDamageAdd;
    private Bounds spawnField;

    // Start is called before the first frame update
    void Start()
    {
        produceReset = lucentProduceTimer;
        shatterEffect = Resources.Load<GameObject>("Particles/LucentHardShatterEffect");
        ShatterCalculation();     

        StartCoroutine(KillHardLucent());
    }

    // Update is called once per frame
    void Update()
    {
        LucentPassive();

        if(shockwaveBuildup >= shockwaveGoal)
        {
            HardLucentShockwave();
        }
    }

    public void ShatterCalculation()
    {
        shatterPercent /= 100;
        shatterPercent *= shatterDamage;
        shatterDamageAdd = (int)shatterPercent;
        shatterDamage += shatterDamageAdd;
    }

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

            //miniLucent.GetComponent<Rigidbody>().AddExplosionForce(100f, transform.forward, 70f, 30f, ForceMode.Impulse);
            miniLucent.GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
        }
    }

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

                    //gameObject.transform.parent.GetComponent<ReplevinScript>().stunMechanic = null;
                    //gameObject.transform.parent.GetComponent<ReplevinScript>().enemy.isImmune = false;

                    //if (gameObject.transform.parent.GetComponent<ReplevinScript>().interrupted == false)
                    //{
                    //    gameObject.transform.parent.GetComponent<ReplevinScript>().interrupted = true;

                    //}
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
                hit.gameObject.GetComponent<LucentScript>().shatterDelayTime = 0.5f;
                hit.gameObject.GetComponent<LucentScript>().StartCoroutine(hit.gameObject.GetComponent<LucentScript>().Shatter());
            }
        }

        GameObject vfx = Instantiate(shatterEffect, transform.position + (Vector3.up * 0.01f), Quaternion.identity);
        vfx.name = shatterEffect.name;

        Destroy(gameObject);
    }

    public void HardLucentShockwave()
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

                    //gameObject.transform.parent.GetComponent<ReplevinScript>().stunMechanic = null;
                    //gameObject.transform.parent.GetComponent<ReplevinScript>().enemy.isImmune = false;

                    //if (gameObject.transform.parent.GetComponent<ReplevinScript>().interrupted == false)
                    //{
                    //    gameObject.transform.parent.GetComponent<ReplevinScript>().interrupted = true;

                    //}
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
                hit.gameObject.GetComponent<LucentScript>().shatterDelayTime = 0.5f;
                hit.gameObject.GetComponent<LucentScript>().StartCoroutine(hit.gameObject.GetComponent<LucentScript>().Shatter());
            }
        }

        GameObject vfx = Instantiate(shatterEffect, transform.position + (Vector3.up * 0.01f), Quaternion.identity);
        vfx.name = shatterEffect.name;

        shockwaveBuildup = 0;
    }

    public IEnumerator KillHardLucent()
    {
        yield return new WaitForSeconds(effectTimeTotal);
        HardLucentShatter();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(fatedCrystal)
        {
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
