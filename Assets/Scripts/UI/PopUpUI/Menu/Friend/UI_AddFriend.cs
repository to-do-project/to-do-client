using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_AddFriend : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Blind_btn,
        Accept_btn,
        Cancel_btn,
    }
    enum Texts
    {
        FriendName_txt,
        FriendLevel_txt,
    }
    enum Images
    {
        FriendProfile_image,
    }
    // ================================ //

    Text friendNameTxt, friendLevelTxt;     // 친구 이름, 친구 레벨 오브젝트
    Image profileImage;                     // 프로필 이미지 오브젝트
    GameObject parent;                      // 부모 오브젝트 (FriendView)
    UI_Friend friend;                       // 부모 스크립트 (UI_Friend)

    int level = 1;                          // 친구 행성 레벨
    bool clicked = false;                   // 웹 통신 중 클릭을 잠구기 위한 변수

    public int id { private get; set; }     // 친구의 userId값

    const string profileName = "Art/UI/Profile/Profile_Color_3x"; // 프로필 이미지 경로

    public override void Init()             // 초기화 함수
    {
        base.Init();

        CameraSet();                        // 카메라 연결(상속)

        parent = GameObject.Find("FriendView(Clone)"); // 부모 오브젝트 연결
        if(parent != null)
        {
            friend = parent.GetComponent<UI_Friend>(); // 부모 스크립트 연결
        } else
        {
            Debug.Log("parent가 NULL입니다. UI_AddFriend");
        }

        SetBtns();  // 버튼 오브젝트 바인드 및 이벤트 할당

        Bind<Text>(typeof(Texts));  // 텍스트 오브젝트 바인드

        friendNameTxt = GetText((int)Texts.FriendName_txt);
        friendNameTxt.text = friend.Name;   // 이름 텍스트 초기화

        friendLevelTxt = GetText((int)Texts.FriendLevel_txt);
        SetLevel(level);    // 레벨 텍스트 초기화
    }

    public void SetLevel(int level) // level >> 텍스트에 들어갈 레벨 값 || 레벨 텍스트를 바꿈
    {
        if (friendLevelTxt != null)
            friendLevelTxt.text = "Lv. " + level.ToString();
        else
            this.level = level;
    }

    public void SetImage(string color)  // color >> UI_Color.Color enum값의 string || 프로필 이미지 초기화
    {
        Bind<Image>(typeof(Images));    // 이미지 바인딩
        profileImage = GetImage((int)Images.FriendProfile_image);   // 오브젝트 바인딩
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));  // string 값을 다시 enum으로 변경 후 enum을 int값으로 변경
        profileImage.sprite = Resources.LoadAll<Sprite>(profileName)[index];    // 해당 index의 이미지로 변경
    }

    void SetBtns()  // 버튼 바인딩 및 이벤트 할당
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Blind_btn, ClosePopupUI);   // 뒷 배경 누를 시 팝업 제거

        SetBtn((int)Buttons.Accept_btn, (data) => {     // 수락 버튼 누를 시 수락 이벤트
            CheckFriend();  // 수락 이벤트
        });

        SetBtn((int)Buttons.Cancel_btn, ClosePopupUI);  // 취소 버튼 누를 시 팝업 제거
    }

    void CheckFriend()  // 수락 이벤트
    {
        if (clicked) return;    // 웹 통신 대기

        clicked = true;         // 웹 통신 대기

        // 헤더 값 설정
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };
        // ============ //

        // 웹 통신 코드
        Managers.Web.SendUniRequest("api/friends/" + id, "POST", null, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text); // 통신에서 받아온 Json 데이터를 오브젝트화
            if (response.isSuccess) // 성공 코드 반환시
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/FriendFadeView"));   // 토스트 알림 생성
                Managers.Sound.PlayPopupSound(); // 알림 생성 사운드
                Managers.UI.ClosePopupUI(); // 팝업 삭제
                clicked = false;            // 웹 통신 완료
            }
            else if (response.code == 6000) // 토큰 재발급 코드
            {
                clicked = false;            // 토큰 재발급 후 재 통신을 위해 초기화
                Managers.Player.SendTokenRequest(CheckFriend);  // 토큰 재발급
            }
            else if (response.code == 5041) // 이미 요청한 친구일 때
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/WaitingFriendView"));   // 토스트 알림 생성
                Managers.Sound.PlayPopupSound(); // 알림 생성 사운드
                Managers.UI.ClosePopupUI(); // 팝업 삭제
                clicked = false;            // 웹 통신 완료
            }
            else // 이외의 실패 코드 반환시
            {
                Debug.Log(response.message); // 오류 메세지 반환
                Managers.UI.ClosePopupUI();  // 팝업 삭제
                clicked = false;             // 웹 통신 완료
            }
        }, hN, hV);
    }

    private void Start()
    {
        Init();
    }
}