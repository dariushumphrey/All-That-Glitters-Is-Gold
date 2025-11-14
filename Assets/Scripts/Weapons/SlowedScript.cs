using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowedScript : MonoBehaviour
{
    internal float slowedLength = 10f; //Duration of effect
    private float slowedPercent = 50f;
    private float slowPctReset;
    private ReplevinScript enemy;

    // Start is called before the first frame update
    void Start()
    {
        slowPctReset = slowedPercent;
        if(GetComponent<ReplevinScript>())
        {
            enemy = GetComponent<ReplevinScript>();
        }

        //Boss-level Enemies cannot be slowed
        //Normal Enemies receive Movement penalties
        if(enemy)
        {
            if(enemy.amBoss)
            {
                Destroy(this);
            }

            else
            {
                SlowCalculation();
                SlowEnemy();
            }
        }

        StartCoroutine(KillDebuff());
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Calculates Movement Speed reduction by %
    /// </summary>
    void SlowCalculation()
    {
        slowedPercent /= 100;
        slowedPercent *= enemy.moveSpeed;
        enemy.moveSpeedSlow = (int)slowedPercent;

        slowedPercent = slowPctReset;
        slowedPercent /= 100;
        slowedPercent *= enemy.boostSpeed;
        enemy.boostSpeedSlow = (int)slowedPercent;

        slowedPercent = slowPctReset;
        slowedPercent /= 100;
        slowedPercent *= enemy.enemyAcceleration;
        enemy.nmaAccelSlow = (int)slowedPercent;
    }

    /// <summary>
    /// Reduces Enemies' moving speeds by %
    /// </summary>
    void SlowEnemy()
    {
        enemy.moveSpeed = enemy.moveSpeedSlow;
        enemy.boostSpeed = enemy.boostSpeedSlow;
        enemy.enemyAcceleration = enemy.nmaAccelSlow;
    }

    /// <summary>
    /// Restores Enemies' moving speeds to default
    /// </summary>
    void RestoreSpeed()
    {
        enemy.moveSpeed = enemy.moveSpeedReset;
        enemy.boostSpeed = enemy.boostSpeedReset;
        enemy.enemyAcceleration = enemy.accelReset;
    }

    /// <summary>
    /// Destroys itself after delay
    /// </summary>
    private IEnumerator KillDebuff()
    {
        yield return new WaitForSeconds(slowedLength);
        RestoreSpeed();
        Destroy(this);
    }
}
