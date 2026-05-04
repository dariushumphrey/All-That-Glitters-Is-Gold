using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumLucentSpawnerScript : MonoBehaviour
{
    public bool active = false;
    public GameObject spectrumLucent;
    public int spawnMax = 5;
    public float lucentProduceTimer = 0.5f; //Goal time to spawn Lucent Clusters
    private float produceReset; //Holds starting Lucent Cluster timer
    private Bounds spawnField; //Bounds that clusters spawn within
    public Collider field; //Zone for cluster spawning

    private GameObject miniLucent; //Lucent game object
    private int spawnCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        produceReset = lucentProduceTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            //Debug.Log(spawnCount + " | " + spawnMax);
            LucentPassive();
        }
    }

    public void LucentPassive()
    {
        lucentProduceTimer -= Time.deltaTime;
        if (lucentProduceTimer <= 0f)
        {
            lucentProduceTimer = produceReset;

            if(spawnCount != spawnMax)
            {
                spawnCount++;

                spawnField = field.bounds;
                Vector3 spawnSite = spawnField.center + new Vector3(Random.Range(-spawnField.extents.x, spawnField.extents.x),
                                                                    Random.Range(-spawnField.extents.y, spawnField.extents.y),
                                                                    Random.Range(-spawnField.extents.z, spawnField.extents.z));

                miniLucent = Instantiate(spectrumLucent, spawnSite, transform.rotation);
                miniLucent.name = spectrumLucent.name;

                miniLucent.GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
            }

            else
            {
                active = false;
                ResetCount();
            }
        }
    }

    public void ResetCount()
    {
        spawnCount = 0;
    }
}
