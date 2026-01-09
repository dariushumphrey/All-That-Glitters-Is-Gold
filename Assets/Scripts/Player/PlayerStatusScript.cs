using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusScript : MonoBehaviour
{

    public int playerScaling; //Increases Player Health and Shield by Difficulty number
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
    public float invincibilityDuration = 0.3f;
    public bool isDead;
    public bool isInvincible;
    public GameObject immunity; //Visual aid to represent Immunity window

    public Slider health, shield;
    public Image hBar, sBar; //Represent Health, Shield values by color
    public ParticleSystem shieldHit;
    public ParticleSystem shieldBreak;
    public ParticleSystem shieldRecharge;
    private int healthAdd; //Player Health is added onto by this value
    private int shieldAdd; //Player Shield is added onto by this value
    private float regenShieldResetSeconds;
    private bool done = false; //Prevents operation from repeating if true
    internal PlayerMoveScript move;
    internal PlayerCameraScript cam;
    internal PlayerInventoryScript inv;
    internal int dmgReceived; //Takes damage received and decrements health
    internal bool playerHit; //Confirms Player has been damaged if true
    internal GameObject counterplayCheat; //Confirms Weapon with Counterplay is present if not null
    internal bool counterplayFlag = false; //Confirms the Cheat Counterplay' condition has been met if true

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

        //Changes color of Health bar by health thresholds
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

        //Changes color of Shield background when depleted
        if (playerShield <= 0)
        {
            sBar.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 0.9f));

            if(!done)
            {
                shieldBreak.Play();
                done = true;
            }
        }

        else
        {
            sBar.color = Color.black;
            if (done)
            {
                shieldBreak.Stop();
                shieldRecharge.Play();
                done = false;
            }
        }

        ShieldDamageCheck();
        //PlayerDeath();
    }

    /// <summary>
    /// Increases Player base Health and Shield by difficulty
    /// </summary>
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

    /// <summary>
    /// Corrects improper Player scaling assignments
    /// </summary>
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

    /// <summary>
    /// Manages events where Player has taken damage
    /// </summary>
    /// <param name="damageTaken">Damage received by Enemies or Berth explosions</param>
    public void InflictDamage(int damageTaken)
    {
        playerHit = true;

        if(isInvincible || isDead)
        {        
            return;
        }

        else
        {
            dmgReceived = damageTaken;

            if (playerShield > 0)
            {
                shieldHit.Play();
                playerShield -= dmgReceived;
                regenShieldSeconds = regenShieldResetSeconds;
                StopCoroutine(RechargeShield());
            }

            else
            {
                playerHealth -= dmgReceived;
                if(playerHealth <= 0f)
                {
                    isDead = true;
                    PlayerDeath();
                }
            }
        }      
    }

    /// <summary>
    /// Manages event where Player has been defeated
    /// </summary>
    void PlayerDeath()
    {
        if(isDead)
        {
            //isDead = true;
            gameObject.GetComponent<Rigidbody>().freezeRotation = false;
            move.enabled = false;

            cam.enabled = false;
            cam.GetComponent<PlayerCameraScript>().playerCamera.gameObject.AddComponent<LookAtPlayerScript>();
            //cam.GetComponent<PlayerCameraScript>().playerCamera.transform.parent = gameObject.transform;

            //inv.lucentFunds = 0;
            if (inv.inventory.Count > 0 && !move.sprinting)
            {
                inv.GetComponentInChildren<FirearmScript>().enabled = false;            
            }

            inv.enabled = false;

            //inv.lucentText.text = " ";
            
        }
    }  
    
    /// <summary>
    /// Waits before recharging Shield if current Shield strength is less than max Shield
    /// </summary>
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

    /// <summary>
    /// Recharges Shield until it reaches full strength
    /// </summary>
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

    /// <summary>
    /// Deactivates Player immunity after delay
    /// </summary>
    public IEnumerator CancelInvulnerable()
    {
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        immunity.SetActive(false);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, 7f);
    //}
}
