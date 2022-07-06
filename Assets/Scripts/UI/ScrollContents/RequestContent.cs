using UnityEngine;
using UnityEngine.UI;

public class RequestContent : MonoBehaviour
{
    [SerializeField]
    private Image profile;

    [SerializeField]
    private Text nameTxt;

    int id;
    long userId;
    const string profileName = "Art/UI/Profile/Profile_Color_3x";
    string color;
    bool clicked = false;

    GameObject parent;
    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public void SetImage(UI_Color.Colors color)
    {
        Debug.Log(color.ToString());
    }

    public void SetName(string name)
    {
        nameTxt.text = name;
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public void SetUserID(long id)
    {
        this.userId = id;
    }

    public void SetImage(string color)
    {
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profile.sprite = Resources.LoadAll<Sprite>(profileName)[index];
        this.color = color;
    }

    public void AcceptRequest()
    {
        if (clicked) return;

        clicked = true;

        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        RequestFriendAccept req = new RequestFriendAccept();
        req.friendId = id;
        req.accepted = true;

        Debug.Log(id);

        Managers.Web.SendUniRequest("api/friends", "PATCH", req, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                parent.GetComponent<UI_Friend>().AddFriend(nameTxt.text, id, color, userId);
                DeleteContent();
                clicked = false;
            }
            else if (response.code == 6000)
            {
                clicked = false;
                Managers.Player.SendTokenRequest(AcceptRequest);
            }
            else
            {
                Debug.Log(response.message);
                clicked = false;
            }
        }, hN, hV);
    }

    public void DeleteRequest()
    {
        if (clicked) return;

        clicked = true;

        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        RequestFriendAccept req = new RequestFriendAccept();
        req.friendId = id;
        req.accepted = false;

        Managers.Web.SendUniRequest("api/friends", "PATCH", req, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                DeleteContent();
                clicked = false;
            }
            else if (response.code == 6000)
            {
                clicked = false;
                Managers.Player.SendTokenRequest(AcceptRequest);
            }
            else
            {
                Debug.Log(response.message);
                clicked = false;
            }
        }, hN, hV);
    }

    public void DeleteContent()
    {
        parent.GetComponent<UI_Friend>().DeleteRequest(this.gameObject);
    }
}
