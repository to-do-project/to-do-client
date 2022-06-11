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
    bool check = false;
    GameObject parent;

    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public void SetImage(UI_Color.Colors color)
    {
        Debug.Log(color.ToString());
    }

    public void SetID(long id)
    {
        this.id = id;
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
            Debug.Log("ģ�� �༺ ����� >> " + name + " id >> " + id);

            string[] hN = { Define.JWT_ACCESS_TOKEN,
                            "User-Id" };
            string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                            Managers.Player.GetString(Define.USER_ID) };

            Managers.Web.SendUniRequest("api/planet/main/" + id, "GET", null, (uwr) =>
            {
                var response = JsonUtility.FromJson<Response<ResponseMainPlanet>>(uwr.downloadHandler.text);

                if (response.code == 1000)
                {
                    GameObject tmp = Managers.Resource.Instantiate("UI/Popup/Menu/Friend/FriendMainView");
                    tmp.GetComponent<UI_FriendMain>().InitView(response);
                }
                else if(response.code == 6000)
                {
                    Managers.Player.SendTokenRequest(() =>
                    {
                        string[] hN = { Define.JWT_ACCESS_TOKEN,
                                        "User-Id" };
                        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                                        Managers.Player.GetString(Define.USER_ID) };

                        Managers.Web.SendUniRequest("api/planet/main/" + id, "GET", null, (uwr) =>
                        {
                            var response = JsonUtility.FromJson<Response<ResponseMainPlanet>>(uwr.downloadHandler.text);

                            if (response.code == 1000)
                            {
                                GameObject tmp = Managers.Resource.Instantiate("UI/Popup/Menu/Friend/FriendMainView");
                                tmp.GetComponent<UI_FriendMain>().InitView(response);
                            }
                            else
                            {
                                Debug.Log("��ū �߱� ����");
                            }
                        }, hN, hV);
                    });
                }
                else
                {
                    Debug.Log(response.message);
                }
            }, hN, hV);
        });
    }
}
