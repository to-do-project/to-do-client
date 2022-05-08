using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// UI_NicknameSet의 코드와 대부분 같은 코드
/// </summary>
public class UI_NickChange : UI_Popup
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

        setBtns();
        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));


        nextBtn = GetButton((int)Buttons.Next_btn).gameObject;
        nextBtn.GetComponent<Button>().interactable = false;

        Ntxt = GetText((int)Texts.Enable_txt);
        Ninput = GetInputfiled((int)InputFields.Nickname_inputfield);
    }

    private void setBtns()
    {
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, BackBtnClick, Define.TouchEvent.Touch);

        GameObject nickCheckBtn = GetButton((int)Buttons.NickCheck_btn).gameObject;
        BindEvent(nickCheckBtn, NickCheckBtnClick, Define.TouchEvent.Touch);

        GameObject nextBtn = GetButton((int)Buttons.Next_btn).gameObject;
        BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
    }

    #region ButtonEvents
    public void BackBtnClick(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
    }

    public void NickCheckBtnClick(PointerEventData data)
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

    public void NextBtnClick(PointerEventData data)
    {
        //닉네임 유효한 입력 했는지
        if (isCheck)
        {
            //닉네임 저장
            Managers.UI.ClosePopupUI();
        }
    }
    #endregion

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