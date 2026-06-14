using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepurposedForm : MonoBehaviour
{
    private float damagePercent = 10f;
    internal int damageApply;
    private LauncherFirearm firearm;
    private GameObject replevinMunition;
    internal GameObject proc; //Text UI that records Cheat activity

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<LauncherFirearm>();

        damagePercent /= 100;
        damagePercent *= firearm.damage;
        damageApply = (int)damagePercent;

        replevinMunition = Resources.Load<GameObject>("Game Items/Munition (AMR_Exotic)");

        firearm.GetComponent<LauncherFirearm>().munition = replevinMunition;

        if (proc)
        {
            proc.GetComponent<Text>().text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }
    }
}
