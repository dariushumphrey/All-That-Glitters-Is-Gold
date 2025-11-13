using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDPHealthDebuff : MonoBehaviour
{
    internal GameObject activation; //VFX used to convey activity
    internal int damageAmp = 2; //Multiplier used to increase damage received
    internal int dmgShare; //Number used to spread damage

    private void Awake()
    {
        activation = Resources.Load<GameObject>("Particles/SocialDistancePleaseActive");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
