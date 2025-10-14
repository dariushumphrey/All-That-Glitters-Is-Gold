using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BerthScript : MonoBehaviour
{
    public int berthDamage = 375;
    //This value buffs damage performed by Berth explosions:
    //-Increasing this number adds a percentage of Berth damage onto itself
    //-Increasing Berth damage allows this number to increase damage in return
    public float berthPercent = 120f;
    public float explodeRadius = 3.0f;
    public float explodeForce = 50.0f;

    private EnemyHealthScript foe;
    private GameObject activation;
    //private GameObject itself;
    private int enemyDamageMultiplier = 30;
    private int berthDamageAdd;
    internal bool exoticOverride = false;

    public void Awake()
    {
        foe = GetComponent<EnemyHealthScript>();
        activation = Resources.Load<GameObject>("Particles/BerthExplosionActive");
    }

    // Start is called before the first frame update
    public void Start()
    {
        //itself = gameObject;
        if(exoticOverride)
        {
            //Do nothing - Exotic Single Fire Weapon added this Script and only needs to change base Damage.
        }

        else
        {
            BerthDifficultyMatch();
        }
    }

    //Update is called once per frame
    void Update()
    {
        
    }

    public void BerthDifficultyMatch()
    {

        if (foe.difficultyValue >= 2)
        {
            //additionalDamage *= foe.difficultyValue;
            //enemyDamageMultiplier *= foe.difficultyValue;
            berthPercent *= foe.difficultyValue;
            berthPercent /= 100;
            berthPercent *= berthDamage;
            berthDamageAdd = (int)berthPercent;
            berthDamage += berthDamageAdd;
            
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
                    hit.GetComponent<EnemyHealthScript>().inflictDamage(berthDamage);
                    if (hit.GetComponent<EnemyHealthScript>().healthCurrent <= 0 && hit.GetComponent<Rigidbody>() == null)
                    {
                        hit.gameObject.AddComponent<Rigidbody>();
                        hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(400f, transform.position, 5f, 500f);
                    }
                }
            }

            if (hit.gameObject.CompareTag("Player") && !exoticOverride)
            {
                if (hit.GetComponent<PlayerStatusScript>() != null)
                {
                    if(hit.GetComponent<PlayerStatusScript>().isInvincible == true)
                    {
                        //Do nothing
                    }

                    else
                    {
                        hit.GetComponent<PlayerStatusScript>().InflictDamage(berthDamage);
                        if (hit.GetComponent<PlayerStatusScript>().playerShield <= 0)
                        {
                            hit.GetComponent<PlayerStatusScript>().InflictDamage(berthDamage);
                        }

                        if (hit.GetComponent<PlayerStatusScript>().playerHealth <= 0)
                        {
                            hit.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explodeForce, epicenter, explodeRadius, 40.0f, ForceMode.Impulse);
                        }
                    }                    
                }
            }

            if (hit.gameObject.CompareTag("Lucent"))
            {
                hit.gameObject.GetComponent<LucentScript>().lucentGift = 0;
                hit.gameObject.GetComponent<LucentScript>().shot = true;
            }
        }

        Instantiate(activation, transform.position, Quaternion.identity);
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
