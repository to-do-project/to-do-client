using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_FriendUI : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Back_btn,
        Fighting_btn,
    }

    enum Texts
    {
        date_txt,
        title_txt,
    }

    enum GameObjects
    {
        GoalList,
    }
    // ================================ //

    public string nickname { private get; set; } = ""; // 닉네임

    //GameObject goalList;
    GameObject btn = null;

    long memberId = 0, userId = 0; // 친구 멤버 Id, 유저 Id
    bool clicked = false; // 웹 통신 체크

    public void SetUserId(long userId)
    {
        this.userId = userId;
    }

    public long GetUserId()
    {
        return userId;
    }

    // 초기화
    public override void Init()
    {
        base.Init();

        // 카메라 설정
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();

        if (UIcam != cam)
        {
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }
        // ============ //

        // 안드로이드 뒤로가기 버튼 이벤트 제거 및 변경
        Managers.Input.SystemTouchAction = OnFriendBackTouched;

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        Text title = GetText((int)Texts.title_txt);
        title.text = nickname + "님의 목표리스트";

        Text date = GetText((int)Texts.date_txt);
        DateTime today = DateTime.Now;
        date.text = today.ToString("yyyy") + "." + today.ToString("MM") + "." + today.ToString("dd");
        // Debug.Log(today);

        // 뒤로가기 버튼
        SetBtn((int)Buttons.Back_btn, (data) => {
            // 친구 행성, 캐릭터 오브젝트 제거
            FindObjectOfType<UI_FriendMain>().DestroyAll();

            // 버튼음 재생 및 메인 오브젝트 활성화
            Managers.Sound.PlayNormalButtonClickSound();
            Managers.UI.ActiveAllUI();
            Managers.Player.GetPlanet().SetActive(true);
            Destroy(gameObject);
        });

        // 응원하기 버튼
        btn = SetBtn((int)Buttons.Fighting_btn, (data) => {
            // 웹 통신 및 버튼음 재생
            Ex_Like();
            Managers.Sound.PlayNormalButtonClickSound();
            btn.SetActive(false);
        });

        // 응원하기 버튼 비활성화
        btn.SetActive(false);
    }

    // 응원하기 버튼 활성화
    // id = 멤버 id
    public void OnFightingView(long id)
    {
        memberId = id;
        if(btn.activeSelf == false)
        {
            // 응원하기 버튼 활성화
            btn.SetActive(true);

            // 친구 투두 미달성 팝업 알림 생성 및 효과음 재생
            Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/NotComplete"));
            Managers.Sound.PlayPopupSound();
        }
    }

    // 응원하기 웹 통신
    void Ex_Like()
    {
        if (clicked) return;
        clicked = true;

        // 웹 통신 헤더 값
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // 응원하기 웹 통신
        Managers.Web.SendUniRequest("api/todo/" + memberId, "GET", null, (uwr) => {
            // 웹 응답 json 데이터를 유니티 데이터로 전환
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            // 통신 성공 시
            if (response.isSuccess)
            {
                // Debug.Log(response.result);
                clicked = false;
            }
            // 토큰 오류 시
            else if (response.code == 6000)
            {
                clicked = false;
                Managers.Player.SendTokenRequest(Ex_Like);
            }
            // 기타 오류 시
            else
            {
                Debug.Log(response.message);
                clicked = false;
            }
        }, hN, hV);
    }

    // 안드로이드 뒤로가기 이벤트
    void OnFriendBackTouched(Define.SystemEvent evt)
    {
        if (evt != Define.SystemEvent.Back)
        {
            return;
        }

        Managers.Input.SystemTouchAction = OnBackTouched;

        FindObjectOfType<UI_FriendMain>().DestroyAll();
        Managers.UI.ActiveAllUI();
        Managers.Player.GetPlanet().SetActive(true);
        Destroy(gameObject);
    }

    void OnBackTouched(Define.SystemEvent evt)
    {
        if (evt != Define.SystemEvent.Back)
        {
            return;
        }

        Managers.UI.CloseAppOrUI();
        Managers.UI.ActivePanelUI();

    }

    void Start()
    {
        Init();
    }
}
