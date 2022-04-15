using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.EventSystems;

public class UI_Email : UI_SignUp
{

    enum Texts
    {
        Etitle_txt,
        EmailCheck_txt,
    }

    enum InputFields
    {
        Email_inputfield,

    }

    enum Buttons
    {
        next_btn,
        EmailCheck_btn,
        Back_btn,
    }

    GameObject nextBtn;
    Text Etxt;
    InputField Einput;

    public override void Init()
    {
        base.Init();

        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GameObject checkBtn = GetButton((int)Buttons.EmailCheck_btn).gameObject;
        BindEvent(checkBtn, CheckBtnClick, Define.TouchEvent.Touch);

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);

        nextBtn = GetButton((int)Buttons.next_btn).gameObject;
        
        nextBtn.GetComponent<Button>().interactable = false;
    }

    void Start()
    {
        Init();

        
    }

    bool isCheck;

    private void CheckBtnClick(PointerEventData data)
    {
        //�̸��� �Է� Ȯ��
        Etxt = GetText((int)Texts.EmailCheck_txt);
        Einput = GetInputfiled((int)InputFields.Email_inputfield);

        if (IsValidEmail(Einput.text))
        {
            Etxt.text = "��� ������ �̸����Դϴ�.";
            isCheck = true;
            nextBtn.GetComponent<Button>().interactable = true;
            BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
        }
        else
        {
            Etxt.text = "��ȿ�� �̸����� �Է����ּ���.";
            isCheck = false;
            nextBtn.GetComponent<Button>().interactable = false;
            ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
        }


    }

    private void NextBtnClick(PointerEventData data)
    {
        //�̸��� �Է� Ȯ��
        if (isCheck)
        {
            loginScene.Email=Einput.text;
            Managers.UI.ShowPopupUI<UI_Auth>("AuthView", "SignUp");
        }
        
    }


    private bool IsValidEmail(string email)
    {
        //��������
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }
        try
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch(RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
