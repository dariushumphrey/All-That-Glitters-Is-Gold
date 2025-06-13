using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public int damage = 100;
    private EnemyManagerScript manager;
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
            manager.damageDealt += damage;
            Destroy(gameObject);

        }
    }
}
