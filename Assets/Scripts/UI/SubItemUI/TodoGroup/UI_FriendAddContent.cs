using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendAddContent : UI_Base
{

    enum GameObjects
    {
        background
    }

    enum Buttons 
    {
        friend_btn,
    }

    enum Texts
    {
        friend_name,
    }

    enum Images
    {
        friend_profile_img
    }

    long userId;
    string nickname;
    string profileColor;

    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    Image profile;
    Text nicknameTxt;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        Button friendBtn = GetButton((int)Buttons.friend_btn);
        BindEvent(friendBtn.gameObject, OnAddClick, Define.TouchEvent.Touch);

        nicknameTxt = GetText((int)Texts.friend_name);
        nicknameTxt.text = nickname;

        profile = GetImage((int)Images.friend_profile_img);

        SetImage(profileColor);

    }

    void Start()
    {
        Init();        
    }

    private void OnAddClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        Debug.Log("Friend Add Click");
        if (Managers.Todo.goalFriendAddAction != null)
        {
            ResponseMemberFind val = new ResponseMemberFind
            {
                userId = this.userId,
                nickname = this.nickname,
                profileColor = this.profileColor
            };

            //Managers.Todo.goalFriendAddAction.Invoke(val);
            this.transform.parent.parent.parent.parent.GetComponent<UI_GoalCreate>().AddfriendList(val);
        }
    }

    public void Setting(long userId, string nickname, string profileColor)
    {
        this.userId = userId;
        this.nickname = nickname;
        this.profileColor = profileColor;
    }

    public void SetImage(string color)
    {
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profile.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

}
