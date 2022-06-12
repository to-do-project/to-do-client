using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Action OnItemClickAction = null;
    public Action OnItemDragAction = null;
    public Action OnItemExitAction = null;

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

    private void OnMouseUp()
    {
        if (OnItemExitAction != null)
        {
            OnItemExitAction.Invoke();
        }
    }
}
