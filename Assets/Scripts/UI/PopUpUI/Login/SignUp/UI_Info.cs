using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Info : UI_SignUp
{
    enum Texts
    {
        //info_txt,
    }

    enum Buttons 
    {
        next_btn,
        Back_btn,
    }


    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GameObject nextBtn = GetButton((int)Buttons.next_btn).gameObject;
        BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);
    }
    private void Start()
    {
        Init();
    }

    public void NextBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        Managers.UI.ShowPopupUI<UI_Email>("EmailView", "SignUp");
    }

}
