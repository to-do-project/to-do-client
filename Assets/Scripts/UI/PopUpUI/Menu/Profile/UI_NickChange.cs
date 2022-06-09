using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    UI_Profile profile;
    UI_Menu menu;
    GameObject nextBtn;
    Text Ntxt;
    InputField Ninput;

    string nickname;

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

        profile = FindObjectOfType<UI_Profile>();
        menu = FindObjectOfType<UI_Menu>();
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        nextBtn = GetButton((int)Buttons.Next_btn).gameObject;

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.NickCheck_btn, (data) => {
            IsVaildNickname();
        });
    }

    public void NextBtnClick(PointerEventData data)
    {
        ExNickChange();
    }

    void ExNickChange()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        RequestNickChange request = new RequestNickChange();
        request.nickname = nickname;

        Managers.Web.SendUniRequest("api/user/nickname", "PATCH", request, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.code == 1000)
            {
                Debug.Log(response.result);
                Managers.Player.SetString(Define.NICKNAME, nickname);
                menu.ChangeNickname(nickname);
                profile.ChangeNickname(nickname);
                ClosePopupUI();
            }
            else if(response.code == 6000)
            {
                Debug.Log(response.message);
                Managers.Player.SendTokenRequest(ExNickChange);
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    private void IsVaildNickname()
    {
        nickname = Ninput.text;
        Managers.Web.SendGetRequest("join/dupli/nickname?nickname=", nickname, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if(response.isSuccess)
            {
                Ntxt.text = " 사용 가능한 닉네임입니다.";
                nextBtn.GetComponent<Button>().interactable = true;
                BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            } else
            {
                Ntxt.text = response.message;
                nextBtn.GetComponent<Button>().interactable = false;
                ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
        });
    }

    void Start()
    {
        Init();
    }
}