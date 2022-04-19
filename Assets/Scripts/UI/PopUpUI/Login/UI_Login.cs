using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Login : UI_Panel
{
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

        GameObject signupBtn = GetButton((int)Buttons.SignUp_btn).gameObject;
        BindEvent(signupBtn, SignUpBtnClick, Define.TouchEvent.Touch);

        GameObject findpwBtn = GetButton((int)Buttons.FindPW_btn).gameObject;
        BindEvent(findpwBtn, FindPWBtnClick, Define.TouchEvent.Touch);

        GameObject loginBtn = GetButton((int)Buttons.Login_btn).gameObject;
        BindEvent(loginBtn, LoginBtnClick, Define.TouchEvent.Touch);
        
        //GameObject gloginBtn = GetButton((int)Buttons.GLogin_btn).gameObject;
        //BindEvent(gloginBtn, GoogleLoginBtnClick, Define.TouchEvent.Touch);

        loginScene = FindObjectOfType<LoginScene>();

    }

    private void Start()
    {
        Init();
    }

    
    public void LoginBtnClick(PointerEventData data)
    {
        //아이디 입력 확인
        InputField idInput = Get<InputField>((int)InputFields.ID_inputfield);
        if (string.IsNullOrWhiteSpace(idInput.text))
        {
            Debug.Log("ID가 공란입니다.");
        }
        else
        {
            Debug.Log($"ID : {idInput.text}");
        }

        //비밀번호 입력 확인
        InputField pwInput = Get<InputField>((int)InputFields.PW_inputfield);
        if(string.IsNullOrWhiteSpace(pwInput.text))
        {
            Debug.Log("PW가 공란입니다.");
        }
        else
        {
            Debug.Log($"PW : {pwInput.text}");
        }


        //로그인 API 호출

        //유저 정보가 입력 안된 상태면 유저정보 입력 뷰로 넘어감
        Managers.UI.ShowPopupUI<UI_NicknameSet>("NicknameView", "UserInfo");
        

        //메인 씬으로 넘어가기
    }

 
    public void SignUpBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Info>("InfoView", "SignUp");
        
    }

    public void FindPWBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_PWfind>("FindView", "PWfind");
    }

    /*public void GoogleLoginBtnClick(PointerEventData data)
    {
        //구글 로그인 API ghcnf
    }*/
}
