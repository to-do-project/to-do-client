using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PlanetSet : UI_UserInfo
{
    enum Buttons
    {
        Back_btn,
        Next_btn,
    }

    GameObject nextBtn;

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);

        nextBtn = GetButton((int)Buttons.Next_btn).gameObject;
        BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);


    }
    void Start()
    {
        Init();
    }

    private void NextBtnClick(PointerEventData data)
    {
        //유효한 행성 골랐는지

        //유저 정보 서버에 넘기기

        //Managers.UI.CloseAllPopupUI();
        Managers.Scene.LoadScene(Define.Scene.Main);
    }
}
