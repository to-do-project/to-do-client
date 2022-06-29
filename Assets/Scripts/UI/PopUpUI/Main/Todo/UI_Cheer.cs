using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Cheer : UI_Popup
{
    enum Texts
    {
        cheer_txt
    }

    enum Buttons
    {
        cheer_btn
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

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

        GameObject cheerBtn = GetButton((int)Buttons.cheer_btn).gameObject;
        BindEvent(cheerBtn, CheerBtnClick);
    }

    void CheerBtnClick(PointerEventData data)
    {
        //응원 API 날리기
    }

}
