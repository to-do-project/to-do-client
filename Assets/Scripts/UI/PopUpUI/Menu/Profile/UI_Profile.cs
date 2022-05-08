using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Profile : UI_Popup
{
    enum Buttons
    {
        Back_btn,
        Profile_btn,
        NickChange_btn,
        PswdChange_btn,
        Logout_btn,
        Delete_btn,
    }

    string pathName = "Menu/Profile";

    public override void Init()
    {
        base.Init();

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

        setBtns();

    }

    private void setBtns()
    {
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, BackBtnClick, Define.TouchEvent.Touch);

        GameObject profileBtn = GetButton((int)Buttons.Profile_btn).gameObject;
        BindEvent(profileBtn, ProfileBtnClick, Define.TouchEvent.Touch);

        GameObject nickChangeBtn = GetButton((int)Buttons.NickChange_btn).gameObject;
        BindEvent(nickChangeBtn, NickChangeBtnClick, Define.TouchEvent.Touch);

        GameObject pswdChangeBtn = GetButton((int)Buttons.PswdChange_btn).gameObject;
        BindEvent(pswdChangeBtn, PswdChangeBtnClick, Define.TouchEvent.Touch);

        GameObject logoutBtn = GetButton((int)Buttons.Logout_btn).gameObject;
        BindEvent(logoutBtn, LogoutBtnClick, Define.TouchEvent.Touch);

        GameObject deleteBtn = GetButton((int)Buttons.Delete_btn).gameObject;
        BindEvent(deleteBtn, DeleteBtnClick, Define.TouchEvent.Touch);
    }

    #region ButtonEvents
    public void BackBtnClick(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
    }
    public void ProfileBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Color>("ColorView", pathName);
    }
    public void NickChangeBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_NickChange>("NickChangeView", pathName);
    }
    public void PswdChangeBtnClick(PointerEventData data)
    {
        Debug.Log("*Clicked Button* PswdChange");
    }
    public void LogoutBtnClick(PointerEventData data)
    {
        Debug.Log("*Clicked Button* Logout");
    }
    public void DeleteBtnClick(PointerEventData data)
    {
        Debug.Log("*Clicked Button* Delete");
    }
    #endregion

    void Start()
    {
        Init();
    }
}
