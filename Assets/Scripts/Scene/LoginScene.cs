using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 로그인, 회원가입, 행성설정, 비밀번호 찾기
/// 자동로그인이 안되어 있을 시 씬에 진입
/// 자동로그인 되어있으면 Main Scene으로 넘어감
/// </summary>
 
//유저 저장 상태에 따라
//로그인, 회원가입, 행성설정, 비밀번호 UI 불러오기(다 Panel UI로 설정)


public class LoginScene : BaseScene 
{

    string email;
    public string Email
    {
        get;
        set;
    }
    string pw;
    public string Pw
    {
        get;
        set;
    }
    string nickname;
    public string Nickname
    {
        get;
        set;
    }
    Define.Planet planet;
    public Define.Planet Planet
    {
        get;
        set;
    }



    public override void Clear()
    {
        Debug.Log("Login Clear");
        Managers.UI.Clear();
        Managers.Input.SystemTouchAction -= OnBackTouched;
    }

    void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Login;

        //Managers.UI.ShowPopupUI<UI_Login>("LoginView");
        Managers.UI.ShowPanelUI<UI_Login>("LoginView");

        Managers.Input.SystemTouchAction -= OnBackTouched;
        Managers.Input.SystemTouchAction += OnBackTouched;

        Managers.FireBase.GetToken();
        Managers.Sound.Clear();
    }

    void OnBackTouched(Define.SystemEvent evt)
    {
        if (evt != Define.SystemEvent.Back)
        {
            return;
        }

        Managers.UI.CloseAppOrUI();

    }

}
