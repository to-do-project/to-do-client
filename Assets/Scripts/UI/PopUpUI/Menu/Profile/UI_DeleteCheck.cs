using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_DeleteCheck : UI_Popup
{
    enum Buttons
    {
        End_btn,
    }

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        //뒤로가기 기능 삭제(팝업 삭제 안되도록)
        Managers.Input.SystemTouchAction -= Managers.Input.SystemTouchAction;
    }

    private void CameraSet()
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

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        GameObject endBtn = GetButton((int)Buttons.End_btn).gameObject;
        BindEvent(endBtn, EndBtnClick, Define.TouchEvent.Touch);
    }

    private void Start()
    {
        Init();
    }

    public void EndBtnClick(PointerEventData data)
    {
        Application.Quit();
    }
}
