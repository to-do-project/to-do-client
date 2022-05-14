using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.EventSystems;

public class UI_PswdChange : UI_Popup
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
    private void CameraSet()
    {
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
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);

        nextBtn = GetButton((int)Buttons.Next_btn).gameObject;
        nextBtn.GetComponent<Button>().interactable = false;
    }

    private void Start()
    {
        Init();
    }

    //비밀번호 유효 체크
    private void CheckPassWord()
    {
        string pw = Changefield.text;

        try
        {
            if (Regex.IsMatch(pw, @"^(?=.*[a-z])(?=.*[0-9]).{6,15}$", RegexOptions.None, TimeSpan.FromMilliseconds(250)))
            {
                Changetxt.text = " 사용가능한 비밀번호입니다.";
                isValid = true;
            }
            else
            {
                Debug.Log(pw);
                Changetxt.text = " 유효하지 않은 비밀번호 입니다. (영문 + 숫자 조합 6~15글자)";
                isValid = false;
            }

        }
        catch (RegexMatchTimeoutException)
        {
            Debug.Log(pw);
            Changetxt.text = " 유효하지 않은 비밀번호 입니다. (영문 + 숫자 조합 6~15글자)";
            isValid = false;
        }
    }

    //비밀번호 일치 체크
    private void SamePassWord()
    {
        string pw = Changefield.text;
        string check = Againfield.text;

        if (isValid)
        {
            if (pw.Equals(check))
            {
                Debug.Log(check);
                Againtxt.text = " 비밀번호와 일치합니다.";
                isCheck = true;
                password = pw;
                nextBtn.GetComponent<Button>().interactable = true;
                BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
            else
            {
                Debug.Log(check);
                password = "";
                Againtxt.text = " 비밀번호와 일치하지 않습니다.";
                ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
        }

        else
        {
            Debug.Log(check);
            Againtxt.text = "유효한 비밀번호를 입력해주세요.";
        }
    }


    private void NextBtnClick(PointerEventData data)
    {
        //비밀번호가 맞는지 확인한 후
        //회원가입 API 날리고
        //팝업 종료
        if (isCheck && isValid)
        {
            if (string.IsNullOrWhiteSpace(password) == false)
            {
                if (ComparePassword())
                {
                    //바뀐 Password 전달
                    Debug.Log(password);
                    Managers.UI.ClosePopupUI();
                } 
                else
                {
                    PswdChecktxt.text = "*비밀번호를 잘못 입력했습니다.";
                }
            }
        }
    }

    private bool ComparePassword()
    {
        //패스워드 체크
        string playerPassword = "";
        bool result = (Pswdfield.text == playerPassword);
        result = true;
        return result;
    }
}
