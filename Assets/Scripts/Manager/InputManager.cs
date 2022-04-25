using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager 
{
    //�ý��� ��ġ
    public Action<Define.SystemEvent> SystemTouchAction = null;
    //�Ϲ� ��ġ
    public Action<Define.TouchEvent> TouchAction = null;

    bool pressed = false;
    float minPressTime = 0.25f;
    float PressTimer = 0;

    public void OnUpdate()
    {
        //�ý��� �ڷΰ��� Ű
        if (SystemTouchAction != null)
        {
            if (Application.platform == RuntimePlatform.Android)
            {

                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    SystemTouchAction.Invoke(Define.SystemEvent.Back);
                }

            }
            else
            {
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    SystemTouchAction.Invoke(Define.SystemEvent.Back);
                }
            }
        }

#if UNITY_EDITOR
        if (TouchAction != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pressed = true;
            }
            else if (Input.GetMouseButton(0) && pressed)
            {
                pressed = true;
                PressTimer += Time.deltaTime;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                pressed = false;
                if(PressTimer>= minPressTime)
                {
                    TouchAction.Invoke(Define.TouchEvent.Press);
                }
                else
                {
                    TouchAction.Invoke(Define.TouchEvent.Touch);
                }

                PressTimer = 0;
            }
            
        }
#else
        //�ȵ���̵忡��
        if (TouchAction != null)
        {
            if (Input.touchCount > 0)
            {
                if (Input.touchCount == 1)
                {
                    if (pressed)
                    {
                        PressTimer += Time.deltaTime;
                    }

                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        pressed = true;
                        //PressTimer += Time.deltaTime;
                    }

                    else if(Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        pressed = false;
                        if (PressTimer >= minPressTime)
                        {
                            TouchAction.Invoke(Define.TouchEvent.Press);
                        }
                        else
                        {
                            TouchAction.Invoke(Define.TouchEvent.Touch);
                        }
                        PressTimer = 0;
                    }
                }

                if (Input.touchCount == 2 )
                {
                    TouchAction.Invoke(Define.TouchEvent.TwoTouch);
                }

            }
        }

#endif




    }

    public void Clear()
    {
        SystemTouchAction = null;
        TouchAction = null;
    }
}
