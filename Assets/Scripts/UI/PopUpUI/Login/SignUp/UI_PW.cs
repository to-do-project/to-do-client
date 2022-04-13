using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PW : UI_SignUp
{
    enum Buttons
    {
        next_btn,
        Back_btn,
    }

    enum Texts
    {
        Ptitle_txt,
        PW_txt,
        PWCheck_txt,
    }

    enum InputFields
    {
        PW_inputfield,
        PWCheck_inputfield,
    }

    GameObject nextBtn;

    public override void Init()
    {
        base.Init();

        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);

        nextBtn = GetButton((int)Buttons.next_btn).gameObject;
        BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);


    }

    private void Start()
    {
        Init();

    }


    private void NextBtnClick(PointerEventData data)
    {
        //비밀번호 유효한 입력 했는지 

        //유저 설정 화면으로 넘어가기

        Managers.UI.ShowPopupUI<UI_PW>("LoginView");
    }
}
