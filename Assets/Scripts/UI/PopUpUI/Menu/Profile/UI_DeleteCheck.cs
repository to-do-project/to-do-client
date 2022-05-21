using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_DeleteCheck : UI_PopupMenu
{
    enum Buttons
    {
        End_btn,
    }

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        //�ڷΰ��� ��� ����(�˾� ���� �ȵǵ���)
        Managers.Input.SystemTouchAction -= Managers.Input.SystemTouchAction;
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.End_btn, (data) => { Application.Quit(); });
    }

    private void Start()
    {
        Init();
    }
}
