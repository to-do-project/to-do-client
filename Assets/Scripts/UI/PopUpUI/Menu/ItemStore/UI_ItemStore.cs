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

        ItemRequest();

        /*      테스트용 코드
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
        */
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }
    void ItemRequest()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Debug.Log(hN[1]);
        Debug.Log(hV[1]);

        Managers.Web.SendUniRequest("api/store", "GET", null, (uwr) => {
            Response<ResponseStoreItemsId> response = JsonUtility.FromJson<Response<ResponseStoreItemsId>>(uwr.downloadHandler.text);
            if (response.code == 1000)
            {
                Debug.Log(response.result);
                charBtnId = response.result.characterItemIdList;
                planetBtnId = response.result.planetItemIdList;

                foreach (var i in charBtnId)
                {
                    AddCharItem(i);
                }

                foreach (var i in planetBtnId)
                {
                    AddPlanetItem(i);
                }
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    private void AddCharItem(long id)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/Item_btn"));
        charBtnDict.Add(id, item.transform);
        charBtnDict[id].SetParent(charItemContent.transform, false);

        UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
        btn.SetValue(id, charScroll.GetComponent<ScrollRect>());
        btn.SetItemScript(gameObject.GetComponent<UI_ItemStore>());
    }

    private void AddPlanetItem(long id)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/Item_btn"));
        planetBtnDict.Add(id, item.transform);
        planetBtnDict[id].SetParent(planetItemContent.transform, false);

        UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
        btn.SetValue(id, planetScroll.GetComponent<ScrollRect>());
        btn.SetItemScript(gameObject.GetComponent<UI_ItemStore>());
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
            string[] hN = { Define.JWT_ACCESS_TOKEN,
                            "User-Id" };
            string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                            Managers.Player.GetString(Define.USER_ID) };

            Managers.Web.SendUniRequest("api/store/items/" + id, "GET", null, (uwr) => {
                Response<ResponseItemDetail> response = JsonUtility.FromJson<Response<ResponseItemDetail>>(uwr.downloadHandler.text);
                if (response.code == 1000)
                {
                    Debug.Log(response.result);
                    UI_ItemBuy item = Managers.UI.ShowPopupUI<UI_ItemBuy>("ItemBuyView", "Menu/ItemStore");
                    item.SetValue(response.result.type == "행성 아이템", response.result.name, response.result.description, response.result.price, response.result.maxCnt, gameObject);
                }
                else
                {
                    Debug.Log(response.message);
                }
                onBtn = false;
            }, hN, hV);
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