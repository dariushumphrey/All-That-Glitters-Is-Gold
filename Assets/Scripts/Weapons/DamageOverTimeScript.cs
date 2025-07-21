using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeScript : MonoBehaviour
{
    private EnemyHealthScript enemy;
    internal int dotDamage;
    private float dotTimer = 0f;
    private float damageOverTimeProc = 0.25f;
    private float damageOverTimeLength = 20f;
    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.GetComponent<EnemyHealthScript>();
        KillDamageOverTime();
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
    }

    IEnumerator KillDamageOverTime()
    {
        yield return new WaitForSeconds(damageOverTimeLength);
        Destroy(this);
    }
}
