using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartParticlesScript : MonoBehaviour
{
    public List<ParticleSystem> particles = new List<ParticleSystem>(); //List of Particles intended for play
    private int repeatCount = 0; //Current number of particle plays
    public int repeatTarget = 0; //Target number of particle plays
    public float repeatTimer = 0f; //Time to wait before replaying particles
    public bool tripped = false; //Checks if reaching target repeats has been reached if true

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(repeatCount);

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

    /// <summary>
    /// Plays particles after a short delay
    /// </summary>
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
