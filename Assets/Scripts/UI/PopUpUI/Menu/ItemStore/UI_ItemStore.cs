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

    GameObject charItemContent, planetItemContent, charScroll, planetScroll;

    List<long> charBtnId;
    List<long> planetBtnId;

    Dictionary<long, Transform> charBtnDict;
    Dictionary<long, Transform> planetBtnDict;

    float gap = 0;
    float maxGap = 0.1f;
    float lerp = 10f;
    bool toggle = true;

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

        if (charScroll == null)
        {
            charScroll = GameObject.Find("CharScroll");
        }

        if (planetScroll == null)
        {
            planetScroll = GameObject.Find("PlanetScroll");
        }

        charBtnId = new List<long>();
        planetBtnId = new List<long>();

        charBtnDict = new Dictionary<long, Transform>();
        planetBtnDict = new Dictionary<long, Transform>();

#if UNITY_EDITOR
        for(int i = 0; i < 10; i++)
        {
            charBtnId.Add(i);
            planetBtnId.Add(i);
            planetBtnId.Add(i + 10);
        }
#endif

        foreach (var i in charBtnId)
        {
            AddCharItem(i);
        }

        foreach (var i in charBtnId)
        {
            AddPlanetItem(i);
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }

    private void AddCharItem(long id)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/Item_btn"));
        charBtnDict.Add(id, item.transform);
        charBtnDict[id].SetParent(charItemContent.transform, false);
        UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
        btn.SetValue(true, "아이템이름", 1000, 1, 7, charScroll);
        //btn.SetValue(); 버튼 정보 넘기기
    }

    private void AddPlanetItem(long id)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/Item_btn"));
        planetBtnDict.Add(id, item.transform);
        planetBtnDict[id].SetParent(planetItemContent.transform, false);
        UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
        btn.SetValue(false, "아이템이름", 1000, 1, 7, planetScroll);
        //btn.SetValue(); 버튼 정보 넘기기
    }

    private void UpdateScales()
    {
        if(toggle)
        {
            if(gap < maxGap - 0.00125f)
            {
                gap = Mathf.Lerp(gap, maxGap, lerp * Time.deltaTime);
            } else
            {
                gap = maxGap;
                toggle = !toggle;
                Debug.Log("반복중");
            }
        } else
        {
            if (gap > -maxGap + 0.00125f)
            {
                gap = Mathf.Lerp(gap, -maxGap, lerp * Time.deltaTime);
            }
            else
            {
                gap = -maxGap;
                toggle = !toggle;
            }
        }
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        UpdateScales();
    }
}
