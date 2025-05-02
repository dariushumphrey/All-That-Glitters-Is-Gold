using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeScript : MonoBehaviour
{
    public int meleeDamage = 5000;
    public float meleeRangeMax = 7f;
    public float meleeRange = 3f;
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
        if(Input.GetKeyDown(KeyCode.F))
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

        if (meleeLock)
        {
            transform.position = Vector3.Lerp(transform.position, meleeTarget.transform.position, meleeSpeed * Time.deltaTime);
            MeleeStrike();
        }
    }

    void MeleeStrike()
    {
        RaycastHit hit;        
        if (Physics.Raycast(transform.position, transform.forward * meleeRange, out hit, meleeRangeMax))
        {
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<EnemyHealthScript>().inflictDamage(meleeDamage);
                if(hit.collider.gameObject.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                {
                    hit.collider.GetComponent<Rigidbody>().AddForce(-hit.collider.transform.forward * 0.5f, ForceMode.Impulse);
                }

                meleeLock = false;
            }
        }
    }
}
