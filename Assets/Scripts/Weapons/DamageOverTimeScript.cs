using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeScript : MonoBehaviour
{
    private EnemyHealthScript enemy;
    private PlayerStatusScript player;
    internal int dotDamage;
    internal bool playerHarm = false;
    internal float damageOverTimeLength = 20f;
    private float dotTimer = 0f;
    internal float damageOverTimeProc = 0.25f;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerHarm)
        {
            player = gameObject.GetComponent<PlayerStatusScript>();
        }

        else
        {
            enemy = gameObject.GetComponent<EnemyHealthScript>();
        }

        StartCoroutine(KillDamageOverTime());
    }

    // Update is called once per frame
    void Update()
    {
        dotTimer += Time.deltaTime;
        if(dotTimer >= damageOverTimeProc)
        {
            if(playerHarm && player != null)
            {
                player.InflictDamage(dotDamage);
            }

            if(enemy != null)
            {
                enemy.inflictDamage(dotDamage);
            }

            dotTimer = 0f;
        }
    }

    public IEnumerator KillDamageOverTime()
    {
        yield return new WaitForSeconds(damageOverTimeLength);
        Destroy(this);
    }
}
