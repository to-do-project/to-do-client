using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// UI_NicknameSet�� �ڵ�� ��κ� ���� �ڵ�
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
            Ntxt.text = " ��� ������ �г����Դϴ�.";
            isCheck = true;
            nextBtn.GetComponent<Button>().interactable = true;
            BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
        }
        else
        {
            Ntxt.text = " �̹� �ִ� �г����Դϴ�.";
            isCheck = false;
            nextBtn.GetComponent<Button>().interactable = false;
            ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
        }
    }

    public void NextBtnClick(PointerEventData data)
    {
        //�г��� ��ȿ�� �Է� �ߴ���
        if (isCheck)
        {
            //�г��� ����
            Managers.UI.ClosePopupUI();
        }
    }
    #endregion

    private bool IsVaildNickname(string nickname)
    {
        //�������� üũ
        return true;
    }

    void Start()
    {
        Init();
    }
}