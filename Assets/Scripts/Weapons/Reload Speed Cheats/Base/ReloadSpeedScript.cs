using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSpeedScript : MonoBehaviour
{
    private FirearmScript firearm;

    public virtual void Awake()
    {
        GetComponent<ReloadSpeedScript>().enabled = false;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        firearm = GetComponent<FirearmScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
