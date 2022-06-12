using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetContent : MonoBehaviour
{
    public Text title;

    long id;

    public void ChangeText(string text)
    {
        this.title.text = text;
    }

    public void SetId(long id)
    {
        this.id = id;
    }

    public void BtnClicked()
    {
        Debug.Log("목표 시작하기");
    }
}
