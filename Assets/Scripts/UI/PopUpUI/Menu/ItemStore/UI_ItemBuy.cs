using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemBuy : UI_Popup
{
    enum Buttons
    {
        Buy_btn,
        Cancel_btn,
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

    Text typeTxt, nameTxt, explainTxt, priceTxt, pointTxt, maxTxt, handleTxt;
    Slider buyAmountSlider;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Text>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));

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

    bool isCharItem;
    string itemName;
    int price;
    int curHave;
    int maxHave;
    GameObject parent;

    public void SetValue(bool isCharItem, string itemName, int price, int curHave, int maxHave, GameObject parent)
    {
        this.isCharItem = isCharItem;
        this.itemName = itemName;
        this.price = price;
        this.curHave = curHave;
        this.maxHave = maxHave;
        this.parent = parent;
    }

    private void CameraSet()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        GameObject buyBtn = GetButton((int)Buttons.Buy_btn).gameObject;
        BindEvent(buyBtn, BuyBtnClick, Define.TouchEvent.Touch);

        GameObject cancelBtn = GetButton((int)Buttons.Cancel_btn).gameObject;
        BindEvent(cancelBtn, ClosePopupUI, Define.TouchEvent.Touch);
    }

    public void BuyBtnClick(PointerEventData data)
    {
        //UI_ItemStore itemStore = parent.GetComponent<UI_ItemStore>();
        //정보 넘긴 후 삭제
        Debug.Log($"{buyAmountSlider.value}개 구매");
        ClosePopupUI();
    }

    public void BuyAmountChanged()
    {
        handleTxt.text = buyAmountSlider.value.ToString();
    }

    private void Start()
    {
        Init();
    }
}
