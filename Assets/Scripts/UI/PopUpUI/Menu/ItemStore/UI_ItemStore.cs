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

    long selectBtn;
    long beforeBtn;

    bool onBtn = false;

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
        
        // 아이템 정보 불러온 후 무명 메소드에 넣을 것

        foreach (var i in charBtnId)
        {
            AddCharItem(i);
        }

        foreach (var i in charBtnId)
        {
            AddPlanetItem(i);
        }

        // ----------------------------------------
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
        btn.SetValue(id, charScroll.GetComponent<ScrollRect>(), gameObject.GetComponent<UI_ItemStore>());
    }

    private void AddPlanetItem(long id)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/Item_btn"));
        planetBtnDict.Add(id, item.transform);
        planetBtnDict[id].SetParent(planetItemContent.transform, false);

        UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
        btn.SetValue(id, planetScroll.GetComponent<ScrollRect>(), gameObject.GetComponent<UI_ItemStore>());
    }

    public void OnBuyView(long id)
    {
        if(onBtn)
        {
            return;
        }
        else
        {
            onBtn = true;
            // 아이템 정보 가져오기 (코루틴 끝나면 onBtn false로)
            UI_ItemBuy item = Managers.UI.ShowPopupUI<UI_ItemBuy>("ItemBuyView", "Menu/ItemStore");
            onBtn = false;
            //item.SetValue();
        }
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

[System.Serializable]
public class Item_ItemStore
{
    public long itemId;
    public string name;
    public string description;
    public int price;
    public string type;
    public int minCnt;
    public int maxCnt;
}