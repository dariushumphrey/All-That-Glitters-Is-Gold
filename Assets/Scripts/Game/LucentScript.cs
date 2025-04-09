using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucentScript : MonoBehaviour
{
    public int lucentGift;

    private float shatterPercent = 150f;
    private int shatterDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lucentGift <= 0)
        {
            Vector3 epicenter = transform.position;
            Collider[] affected = Physics.OverlapSphere(transform.position, 5f);
            foreach (Collider hit in affected)
            {
                Rigidbody inflict = hit.GetComponent<Rigidbody>();
                if (inflict != null)
                {
                    if (inflict.GetComponent<EnemyHealthScript>() != null)
                    {
                        inflict.GetComponent<EnemyHealthScript>().inflictDamage(shatterDamage);
                    }
                }
            }

            Destroy(gameObject);
        }
    }
    
    public void ShatterCalculation()
    {
        shatterPercent /= 100;
        shatterPercent *= lucentGift;
        shatterDamage = (int)shatterPercent;
    }
}
