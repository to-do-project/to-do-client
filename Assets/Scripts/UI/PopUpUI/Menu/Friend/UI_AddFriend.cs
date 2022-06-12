using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_AddFriend : UI_PopupMenu
{
    enum Buttons
    {
        Blind_btn,
        Accept_btn,
        Cancel_btn,
    }
    enum Texts
    {
        FriendName_txt,
        FriendLevel_txt,
    }

    Text friendNameTxt, friendLevelTxt;
    GameObject parent;
    UI_Friend friend;
    public int id { private get; set; }

    public override void Init()
    {
        base.Init();

        CameraSet();

        parent = GameObject.Find("FriendView(Clone)");
        if(parent != null)
        {
            friend = parent.GetComponent<UI_Friend>();
        } else
        {
            Debug.Log("parent가 NULL입니다. UI_AddFriend");
        }

        SetBtns();

        Bind<Text>(typeof(Texts));

        friendNameTxt = GetText((int)Texts.FriendName_txt);
        friendNameTxt.text = friend.Name;

        friendLevelTxt = GetText((int)Texts.FriendLevel_txt);
    }

    public void SetLevel(int level)
    {
        friendLevelTxt.text = "Lv. " + level.ToString();
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Blind_btn, ClosePopupUI);

        SetBtn((int)Buttons.Accept_btn, (data) => {
            if (CheckFriend())
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/FriendFadeView"));
                friend.AddFriend(friendNameTxt.text, id);
            }
            else
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/CantFindFadeView"));
            }
            Managers.UI.ClosePopupUI();
        });

        SetBtn((int)Buttons.Cancel_btn, ClosePopupUI);
    }

    bool CheckFriend()
    {
        //해당 닉네임을 가지는 친구가 친구 목록에 없고, 검색이 되면 true 반환, 아니면 false 반환
        //API에서 넘어오는 값으로 정하면 된다.
        return true;
    }

    private void Start()
    {
        Init();
    }
}