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

    string name, image;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        Text friendName = GetText((int)Texts.friendname_txt);
        Image profile = GetImage((int)Images.profile_img);

        friendName.text = name;


    }

    void Start()
    {
        Init();
    }

    public void Setting(string name, string image)
    {
        this.name = name;
        this.image = image;
    }

}
