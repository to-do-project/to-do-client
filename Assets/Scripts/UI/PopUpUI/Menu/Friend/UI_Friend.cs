using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Friend : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Back_btn,
        Search_btn,
    }
    enum Texts
    {
        FriendOnly_txt,
        Request_txt,
        Friend_txt,
    }

    enum InputFields
    {
        Friend_inputfield
    }
    // ================================ //

    public string Name { get; private set; } // 친구명 저장 변수, 웹 통신 Parameter으로 사용

    GameObject content = null, requestContent = null, friendContent = null; // Rect Layer의 Content부모 오브젝트들
    Text friendOnlyTxt, friendTxt, requestTxt; // 텍스트 오브젝트
    InputField friendInputField; // 친구명 입력창

    int requestCount = 0, friendCount = 0; // 친구 요청 및 친구 숫자
    bool onSearch = false; // 웹 통신 체크

    // 경로
    const string pathName = "Menu/Friend";
    const string contentPath = "UI/ScrollContents/";

    // 초기화
    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Text>(typeof(Texts));
        Bind<InputField>(typeof(InputFields));

        friendOnlyTxt = GetText((int)Texts.FriendOnly_txt);
        requestTxt = GetText((int)Texts.Request_txt);
        friendTxt = GetText((int)Texts.Friend_txt);

        friendInputField = GetInputfiled((int)InputFields.Friend_inputfield);

        if (content == null)
        {
            content = GameObject.Find("FriendContent");
        }
        if (requestContent == null)
        {
            requestContent = GameObject.Find("RequestContents");
        }
        if (friendContent == null)
        {
            friendContent = GameObject.Find("FriendContents");
        }

        StartCoroutine(SetContents());

        RelocationAll();

        StartCoroutine(PlusCheck());
    }

    // 친구 오브젝트 추가
    // name = 친구 이름
    // friendId = 친구 고유 id
    // color = 친구 프로필 컬러
    // userId = 자신의 고유 id
    public bool AddFriend(string name, int friendId, string color, long userId)
    {
        // 친구 카운트가 100이상일 경우 리턴
        if (friendCount < 100)
        {
            // 친구 컨텐츠 생성
            GameObject target = Managers.Resource.Instantiate(contentPath + "FriendContent");
            target.transform.SetParent(friendContent.transform);
            target.transform.localScale = new Vector3(1, 1, 1);

            // 친구 컨텐츠 초기화
            FriendContent tmp = target.GetComponent<FriendContent>();
            tmp.SetParent(this.gameObject);
            tmp.SetName(name);
            tmp.SetID(friendId);
            tmp.SetUserID(userId);
            tmp.SetImage(color);

            // 카운트 증가 및 데이터 재 통신
            friendCount++;
            dataContainer.RefreshFriendData();
            RelocationAll();
            return true;
        }
        return false;
    }

    // 친구 컨텐츠 제거
    // target = 제거할 오브젝트
    public void DeleteFriend(GameObject target)
    {
        // 친구 카운트 감소 및 데이터 재 통신
        friendCount--;
        dataContainer.RefreshFriendData();

        // 오브젝트 제거
        target.SetActive(false);
        Destroy(target);
        RelocationAll();
    }

    // 요청 컨텐츠 제거
    // target = 제거할 오브젝트
    public void DeleteRequest(GameObject target)
    {
        // 요청 카운트 감소 및 데이터 재 통신
        dataContainer.RefreshFriendData();
        requestCount--;

        // 오브젝트 제거
        target.SetActive(false);
        Destroy(target);
        RelocationAll();
    }

    // 생성된 컨텐츠와 데이터를 비교하여 부족한 컨텐츠를 추가 생성하는 코루틴 함수
    IEnumerator PlusCheck()
    {
        // 비교를 위한 임시 딕셔너리
        Dictionary<long, ResponseFriendList> tmpFriend = new Dictionary<long, ResponseFriendList>();
        Dictionary<long, ResponseFriendList> tmpWait = new Dictionary<long, ResponseFriendList>();

        // 현재의 데이터 저장
        foreach (var tmp in dataContainer.friendList)
        {
            if (tmpFriend.ContainsKey(tmp.friendId)) continue;
            tmpFriend.Add(tmp.friendId, tmp);
        }
        foreach (var tmp in dataContainer.waitFriendList)
        {
            if (tmpWait.ContainsKey(tmp.friendId)) continue;
            tmpWait.Add(tmp.friendId, tmp);
        }

        // 친구 데이터 재통신
        dataContainer.RefreshFriendData();
        // 데이터 재통신 완료 시까지 대기
        while(dataContainer.friendCheck)
        {
            yield return null;
        }

        // 현재의 데이터와 재통신 후 데이터를 비교하여 추가된 오브젝트 생성
        foreach (var tmp in dataContainer.friendList)
        {
            if (tmpFriend.ContainsKey(tmp.friendId)) continue;
            AddFriend(tmp.nickName, (int)tmp.friendId, tmp.profileColor, tmp.userId);
        }
        foreach (var tmp in dataContainer.waitFriendList)
        {
            if (tmpWait.ContainsKey(tmp.friendId)) continue;
            AddRequest(tmp.nickName, (int)tmp.friendId, tmp.profileColor, tmp.userId);
        }

        // 오브젝트 위치 재조정
        RelocationAll();
    }

    // 컨텐츠 생성 코루틴 함수
    IEnumerator SetContents()
    {
        // 친구 목록이 초기화 되지 않았다면 대기
        while (dataContainer.friendCheck)
        {
            yield return null;
        }

        // 친구 목록 리스트에 따라 오브젝트 생성
        foreach (var tmp in dataContainer.friendList)
        {
            AddFriend(tmp.nickName, (int)tmp.friendId, tmp.profileColor, tmp.userId);
        }
        foreach (var tmp in dataContainer.waitFriendList)
        {
            AddRequest(tmp.nickName, (int)tmp.friendId, tmp.profileColor, tmp.userId);
        }
    }

    // 버튼 이벤트 설정
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // 뒤로가기 버튼
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // 친구 탐색 버튼
        SetBtn((int)Buttons.Search_btn, (data) => {
            // 친구명 저장
            Name = friendInputField.text;

            // 친구명이 비어있지 않을 시
            if (string.IsNullOrWhiteSpace(Name) == false)
            {
                if (onSearch) return;
                onSearch = true;

                // 친구 탐색
                SearchFriend();
            }
        });
    }

    // 친구 탐색 웹 통신
    void SearchFriend()
    {
        // 통신 헤더 값
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // 친구 탐색 웹 통신
        Managers.Web.SendUniRequest("api/users?keyword=" + Name, "GET", null, (uwr) => {
            // Json 응답 데이터를 유니티 데이터로 변환
            Response<ResponseSearchFriend> response = JsonUtility.FromJson<Response<ResponseSearchFriend>>(uwr.downloadHandler.text);

            // 웹 통신 성공 시
            if (response.isSuccess)
            {
                // Debug.Log(uwr.downloadHandler.text);
                // 친구 요청 팝업 생성 및 팝업 초기화
                var friend = Managers.UI.ShowPopupUI<UI_AddFriend>("AddFriendView", pathName);
                friend.id = (int)response.result.userId;
                friend.SetLevel(response.result.planetLevel);
                friend.SetImage(response.result.profileColor);

                // 버튼음 재생
                Managers.Sound.PlayNormalButtonClickSound();
                onSearch = false;
            }
            // 토큰 오류 시
            else if (response.code == 6000)
            {
                onSearch = false;
                // 토큰 재발급
                Managers.Player.SendTokenRequest(SearchFriend);
            }
            // 유저 풀에 요청명이 없을 시
            else if (response.code == 5003 || response.code == 5004 || response.code == 6001)
            {
                // 오류 메세지 팝업 생성 및 버튼음 재생
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/CantFindFadeView"));
                Managers.Sound.PlayPopupSound();
                onSearch = false;
            }
            // 기타 오류 시
            else
            {
                Debug.Log(response.message);
                onSearch = false;
            }
        }, hN, hV);
    }

    void Start()
    {
        Init();
    }

    // 요청 오브젝트 추가
    // name = 요청 친구 이름
    // friendId = 요청 친구 고유 id
    // color = 요청 친구 프로필 컬러
    // userId = 자신의 고유 id
    void AddRequest(string name, int friendId, string color, long userId)
    {
        // 요청 컨텐츠 생성
        GameObject target = Managers.Resource.Instantiate(contentPath + "RequestContent");
        target.transform.SetParent(requestContent.transform);
        target.transform.localScale = new Vector3(1, 1, 1);

        // 요청 컨텐츠 초기화
        RequestContent tmp = target.GetComponent<RequestContent>();
        tmp.SetParent(this.gameObject);
        tmp.SetName(name);
        tmp.SetId(friendId);
        tmp.SetUserID(userId);
        tmp.SetImage(color);

        // 요청 리스트 카운트 증가
        requestCount++;
        RelocationAll();
    }

    // 오브젝트 위치 재조정
    void RelocationAll()
    {
        // 요청 친구가 없을 시 요청 UI 숨김
        if(requestCount <= 0)
        {
            requestCount = 0;
            activeChange(false);
        } 
        else
        {
            activeChange(true);
        }

        // 텍스트 초기화
        requestTxt.text = $"      친구 요청 [{requestCount}]";
        friendTxt.text = $"      친구 [{friendCount}/100]";
        friendOnlyTxt.text = $"      친구 [{friendCount}/100]";
    }

    // 요청 UI on/off
    // toggle = true시 on, false시 off
    void activeChange(bool toggle)
    {
        // 친구 UI만 있는 경우
        friendOnlyTxt.gameObject.SetActive(!toggle);

        // 요청 및 친구 UI 둘 다 있는 경우
        requestTxt.gameObject.SetActive(toggle);
        friendTxt.gameObject.SetActive(toggle);

        ContentSizeFitter fitter = content.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);

        fitter = requestContent.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);

        fitter = friendContent.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
    }
}
