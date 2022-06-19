using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_AddFriend : UI_PopupMenu
{
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

    Text friendNameTxt, friendLevelTxt;
    Image profileImage;
    GameObject parent;
    UI_Friend friend;
    int level = 1;
    bool clicked = false;
    public int id { private get; set; }
    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    public override void Init()
    {
        base.Init();

        CameraSet();

        parent = GameObject.Find("FriendView(Clone)");
        if(parent != null)
        {
            friend = parent.GetComponent<UI_Friend>();
        } else
        {
            Debug.Log("parent가 NULL입니다. UI_AddFriend");
        }

        SetBtns();

        Bind<Text>(typeof(Texts));

        friendNameTxt = GetText((int)Texts.FriendName_txt);
        friendNameTxt.text = friend.Name;

        friendLevelTxt = GetText((int)Texts.FriendLevel_txt);
        SetLevel(level);
    }

    public void SetLevel(int level)
    {
        if (friendLevelTxt != null)
            friendLevelTxt.text = "Lv. " + level.ToString();
        else
            this.level = level;
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Blind_btn, ClosePopupUI);

        SetBtn((int)Buttons.Accept_btn, (data) => {
            CheckFriend();
        });

        SetBtn((int)Buttons.Cancel_btn, ClosePopupUI);
    }

    void CheckFriend()
    {
        if (clicked) return;

        clicked = true;

        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/friends/" + id, "POST", null, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/FriendFadeView"));
                Managers.UI.ClosePopupUI();
                clicked = false;
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(CheckFriend);
            }
            else
            {
                Debug.Log(response.message);
                Managers.UI.ClosePopupUI();
                clicked = false;
            }
        }, hN, hV);
    }

    public void SetImage(string color)
    {
        Bind<Image>(typeof(Images));
        profileImage = GetImage((int)Images.FriendProfile_image);
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profileImage.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    private void Start()
    {
        Init();
    }
}