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
    Sprite sprite;
    Image image;
    bool sizeUp = false;

    public void SetValue(long itemId, ScrollRect scroll, Sprite sprite)
    {
        this.itemId = itemId;
        this.scroll = scroll;
        this.sprite = sprite;
        image = transform.Find("Item_img").GetComponent<Image>();
        image.sprite = sprite;
        image.SetNativeSize();
        image.transform.localScale *= 0.5f;
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

    public void ItemSizeUp()
    {
        image.transform.localScale *= 5f;
        sizeUp = true;
    }

    public void ItemBtnClick(PointerEventData data)
    {
        if(itemScript != null)
        {
            itemScript.OnBuyView(itemId, sprite, sizeUp);
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
