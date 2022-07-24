using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �α���, ȸ������, �༺����, ��й�ȣ ã��
/// �ڵ��α����� �ȵǾ� ���� �� ���� ����
/// �ڵ��α��� �Ǿ������� Main Scene���� �Ѿ
/// </summary>
 
//���� ���� ���¿� ����
//�α���, ȸ������, �༺����, ��й�ȣ UI �ҷ�����(�� Panel UI�� ����)


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
