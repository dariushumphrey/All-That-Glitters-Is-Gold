using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public int damage = 100;
    private EnemyManagerScript manager;
    public bool berthFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<EnemyManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Player")
        {
            manager.damageDealt += damage;
            Destroy(gameObject);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerStatusScript>().InflictDamage(damage);

            if(berthFlag)
            {
                if(other.gameObject.GetComponent<DamageOverTimeScript>())
                {
                    //Do nothing else -- Player already has Damage Over Time applied
                }

                else
                {
                    other.gameObject.AddComponent<DamageOverTimeScript>();
                    other.gameObject.GetComponent<DamageOverTimeScript>().playerHarm = true;
                    other.gameObject.GetComponent<DamageOverTimeScript>().dotDamage = 14 * manager.dropRarity;
                    other.gameObject.GetComponent<DamageOverTimeScript>().damageOverTimeLength = 1f;
                }
            }

            manager.damageDealt += damage;
            Destroy(gameObject);

        }
    }
}
