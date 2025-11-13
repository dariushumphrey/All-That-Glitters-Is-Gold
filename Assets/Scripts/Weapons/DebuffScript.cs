using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffScript : MonoBehaviour
{
    internal float damageAmp = 1.5f; //Muliplier to increase damage taken
    internal float debuffLength = 5f; //Duration of effect

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(KillDebuff());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Destroys itself after delay
    /// </summary>
    private IEnumerator KillDebuff()
    {
        yield return new WaitForSeconds(debuffLength);
        Destroy(this);
    }
}
