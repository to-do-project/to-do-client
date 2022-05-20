using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginData
{
    public string email;
    public string password;
    public string deviceToken;
}

public class LoginResult
{
    public long userId;
    public long planetId;
    public string email;
    public string nickname;
    public string characterColor;
    public string profileColor;
    public int point;
    public int missionStatus;
    public string deviceToken;
}

public class UI_Login : UI_Panel
{
    Action<UnityWebRequest> callback;
    Response<LoginResult> res;

    protected LoginScene loginScene;

    enum Buttons
    {
        Login_btn,
        FindPW_btn,
        SignUp_btn,
        //GLogin_btn,
    }

    enum InputFields
    {
        ID_inputfield,
        PW_inputfield,
    }

    enum Texts
    {
        failMessage_txt,
    }

    Text failMessage;

    public override void Init()
    {
        base.Init();
        Managers.UI.CloseAllPopupUI();

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }

        Bind<Button>(typeof(Buttons));
        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));

        GameObject signupBtn = GetButton((int)Buttons.SignUp_btn).gameObject;
        BindEvent(signupBtn, SignUpBtnClick, Define.TouchEvent.Touch);

        GameObject findpwBtn = GetButton((int)Buttons.FindPW_btn).gameObject;
        BindEvent(findpwBtn, FindPWBtnClick, Define.TouchEvent.Touch);

        GameObject loginBtn = GetButton((int)Buttons.Login_btn).gameObject;
        BindEvent(loginBtn, LoginBtnClick, Define.TouchEvent.Touch);

        //GameObject gloginBtn = GetButton((int)Buttons.GLogin_btn).gameObject;
        //BindEvent(gloginBtn, GoogleLoginBtnClick, Define.TouchEvent.Touch);

        failMessage = GetText((int)Texts.failMessage_txt);

        loginScene = FindObjectOfType<LoginScene>();

        callback -= ResponseAction;
        callback += ResponseAction;
    }

    private void Start()
    {
        Init();
    }


    private void LoginBtnClick(PointerEventData data)
    {
        LoginData val = new LoginData();
        //아이디 입력 확인
        InputField idInput = Get<InputField>((int)InputFields.ID_inputfield);
        if (isValidEmail(idInput.text))
        {
            val.email = idInput.text;
        }
        else
        {
            failMessage.text = "*이메일 혹은 비밀번호를 잘못 입력했습니다.";
            return;
        }

        //비밀번호 입력 확인
        InputField pwInput = Get<InputField>((int)InputFields.PW_inputfield);
        if (isValidPassword(pwInput.text))
        {
            val.password = idInput.text;
        }
        else
        {
            failMessage.text = "*이메일 혹은 비밀번호를 잘못 입력했습니다.";
            return;
        }

        val.deviceToken = "12345";

        //로그인 API 호출
        res = new Response<LoginResult>();
        Managers.Web.SendPostRequest<SignupResult>("login", val, callback);

        //Managers.UI.ShowPopupUI<UI_NicknameSet>("NicknameView", "UserInfo");


        //메인 씬으로 넘어가기
    }


    private void SignUpBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Info>("InfoView", "SignUp");

    }

    private void FindPWBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_PWfind>("FindView", "PWfind");
    }

    private void ResponseAction(UnityWebRequest request)
    {
        if (res != null)
        {
            res = JsonUtility.FromJson<Response<LoginResult>>(request.downloadHandler.text);
            /*Debug.Log(request.GetResponseHeader("Jwt-Access-Token"));
            Debug.Log(request.GetResponseHeader("Jwt-Refresh-Token"));*/

            Debug.Log(res.code);
            Debug.Log(res.isSuccess);
            Debug.Log(res.message);

            if (res.isSuccess)
            {
                if (res.code == 1000)
                {
                    Managers.Scene.LoadScene(Define.Scene.Main);
                }
            }
            else
            {
                switch (res.code)
                {
                    case 6002:
                        failMessage.text = "*이메일 혹은 비밀번호를 잘못 입력했습니다.";
                        break;
                    case 6003:
                        Debug.Log(res.message);
                        break;
                    case 6009:
                        Debug.Log(res.message);
                        break;
                    case 6010:
                        failMessage.text = "*이메일 혹은 비밀번호를 잘못 입력했습니다.";
                        break;
                    case 6011:
                        failMessage.text = "*이메일 혹은 비밀번호를 잘못 입력했습니다.";
                        break;

                }
            }
            res = null;
        }
    }

    private bool isValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }
        try
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private bool isValidPassword(string pw)
    {
        if (string.IsNullOrWhiteSpace(pw))
        {
            return false;
        }
        try
        {
            return Regex.IsMatch(pw, @"^(?=.*[a-z])(?=.*[0-9]).{6,15}$", 
                RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}


