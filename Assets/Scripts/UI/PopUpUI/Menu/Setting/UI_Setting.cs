using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Setting : UI_Popup
{

    enum Buttons
    {
        Back_btn,
        Notification_btn,
        Sound_btn,
        Application_btn,
        Announce_btn,
        Policy_btn,
    }

    enum Texts
    {
        Version_txt,
    }

    string pathName = "Menu/Setting";

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Text>(typeof(Texts));

        GetText((int)Texts.Version_txt).text = "0.ver";
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

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, BackBtnClick, Define.TouchEvent.Touch);

        GameObject notificationBtn = GetButton((int)Buttons.Notification_btn).gameObject;
        BindEvent(notificationBtn, NotificationBtnClick, Define.TouchEvent.Touch);

        GameObject soundBtn = GetButton((int)Buttons.Sound_btn).gameObject;
        BindEvent(soundBtn, SoundBtnClick, Define.TouchEvent.Touch);

        GameObject applicationBtn = GetButton((int)Buttons.Application_btn).gameObject;
        BindEvent(applicationBtn, ApplicationBtnClick, Define.TouchEvent.Touch);

        GameObject announceBtn = GetButton((int)Buttons.Announce_btn).gameObject;
        BindEvent(announceBtn, AnnounceBtnClick, Define.TouchEvent.Touch);

        GameObject policyBtn = GetButton((int)Buttons.Policy_btn).gameObject;
        BindEvent(policyBtn, PolicyBtnClick, Define.TouchEvent.Touch);
    }

    #region ButtonEvents
    public void BackBtnClick(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
    }
    public void NotificationBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Notification>("NotificationView", pathName);
    }
    public void SoundBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Sound>("SoundView", pathName);
    }
    public void ApplicationBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Application>("ApplicationView", pathName);
    }
    public void AnnounceBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Announce>("AnnounceView", pathName);
    }
    public void PolicyBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Policy>("PolicyView", pathName);
    }
    #endregion

    private void Start()
    {
        Init();
    }
}
