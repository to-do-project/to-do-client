using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.EventSystems;

public class UI_PswdChange : UI_PopupMenu
{
    enum Buttons
    {
        Next_btn,
        Back_btn,
    }

    enum Texts
    {
        Change_txt,
        Again_txt,
        PswdCheck_txt,
    }

    enum InputFields
    {
        Pswd_inputfield,
        Change_inputfield,
        Again_inputfield,
    }

    GameObject nextBtn;
    InputField Pswdfield, Changefield, Againfield;
    Text Changetxt, Againtxt, PswdChecktxt;
    bool isCheck = false;
    bool isValid = false;
    string password;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));


        Pswdfield = GetInputfiled((int)InputFields.Pswd_inputfield);

        Changefield = GetInputfiled((int)InputFields.Change_inputfield);
        Changefield.onEndEdit.AddListener(delegate { CheckPassWord(); });

        Againfield = GetInputfiled((int)InputFields.Again_inputfield);
        Againfield.onEndEdit.AddListener(delegate { SamePassWord(); });

        Changetxt = GetText((int)Texts.Change_txt);
        Againtxt = GetText((int)Texts.Again_txt);
        PswdChecktxt = GetText((int)Texts.PswdCheck_txt);
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        nextBtn = GetButton((int)Buttons.Next_btn).gameObject;
        nextBtn.GetComponent<Button>().interactable = false;
    }

    private void Start()
    {
        Init();
    }

    //��й�ȣ ��ȿ üũ
    private void CheckPassWord()
    {
        string pw = Changefield.text;

        try
        {
            if (Regex.IsMatch(pw, @"^(?=.*[a-z])(?=.*[0-9]).{6,15}$", RegexOptions.None, TimeSpan.FromMilliseconds(250)))
            {
                Changetxt.text = " ��밡���� ��й�ȣ�Դϴ�.";
                isValid = true;
            }
            else
            {
                Debug.Log(pw);
                Changetxt.text = " ��ȿ���� ���� ��й�ȣ �Դϴ�. (���� + ���� ���� 6~15����)";
                isValid = false;
            }

        }
        catch (RegexMatchTimeoutException)
        {
            Debug.Log(pw);
            Changetxt.text = " ��ȿ���� ���� ��й�ȣ �Դϴ�. (���� + ���� ���� 6~15����)";
            isValid = false;
        }
    }

    //��й�ȣ ��ġ üũ
    private void SamePassWord()
    {
        string pw = Changefield.text;
        string check = Againfield.text;

        if (isValid)
        {
            if (pw.Equals(check))
            {
                Debug.Log(check);
                Againtxt.text = " ��й�ȣ�� ��ġ�մϴ�.";
                isCheck = true;
                password = pw;
                nextBtn.GetComponent<Button>().interactable = true;
                BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
            else
            {
                Debug.Log(check);
                password = "";
                Againtxt.text = " ��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
                ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
        }

        else
        {
            Debug.Log(check);
            Againtxt.text = "��ȿ�� ��й�ȣ�� �Է����ּ���.";
        }
    }


    private void NextBtnClick(PointerEventData data)
    {
        //��й�ȣ�� �´��� Ȯ���� ��
        //ȸ������ API ������
        //�˾� ����
        if (isCheck && isValid)
        {
            if (string.IsNullOrWhiteSpace(password) == false)
            {
                Managers.Sound.PlayNormalButtonClickSound();
                ComparePassword();
            }
        }
    }

    private void ComparePassword()
    {
        //�н����� üũ
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        RequestPswdChange request = new RequestPswdChange();
        request.oldPassword = Pswdfield.text;
        request.newPassword = password;

        Managers.Web.SendUniRequest("api/user/pwd", "PATCH", request, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.code == 1000)
            {
                Debug.Log(password);
                Managers.Player.SetString(Define.PASSWORD, password);
                Managers.UI.ClosePopupUI();
            }
            else if (response.code == 6000)
            {
                Debug.Log(response.message);
                Managers.Player.SendTokenRequest(ComparePassword);
            }
            else
            {
                Debug.Log(response.message);
                PswdChecktxt.text = response.message;
            }
        }, hN, hV);
    }
}
