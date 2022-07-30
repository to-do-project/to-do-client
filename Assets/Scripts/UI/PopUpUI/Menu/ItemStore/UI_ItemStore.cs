using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemStore : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
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
    // ================================ //

    // ��ũ�� �� �� ������ �θ� ������Ʈ
    GameObject charItemContent, planetItemContent, charScroll, planetScroll;
    // �ؽ�Ʈ ������Ʈ
    Text profileText, pointText;
    // ������ ���� �̹���
    Image profileImage;
    // ������ �̹��� ���� ��ũ��Ʈ
    ItemImageContainer itemImages;

    // ������ ��ư�� Transform�� �����ϴ� Dictionary
    Dictionary<long, Transform> charBtnDict;
    Dictionary<long, Transform> planetBtnDict;

    bool onBtn = false; // �� ��� üũ

    // ���
    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    // �ʱ�ȭ
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

        // PlayerPrefs�� ���� ������ �̸� �ʱ�ȭ
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
        // =========== //

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

        // �� ����� ���� ���� �����͸� �޾ƿ� �� �ʱ�ȭ
        CheckPoint();
    }

    // ��ư �̺�Ʈ ����
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // �ڷΰ��� ��ư
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }

    // ������ ���� â �� ���
    // id = ������ ������ ���� id
    // sprite = ������ ������ �̹���
    // sizeUp = ������ �̹��� ������ ���� ����
    public void OnBuyView(long id, Sprite sprite, bool sizeUp)
    {
        if (onBtn) return;
        onBtn = true;
        
        // �� ��� ��� ��
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // ������ ���� â �� ���
        Managers.Web.SendUniRequest("api/store/items/" + id, "GET", null, (uwr) => {
            // �� ���� json �����͸� ����Ƽ �����ͷ� ��ȯ
            Response<ResponseItemDetail> response = JsonUtility.FromJson<Response<ResponseItemDetail>>(uwr.downloadHandler.text);

            // �� ��� ���� ��
            if (response.code == 1000)
            {
                // Debug.Log(response.result);
                // ������ ���� â ����
                UI_ItemBuy item = Managers.UI.ShowPopupUI<UI_ItemBuy>("ItemBuyView", "Menu/ItemStore");

                // �� ��� ����� ���� ���� â �ʱ�ȭ
                item.SetValue(id, response.result.type, response.result.name, response.result.description,
                              response.result.price, response.result.maxCnt, gameObject, sprite);

                // ������ �̹��� ũ�� ������ �ʿ��� ��� ũ�� ����
                if (sizeUp) item.ItemSizeUp();
                onBtn = false;
            }
            // ��ū ���� ��
            else if (response.code == 6000)
            {
                // Debug.Log(response.message);
                onBtn = false;

                // ��ū ��߱� �� ���� �� ��� ����
                Managers.Player.SendTokenRequest(() => {
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
                            Debug.Log("��ū ��߱� ����(������ �����)");
                        }
                    }, new_hN, new_hV);
                });
            }
            // ��Ÿ ���� ��
            else
            {
                Debug.Log(response.message);
                onBtn = false;
            }
        }, hN, hV);
    }

    // ����Ʈ �ؽ�Ʈ ����
    public void SetPoint(int amount)
    {
        int originUsedPoint = (Managers.Player.GetInt(Define.USEDPOINT) != -1) ? Managers.Player.GetInt(Define.USEDPOINT) : 0;
        int usedPoint = amount - Managers.Player.GetInt(Define.POINT) + originUsedPoint;
        Managers.Player.SetInt(Define.USEDPOINT, usedPoint);
        Managers.Player.SetInt(Define.POINT, amount);
        pointText.text = amount.ToString();
    }

    // �������� �ܿ� ������ ���� ��� ������ ����
    // id = ���� �� ������ ���� id
    public void DeleteItem(long id)
    {
        // Dictionary���� id ��ȸ �� ����
        if (charBtnDict.ContainsKey(id))
        {
            Destroy(charBtnDict[id].gameObject);
            for (int i = 0; i < dataContainer.charBtnId.Count; i++)
                if (dataContainer.charBtnId[i] == id) dataContainer.charBtnId.RemoveAt(i);

            // ĳ���� �������� ��� ������ ����� ������ ���ÿ� �κ��丮�� �߰� (�ִ� ������ 1��)
            dataContainer.invenIdList.Add(id);
        }

        if (planetBtnDict.ContainsKey(id))
        {
            Destroy(planetBtnDict[id].gameObject);
            for (int i = 0; i < dataContainer.planetBtnId.Count; i++)
                if (dataContainer.planetBtnId[i] == id) dataContainer.planetBtnId.RemoveAt(i);
        }
    }

    // ������ �÷� ����
    // color = ������ ����
    public void ChangeColor(string color)
    {
        // �÷����� UI_Color�� �ִ� enum�� Colors�� Ȱ���Ͽ� index������ ����
        int index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profileImage.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    // ������ ��ư ����
    void InitItemBtns()
    {
        // ĳ���� ������ ��ư ����
        foreach (var i in dataContainer.charBtnId)
        {
            AddCharItem(i);
        }

        // ��� ������ ��ư ����
        foreach (var i in dataContainer.planetBtnId)
        {
            AddPlanetItem(i);
        }
    }

    // ĳ���� ������ ��ư�� ����, �ʱ�ȭ�ϰ� Dictionary�� �߰��ϴ� �Լ�
    // id = ������ ���� id
    void AddCharItem(long id)
    {
        // ��ư ������Ʈ ���� �� �θ� ���
        GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/Item_btn"));
        charBtnDict.Add(id, item.transform);
        charBtnDict[id].SetParent(charItemContent.transform, false);

        // ��ư �ʱ�ȭ
        UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
        btn.SetValue(id, charScroll.GetComponent<ScrollRect>(), itemImages.CharItemSprites[id - itemImages.CharGap]);
        btn.SetItemScript(gameObject.GetComponent<UI_ItemStore>());
    }

    // ��� ������ ��ư�� ����, �ʱ�ȭ�ϰ� Dictionary�� �߰��ϴ� �Լ�
    // id = ������ ���� id
    void AddPlanetItem(long id)
    {
        // ��ư ������Ʈ ���� �� �θ� ���
        GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/Item_btn"));
        planetBtnDict.Add(id, item.transform);
        planetBtnDict[id].SetParent(planetItemContent.transform, false);

        // ��ư �ʱ�ȭ
        UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
        btn.SetValue(id, planetScroll.GetComponent<ScrollRect>(), itemImages.PlanetItemSprites[id - itemImages.PlanetGap]);
        btn.SetItemScript(gameObject.GetComponent<UI_ItemStore>());
        if (id - itemImages.PlanetGap == 8) btn.ItemSizeUp();
    }

    // ���� ����Ʈ ���� �ޱ����� �� ���
    void CheckPoint()
    {
        // �� ��� ��� ��
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // ���� ���� �� ���
        Managers.Web.SendUniRequest("api/planet/my-info", "GET", null, (uwr) => {
            // �� ���� json �����͸� ����Ƽ �����ͷ� ��ȯ
            Response<ResponsePlanetInfo> response = JsonUtility.FromJson<Response<ResponsePlanetInfo>>(uwr.downloadHandler.text);

            // �� ��� ���� ��
            if (response.code == 1000)
            {
                // ����Ʈ �� �缳��
                Managers.Player.SetInt(Define.POINT, response.result.point);
                pointText.text = Managers.Player.GetInt(Define.POINT).ToString();
            }
            // ��ū ���� ��
            else if (response.code == 6000)
            {
                Debug.Log(response.message);
                Managers.Player.SendTokenRequest(CheckPoint);
            }
            // ��Ÿ ���� ��
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    void Start()
    {
        Init();
    }
}