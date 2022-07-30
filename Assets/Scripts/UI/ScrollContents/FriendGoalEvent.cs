using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FriendGoalEvent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ScrollRect scroll;

    void Start()
    {
        scroll = FindObjectOfType<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (scroll)
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
