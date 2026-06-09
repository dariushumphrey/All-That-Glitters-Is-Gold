using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatchNotesManagerScript : MonoBehaviour
{
    public Text displayText;
    public TextAsset[] patchNotes;
    public ScrollRect inspectScroll;

    private string patchText;

    // Start is called before the first frame update
    void Start()
    {
        patchText = patchNotes[patchNotes.Length - 1].text;
        displayText.text = patchText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNotes(int note)
    {
        patchText = patchNotes[note].text;
        displayText.text = patchText;

        inspectScroll.verticalNormalizedPosition = 1f;

    }
}
