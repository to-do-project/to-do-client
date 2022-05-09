using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Main : UI_Panel
{
    enum Buttons
    {
        menu_btn,
        notice_btn,
    }

    enum Texts
    {
        date_txt,
    }

    public override void Init()
    {
        base.Init();

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();

        if (UIcam != cam)
        {
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GameObject menuBtn = GetButton((int)Buttons.menu_btn).gameObject;
        BindEvent(menuBtn, MenuBtnClick, Define.TouchEvent.Touch);
    }

    private void Start()
    {
        Init();
    }


    private void MenuBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Menu>("MenuView", "Menu");
    }
}