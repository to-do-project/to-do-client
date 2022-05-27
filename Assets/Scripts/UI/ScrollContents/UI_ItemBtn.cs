using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_ItemBtn : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    long itemId;
    UI_ItemStore itemScript;
    UI_Deco decoScript;
    ScrollRect scroll;
    Action<PointerEventData> OnClickHandler;

    public void SetValue(long itemId, ScrollRect scroll)
    {
        this.itemId = itemId;
        this.scroll = scroll;
        OnClickHandler = ItemBtnClick;
    }

    public void SetItemScript(UI_ItemStore itemScript)
    {
        this.itemScript = itemScript;
    }

    public void SetDecoScript(UI_Deco decoScript)
    {
        this.decoScript = decoScript;
    }

    public void ItemBtnClick(PointerEventData data)
    {
        if(itemScript != null)
        {
            itemScript.OnBuyView(itemId);
        }
        if(decoScript != null)
        {
            decoScript.ChangeCloth(itemId);
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
