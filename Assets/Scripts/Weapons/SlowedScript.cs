using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowedScript : MonoBehaviour
{
    internal float slowedLength = 10f; //Duration of effect

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
        yield return new WaitForSeconds(slowedLength);
        Destroy(this);
    }
}
