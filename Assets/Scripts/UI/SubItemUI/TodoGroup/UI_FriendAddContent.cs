using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FriendAddContent : UI_Base
{

    long userId;
    string nickname;
    string profileColor;

    public override void Init()
    {

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
