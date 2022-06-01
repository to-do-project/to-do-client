using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemBuy : UI_PopupMenu
{
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

    GameObject sliderObject;
    Text typeTxt, nameTxt, explainTxt, priceTxt, pointTxt, maxTxt, handleTxt;
    Slider buyAmountSlider;
    Image itemImg;
    Button buyBtn;

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

    long itemId;
    int price, point;
    GameObject parent;

    public void SetValue(long itemId, string itemType, string itemName, string description, int price, int maxBuy, GameObject parent, Sprite sprite)
    {
        this.itemId = itemId;
        this.price = price;
        this.parent = parent;
        point = Managers.Player.GetInt("point");
        point = 2000;

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

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        buyBtn = GetButton((int)Buttons.Buy_btn);
        BindEvent(buyBtn.gameObject, BuyBtnClick);

        SetBtn((int)Buttons.Cancel_btn, ClosePopupUI);
        SetBtn((int)Buttons.Minus_btn, (data) => { buyAmountSlider.value--; });
        SetBtn((int)Buttons.Plus_btn, (data) => { buyAmountSlider.value++; });
    }

    public void BuyAmountChanged()
    {
        if (PointCheck()) return;

        if (buyAmountSlider.value * price > point) buyAmountSlider.value = (int)(price / point);

        handleTxt.text = buyAmountSlider.value.ToString();
        pointTxt.text = ((int)buyAmountSlider.value * price).ToString() + " Point";
    }

    bool PointCheck()
    {
        if (point == 0 || point / price < 1)
        {
            buyBtn.interactable = false;
            ClearEvent(buyBtn.gameObject, BuyBtnClick);
            return true;
        }
        return false;
    }

    public void BuyBtnClick(PointerEventData data)
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        RequestBuyItem request = new RequestBuyItem
        {
            count = (int)buyAmountSlider.value,
            itemId = itemId,
            totalPrice = (int)buyAmountSlider.value * price
        };

        Managers.Web.SendUniRequest("api/store/item", "POST", request, (uwr) => {
            Response<ResponseBuyItem> response = JsonUtility.FromJson<Response<ResponseBuyItem>>(uwr.downloadHandler.text);
            if (response.code == 1000)
            {
                Debug.Log(uwr.downloadHandler.text);
                parent.GetComponent<UI_ItemStore>().SetPoint(response.result.point);
            }
            else
            {
                Debug.Log(response.message);
            }
            Debug.Log($"{buyAmountSlider.value}°³ ±¸¸Å");
            ClosePopupUI();
        }, hN, hV);
    }

    private void Awake()
    {
        Init();
    }
}
