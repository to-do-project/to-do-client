using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalContent : MonoBehaviour
{
    [SerializeField]
    private Image profile;

    [SerializeField]
    private Text nameTxt, managerTxt;

    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    public void SetImage(string color)
    {
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profile.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    public void SetName(string name)
    {
        if (nameTxt != null)
            nameTxt.text = name + " ��";
    }

    public void SetManager(bool manager)
    {
        if(manager)
        {
            managerTxt.text = "��ǥ ������";
        }
        else
        {
            managerTxt.text = "�ʴ� ���� ���";
        }
    }
}
