using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Superweapon : MonoBehaviour
{
    private FirearmScript firearm;
    internal GameObject proc; //Text UI that records Cheat activity
    private PlayerStatusScript player;
    private Material superweaponShotMaterial; //LineRenderer Material for bullet visual
    private GameObject superweaponLight; //Light object used to visualize charge
    private GameObject destructGrenade; //Destruct Grenade - used to spawn on defeated enemies
    private GameObject activation; //VFX used to convey activity
    private GameObject light; //Holds reference to spawned light
    private LayerMask contactOnly; //Permits raycast interaction only with specified Layers
    private float resistPercent = 0f; //% of damage to absorb
    private float resistPercentReset; //Holds starting damage absorption %
    private float resistMax = 80f; //Maximum allowable damage absorption %
    private float superweaponCharge = 0f; //Current charge of Superweapon attack
    private float superweaponPercent = 0f; //% of Weapon damage used as Superweapon damage
    private float superweaponReset; //Holds starting Superweapon damage
    private float superweaponLightMax = 5f; //Maximum light intensity for Superweapon charge
    private int superweaponDamage;
    private int dmgAbsorbAdd; //Number used to return Health/Shield
    private int damageNegate = 0; //Receives damage taken
    private int stackCount = 0;
    private int stackMax = 8;
    private int chargeAccelerant = 30; //Multipler that increases Superweapon charge when moving
    private int refrainAccelerant = 3; //Multipler that increases Superweapon charge when not moving
    internal bool killConfirmed = false; //Affirms an achieved kill if true
    internal bool toggle = false; //Enables/Disables effect if true/false

    // Start is called before the first frame update
    void Start()
    {
        firearm = GetComponent<FirearmScript>();
        player = FindObjectOfType<PlayerStatusScript>();
        proc.GetComponent<Text>().text = " ";
        contactOnly = LayerMask.GetMask("Enemy", "Surface");

        superweaponShotMaterial = Resources.Load<Material>("Materials/Weapons/SuperweaponShotMaterial");
        destructGrenade = Resources.Load<GameObject>("Game Items/DestructGrenade");
        activation = Resources.Load<GameObject>("Particles/SuperweaponActive");
        superweaponLight = Resources.Load<GameObject>("Particles/Lights/lucentShatterThreatIntensityLight");

        resistPercentReset = resistPercent;
        superweaponReset = superweaponPercent;
    }

    // Update is called once per frame
    void Update()
    {
        //Superweapon
        //___.text = Kills grant stacks of damage resistance. Stacks 8x. [E] - Charge an extreme-damage shot, inflicting 1000% of Weapon damage per stack. 

        if(toggle)
        {
            proc.GetComponent<Text>().text = "Superweapon Charge: " + superweaponCharge.ToString("F0") + "%";

            //Charges Superweapon attack, fires when 100%. Charge stops if input is released.
            if (Input.GetButton("Fire2"))
            {
                if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
                {
                    superweaponCharge += Time.deltaTime * chargeAccelerant;
                }

                else
                {
                    superweaponCharge += Time.deltaTime * (chargeAccelerant * refrainAccelerant);
                }

                GameObject effect = Instantiate(activation, firearm.barrel.transform.position, transform.rotation, firearm.barrel.transform);
                effect.name = activation.name;

                if(!light)
                {
                    GameObject shiner = Instantiate(superweaponLight, firearm.barrel.transform.position, transform.rotation, firearm.barrel.transform);
                    shiner.name = superweaponLight.name;
                    light = shiner;

                    light.GetComponent<Light>().intensity = 1;
                }

                else
                {
                    float newIntensity = (superweaponCharge / 100) * superweaponLightMax;
                    light.GetComponent<Light>().intensity = newIntensity;
                }

                if (superweaponCharge >= 100f)
                {
                    superweaponPercent /= 100;
                    superweaponPercent *= firearm.damage;
                    superweaponDamage = (int)superweaponPercent;

                    Vector3 rayOrigin = firearm.gunCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

                    GameObject start = new GameObject();
                    GameObject.Destroy(start, 0.1f);

                    start.name = "Trail";
                    start.AddComponent<LineRenderer>();
                    start.GetComponent<LineRenderer>().startWidth = 1f;
                    start.GetComponent<LineRenderer>().endWidth = 1f;
                    start.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    start.GetComponent<LineRenderer>().material = superweaponShotMaterial;
                    start.GetComponent<LineRenderer>().SetPosition(0, firearm.barrel.transform.position);

                    RaycastHit[] allHits = Physics.RaycastAll(rayOrigin, firearm.gunCam.transform.forward, 90f, contactOnly);                 

                    for (int s = 0; s < allHits.Length; s++)
                    {
                        if(allHits[s].collider.CompareTag("Enemy"))
                        {
                            allHits[s].collider.GetComponent<EnemyHealthScript>().inflictDamage(superweaponDamage);
                            if(allHits[s].collider.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
                            {
                                GameObject destruction = Instantiate(destructGrenade, allHits[s].collider.transform.position, allHits[s].collider.transform.rotation);

                                destruction.GetComponent<DestructGrenadeScript>().armingTime = 0f;
                                destruction.GetComponent<DestructGrenadeScript>().StartCoroutine(destruction.GetComponent<DestructGrenadeScript>().SetupGrenade());
                            }
                        }
                    }

                    Vector3 knockbackDir = -transform.parent.root.forward;
                    knockbackDir.y = 0;
                    transform.parent.root.GetComponent<Rigidbody>().AddForce(knockbackDir * 100000f);

                    start.gameObject.transform.position = rayOrigin + (firearm.gunCam.transform.forward * 90f);
                    start.GetComponent<LineRenderer>().SetPosition(1, rayOrigin + (firearm.gunCam.transform.forward * 90f));

                    superweaponCharge = 0f;
                    superweaponReset = superweaponPercent;
                    superweaponDamage = 0;

                    if (light)
                    {
                        Destroy(light, 0.1f);
                    }

                    stackCount = 0;
                    resistPercent = 0f;
                    resistPercentReset = resistPercent;

                    proc.GetComponent<Text>().text = "";

                    toggle = false;
                }
            }

            else
            {
                superweaponCharge = 0f;
                if(light)
                {
                    Destroy(light);
                }
            }
        }

        //Toggles Cheat effects
        if (Input.GetKeyDown(KeyCode.E) && stackCount >= 1)
        {
            if (!toggle)
            {
                toggle = true;
            }

            else
            {
                toggle = false;
            }
        }

        //Increases damage resistance, Superweapon damage % on kills
        if (killConfirmed)
        {
            if(resistPercent != resistMax)
            {
                resistPercent += 10f;
                resistPercentReset = resistPercent;

                superweaponPercent += 1000f;
                superweaponReset = superweaponPercent;
            }

            if(stackCount != stackMax)
            {
                stackCount++;
            }

            killConfirmed = false;
        }

        if(stackCount >= 1 && !toggle)
        {
            proc.GetComponent<Text>().text = "Damage Resist x" + stackCount;
        }

        //Taking damage triggers damage resistance if Cheat is active
        if (player.playerHit == true && resistPercent > 0f)
        {
            damageNegate = player.dmgReceived;
            player.dmgReceived = 0;
            DamageResistConversion();
            player.playerHit = false;
        }           
    }

    /// <summary>
    /// Converts damaged received into Shield or Health dependent on condition
    /// </summary>
    void DamageResistConversion()
    {
        resistPercent /= 100;
        resistPercent *= damageNegate;
        dmgAbsorbAdd = (int)resistPercent;

        resistPercent = resistPercentReset;
        
        if(player.playerShield <= 0)
        {
            if(player.playerHealth >= player.playerHealthMax)
            {
                player.playerHealth = player.playerHealthMax;
            }

            else
            {
                player.playerHealth += dmgAbsorbAdd;
            }
        }

        else
        {
            if (player.playerShield >= player.playerShieldMax)
            {
                player.playerShield = player.playerShieldMax;
            }

            else
            {
                player.playerShield += dmgAbsorbAdd;

            }
        }
    }

    private void OnDisable()
    {
        if (proc != null)
        {
            proc.GetComponent<Text>().text = "";
        }
    }
}
