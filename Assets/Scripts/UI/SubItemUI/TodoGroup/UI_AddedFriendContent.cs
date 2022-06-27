using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AddedFriendContent : UI_Base
{
    enum GameObjects
    {
        background
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
        Bind<Image>(typeof(Images));


        nicknameTxt = GetText((int)Texts.friend_name);
        nicknameTxt.text = nickname;

        profile = GetImage((int)Images.friend_profile_img);

        SetImage(profileColor);
    }

    void Start()
    {
        Init();
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
