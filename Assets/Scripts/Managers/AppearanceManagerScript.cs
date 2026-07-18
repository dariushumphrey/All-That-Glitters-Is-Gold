using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceManagerScript : MonoBehaviour
{
    public FirearmScript weapon;
    public GameObject weaponModel, exoticWeaponModel;
    public GameObject[] rarityAddons;
    public ParticleSystem replacementFlash;
    public Material replacementBulletTrail;

    public bool relocateBarrel, relocateBarrelExotic, replaceMuzzleEffect, replaceTrailEffect;
    public int atRarity;
    public Vector3 relocationPos, relocationExoticPos;

    // Start is called before the first frame update
    void Start()
    {
        if(!weapon.isExotic)
        {
            if(weapon.weaponRarity == 2)
            {
                rarityAddons[0].gameObject.SetActive(true);
            }

            else if(weapon.weaponRarity > 2)
            {
                for (int a = 0; a < weapon.weaponRarity - 1; a++)
                {
                    rarityAddons[a].gameObject.SetActive(true);
                }            
            }

            if (relocateBarrel)
            {
                if(atRarity <= weapon.weaponRarity)
                {
                    weapon.barrel.transform.localPosition = relocationPos;
                    weapon.muzzleFlash.transform.localPosition = relocationPos;
                }
            }
        }

        else
        {
            weaponModel.gameObject.SetActive(false);
            exoticWeaponModel.gameObject.SetActive(true);

            if(relocateBarrelExotic)
            {
                weapon.barrel.transform.localPosition = relocationExoticPos;
                weapon.muzzleFlash.transform.localPosition = relocationExoticPos;
            }

            if (replaceMuzzleEffect)
            {
                weapon.muzzleFlash = replacementFlash;
            }

            if (replaceMuzzleEffect)
            {
                weapon.bulletTrail = replacementBulletTrail;
            }
        }     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
