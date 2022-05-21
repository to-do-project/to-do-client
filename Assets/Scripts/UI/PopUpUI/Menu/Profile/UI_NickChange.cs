using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// UI_NicknameSet의 코드와 대부분 같은 코드
/// </summary>
public class UI_NickChange : UI_PopupMenu
{

    enum Buttons
    {
        Back_btn,
        NickCheck_btn,
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

        CameraSet();

        SetBtns();

        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));

        nextBtn.GetComponent<Button>().interactable = false;

        Ntxt = GetText((int)Texts.Enable_txt);
        Ninput = GetInputfiled((int)InputFields.Nickname_inputfield);
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        nextBtn = GetButton((int)Buttons.Next_btn).gameObject;

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.NickCheck_btn, (data) => {
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
        });
    }

    public void NextBtnClick(PointerEventData data)
    {
        //닉네임 유효한 입력 했는지
        if (isCheck)
        {
            //닉네임 저장
            Managers.UI.ClosePopupUI();
        }
    }

    private bool IsVaildNickname(string nickname)
    {
        //서버에서 체크
        return true;
    }

    void Start()
    {
        Init();
    }
}