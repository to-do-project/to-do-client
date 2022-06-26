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

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));


        Text nicknameTxt = GetText((int)Texts.friend_name);
        nicknameTxt.text = nickname;
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
}
