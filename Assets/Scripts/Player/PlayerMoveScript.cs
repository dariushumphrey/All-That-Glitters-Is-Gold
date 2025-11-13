using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveScript : MonoBehaviour
{
    public int speed = 20;
    public int sprintSpeed = 22;
    public float speedDampening = -1f; //Governs time taken to slow to a stop after letting go of movement input.
    public float speedAccelerant = 0.5f; //Multiplier to exponentially increase movement speed
    public float slopeForce; //Force to apply when traversing slopes
    public float slopeCheckLength; //Length of raycast used to detect slopes
    public float airborneCheck; //Length of raycast used to check if airborne
    public float airbornePull = 0.1f; //Multipler to force Player downwards if airborne
    public float evasionUpForce = 8f;
    public float evasionForwardForce = 16f;
    public float evasionTimeout = 0.8f; //Duration to wait before ability to Evade again
    public Sprite blankReticle;
    public bool zeroGravity = false; //Governs if Player can freely move in open space; no longer zeroes movement on Y-axis if true

    public List<ParticleSystem> backThrust = new List<ParticleSystem>();
    public List<ParticleSystem> frontThrust = new List<ParticleSystem>();
    public List<ParticleSystem> rightThrust = new List<ParticleSystem>();
    public List<ParticleSystem> leftThrust = new List<ParticleSystem>();
    private PlayerInventoryScript inventory;
    private PlayerStatusScript status;
    private PlayerMeleeScript melee;
    internal Rigidbody playerRigid;
    private Camera playerCamera;
    private bool done = false;
    internal float airbornePullReset;
    internal bool sprinting = false; //Player is in the sprinting state if true
    internal bool evading = false; //Player initiated evading if true
    internal bool evaded = false; //Player is in Evasion cooldown if true
    internal float horizInput;
    internal float vertInput;
    // Start is called before the first frame update
    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        playerCamera = Camera.main;

        inventory = gameObject.GetComponent<PlayerInventoryScript>();
        status = gameObject.GetComponent<PlayerStatusScript>();
        melee = gameObject.GetComponent<PlayerMeleeScript>();


        airbornePullReset = airbornePull;
    }

    // Update is called once per frame
    void Update()
    {
        //Inititates Evasion behavior when Player is grounded and can evade again
        if (Input.GetKeyDown(KeyCode.Space) && !evading && !evaded && !Airborne())
        {
            evading = true;
            status.isInvincible = true;
            status.immunity.SetActive(true);
            status.StartCoroutine(status.CancelInvulnerable());

        }

        //Disengages sprinting if Player lets go of input or if they stop moving forward
        if (Input.GetKeyUp(KeyCode.LeftShift) && sprinting || vertInput == 0)
        {
            sprinting = false;

            if(inventory.inventory.Count >= 1)
            {
                if (!inventory.inventory[inventory.selection].GetComponent<FirearmScript>().enabled)
                {
                    inventory.inventory[inventory.selection].GetComponent<FirearmScript>().enabled = true;
                }
            }
            
            if(inventory.inventory.Count >= 1)
            {
                inventory.reticleSprite.sprite = inventory.inventory[inventory.selection].GetComponent<FirearmScript>().reticle;
            }

            if (done)
            {
                for (int p = 0; p < backThrust.Count; p++)
                {
                    var main = backThrust[p].GetComponent<ParticleSystem>().main;
                    main.loop = false;
                    backThrust[p].Stop();
                }

                done = false;
            }

            //inventory.reticleSprite.sprite = inventory.inventory[inventory.selection].GetComponent<FirearmScript>().reticle;

        }
    }

    private void FixedUpdate()
    {
        horizInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.forward * vertInput;
        Vector3 sideways = transform.right * horizInput;

        //Disables gravity and zeroes downward pull during Melees or Zero Gravity play
        if(melee.meleeLock || zeroGravity)
        {
            playerRigid.useGravity = false;
            airbornePull = 0f;
        }

        //Otherwise, leaves gravity, downward pull unchanged
        else
        {
            forward.y = 0f;
            sideways.y = 0f;
            playerRigid.useGravity = true;
            airbornePull = airbornePullReset;
        }     

        if (Input.GetButton("Horizontal"))
        {
            playerRigid.AddForce(sideways * speed * speedAccelerant);
                           
        }

        if (Input.GetButton("Vertical"))
        {
            //Applies additional force if Player is sprinting. Otherwise, applies standard force
            if (Input.GetKey(KeyCode.LeftShift) && vertInput == 1 && !Airborne())
            {
                sprinting = true;
                playerRigid.AddForce(forward * sprintSpeed * speedAccelerant);

                if(!inventory.inventory[inventory.selection].GetComponent<FirearmScript>().isReloading)
                {
                    inventory.inventory[inventory.selection].GetComponent<FirearmScript>().enabled = false;
                }

                inventory.reticleSprite.sprite = blankReticle;

                if(!done)
                {
                    for(int p = 0; p < backThrust.Count; p++)
                    {
                        var main = backThrust[p].GetComponent<ParticleSystem>().main;
                        main.loop = true;
                        backThrust[p].Play();
                    }

                    done = true;
                }               

            }

            else
            {
                playerRigid.AddForce(forward * speed * speedAccelerant);              
            }
        }

        if(Airborne())
        {
            //Provides controls, VFX visuals for Zero Gravity movement. Otherwise, provides downward force if Player is airborne
            if(zeroGravity)
            {
                if(Input.GetKey(KeyCode.Z))
                {
                    playerRigid.AddForce(transform.up * speed * speedAccelerant);
                }

                if (Input.GetKey(KeyCode.C))
                {
                    playerRigid.AddForce(-transform.up * speed * speedAccelerant);
                }

                if(vertInput == 1)
                {
                    for (int p = 0; p < backThrust.Count; p++)
                    {
                        backThrust[p].Play();
                    }
                }

                else
                {
                    for (int p = 0; p < backThrust.Count; p++)
                    {
                        backThrust[p].Stop();
                    }
                }

                if (vertInput == -1)
                {
                    for (int p = 0; p < frontThrust.Count; p++)
                    {
                        frontThrust[p].Play();
                    }
                }

                else
                {
                    for (int p = 0; p < frontThrust.Count; p++)
                    {
                        frontThrust[p].Stop();
                    }
                }

                if (horizInput == 1)
                {
                    leftThrust[0].Play();
                }

                else
                {
                    leftThrust[0].Stop();
                }

                if (horizInput == -1)
                {
                    rightThrust[0].Play();
                }

                else
                {
                    rightThrust[0].Stop();
                }
            }

            else
            {
                playerRigid.AddForce(-Vector3.up * airbornePull);
            }
        }

        //Slows Player-character movement to zero if Player stops moving
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

        if(evading)
        {
            playerRigid.velocity = Vector3.zero;

            //Not moving or moving backwards will make the Player dodge backwards
            if (horizInput == 0 && vertInput == 0 || vertInput < 0)
            {
                playerRigid.AddForce(-transform.forward * evasionForwardForce, ForceMode.Impulse);
                playerRigid.AddForce(Vector3.up * evasionUpForce, ForceMode.Impulse);

                for (int p = 0; p < frontThrust.Count; p++)
                {
                    frontThrust[p].Play();
                }
            }

            //Moving forward will make the Player dodge forwards
            if (vertInput > 0)
            {
                playerRigid.AddForce(transform.forward * evasionForwardForce, ForceMode.Impulse);
                playerRigid.AddForce(Vector3.up * evasionUpForce, ForceMode.Impulse);

                for (int p = 0; p < backThrust.Count; p++)
                {
                    backThrust[p].Play();
                }
            }

            //Moving left will make the Player dodge left
            if (horizInput < 0)
            {
                playerRigid.AddForce(-transform.right * evasionForwardForce, ForceMode.Impulse);
                playerRigid.AddForce(Vector3.up * evasionUpForce, ForceMode.Impulse);

                for (int p = 0; p < rightThrust.Count; p++)
                {
                    rightThrust[p].Play();
                }
            }

            //Moving right will make the Player dodge right
            if (horizInput > 0)
            {
                playerRigid.AddForce(transform.right * evasionForwardForce, ForceMode.Impulse);
                playerRigid.AddForce(Vector3.up * evasionUpForce, ForceMode.Impulse);

                for (int p = 0; p < leftThrust.Count; p++)
                {
                    leftThrust[p].Play();
                }
            }

            evaded = true;
            StartCoroutine(ResetEvade());

            evading = false;
        }
    }

    /// <summary>
    /// Returns true if Player is on slope; returns false if Player is on flat ground
    /// </summary>
    /// <returns></returns>
    private bool OnSlope()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, slopeCheckLength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if Player is airborne; returns false if Player is grounded
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Applies Rigidbody force up/down/sides of slopes dependent on Dot Product result
    /// </summary>
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
            }

            else if (Vector3.Dot(hit.normal, transform.forward) > 0)
            {
                if (vertInput < 0)
                {
                    playerRigid.AddForce(Vector3.up * slopeForce);
                }

                else
                {
                    //Debug.Log("Downwards");
                    playerRigid.AddForce(-Vector3.up * slopeForce);
                }
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
                    //Debug.Log("To the Left");
                    playerRigid.AddForce(Vector3.up * slopeForce);
                }
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
        }        
    }

    private IEnumerator ResetEvade()
    {
        yield return new WaitForSeconds(evasionTimeout);
        evaded = false;
    }
}
