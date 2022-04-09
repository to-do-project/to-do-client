using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Auth : UI_SignUp
{
    enum Buttons
    {
        next_btn,
        AuthCheck_btn,
        Back_btn,
    }

    enum Texts
    {
        Atitle_txt,
        AuthCheck_txt,
    }

    enum InputFields
    {
        Auth_inputfield,
    }

    GameObject nextBtn;
    GameObject authCheckBtn;

    public override void Init()
    {
        base.Init();

        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.UIEvent.Click);

        nextBtn = GetButton((int)Buttons.next_btn).gameObject;
        BindEvent(nextBtn, NextBtnClick, Define.UIEvent.Click);

        authCheckBtn = GetButton((int)Buttons.AuthCheck_btn).gameObject;
        BindEvent(authCheckBtn, AuthCheckBtnClick, Define.UIEvent.Click);


        
    }

    private void Start()
    {
        Init();

    }

    public void AuthCheckBtnClick(PointerEventData data)
    {
        //인증번호 동일 확인


    }


    public void NextBtnClick(PointerEventData data)
    {
        //인증완료됐는지 확인

        Managers.UI.ShowPopupUI<UI_PW>("PWView", "SignUp");
    }
}
