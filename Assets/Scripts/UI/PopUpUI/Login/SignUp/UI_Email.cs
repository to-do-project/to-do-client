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

    public override void Init()
    {
        base.Init();

        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GameObject checkBtn = GetButton((int)Buttons.EmailCheck_btn).gameObject;
        BindEvent(checkBtn, CheckBtnClick, Define.UIEvent.Click);

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.UIEvent.Click);

        nextBtn = GetButton((int)Buttons.next_btn).gameObject;
        BindEvent(nextBtn, NextBtnClick, Define.UIEvent.Click);
        nextBtn.GetComponent<Button>().interactable = false;
    }

    void Start()
    {
        Init();

        
    }

    bool isCheck;

    public void CheckBtnClick(PointerEventData data)
    {
        //�̸��� �Է� Ȯ��
        Text Etxt = GetText((int)Texts.EmailCheck_txt);
        InputField Einput = GetInputfiled((int)InputFields.Email_inputfield);

        if (IsValidEmail(Einput.text))
        {
            Etxt.text = "��� ������ �̸����Դϴ�.";
            isCheck = true;
            nextBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            Etxt.text = "��ȿ�� �̸����� �Է����ּ���.";
            isCheck = false;
            nextBtn.GetComponent<Button>().interactable = false;
        }


    }

    public void NextBtnClick(PointerEventData data)
    {
        //�̸��� �Է� Ȯ��
        if (isCheck)
        {
            
            Managers.UI.ShowPopupUI<UI_Auth>("AuthView", "SignUp");
        }
        
    }


    public bool IsValidEmail(string email)
    {
        //��������
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }
        try
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch(RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
