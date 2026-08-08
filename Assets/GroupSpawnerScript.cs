using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupSpawnerScript : MonoBehaviour
{
    public int spawnRepeat = 4; //number of times to repeat spawns
    public float spawnDelayTime = 2f;
    public List<GameObject> spawned = new List<GameObject>(); //List of Enemies to spawn
    public bool playVFX = false;
    public GameObject activation;
    public Collider field; //Trigger-marked Collider used to spawn Enemies within
    public bool lucentHarm = false;
    private Bounds spawnField;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnObject()
    {
        for (int s = 0; s < spawnRepeat; s++)
        {
            int picker = Random.Range(0, spawned.Count);
            spawnField = field.bounds;
            Vector3 spawnSite = spawnField.center + new Vector3(Random.Range(-spawnField.extents.x, spawnField.extents.x),
                                                                Random.Range(-spawnField.extents.y, spawnField.extents.y),
                                                                Random.Range(-spawnField.extents.z, spawnField.extents.z));

            GameObject fresh = Instantiate(spawned[picker], spawnSite, Quaternion.identity);
            fresh.name = spawned[picker].name;

            if(fresh.GetComponent<LucentScript>())
            {
                if(lucentHarm)
                {
                    fresh.GetComponent<Rigidbody>().AddForce(Vector3.down * 20f);

                    fresh.GetComponent<LucentScript>().threat = true;
                    fresh.GetComponent<LucentScript>().shatterDelayTime = 3f;
                    fresh.GetComponent<LucentScript>().StartCoroutine(fresh.GetComponent<LucentScript>().Shatter());
                }
            }
        }

        if (playVFX)
        {
            GameObject effect = Instantiate(activation, transform.position, Quaternion.identity);
        }
    }
    public IEnumerator SpawnObjectOnDelay()
    {
        yield return new WaitForSeconds(spawnDelayTime);

        for (int s = 0; s < spawnRepeat; s++)
        {
            int picker = Random.Range(0, spawned.Count);
            spawnField = field.bounds;
            Vector3 spawnSite = spawnField.center + new Vector3(Random.Range(-spawnField.extents.x, spawnField.extents.x),
                                                                Random.Range(-spawnField.extents.y, spawnField.extents.y),
                                                                Random.Range(-spawnField.extents.z, spawnField.extents.z));


            GameObject fresh = Instantiate(spawned[picker], spawnSite, Quaternion.identity);
            fresh.name = spawned[picker].name;

            if (fresh.GetComponent<LucentScript>())
            {
                if (lucentHarm)
                {
                    fresh.GetComponent<Rigidbody>().AddForce(Vector3.down * 20f);

                    fresh.GetComponent<LucentScript>().threat = true;
                    fresh.GetComponent<LucentScript>().shatterDelayTime = 3f;
                    fresh.GetComponent<LucentScript>().StartCoroutine(fresh.GetComponent<LucentScript>().Shatter());
                }
            }
        }

        if (playVFX)
        {
            GameObject effect = Instantiate(activation, transform.position, Quaternion.identity);
        }
    }
}
