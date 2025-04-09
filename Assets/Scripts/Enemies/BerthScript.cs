using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BerthScript : MonoBehaviour
{
    public int additionalDamage = 75;
    public float explodeRadius = 10.0f;
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
        BerthDifficultyMatch();
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
            Rigidbody inflict = hit.GetComponent<Rigidbody>();
            if (inflict != null)
            {
                if (inflict.GetComponent<EnemyHealthScript>() != null)
                {
                    inflict.GetComponent<EnemyHealthScript>().inflictDamage(additionalDamage * enemyDamageMultiplier);
                }

                if (inflict.GetComponent<PlayerStatusScript>() != null)
                {
                    inflict.GetComponent<PlayerStatusScript>().InflictDamage(additionalDamage);

                    if(inflict.GetComponent<PlayerStatusScript>().playerShield <= 0)
                    {
                        inflict.GetComponent<PlayerStatusScript>().playerHealth -= additionalDamage;
                    }
                }

                inflict.AddExplosionForce(explodeForce, epicenter, explodeRadius, 40.0f, ForceMode.Impulse);
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explodeRadius);
    }
}
