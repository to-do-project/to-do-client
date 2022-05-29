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

    GameObject invenScroll, invenContent;

    List<long> invenIdList;
    List<GameObject> clothList;
    Dictionary<long, GameObject> contentList;

    string itemPath = "Prefabs/UI/ScrollContents/Item_btn";
    Image clothImage;
    Button leftBtn, rightBtn;
    int index = 0;
    int startIndex = 0;

    public override void Init()
    {
        clothList = new List<GameObject>();
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

        InvenWeb();

        index = startIndex;
    }

    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Done_btn, (data) => { });

        SetBtn((int)Buttons.Cancel_btn, (data) => { });

        leftBtn = GetButton((int)Buttons.Left_btn);
        BindEvent(leftBtn.gameObject, (data) => { }, Define.TouchEvent.Touch);

        rightBtn = GetButton((int)Buttons.Right_btn);
        BindEvent(rightBtn.gameObject, (data) => { }, Define.TouchEvent.Touch);
    }

    void Start()
    {
        Init();
    }

    void InvenWeb()
    {
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
            } else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    void InitContents()
    {
        foreach (var tmp in invenIdList)
        {
            GameObject item = Instantiate(Resources.Load<GameObject>(itemPath));
            item.transform.SetParent(invenContent.transform, false);

            UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
            btn.SetValue(tmp, invenScroll.GetComponent<ScrollRect>());

            contentList.Add(tmp, item);
        }
    }

    void RefreshBtnLR()
    {
        if(index == 0)
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

    public void ChangeCloth(long id)
    {
        Debug.Log("*버튼 클릭* 아이템 아이디 >> " + id);
    }
}
