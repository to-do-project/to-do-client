using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.EventSystems;


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
    InputField PWfield, PWCheckfield;
    Text PWtxt, PWChecktxt;
    bool isCheck = false;
    bool isValid = false;
    string password;

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

        PWfield = GetInputfiled((int)InputFields.PW_inputfield);
        PWfield.onEndEdit.AddListener(delegate { CheckPassWord(); });


        PWCheckfield = GetInputfiled((int)InputFields.PWCheck_inputfield);
        PWCheckfield.onEndEdit.AddListener(delegate { SamePassWord(); });

        PWtxt = GetText((int)Texts.PW_txt);
        PWChecktxt = GetText((int)Texts.PWCheck_txt);


    }

    private void Start()
    {
        Init();

    }

    //��й�ȣ ��ȿ üũ
    private void CheckPassWord()
    {
        string pw = PWfield.text;

        try
        {
            if (Regex.IsMatch(pw, @"^[a-z0-9_]{6,15}$", RegexOptions.None, TimeSpan.FromMilliseconds(250)))
            {
                PWtxt.text = " ��밡���� ��й�ȣ�Դϴ�.";
                isValid = true;
            }
            else
            {
                PWtxt.text = " ��ȿ���� ���� ��й�ȣ �Դϴ�. (���� + ���� ���� 6~15����)";
                isValid = false;
            }

        }
        catch (RegexMatchTimeoutException)
        {
            PWtxt.text = " ��ȿ���� ���� ��й�ȣ �Դϴ�. (���� + ���� ���� 6~15����)";
            isValid = false;
        }
    }

    //��й�ȣ ��ġ üũ
    private void SamePassWord()
    {
        string pw = PWfield.text;
        string check = PWCheckfield.text;

        if (isValid)
        {
            if (pw.Equals(check))
            {
                PWChecktxt.text = " ��й�ȣ�� ��ġ�մϴ�.";
                isCheck = true;
                password = pw;
                nextBtn.GetComponent<Button>().interactable = true;
                BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
            else
            {
                password = "";
                PWChecktxt.text = " ��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
                ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
        }

        else
        {
            PWChecktxt.text = "��ȿ�� ��й�ȣ�� �Է����ּ���.";
        }
    }
    

    private void NextBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        //��й�ȣ ��ȿ�� �Է� �ߴ��� 
        //���� ���� ȭ������ �Ѿ��
        if (isCheck&&isValid)
        {
            if (!string.IsNullOrWhiteSpace(password))
            {
                loginScene.Pw = password;
                

                Managers.UI.ShowPopupUI<UI_NicknameSet>("NicknameView", "UserInfo");
                Managers.UI.CLoseExceptLastPopupUI();

            }
        }

    }
}
