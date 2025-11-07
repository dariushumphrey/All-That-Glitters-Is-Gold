using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerpScript : MonoBehaviour
{
    public Color colorOne = Color.gray;
    public Color colorTwo = Color.gray;
    public bool enemyUse = false; //Lerps Material color used for Enemies' eyes if true
    public int materialIndex = 0; //Index used to find Material
    private Renderer subject;
    private Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        subject = GetComponent<Renderer>();
        materials = subject.materials;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyUse)
        {
            subject.materials[materialIndex].color = Color.Lerp(colorOne, colorTwo, Mathf.PingPong(Time.time, 1));
        }

        else
        {
            subject.material.color = Color.Lerp(colorOne, colorTwo, Mathf.PingPong(Time.time, 1));
        }
    }
}
