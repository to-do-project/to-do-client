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
        Test_btn,
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

    string pathName = "Menu/Friend";

    Text friendOnlyTxt;
    Text requestTxt;
    Text friendTxt;
    InputField friendInputField;

    GameObject content = null;
    GameObject requestContent = null;
    GameObject friendContent = null;
    int requestCount = 0;
    int friendCount = 0;
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
            content = GameObject.Find("Content");
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

    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Test_btn, (data) => { AddRequest($"�׽�Ʈ {requestCount}"); });

        SetBtn((int)Buttons.Search_btn, (data) => {
            Name = friendInputField.text;
            if (string.IsNullOrWhiteSpace(Name) == false)
            {
                Managers.UI.ShowPopupUI<UI_AddFriend>("AddFriendView", pathName);
            }
        });
    }

    private void Start()
    {
        Init();
    }

    public bool AddFriend(string name)
    {
        if(friendCount < 100)
        {
            GameObject target = Managers.Resource.Instantiate("UI/ScrollContents/FriendContent");
            target.transform.SetParent(friendContent.transform);
            target.transform.localScale = new Vector3(1, 1, 1);

            FriendContent tmp = target.GetComponent<FriendContent>();
            tmp.SetParent(this.gameObject);
            tmp.SetName(name);

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

    void AddRequest(string name)
    {
        GameObject target = Managers.Resource.Instantiate("UI/ScrollContents/RequestContent");
        target.transform.SetParent(requestContent.transform);
        target.transform.localScale = new Vector3(1, 1, 1);

        RequestContent tmp = target.GetComponent<RequestContent>();
        tmp.SetParent(this.gameObject);
        tmp.SetName(name);

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
        requestTxt.text = $"      ģ�� ��û [{requestCount}]";
        friendTxt.text = $"      ģ�� [{friendCount}/100]";
        friendOnlyTxt.text = $"      ģ�� [{friendCount}/100]";
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
