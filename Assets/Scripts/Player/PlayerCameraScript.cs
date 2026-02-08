using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCameraScript : MonoBehaviour
{
    public float rotateH = 2.0f; //Strength of horizontal rotation
    public float rotateV = 2.0f; //Strength of vertical rotation
    public float vClamp = 30f; //Extent of up/down rotation
    public float characterTurnSpeed = 100f;
    public float zoomDefault = 60f;
    public float zoomMax = 40f;
    public float zoomSpeed = 0.2f;
    public float collideCheck = 3.0f; //Governs Ray length to check for threats of surface clipping
    public float offsetMult = 2.0f; //Governs additional distance for Camera offset from surface
    public float offsetPushZ = 1.0f; //Governs physical pushing of Camera in the 'Z' direction of offset point
    public float offsetPushY = 1.0f; //Governs physical pushing of Camera in the 'Y' direction of offset point
    public Vector3 cameraPosition; //Governs Camera offset position from Player-Character
    public Transform offsetCheckPos; //Empty GameObject, used to cast Rays from when checking surface collision
    public Image reticleSprite;
    public Image meleeReticle;
    public Sprite meleeSprite;

    //contactOnly - LayerMask that only interacts with Enemies and Surfaces; used for Aim Assist, Melee attacks and Canvas reveals
    //cameraOnly - LayerMask that only interacts with Surfaces; used for Camera clipping checks
    public LayerMask contactOnly, cameraOnly;

    private float zoomReset;
    private Vector3 forwardDirection; //Used to determine where Camera is looking when compared to the body
    private Vector3 camPosReset; //Starting position of Camera 
    private Vector3 rayOrigin;
    private Vector3 distance; //Used to determine length between Enemy and screen center; used for Aim Assist
    private RaycastHit hit;
    internal Camera playerCamera;
    private PlayerInventoryScript player;
    private PlayerMoveScript move;
    private PlayerMeleeScript melee;
    internal float yaw = 0.0f; //Value that rotates Player, Camera on Y-axis
    internal float pitch = 0.0f; //Value that rotates Player, Camera on X-axis

    // Start is called before the first frame update
    void Start()
    {
        camPosReset = cameraPosition;
        playerCamera = Camera.main;
        player = FindObjectOfType<PlayerInventoryScript>();
        move = FindObjectOfType<PlayerMoveScript>();
        melee = FindObjectOfType<PlayerMeleeScript>();
        playerCamera.fieldOfView = zoomDefault;
        zoomReset = zoomDefault;
        meleeReticle.sprite = move.blankReticle;

    }

    // Update is called once per frame
    void Update()
    {
        //Locks the cursor on number inputs
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }

    private void LateUpdate()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        else
        {
            //Controls Camera rotation
            yaw += rotateH * Input.GetAxis("Mouse X");
            pitch -= rotateV * Input.GetAxis("Mouse Y");
            pitch = Mathf.Clamp(pitch, -vClamp, vClamp);

            //Offsets Camera around Player shoulder if camera clipping protections are inactive
            if(!SurfaceIntersection())
            {
                playerCamera.transform.position = transform.position + (Quaternion.Euler(pitch, yaw, 0) * cameraPosition);
                playerCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

                //Debug.DrawRay(offsetCheckPos.transform.position, (playerCamera.transform.position - offsetCheckPos.transform.position).normalized * slider, Color.blue);
            }

            //Rotates Player-character when moving, firing, or throwing grenades
            forwardDirection = (playerCamera.transform.position - transform.position);
            if (move.horizInput != 0 || move.vertInput != 0 || Input.GetButton("Fire1") || player.throwing)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(playerCamera.transform.forward, Vector3.up), Time.deltaTime * characterTurnSpeed);
            }

            //Rotates Player-character every frame towards Camera forward direction
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(pitch, yaw, 0), Time.deltaTime * characterTurnSpeed);

            Zoom();
            CameraClipCountermeasure();
            AimAssistance();

            if(!melee.meleeLock)
            {
                MeleeAssistance();
            }
        }      
    }

    /// <summary>
    /// Controls Camera zooming and Canvas (Player, Enemy) reveals
    /// </summary>
    private void Zoom()
    {
        playerCamera.fieldOfView = zoomDefault;

        if (Input.GetButton("Fire2"))
        {
            //Zooms camera until it reaches max zoom
            zoomDefault -= zoomSpeed * Time.deltaTime;
            if (zoomDefault <= zoomMax)
            {
                zoomDefault = zoomMax;
            }

            //Rotates Player-character in Camera forward direction
            forwardDirection = (playerCamera.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(playerCamera.transform.forward, Vector3.up), Time.deltaTime * characterTurnSpeed);

            //Reveals Enemy canvas when zooming into them
            rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, Mathf.Infinity, contactOnly))
            {
                if(hit.collider.tag == "Enemy")
                {
                    if(hit.collider.gameObject.GetComponent<EnemyHealthScript>().visual.enabled == false)
                    {
                        hit.collider.gameObject.GetComponent<EnemyHealthScript>().visual.gameObject.SetActive(true);
                        hit.collider.gameObject.GetComponent<EnemyHealthScript>().canvasTimer = hit.collider.gameObject.GetComponent<EnemyHealthScript>().canvasTimerReset;

                    }

                    else
                    {
                        hit.collider.gameObject.GetComponent<EnemyHealthScript>().visual.gameObject.SetActive(true);
                        hit.collider.gameObject.GetComponent<EnemyHealthScript>().canvasTimer = hit.collider.gameObject.GetComponent<EnemyHealthScript>().canvasTimerReset;
                    }
                }
            }


            //Reveals Weapon canvas
            player.weaponAmmoPage.gameObject.SetActive(true);
            //player.weaponLoad.gameObject.SetActive(true);
            player.lucentText.gameObject.SetActive(true);
            player.grenadeText.gameObject.SetActive(true);
            player.wepStateTimer = player.wepStateTimerReset;
        }

        //Restores Camera zoom to default setting
        else
        {
            zoomDefault += zoomSpeed * Time.deltaTime;
            if (zoomDefault >= zoomReset)
            {
                zoomDefault = zoomReset;
            }
        }
    }

    /// <summary>
    /// Offsets the Camera from an intersecting surface if the raycast between the Player and Camera position registers a hit
    /// </summary>
    private void CameraClipCountermeasure()
    {
        Vector3 offset;
        RaycastHit hit;
        if (Physics.Raycast(offsetCheckPos.transform.position, (playerCamera.transform.position - offsetCheckPos.transform.position).normalized, out hit, collideCheck, cameraOnly))
        {
            if (hit.point != null)
            {
                if(hit.collider.tag == "Projectile" || hit.collider.tag == "Enemy" || hit.collider.tag == "Lucent" || hit.collider.tag == "Ammo" || hit.collider.tag == "Corpse")
                {
                    //Do nothing
                }
                
                else
                {
                    //hit.normal = new Vector3(0, offsetPushY, offsetPushZ);
                    offset = hit.point + (hit.normal + new Vector3(0, offsetPushY, offsetPushZ));
                    playerCamera.transform.position = offset * offsetMult;

                    //cameraPosition.z = -hit.distance * offsetPushZ;
                    //cameraPosition.x = hit.distance * offsetPushX;
                    //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(playerCamera.transform.forward, Vector3.up), Time.deltaTime * characterTurnSpeed);

                    Debug.DrawRay(offsetCheckPos.transform.position, (playerCamera.transform.position - offsetCheckPos.transform.position).normalized * collideCheck, Color.yellow);
                    Debug.DrawLine(hit.point, offset * offsetMult, Color.red);
                    //Debug.DrawRay(hit.point, hit.normal + new Vector3(offsetPushX, 0, 0), Color.red);
                    //Debug.DrawRay(hit.point, hit.normal + new Vector3(0, 0, offsetPushZ), Color.blue);
                    //Debug.DrawRay(hit.point, hit.normal + new Vector3(0, offsetPushY, 0), Color.green);
                    //Debug.DrawLine(transform.position, playerCamera.transform.position, Color.blue);

                }
            }                  
        }      
    }
    
    /// <summary>
    /// Returns true if raycast detects a surface between Player and Camera position; returns false if no hit is detected
    /// </summary>
    /// <returns></returns>
    public bool SurfaceIntersection()
    {      
        if (Physics.Raycast(offsetCheckPos.transform.position, (playerCamera.transform.position - offsetCheckPos.transform.position).normalized, out hit, collideCheck, cameraOnly))
        {
            if (hit.point != null)
            {
                if (hit.collider.tag == "Projectile" || hit.collider.tag == "Enemy" || hit.collider.tag == "Lucent" || hit.collider.tag == "Ammo" || hit.collider.tag == "Corpse")
                {
                    //Do nothing
                }

                return true;
            }       
        }

        return false;
    }

    /// <summary>
    /// Subtly rotates Camera towards Enemy if distance between them and screen center is less than a Weapon's effective range
    /// Changes color of reticle between red and white dependent on satisfied condition
    /// </summary>
    private void AimAssistance()
    {
        rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        if(player.selection != -1)
        {
            if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, Mathf.Infinity, contactOnly))
            {
                if (hit.collider.tag == "Enemy")
                {
                    distance = (hit.transform.position - rayOrigin);
                    if (distance.magnitude <= player.inventory[player.selection].GetComponent<FirearmScript>().effectiveRange)
                    {
                        playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, Quaternion.LookRotation(distance), player.inventory[player.selection].GetComponent<FirearmScript>().aimAssistStrength);
                        reticleSprite.color = Color.red;
                    }

                    else
                    {
                        reticleSprite.color = Color.white;
                    }

                }

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Surface"))
                {
                    reticleSprite.color = Color.white;
                }
            }
        }           
    }

    /// <summary>
    /// Assigns a Melee target if raycast detects an Enemy.
    /// Removes Enemy as target if raycast does not detect an Enemy.
    /// Overlays a Melee reticle onto an enemy when in range to be attacked.
    /// </summary>
    private void MeleeAssistance()
    {
        rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, melee.meleeRange, contactOnly))
        {
            if (hit.collider.tag == "Enemy" || hit.collider.tag == "Combustible Lucent")
            {
                melee.meleeTarget = hit.collider.gameObject;

                meleeReticle.sprite = meleeSprite;
                meleeReticle.color = Color.yellow;
                Vector3 reticlePos = Camera.main.WorldToScreenPoint(melee.meleeTarget.transform.position);
                meleeReticle.rectTransform.position = reticlePos;
                
            }          
        }

        else
        {
            melee.meleeTarget = null;
            meleeReticle.color = Color.white;
            meleeReticle.sprite = move.blankReticle;
        }

        if (melee.meleeTarget != null)
        {
            if(melee.meleeTarget.GetComponent<EnemyHealthScript>())
            {
                if(melee.meleeTarget.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                {
                    melee.meleeTarget = null;
                    meleeReticle.sprite = move.blankReticle;
                    meleeReticle.color = Color.white;
                }
            }
        }       
    }
}
