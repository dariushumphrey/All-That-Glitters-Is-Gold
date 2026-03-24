using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningPlatform : MonoBehaviour
{
    internal bool confirmedHit = false;

    private float damagePercent = 5f;
    private float cameraZoomPercent = 100f;
    private float cameraZoomNew, cameraZoomOld;

    private float lucentPercent = 1f;
    private int lucentAdd;

    private FirearmScript firearm;
    private PlayerInventoryScript inventory;
    private PlayerCameraScript cam;
    private GameObject cluster;
    internal Vector3 clusterPosition; //Lucent Cluster spawn position

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        inventory = FindObjectOfType<PlayerInventoryScript>();
        cam = FindObjectOfType<PlayerCameraScript>();
        cluster = Resources.Load<GameObject>("Game Items/testLucent");

        damagePercent /= 100f;
        damagePercent *= firearm.damage;
        firearm.damage += (int)damagePercent;

        lucentPercent /= 100;
        lucentPercent *= inventory.lucentFunds;
        lucentAdd = (int)lucentPercent;

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
            GameObject lucent = Instantiate(cluster, clusterPosition, transform.rotation);
            lucent.name = cluster.name;
            lucent.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            lucent.GetComponent<LucentScript>().shatterDelayTime = 0f;
            //lucent.GetComponent<LucentScript>().lucentGift = (int)damagePercent;
            lucent.GetComponent<LucentScript>().lucentGift *= firearm.weaponRarity;
            lucent.GetComponent<LucentScript>().ShatterCalculation();
            lucent.GetComponent<LucentScript>().StartCoroutine(lucent.GetComponent<LucentScript>().Shatter());

            //inventory.lucentFunds += lucentAdd;
            //if(inventory.lucentFunds >= 100000)
            //{
            //    inventory.lucentFunds = 100000;
            //}

            confirmedHit = false;
        }
    }
}
