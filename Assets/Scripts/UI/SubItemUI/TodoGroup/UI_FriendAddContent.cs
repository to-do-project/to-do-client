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

        GameObject background = Get<GameObject>((int)GameObjects.background);
        BindEvent(background, OnAddClick, Define.TouchEvent.Touch);

        Text nicknameTxt = GetText((int)Texts.friend_name);
        nicknameTxt.text = nickname;


    }

    void Start()
    {
        Init();        
    }

    private void OnAddClick(PointerEventData data)
    {
        Debug.Log("Friend Add Click");
        if (Managers.Todo.goalFriendAddAction != null)
        {
            ResponseMemberFind val = new ResponseMemberFind
            {
                userId = this.userId,
                nickname = this.nickname,
                profileColor = this.profileColor
            };

            Managers.Todo.goalFriendAddAction.Invoke(val);
        }
    }

    public void Setting(long userId, string nickname, string profileColor)
    {
        this.userId = userId;
        this.nickname = nickname;
        this.profileColor = profileColor;
    }


}
