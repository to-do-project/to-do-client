using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DoneEdit : UI_Popup
{
    enum Buttons
    {
        done_btn,
        cancle_btn,
    }



    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        GameObject doneBtn = GetButton((int)Buttons.done_btn).gameObject;
        GameObject cancleBtn = GetButton((int)Buttons.cancle_btn).gameObject;

        BindEvent(doneBtn, DoneBtnClick, Define.TouchEvent.Touch);
        BindEvent(cancleBtn, CancleBtnClick, Define.TouchEvent.Touch);

    }

    void CancleBtnClick(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
    }

    void DoneBtnClick(PointerEventData data)
    {
        Managers.Scene.LoadScene(Define.Scene.Main);
    }
}
