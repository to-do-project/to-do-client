using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Color : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Back_btn,
        LightRed_btn,
        Yellow_btn,
        Green_btn,
        SkyBlue_btn,
        Blue_btn,
        LightPurple_btn,
        Purple_btn,
        Pink_btn,
        Gray_btn,
        Black_btn,
        Next_btn,
    }

    enum Images
    {
        Check_image,
    }
    // ================================ //

    public enum Colors // 프로필 컬러 enum값
    {
        LightRed = 0,
        Yellow,
        Green,
        SkyBlue,
        Blue,
        LightPurple,
        Purple,
        Pink,
        Gray,
        Black,
    }

    UI_Profile profile;       // ProfileView 스크립트
    UI_Menu menu;             // MenuView 스크립트
    Transform checkTransform; // 체크 표시 위치
    GameObject checkImage;    // 체크 표시 이미지
    GameObject curBtn;        // 현재 버튼

    string selectColor;       // 선택된 컬러 이름

    public override void Init() // 초기화
    {
        base.Init();

        CameraSet(); // 카메라 설정

        SetBtns();

        selectColor = null;
        curBtn = null;

        Bind<GameObject>(typeof(Images)); // 이미지 바인딩

        checkImage = Get<GameObject>((int)Images.Check_image); // 체크 이미지 오브젝트 설정
        checkImage.SetActive(false);           // 체크 이미지 비활성화
        checkTransform = checkImage.transform; // 체크 이미지 Transform 바인드

        profile = FindObjectOfType<UI_Profile>();
        menu = FindObjectOfType<UI_Menu>();
    }

    public void ColorBtnEnter(GameObject gameObject) // 버튼에 터치 인식 시 버튼 크기 확대
    {
        if (checkImage.activeSelf) return;
        gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
    }

    public void ColorBtnExit(GameObject gameObject) // 버튼에 터치 나감 인식 시 버튼 크기 축소
    {
        if (checkImage.activeSelf) return;
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void SetBtns() // 버튼 이벤트 설정
    {
        Bind<Button>(typeof(Buttons));

        // 뒤로가기 버튼
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // 완료 버튼 || 완료 함수 실행
        SetBtn((int)Buttons.Next_btn, (data) => { ColorChange(); });

        // 프로필 색상 버튼 || ColorBtnClick(p1, p2) p1에 색상 전달
        GameObject btnLR = GetButton((int)Buttons.LightRed_btn).gameObject;
        BindEvent(btnLR, (data) => { ColorBtnClick(Colors.LightRed, btnLR); });

        GameObject btnY = GetButton((int)Buttons.Yellow_btn).gameObject;
        BindEvent(btnY, (data) => { ColorBtnClick(Colors.Yellow, btnY); });

        GameObject btnG = GetButton((int)Buttons.Green_btn).gameObject;
        BindEvent(btnG, (data) => { ColorBtnClick(Colors.Green, btnG); });

        GameObject btnSB = GetButton((int)Buttons.SkyBlue_btn).gameObject;
        BindEvent(btnSB, (data) => { ColorBtnClick(Colors.SkyBlue, btnSB); });

        GameObject btnB = GetButton((int)Buttons.Blue_btn).gameObject;
        BindEvent(btnB, (data) => { ColorBtnClick(Colors.Blue, btnB); });

        GameObject btnLP = GetButton((int)Buttons.LightPurple_btn).gameObject;
        BindEvent(btnLP, (data) => { ColorBtnClick(Colors.LightPurple, btnLP); });

        GameObject btnP = GetButton((int)Buttons.Purple_btn).gameObject;
        BindEvent(btnP, (data) => { ColorBtnClick(Colors.Purple, btnP); });

        GameObject btnPink = GetButton((int)Buttons.Pink_btn).gameObject;
        BindEvent(btnPink, (data) => { ColorBtnClick(Colors.Pink, btnPink); });

        GameObject btnGray = GetButton((int)Buttons.Gray_btn).gameObject;
        BindEvent(btnGray, (data) => { ColorBtnClick(Colors.Gray, btnGray); });

        GameObject btnBlack = GetButton((int)Buttons.Black_btn).gameObject;
        BindEvent(btnBlack, (data) => { ColorBtnClick(Colors.Black, btnBlack); });
    }

    // 완료 버튼 함수
    void ColorChange()
    {
        // 웹 통신 헤더
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // 웹 통신 리퀘스트
        RequestProfileColor request = new RequestProfileColor();
        request.profileColor = selectColor;

        // 프로필 컬러값 변경 웹 통신
        Managers.Web.SendUniRequest("api/user/profile", "PATCH", request, (uwr) => {
            // Json 응답 데이터를 유니티 데이터로 변환
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            // 응답 성공 시
            if (response.code == 1000)
            {
                // Debug.Log(response.result);
                
                // PlayerPrefs의 프로필 색상 값 전환
                Managers.Player.SetString(Define.PROFILE_COLOR, selectColor);
                // 활성화 되어있는 MenuView, ProfileView의 프로필 색상 변경 및 창 닫기
                menu.ChangeColor(selectColor);
                profile.ChangeColor(selectColor);
                Managers.UI.ClosePopupUI();
            }
            // 토큰 재발급 오류 시
            else if (response.code == 6000)
            {
                // 토큰 재발급
                Managers.Player.SendTokenRequest(ColorChange);
            }
            // 응답 실패 시
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    // 프로필 색상 선택 버튼 이벤트 함수
    // color = 색상 값
    // target = 클릭된 오브젝트
    void ColorBtnClick(Colors color, GameObject target)
    {
        // 선택된 컬러값 string으로 저장
        selectColor = color.ToString();

        // 체크 이미지 활성화
        if(checkImage.activeSelf == false)
        {
            checkImage.SetActive(true);
        }

        // 이전에 선택되었던 버튼의 크기 조정
        if(curBtn != null)
        {
            curBtn.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        // 현재 버튼 크기 조정 및 체크 이미지 위치 조정
        curBtn = target;
        curBtn.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        checkTransform.position = curBtn.transform.position;

        // 사운드 재생
        Managers.Sound.PlayNormalButtonClickSound();
    }

    void Start()
    {
        Init();
    }
}
