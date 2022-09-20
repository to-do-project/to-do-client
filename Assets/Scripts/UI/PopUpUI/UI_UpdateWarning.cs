using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UpdateWarning : UI_Popup
{
    enum Buttons
    {
        check_btn,
    }

    void Start()
    {
        
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GameObject checkBtn = GetButton((int)Buttons.check_btn).gameObject;
        BindEvent(checkBtn, CheckBtnClick);
    }

    void CheckBtnClick(PointerEventData data)
    {
        Application.Quit();
    }

}
