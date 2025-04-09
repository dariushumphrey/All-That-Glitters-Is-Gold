using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerpScript : MonoBehaviour
{
    public Color colorOne = Color.gray;
    public Color colorTwo = Color.gray;
    private Renderer subject;

    // Start is called before the first frame update
    void Start()
    {
        subject = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        subject.material.color = Color.Lerp(colorOne, colorTwo, Mathf.PingPong(Time.time, 1));
    }
}
