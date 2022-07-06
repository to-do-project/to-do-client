using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ButtonTouchHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ScrollRect parent;

    void Start()
    {
        parent = transform.parent.parent.parent.parent.GetComponent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        //Debug.Log("Scroll!!");
        parent.OnBeginDrag(e);
    }
    public void OnDrag(PointerEventData e)
    {
        //Debug.Log("Scroll!!");
        parent.OnDrag(e);
    }
    public void OnEndDrag(PointerEventData e)
    {
        //Debug.Log("Scroll!!");
        parent.OnEndDrag(e);
    }


    public void SetParent(ScrollRect sc)
    {
        parent = sc;
    }
}
