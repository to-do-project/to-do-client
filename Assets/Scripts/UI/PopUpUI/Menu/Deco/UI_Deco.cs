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

        SetInven();
    }

    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, (data) => { Managers.Sound.PlayNormalButtonClickSound(); ClosePopupUI(); });

        SetBtn((int)Buttons.Done_btn, (data) => { SaveInven(); });

        SetBtn((int)Buttons.Cancel_btn, (data) => {
            index = startIndex;
            ChangeCloth(dataContainer.invenIdList[index]);
            Managers.Resource.Instantiate(fadePath + "CancelFadeView");
            Managers.Sound.PlayPopupSound();
        });

        leftBtn = GetButton((int)Buttons.Left_btn);
        BindEvent(leftBtn.gameObject, (data) => {
            if (index == 0) return;
            index--; 
            ChangeCloth(dataContainer.invenIdList[index]); 
            Managers.Sound.PlayNormalButtonClickSound();
        }, Define.TouchEvent.Touch);

        rightBtn = GetButton((int)Buttons.Right_btn);
        BindEvent(rightBtn.gameObject, (data) => {
            if (index == dataContainer.invenIdList.Count - 1) return;
            index++; 
            ChangeCloth(dataContainer.invenIdList[index]);
            Managers.Sound.PlayNormalButtonClickSound();
        }, Define.TouchEvent.Touch);
    }

    void Start()
    {
        Init();
    }

    void SetInven()
    {
        InitContents();
        RefreshBtnLR();

        SetIndex(Managers.Player.GetInt(Define.CHARACTER_ITEM));
        startIndex = index;

        ChangeCloth(dataContainer.invenIdList[index]);
    }

    void InitContents()
    {
        foreach (var tmp in dataContainer.invenIdList)
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
        }
        else
        {
            leftBtn.interactable = true;
        }

        if (index == dataContainer.invenIdList.Count - 1)
        {
            rightBtn.interactable = false;
        }
        else
        {
            rightBtn.interactable = true;
        }
    }

    void SetIndex(long id)
    {
        for (int i = 0; i < dataContainer.invenIdList.Count; i++)
        {
            if (dataContainer.invenIdList[i] == id) index = i;
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

    void SaveInven()
    {
        if (onSave) return;
        onSave = true;
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/closet/character-items/" + dataContainer.invenIdList[index].ToString(), "PATCH", null, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                Debug.Log(response.result);
                Managers.Player.SetInt(Define.CHARACTER_ITEM, (int)dataContainer.invenIdList[index]);
                Managers.Player.UpdateCharacterItem(dataContainer.invenIdList[index]);
                Managers.Resource.Instantiate(fadePath + "DoneFadeView");
                Managers.Sound.PlayPopupSound();
            }
            else if(response.code == 6000)
            {
                Debug.Log(response.message);
                Managers.Player.SendTokenRequest(SaveInven);
            }
            else
            {
                Debug.Log(response.message);
            }
            onSave = false;
        }, hN, hV);
    }
}
