using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MultiWeaponFirearm : FirearmScript
{
    public override void Update()
    {
        base.Update();

        if(display)
        {
            return;
        }

        else
        {
            inv.GetComponent<PlayerCameraScript>().multiWeapon = true;
            inv.GetComponent<PlayerMeleeScript>().multiWeapon = true;

            MultiWeaponAttack();
        }
    }

    public void MultiWeaponAttack()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            if(inv.GetComponent<PlayerMeleeScript>().meleeTarget != null)
            {
                inv.GetComponent<PlayerMeleeScript>().meleeLock = true;
            }
        }
    }

    private void OnDisable()
    {
        if(inv != null)
        {
            inv.GetComponent<PlayerCameraScript>().multiWeapon = false;
            inv.GetComponent<PlayerMeleeScript>().multiWeapon = false;

        }
    }
}
