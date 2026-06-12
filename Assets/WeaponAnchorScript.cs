using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnchorScript : MonoBehaviour
{
    public float actionSpeed = 3f;
    public Vector3 restPosition = Vector3.zero;
    public Vector3 restRotation = Vector3.zero;

    public Vector3 recoilPosition;
    public Vector3 sprintingPosition;
    public Vector3 sprintingRotation;
    public Vector3 idleHolsterPosition;
    public Vector3 idleHolsterRotation;
    public Vector3 meleeLockRotation;
    public Vector3 guardLockPosition;
    public Vector3 guardLockRotation;

    public Transform move;
    private float repeatTimer = 0f;
    private float idleTimer = 0f;
    private float idleStop = 0f;
    private bool recoilToRest = false;
    private bool repeatTimeout = false;
    private bool sprintToRest = false;
    private bool idleToReady = false;
    private bool idlePause = false;
    internal bool restorePoseFromMelee;
    internal bool restorePoseFromGuard;
    internal float fireRateInsert;
    internal float reloadSpeedInsert;
    // Start is called before the first frame update
    void Start()
    {
        move = gameObject.transform.parent;   
    }

    // Update is called once per frame
    void Update()
    {
        if(repeatTimeout)
        {
            repeatTimer += Time.deltaTime;
            if(repeatTimer >= fireRateInsert)
            {
                repeatTimer = 0f;
                repeatTimeout = false;
            }
        }

        if(recoilToRest)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, restPosition, (actionSpeed * Time.deltaTime));
            if(transform.localPosition == restPosition)
            {
                recoilToRest = false;
            }
        }

        if(move.GetComponent<PlayerMoveScript>().sprinting)
        {
            if (transform.localPosition != sprintingPosition && transform.localRotation != Quaternion.Euler(sprintingRotation))
            {
                Quaternion rotation = Quaternion.Euler(sprintingRotation);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * actionSpeed);
                transform.localPosition = Vector3.Lerp(transform.localPosition, sprintingPosition, (actionSpeed * Time.deltaTime));
            }

            else
            {
                sprintToRest = true;
            }
        }

        else
        {
            if(sprintToRest)
            {
                if (transform.localPosition != restPosition && transform.localRotation != Quaternion.Euler(restRotation))
                {
                    Quaternion rotation = Quaternion.Euler(restRotation);
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * actionSpeed);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, restPosition, (actionSpeed * Time.deltaTime));
                }

                else
                {
                    sprintToRest = false;
                }
            }
        }

        if(!move.GetComponent<PlayerCameraScript>().activeAction || move.GetComponent<PlayerMeleeScript>().meleeLock || move.GetComponent<PlayerMeleeScript>().guarding)
        {
            if(!idlePause)
            {
                idleTimer += Time.deltaTime;
            }

            if(idleTimer >= 3f)
            {
                idleTimer = 3f;
                idleToReady = true;
            }
        }

        else
        {
            idleTimer = 0f;
            idleToReady = false;
        }

        if(idlePause)
        {
            idleStop += Time.deltaTime;
            if(idleStop >= reloadSpeedInsert)
            {
                idleStop = 0f;
                idlePause = false;
            }
        }

        if(idleToReady)
        {
            if (transform.localPosition != idleHolsterPosition && transform.localRotation != Quaternion.Euler(idleHolsterRotation))
            {
                Quaternion rotation = Quaternion.Euler(idleHolsterRotation);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * actionSpeed);
                transform.localPosition = Vector3.Lerp(transform.localPosition, idleHolsterPosition, (actionSpeed * Time.deltaTime));
            }
        }

        else
        {
            if (transform.localPosition != restPosition && transform.localRotation != Quaternion.Euler(restRotation))
            {
                Quaternion rotation = Quaternion.Euler(restRotation);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * actionSpeed);
                transform.localPosition = Vector3.Lerp(transform.localPosition, restPosition, (actionSpeed * Time.deltaTime));
            }
        }

        if (move.GetComponent<PlayerMeleeScript>().meleeLock)
        {
            transform.localRotation = Quaternion.Euler(meleeLockRotation);
        }

        if (restorePoseFromMelee)
        {
            if (transform.localRotation != Quaternion.Euler(restRotation))
            {
                Quaternion rotation = Quaternion.Euler(restRotation);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * actionSpeed);
            }

            else
            {
                restorePoseFromMelee = false;
            }
        }

        if (move.GetComponent<PlayerMeleeScript>().guarding)
        {
            transform.localPosition = guardLockPosition;
            transform.localRotation = Quaternion.Euler(guardLockRotation);

            idleTimer = 0f;
            idleToReady = false;
        }

        if (restorePoseFromGuard)
        {
            if (transform.localRotation != Quaternion.Euler(restRotation) && transform.localPosition != restPosition)
            {
                Quaternion rotation = Quaternion.Euler(restRotation);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * actionSpeed);
                transform.localPosition = Vector3.Lerp(transform.localPosition, restPosition, (actionSpeed * Time.deltaTime));
            }

            else
            {
                restorePoseFromGuard = false;
            }
        }
    }

    public void SimulateRecoil()
    {
        if(!repeatTimeout)
        {
            transform.localPosition = recoilPosition;
            recoilToRest = true;

            //idleTimer = 0f;
            //idleToReady = false;

            repeatTimeout = true;
        }
    }

    public void PauseIdleTimerForReload()
    {
        idlePause = true;
    }
}
