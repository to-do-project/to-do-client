using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Friend : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
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
    // ================================ //

    public string Name { get; private set; } // ģ���� ���� ����, �� ��� Parameter���� ���

    GameObject content = null, requestContent = null, friendContent = null; // Rect Layer�� Content�θ� ������Ʈ��
    Text friendOnlyTxt, friendTxt, requestTxt; // �ؽ�Ʈ ������Ʈ
    InputField friendInputField; // ģ���� �Է�â

    int requestCount = 0, friendCount = 0; // ģ�� ��û �� ģ�� ����
    bool onSearch = false; // �� ��� üũ

    // ���
    const string pathName = "Menu/Friend";
    const string contentPath = "UI/ScrollContents/";

    // �ʱ�ȭ
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

        StartCoroutine(SetContents());

        RelocationAll();

        StartCoroutine(PlusCheck());
    }

    // ģ�� ������Ʈ �߰�
    // name = ģ�� �̸�
    // friendId = ģ�� ���� id
    // color = ģ�� ������ �÷�
    // userId = �ڽ��� ���� id
    public bool AddFriend(string name, int friendId, string color, long userId)
    {
        // ģ�� ī��Ʈ�� 100�̻��� ��� ����
        if (friendCount < 100)
        {
            // ģ�� ������ ����
            GameObject target = Managers.Resource.Instantiate(contentPath + "FriendContent");
            target.transform.SetParent(friendContent.transform);
            target.transform.localScale = new Vector3(1, 1, 1);

            // ģ�� ������ �ʱ�ȭ
            FriendContent tmp = target.GetComponent<FriendContent>();
            tmp.SetParent(this.gameObject);
            tmp.SetName(name);
            tmp.SetID(friendId);
            tmp.SetUserID(userId);
            tmp.SetImage(color);

            // ī��Ʈ ���� �� ������ �� ���
            friendCount++;
            dataContainer.RefreshFriendData();
            RelocationAll();
            return true;
        }
        return false;
    }

    // ģ�� ������ ����
    // target = ������ ������Ʈ
    public void DeleteFriend(GameObject target)
    {
        // ģ�� ī��Ʈ ���� �� ������ �� ���
        friendCount--;
        dataContainer.RefreshFriendData();

        // ������Ʈ ����
        target.SetActive(false);
        Destroy(target);
        RelocationAll();
    }

    // ��û ������ ����
    // target = ������ ������Ʈ
    public void DeleteRequest(GameObject target)
    {
        // ��û ī��Ʈ ���� �� ������ �� ���
        dataContainer.RefreshFriendData();
        requestCount--;

        // ������Ʈ ����
        target.SetActive(false);
        Destroy(target);
        RelocationAll();
    }

    // ������ �������� �����͸� ���Ͽ� ������ �������� �߰� �����ϴ� �ڷ�ƾ �Լ�
    IEnumerator PlusCheck()
    {
        // �񱳸� ���� �ӽ� ��ųʸ�
        Dictionary<long, ResponseFriendList> tmpFriend = new Dictionary<long, ResponseFriendList>();
        Dictionary<long, ResponseFriendList> tmpWait = new Dictionary<long, ResponseFriendList>();

        // ������ ������ ����
        foreach (var tmp in dataContainer.friendList)
        {
            if (tmpFriend.ContainsKey(tmp.friendId)) continue;
            tmpFriend.Add(tmp.friendId, tmp);
        }
        foreach (var tmp in dataContainer.waitFriendList)
        {
            if (tmpWait.ContainsKey(tmp.friendId)) continue;
            tmpWait.Add(tmp.friendId, tmp);
        }

        // ģ�� ������ �����
        dataContainer.RefreshFriendData();
        // ������ ����� �Ϸ� �ñ��� ���
        while(dataContainer.friendCheck)
        {
            yield return null;
        }

        // ������ �����Ϳ� ����� �� �����͸� ���Ͽ� �߰��� ������Ʈ ����
        foreach (var tmp in dataContainer.friendList)
        {
            if (tmpFriend.ContainsKey(tmp.friendId)) continue;
            AddFriend(tmp.nickName, (int)tmp.friendId, tmp.profileColor, tmp.userId);
        }
        foreach (var tmp in dataContainer.waitFriendList)
        {
            if (tmpWait.ContainsKey(tmp.friendId)) continue;
            AddRequest(tmp.nickName, (int)tmp.friendId, tmp.profileColor, tmp.userId);
        }

        // ������Ʈ ��ġ ������
        RelocationAll();
    }

    // ������ ���� �ڷ�ƾ �Լ�
    IEnumerator SetContents()
    {
        // ģ�� ����� �ʱ�ȭ ���� �ʾҴٸ� ���
        while (dataContainer.friendCheck)
        {
            yield return null;
        }

        // ģ�� ��� ����Ʈ�� ���� ������Ʈ ����
        foreach (var tmp in dataContainer.friendList)
        {
            AddFriend(tmp.nickName, (int)tmp.friendId, tmp.profileColor, tmp.userId);
        }
        foreach (var tmp in dataContainer.waitFriendList)
        {
            AddRequest(tmp.nickName, (int)tmp.friendId, tmp.profileColor, tmp.userId);
        }
    }

    // ��ư �̺�Ʈ ����
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // �ڷΰ��� ��ư
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // ģ�� Ž�� ��ư
        SetBtn((int)Buttons.Search_btn, (data) => {
            // ģ���� ����
            Name = friendInputField.text;

            // ģ������ ������� ���� ��
            if (string.IsNullOrWhiteSpace(Name) == false)
            {
                if (onSearch) return;
                onSearch = true;

                // ģ�� Ž��
                SearchFriend();
            }
        });
    }

    // ģ�� Ž�� �� ���
    void SearchFriend()
    {
        // ��� ��� ��
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // ģ�� Ž�� �� ���
        Managers.Web.SendUniRequest("api/users?keyword=" + Name, "GET", null, (uwr) => {
            // Json ���� �����͸� ����Ƽ �����ͷ� ��ȯ
            Response<ResponseSearchFriend> response = JsonUtility.FromJson<Response<ResponseSearchFriend>>(uwr.downloadHandler.text);

            // �� ��� ���� ��
            if (response.isSuccess)
            {
                // Debug.Log(uwr.downloadHandler.text);
                // ģ�� ��û �˾� ���� �� �˾� �ʱ�ȭ
                var friend = Managers.UI.ShowPopupUI<UI_AddFriend>("AddFriendView", pathName);
                friend.id = (int)response.result.userId;
                friend.SetLevel(response.result.planetLevel);
                friend.SetImage(response.result.profileColor);

                // ��ư�� ���
                Managers.Sound.PlayNormalButtonClickSound();
                onSearch = false;
            }
            // ��ū ���� ��
            else if (response.code == 6000)
            {
                onSearch = false;
                // ��ū ��߱�
                Managers.Player.SendTokenRequest(SearchFriend);
            }
            // ���� Ǯ�� ��û���� ���� ��
            else if (response.code == 5003 || response.code == 5004 || response.code == 6001)
            {
                // ���� �޼��� �˾� ���� �� ��ư�� ���
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/CantFindFadeView"));
                Managers.Sound.PlayPopupSound();
                onSearch = false;
            }
            // ��Ÿ ���� ��
            else
            {
                Debug.Log(response.message);
                onSearch = false;
            }
        }, hN, hV);
    }

    void Start()
    {
        Init();
    }

    // ��û ������Ʈ �߰�
    // name = ��û ģ�� �̸�
    // friendId = ��û ģ�� ���� id
    // color = ��û ģ�� ������ �÷�
    // userId = �ڽ��� ���� id
    void AddRequest(string name, int friendId, string color, long userId)
    {
        // ��û ������ ����
        GameObject target = Managers.Resource.Instantiate(contentPath + "RequestContent");
        target.transform.SetParent(requestContent.transform);
        target.transform.localScale = new Vector3(1, 1, 1);

        // ��û ������ �ʱ�ȭ
        RequestContent tmp = target.GetComponent<RequestContent>();
        tmp.SetParent(this.gameObject);
        tmp.SetName(name);
        tmp.SetId(friendId);
        tmp.SetUserID(userId);
        tmp.SetImage(color);

        // ��û ����Ʈ ī��Ʈ ����
        requestCount++;
        RelocationAll();
    }

    // ������Ʈ ��ġ ������
    void RelocationAll()
    {
        // ��û ģ���� ���� �� ��û UI ����
        if(requestCount <= 0)
        {
            requestCount = 0;
            activeChange(false);
        } 
        else
        {
            activeChange(true);
        }

        // �ؽ�Ʈ �ʱ�ȭ
        requestTxt.text = $"      ģ�� ��û [{requestCount}]";
        friendTxt.text = $"      ģ�� [{friendCount}/100]";
        friendOnlyTxt.text = $"      ģ�� [{friendCount}/100]";
    }

    // ��û UI on/off
    // toggle = true�� on, false�� off
    void activeChange(bool toggle)
    {
        // ģ�� UI�� �ִ� ���
        friendOnlyTxt.gameObject.SetActive(!toggle);

        // ��û �� ģ�� UI �� �� �ִ� ���
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
