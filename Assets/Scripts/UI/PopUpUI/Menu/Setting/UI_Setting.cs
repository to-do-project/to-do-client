using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : UI_PopupMenu
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

        GetText((int)Texts.Version_txt).text = "v0.4.1";
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Notification_btn, (data) => { Managers.UI.ShowPopupUI<UI_Notification>("NotificationView", pathName); });

        SetBtn((int)Buttons.Sound_btn, (data) => { Managers.UI.ShowPopupUI<UI_Sound>("SoundView", pathName); });

        SetBtn((int)Buttons.Application_btn, (data) => { Managers.UI.ShowPopupUI<UI_Application>("ApplicationView", pathName); });

        SetBtn((int)Buttons.Announce_btn, (data) => { Managers.UI.ShowPopupUI<UI_Announce>("AnnounceView", pathName); });

        SetBtn((int)Buttons.Policy_btn, (data) => { Managers.UI.ShowPopupUI<UI_Policy>("PolicyView", pathName); });
    }

    private void Start()
    {
        Init();
    }
}
