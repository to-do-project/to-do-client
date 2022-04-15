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
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);

        nextBtn = GetButton((int)Buttons.next_btn).gameObject;
        nextBtn.GetComponent<Button>().interactable = false;

        authCheckBtn = GetButton((int)Buttons.AuthCheck_btn).gameObject;
        BindEvent(authCheckBtn, AuthCheckBtnClick, Define.TouchEvent.Touch);


        
    }

    private void Start()
    {
        Init();

    }

    private void AuthCheckBtnClick(PointerEventData data)
    {
        //������ȣ ���� Ȯ��
        nextBtn.GetComponent<Button>().interactable = true;
        BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);

    }


    private void NextBtnClick(PointerEventData data)
    {
        //�����Ϸ�ƴ��� Ȯ��

        Managers.UI.ShowPopupUI<UI_PW>("PWView", "SignUp");
    }
}
