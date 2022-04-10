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
    public override void Clear()
    {
        Debug.Log("Login Clear");
        Managers.UI.Clear();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Login;

        Managers.UI.ShowPopupUI<UI_Login>("LoginView");

        Managers.Input.TouchAction -= OnBackTouched;
        Managers.Input.TouchAction += OnBackTouched;
    }

    void OnBackTouched(Define.SystemEvent evt)
    {
        if (evt != Define.SystemEvent.Back)
        {
            return;
        }

        Managers.UI.CloseAppOrUI(Define.Scene.Login);

    }
}
