using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Policy : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
    }

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }

    private void Start()
    {
        Init();
    }
}
