using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootScript : MonoBehaviour
{
    public int raritySpawn; //Value used to assign Rarity level to Weapons
    public int lootSpamMax = 3; //Repeats spawning behavior up to this value
    public int focusTarget = -1; //Represent Weapon type focus by number
    public bool isDrop; //Confirmed as a Delivery item if true
    public bool isChest; //Confirmed as a loot chest if true
    public bool debug; //Allows manual trigger of SpawnLoot() if true
    public bool spamLoot = false; //Allows repeated Delivery item spawns if true
    public float spawnRate = 0.08f; //Rate of speed for Loot spawns

    //drop - Loot delivery item that spawns standard Weapons
    //exoticDrop - Loot delivery item that spawns Exotic Weapons
    public GameObject drop, exoticDrop;
    public List<GameObject> loot = new List<GameObject>(); //List of Weapons to spawn
    public Transform lootSpawn; //Spawn position of Weapons, Delivery items

    private float spawnAgain; //Timer used to mediate repeated spawn rate
    private int lootGrant; //Index used to select specific Weapon
    private PlayerMoveScript player;
    private int spawnCount = 0; //Number of times a spawn has occured

    // Start is called before the first frame update
    void Start()
    {
        spawnAgain = 0.0f;

        RarityCorrection();

        if (debug)
        {
            if (isChest)
            {
                SpawnLoot();
                return;
            }
        }

        player = FindObjectOfType<PlayerMoveScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0) && isChest)
            {
                SpawnLoot();
            }
        }

        if (isChest)
        {
            if (spamLoot)
            {
                //Debug.Log(spawnCount);
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
    }

    /// <summary>
    /// Fixes incorrect rarity assignments
    /// </summary>
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

    /// <summary>
    /// Spawns Loot delivery items
    /// Colors delivery items by rarity
    /// </summary>
    public void SpawnDrop()
    {
        drop.GetComponent<LootScript>().raritySpawn = raritySpawn;
        GameObject reward = Instantiate(drop, lootSpawn.transform.position, lootSpawn.transform.rotation);
        reward.GetComponent<LootScript>().focusTarget = focusTarget;
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

        if (player.zeroGravity)
        {
            reward.GetComponent<Rigidbody>().useGravity = false;
        }

        //reward.GetComponent<Rigidbody>().AddForce((lootSpawn.transform.forward + lootSpawn.transform.up) * 3f, ForceMode.Impulse);
        reward.GetComponent<Rigidbody>().AddExplosionForce(400f, lootSpawn.transform.position, 10f, 500f);

    }

    /// <summary>
    /// Spawns Loot delivery items
    /// Colors delivery items by rarity
    /// </summary>
    public void SpawnExotic()
    {
        exoticDrop.GetComponent<LootScript>().raritySpawn = raritySpawn;
        GameObject reward = Instantiate(exoticDrop, lootSpawn.transform.position, lootSpawn.transform.rotation);
        reward.GetComponent<LootScript>().focusTarget = focusTarget;
        reward.name = drop.name;

        if (reward.GetComponent<ColorLerpScript>() != null)
        {
            reward.GetComponent<ColorLerpScript>().colorOne = Color.cyan;
            reward.GetComponent<ColorLerpScript>().colorTwo = Color.white;
        }

        if (player.zeroGravity)
        {
            reward.GetComponent<Rigidbody>().useGravity = false;
        }

        //reward.GetComponent<Rigidbody>().AddForce((-Vector3.right + Vector3.up) * 3f, ForceMode.Impulse);
        reward.GetComponent<Rigidbody>().AddExplosionForce(400f, lootSpawn.transform.position, 10f, 500f);
    }

    /// <summary>
    /// Spawns Weapon randomly or specified by focus value
    /// </summary>
    public void SpawnLoot()
    {
        for (int l = 0; l < loot.Count; l++)
        {
            loot[l].GetComponent<FirearmScript>().weaponRarity = raritySpawn;
        }

        if(focusTarget <= -1)
        {
            lootGrant = Random.Range(0, loot.Count);
        }
        

        else
        {
            lootGrant = focusTarget;
        }

        GameObject reward = Instantiate(loot[lootGrant], lootSpawn.transform.position, lootSpawn.transform.rotation);

        //Removes (Clone) from name
        reward.name = loot[lootGrant].name;
    }

    /// <summary>
    /// Triggers SpawnDrop() after a delay
    /// </summary>
    IEnumerator SpawnRepeatedly()
    {
        spawnAgain = 0.0f;
        SpawnDrop();
        yield return new WaitForSeconds(spawnRate);
    }

    private void OnCollisionEnter(Collision collision)
    {       
        if(isDrop == true)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                SpawnLoot();
                Destroy(gameObject);
            }
        }
    } 
}