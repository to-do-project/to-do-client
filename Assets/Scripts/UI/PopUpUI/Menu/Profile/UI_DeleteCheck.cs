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

        //뒤로가기 기능 삭제(팝업 삭제 안되도록)
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
