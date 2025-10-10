using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDPHealthDebuff : MonoBehaviour
{
    internal GameObject activation;
    internal int damageAmp = 2;
    internal int dmgShare;

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
