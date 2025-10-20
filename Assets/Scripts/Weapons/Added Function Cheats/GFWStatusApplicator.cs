using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFWStatusApplicator : MonoBehaviour
{
    public GameObject particles;
    private GameObject targeted;
    private List<GameObject> targets = new List<GameObject>();
    private bool lockedIn = false;
    private int targetRandom;
    internal float debuffMultiplier = 1.75f;
    internal float travelRadius = 20f;
    internal float travelLerpSpeed = 1f;
    internal float effectRadius = 10f;
    internal float effectDuration = 45f;
    internal bool fatedFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject winds = Instantiate(particles, transform.position, Quaternion.identity, gameObject.transform);
        winds.name = particles.name;

        FindTarget();

        Destroy(gameObject, effectDuration);
    }

    // Update is called once per frame
    void Update()
    {
        if(targeted != null)
        {
            transform.position = Vector3.Lerp(transform.position, targeted.transform.position, travelLerpSpeed * Time.deltaTime);
            if (!targeted.GetComponent<DamageOverTimeScript>() && !targeted.GetComponent<EnemyHealthScript>().isImmune && fatedFlag)
            {
                targeted.gameObject.AddComponent<DamageOverTimeScript>();
                targeted.GetComponent<DamageOverTimeScript>().dotDamage = 125;
                targeted.GetComponent<DamageOverTimeScript>().damageOverTimeProc = 1f;
            }

            if (targeted.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
            {
                targeted = null;
                FindTarget();
            }
        }

        else
        {
            transform.position = transform.position;
        }

        if (targets.Count != 0)
        {
            for (int e = 0; e < targets.Count; e++)
            {
                if (targets[e].GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                {
                    targets.Remove(targets[e]);
                }
            }
        }
    }

    private void FindTarget()
    {
        Vector3 epicenter = transform.position;
        Collider[] affected = Physics.OverlapSphere(transform.position, travelRadius);
        foreach (Collider hit in affected)
        {
            if (hit.gameObject.CompareTag("Enemy"))
            {
                targets.Add(hit.gameObject);
            }
        }

        if(targets.Count != 0)
        {
            targetRandom = Random.Range(0, targets.Count);
            targeted = targets[targetRandom];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            if(targeted == null)
            {
                targeted = other.gameObject;              
            }

            if(!other.gameObject.GetComponent<DebuffScript>() && !other.gameObject.GetComponent<EnemyHealthScript>().isImmune)
            {
                other.gameObject.AddComponent<DebuffScript>();
                other.gameObject.GetComponent<DebuffScript>().damageAmp = debuffMultiplier;
                other.gameObject.GetComponent<DebuffScript>().debuffLength = 45f;
            }        

            if (!other.gameObject.GetComponent<SlowedScript>() && !other.gameObject.GetComponent<ReplevinScript>().amBoss)
            {
                other.gameObject.AddComponent<SlowedScript>();
                other.gameObject.GetComponent<SlowedScript>().slowedLength = 45f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            if (targeted == null)
            {
                targeted = other.gameObject;
            }

            if (!other.gameObject.GetComponent<DebuffScript>() && !other.gameObject.GetComponent<EnemyHealthScript>().isImmune)
            {
                other.gameObject.AddComponent<DebuffScript>();
                other.gameObject.GetComponent<DebuffScript>().damageAmp = debuffMultiplier;
                other.gameObject.GetComponent<DebuffScript>().debuffLength = 45f;
            }

            if(!other.gameObject.GetComponent<SlowedScript>() && !other.gameObject.GetComponent<ReplevinScript>().amBoss)
            {
                other.gameObject.AddComponent<SlowedScript>();
                other.gameObject.GetComponent<SlowedScript>().slowedLength = 45f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}
