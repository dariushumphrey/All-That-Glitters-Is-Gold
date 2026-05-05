using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardLucentCoverManagerScript : MonoBehaviour
{
    public bool active = false;
    public float restoreCoverTimer = 60f;
    public List<GameObject> covers = new List<GameObject>();
    public GameObject shatterEffect; //VFX that plays on condition

    private float restoreReset;
    private GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        restoreReset = restoreCoverTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            restoreCoverTimer -= Time.deltaTime;
            if(restoreCoverTimer <= 0f)
            {
                restoreCoverTimer = restoreReset;

                for(int c = 0; c < covers.Count; c++)
                {
                    if(!covers[c].activeInHierarchy)
                    {
                        covers[c].SetActive(true);

                        effect = Instantiate(shatterEffect, covers[c].transform.position, Quaternion.identity);
                        effect.name = "Shatter VFX";
                    }
                }
            }
        }
    }
}
