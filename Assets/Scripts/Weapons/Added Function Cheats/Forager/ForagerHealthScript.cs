using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForagerHealthScript : MonoBehaviour
{
    private GameObject player;
    internal int healthAdd;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(AutoCollection());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStatusScript>().playerHealth += healthAdd;
            if (other.gameObject.GetComponent<PlayerStatusScript>().playerHealth >= other.gameObject.GetComponent<PlayerStatusScript>().playerHealthMax)
            {
                other.gameObject.GetComponent<PlayerStatusScript>().playerHealth = other.gameObject.GetComponent<PlayerStatusScript>().playerHealthMax;
            }

            Destroy(gameObject);
        }
    }

    IEnumerator AutoCollection()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<BoxCollider>().isTrigger = true;

        gameObject.AddComponent<LerpScript>();
        gameObject.GetComponent<LerpScript>().positionOne = gameObject.transform;
        gameObject.GetComponent<LerpScript>().positionTwo = player.transform;
        gameObject.GetComponent<LerpScript>().thing = gameObject;

        gameObject.GetComponent<LerpScript>().rate = 0.025f;
        gameObject.GetComponent<LerpScript>().automated = true;

    }
}
