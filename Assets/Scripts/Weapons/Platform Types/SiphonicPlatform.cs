using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiphonicPlatform : MonoBehaviour
{
    internal bool confirmedHit = false;
    internal bool confirmedMeleeKill = false;

    private float damagePercent = 5f;
    private float cameraZoomPercent = 100f;
    private float cameraZoomNew, cameraZoomOld;

    private float healthPercent = 1f;
    private int healthAdd;
    private float shieldPercent = 1f;
    private int shieldAdd;
    private int meleeMultiplier = 15;

    private FirearmScript firearm;
    private PlayerStatusScript status;
    private PlayerCameraScript cam;

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        status = FindObjectOfType<PlayerStatusScript>();
        cam = FindObjectOfType<PlayerCameraScript>();

        damagePercent /= 100f;
        damagePercent *= firearm.damage;
        firearm.damage += (int)damagePercent;

        healthPercent /= 100;
        healthPercent *= status.playerHealthMax;
        healthAdd = (int)healthPercent;

        shieldPercent /= 100;
        shieldPercent *= status.playerShieldMax;
        shieldAdd = (int)shieldPercent;

        cameraZoomOld = 40f;

        cameraZoomPercent /= 100f;
        cameraZoomPercent *= cameraZoomOld;
        cameraZoomNew = cameraZoomPercent;

    }

    // Update is called once per frame
    void Update()
    {
        cam.zoomMax = cameraZoomNew;

        if (confirmedHit)
        {
            status.playerHealth += healthAdd;
            if(status.playerHealth >= status.playerHealthMax)
            {
                status.playerHealth = status.playerHealthMax;
            }

            status.playerShield += shieldAdd;
            if (status.playerShield >= status.playerShieldMax)
            {
                status.playerShield = status.playerShieldMax;
            }

            confirmedHit = false;
        }

        if(confirmedMeleeKill)
        {
            status.playerHealth += healthAdd * meleeMultiplier;
            if (status.playerHealth >= status.playerHealthMax)
            {
                status.playerHealth = status.playerHealthMax;
            }

            status.playerShield += shieldAdd * meleeMultiplier;
            if (status.playerShield >= status.playerShieldMax)
            {
                status.playerShield = status.playerShieldMax;
            }

            confirmedMeleeKill = false;
        }
    }

    public void RemoteProc()
    {
        status.playerHealth += healthAdd;
        if (status.playerHealth >= status.playerHealthMax)
        {
            status.playerHealth = status.playerHealthMax;
        }

        status.playerShield += shieldAdd;
        if (status.playerShield >= status.playerShieldMax)
        {
            status.playerShield = status.playerShieldMax;
        }
    }
}
