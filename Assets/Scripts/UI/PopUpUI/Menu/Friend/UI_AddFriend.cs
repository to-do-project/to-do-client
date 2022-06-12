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
            Debug.Log("parent�� NULL�Դϴ�. UI_AddFriend");
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
        //�ش� �г����� ������ ģ���� ģ�� ��Ͽ� ����, �˻��� �Ǹ� true ��ȯ, �ƴϸ� false ��ȯ
        //API���� �Ѿ���� ������ ���ϸ� �ȴ�.
        return true;
    }

    private void Start()
    {
        Init();
    }
}