using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// �г��� ���� UI�� ��ũ��Ʈ
public class UI_NickChange : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
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
    // ================================ //

    // �θ� ��ũ��Ʈ(������, �޴�)
    UI_Profile profile;
    UI_Menu menu;
    // �Ϸ� ��ư
    GameObject nextBtn;
    Text Ntxt; // �Է� ���� �˸� �ؽ�Ʈ
    InputField Ninput; // �г��� �Է� �ʵ�

    string nickname; // �г���

    // �ʱ�ȭ
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

    // ��ư �̺�Ʈ ����
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // �Ϸ� ��ư
        nextBtn = GetButton((int)Buttons.Next_btn).gameObject;

        // �ڷΰ��� ��ư
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // �г��� �ߺ� Ȯ�� ��ư
        SetBtn((int)Buttons.NickCheck_btn, (data) => {
            IsVaildNickname();
        });
    }

    // �Ϸ� ��ư Ŭ�� �̺�Ʈ
    public void NextBtnClick(PointerEventData data)
    {
        // ��ư�� ��� �� �� ���
        Managers.Sound.PlayNormalButtonClickSound();
        ExNickChange();
    }

    // �г��� ���� �� ���
    void ExNickChange()
    {
        // �� ��� ��� ��
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // �� ��� request ��
        RequestNickChange request = new RequestNickChange();
        request.nickname = nickname;

        // �г��� ���� �� ���
        Managers.Web.SendUniRequest("api/user/nickname", "PATCH", request, (uwr) => {
            // �� ���� json �����͸� ����Ƽ �����ͷ� ��ȯ
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            // �� ��� ���� ��
            if (response.code == 1000)
            {
                // Debug.Log(response.result);
                // �г��� ����
                Managers.Player.SetString(Define.NICKNAME, nickname);
                menu.ChangeNickname(nickname);
                profile.ChangeNickname(nickname);
                ClosePopupUI();
            }
            // �ڵ� ���� ��
            else if(response.code == 6000)
            {
                Debug.Log(response.message);
                Managers.Player.SendTokenRequest(ExNickChange);
            }
            // ��Ÿ ���� ��
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    // �г��� ����
    void IsVaildNickname()
    {
        nickname = Ninput.text; 
        Managers.Sound.PlayNormalButtonClickSound();
        Managers.Web.SendGetRequest("join/dupli/nickname?nickname=", nickname, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if(response.isSuccess)
            {
                Ntxt.text = " ��� ������ �г����Դϴ�.";
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