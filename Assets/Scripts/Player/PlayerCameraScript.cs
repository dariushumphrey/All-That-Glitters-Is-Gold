using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCameraScript : MonoBehaviour
{
    public float rotateH = 2.0f;
    public float rotateV = 2.0f;
    public float vClamp = 30f;
    public float characterTurnSpeed = 100f;
    public float zoomDefault = 60f;
    public float zoomMax = 40f;
    public float zoomSpeed = 0.2f;
    public float collideCheck = 3.0f; //Governs Ray length to check for threats of surface clipping
    public float offsetMult = 2.0f; //Governs additional distance for Camera offset from surface
    public float offsetPushZ = 1.0f; //Governed physical pushing of Camera in the 'Z' direction of its resting position
    public float offsetPushX = 1.0f; //Governed physical pushing of Camera in the 'X' direction of its resting position
    public Vector3 cameraPosition; //Governs Camera offset position from Player-Character
    public Transform offsetCheckPos; //Empty GameObject, used to cast Rays from when checking surface collision
    public Image reticleSprite;

    private float zoomReset = 60f;
    private Vector3 forwardDirection; //Used to determine where Camera is looking when compared to the body
    private Vector3 camPosReset;
    private Vector3 rayOrigin;
    private Vector3 distance; //Used to determine where Enemy is when compared to center of screen; used for Aim Assist
    private RaycastHit hit;
    private Camera playerCamera;
    private PlayerInventoryScript player;
    private PlayerMoveScript move;
    private PlayerMeleeScript melee;
    internal float yaw = 0.0f;
    internal float pitch = 0.0f;
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
    }

    // Update is called once per frame
    void Update()
    {
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
            yaw += rotateH * Input.GetAxis("Mouse X");
            pitch -= rotateV * Input.GetAxis("Mouse Y");
            pitch = Mathf.Clamp(pitch, -vClamp, vClamp);

            playerCamera.transform.position = transform.position + (Quaternion.Euler(pitch, yaw, 0) * cameraPosition);
            playerCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

            forwardDirection = (playerCamera.transform.position - transform.position);
            if (move.horizInput != 0 || move.vertInput != 0 || Input.GetButton("Fire1"))
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(playerCamera.transform.forward, Vector3.up), Time.deltaTime * characterTurnSpeed);
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(pitch, yaw, 0), Time.deltaTime * characterTurnSpeed);
            }

            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(pitch, yaw, 0), Time.deltaTime * characterTurnSpeed);

            Zoom();
            CollisionCheck();
            AimAssistance();
            MeleeAssistance();
        }      
    }

    private void Zoom()
    {
        playerCamera.fieldOfView = zoomDefault;

        if (Input.GetButton("Fire2"))
        {
            zoomDefault -= zoomSpeed * Time.deltaTime;
            if (zoomDefault <= zoomMax)
            {
                zoomDefault = zoomMax;
            }
        }

        else
        {
            zoomDefault += zoomSpeed * Time.deltaTime;
            if (zoomDefault >= zoomReset)
            {
                zoomDefault = zoomReset;
            }
        }
    }

    private void CollisionCheck()
    {
        Vector3 offset;
        RaycastHit hit;
        if (Physics.Raycast(offsetCheckPos.transform.position, offsetCheckPos.forward, out hit, collideCheck)   ||
            Physics.Raycast(offsetCheckPos.transform.position, -offsetCheckPos.right, out hit, collideCheck) ||
            Physics.Raycast(offsetCheckPos.transform.position, offsetCheckPos.right, out hit, collideCheck) /* ||
            Physics.Raycast(playerCamera.transform.position, -playerCamera.transform.forward, out hit, collideCheck) ||
            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.right, out hit, collideCheck) ||
            Physics.Raycast(playerCamera.transform.position, -playerCamera.transform.right, out hit, collideCheck)*/)
        {
            if (hit.point != null)
            {
                if(hit.collider.tag == "Projectile" || hit.collider.tag == "Enemy" || hit.collider.tag == "Lucent" || hit.collider.tag == "Ammo" || hit.collider.tag == "Corpse")
                {
                    //Do nothing
                }
                
                else
                {
                    //offset = hit.normal * offsetMult;
                    //playerCamera.transform.position = hit.point + offset + (transform.rotation * cameraPosition);
                    cameraPosition.z = -hit.distance * offsetPushZ;
                    cameraPosition.x = hit.distance * offsetPushX;
                    //Debug.DrawLine(offsetCheckPos.transform.position, hit.point, Color.red);
                }
            }           
        }      

        else
        {
            cameraPosition = camPosReset;
        }
    }  

    private void AimAssistance()
    {
        rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        if(player.selection != -1)
        {
            if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, player.inventory[player.selection].GetComponent<FirearmScript>().range, 7))
            {
                if (hit.collider.tag == "Enemy")
                {
                    distance = (hit.transform.position - rayOrigin).normalized;

                    if (distance.magnitude <= player.inventory[player.selection].GetComponent<FirearmScript>().effectiveRange)
                    {
                        playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, Quaternion.LookRotation(distance), player.inventory[player.selection].GetComponent<FirearmScript>().aimAssistStrength);
                        reticleSprite.color = Color.red;
                    }                  

                }

                else
                {
                    reticleSprite.color = Color.white;
                }
            }
        }           
    }

    private void MeleeAssistance()
    {
        rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, Mathf.Infinity))
        {
            if(hit.collider.tag == "Enemy")
            {
                melee.meleeTarget = hit.collider.gameObject;
                
            }
            
            else
            {
                melee.meleeTarget = null;
            }
        }

        if(melee.meleeTarget != null)
        {
            if (melee.meleeTarget.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
            {
                melee.meleeTarget = null;
            }
        }       
    }
}
