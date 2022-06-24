using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_SignalContent : UI_Base, IPointerClickHandler
{
    enum Texts
    {
        Text,
    }

    Text title;
    string type;
    long noticeId, id;
    bool clicked = false;
    bool isRead = false;

    //GameObject parent;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        title = GetText((int)Texts.Text);
    }

    public void SetTitle(string text)
    {
        title.text = text;
    }

    public void SetType(string type)
    {
        this.type = type;
    }

    public void SetNoticeId(long id)
    {
        noticeId = id;
    }

    public void SetId(long id)
    {
        this.id = id;
    }

    public void SetRead()
    {
        isRead = true;
    }

    public void OnPointerClick(PointerEventData data)
    {
        ReadSave();
    }

    void ReadSave()
    {
        if (clicked) return;
        clicked = true;

        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/notifications/read/" + noticeId, "PATCH", null, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                if (type == UI_Signal.SignalType.NOTICE_TWO.ToString())
                {
                    Managers.UI.ShowPopupUI<UI_Announce>("AnnounceView", "Menu/Setting").ExpandContent(noticeId);
                }
                else if (type == UI_Signal.SignalType.FRIEND_REQUEST.ToString())
                {
                    Managers.UI.ShowPopupUI<UI_Friend>("FriendView", "Menu/Friend");
                }
                else if (type == UI_Signal.SignalType.GROUP_REQUEST.ToString())
                {
                    WebGetGroup();
                }

                if (isRead == false)
                {
                    if (type == UI_Signal.SignalType.GROUP_REQUEST.ToString())
                    {
                        Managers.UI.ClosePopupUI();
                    }
                }
                isRead = true;
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(ReadSave);
            }
            else
            {
                Debug.Log(response.message);
            }
            clicked = false;
        }, hN, hV);
    }

    void WebGetGroup()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/goals/accept/" + id, "GET", null, (uwr) => {
            Response<ResponseGoalRequest> response = JsonUtility.FromJson<Response<ResponseGoalRequest>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                UI_GroupGoal tmp = Managers.UI.ShowPopupUI<UI_GroupGoal>("GroupGoalView", "Menu/Signal");
                tmp.Setid(id);
                tmp.SetText(response.result.title);
                tmp.Members = response.result.goalMemberList;
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(WebGetGroup);
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    /*public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public void SizeRefresh()
    {
        ContentSizeFitter fitter = gameObject.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
    }*/
}
