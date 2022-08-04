using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ExitEdit : UI_Popup
{
    enum Buttons
    {
        exit_btn,
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
        GameObject exitBtn = GetButton((int)Buttons.exit_btn).gameObject;
        GameObject cancleBtn = GetButton((int)Buttons.cancle_btn).gameObject;

        BindEvent(exitBtn, ExitBtnClick, Define.TouchEvent.Touch);
        BindEvent(cancleBtn, CancleBtnClick, Define.TouchEvent.Touch);

    }

    void CancleBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        Managers.UI.ClosePopupUI();
    }

    void ExitBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        Managers.Scene.Clear();
        Managers.Todo.SendMainGoalRequest(Managers.Player.GetString(Define.USER_ID));
        UI_Load.Instance.InstantLoad("Main");

        //Managers.Scene.LoadScene(Define.Scene.Main);
    }
}
