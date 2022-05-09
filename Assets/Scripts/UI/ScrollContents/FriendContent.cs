using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendContent : MonoBehaviour
{
    [SerializeField]
    private Image profile;

    [SerializeField]
    private Text nameTxt;

    GameObject parent;

    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public void SetImage(UI_Color.Colors color)
    {
        Debug.Log(color.ToString());
    }

    public void SetName(string name)
    {
        nameTxt.text = name;
    }

    public void DeleteFriend()
    {
        parent.GetComponent<UI_Friend>().DeleteFriend(this.gameObject);
    }
}
