using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDeliveryScript : MonoBehaviour
{
    public float healthPercent = 100f;
    public int healthAdd;
    public LayerMask contactOnly;
    public ParticleSystem acceptEffect;
    public GameObject lootFocusCircle;

    // Start is called before the first frame update
    void Start()
    {
        SetupDelivery();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupDelivery()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, contactOnly))
        {
            if (hit.collider.gameObject.layer == 8)
            {
                transform.position = hit.point + (hit.normal * 1.1f);
            }
        }

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, contactOnly))
        {
            if (hit.point != null)
            {
                GameObject deliveryItemCircle = Instantiate(lootFocusCircle, hit.point + (hit.normal * 0.01f), Quaternion.FromToRotation(Vector3.up, hit.normal));
                deliveryItemCircle.GetComponent<Renderer>().material.color = Color.green;
                deliveryItemCircle.name = lootFocusCircle.name;
                deliveryItemCircle.transform.parent = gameObject.transform;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<PlayerStatusScript>().playerHealth != other.gameObject.GetComponent<PlayerStatusScript>().playerHealthMax)
            {
                healthPercent /= 100f;
                healthPercent *= other.gameObject.GetComponent<PlayerStatusScript>().playerHealthMax;
                healthAdd = (int)healthPercent;

                other.gameObject.GetComponent<PlayerStatusScript>().playerHealth += healthAdd;

                var main = acceptEffect.GetComponent<ParticleSystem>().main;
                main.startColor = Color.green;
                Instantiate(acceptEffect, other.gameObject.transform.position, other.gameObject.transform.rotation);

                gameObject.SetActive(false);
            }         
        }
    }
}
