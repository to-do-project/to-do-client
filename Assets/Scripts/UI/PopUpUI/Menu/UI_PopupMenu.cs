using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UI_PopupMenu : UI_Popup // Menu���� ���� ����ϴ� �Լ����� ���⿡ ����
{
    protected UIDataCamera dataContainer; // UICamera�� Ȱ���� ���ε� ������ �����

    protected GameObject SetBtn(int index, Action<PointerEventData> action) // ���ε�� ��ư���� ���� �޼ҵ�� �̺�Ʈ ����
    {
        GameObject btn = GetButton(index).gameObject;
        BindEvent(btn, action, Define.TouchEvent.Touch);
        return btn;
    }

    protected void CameraSet()  // ĵ������ UI ī�޶� ����, Init()�� ���
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>(); // UICamera�� ã�� ����
            canvas.worldCamera = cam;
            dataContainer = cam.gameObject.GetComponent<UIDataCamera>();            // UICamera�� �ִ� ������ �����̳� ����
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }
    }
}
