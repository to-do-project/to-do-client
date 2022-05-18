using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemStore : UI_PopupMenu
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
        AddCharItem();
        AddCharItem();

        AddPlanetItem();
        AddPlanetItem();
        AddPlanetItem();
        AddPlanetItem();
        AddPlanetItem();
        AddPlanetItem();
        AddPlanetItem();
        AddPlanetItem();
        AddPlanetItem();
        AddPlanetItem();
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
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
