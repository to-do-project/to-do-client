using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<Define.SystemEvent> TouchAction = null;
    
    public void OnUpdate()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                TouchAction.Invoke(Define.SystemEvent.Back);
            }
        }

    }

    public void Clear()
    {
        TouchAction = null;
    }
}
