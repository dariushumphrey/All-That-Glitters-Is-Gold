using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bolster : MonoBehaviour
{
    private FirearmScript firearm;
    private PlayerStatusScript player;
    private GameObject activation; //VFX used to convey activity
    internal GameObject proc; //Text UI that records Cheat activity
    private float regenResetReference;
    private float regenSubtractPercent = 30f;
    private float regenSubtractPercentMax = 10f;
    private float regenSubtractBy;
    internal bool killConfirmed = false; //Affirms achieved kill if true
    internal bool maxed = false;
    private float buffTimer = 20f;
    private float buffTimerReset;
    internal bool vfxToggle = false;
    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        proc.GetComponent<Text>().text = "";
        activation = Resources.Load<GameObject>("Particles/BolsterActive");

        regenResetReference = player.regenShieldResetSeconds;

        regenSubtractPercent /= 100;
        regenSubtractPercent *= regenResetReference;
        regenSubtractBy = regenSubtractPercent;

        regenSubtractPercentMax /= 100;
        regenSubtractPercentMax *= regenResetReference;

        buffTimerReset = buffTimer;

    }

    // Update is called once per frame
    void Update()
    {
        if(!maxed)
        {
            if (killConfirmed)
            {
                player.regenShieldSeconds -= regenSubtractBy;
                player.regenShieldResetSeconds = player.regenShieldSeconds;

                if (player.regenShieldSeconds <= regenSubtractPercentMax)
                {
                    player.regenShieldSeconds = regenSubtractPercentMax;
                    maxed = true;
                }

                PlayVFXFree();
                killConfirmed = false;
            }
        }
        
        else
        {
            if(firearm.weaponRarity != 5)
            {
                player.regenShieldResetSeconds = regenSubtractPercentMax;

                buffTimer -= Time.deltaTime;
                proc.GetComponent<Text>().text = "Bolster: " + buffTimer.ToString("F0") + "s";

                if (buffTimer <= 0f)
                {
                    buffTimer = buffTimerReset;
                    player.regenShieldResetSeconds = regenResetReference;
                    player.regenShieldSeconds = regenResetReference;
                    proc.GetComponent<Text>().text = "";

                    if (killConfirmed)
                    {
                        killConfirmed = false;
                    }

                    maxed = false;

                }
            }
            
            else
            {
                player.regenShieldResetSeconds = regenSubtractPercentMax;
                proc.GetComponent<Text>().text = "Bolster: INF";
            }
        }
    }

    public void PlayVFX()
    {
        if(vfxToggle)
        {
            GameObject effect = Instantiate(activation, gameObject.transform.root.gameObject.transform.position, Quaternion.identity);
            effect.name = activation.name;
            vfxToggle = false;
        }
    }

    public void PlayVFXFree()
    {
        GameObject effect = Instantiate(activation, gameObject.transform.root.gameObject.transform.position, Quaternion.identity);
        effect.name = activation.name;
    }

    public void RemoteProc()
    {
        player.regenShieldSeconds -= regenSubtractBy;
        player.regenShieldResetSeconds = player.regenShieldSeconds;

        if (player.regenShieldSeconds <= regenSubtractPercentMax)
        {
            player.regenShieldSeconds = regenSubtractPercentMax;
            maxed = true;
        }

        PlayVFXFree();
    }

    public void ActivatorDroneRemoteProc()
    {
        if(!maxed)
        {
            player.regenShieldSeconds -= regenSubtractBy;
            player.regenShieldResetSeconds = player.regenShieldSeconds;

            if (player.regenShieldSeconds <= regenSubtractPercentMax)
            {
                player.regenShieldSeconds = regenSubtractPercentMax;
                maxed = true;
            }

            PlayVFXFree();
        }

        else
        {
            player.regenShieldSeconds = 0f;
            PlayVFXFree();
        }
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }

        player.regenShieldResetSeconds = regenResetReference;
        player.regenShieldSeconds = regenResetReference;
        buffTimer = buffTimerReset;
        maxed = false;
        vfxToggle = false;
    }
}
