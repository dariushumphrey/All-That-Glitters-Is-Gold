using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BerthScript : MonoBehaviour
{
    public int berthDamage = 375; //Berth explosive damage

    //This value buffs damage performed by Berth explosions:
    //-Increasing this number adds a percentage of Berth damage onto itself
    //-Increasing Berth damage allows this number to increase damage in return
    public float berthPercent = 120f;
    public float explodeRadius = 4.0f;
    public float explodeForce = 50.0f;

    private EnemyHealthScript foe;
    private GameObject activation; //VFX used to convey activity
    private int berthDamageAdd; //Number used to add onto Berth damage
    internal bool exoticOverride = false; //Negates Player damage if true

    public void Awake()
    {
        foe = GetComponent<EnemyHealthScript>();
        activation = Resources.Load<GameObject>("Particles/BerthExplosionActive");
    }

    // Start is called before the first frame update
    public void Start()
    {
        if(!exoticOverride)
        {
            BerthDifficultyMatch();
        }
    }

    //Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Increases explosive damage by Enemy difficulty
    /// </summary>
    public void BerthDifficultyMatch()
    {
        if (foe.difficultyValue >= 2)
        {
            berthPercent *= foe.difficultyValue;
            berthPercent /= 100;
            berthPercent *= berthDamage;

            berthDamageAdd = (int)berthPercent;
            berthDamage += berthDamageAdd;           
        }
    }

    /// <summary>
    /// Applies damage to Players, other Enemies, and detonates Lucent clusters
    /// </summary>
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
                    if(!hit.GetComponent<PlayerStatusScript>().isInvincible)
                    {
                        hit.GetComponent<PlayerStatusScript>().InflictDamage(berthDamage);

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
