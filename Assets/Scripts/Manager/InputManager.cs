using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager 
{
    //시스템 터치
    public Action<Define.SystemEvent> SystemTouchAction = null;
    //일반 터치
    public Action<Define.TouchEvent> TouchAction = null;

    //unity editor test용
    public Action<Define.EditorEvent> WheelAction = null;

    bool pressed = false;
    float minPressTime = 0.2f;
    float PressTimer = 0;

    public void OnUpdate()
    {
        //시스템 뒤로가기 키
        if (SystemTouchAction != null)
        {
            if (Application.platform == RuntimePlatform.Android)
            {

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Debug.Log("Press Escape Key");
                    SystemTouchAction.Invoke(Define.SystemEvent.Back);
                }

            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {

                    SystemTouchAction.Invoke(Define.SystemEvent.Back);
                }
            }
        }

#if UNITY_EDITOR
        //유니티 에디터에서 확대 체크 용
        if (Input.GetAxis("Mouse ScrollWheel") !=0)
        {
            if (WheelAction != null)
            {
                WheelAction.Invoke(Define.EditorEvent.Wheel);
            }
        }

        //꾹 누르는거
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
        //안드로이드에서
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
                        TouchAction.Invoke(Define.TouchEvent.Touch);
                    }
                    else if(Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        TouchAction.Invoke(Define.TouchEvent.TouchMove);
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
                            //TouchAction.Invoke(Define.TouchEvent.Touch);
                            //TouchAction.Invoke(Define.TouchEvent.TouchEnd);
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
