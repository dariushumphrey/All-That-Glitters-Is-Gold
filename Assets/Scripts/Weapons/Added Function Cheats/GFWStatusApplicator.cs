using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFWStatusApplicator : MonoBehaviour
{
    public GameObject particles; //VFX used to convey activity
    private GameObject targeted; //Target that winds will track
    private List<GameObject> targets = new List<GameObject>(); //List of Enemies
    private int targetRandom; //Number used to select random targets
    internal float debuffMultiplier = 1.75f; //Strength of Health debuff
    internal float travelRadius = 20f; //Radius that winds use to find Enemies
    internal float travelLerpSpeed = 1f; //Rate of speed for winds 
    internal float effectRadius = 10f; //Radius that winds use to apply effects
    internal float effectDuration = 45f; //Duration of winds
    internal bool fatedFlag = false; //Permits Rarity 5 effects if true

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
        //Tracks an Enemy
        if(targeted != null)
        {
            transform.position = Vector3.Lerp(transform.position, targeted.transform.position, travelLerpSpeed * Time.deltaTime);

            //Applies damage-over-time if Rarity 5 Weapon made the winds
            if (!targeted.GetComponent<DamageOverTimeScript>() && !targeted.GetComponent<EnemyHealthScript>().isImmune && fatedFlag)
            {
                targeted.gameObject.AddComponent<DamageOverTimeScript>();
                targeted.GetComponent<DamageOverTimeScript>().dotDamage = 125;
                targeted.GetComponent<DamageOverTimeScript>().damageOverTimeProc = 1f;
            }

            //Finds a new target if tracked target is defeated
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

        //Removes an Enemy from its list when defeated
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

    /// <summary>
    /// Searches for Enemies within its radius to track
    /// </summary>
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
        //Applies debuffs to once-unaffected Enemies
        //Tracks a target if one was not being tracked
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
        //Applies debuffs to once-unaffected Enemies
        //Tracks a target if one was not being tracked
        if (other.gameObject.CompareTag("Enemy"))
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
