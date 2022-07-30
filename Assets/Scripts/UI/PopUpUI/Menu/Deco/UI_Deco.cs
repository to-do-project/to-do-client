using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Deco : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Back_btn,
        Done_btn,
        Cancel_btn,
        Left_btn,
        Right_btn,
    }

    enum Images
    {
        Cloth_img,
    }
    // ================================ //

    ItemImageContainer itemImages; // 아이템 이미지 저장 스크립트

    GameObject invenScroll, invenContent, invenButton; // Scroll, Content, Button 오브젝트
    Image clothImage;         // 캐릭터 옷 이미지
    Button leftBtn, rightBtn; // 좌, 우 선택 버튼

    Dictionary<long, GameObject> contentList; // Content의 자식 아이템 버튼 오브젝트의 Dictionary

    int index = 0, startIndex = 0; // 선택 아이템 인덱스
    bool onSave = false; // 웹 통신 체크

    // 경로
    const string itemPath = "Prefabs/UI/ScrollContents/Item_btn";
    const string fadePath = "UI/Popup/Menu/Deco/";

    // 초기화
    public override void Init()
    {
        base.Init();

        CameraSet();

        contentList = new Dictionary<long, GameObject>();

        SetBtns();

        Bind<Image>(typeof(Images));
        clothImage = GetImage((int)Images.Cloth_img);

        if (invenContent == null)
        {
            invenContent = GameObject.Find("InvenContent");
        }
        if (invenScroll == null)
        {
            invenScroll = GameObject.Find("InvenScroll");
        }

        itemImages = GetComponent<ItemImageContainer>();

        SetInven(); // 인벤 초기화
    }

    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // 뒤로가기 버튼
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // 완료 버튼 || 웹 통신 함수 실행
        SetBtn((int)Buttons.Done_btn, (data) => { SaveInven(); });

        // 취소 버튼 || 초기값으로 변경
        SetBtn((int)Buttons.Cancel_btn, (data) => {
            index = startIndex; // 현재 아이템을 시작 아이템 인덱스로 변경
            ChangeCloth(dataContainer.invenIdList[index]); // 인덱스에 맞춰 옷 변경

            // 팝업 알림 띄우기 및 버튼음 재생
            Managers.Resource.Instantiate(fadePath + "CancelFadeView");
            Managers.Sound.PlayPopupSound();
        });

        // 왼쪽 버튼 클릭 || 인덱스를 -1 하여 버튼 한칸 이동
        leftBtn = GetButton((int)Buttons.Left_btn);
        BindEvent(leftBtn.gameObject, (data) => {
            // 인덱스 -1 후 옷 변경, 버튼음 재생
            if (index == 0) return;
            index--; 

            ChangeCloth(dataContainer.invenIdList[index]); 
            Managers.Sound.PlayNormalButtonClickSound();
        }, Define.TouchEvent.Touch);

        // 오른쪽 버튼 클릭 || 인덱스를 +1 하여 버튼 한칸 이동
        rightBtn = GetButton((int)Buttons.Right_btn);
        BindEvent(rightBtn.gameObject, (data) => {
            // 인덱스 +1 후 옷 변경, 버튼음 재생
            if (index == dataContainer.invenIdList.Count - 1) return;
            index++; 

            ChangeCloth(dataContainer.invenIdList[index]);
            Managers.Sound.PlayNormalButtonClickSound();
        }, Define.TouchEvent.Touch);
    }

    void Start()
    {
        Init();
    }

    // 인벤 초기화
    void SetInven()
    {
        // 스크롤 컨텐츠(버튼) 초기화
        InitContents();
        // 좌, 우 버튼 초기화
        RefreshBtnLR();

        // 인덱스 초기화
        SetIndex(Managers.Player.GetInt(Define.CHARACTER_ITEM));
        startIndex = index;
        // 옷 초기화
        ChangeCloth(dataContainer.invenIdList[index]);
    }

    // 스크롤 컨텐츠(버튼) 초기화
    void InitContents()
    {
        // 데이터에 저장된 리스트를 탐색
        foreach (var tmp in dataContainer.invenIdList)
        {
            // 오브젝트 생성
            GameObject item = Instantiate(Resources.Load<GameObject>(itemPath));

            // 오브젝트를 invenContent의 자식으로 설정
            item.transform.SetParent(invenContent.transform, false);

            // 버튼 transition 속성을 none으로 변경, 버튼 배경 회색으로 변경
            item.GetComponent<Button>().transition = Selectable.Transition.None;
            item.transform.Find("Base").GetComponent<Image>().color = new Color(217f / 255f, 217f / 255f, 217f / 255f);

            // 버튼 초기화
            UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
            btn.SetValue(tmp, invenScroll.GetComponent<ScrollRect>(), itemImages.CharItemSprites[tmp - itemImages.CharGap]);
            btn.SetDecoScript(GetComponent<UI_Deco>());

            // Dictionary에 오브젝트 추가
            contentList.Add(tmp, item);
        }
    }

    // 좌, 우 버튼 재설정
    void RefreshBtnLR()
    {
        // 웹 통신 중 일 경우 리턴
        if (onSave) return;

        // index가 0이면 좌 버튼 비활성화
        if (index == 0)
        {
            leftBtn.interactable = false;
        }
        else
        {
            leftBtn.interactable = true;
        }

        // index가 리스트의 끝이면 우 버튼 비활성화
        if (index == dataContainer.invenIdList.Count - 1)
        {
            rightBtn.interactable = false;
        }
        else
        {
            rightBtn.interactable = true;
        }
    }

    // 인덱스 설정
    // id = 아이템 id값
    void SetIndex(long id)
    {
        // 리스트에 id와 같은 아이템의 index를 구한다
        for (int i = 0; i < dataContainer.invenIdList.Count; i++)
        {
            if (dataContainer.invenIdList[i] == id) index = i;
        }
    }

    // 캐릭터 옷 변경
    // id = 옷 아이템 id값
    public void ChangeCloth(long id)
    {
        // 웹 통신 중 일 경우 리턴
        if (onSave) return;

        // 현재 버튼 크기 축소 및 색상 변경
        if (invenButton != null)
        {
            invenButton.transform.localScale = new Vector3(1, 1, 1);
            invenButton.transform.Find("Base").GetComponent<Image>().color = new Color(217f / 255f, 217f / 255f, 217f / 255f);
        }
        // 다음 버튼으로 변경, 확대 및 색상 변경
        invenButton = contentList[id];
        invenButton.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        invenButton.transform.Find("Base").GetComponent<Image>().color = new Color(1, 1, 1);

        // 캐릭터의 옷 이미지 변경
        clothImage.sprite = itemImages.CharItemSprites[id - itemImages.CharGap];

        // 아이템 ScrollView 사이즈 재계산
        ContentSizeFitter fitter = invenContent.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);

        // 현재 아이템 인덱스 조정 및 좌, 우 버튼 재설정
        SetIndex(id);
        RefreshBtnLR();
    }

    // 캐릭터 옷 저장 웹 통신
    void SaveInven()
    {
        // 이미 통신중 인 경우 리턴
        if (onSave) return;
        onSave = true;

        // 웹 통신 헤더 설정
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // 캐릭터 옷 저장 웹 통신
        Managers.Web.SendUniRequest("api/closet/character-items/" + dataContainer.invenIdList[index].ToString(), "PATCH", null, (uwr) => {
            // 웹 응답 json 데이터를 유니티 데이터로 전환
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            // 웹 통신 성공 시
            if (response.isSuccess)
            {
                // Debug.Log(response.result);
                // PlayerPrefs에 저장된 Character Item 수정
                Managers.Player.SetInt(Define.CHARACTER_ITEM, (int)dataContainer.invenIdList[index]);

                // 메인화면 캐릭터 업데이트
                Managers.Player.UpdateCharacterItem(dataContainer.invenIdList[index]);

                // 팝업 알림 생성 및 효과음 재생
                Managers.Resource.Instantiate(fadePath + "DoneFadeView");
                Managers.Sound.PlayPopupSound();
            }
            // 토큰 재발급 오류 시
            else if(response.code == 6000)
            {
                // 재발급 후 재통신을 위해 변수 초기화
                onSave = false;
                Debug.Log(response.message);
                // 토큰 재발급
                Managers.Player.SendTokenRequest(SaveInven);
            }
            // 웹 통신 실패 시
            else
            {
                Debug.Log(response.message);
            }
            onSave = false;
        }, hN, hV);
    }
}
