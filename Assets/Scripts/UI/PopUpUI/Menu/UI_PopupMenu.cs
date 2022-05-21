using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UI_PopupMenu : UI_Popup // Menu에서 많이 사용하는 함수들을 여기에 정리
{
    protected void SetBtn(int index, Action<PointerEventData> action) // 바인드된 버튼들을 무명 메소드로 이벤트 연결
    {
        GameObject btn = GetButton(index).gameObject;
        BindEvent(btn, action, Define.TouchEvent.Touch);
    }

    protected void CameraSet()  // 캔버스에 UI 카메라 연결, Init()에 사용
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }
    }
}
