using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartParticlesScript : MonoBehaviour
{
    public List<ParticleSystem> particles = new List<ParticleSystem>();
    private int repeatCount = 0;
    public int repeatTarget = 0;
    public float repeatTimer = 0f;
    public bool tripped = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(repeatCount);

        if(tripped)
        {
            if (repeatCount >= repeatTarget)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            tripped = true;
            StartCoroutine(PlayParticles());
        }
    }

    public IEnumerator PlayParticles()
    {
        for(int p = 0; p < particles.Count; p++)
        {
            particles[p].Play();
        }

        repeatCount++;
        yield return new WaitForSeconds(repeatTimer);
        StartCoroutine(PlayParticles());
    }
}
