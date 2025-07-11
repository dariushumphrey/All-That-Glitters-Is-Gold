using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusScript : MonoBehaviour
{

    public int playerScaling;
    public int playerHealth = 500;
    public int playerHealthMax = 500;
    //This value buffs player health:
    //-Increasing this number adds a percentage of max health onto current health
    //-Increasing max health allows this number to give more health in return
    public float healthPercent = 20f;

    public int playerShield = 550;
    public int playerShieldMax = 550;
    //This value buffs shield capacity:
    //-Increasing this number adds a percentage of max shield onto current shield
    //-Increasing max health allows this number to give more shield in return
    public float shieldPercent = 20f;

    public float regenShieldSeconds = 7.0f;
    public bool isDead;

    public Slider health, shield;
    public Image hBar, sBar;
    private int healthAdd;
    private int shieldAdd;
    private float regenShieldResetSeconds;
    private float regenHealthResetSeconds;
    internal PlayerMoveScript move;
    internal PlayerCameraScript cam;
    internal PlayerInventoryScript inv;
    internal int dmgReceived;
    internal bool playerHit;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<PlayerMoveScript>();
        cam = GetComponent<PlayerCameraScript>();
        inv = GetComponent<PlayerInventoryScript>();

        health.maxValue = playerHealthMax;
        health.value = playerHealth;
        shield.maxValue = playerShieldMax;
        shield.value = playerShield;

        //StatusCorrections();
        //StatusScaling();

        regenShieldResetSeconds = regenShieldSeconds;
        isDead = false;

    }

    // Update is called once per frame
    void Update()
    {
        health.value = playerHealth;
        shield.value = playerShield;

        if (playerHealth >= playerHealthMax / 2)
        {
            hBar.color = Color.green;
        }

        if (playerHealth < playerHealthMax / 2)
        {
            hBar.color = Color.cyan;
        }

        if (playerHealth > 0 && playerHealth <= playerHealthMax / 4)
        {
            hBar.color = Color.blue;
        }

        if (playerShield <= 0)
        {
            sBar.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 0.9f));
        }

        else
        {
            sBar.color = Color.black;
        }

        ShieldDamageCheck();
        PlayerDeath();
    }

    public void StatusScaling()
    {
        healthPercent *= playerScaling;
        healthPercent /= 100;
        healthPercent *= playerHealthMax;
        healthAdd = (int)healthPercent;

        shieldPercent *= playerScaling;
        shieldPercent /= 100;
        shieldPercent *= playerShieldMax;
        shieldAdd = (int)shieldPercent;

        if (playerScaling == 1)
        {
            playerHealth = playerHealthMax;
            playerShield = playerShieldMax;

            health.maxValue = playerHealthMax;
            shield.maxValue = playerShieldMax;
        }

        if (playerScaling == 2)
        {
            playerHealth = playerHealthMax + healthAdd;
            playerShield = playerShieldMax + shieldAdd;
            playerHealthMax = playerHealth;
            playerShieldMax = playerShield;

            health.maxValue = playerHealthMax;
            shield.maxValue = playerShieldMax;
        }

        if (playerScaling == 3)
        {
            playerHealth = playerHealthMax + healthAdd;
            playerShield = playerShieldMax + shieldAdd;
            playerHealthMax = playerHealth;
            playerShieldMax = playerShield;

            health.maxValue = playerHealthMax;
            shield.maxValue = playerShieldMax;
        }

        if (playerScaling == 4)
        {
            playerHealth = playerHealthMax + healthAdd;
            playerShield = playerShieldMax + shieldAdd;
            playerHealthMax = playerHealth;
            playerShieldMax = playerShield;

            health.maxValue = playerHealthMax;
            shield.maxValue = playerShieldMax;
        }

        if (playerScaling == 5)
        {
            playerHealth = playerHealthMax + healthAdd;
            playerShield = playerShieldMax + shieldAdd;
            playerHealthMax = playerHealth;
            playerShieldMax = playerShield;

            health.maxValue = playerHealthMax;
            shield.maxValue = playerShieldMax;
        }
    }

    public void StatusCorrections()
    {
        if(playerScaling <= 0)
        {
            playerScaling = 1;
        }

        if(playerScaling >= 6)
        {
            playerScaling = 5;
        }
    }

    public void InflictDamage(int damageTaken)
    {
        playerHit = true;
        dmgReceived = damageTaken;

        if (playerShield > 0)
        {
            playerShield -= dmgReceived;
            regenShieldSeconds = regenShieldResetSeconds;
            StopCoroutine(RechargeShield());        
            return;
        }

        else
        {
            playerHealth -= dmgReceived;
            return;
        }
    }

    void PlayerDeath()
    {
        if(playerHealth <= 0)
        {
            isDead = true;
            gameObject.GetComponent<Rigidbody>().freezeRotation = false;
            move.enabled = false;
            cam.enabled = false;

            inv.lucentFunds = 0;
            if (inv.inventory.Count > 0)
            {
                inv.GetComponentInChildren<FirearmScript>().enabled = false;            
            }

            inv.enabled = false;

            //inv.lucentText.text = " ";
            
        }
    }  

    void ShieldDamageCheck()
    {
        if(isDead != true)
        {
            if (playerShield < playerShieldMax)
            {
                regenShieldSeconds -= Time.deltaTime;
                if (regenShieldSeconds <= 0.0f)
                {
                    //shieldText.text = "Recharging...";
                    StartCoroutine(RechargeShield());
                    return;
                }
            }
        }     
    }

    public IEnumerator RechargeShield()
    {
        yield return new WaitForSeconds(0.1f);
        playerShield++;
        if (playerShield >= playerShieldMax)
        {
            playerShield = playerShieldMax;
            regenShieldSeconds = regenShieldResetSeconds;
        }
    }
}
