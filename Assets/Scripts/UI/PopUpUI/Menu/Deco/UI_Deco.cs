using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Deco : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
        Done_btn,
        Cancel_btn,
        Left_btn,
        Right_btn,
    }

    enum Images
    {
        Cloth_img,
    }

    GameObject invenScroll, invenContent, invenButton;
    ItemImageContainer itemImages;

    List<long> invenIdList;
    Dictionary<long, GameObject> contentList;

    string itemPath = "Prefabs/UI/ScrollContents/Item_btn";
    string fadePath = "UI/Popup/Menu/Deco/";
    Image clothImage;
    Button leftBtn, rightBtn;
    int index = 0;
    int startIndex = 0;
    bool onSave = false;

    public override void Init()
    {
        contentList = new Dictionary<long, GameObject>();
        invenIdList = new List<long>();
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Image>(typeof(Images));
        clothImage = GetImage((int)Images.Cloth_img);

        if (invenContent == null)
        {
            invenContent = GameObject.Find("InvenContent");
        }
        if (invenScroll == null)
        {
            invenScroll = GameObject.Find("InvenScroll");
        }

        itemImages = GetComponent<ItemImageContainer>();

        InvenWeb();
    }

    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Done_btn, (data) => { SaveInven(); });

        SetBtn((int)Buttons.Cancel_btn, (data) => {
            index = startIndex;
            ChangeCloth(invenIdList[index]);
            Managers.Resource.Instantiate(fadePath + "CancelFadeView");
        });

        leftBtn = GetButton((int)Buttons.Left_btn);
        BindEvent(leftBtn.gameObject, (data) => {
            if (index == 0) return;
            index--; 
            ChangeCloth(invenIdList[index]); 
        }, Define.TouchEvent.Touch);

        rightBtn = GetButton((int)Buttons.Right_btn);
        BindEvent(rightBtn.gameObject, (data) => {
            if (index == invenIdList.Count - 1) return;
            index++; 
            ChangeCloth(invenIdList[index]);
        }, Define.TouchEvent.Touch);
    }

    void Start()
    {
        Init();
    }

    void InvenWeb()
    {
        SetIndex(Managers.Player.GetInt(Define.CHARACTER_ITEM));
        ChangeCloth(invenIdList[index]);

        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/closet/character-items", "GET", null, (uwr) => {
            Response<ResponseCloset> response = JsonUtility.FromJson<Response<ResponseCloset>>(uwr.downloadHandler.text);
            if(response.isSuccess)
            {
                Debug.Log(uwr.downloadHandler.text);
                invenIdList = response.result.characterItemIdList;

                InitContents();
                RefreshBtnLR();

                SetIndex(Managers.Player.GetInt(Define.CHARACTER_ITEM));
                startIndex = index;
                Debug.Log(startIndex);

                ChangeCloth(invenIdList[index]);
            } else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    void SaveInven()
    {
        if (onSave) return;
        onSave = true;
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/closet/character-items/" + invenIdList[index].ToString(), "PATCH", null, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                Debug.Log(response.result);
                Managers.Player.SetInt(Define.CHARACTER_ITEM, (int)invenIdList[index]);
                Managers.Resource.Instantiate(fadePath + "DoneFadeView");
            }
            else
            {
                Debug.Log(response.message);
            }
            onSave = false;
        }, hN, hV);
    }

    void InitContents()
    {
        foreach (var tmp in invenIdList)
        {
            GameObject item = Instantiate(Resources.Load<GameObject>(itemPath));
            item.transform.SetParent(invenContent.transform, false);
            item.GetComponent<Button>().transition = Selectable.Transition.None;
            item.transform.Find("Base").GetComponent<Image>().color = new Color(217f / 255f, 217f / 255f, 217f / 255f);

            UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
            btn.SetValue(tmp, invenScroll.GetComponent<ScrollRect>(), itemImages.CharItemSprites[tmp - itemImages.CharGap]);
            btn.SetDecoScript(GetComponent<UI_Deco>());

            contentList.Add(tmp, item);
        }
    }

    void RefreshBtnLR()
    {
        if (onSave) return;
        if (index == 0)
        {
            leftBtn.interactable = false;
        } else
        {
            leftBtn.interactable = true;
        }

        if(index == invenIdList.Count - 1)
        {
            rightBtn.interactable = false;
        } else
        {
            rightBtn.interactable = true;
        }
    }

    void SetIndex(long id)
    {
        for(int i = 0; i < invenIdList.Count; i++)
        {
            if (invenIdList[i] == id) index = i;
        }
    }

    public void ChangeCloth(long id)
    {
        if (onSave) return;
        if (invenButton != null)
        {
            invenButton.transform.localScale = new Vector3(1, 1, 1);
            invenButton.transform.Find("Base").GetComponent<Image>().color = new Color(217f / 255f, 217f / 255f, 217f / 255f);
        }
        invenButton = contentList[id];
        invenButton.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        invenButton.transform.Find("Base").GetComponent<Image>().color = new Color(1, 1, 1);

        clothImage.sprite = itemImages.CharItemSprites[id - itemImages.CharGap];

        ContentSizeFitter fitter = invenContent.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);

        SetIndex(id);
        RefreshBtnLR();
    }
}
