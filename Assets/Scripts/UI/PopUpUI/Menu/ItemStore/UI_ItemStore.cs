using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemStore : UI_Popup
{
    enum Buttons
    {
        Back_btn,
    }

    private GameObject charItemContent, planetItemContent;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        if (charItemContent == null)
        {
            charItemContent = GameObject.Find("CharItemContent");
        }

        if (planetItemContent == null)
        {
            planetItemContent = GameObject.Find("PlanetItemContent");
        }

        AddCharItem();
        AddCharItem();
        AddCharItem();

        AddPlanetItem();
        AddPlanetItem();
        AddPlanetItem();
        AddPlanetItem();
        AddPlanetItem();
        AddPlanetItem();
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
    }

    private void AddCharItem()
    {
        GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/Item_btn"));
        item.transform.SetParent(charItemContent.transform, false);
        UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
        //btn.SetValue(); 버튼 정보 넘기기
    }

    private void AddPlanetItem()
    {
        GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/Item_btn"));
        item.transform.SetParent(planetItemContent.transform, false);
        UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
        //btn.SetValue(); 버튼 정보 넘기기
    }

    private void Start()
    {
        Init();
    }
}
