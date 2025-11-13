using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosNegDOT : MonoBehaviour
{
    private EnemyHealthScript enemy;
    internal int dotDamage; //Damage applied
    internal int dotRarity = 1; //Rarity level of damage-over-time effect
    private float dotTimer = 0f; //Damage timer
    private float damageOverTimeProc = 1f; //Time to reach before applying damage
    private float damageOverTimeLength = 10f; //Duration of damage-over-time

    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.GetComponent<EnemyHealthScript>();
        if(dotRarity == 5)
        {
            damageOverTimeProc = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        dotTimer += Time.deltaTime;
        if(dotTimer >= damageOverTimeProc)
        {
            enemy.inflictDamage(dotDamage);
            dotTimer = 0f;
        }

        damageOverTimeLength -= Time.deltaTime;
        if(damageOverTimeLength <= 0)
        {
            Destroy(this);
        }
    }
}
