using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RequestLogin
{
    public string email;
    public string password;
    public string deviceToken;
}

[System.Serializable]
public class ResponseLogin
{
    public long userId;
    public long planetId;
    public int planetLevel;
    public string planetColor;
    public string email;
    public string nickname;
    public long characterItem;
    public string profileColor;
    public int point;
    public int missionStatus;
    public string deviceToken;
}

public class UI_Login : UI_Panel
{
    Action<UnityWebRequest> callback;
    Response<ResponseLogin> res;

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
        RequestLogin val = new RequestLogin();
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
            val.password = pwInput.text;
        }
        else
        {
            failMessage.text = "*이메일 혹은 비밀번호를 잘못 입력했습니다.";
            return;
        }

        val.deviceToken = "12345";

        //로그인 API 호출
        res = new Response<ResponseLogin>();
        Managers.Web.SendPostRequest<ResponseSignUp>("login", val, callback);

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
            res = JsonUtility.FromJson<Response<ResponseLogin>>(request.downloadHandler.text);

            Debug.Log(res.code);
            Debug.Log(res.message);
            
            if (res.isSuccess)
            {
                Debug.Log("Success Login");
                if (res.code == 1000)
                {
                    //토큰 저장
                    Managers.Player.SetString(Define.JWT_ACCESS_TOKEN, request.GetResponseHeader(Define.JWT_ACCESS_TOKEN));
                    Managers.Player.SetString(Define.JWT_REFRESH_TOKEN, request.GetResponseHeader(Define.JWT_REFRESH_TOKEN));
                    /*Debug.Log(Managers.Player.GetString(Define.JWT_ACCESS_TOKEN));
                    Debug.Log(Managers.Player.GetString(Define.JWT_REFRESH_TOKEN));
*/

                    ResponseLogin result = res.result;

                    //유저정보
                    Managers.Player.SetString(Define.EMAIL, result.email);
                    Managers.Player.SetString(Define.NICKNAME, result.nickname);
                    Managers.Player.SetString(Define.USER_ID, result.userId.ToString());
                    Managers.Player.SetString(Define.PLANET_ID, result.userId.ToString());
                    Managers.Player.SetInt(Define.PLANET_LEVEL, result.planetLevel);
                    Managers.Player.SetString(Define.PLANET_COLOR, result.planetColor);
                    Managers.Player.SetString(Define.EMAIL, result.email);
                    Managers.Player.SetString(Define.NICKNAME, result.nickname);
                    Managers.Player.SetString(Define.CHARACTER_COLOR, result.characterItem.ToString());


                    //Managers.Player.Init();
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


