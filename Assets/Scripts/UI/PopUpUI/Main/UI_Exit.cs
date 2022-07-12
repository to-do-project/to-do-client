using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.EventSystems;

public class UI_Exit : UI_Popup
{
    enum Buttons
    {
        exit_btn,
        remain_btn,
    }

    void Start()
    {
        Init();
    }


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        GameObject exitBtn = GetButton((int)Buttons.exit_btn).gameObject;
        GameObject remainBtn = GetButton((int)Buttons.remain_btn).gameObject;

        BindEvent(exitBtn, ExitBtnClick, Define.TouchEvent.Touch);
        BindEvent(remainBtn, RemainBtnClick, Define.TouchEvent.Touch);

    }


    private void ExitBtnClick(PointerEventData evt)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        Application.Quit();
    }

    private void RemainBtnClick(PointerEventData evt)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        Managers.UI.ClosePopupUI();
    }
}
