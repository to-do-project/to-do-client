using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Collector : UI_Popup
{
    enum Buttons
    {
        Back_btn,
        Test_btn
    }
    private GameObject content = null;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        if (content == null)
        {
            content = GameObject.Find("Content");
        }
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

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);

        GameObject testBtn = GetButton((int)Buttons.Test_btn).gameObject;
        BindEvent(testBtn, TestBtnClick, Define.TouchEvent.Touch);
    }

    private void Start()
    {
        Init();
    }

    public void TestBtnClick(PointerEventData data)
    {
        AddTarget("목표 제목", "한 달 전");
    }

    //API에서 데이터 가져와서 컨텐츠에 집어넣기(최신순 먼저)
    void AddTarget(string name, string period)
    {
        GameObject target = Managers.Resource.Instantiate("UI/ScrollContents/TargetContent");
        target.transform.SetParent(content.transform);
        target.transform.localScale = new Vector3(1, 1, 1);
    }
}
