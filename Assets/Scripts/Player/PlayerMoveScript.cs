using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    public int speed = 20;
    public float slopeForce;
    public float slopeForceRayLength;
    public float slopeCheckLength;
    private Rigidbody playerRigid;
    private Camera playerCamera;
    private Vector3 a, b, c;
    internal float horizInput;
    internal float vertInput;
    // Start is called before the first frame update
    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        SlopeVector();
    }

    private void FixedUpdate()
    {
        horizInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");

        Vector3 forward = playerCamera.transform.forward * vertInput;
        Vector3 sideways = playerCamera.transform.right * horizInput;
        forward.y = 0f;
        sideways.y = 0f;

        if(Input.GetButton("Horizontal"))
        {
            playerRigid.AddForce(sideways * speed - playerRigid.velocity);
        }

        if (Input.GetButton("Vertical"))
        {
            playerRigid.AddForce(forward * speed - playerRigid.velocity);
        }

        if((horizInput != 0 || vertInput != 0) && OnSlope())
        {
            playerRigid.AddForce(forward * speed + ((c - a) * slopeForce));
        }     

        //if (Input.GetKey(KeyCode.W))
        //{
        //    playerRigid.AddForce(forward * speed - playerRigid.velocity);
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    playerRigid.AddForce(-sideways * speed - playerRigid.velocity);
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    playerRigid.AddForce(-forward * speed - playerRigid.velocity);
        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    playerRigid.AddForce(sideways * speed - playerRigid.velocity);
        //}

    }

    private bool OnSlope()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, slopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }

    private void SlopeVector()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeForceRayLength))
        {
            //Debug.DrawLine(transform.position + hit.normal, transform.position + hit.normal * slopeCheckLength);

            a = hit.transform.up;
            b = hit.transform.right;
            c = hit.transform.forward;

            //Debug.DrawLine(transform.position + a, transform.position + a * slopeCheckLength, Color.green);
            //Debug.DrawLine(transform.position + b, transform.position + b * slopeCheckLength, Color.red);
            //Debug.DrawLine(transform.position + c, transform.position + c * slopeCheckLength, Color.blue);

        }
    }
}
