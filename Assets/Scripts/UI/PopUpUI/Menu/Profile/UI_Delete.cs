using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Delete : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Next_btn,
        Back_btn,
    }

    enum Texts
    {
        PswdCheck_txt,
    }

    enum InputFields
    {
        Pswd_inputfield,
    }
    // ================================ //

    InputField Pswdfield; // 비밀번호 입력 창
    Text PswdChecktxt;    // 입력 실패 알림 텍스트

    // 경로
    const string pathName = "Menu/Profile";

    // 초기화
    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));

        Pswdfield = GetInputfiled((int)InputFields.Pswd_inputfield);

        PswdChecktxt = GetText((int)Texts.PswdCheck_txt);
    }

    // 버튼 이벤트 설정
    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // 뒤로가기 버튼
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // 완료 버튼 || 계정 삭제 웹 통신
        SetBtn((int)Buttons.Next_btn, (data) => {
            if (string.IsNullOrWhiteSpace(Pswdfield.text) == false)
            {
                Managers.Sound.PlayNormalButtonClickSound();
                DeleteAccount();
            }
        });
    }

    // 계정 삭제 웹 통신
    private void DeleteAccount()
    {
        // 웹 통신 헤더 값
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // 웹 통신 Request 값
        RequestDelete req = new RequestDelete();
        req.password = Pswdfield.text;

        // Debug.Log(req.password);
        // 계정 삭제 웹 통신
        Managers.Web.SendUniRequest("api/user", "DELETE", req, (data) => {
            // Json 응답 데이터를 유니티 데이터로 변환
            Response<string> response = JsonUtility.FromJson<Response<string>>(data.downloadHandler.text);

            // 웹 통신 성공 시
            if (response.isSuccess)
            {
                // Debug.Log(response.result);
                // FireBase 토큰 삭제 및 PlayerPrefs 데이터 삭제
                Managers.FireBase.DeleteToken();
                PlayerPrefs.DeleteAll();

                // DeleteCheckView 생성
                Managers.UI.ShowPopupUI<UI_DeleteCheck>("DeleteCheckView", pathName);
            }
            // 토근 재발급 오류 시
            else if (response.code == 6000)
            {
                // 토큰 재발급
                Managers.Player.SendTokenRequest(DeleteAccount);
            }
            // 웹 통신 실패 시
            else
            {
                Debug.Log(response.message);
                // Debug.Log(response.code);
                // 입력 실패 텍스트 변경
                PswdChecktxt.text = "*비밀번호를 잘못 입력했습니다.";
            }
        }, hN, hV);
    }

    void Start()
    {
        Init();
    }
}
