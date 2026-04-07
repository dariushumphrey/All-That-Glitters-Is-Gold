using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CachePlatform : MonoBehaviour
{
    private float regenerateTimer = 2f;
    private float regenerateReset;
    private float damagePercent = 5f;
    private float cameraZoomPercent = 100f;
    private float cameraZoomNew, cameraZoomOld;

    private FirearmScript firearm;
    private PlayerCameraScript cam;
    private PlayerInventoryScript inventory;

    // Start is called before the first frame update
    public void Start()
    {
        regenerateReset = regenerateTimer;

        firearm = GetComponent<FirearmScript>();

        if (!firearm.display)
        {
            cam = FindObjectOfType<PlayerCameraScript>();
            inventory = FindObjectOfType<PlayerInventoryScript>();
        }

        damagePercent /= 100f;
        damagePercent *= firearm.damage;
        firearm.damage += (int)damagePercent;

        cameraZoomOld = 40f;
        cameraZoomPercent /= 100f;
        cameraZoomPercent *= cameraZoomOld;
        cameraZoomNew = cameraZoomPercent;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam)
        {
            cam.zoomMax = cameraZoomNew;
        }

        if(!firearm.display)
        {
            regenerateTimer -= Time.deltaTime;
            if (regenerateTimer <= 0f)
            {
                regenerateTimer = regenerateReset;

                inventory.fogGrenadeCharges++;
                inventory.solGrenadeCharges++;
                inventory.desGrenadeCharges++;
            }
        }
    }
}
