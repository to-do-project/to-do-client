using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Friend : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
        Search_btn,
    }
    enum Texts
    {
        FriendOnly_txt,
        Request_txt,
        Friend_txt,
    }

    enum InputFields
    {
        Friend_inputfield
    }

    const string pathName = "Menu/Friend";
    const string contentPath = "UI/ScrollContents/";

    Text friendOnlyTxt;
    Text requestTxt;
    Text friendTxt;
    InputField friendInputField;

    GameObject content = null;
    GameObject requestContent = null;
    GameObject friendContent = null;
    int requestCount = 0;
    int friendCount = 0;
    bool onSearch = false;
    public string Name { get; private set; }

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Text>(typeof(Texts));
        Bind<InputField>(typeof(InputFields));

        friendOnlyTxt = GetText((int)Texts.FriendOnly_txt);
        requestTxt = GetText((int)Texts.Request_txt);
        friendTxt = GetText((int)Texts.Friend_txt);

        friendInputField = GetInputfiled((int)InputFields.Friend_inputfield);

        if (content == null)
        {
            content = GameObject.Find("FriendContent");
        }
        if (requestContent == null)
        {
            requestContent = GameObject.Find("RequestContents");
        }
        if (friendContent == null)
        {
            friendContent = GameObject.Find("FriendContents");
        }

        SetContents();

        RelocationAll();
    }

    private void SetContents()
    {
        foreach (var tmp in dataContainer.friendList)
        {
            AddFriend(tmp.nickName, (int)tmp.friendId);
        }
        foreach (var tmp in dataContainer.waitFriendList)
        {
            AddRequest(tmp.nickName, (int)tmp.friendId);
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Search_btn, (data) => {
            Name = friendInputField.text;
            if (string.IsNullOrWhiteSpace(Name) == false)
            {
                if (onSearch) return;
                onSearch = true;

                SearchFriend();
            }
        });
    }

    void SearchFriend()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                                "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                                Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/users/keyword=" + Name, "GET", null, (uwr) => {
            Response<ResponseSearchFriend> response = JsonUtility.FromJson<Response<ResponseSearchFriend>>(uwr.downloadHandler.text);

            if (response.isSuccess)
            {
                var friend = Managers.UI.ShowPopupUI<UI_AddFriend>("AddFriendView", pathName);
                friend.id = (int)response.result.userId;
                friend.SetLevel(response.result.planetLevel);
                onSearch = false;
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(SearchFriend);
            }
            else
            {
                Debug.Log(response.message);
                onSearch = false;
            }
        }, hN, hV);
    }

    private void Start()
    {
        Init();
    }

    public bool AddFriend(string name, int id)
    {
        if(friendCount < 100)
        {
            GameObject target = Managers.Resource.Instantiate(contentPath + "FriendContent");
            target.transform.SetParent(friendContent.transform);
            target.transform.localScale = new Vector3(1, 1, 1);

            FriendContent tmp = target.GetComponent<FriendContent>();
            tmp.SetParent(this.gameObject);
            tmp.SetName(name);
            tmp.SetID(id);

            friendCount++;
            RelocationAll();
            return true;
        }
        return false;
    }

    public void DeleteFriend(GameObject target)
    {
        friendCount--;
        target.SetActive(false);
        Destroy(target);
        RelocationAll();
    }

    void AddRequest(string name, int id)
    {
        GameObject target = Managers.Resource.Instantiate(contentPath + "RequestContent");
        target.transform.SetParent(requestContent.transform);
        target.transform.localScale = new Vector3(1, 1, 1);

        RequestContent tmp = target.GetComponent<RequestContent>();
        tmp.SetParent(this.gameObject);
        tmp.SetName(name);
        tmp.SetId(id);

        requestCount++;
        RelocationAll();
    }

    public void DeleteRequest(GameObject target)
    {
        requestCount--;
        target.SetActive(false);
        Destroy(target);
        RelocationAll();
    }

    void RelocationAll()
    {
        if(requestCount <= 0)
        {
            requestCount = 0;
            activeChange(false);
        } 
        else
        {
            activeChange(true);
        }
        requestTxt.text = $"      模备 夸没 [{requestCount}]";
        friendTxt.text = $"      模备 [{friendCount}/100]";
        friendOnlyTxt.text = $"      模备 [{friendCount}/100]";
    }

    void activeChange(bool toggle)
    {
        friendOnlyTxt.gameObject.SetActive(!toggle);
        requestTxt.gameObject.SetActive(toggle);
        friendTxt.gameObject.SetActive(toggle);

        ContentSizeFitter fitter = content.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);

        fitter = requestContent.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);

        fitter = friendContent.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
    }
}
