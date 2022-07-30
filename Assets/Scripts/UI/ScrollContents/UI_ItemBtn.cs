using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// 아이템 스토어 및 캐릭터 꾸미기에 사용되는 아이템 버튼 스크립트
public class UI_ItemBtn : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    // 스크립트 변수
    UI_ItemStore itemScript;
    UI_Deco decoScript;
    // 스크롤 뷰의 ScrollRect
    ScrollRect scroll;
    // 이미지 및 이미지의 스프라이트 
    Image image;
    Sprite sprite;
    // 아이템 버튼 클릭 이벤트
    Action<PointerEventData> OnClickHandler;

    long itemId; // 아이템 Id
    bool sizeUp = false;

    // 버튼 초기화 함수
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

    // 아이템 스토어에서 사용 시 아이템 스토어 스크립트 추가
    public void SetItemScript(UI_ItemStore itemScript)
    {
        this.itemScript = itemScript;
    }

    // 캐릭터 꾸미기에서 사용 시 캐릭터 꾸미기 스크립트 추가
    public void SetDecoScript(UI_Deco decoScript)
    {
        this.decoScript = decoScript;
    }

    // 아이템 사이즈 키우기
    public void ItemSizeUp()
    {
        image.transform.localScale *= 5f;
        sizeUp = true;
    }

    // 버튼 클릭 이벤트
    public void ItemBtnClick(PointerEventData data)
    {
        // 아이템 스토어일 경우
        if(itemScript != null)
        {
            // 구매 이벤트 발생
            itemScript.OnBuyView(itemId, sprite, sizeUp);
        }
        // 캐릭터 꾸미기일 경우
        if(decoScript != null)
        {
            // 옷 변경 이벤트 발생
            decoScript.ChangeCloth(itemId);
        }

        // 버튼음 재생
        Managers.Sound.PlayNormalButtonClickSound();
    }

    // Scroll에 이벤트 데이터 전달
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(scroll)
        {
            scroll.OnBeginDrag(eventData);
            OnClickHandler = null;
        }
    }

    // Scroll에 이벤트 데이터 전달
    public void OnDrag(PointerEventData eventData)
    {
        if (scroll)
        {
            scroll.OnDrag(eventData);
        }
    }

    // Scroll에 이벤트 데이터 전달
    public void OnEndDrag(PointerEventData eventData)
    {
        if (scroll)
        {
            scroll.OnEndDrag(eventData);
            OnClickHandler = ItemBtnClick;
        }
    }

    // Scroll에 이벤트 데이터 전달
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
        }
    }
}
