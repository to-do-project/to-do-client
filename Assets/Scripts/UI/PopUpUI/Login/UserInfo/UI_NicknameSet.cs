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

    enum Texts
    {
        Enable_txt,
    }

    GameObject nextBtn;
    Text Ntxt;
    InputField Ninput;

    bool isCheck;

    public override void Init()
    {
        base.Init();

        Bind<InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);

        GameObject checkBtn = GetButton((int)Buttons.NickCheck_btn).gameObject;
        BindEvent(checkBtn, CheckBtnClick, Define.TouchEvent.Touch);

        nextBtn = GetButton((int)Buttons.Next_btn).gameObject;
        nextBtn.GetComponent<Button>().interactable = false;

        Ntxt = GetText((int)Texts.Enable_txt);
        Ninput = GetInputfiled((int)InputFields.Nickname_inputfield);
    }

    private void Start()
    {
        Init();
    }



    private void CheckBtnClick(PointerEventData data)
    {
        if (IsVaildNickname(Ninput.text))
        {
            Ntxt.text = " 사용 가능한 닉네임입니다.";
            isCheck = true;
            nextBtn.GetComponent<Button>().interactable = true;
            BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
        }
        else
        {
            Ntxt.text = " 이미 있는 닉네임입니다.";
            isCheck = false;
            nextBtn.GetComponent<Button>().interactable = false;
            ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
        }
    }

    private bool IsVaildNickname(string nickname)
    {
        //서버에서 체크
        return true;
    }

    private void NextBtnClick(PointerEventData data)
    {
        //닉네임 유효한 입력 했는지
        if (isCheck)
        {
            loginScene.Nickname = Ninput.text;
            Managers.UI.ShowPopupUI<UI_PlanetSet>("PlanetView", "UserInfo");
        }

    }

}
