using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpScript : MonoBehaviour
{
    //positionOne - first position used for Lerp behavior
    //positionTwo - last position used for Lerp behavior
    public Transform positionOne, positionTwo;
    public GameObject thing; //Object to use for Lerp movement

    //rate, lerpProgress - Represents progress through Lerp movement
    public float rate = 0.1f;
    public float lerpProgress = 0f;
    public float lerpSpeed = 2f;
    public bool automated; //Lerp progresses every frame if true
    public float targetSpeed = 0f; //Speed to reach when accelerating, decelerating

    //accelerating - increases Lerp speed if true
    //decelerating - decreases Lerp speed if true
    public bool accelerating, decelerating = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Makes object Lerp automatically if true, Lerp manually if false
        if(automated)
        {
            lerpProgress += Time.deltaTime * lerpSpeed;
            thing.transform.position = Vector3.Lerp(positionOne.transform.position, positionTwo.transform.position, lerpProgress);

            //Restarts Lerp movement when reaching the end
            if(lerpProgress >= 1f)
            {
                lerpProgress = 0f;
            }

            //Increases Lerp speed to target speed if true
            if(accelerating)
            {
                lerpSpeed += Time.deltaTime;
                if(lerpSpeed >= targetSpeed)
                {
                    lerpSpeed = targetSpeed;
                    accelerating = false;
                }
            }

            //Decreases Lerp speed to target speed if true
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
