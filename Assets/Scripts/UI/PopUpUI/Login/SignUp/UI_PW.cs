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

    //비밀번호 유효 체크
    private void CheckPassWord()
    {
        string pw = PWfield.text;

        try
        {
            if (Regex.IsMatch(pw, @"^[a-z0-9_]{6,15}$", RegexOptions.None, TimeSpan.FromMilliseconds(250)))
            {
                PWtxt.text = " 사용가능한 비밀번호입니다.";
                isValid = true;
            }
            else
            {
                PWtxt.text = " 유효하지 않은 비밀번호 입니다. (영문 + 숫자 조합 6~15글자)";
                isValid = false;
            }

        }
        catch (RegexMatchTimeoutException)
        {
            PWtxt.text = " 유효하지 않은 비밀번호 입니다. (영문 + 숫자 조합 6~15글자)";
            isValid = false;
        }
    }

    //비밀번호 일치 체크
    private void SamePassWord()
    {
        string pw = PWfield.text;
        string check = PWCheckfield.text;

        if (isValid)
        {
            if (pw.Equals(check))
            {
                PWChecktxt.text = " 비밀번호와 일치합니다.";
                isCheck = true;
                password = pw;
                nextBtn.GetComponent<Button>().interactable = true;
                BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
            else
            {
                password = "";
                PWChecktxt.text = " 비밀번호와 일치하지 않습니다.";
                ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
        }

        else
        {
            PWChecktxt.text = "유효한 비밀번호를 입력해주세요.";
        }
    }
    

    private void NextBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        //비밀번호 유효한 입력 했는지 
        //유저 설정 화면으로 넘어가기
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
