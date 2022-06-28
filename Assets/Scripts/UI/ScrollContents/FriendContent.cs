using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendContent : UI_PopupMenu
{
    enum Buttons
    {
        FriendPlanet_btn,
    }

    [SerializeField]
    private Image profile;

    [SerializeField]
    private Text nameTxt;

    long id;
    long userId = 0;
    bool check = false;
    GameObject parent;
    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public void SetImage(string color)
    {
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profile.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    public void SetID(long id)
    {
        this.id = id;
    }

    public void SetUserID(long id)
    {
        userId = id;
    }

    public void SetName(string name)
    {
        nameTxt.text = name;
    }

    public void DeleteFriend()
    {
        parent.GetComponent<UI_Friend>().DeleteFriend(this.gameObject);
    }

    void Start()
    {
        Bind<Button>(typeof(Buttons));
        SetBtn((int)Buttons.FriendPlanet_btn, (data) => {
            if (check) return;
            check = true;
            Debug.Log("친구 행성 놀러가기 >> " + nameTxt.text + " id >> " + userId + " userID >> " + Managers.Player.GetString(Define.USER_ID));

            string[] hN = { Define.JWT_ACCESS_TOKEN,
                            "User-Id" };
            string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                            Managers.Player.GetString(Define.USER_ID) };

            Managers.Web.SendUniRequest("api/planet/main/" + userId, "GET", null, (uwr) =>
            {
                var response = JsonUtility.FromJson<Response<ResponseMainPlanet>>(uwr.downloadHandler.text);

                if (response.code == 1000)
                {
                    Managers.UI.DeactiveAllUI();
                    Managers.Player.GetPlanet().SetActive(false);
                    GameObject tmp = Managers.Resource.Instantiate("UI/Popup/Menu/Friend/FriendMainView");
                    tmp.GetComponent<UI_FriendMain>().InitView(response.result);
                    var sec = Managers.Resource.Instantiate("UI/Popup/Menu/Friend/FriendUIView").GetComponent<UI_FriendUI>();
                    sec.SetUserId(userId);
                    sec.nickname = nameTxt.text;
                    check = false;
                }
                else if(response.code == 6000)
                {
                    Managers.Player.SendTokenRequest(() =>
                    {
                        string[] hN = { Define.JWT_ACCESS_TOKEN,
                                        "User-Id" };
                        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                                        Managers.Player.GetString(Define.USER_ID) };

                        Managers.Web.SendUniRequest("api/planet/main/" + userId, "GET", null, (uwr) =>
                        {
                            var response = JsonUtility.FromJson<Response<ResponseMainPlanet>>(uwr.downloadHandler.text);

                            if (response.code == 1000)
                            {
                                Managers.UI.DeactiveAllUI();
                                GameObject tmp = Managers.Resource.Instantiate("UI/Popup/Menu/Friend/FriendMainView");
                                tmp.GetComponent<UI_FriendMain>().InitView(response.result);
                            }
                            else
                            {
                                Debug.Log("토큰 발급 실패");
                            }
                            check = false;
                        }, hN, hV);
                    });
                }
                else
                {
                    Debug.Log(response.message);
                    check = false;
                }
            }, hN, hV);
        });
    }
}
