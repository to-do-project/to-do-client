using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 닉네임 변경 UI의 스크립트
public class UI_NickChange : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
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

    // 부모 스크립트(프로필, 메뉴)
    UI_Profile profile;
    UI_Menu menu;
    // 완료 버튼
    GameObject nextBtn;
    Text Ntxt; // 입력 오류 알림 텍스트
    InputField Ninput; // 닉네임 입력 필드

    string nickname; // 닉네임

    // 초기화
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

    // 버튼 이벤트 설정
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // 완료 버튼
        nextBtn = GetButton((int)Buttons.Next_btn).gameObject;

        // 뒤로가기 버튼
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // 닉네임 중복 확인 버튼
        SetBtn((int)Buttons.NickCheck_btn, (data) => {
            IsVaildNickname();
        });
    }

    // 완료 버튼 클릭 이벤트
    public void NextBtnClick(PointerEventData data)
    {
        // 버튼음 재생 및 웹 통신
        Managers.Sound.PlayNormalButtonClickSound();
        ExNickChange();
    }

    // 닉네임 변경 웹 통신
    void ExNickChange()
    {
        // 웹 통신 헤더 값
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // 웹 통신 request 값
        RequestNickChange request = new RequestNickChange();
        request.nickname = nickname;

        // 닉네임 변경 웹 통신
        Managers.Web.SendUniRequest("api/user/nickname", "PATCH", request, (uwr) => {
            // 웹 응답 json 데이터를 유니티 데이터로 전환
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            // 웹 통신 성공 시
            if (response.code == 1000)
            {
                // Debug.Log(response.result);
                // 닉네임 변경
                Managers.Player.SetString(Define.NICKNAME, nickname);
                menu.ChangeNickname(nickname);
                profile.ChangeNickname(nickname);
                ClosePopupUI();
            }
            // 코드 오류 시
            else if(response.code == 6000)
            {
                Debug.Log(response.message);
                Managers.Player.SendTokenRequest(ExNickChange);
            }
            // 기타 오류 시
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    // 닉네임 검증
    void IsVaildNickname()
    {
        nickname = Ninput.text; 
        Managers.Sound.PlayNormalButtonClickSound();
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