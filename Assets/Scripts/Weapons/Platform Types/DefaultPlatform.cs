using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlatform : MonoBehaviour
{
    private float damagePercent = 0f;
    private float fireRatePercent = 0f;
    private float recoilPercent = 0f;
    private float aimAssistPercent = 0f;
    private float cameraZoomPercent = 100f;
    private float cameraZoomNew, cameraZoomOld;

    private FirearmScript firearm;
    private PlayerCameraScript cam;

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();

        if(!firearm.display)
        {
            cam = FindObjectOfType<PlayerCameraScript>();
        }

        damagePercent /= 100f;
        damagePercent *= firearm.damage;
        firearm.damage += (int)damagePercent;

        fireRatePercent /= 100f;
        fireRatePercent *= firearm.fireRate;
        firearm.fireRate -= fireRatePercent;

        recoilPercent /= 100f;
        recoilPercent *= firearm.wepRecoil;
        firearm.wepRecoil += recoilPercent;

        aimAssistPercent /= 100f;
        aimAssistPercent *= firearm.aimAssistStrength;
        firearm.aimAssistStrength += aimAssistPercent;

        cameraZoomOld = 40f;

        cameraZoomPercent /= 100f;
        cameraZoomPercent *= cameraZoomOld;
        cameraZoomNew = cameraZoomPercent;
    }

    // Update is called once per frame
    void Update()
    {
        if(cam)
        {
            cam.zoomMax = cameraZoomNew;
        }
    }
}
