using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// ������ ���� â UI�� ���� ��ũ��Ʈ
public class UI_ItemBuy : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
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

    // �θ�(UI_ItemStore
    GameObject parent;
    // �����̴� ����
    GameObject sliderObject;
    Slider buyAmountSlider;
    // ���� �ؽ�Ʈ��
    Text typeTxt, nameTxt, explainTxt, priceTxt, pointTxt, maxTxt, handleTxt;
    Image itemImg; // ������ �̹���
    Button buyBtn; // ���� ��ư

    long itemId; // ������ ���� id
    int price, point; // ������ ���ݰ� ���� point ����

    // �ʱ�ȭ
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


    // ��ư �̺�Ʈ ����
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        buyBtn = GetButton((int)Buttons.Buy_btn);
        BindEvent(buyBtn.gameObject, BuyBtnClick);

        // ��� ��ư
        SetBtn((int)Buttons.Cancel_btn, ClosePopupUI);

        // ���� - ��ư
        SetBtn((int)Buttons.Minus_btn, (data) => { buyAmountSlider.value--; Managers.Sound.PlayNormalButtonClickSound(); });

        // ���� + ��ư
        SetBtn((int)Buttons.Plus_btn, (data) => { buyAmountSlider.value++; Managers.Sound.PlayNormalButtonClickSound(); });
    }

    // UI ���� �ʱ�ȭ
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

    // ���� ���� ���� �� UI ���� �Լ�
    public void BuyAmountChanged()
    {
        // ������ ����Ʈ�� ���ٸ� ���� ���� �Ұ�
        if (PointCheck()) return;

        // ���� ����Ʈ�� ���� ����Ʈ���� ���� ��� �ִ� ���� �������� ����
        if (buyAmountSlider.value * price > point) buyAmountSlider.value = (int)(price / point);

        // �ؽ�Ʈ ����
        handleTxt.text = buyAmountSlider.value.ToString();
        pointTxt.text = ((int)buyAmountSlider.value * price).ToString() + " Point";
    }

    // ���� ��ư Ŭ�� �̺�Ʈ
    public void BuyBtnClick(PointerEventData data)
    {
        // ��ư�� ��� �� �� ���
        Managers.Sound.PlayNormalButtonClickSound();
        ExBuy();
    }

    // ������ �̹��� ũ�� ����
    public void ItemSizeUp()
    {
        itemImg.transform.localScale *= 5f;
    }

    // ����Ʈ�� Ȯ���Ͽ� ������ �� ���� ��� true, ������ �� �ִ� ��� false ��ȯ �� ��ư �̺�Ʈ ����
    bool PointCheck()
    {
        // ������ ����Ʈ�� ���� ���
        if (point == 0 || point / price < 1)
        {
            // ���� ��ư ��Ȱ��ȭ
            buyBtn.interactable = false;
            ClearEvent(buyBtn.gameObject, BuyBtnClick);
            return true;
        }
        // ������ ����Ʈ�� �ִ� ���
        return false;
    }

    // ������ ���� �� ���
    void ExBuy()
    {
        // �� ��� ��� ��
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // �� ��� Request ��
        RequestBuyItem request = new RequestBuyItem
        {
            count = (int)buyAmountSlider.value,
            itemId = itemId,
            totalPrice = (int)buyAmountSlider.value * price
        };

        // ������ ���� �� ���
        Managers.Web.SendUniRequest("api/store/item", "POST", request, (uwr) => {
            // �� ���� json �����͸� ����Ƽ �����ͷ� ��ȯ
            Response<ResponseBuyItem> response = JsonUtility.FromJson<Response<ResponseBuyItem>>(uwr.downloadHandler.text);

            // �� ��� ���� ��
            if (response.code == 1000)
            {
                // Debug.Log(uwr.downloadHandler.text);
                // ������ ����� ȭ�� ����Ʈ ����
                parent.GetComponent<UI_ItemStore>().SetPoint(response.result.point);

                // �� ������ �������� ���� ��� �ش� ������ ��ư ����
                if(response.result.maxCnt <= 0)
                    parent.GetComponent<UI_ItemStore>().DeleteItem(response.result.itemId);

                // Debug.Log($"{buyAmountSlider.value}�� ����");
                ClosePopupUI();
            }
            // ��ū ���� ��
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(ExBuy);
            }
            // ��Ÿ ���� ��
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
