using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// ������ ����� �� ĳ���� �ٹ̱⿡ ���Ǵ� ������ ��ư ��ũ��Ʈ
public class UI_ItemBtn : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    // ��ũ��Ʈ ����
    UI_ItemStore itemScript;
    UI_Deco decoScript;
    // ��ũ�� ���� ScrollRect
    ScrollRect scroll;
    // �̹��� �� �̹����� ��������Ʈ 
    Image image;
    Sprite sprite;
    // ������ ��ư Ŭ�� �̺�Ʈ
    Action<PointerEventData> OnClickHandler;

    long itemId; // ������ Id
    bool sizeUp = false;

    // ��ư �ʱ�ȭ �Լ�
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

    // ������ ������ ��� �� ������ ����� ��ũ��Ʈ �߰�
    public void SetItemScript(UI_ItemStore itemScript)
    {
        this.itemScript = itemScript;
    }

    // ĳ���� �ٹ̱⿡�� ��� �� ĳ���� �ٹ̱� ��ũ��Ʈ �߰�
    public void SetDecoScript(UI_Deco decoScript)
    {
        this.decoScript = decoScript;
    }

    // ������ ������ Ű���
    public void ItemSizeUp()
    {
        image.transform.localScale *= 5f;
        sizeUp = true;
    }

    // ��ư Ŭ�� �̺�Ʈ
    public void ItemBtnClick(PointerEventData data)
    {
        // ������ ������� ���
        if(itemScript != null)
        {
            // ���� �̺�Ʈ �߻�
            itemScript.OnBuyView(itemId, sprite, sizeUp);
        }
        // ĳ���� �ٹ̱��� ���
        if(decoScript != null)
        {
            // �� ���� �̺�Ʈ �߻�
            decoScript.ChangeCloth(itemId);
        }

        // ��ư�� ���
        Managers.Sound.PlayNormalButtonClickSound();
    }

    // Scroll�� �̺�Ʈ ������ ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(scroll)
        {
            scroll.OnBeginDrag(eventData);
            OnClickHandler = null;
        }
    }

    // Scroll�� �̺�Ʈ ������ ����
    public void OnDrag(PointerEventData eventData)
    {
        if (scroll)
        {
            scroll.OnDrag(eventData);
        }
    }

    // Scroll�� �̺�Ʈ ������ ����
    public void OnEndDrag(PointerEventData eventData)
    {
        if (scroll)
        {
            scroll.OnEndDrag(eventData);
            OnClickHandler = ItemBtnClick;
        }
    }

    // Scroll�� �̺�Ʈ ������ ����
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
        }
    }
}
