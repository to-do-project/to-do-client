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
