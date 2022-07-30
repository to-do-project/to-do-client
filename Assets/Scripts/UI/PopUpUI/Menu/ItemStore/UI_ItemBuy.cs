using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 아이템 구매 창 UI에 들어가는 스크립트
public class UI_ItemBuy : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Objects
    {
        Slider_object
    }

    enum Buttons
    {
        Buy_btn,
        Cancel_btn,
        Minus_btn,
        Plus_btn,
    }

    enum Images
    {
        Item_img,
    }

    enum Texts
    {
        Type_txt,
        Name_txt,
        Explain_txt,
        Price_txt,
        Point_txt,
        Max_txt,
        Handle_txt,
    }

    enum Sliders
    {
        BuyAmount_slider,
    }
    // ================================ //

    // 부모(UI_ItemStore
    GameObject parent;
    // 슬라이더 관련
    GameObject sliderObject;
    Slider buyAmountSlider;
    // 각종 텍스트들
    Text typeTxt, nameTxt, explainTxt, priceTxt, pointTxt, maxTxt, handleTxt;
    Image itemImg; // 아이템 이미지
    Button buyBtn; // 구매 버튼

    long itemId; // 아이템 고유 id
    int price, point; // 아이템 가격과 유저 point 정보

    // 초기화
    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));
        Bind<GameObject>(typeof(Objects));

        sliderObject = getObject((int)Objects.Slider_object);
        itemImg = GetImage((int)Images.Item_img);
        typeTxt = GetText((int)Texts.Type_txt);
        nameTxt = GetText((int)Texts.Name_txt);
        explainTxt = GetText((int)Texts.Explain_txt);
        priceTxt = GetText((int)Texts.Price_txt);
        pointTxt = GetText((int)Texts.Point_txt);
        maxTxt = GetText((int)Texts.Max_txt);
        handleTxt = GetText((int)Texts.Handle_txt);

        buyAmountSlider = Get<Slider>((int)Sliders.BuyAmount_slider);
        buyAmountSlider.onValueChanged.AddListener(delegate { BuyAmountChanged(); });
    }


    // 버튼 이벤트 설정
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        buyBtn = GetButton((int)Buttons.Buy_btn);
        BindEvent(buyBtn.gameObject, BuyBtnClick);

        // 취소 버튼
        SetBtn((int)Buttons.Cancel_btn, ClosePopupUI);

        // 수량 - 버튼
        SetBtn((int)Buttons.Minus_btn, (data) => { buyAmountSlider.value--; Managers.Sound.PlayNormalButtonClickSound(); });

        // 수량 + 버튼
        SetBtn((int)Buttons.Plus_btn, (data) => { buyAmountSlider.value++; Managers.Sound.PlayNormalButtonClickSound(); });
    }

    // UI 정보 초기화
    public void SetValue(long itemId, string itemType, string itemName, string description, int price, int maxBuy, GameObject parent, Sprite sprite)
    {
        this.itemId = itemId;
        this.price = price;
        this.parent = parent;
        point = Managers.Player.GetInt(Define.POINT);

        if(point == 0 || point / price < 1)
        {
            maxBuy = 1;
            PointCheck();
        }

        maxBuy = Mathf.Min((int)point / price, maxBuy);

        if (maxBuy <= 1)
        {
            buyAmountSlider.value = 1;
            sliderObject.SetActive(false);
        }

        nameTxt.text = itemName;
        typeTxt.text = itemType;
        explainTxt.text = description;
        priceTxt.text = price.ToString() + " Point";
        pointTxt.text = price.ToString() + " Point";
        maxTxt.text = "max " + maxBuy.ToString();
        buyAmountSlider.maxValue = maxBuy;
        itemImg.sprite = sprite;
        itemImg.SetNativeSize();
        itemImg.transform.localScale *= 0.5f;
    }

    // 구매 수량 변경 시 UI 변경 함수
    public void BuyAmountChanged()
    {
        // 구매할 포인트가 없다면 수량 변경 불가
        if (PointCheck()) return;

        // 구매 포인트가 현재 포인트보다 높은 경우 최대 구매 수량으로 변경
        if (buyAmountSlider.value * price > point) buyAmountSlider.value = (int)(price / point);

        // 텍스트 변경
        handleTxt.text = buyAmountSlider.value.ToString();
        pointTxt.text = ((int)buyAmountSlider.value * price).ToString() + " Point";
    }

    // 구매 버튼 클릭 이벤트
    public void BuyBtnClick(PointerEventData data)
    {
        // 버튼음 재생 및 웹 통신
        Managers.Sound.PlayNormalButtonClickSound();
        ExBuy();
    }

    // 아이템 이미지 크기 조정
    public void ItemSizeUp()
    {
        itemImg.transform.localScale *= 5f;
    }

    // 포인트를 확인하여 구매할 수 없는 경우 true, 구매할 수 있는 경우 false 반환 및 버튼 이벤트 변경
    bool PointCheck()
    {
        // 구매할 포인트가 없는 경우
        if (point == 0 || point / price < 1)
        {
            // 구매 버튼 비활성화
            buyBtn.interactable = false;
            ClearEvent(buyBtn.gameObject, BuyBtnClick);
            return true;
        }
        // 구매할 포인트가 있는 경우
        return false;
    }

    // 아이템 구매 웹 통신
    void ExBuy()
    {
        // 웹 통신 헤더 값
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // 웹 통신 Request 값
        RequestBuyItem request = new RequestBuyItem
        {
            count = (int)buyAmountSlider.value,
            itemId = itemId,
            totalPrice = (int)buyAmountSlider.value * price
        };

        // 아이템 구매 웹 통신
        Managers.Web.SendUniRequest("api/store/item", "POST", request, (uwr) => {
            // 웹 응답 json 데이터를 유니티 데이터로 전환
            Response<ResponseBuyItem> response = JsonUtility.FromJson<Response<ResponseBuyItem>>(uwr.downloadHandler.text);

            // 웹 통신 성공 시
            if (response.code == 1000)
            {
                // Debug.Log(uwr.downloadHandler.text);
                // 아이템 스토어 화면 포인트 변경
                parent.GetComponent<UI_ItemStore>().SetPoint(response.result.point);

                // 더 구매할 아이템이 없을 경우 해당 아이템 버튼 제거
                if(response.result.maxCnt <= 0)
                    parent.GetComponent<UI_ItemStore>().DeleteItem(response.result.itemId);

                // Debug.Log($"{buyAmountSlider.value}개 구매");
                ClosePopupUI();
            }
            // 토큰 오류 시
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(ExBuy);
            }
            // 기타 오류 시
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    void Awake()
    {
        Init();
    }
}
