using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManagerScript : MonoBehaviour
{
    public float fadeAccelerant = 2f;
    public Image fadeImage;
    public bool fadeToBlack, fadeToGame;
    public Sprite[] screenshots;
    private float alphaValue;
    private Color fadeImageColor;
    private bool directControl;

    // Start is called before the first frame update
    void Start()
    {
        fadeImageColor = fadeImage.color;
        alphaValue = fadeImage.color.a;
        fadeToGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        alphaValue = Mathf.Clamp(alphaValue, 0, 1);

        if(!directControl)
        {
            fadeImageColor.a = alphaValue;
            fadeImage.color = fadeImageColor;
        }

        if(fadeToGame)
        {
            alphaValue -= Time.deltaTime * fadeAccelerant;
            if(alphaValue <= 0)
            {
                fadeToGame = false;
            }
        }

        if (fadeToBlack)
        {
            alphaValue += Time.deltaTime * fadeAccelerant;

            if(directControl)
            {
                fadeImage.color = Color.Lerp(fadeImageColor, Color.white, alphaValue);
            }

            if (alphaValue >= 1)
            {
                fadeToBlack = false;
            }
        }
    }

    public void GameInitializeTransition()
    {
        directControl = true;
        int background = Random.Range(0, screenshots.Length);
        fadeImage.sprite = screenshots[background];

        fadeToBlack = true;
    }
}
