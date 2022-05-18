using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Profile : UI_PopupMenu
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

        CameraSet();

        SetBtns();

    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Profile_btn, (data) => { Managers.UI.ShowPopupUI<UI_Color>("ColorView", pathName); });

        SetBtn((int)Buttons.NickChange_btn, (data) => { Managers.UI.ShowPopupUI<UI_NickChange>("NickChangeView", pathName); });

        SetBtn((int)Buttons.PswdChange_btn, (data) => { Managers.UI.ShowPopupUI<UI_PswdChange>("PswdChangeView", pathName); });

        SetBtn((int)Buttons.Logout_btn, (data) => { Debug.Log("*Clicked Button* Logout"); });

        SetBtn((int)Buttons.Delete_btn, (data) => { Managers.UI.ShowPopupUI<UI_Delete>("DeleteView", pathName); });
    }

    void Start()
    {
        Init();
    }
}
