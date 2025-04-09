using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosNegDOT : MonoBehaviour
{
    private EnemyHealthScript enemy;
    internal int dotDamage;
    internal int dotRarity = 1;
    private float dotTimer = 0f;
    private float damageOverTimeProc = 1f;
    private float damageOverTimeLength = 10f;
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
