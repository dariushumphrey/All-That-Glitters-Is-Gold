using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LauncherFirearm : FirearmScript
{
    public GameObject munition;
    public float shotForceForward, shotForceUp;

    internal List<GameObject> detectedTargets = new List<GameObject>();
    public override void FireWeapon()
    {
        fireAgain += Time.deltaTime;

        if(Input.GetButton("Fire1") && currentAmmo >= 1 && fireAgain >= fireRate && !isReloading)
        {
            //Firing timer resets, Ammo decrements/ records number of shots
            fireAgain = 0.0f;
            currentAmmo--;
            ammoSpent++;

            //Recoil
            gunCam.transform.eulerAngles = new Vector3(GetComponentInParent<PlayerCameraScript>().pitch -=
                Random.Range(-wepRecoil, wepRecoil), GetComponentInParent<PlayerCameraScript>().yaw +=
                Random.Range(-wepRecoil, wepRecoil), 0.0f);

            //Prevents value that tracks times weapon has fired to be more than the total magazine size
            if (ammoSpent >= ammoSize)
            {
                ammoSpent = ammoSize;
            }

            RaycastHit hit;
            Vector3 shotVector;
            Vector3 estLandingPos;

            Ray ray = gunCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out hit, effectiveRange, contactOnly))
            {
                shotVector = hit.point;
            }

            else
            {
                shotVector = ray.GetPoint(range);
            }

            estLandingPos = (shotVector - barrel.transform.position).normalized;

            GameObject launched = Instantiate(munition, barrel.transform.position, barrel.transform.rotation * Quaternion.Euler(90f, 0f, 0f));
            launched.name = munition.name;

            launched.GetComponent<MunitionScript>().hostLauncher = gameObject;
            launched.GetComponent<MunitionScript>().explosiveDamage = damage;

            if(isExotic)
            {
                launched.GetComponent<MunitionScript>().isExoticMunition = true;
                launched.GetComponent<MunitionScript>().isMine = true;
            }

            launched.GetComponent<Rigidbody>().AddForce(estLandingPos * shotForceForward + Vector3.up * shotForceUp, ForceMode.VelocityChange);


            
            muzzleFlash.Play();          
        }
    }
}
