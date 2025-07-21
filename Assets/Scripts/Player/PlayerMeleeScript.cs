using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeScript : MonoBehaviour
{
    public int meleeDamage = 5000;
    public float meleeRange = 4f;
    public float meleeSpeed = 3f;

    internal bool meleeLock;
    internal GameObject meleeTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(meleeTarget);

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (meleeTarget == null)
            {
                //Do nothing
            }

            else
            {
                meleeLock = true;
            }
        }

        if (meleeLock && meleeTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, meleeTarget.transform.position, meleeSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(meleeTarget.transform.position - transform.position, Vector3.up), 2f);
            MeleeStrike();
        }
    }

    void MeleeStrike()
    {
        RaycastHit hit;        
        if (Physics.Raycast(transform.position, transform.forward, out hit, meleeRange - 3))
        {
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<EnemyHealthScript>().inflictDamage(meleeDamage);
                if(hit.collider.gameObject.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                {
                    if(hit.collider.GetComponent<Rigidbody>() != null)
                    {
                        hit.collider.GetComponent<Rigidbody>().AddForce(-hit.collider.transform.forward * 0.5f, ForceMode.Impulse);
                    }
                }

                meleeLock = false;
            }
        }
    }
}
