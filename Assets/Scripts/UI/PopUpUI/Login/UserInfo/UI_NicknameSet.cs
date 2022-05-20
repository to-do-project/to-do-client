using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UI_NicknameSet : UI_UserInfo
{
    Action<UnityWebRequest> callback;
    Response<string> res; //������ȣ �߼� ��

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

        callback -= ResponseAction;
        callback += ResponseAction;
    }

    private void Start()
    {
        Init();
    }



    private void CheckBtnClick(PointerEventData data)
    {


        if (IsVaildNickname(Ninput.text))
        {
            Debug.Log("valid");
            res = new Response<string>();
            Managers.Web.SendGetRequest("join/dupli/nickname?nickname=", Ninput.text, callback);

            
        }
        else
        {
            Debug.Log("invalid");
            Ntxt.text = " ����� �� ���� �г����Դϴ�. Ư������, ����� ����Ͻ� �� �����ϴ�.";
            isCheck = false;
            nextBtn.GetComponent<Button>().interactable = false;
            ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
        }
    }


    private void ResponseAction(UnityWebRequest request)
    {
        if(res != null)
        {
            res = JsonUtility.FromJson<Response<string>>(request.downloadHandler.text);

            /*Debug.Log(res.code);
            Debug.Log(res.isSuccess); */

            if (res.isSuccess)
            {
                switch (res.code)
                {
                    case 1000:
                        Ntxt.text = " ��� ������ �г����Դϴ�.";
                        isCheck = true;
                        nextBtn.GetComponent<Button>().interactable = true;
                        BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
                        break;
                    
                }
            }
            else
            {
                switch (res.code)
                {
                    case 4000:
                        break;
                    case 6013:
                        Ntxt.text = " ����� �� ���� �г����Դϴ�. Ư������, ����� ����Ͻ� �� �����ϴ�.";
                        isCheck = false;
                        nextBtn.GetComponent<Button>().interactable = false;
                        ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
                        break;
                    case 6016:
                        Ntxt.text = " �̹� �ִ� �г����Դϴ�.";
                        isCheck = false;
                        nextBtn.GetComponent<Button>().interactable = false;
                        ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
                        break;
                }
            }

            res = null;
        }
    }

    private bool IsVaildNickname(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(nickname, @"^[A-Za-z0-9��-����-�R]{1,8}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private void NextBtnClick(PointerEventData data)
    {
        //�г��� ��ȿ�� �Է� �ߴ���
        if (isCheck)
        {
            loginScene.Nickname = Ninput.text;
            Managers.UI.ShowPopupUI<UI_PlanetSet>("PlanetView", "UserInfo");
        }

    }

}
