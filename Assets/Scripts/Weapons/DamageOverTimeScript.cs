using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeScript : MonoBehaviour
{
    private EnemyHealthScript enemy;
    private PlayerStatusScript player;
    internal int dotDamage; //Damage to apply
    internal bool playerHarm = false; //Activates ability to damage Players if true
    internal float damageOverTimeLength = 20f; //Duration of effect
    private float dotTimer = 0f; //Damage timer 
    internal float damageOverTimeProc = 0.25f; //Time to reach before applying damage

    // Start is called before the first frame update
    void Start()
    {
        //Damages the Player if available. Otherwise, damages an Enemy
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
        //Applies damage to either a Player or Enemy when goal time has been reached
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

    /// <summary>
    /// Destroys itself after delay
    /// </summary>
    public IEnumerator KillDamageOverTime()
    {
        yield return new WaitForSeconds(damageOverTimeLength);
        Destroy(this);
    }
}
