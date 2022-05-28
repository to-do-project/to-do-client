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
        List<string> hN = new List<string>();
        List<string> hV = new List<string>();

        hN.Add("Jwt-Access-Token");
        hV.Add(Testing.instance.AccessToken);
        hN.Add("User-Id");
        hV.Add(Testing.instance.UserId);

        RequestNickChange request = new RequestNickChange();
        request.nickname = nickname;

        Testing.instance.Webbing("api/user/nickname", "PATCH", request, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.code == 1000)
            {
                Debug.Log(response.result);
                ClosePopupUI();
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