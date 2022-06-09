using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendContent : UI_PopupMenu
{
    enum Buttons
    {
        FriendPlanet_btn,
    }

    [SerializeField]
    private Image profile;

    [SerializeField]
    private Text nameTxt;

    private long id;
    GameObject parent;

    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public void SetImage(UI_Color.Colors color)
    {
        Debug.Log(color.ToString());
    }

    public void SetID(long id)
    {
        this.id = id;
    }

    public void SetName(string name)
    {
        nameTxt.text = name;
    }

    public void DeleteFriend()
    {
        parent.GetComponent<UI_Friend>().DeleteFriend(this.gameObject);
    }

    void Start()
    {
        Bind<Button>(typeof(Buttons));
        SetBtn((int)Buttons.FriendPlanet_btn, (data) => {
            Debug.Log("模备 青己 愁矾啊扁 >> " + name);
        });
    }
}
