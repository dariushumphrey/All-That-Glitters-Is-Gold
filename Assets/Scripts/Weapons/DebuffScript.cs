using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffScript : MonoBehaviour
{
    internal float damageAmp = 1.5f;
    private float debuffLength = 5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(KillDebuff());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator KillDebuff()
    {
        yield return new WaitForSeconds(debuffLength);
        Destroy(this);
    }
}
