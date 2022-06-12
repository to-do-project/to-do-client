using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UI_DoneEdit : UI_Popup
{
    enum Buttons
    {
        done_btn,
        cancle_btn,
    }

    //Action innerCallback;


    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
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

        //innerCallback -= SendArrangeRequest;
        //innerCallback += SendArrangeRequest;
/*        arrangeCallback -= ArrangeResponseAction;
        arrangeCallback += ArrangeResponseAction;*/

        Bind<Button>(typeof(Buttons));
        GameObject doneBtn = GetButton((int)Buttons.done_btn).gameObject;
        GameObject cancleBtn = GetButton((int)Buttons.cancle_btn).gameObject;

        BindEvent(doneBtn, DoneBtnClick, Define.TouchEvent.Touch);
        BindEvent(cancleBtn, CancleBtnClick, Define.TouchEvent.Touch);

    }

    void CancleBtnClick(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
    }

    void DoneBtnClick(PointerEventData data)
    {
        if (Managers.Player.CheckItemFixState())
        {
            //Managers.Player.ConvertToRequestList();
            //Managers.Player.SendTokenRequest(innerCallback);
            Managers.Player.SendArrangementRequest();

            //Managers.Scene.LoadScene(Define.Scene.Main);
        }
        else
        {

        }
    }

    /*void SendArrangeRequest()
    {
        //배치 API 호출
        Managers.Player.SendArrangementRequest();

    }*/
}
