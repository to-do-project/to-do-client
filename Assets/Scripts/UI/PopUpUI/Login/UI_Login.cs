using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Login : UI_Popup
{

    enum Buttons
    {
        Login_btn,
        FindPW_btn,
        SignUp_btn,
        GLogin_btn,
    }

    enum InputFields
    {
        ID_inputfield,
        PW_inputfield,
    }

    public override void Init()
    {
        base.Init(); 

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
        BindEvent(signupBtn, SignUpBtnClick, Define.UIEvent.Click);

    }

    private void Start()
    {
        Init();
    }

    
    public void LoginBtnClick(PointerEventData data)
    {
        //아이디 입력 확인

        //비밀번호 입력 확인
        
        //로그인 API 호출
        
    }

 
    public void SignUpBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_SignUp>("SignUpView");
        
    }

}
