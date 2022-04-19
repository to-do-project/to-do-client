using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PWAuth : UI_PWfind
{
    enum Buttons
    {
        Back_btn,
        login_btn,
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);

        GameObject loginBtn = GetButton((int)Buttons.login_btn).gameObject;
        BindEvent(loginBtn, loginBtnClick,Define.TouchEvent.Touch);
    }

    void Start()
    {
        Init();
    }

    private void loginBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPanelUI<UI_Login>("LoginView");
    }


}
