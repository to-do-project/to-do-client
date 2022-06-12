using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GoalModify : UI_Popup
{
    enum Buttons
    {
        exit_btn,
        todoStore_btn,
        todoDelete_btn,
        check_btn,
    }

    enum Texts
    {
        date_txt,
    }

    enum Toggles
    {
        open_toggle,
    }


    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Toggle>(typeof(Toggles));

        GameObject checkbtn = GetButton((int)Buttons.check_btn).gameObject;
        BindEvent(checkbtn, CheckBtnClick);

        GameObject backBtn = GetButton((int)Buttons.exit_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI);
    }

    private void CheckBtnClick(PointerEventData data)
    {

        ClosePopupUI();
    }

}
