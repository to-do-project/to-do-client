using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LikeFriendContent : UI_Base
{

    enum Texts
    {
        friendname_txt,
    }

    enum Images
    {
        profile_img,
    }

    string nickname, color;
    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    Image profile;
    Text friendName;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        friendName = GetText((int)Texts.friendname_txt);
        profile = GetImage((int)Images.profile_img);

        Setting();

    }

    void Start()
    {
        Init();
    }

    public void Setting(string name, string color)
    {
        nickname = name;
        this.color = color;
    }

    private void Setting()
    {
        friendName.text = nickname;

        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profile.sprite = Resources.LoadAll<Sprite>(profileName)[index];

    }
}
