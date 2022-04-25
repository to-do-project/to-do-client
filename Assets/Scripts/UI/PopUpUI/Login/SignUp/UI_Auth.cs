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
        AuthResend_btn,
    }

    enum Texts
    {
        Atitle_txt,
        AuthCheck_txt,
        Timer,
    }

    enum InputFields
    {
        Auth_inputfield,
    }

    GameObject nextBtn;
    GameObject authCheckBtn;
    Text authChecktxt;
    Text timer;

    bool timeover = false;
    float time=0f;
    float maxTime = 300f;

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

        authChecktxt = GetText((int)Texts.AuthCheck_txt);
        timer = GetText((int)Texts.Timer);

        time = 0f;

        GameObject resendBtn = GetButton((int)Buttons.AuthResend_btn).gameObject;
        BindEvent(resendBtn, ResendBtnClick, Define.TouchEvent.Touch);

        Text Atitle = GetText((int)Texts.Atitle_txt);
        Atitle.text = loginScene.Email + Atitle.text;
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        AuthTimer();
    }

    private void AuthTimer()
    {
        time += Time.deltaTime;

        float remain = maxTime - time;

        if (remain < 0.1f)
        {
            timer.text = "00:00";
            timeover = true;
        }

        timer.text = ((int)remain/60).ToString()
            +":"+((int)remain%60);

    }

    private void AuthCheckBtnClick(PointerEventData data)
    {
        //인증번호 동일 확인

        authChecktxt.text = " 인증되었습니다.";
        nextBtn.GetComponent<Button>().interactable = true;
        BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);

    }


    private void NextBtnClick(PointerEventData data)
    {
        //인증완료됐는지 확인
        if (timeover)
        {

        }
        else
        {
            Managers.UI.ShowPopupUI<UI_PW>("PWView", "SignUp");
        }
        
    }

    private void ResendBtnClick(PointerEventData data)
    {
        //인증번호 API 다시 호출

        //타이머 다시 돌리기
        time = 0f;
        nextBtn.GetComponent<Button>().interactable = false;
    }
}
