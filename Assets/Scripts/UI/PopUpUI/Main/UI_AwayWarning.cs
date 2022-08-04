using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AwayWarning : UI_Popup
{
    enum Buttons
    {
        check_btn
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GameObject checkBtn = GetButton((int)Buttons.check_btn).gameObject;
        BindEvent(checkBtn, CheckBtnClick);

    }

    private void CheckBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        Managers.UI.ClosePopupUI();
    }
}
