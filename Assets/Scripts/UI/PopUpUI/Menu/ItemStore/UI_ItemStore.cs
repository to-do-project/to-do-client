using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemStore : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Back_btn,
    }

    enum Texts
    {
        Profile_text,
        Point_text
    }

    enum Images
    {
        Profile_image,
    }
    // ================================ //

    // 스크롤 뷰 및 컨텐츠 부모 오브젝트
    GameObject charItemContent, planetItemContent, charScroll, planetScroll;
    // 텍스트 오브젝트
    Text profileText, pointText;
    // 프로필 색상 이미지
    Image profileImage;
    // 아이템 이미지 저장 스크립트
    ItemImageContainer itemImages;

    // 아이템 버튼의 Transform을 저장하는 Dictionary
    Dictionary<long, Transform> charBtnDict;
    Dictionary<long, Transform> planetBtnDict;

    bool onBtn = false; // 웹 통신 체크

    // 경로
    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    // 초기화
    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        profileText = GetText((int)Texts.Profile_text);
        pointText = GetText((int)Texts.Point_text);

        profileImage = GetImage((int)Images.Profile_image);

        // PlayerPrefs에 값이 있으면 미리 초기화
        if(PlayerPrefs.HasKey(Define.NICKNAME))
        {
            profileText.text = Managers.Player.GetString(Define.NICKNAME);
        }
        if(PlayerPrefs.HasKey(Define.POINT)) 
        {
            pointText.text = Managers.Player.GetInt(Define.POINT).ToString();
        }
        if(PlayerPrefs.HasKey(Define.PROFILE_COLOR))
        {
            ChangeColor(Managers.Player.GetString(Define.PROFILE_COLOR));
        }
        // =========== //

        if (charItemContent == null)
        {
            charItemContent = GameObject.Find("CharItemContent");
        }

        if (planetItemContent == null)
        {
            planetItemContent = GameObject.Find("PlanetItemContent");
        }

        if (charScroll == null)
        {
            charScroll = GameObject.Find("CharScroll");
        }

        if (planetScroll == null)
        {
            planetScroll = GameObject.Find("PlanetScroll");
        }

        itemImages = GetComponent<ItemImageContainer>();

        charBtnDict = new Dictionary<long, Transform>();
        planetBtnDict = new Dictionary<long, Transform>();

        InitItemBtns();

        // 웹 통신을 통해 실제 데이터를 받아와 재 초기화
        CheckPoint();
    }

    // 버튼 이벤트 설정
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // 뒤로가기 버튼
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }

    // 아이템 구매 창 웹 통신
    // id = 선택한 아이템 고유 id
    // sprite = 선택한 아이템 이미지
    // sizeUp = 아이템 이미지 사이즈 조절 여부
    public void OnBuyView(long id, Sprite sprite, bool sizeUp)
    {
        if (onBtn) return;
        onBtn = true;
        
        // 웹 통신 헤더 값
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // 아이템 구매 창 웹 통신
        Managers.Web.SendUniRequest("api/store/items/" + id, "GET", null, (uwr) => {
            // 웹 응답 json 데이터를 유니티 데이터로 전환
            Response<ResponseItemDetail> response = JsonUtility.FromJson<Response<ResponseItemDetail>>(uwr.downloadHandler.text);

            // 웹 통신 성공 시
            if (response.code == 1000)
            {
                // Debug.Log(response.result);
                // 아이템 구매 창 띄우기
                UI_ItemBuy item = Managers.UI.ShowPopupUI<UI_ItemBuy>("ItemBuyView", "Menu/ItemStore");

                // 웹 통신 결과를 토대로 구매 창 초기화
                item.SetValue(id, response.result.type, response.result.name, response.result.description,
                              response.result.price, response.result.maxCnt, gameObject, sprite);

                // 아이템 이미지 크기 조정이 필요한 경우 크기 조정
                if (sizeUp) item.ItemSizeUp();
                onBtn = false;
            }
            // 토큰 오류 시
            else if (response.code == 6000)
            {
                // Debug.Log(response.message);
                onBtn = false;

                // 토큰 재발급 후 같은 웹 통신 실행
                Managers.Player.SendTokenRequest(() => {
                    string[] new_hN = { Define.JWT_ACCESS_TOKEN,
                                    "User-Id" };
                    string[] new_hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                                    Managers.Player.GetString(Define.USER_ID) };

                    Managers.Web.SendUniRequest("api/store/items/" + id, "GET", null, (uwr) =>
                    {
                        Response<ResponseItemDetail> response = JsonUtility.FromJson<Response<ResponseItemDetail>>(uwr.downloadHandler.text);
                        if (response.code == 1000)
                        {
                            Debug.Log(response.result);
                            UI_ItemBuy item = Managers.UI.ShowPopupUI<UI_ItemBuy>("ItemBuyView", "Menu/ItemStore");
                            item.SetValue(id, response.result.type, response.result.name, response.result.description,
                                          response.result.price, response.result.maxCnt, gameObject, sprite);
                            if (sizeUp) item.ItemSizeUp();
                        }
                        else
                        {
                            Debug.Log("토큰 재발급 실패(아이템 스토어)");
                        }
                    }, new_hN, new_hV);
                });
            }
            // 기타 오류 시
            else
            {
                Debug.Log(response.message);
                onBtn = false;
            }
        }, hN, hV);
    }

    // 포인트 텍스트 변경
    public void SetPoint(int amount)
    {
        int originUsedPoint = (Managers.Player.GetInt(Define.USEDPOINT) != -1) ? Managers.Player.GetInt(Define.USEDPOINT) : 0;
        int usedPoint = amount - Managers.Player.GetInt(Define.POINT) + originUsedPoint;
        Managers.Player.SetInt(Define.USEDPOINT, usedPoint);
        Managers.Player.SetInt(Define.POINT, amount);
        pointText.text = amount.ToString();
    }

    // 아이템이 잔여 개수가 없을 경우 아이템 삭제
    // id = 삭제 할 아이템 고유 id
    public void DeleteItem(long id)
    {
        // Dictionary에서 id 조회 후 삭제
        if (charBtnDict.ContainsKey(id))
        {
            Destroy(charBtnDict[id].gameObject);
            for (int i = 0; i < dataContainer.charBtnId.Count; i++)
                if (dataContainer.charBtnId[i] == id) dataContainer.charBtnId.RemoveAt(i);

            // 캐릭터 아이템의 경우 아이템 스토어 삭제와 동시에 인벤토리에 추가 (최대 개수가 1개)
            dataContainer.invenIdList.Add(id);
        }

        if (planetBtnDict.ContainsKey(id))
        {
            Destroy(planetBtnDict[id].gameObject);
            for (int i = 0; i < dataContainer.planetBtnId.Count; i++)
                if (dataContainer.planetBtnId[i] == id) dataContainer.planetBtnId.RemoveAt(i);
        }
    }

    // 프로필 컬러 변경
    // color = 프로필 색상
    public void ChangeColor(string color)
    {
        // 컬러값을 UI_Color에 있는 enum인 Colors를 활용하여 index값으로 변경
        int index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profileImage.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    // 아이템 버튼 생성
    void InitItemBtns()
    {
        // 캐릭터 아이템 버튼 생성
        foreach (var i in dataContainer.charBtnId)
        {
            AddCharItem(i);
        }

        // 장식 아이템 버튼 생성
        foreach (var i in dataContainer.planetBtnId)
        {
            AddPlanetItem(i);
        }
    }

    // 캐릭터 아이템 버튼을 생성, 초기화하고 Dictionary에 추가하는 함수
    // id = 아이템 고유 id
    void AddCharItem(long id)
    {
        // 버튼 오브젝트 생성 및 부모에 등록
        GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/Item_btn"));
        charBtnDict.Add(id, item.transform);
        charBtnDict[id].SetParent(charItemContent.transform, false);

        // 버튼 초기화
        UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
        btn.SetValue(id, charScroll.GetComponent<ScrollRect>(), itemImages.CharItemSprites[id - itemImages.CharGap]);
        btn.SetItemScript(gameObject.GetComponent<UI_ItemStore>());
    }

    // 장식 아이템 버튼을 생성, 초기화하고 Dictionary에 추가하는 함수
    // id = 아이템 고유 id
    void AddPlanetItem(long id)
    {
        // 버튼 오브젝트 생성 및 부모에 등록
        GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/Item_btn"));
        planetBtnDict.Add(id, item.transform);
        planetBtnDict[id].SetParent(planetItemContent.transform, false);

        // 버튼 초기화
        UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
        btn.SetValue(id, planetScroll.GetComponent<ScrollRect>(), itemImages.PlanetItemSprites[id - itemImages.PlanetGap]);
        btn.SetItemScript(gameObject.GetComponent<UI_ItemStore>());
        if (id - itemImages.PlanetGap == 8) btn.ItemSizeUp();
    }

    // 현재 포인트 값을 받기위한 웹 통신
    void CheckPoint()
    {
        // 웹 통신 헤더 값
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // 유저 정보 웹 통신
        Managers.Web.SendUniRequest("api/planet/my-info", "GET", null, (uwr) => {
            // 웹 응답 json 데이터를 유니티 데이터로 전환
            Response<ResponsePlanetInfo> response = JsonUtility.FromJson<Response<ResponsePlanetInfo>>(uwr.downloadHandler.text);

            // 웹 통신 성공 시
            if (response.code == 1000)
            {
                // 포인트 값 재설정
                Managers.Player.SetInt(Define.POINT, response.result.point);
                pointText.text = Managers.Player.GetInt(Define.POINT).ToString();
            }
            // 토큰 오류 시
            else if (response.code == 6000)
            {
                Debug.Log(response.message);
                Managers.Player.SendTokenRequest(CheckPoint);
            }
            // 기타 오류 시
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    void Start()
    {
        Init();
    }
}