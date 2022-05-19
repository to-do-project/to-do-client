using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemBtn : UI_Base, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    bool isCharItem;
    string itemName;
    int price;
    int curHave;
    int maxHave;
    GameObject parent;
    ScrollRect scroll;

    public void SetValue(bool isCharItem, string itemName, int price, int curHave, int maxHave, GameObject parent)
    {
        this.isCharItem = isCharItem;
        this.itemName = itemName;
        this.price = price;
        this.curHave = curHave;
        this.maxHave = maxHave;
        this.parent = parent;
        scroll = parent.GetComponent<ScrollRect>();
    }

    public override void Init()
    {
        BindEvent(gameObject, ItemBtnClick, Define.TouchEvent.Touch);
    }

    public void ItemBtnClick(PointerEventData data)
    {
        UI_ItemBuy item = Managers.UI.ShowPopupUI<UI_ItemBuy>("ItemBuyView", "Menu/ItemStore");
        //item.SetValue(isCharItem, itemName, price, curHave, maxHave, parent); 정보 넘기기
    }

    private void Start()
    {
        Init();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(scroll)
        {
            scroll.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (scroll)
        {
            scroll.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (scroll)
        {
            scroll.OnEndDrag(eventData);
        }
    }
}
