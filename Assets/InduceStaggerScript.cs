using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InduceStaggerScript : MonoBehaviour
{
    Vector3 epicenterDistance;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            if(!other.gameObject.GetComponent<ReplevinScript>().amBoss)
            {
                epicenterDistance = transform.position - other.gameObject.transform.position;

                if(other.gameObject.GetComponent<ReplevinScript>().self.isOnNavMesh)
                {
                    other.gameObject.GetComponent<ReplevinScript>().self.ResetPath();
                }

                other.gameObject.GetComponent<ReplevinScript>().self.velocity = Vector3.zero;

                other.gameObject.GetComponent<ReplevinScript>().staggerPosition = transform.position - epicenterDistance * 3f;

                other.gameObject.GetComponent<ReplevinScript>().staggered = true;

                StartCoroutine(DeactivateImmediate());
            }       
        }
    }

    public IEnumerator DeactivateImmediate()
    {
        yield return null;
        gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }
}
