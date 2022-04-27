using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item_EventHandler : MonoBehaviour
{
    // public Action<PointerEventData> OnClickHandler = null;

    /*    public void OnPointerClick(PointerEventData eventData)
        {
            if (OnClickHandler != null)
            {
                Debug.Log("Click Object");
                OnClickHandler.Invoke(eventData);
            }
        }*/

    void OnMouseDrag()
    {
        Debug.Log("Check btn click");
    }
}
