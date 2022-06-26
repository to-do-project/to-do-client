using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ResponseLikeList
{
    public int likeCount;
    public List<UserInfo> userInfo;
}

[System.Serializable]
public class UserInfo
{
    public long userId;
    public string nickname;
    public string profileColor;
}

public class UI_Like : UI_Popup
{
    enum GameObjects
    {
        Content,
    }

    enum Texts
    {
        likenum2_txt,
    }

    string todoMemberId;

    GameObject root;
    Text likenum;

    public override void Init()
    {
        base.Init();

        Canvas canvas = GetComponent<Canvas>();
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }

        else
        {
            Debug.Log($"{UIcam.name}");
        }

        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));

        root = Get<GameObject>((int)GameObjects.Content);
        likenum = GetText((int)Texts.likenum2_txt);


    }

    private void Start()
    {
        Init();
    }

    public void Setting(string todoMemberId)
    {
        this.todoMemberId = todoMemberId;

        Managers.Web.SendGetRequest("api/todo/like/", todoMemberId, (uwr) => {
            Response<ResponseLikeList> res = JsonUtility.FromJson<Response<ResponseLikeList>>(uwr.downloadHandler.text);

            if (res.isSuccess)
            {
                likenum.text = res.result.userInfo.Count.ToString();

                foreach (UserInfo item in res.result.userInfo)
                {
                    UI_LikeFriendContent friend = Managers.UI.MakeSubItem<UI_LikeFriendContent>("GoalList", root.transform, "LikeFriend_content");
                    friend.Setting(item.nickname, item.profileColor);
                }
            }
            else
            {
                switch (res.code)
                {
                    case 5039:
                        likenum.text = "0";

                        break;
                    case 6023:
                        break;
                }
            }
        }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());
    }
}
