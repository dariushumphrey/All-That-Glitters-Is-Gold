using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextClearScript : MonoBehaviour
{
    public float clearTimer = 2f; //Time to wait before clearing Text
    internal float timerReset; //Holds original timer setting
    private Text textField;
    // Start is called before the first frame update
    void Start()
    {
        timerReset = clearTimer;
        textField = gameObject.GetComponent<Text>();       
    }

    // Update is called once per frame
    void Update()
    {
        //Clears text field after a short time if text is present
        if(textField.text.Length != 0)
        {
            clearTimer -= Time.deltaTime;
            if(clearTimer <= 0f)
            {
                textField.text = "";
                clearTimer = timerReset;
            }
        }
    }
}
