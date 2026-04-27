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
            LucentPassive();
        }
    }

    public void LucentPassive()
    {
        lucentProduceTimer -= Time.deltaTime;
        if (lucentProduceTimer <= 0f)
        {
            lucentProduceTimer = produceReset;

            spawnField = field.bounds;
            Vector3 spawnSite = spawnField.center + new Vector3(Random.Range(-spawnField.extents.x, spawnField.extents.x),
                                                                Random.Range(-spawnField.extents.y, spawnField.extents.y),
                                                                Random.Range(-spawnField.extents.z, spawnField.extents.z));

            miniLucent = Instantiate(spectrumLucent, spawnSite, transform.rotation);
            miniLucent.name = spectrumLucent.name;

            miniLucent.GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
            if (spawnCount <= spawnMax)
            {
                spawnCount++;
            }

            else
            {
                active = false;
                StartCoroutine(ResetCount());
            }
        }
    }

    public IEnumerator ResetCount()
    {
        yield return new WaitForSeconds(1f);
        spawnCount = 0;
    }
}
