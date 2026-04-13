using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppendageScript : MonoBehaviour
{
    public GameObject appendage; //Optional gameObject for visual attack aid
    public float rotationAccelerant = 15f;
    public ReplevinScript replevin;
    public bool useAttackAgain = false;
    public bool useIsGrounded = false;

    public Vector3 attackRotation = new Vector3(-50f, 0f, 0f);
    public Vector3 restRotation = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(useAttackAgain)
        {
            if (replevin.attackAgain <= 0.0f)
            {
                Quaternion rotation = Quaternion.Euler(attackRotation);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * rotationAccelerant);
            }

            else
            {
                Quaternion rotation = Quaternion.Euler(restRotation);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * rotationAccelerant);
            }
        }

        else if(useIsGrounded)
        {
            if (!replevin.AmIGrounded())
            {
                Quaternion rotation = Quaternion.Euler(attackRotation);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * rotationAccelerant);
            }

            else
            {
                Quaternion rotation = Quaternion.Euler(restRotation);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * rotationAccelerant);
            }
        }

        else
        {
            if (replevin.attackLock)
            {
                Quaternion rotation = Quaternion.Euler(attackRotation);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * rotationAccelerant);
            }

            else
            {
                Quaternion rotation = Quaternion.Euler(restRotation);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * rotationAccelerant);
            }
        }      
    }
}
