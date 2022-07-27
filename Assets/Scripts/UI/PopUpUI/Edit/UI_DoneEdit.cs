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

    enum Texts 
    {
        warning_txt,
    }


    Text warning;


    void Start()
    {
        Init();
    }


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GameObject doneBtn = GetButton((int)Buttons.done_btn).gameObject;
        GameObject cancleBtn = GetButton((int)Buttons.cancle_btn).gameObject;
        warning = GetText((int)Texts.warning_txt);
        warning.gameObject.SetActive(false);

        BindEvent(doneBtn, DoneBtnClick, Define.TouchEvent.Touch);
        BindEvent(cancleBtn, CancleBtnClick, Define.TouchEvent.Touch);

    }

    void CancleBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        Managers.UI.ClosePopupUI();
    }

    void DoneBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        warning.gameObject.SetActive(false);
        if (Managers.Player.CheckItemFixState())
        {
            //Managers.Player.ConvertToRequestList();
            //Managers.Player.SendTokenRequest(innerCallback);

            Managers.Player.SendArrangementRequest();
            
            //Managers.Scene.LoadScene(Define.Scene.Main);
        }
        else
        {
            warning.gameObject.SetActive(true);
        }
    }

}
