using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootScript : MonoBehaviour
{
    public int raritySpawn;
    public int kioskPrice;
    public int gamblePrice;
    public int lootSpamMax = 3;
    public float pricePercent = 10f;
    public float gamblePercent = 60f;
    private int priceAdd;
    private float percentReset;
    private float gambleReset;
    public bool isDrop;
    public bool isChest;
    public bool isKiosk;
    public GameObject drop, exoticDrop;
    public List<GameObject> loot = new List<GameObject>();
    public Transform lootSpawn;
    public Material deactivated;
    public bool debug;
    public bool spamLoot = false;

    private float spawnAgain, spawnRate;
    private int lootGrant;
    private PlayerInventoryScript player;
    private bool inProx;
    private bool turnOff;
    private int spawnCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        spawnAgain = 0.0f;
        spawnRate = 0.08f;

        percentReset = pricePercent;
        gambleReset = gamblePercent;
        RarityCorrection();

        if(isChest == true)
        {
            if (debug == true)
            {              
                SpawnLoot();
                return;
            }
        }

        if(isKiosk == true)
        {
            player = FindObjectOfType<PlayerInventoryScript>();

            turnOff = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isChest == true)
        {
            if (debug == true)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    SpawnLoot();
                }
            }

            if(spamLoot == true)
            {
                Debug.Log(spawnCount);
                spawnAgain += Time.deltaTime;
                if(spawnAgain >= spawnRate)
                {
                    StartCoroutine(SpawnRepeatedly());
                    spawnCount++;

                    if (spawnCount >= lootSpamMax)
                    {
                        spamLoot = false;
                    }
                }               
            }
        }
        
        //if(isKiosk == true)
        //{
        //    if(Input.GetKeyDown(KeyCode.E))
        //    {
        //        if(player.lucentFunds < kioskPrice)
        //        {
        //            //The Player cannot buy a Weapon; There is too little or no Lucent to spend
        //        }

        //        if (inProx == true && turnOff == false && player.lucentFunds >= kioskPrice)
        //        {
        //            player.lucentFunds -= kioskPrice;

        //            if(raritySpawn == 5)
        //            {
        //                SpawnExotic();
        //            }

        //            else
        //            {
        //                SpawnDrop();
        //            }
        //        }
        //    }

        //    if (Input.GetKeyDown(KeyCode.Q))
        //    {
        //        if (inProx == true && turnOff == false)
        //        {
        //            if(player.lucentFunds < gamblePrice)
        //            {
        //                //The Player cannot spawn a higher rarity Weapon; There is too little or no Lucent to forfeit
        //            }
                   
        //            else
        //            {
        //                if(raritySpawn >= 5)
        //                {
        //                    player.lucentFunds -= gamblePrice;
        //                    SpawnDrop();
        //                }

        //                else
        //                {
        //                    player.lucentFunds -= gamblePrice;
        //                    raritySpawn++;
        //                    SpawnDrop();

        //                    player.kioskText.text = " ";
        //                    //GetComponent<Renderer>().material = deactivated;
        //                    player.kioskText.text = "Unavailable";
        //                    turnOff = true;
        //                }                       
        //            }                   
        //        }
        //    }
        //}
    }

    void RarityCorrection()
    {
        if(raritySpawn <= 0)
        {
            raritySpawn = 1;
        }      

        if (raritySpawn >= 6)
        {
            raritySpawn = 5;
        }
    }

    public void PriceAdjust()
    {
        if(raritySpawn >= 2)
        {
            pricePercent /= 100;
            pricePercent *= kioskPrice;
            pricePercent *= raritySpawn;
            priceAdd = (int)pricePercent;
            kioskPrice += priceAdd;

            gamblePercent /= 100;
            gamblePercent *= kioskPrice;
            gamblePrice = (int)gamblePercent;
        }

        else
        {         
            gamblePercent /= 100;
            gamblePercent *= kioskPrice;
            gamblePrice = (int)gamblePercent;
        }     

        pricePercent = percentReset;
        gamblePercent = gambleReset;
    }

    public void SpawnDrop()
    {
        drop.GetComponent<LootScript>().raritySpawn = raritySpawn;
        GameObject reward = Instantiate(drop, lootSpawn.transform.position, lootSpawn.transform.rotation);
        reward.name = drop.name;

        if (raritySpawn == 1)
        {
            reward.GetComponent<Renderer>().material.color = Color.gray;
        }

        else if (raritySpawn == 2)
        {
            reward.GetComponent<Renderer>().material.color = Color.green;
        }

        else if (raritySpawn == 3)
        {
            reward.GetComponent<Renderer>().material.color = Color.red;
        }

        else if (raritySpawn == 4)
        {
            reward.GetComponent<Renderer>().material.color = Color.yellow;
        }

        else if (raritySpawn == 5)
        {
            if (reward.GetComponent<ColorLerpScript>() != null)
            {
                reward.GetComponent<ColorLerpScript>().colorOne = Color.cyan;
                reward.GetComponent<ColorLerpScript>().colorTwo = Color.white;
            }

            else
            {
                reward.GetComponent<Renderer>().material.color = Color.cyan;
            }
        }

        //reward.GetComponent<Rigidbody>().AddForce((lootSpawn.transform.forward + lootSpawn.transform.up) * 3f, ForceMode.Impulse);
        reward.GetComponent<Rigidbody>().AddExplosionForce(400f, lootSpawn.transform.position, 10f, 500f);
    }

    public void SpawnExotic()
    {
        exoticDrop.GetComponent<LootScript>().raritySpawn = raritySpawn;
        GameObject reward = Instantiate(exoticDrop, lootSpawn.transform.position, lootSpawn.transform.rotation);
        reward.name = drop.name;

        if (reward.GetComponent<ColorLerpScript>() != null)
        {
            reward.GetComponent<ColorLerpScript>().colorOne = Color.cyan;
            reward.GetComponent<ColorLerpScript>().colorTwo = Color.white;
        }

        //reward.GetComponent<Rigidbody>().AddForce((-Vector3.right + Vector3.up) * 3f, ForceMode.Impulse);
        reward.GetComponent<Rigidbody>().AddExplosionForce(400f, lootSpawn.transform.position, 10f, 500f);
    }

    public void SpawnLoot()
    {
        for (int l = 0; l < loot.Count; l++)
        {
            loot[l].GetComponent<FirearmScript>().weaponRarity = raritySpawn;
        }

        lootGrant = Random.Range(0, loot.Count);
        //if(loot[lootGrant].GetComponent<FirearmScript>() != null /*&& loot[lootGrant].GetComponent<FirearmScript>().weaponRarity != raritySpawn*/)
        //{
        //    if (loot[lootGrant].GetComponent<FirearmScript>().isExotic == true)
        //    {
        //        loot[lootGrant].GetComponent<FirearmScript>().weaponRarity = 5;
        //    }

        //    else
        //    {
        //        loot[lootGrant].GetComponent<FirearmScript>().weaponRarity = raritySpawn;
        //    }
        //}

        GameObject reward = Instantiate(loot[lootGrant], lootSpawn.transform.position, lootSpawn.transform.rotation);
        //reward.GetComponent<FirearmScript>().weaponRarity = raritySpawn;
        //reward.GetComponent<FirearmScript>().RarityAugment();

        //Removes (Clone) from name
        reward.name = loot[lootGrant].name;
    }

    IEnumerator SpawnRepeatedly()
    {
        spawnAgain = 0.0f;
        SpawnDrop();
        yield return new WaitForSeconds(spawnRate);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isChest == true)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                return;
            }
        }
        
        if(isDrop == true)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                SpawnLoot();
                Destroy(gameObject);
            }
        }
    } 

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //other.gameObject.GetComponent<PlayerInventoryScript>().kioskText.text = "'E' Purchase Random Weapon: " + kioskPrice + "\n" +
            //        "'Q' Raise Rarity (Spawns rarer weapon, closes Kiosk on purchase): " + gamblePrice;

            //if(raritySpawn == 5)
            //{
            //    other.gameObject.GetComponent<PlayerInventoryScript>().kioskText.text = "'E' Purchase Exotic Weapon: " + kioskPrice + "\n" +
            //        "'Q' Purchase Random Weapon: " + gamblePrice;
            //}

            //inProx = true;
            
            //if(turnOff == true)
            //{
            //    other.gameObject.GetComponent<PlayerInventoryScript>().kioskText.text = "Unavailable.";
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inProx = false;

            //if (isKiosk == true)
            //{
            //    other.gameObject.GetComponent<PlayerInventoryScript>().kioskText.text = " ";
            //}
        }
    }

}
