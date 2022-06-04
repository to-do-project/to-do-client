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

    enum Texts
    {
        Profile_text,
        Point_text
    }

    enum Images
    {
        Profile_image,
    }

    GameObject charItemContent, planetItemContent, charScroll, planetScroll;
    Text profileText, pointText;
    Image profileImage;
    ItemImageContainer itemImages;

    List<long> charBtnId;
    List<long> planetBtnId;

    Dictionary<long, Transform> charBtnDict;
    Dictionary<long, Transform> planetBtnDict;

    bool onBtn = false;

    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        profileText = GetText((int)Texts.Profile_text);
        pointText = GetText((int)Texts.Point_text);

        profileImage = GetImage((int)Images.Profile_image);

        if(PlayerPrefs.HasKey(Define.NICKNAME))
        {
            profileText.text = Managers.Player.GetString(Define.NICKNAME);
        }
        if(PlayerPrefs.HasKey(Define.POINT)) {
            pointText.text = Managers.Player.GetInt(Define.POINT).ToString();
        }
        if(PlayerPrefs.HasKey(Define.PROFILE_COLOR))
        {
            ChangeColor(Managers.Player.GetString(Define.PROFILE_COLOR));
        }

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

        itemImages = GetComponent<ItemImageContainer>();

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

        Managers.Web.SendUniRequest("api/store", "GET", null, (uwr) => {
            Response<ResponseStoreItemsId> response = JsonUtility.FromJson<Response<ResponseStoreItemsId>>(uwr.downloadHandler.text);
            if (response.code == 1000)
            {
                Debug.Log(uwr.downloadHandler.text);
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
        btn.SetValue(id, charScroll.GetComponent<ScrollRect>(), itemImages.CharItemSprites[id - itemImages.CharGap]);
        btn.SetItemScript(gameObject.GetComponent<UI_ItemStore>());
    }

    private void AddPlanetItem(long id)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/Item_btn"));
        planetBtnDict.Add(id, item.transform);
        planetBtnDict[id].SetParent(planetItemContent.transform, false);

        UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
        btn.SetValue(id, planetScroll.GetComponent<ScrollRect>(), itemImages.PlanetItemSprites[id - itemImages.PlanetGap]);
        btn.SetItemScript(gameObject.GetComponent<UI_ItemStore>());
    }

    public void OnBuyView(long id, Sprite sprite)
    {
        if(onBtn) return;
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
                item.SetValue(id, response.result.type, response.result.name, response.result.description, 
                                response.result.price, response.result.maxCnt, gameObject, sprite);
            }
            else
            {
                Debug.Log(response.message);
            }
            onBtn = false;
        }, hN, hV);
    }

    public void SetPoint(int amount)
    {
        Managers.Player.SetInt(Define.POINT, amount);
        pointText.text = amount.ToString();
    }

    public void DeleteItem(long id)
    {
        if (charBtnDict.ContainsKey(id)) Destroy(charBtnDict[id].gameObject);
        if (planetBtnDict.ContainsKey(id)) Destroy(planetBtnDict[id].gameObject);
    }

    public void ChangeColor(string color)
    {
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profileImage.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    private void Start()
    {
        Init();
    }
}