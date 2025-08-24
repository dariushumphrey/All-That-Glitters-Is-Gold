using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpScript : MonoBehaviour
{
    public Transform positionOne, positionTwo;
    public GameObject thing;
    public float rate = 0.1f;
    public float lerpProgress = 0f;
    public float lerpSpeed = 2f;
    public bool automated;
    public float targetSpeed = 0f;
    public bool accelerating, decelerating = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(automated)
        {
            lerpProgress += Time.deltaTime * lerpSpeed;
            thing.transform.position = Vector3.Lerp(positionOne.transform.position, positionTwo.transform.position, lerpProgress);

            if(lerpProgress >= 1f)
            {
                lerpProgress = 0f;
            }

            if(accelerating)
            {
                lerpSpeed += Time.deltaTime;
                if(lerpSpeed >= targetSpeed)
                {
                    lerpSpeed = targetSpeed;
                    accelerating = false;
                }
            }

            if (decelerating)
            {
                lerpSpeed -= Time.deltaTime;
                if (lerpSpeed <= targetSpeed)
                {
                    lerpSpeed = targetSpeed;
                    decelerating = false;
                }
            }
        }

        else
        {
            thing.transform.position = Vector3.Lerp(positionOne.transform.position, positionTwo.transform.position, rate);
        }
    }
}
