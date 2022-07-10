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
        if(PlayerPrefs.HasKey(Define.POINT)) 
        {
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

        charBtnDict = new Dictionary<long, Transform>();
        planetBtnDict = new Dictionary<long, Transform>();

        InitItemBtns();

        CheckPoint();
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }
    void InitItemBtns()
    {
        foreach (var i in dataContainer.charBtnId)
        {
            AddCharItem(i);
        }

        foreach (var i in dataContainer.planetBtnId)
        {
            AddPlanetItem(i);
        }
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
        if (id - itemImages.PlanetGap == 8) btn.ItemSizeUp();
    }

    public void OnBuyView(long id, Sprite sprite, bool sizeUp)
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
                if (sizeUp) item.ItemSizeUp();
                onBtn = false;
            }
            else if (response.code == 6000)
            {
                Debug.Log(response.message);
                string[] new_hN = { Define.JWT_ACCESS_TOKEN,
                                    "User-Id" };
                string[] new_hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                                    Managers.Player.GetString(Define.USER_ID) };
                Managers.Web.SendUniRequest("api/store/items/" + id, "GET", null, (uwr) =>
                {
                    Response<ResponseItemDetail> response = JsonUtility.FromJson<Response<ResponseItemDetail>>(uwr.downloadHandler.text);
                    if (response.code == 1000)
                    {
                        Debug.Log(response.result);
                        UI_ItemBuy item = Managers.UI.ShowPopupUI<UI_ItemBuy>("ItemBuyView", "Menu/ItemStore");
                        item.SetValue(id, response.result.type, response.result.name, response.result.description,
                                      response.result.price, response.result.maxCnt, gameObject, sprite);
                        if (sizeUp) item.ItemSizeUp();
                    } 
                    else
                    {
                        Debug.Log("토큰 재발급 실패(아이템 스토어)");
                    }
                    onBtn = false;
                }, new_hN, new_hV);
            }
            else
            {
                Debug.Log(response.message);
                onBtn = false;
            }
        }, hN, hV);
    }

    public void SetPoint(int amount)
    {
        int originUsedPoint = (Managers.Player.GetInt(Define.USEDPOINT) != -1) ? Managers.Player.GetInt(Define.USEDPOINT) : 0; 
        int usedPoint = amount - Managers.Player.GetInt(Define.POINT) + originUsedPoint;
        Managers.Player.SetInt(Define.USEDPOINT, usedPoint);
        Managers.Player.SetInt(Define.POINT, amount);
        pointText.text = amount.ToString();
    }

    public void DeleteItem(long id)
    {
        if (charBtnDict.ContainsKey(id))
        {
            Destroy(charBtnDict[id].gameObject);
            for(int i = 0; i < dataContainer.charBtnId.Count; i++)
                if (dataContainer.charBtnId[i] == id) dataContainer.charBtnId.RemoveAt(i);
            dataContainer.invenIdList.Add(id);
        }

        if (planetBtnDict.ContainsKey(id)) {
            Destroy(planetBtnDict[id].gameObject);
            for (int i = 0; i < dataContainer.planetBtnId.Count; i++)
                if (dataContainer.planetBtnId[i] == id) dataContainer.planetBtnId.RemoveAt(i);
        }
    }

    public void ChangeColor(string color)
    {
        int index;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profileImage.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    void CheckPoint()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/planet/my-info", "GET", null, (uwr) => {
            Response<ResponsePlanetInfo> response = JsonUtility.FromJson<Response<ResponsePlanetInfo>>(uwr.downloadHandler.text);
            if (response.code == 1000)
            {
                Managers.Player.SetInt(Define.POINT, response.result.point);
                pointText.text = Managers.Player.GetInt(Define.POINT).ToString();

            }
            else if (response.code == 6000)
            {
                Debug.Log(response.message);
                Managers.Player.SendTokenRequest(CheckPoint);
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    private void Start()
    {
        Init();
    }
}