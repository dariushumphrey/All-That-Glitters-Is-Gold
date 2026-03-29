using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrenchantPlatform : MonoBehaviour
{
    internal bool confirmedHit = false;
    internal bool confirmedMeleeHit = false;
    internal GameObject enemy;

    private float damagePercent = 5f;
    private float cameraZoomPercent = 100f;
    private float cameraZoomNew, cameraZoomOld;

    private FirearmScript firearm;
    private PlayerCameraScript cam;
    private PlayerMoveScript move;

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();     
        cam = FindObjectOfType<PlayerCameraScript>();
        move = FindObjectOfType<PlayerMoveScript>();

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
        cam.zoomMax = cameraZoomNew;

        if(confirmedHit)
        {
            if(!enemy.GetComponent<DebuffScript>())
            {
                enemy.AddComponent<DebuffScript>();
                enemy = null;         
            }

            else
            {
                enemy = null;
            }

            confirmedHit = false;
        }

        if(confirmedMeleeHit)
        {
            if (!enemy.GetComponent<DamageOverTimeScript>())
            {
                enemy.AddComponent<DamageOverTimeScript>();
                enemy.GetComponent<DamageOverTimeScript>().dotDamage = 875;
                enemy.GetComponent<DamageOverTimeScript>().damageOverTimeLength = 3f;
                enemy = null;
            }

            else
            {
                enemy = null;
            }

            confirmedMeleeHit = false;
        }
        
        if(move.evading)
        {
            Vector3 epicenter = transform.position;
            Collider[] affected = Physics.OverlapSphere(transform.position, 5f);
            foreach (Collider hit in affected)
            {
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    if (!hit.gameObject.GetComponent<SlowedScript>())
                    {
                        hit.gameObject.AddComponent<SlowedScript>();
                    }
                }
            }
        }
    }

    public void RemoteProc()
    {
        if (!enemy.GetComponent<DebuffScript>())
        {
            enemy.AddComponent<DebuffScript>();
            enemy = null;
        }

        else
        {
            enemy = null;
        }
    }
}
