using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotComplete : UI_Fade
{
    [SerializeField]
    private Text text;

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
