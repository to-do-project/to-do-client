using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_ItemBtn : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    long itemId;
    UI_ItemStore parScript;
    ScrollRect scroll;
    Action<PointerEventData> OnClickHandler;

    public void SetValue(long itemId, ScrollRect scroll, UI_ItemStore parScript)
    {
        this.itemId = itemId;
        this.scroll = scroll;
        this.parScript = parScript;
        OnClickHandler = ItemBtnClick;
    }

    public void ItemBtnClick(PointerEventData data)
    {
        if(parScript)
        {
            parScript.OnBuyView(itemId);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(scroll)
        {
            scroll.OnBeginDrag(eventData);
            OnClickHandler = null;
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
            OnClickHandler = ItemBtnClick;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
        }
    }
}
