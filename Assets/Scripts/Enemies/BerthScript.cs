using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BerthScript : MonoBehaviour
{
    public int additionalDamage = 375;
    public float explodeRadius = 5.0f;
    public float explodeForce = 50.0f;

    private EnemyHealthScript foe;
    private NavMeshAgent enemy;
    //private GameObject itself;
    private int enemyDamageMultiplier = 30;

    // Start is called before the first frame update
    public void Start()
    {
        //itself = gameObject;
        enemy = GetComponent<NavMeshAgent>();
        foe = GetComponent<EnemyHealthScript>();
        //BerthDifficultyMatch();
    }

    //Update is called once per frame
    void Update()
    {
        
    }

    public void BerthDifficultyMatch()
    {
        if(foe.difficultyValue <= 1)
        {
            additionalDamage *= foe.difficultyValue;
            enemyDamageMultiplier *= foe.difficultyValue;
        }

        if (foe.difficultyValue == 2)
        {
            additionalDamage *= foe.difficultyValue;
            enemyDamageMultiplier *= foe.difficultyValue;
        }

        if (foe.difficultyValue == 3)
        {
            additionalDamage *= foe.difficultyValue;
            enemyDamageMultiplier *= foe.difficultyValue;
        }

        if (foe.difficultyValue == 4)
        {
            additionalDamage *= foe.difficultyValue;
            enemyDamageMultiplier *= foe.difficultyValue;
        }

        if (foe.difficultyValue >= 5)
        {
            additionalDamage *= foe.difficultyValue;
            enemyDamageMultiplier *= foe.difficultyValue;
        }
    }

    public void Explode()
    {
        Vector3 epicenter = transform.position;
        Collider[] affected = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider hit in affected)
        {
            if(hit.gameObject.CompareTag("Enemy"))
            {
                if (hit.GetComponent<EnemyHealthScript>() != null)
                {
                    hit.GetComponent<EnemyHealthScript>().inflictDamage(additionalDamage);
                    if (hit.GetComponent<EnemyHealthScript>().healthCurrent <= 0 && hit.GetComponent<Rigidbody>() == null)
                    {
                        hit.gameObject.AddComponent<Rigidbody>();
                        hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 5f, 500f);
                    }
                }
            }

            //if (hit.gameObject.CompareTag("Player"))
            //{
            //    if (hit.GetComponent<PlayerStatusScript>() != null)
            //    {
            //        hit.GetComponent<PlayerStatusScript>().InflictDamage(additionalDamage);
            //        if (hit.GetComponent<PlayerStatusScript>().playerShield <= 0)
            //        {
            //            hit.GetComponent<PlayerStatusScript>().playerHealth -= additionalDamage;
            //        }

            //        if(hit.GetComponent<PlayerStatusScript>().playerHealth <= 0)
            //        {
            //            hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explodeForce, epicenter, explodeRadius, 40.0f, ForceMode.Impulse);
            //        }
            //    }
            //}

            if (hit.gameObject.CompareTag("Lucent"))
            {
                hit.gameObject.GetComponent<LucentScript>().lucentGift = 0;
                hit.gameObject.GetComponent<LucentScript>().shot = true;
            }
        }
    }

    public IEnumerator RemoveSelf()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explodeRadius);
    }
}
