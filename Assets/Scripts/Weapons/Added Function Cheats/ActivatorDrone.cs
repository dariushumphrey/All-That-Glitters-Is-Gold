using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatorDrone : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerCameraScript camera;
    internal GameObject proc; //Text UI that records Cheat activity

    private float damagePercent = 75f;
    private int damageAssign;
    private GameObject adDrone; //Drone game object
    private GameObject adInstance;
    private Vector3 adOffset = new Vector3(1.3f, 1.5f, 0f);
    private Vector3 forwardDirection; //Used to determine where Camera is looking when compared to the drone
    private float characterTurnSpeed = 0.1f;
    private float droneOrbitRate = 1f;
    private float droneFollowSpeed = 2f;
    private float droneFollowAccelerant = 2f;
    private float damageReset;

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        camera = FindObjectOfType<PlayerCameraScript>();

        if(proc)
        {
            proc.GetComponent<Text>().text = "";
        }

        damageReset = damagePercent;

        if(!adInstance)
        {
            adDrone = Resources.Load<GameObject>("Game Items/adDrone");
            damagePercent /= 100;
            damagePercent *= firearm.damage;
            //damagePercent *= firearm.weaponRarity;
            damageAssign = (int)damagePercent;

            damagePercent = damageReset;

            adInstance = Instantiate(adDrone, transform.position, transform.rotation);
            adInstance.name = adDrone.name;
            adInstance.GetComponent<ADDrone>().hostWeapon = gameObject;
            adInstance.GetComponent<ADDrone>().damage = damageAssign;

            if (firearm.GetComponent<SiphonicPlatform>())
            {
                adInstance.GetComponent<ADDrone>().siphonicFlag = true;
            }

            if (firearm.GetComponent<MiningPlatform>())
            {
                adInstance.GetComponent<ADDrone>().miningFlag = true;
            }

            if (firearm.GetComponent<TrenchantPlatform>())
            {
                adInstance.GetComponent<ADDrone>().trenchantFlag = true;
            }

            if (firearm.GetComponent<CachePlatform>())
            {
                adInstance.GetComponent<ADDrone>().cacheFlag = true;
                adInstance.GetComponent<ADDrone>().fireRate = 0.2f;
            }

            if(firearm.weaponRarity == 5 && !firearm.isExotic)
            {
                adInstance.GetComponent<ADDrone>().fatedFlag = true;
            }
        }      
    }

    // Update is called once per frame
    void Update()
    {
        forwardDirection = (camera.playerCamera.transform.position - transform.position);
        if (Input.GetButton("Fire2"))
        {
            adInstance.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(camera.playerCamera.transform.forward, Vector3.up), Time.deltaTime * characterTurnSpeed);
            //adInstance.transform.eulerAngles = new Vector3(0f, camera.yaw, 0.0f);
            proc.GetComponent<Text>().text = "AD: Targeting";
        }

        else if (adInstance.GetComponent<ADDrone>().CanSeeEnemy())
        {
            adInstance.transform.rotation = Quaternion.Lerp(adInstance.transform.rotation,
                Quaternion.LookRotation(adInstance.GetComponent<ADDrone>().targetVector, Vector3.up), characterTurnSpeed);
            proc.GetComponent<Text>().text = "AD: Engaging";
        }

        else
        {
            proc.GetComponent<Text>().text = "AD: Standby";
        }
    }

    void FixedUpdate()
    {
        droneOrbitRate = Mathf.PingPong(Time.time, 1f);
        adOffset = new Vector3(1.3f, 1.5f + droneOrbitRate * 0.1f, 0f);

        adInstance.transform.position = Vector3.Lerp(adInstance.transform.position,
            camera.transform.position + (Quaternion.Euler(0f, camera.yaw, camera.pitch)) * adOffset, (droneFollowSpeed * Time.deltaTime) * droneFollowAccelerant);

        //adInstance.transform.position = firearm.transform.parent.root.transform.position + (Quaternion.Euler(0f, camera.yaw, 0f) * adOffset);
    }

    public void ActivateSiphonicPlatform()
    {
        firearm.GetComponent<SiphonicPlatform>().RemoteProc();
    }

    public void ActivateMiningPlatform()
    {
        firearm.GetComponent<MiningPlatform>().RemoteProc();
    }

    public void ActivateTrenchantPlatform()
    {
        firearm.GetComponent<TrenchantPlatform>().RemoteProc();
    }

    private void OnEnable()
    {
        Start();
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }

        if(adInstance)
        {
            adInstance.SetActive(false);
            Destroy(adInstance);
        }
    }
}
