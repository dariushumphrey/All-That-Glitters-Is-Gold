using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    public int speed = 20;
    public float speedDampening = -1f; //Governs time taken to slow to a stop after letting go of movement input.
    public float speedAccelerant = 0.5f; //Multiplier to exponentially increase movement speed
    public float slopeForce;
    public float slopeForceRayLength;
    public float slopeCheckLength;
    public float airborneCheck;
    public float airbornePull = 0.1f; //Multipler to force Player downwards if airborne
    private Rigidbody playerRigid;
    private Camera playerCamera;
    private Vector3 a, b, c;
    private Vector3 input;
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

    }

    private void FixedUpdate()
    {
        horizInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.forward * vertInput;
        Vector3 sideways = transform.right * horizInput;

        forward.y = 0f;
        sideways.y = 0f;

        if(Input.GetButton("Horizontal"))
        {
            playerRigid.AddForce(sideways * speed * speedAccelerant);
        }

        if (Input.GetButton("Vertical"))
        {
            playerRigid.AddForce(forward * speed * speedAccelerant);
        }

        if(Airborne())
        {
            playerRigid.AddForce(-Vector3.up * airbornePull);

            //float radius = collider.radius;
            //float height = collider.height;

            //RaycastHit hit;
            //if (Physics.Raycast(transform.position + Vector3.up * 0.25f, Vector3.down, out hit, 3f))
            //{
            //    if (hit.point != null)
            //    {
            //        transform.position = new Vector3(transform.position.x, hit.point.y + height / 2f - collider.center.y, transform.position.z);
            //    }
            //}
        }

        if (horizInput == 0 && vertInput == 0)
        {
            Vector3 velocityActive = playerRigid.velocity;
            Vector3 velocityRest = Vector3.zero;
            playerRigid.velocity = Vector3.Lerp(velocityActive, velocityRest, 1 - Mathf.Exp(speedDampening * Time.deltaTime));
        }

        //Airborne();
        //Debug.Log(Airborne());

        if ((horizInput != 0 || vertInput != 0) && OnSlope())
        {
            SlopeVector();
        }
       
        //if((horizInput != 0 || vertInput != 0) && OnSlope())
        //{
        //    playerRigid.AddForce(forward * speed + ((c - a) * slopeForce));
        //}     

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

    private bool Airborne()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down * airborneCheck, Color.cyan);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, airborneCheck))
        {
            if (hit.point != null)
            {
                return false;
            }
        }

        return true;
    }

    private void SlopeVector()
    {
        Vector3 sideVector;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, slopeCheckLength))
        {

            Debug.DrawRay(transform.position, Vector3.Cross(transform.forward, hit.normal), Color.red);
            //Debug.DrawRay(transform.position, hit.normal + new Vector3(0, hit.normal.y, 0), Color.green);
            //Debug.DrawRay(transform.position, hit.normal + new Vector3(0, 0, hit.normal.z), Color.blue);
            Debug.DrawRay(transform.position, transform.forward, Color.yellow);


            //Debug.Log(Vector3.Cross(transform.forward, hit.normal));
            sideVector = Vector3.Cross(transform.position, hit.normal).normalized;
            //Debug.Log(Vector3.Dot(sideVector, transform.forward));

            //Handles vertical slope traversal
            if (Vector3.Dot(hit.normal, transform.forward) < 0)
            {
                if (vertInput < 0)
                {
                    playerRigid.AddForce(-Vector3.up * slopeForce);
                }

                else
                {
                    //Debug.Log("Upwards");
                    playerRigid.AddForce(Vector3.up * slopeForce);
                }

                //Debug.Log("Upwards");
                //playerRigid.AddForce(Vector3.up * slopeForce);
            }

            else if (Vector3.Dot(hit.normal, transform.forward) > 0)
            {
                if (vertInput < 0 /*|| horizInput > 0*/)
                {
                    playerRigid.AddForce(Vector3.up * slopeForce);
                }

                //else if(horizInput < 0)
                //{
                //    playerRigid.AddForce(Vector3.up * slopeForce);
                //}

                else
                {
                    //Debug.Log("Downwards");
                    playerRigid.AddForce(-Vector3.up * slopeForce);
                }

                //Debug.Log("Downwards");
                //playerRigid.AddForce(-Vector3.up * slopeForce);
            }
        
            //Handles Horizontal slope traversal
            if (Vector3.Dot(sideVector, transform.forward) > 0)
            {
                if (horizInput > 0)
                {
                    playerRigid.AddForce(-Vector3.up * slopeForce);
                }


                else
                {
                    Debug.Log("To the Left");
                    playerRigid.AddForce(Vector3.up * slopeForce);
                }

                //Debug.Log("To the Left");
                //playerRigid.AddForce(Vector3.up * slopeForce);

            }

            else if (Vector3.Dot(sideVector, transform.forward) < 0)
            {
                if (horizInput < 0)
                {
                    playerRigid.AddForce(-Vector3.up * slopeForce);
                }

                else
                {
                    Debug.Log("To the Right");
                    playerRigid.AddForce(Vector3.up * slopeForce);
                }

                //Debug.Log("To the Right");
                //playerRigid.AddForce(Vector3.up * slopeForce);
            }

            else
            {
                //if (vertInput != 0)
                //{
                //    playerRigid.AddForce(-Vector3.up * slopeForce);
                //}

                //Debug.Log("Perpendicular");
            }
        }        
    }
}
