using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UI_PopupMenu : UI_Popup // Menu���� ���� ����ϴ� �Լ����� ���⿡ ����
{
    protected UIDataCamera dataContainer; // UICamera�� Ȱ���� ���ε� ������ �����

    protected void SetBtn(int index, Action<PointerEventData> action) // ���ε�� ��ư���� ���� �޼ҵ�� �̺�Ʈ ����
    {
        GameObject btn = GetButton(index).gameObject;
        BindEvent(btn, action, Define.TouchEvent.Touch);
    }

    protected void CameraSet()  // ĵ������ UI ī�޶� ����, Init()�� ���
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
            dataContainer = cam.gameObject.GetComponent<UIDataCamera>();
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }
    }
}
