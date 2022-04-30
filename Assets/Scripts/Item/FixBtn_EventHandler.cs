using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixBtn_EventHandler : MonoBehaviour
{
    public Action OnClickHandler = null;

    void OnMouseDown()
    {
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke();
        }

    }
}
