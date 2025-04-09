using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    public int damage;
    public int attackRange;
    public float attackRate;
    public Transform attackStartPoint;

    public LineRenderer attackLine;

    private float attackAgain;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attacking();
    }


    void Attacking()
    {
        attackAgain += Time.deltaTime;

        Vector3 rayOrigin = attackStartPoint.transform.position;
        RaycastHit hit;

        attackLine.SetPosition(0, attackStartPoint.position);

        if(Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, attackRange))
        {
            attackLine.SetPosition(1, hit.point);

            if(hit.collider.tag == "Player" && attackAgain >= attackRate)
            {
                attackAgain = 0.0f;
                hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                if (GetComponent<EnemyLeaderScript>() != null)
                {
                    GetComponent<EnemyLeaderScript>().Pursuit();
                }

                if(GetComponent<EnemyFollowerScript>() != null)
                {
                    if(GetComponent<EnemyFollowerScript>().leader != null && GetComponent<EnemyFollowerScript>().leader.GetComponent<EnemyHealthScript>().healthCurrent > 0)
                    {
                        GetComponent<EnemyFollowerScript>().leader.Pursuit();
                    }

                    GetComponent<EnemyFollowerScript>().ChasePlayer();
                }
            }
        }

        else
        {
            attackLine.SetPosition(1, rayOrigin + (attackStartPoint.transform.forward * attackRange));
        }
    }
}
