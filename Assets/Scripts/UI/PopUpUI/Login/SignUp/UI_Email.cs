using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

[Serializable]
class RequestEmail
{
    public string email;
}


public class UI_Email : UI_SignUp
{
    Action<UnityWebRequest> callback;
    Response<string> Authres; //인증번호 발송 용
    Response<string> Checkres; //이메일 확인 용

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

    bool isCheck;

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

        Etxt = GetText((int)Texts.EmailCheck_txt);
        Einput = GetInputfiled((int)InputFields.Email_inputfield);
        
        callback -= ResponseAction;
        callback += ResponseAction;

    }

    void Start()
    {
        Init();

        
    }



    private void CheckBtnClick(PointerEventData data)
    {

        //이메일 입력 확인
        if (IsValidEmail(Einput.text))
        {
            Checkres = new Response<string>();
            Managers.Web.SendGetRequest("join/dupli/email?email=",Einput.text,callback);

            
        }
        else
        {
            Etxt.text = "올바른 이메일이 아닙니다.";
            isCheck = false;
            nextBtn.GetComponent<Button>().interactable = false;
            ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
        }


    }

    private void NextBtnClick(PointerEventData data)
    {
        //이메일 입력 확인
        if (isCheck)
        {
            //이메일 인증번호 발송 API
            loginScene.Email=Einput.text;

            RequestEmail val = new RequestEmail { email = loginScene.Email };
            Authres = new Response<string>();

            Managers.Web.SendPostRequest<string>("join/auth/new-num",val, callback);
            //Managers.UI.ShowPopupUI<UI_Auth>("AuthView", "SignUp");
        }
        
    }

    private void ResponseAction(UnityWebRequest request)
    {
        if (Authres != null)
        {
            Authres = JsonUtility.FromJson<Response<string>>(request.downloadHandler.text);



            if (Authres.isSuccess)
            {
                switch (Authres.code)
                {
                    case 1000:
                        Managers.UI.ShowPopupUI<UI_Auth>("AuthView", "SignUp");
                        break;
                   
                }
            }
            else
            {
                switch (Authres.code)
                {
                    case 4002:
                        Etxt.text = "서버 연결에 실패했습니다.";
                        break;
                    case 6010:
                        Etxt.text = "이메일 형식이 올바르지 않습니다.";
                        break;
                    case 6015:
                        Etxt.text = "이미 가입한 이메일입니다.";
                        break;
                    case 6017:
                        Etxt.text = "email을 입력해주세요.";
                        break;
                    case 6018:
                        Etxt.text = "메일을 전송하는데 실패했습니다.";
                        break;
                }
            }

            Authres = null;
        }
        else if(Checkres != null)
        {
            Checkres = JsonUtility.FromJson<Response<string>>(request.downloadHandler.text);

            Debug.Log(Checkres.code);
            Debug.Log(Checkres.isSuccess);

            if (Checkres.isSuccess)
            {
                switch (Checkres.code)
                {
                    case 1000:
                        Etxt.text = "사용 가능한 이메일입니다.";
                        isCheck = true;
                        nextBtn.GetComponent<Button>().interactable = true;
                        BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
                        break;
                    
                }
            }
            else
            {
                switch (Checkres.code) 
                {
                    case 4000:
                        break;
                    case 6010:
                        Etxt.text = "올바른 이메일이 아닙니다.";
                        isCheck = false;
                        nextBtn.GetComponent<Button>().interactable = false;
                        ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
                        break;
                    case 6015:
                        Etxt.text = "이미 존재하는 이메일입니다.";
                        isCheck = false;
                        nextBtn.GetComponent<Button>().interactable = false;
                        ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
                        break;
                }

            }

            Checkres = null;
        }
    }

    private bool IsValidEmail(string email)
    {
        //공란인지
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
