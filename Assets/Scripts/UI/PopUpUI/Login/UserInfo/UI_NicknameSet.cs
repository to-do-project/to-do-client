using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_NicknameSet : UI_UserInfo
{
    enum Buttons 
    {
        NickCheck_btn,
        Back_btn,
        Next_btn,
    }

    enum InputFields 
    { 
        Nickname_inputfield,
    }

    GameObject nextBtn;

    public override void Init()
    {
        base.Init();

        Bind<InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);

        nextBtn = GetButton((int)Buttons.Next_btn).gameObject;
        BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);


    }

    private void Start()
    {
        Init();
    }

    private void NextBtnClick(PointerEventData data)
    {
        //닉네임 유효한 입력 했는지
        Debug.Log("Next btn click");
        //Managers.UI.CloseAllPopupUI();
        Managers.UI.ShowPopupUI<UI_PlanetSet>("PlanetView","UserInfo");
    }

}
