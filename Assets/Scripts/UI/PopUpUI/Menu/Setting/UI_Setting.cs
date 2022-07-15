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

        GetText((int)Texts.Version_txt).text = "v1.0";
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, (data) => { Managers.Sound.PlayNormalButtonClickSound(); ClosePopupUI(); });

        SetBtn((int)Buttons.Notification_btn, (data) => { Managers.UI.ShowPopupUI<UI_Notification>("NotificationView", pathName); 
                                                          Managers.Sound.PlayNormalButtonClickSound(); });

        SetBtn((int)Buttons.Sound_btn, (data) => { Managers.UI.ShowPopupUI<UI_Sound>("SoundView", pathName); 
                                                   Managers.Sound.PlayNormalButtonClickSound(); });

        SetBtn((int)Buttons.Application_btn, (data) => { Managers.UI.ShowPopupUI<UI_Application>("ApplicationView", pathName); 
                                                         Managers.Sound.PlayNormalButtonClickSound(); });

        SetBtn((int)Buttons.Announce_btn, (data) => { Managers.UI.ShowPopupUI<UI_Announce>("AnnounceView", pathName); 
                                                      Managers.Sound.PlayNormalButtonClickSound(); });

        SetBtn((int)Buttons.Policy_btn, (data) => { Managers.UI.ShowPopupUI<UI_Policy>("PolicyView", pathName); 
                                                    Managers.Sound.PlayNormalButtonClickSound(); });
    }

    private void Start()
    {
        Init();
    }
}
