using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Action OnItemClickAction = null;
    public Action OnItemDragAction = null;

    void OnMouseDown()
    {
        if (OnItemClickAction != null)
        {
            OnItemClickAction.Invoke();
        }
    }
    void OnMouseDrag()
    {
        if (OnItemDragAction != null)
        {
            OnItemDragAction.Invoke();
        }
    }

}
