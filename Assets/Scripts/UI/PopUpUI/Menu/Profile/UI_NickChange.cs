using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// UI_NicknameSet�� �ڵ�� ��κ� ���� �ڵ�
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
        });
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