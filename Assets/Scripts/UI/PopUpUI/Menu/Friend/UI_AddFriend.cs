using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_AddFriend : UI_Popup
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

    private void CameraSet()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        GameObject blindBtn = GetButton((int)Buttons.Blind_btn).gameObject;
        BindEvent(blindBtn, ClosePopupUI, Define.TouchEvent.Touch);

        GameObject acceptBtn = GetButton((int)Buttons.Accept_btn).gameObject;
        BindEvent(acceptBtn, AcceptBtnClick, Define.TouchEvent.Touch);

        GameObject cancelBtn = GetButton((int)Buttons.Cancel_btn).gameObject;
        BindEvent(cancelBtn, ClosePopupUI, Define.TouchEvent.Touch);
    }

    private void Start()
    {
        Init();
    }

    public void AcceptBtnClick(PointerEventData data)
    {
        friend.AddFriend(friendNameTxt.text);
        Managers.UI.ClosePopupUI();
    }
}
