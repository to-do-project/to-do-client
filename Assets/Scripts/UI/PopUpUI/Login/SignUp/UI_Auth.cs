using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class RequestAuth
{
    public string email;
    public string authNum;
}

public class UI_Auth : UI_SignUp
{
    Action<UnityWebRequest> callback;
    Response<string> Authres;
    Response<string> Sendres;

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
    InputField Ainputfield;
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

        Ainputfield = GetInputfiled((int)InputFields.Auth_inputfield);

        time = 0f;

        GameObject resendBtn = GetButton((int)Buttons.AuthResend_btn).gameObject;
        BindEvent(resendBtn, ResendBtnClick, Define.TouchEvent.Touch);

        Text Atitle = GetText((int)Texts.Atitle_txt);
        Atitle.text = loginScene.Email + Atitle.text;

        callback -= ResponseAction;
        callback += ResponseAction;
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
        else
        {
            timer.text = ((int)remain / 60).ToString()
            + ":" + ((int)remain % 60);
        }


    }

    private void AuthCheckBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        //인증번호 미입력시
        if (string.IsNullOrWhiteSpace(Ainputfield.text))
        {
            authChecktxt.text = "인증번호를 입력해주세요.";
        }
        else
        {
            //인증번호 검증 API
            RequestAuth val = new RequestAuth { email = loginScene.Email, authNum = Ainputfield.text };
            Authres = new Response<string>();

            Managers.Web.SendPostRequest<string>("join/auth/check-num", val, callback);
    
        }

    }

    private void ResponseAction(UnityWebRequest request)
    {
        if (Authres != null)
        {
            Authres = JsonUtility.FromJson<Response<string>>(request.downloadHandler.text);

            Debug.Log(Authres.code);
            Debug.Log(Authres.isSuccess);

            if (Authres.isSuccess)
            {
                switch (Authres.code)
                {
                    case 1000:
                        nextBtn.GetComponent<Button>().interactable = true;
                        BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
                        break;
                }
            }
            else
            {
                authChecktxt.text = "잘못된 인증번호입니다. 다시 입력해주세요.";
                nextBtn.GetComponent<Button>().interactable = false;
                ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
       
            }
            Authres = null;
        }
        else if (Sendres != null)
        {
            if (Sendres.isSuccess)
            {
                switch (Sendres.code) 
                {
                    case 1000:
                        authChecktxt.text = "인증번호를 다시 전송했습니다.";
                        nextBtn.GetComponent<Button>().interactable = false;
                        ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
                        break;
                }

            }
            else
            {
                authChecktxt.text = "인증번호를 전송하는데 실패했습니다.";
                nextBtn.GetComponent<Button>().interactable = false;
                ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);

            }
            Sendres = null;
        }
        
    }

    private void NextBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        //인증완료됐는지 확인
        if (timeover)
        {
            nextBtn.GetComponent<Button>().interactable = false;
            ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
        }
        else
        {
            Managers.UI.ShowPopupUI<UI_PW>("PWView", "SignUp");
        }
        
    }

    private void ResendBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        //인증번호 API 다시 호출
        Ainputfield.text = "";
        RequestEmail val = new RequestEmail { email = loginScene.Email };
        Sendres = new Response<string>();

        Managers.Web.SendPostRequest<string>("join/auth/new-num", val, callback);


        //타이머 다시 돌리기
        time = 0f;
        nextBtn.GetComponent<Button>().interactable = false;
        ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
        timeover = false;
    }
}
