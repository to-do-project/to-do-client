using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_DeleteCheck : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
    enum Buttons
    {
        End_btn,
    }
    // ================================ //

    // �ʱ�ȭ
    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        // �ȵ���̵� �ڷΰ��� ��� ����(�˾� ������ �����ϱ� ����)
        Managers.Input.SystemTouchAction -= Managers.Input.SystemTouchAction;
    }

    // ��ư �̺�Ʈ ����
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // Ȯ�� ��ư || �� ����
        SetBtn((int)Buttons.End_btn, (data) => { Application.Quit(); });
    }

    void Start()
    {
        Init();
    }
}
