using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_AddFriend : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
    enum Buttons
    {
        Blind_btn,
        Accept_btn,
        Cancel_btn,
    }
    enum Texts
    {
        FriendName_txt,
        FriendLevel_txt,
    }
    enum Images
    {
        FriendProfile_image,
    }
    // ================================ //

    Text friendNameTxt, friendLevelTxt;     // ģ�� �̸�, ģ�� ���� ������Ʈ
    Image profileImage;                     // ������ �̹��� ������Ʈ
    GameObject parent;                      // �θ� ������Ʈ (FriendView)
    UI_Friend friend;                       // �θ� ��ũ��Ʈ (UI_Friend)

    int level = 1;                          // ģ�� �༺ ����
    bool clicked = false;                   // �� ��� �� Ŭ���� �ᱸ�� ���� ����

    public int id { private get; set; }     // ģ���� userId��

    const string profileName = "Art/UI/Profile/Profile_Color_3x"; // ������ �̹��� ���

    public override void Init()             // �ʱ�ȭ �Լ�
    {
        base.Init();

        CameraSet();                        // ī�޶� ����(���)

        parent = GameObject.Find("FriendView(Clone)"); // �θ� ������Ʈ ����
        if(parent != null)
        {
            friend = parent.GetComponent<UI_Friend>(); // �θ� ��ũ��Ʈ ����
        } else
        {
            Debug.Log("parent�� NULL�Դϴ�. UI_AddFriend");
        }

        SetBtns();  // ��ư ������Ʈ ���ε� �� �̺�Ʈ �Ҵ�

        Bind<Text>(typeof(Texts));  // �ؽ�Ʈ ������Ʈ ���ε�

        friendNameTxt = GetText((int)Texts.FriendName_txt);
        friendNameTxt.text = friend.Name;   // �̸� �ؽ�Ʈ �ʱ�ȭ

        friendLevelTxt = GetText((int)Texts.FriendLevel_txt);
        SetLevel(level);    // ���� �ؽ�Ʈ �ʱ�ȭ
    }

    public void SetLevel(int level) // level >> �ؽ�Ʈ�� �� ���� �� || ���� �ؽ�Ʈ�� �ٲ�
    {
        if (friendLevelTxt != null)
            friendLevelTxt.text = "Lv. " + level.ToString();
        else
            this.level = level;
    }

    public void SetImage(string color)  // color >> UI_Color.Color enum���� string || ������ �̹��� �ʱ�ȭ
    {
        Bind<Image>(typeof(Images));    // �̹��� ���ε�
        profileImage = GetImage((int)Images.FriendProfile_image);   // ������Ʈ ���ε�
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));  // string ���� �ٽ� enum���� ���� �� enum�� int������ ����
        profileImage.sprite = Resources.LoadAll<Sprite>(profileName)[index];    // �ش� index�� �̹����� ����
    }

    void SetBtns()  // ��ư ���ε� �� �̺�Ʈ �Ҵ�
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Blind_btn, ClosePopupUI);   // �� ��� ���� �� �˾� ����

        SetBtn((int)Buttons.Accept_btn, (data) => {     // ���� ��ư ���� �� ���� �̺�Ʈ
            CheckFriend();  // ���� �̺�Ʈ
        });

        SetBtn((int)Buttons.Cancel_btn, ClosePopupUI);  // ��� ��ư ���� �� �˾� ����
    }

    void CheckFriend()  // ���� �̺�Ʈ
    {
        if (clicked) return;    // �� ��� ���

        clicked = true;         // �� ��� ���

        // ��� �� ����
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };
        // ============ //

        // �� ��� �ڵ�
        Managers.Web.SendUniRequest("api/friends/" + id, "POST", null, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text); // ��ſ��� �޾ƿ� Json �����͸� ������Ʈȭ
            if (response.isSuccess) // ���� �ڵ� ��ȯ��
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/FriendFadeView"));   // �佺Ʈ �˸� ����
                Managers.Sound.PlayPopupSound(); // �˸� ���� ����
                Managers.UI.ClosePopupUI(); // �˾� ����
                clicked = false;            // �� ��� �Ϸ�
            }
            else if (response.code == 6000) // ��ū ��߱� �ڵ�
            {
                clicked = false;            // ��ū ��߱� �� �� ����� ���� �ʱ�ȭ
                Managers.Player.SendTokenRequest(CheckFriend);  // ��ū ��߱�
            }
            else if (response.code == 5041) // �̹� ��û�� ģ���� ��
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/WaitingFriendView"));   // �佺Ʈ �˸� ����
                Managers.Sound.PlayPopupSound(); // �˸� ���� ����
                Managers.UI.ClosePopupUI(); // �˾� ����
                clicked = false;            // �� ��� �Ϸ�
            }
            else // �̿��� ���� �ڵ� ��ȯ��
            {
                Debug.Log(response.message); // ���� �޼��� ��ȯ
                Managers.UI.ClosePopupUI();  // �˾� ����
                clicked = false;             // �� ��� �Ϸ�
            }
        }, hN, hV);
    }

    private void Start()
    {
        Init();
    }
}