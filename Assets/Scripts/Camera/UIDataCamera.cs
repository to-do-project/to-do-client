using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataCamera : MonoBehaviour
{
    public List<ResponseAnnounceList> announceList = null;
    public List<long> charBtnId = null;
    public List<long> planetBtnId = null;
    public List<long> invenIdList = null;
    public List<ResponseFriendList> friendList;
    public List<ResponseFriendList> waitFriendList;
    public ResponsePush pushLists;

    public void DataInit()
    {
        RefreshAnnounceData();
        RefreshItemStoreData();
        RefreshDecoData();
        RefreshFriendData();
        RefreshPushData();
    }

    public void RefreshAnnounceData()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/notices", "GET", null, (uwr) => {
            Response<ResponseAnnounce> response = JsonUtility.FromJson<Response<ResponseAnnounce>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                announceList = response.result.noticeList;
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(RefreshAnnounceData);
            } 
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    public void RefreshItemStoreData()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/store", "GET", null, (uwr) => {
            Response<ResponseStoreItemsId> response = JsonUtility.FromJson<Response<ResponseStoreItemsId>>(uwr.downloadHandler.text);
            if (response.code == 1000)
            {
                charBtnId = response.result.characterItemIdList;
                planetBtnId = response.result.planetItemIdList;
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(RefreshItemStoreData);
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    public void RefreshDecoData()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/closet/character-items", "GET", null, (uwr) => {
            Response<ResponseCloset> response = JsonUtility.FromJson<Response<ResponseCloset>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                invenIdList = response.result.characterItemIdList;
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(RefreshDecoData);
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    public void RefreshFriendData()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/friends", "GET", null, (uwr) => {
            Response<ResponseFriend> response = JsonUtility.FromJson<Response<ResponseFriend>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                friendList = response.result.friends;
                waitFriendList = response.result.waitFriends;

                ResponseFriendList friends = new ResponseFriendList();
                friends.friendId = 1;
                friends.nickName = "nick";
                friends.profileColor = UI_Color.Colors.Blue.ToString();
                friends.userId = 33;
                friends.waitFlag = false;

                ResponseFriendList friend2 = new ResponseFriendList();
                friend2.friendId = 2;
                friend2.nickName = "name";
                friend2.profileColor = UI_Color.Colors.LightRed.ToString();
                friend2.userId = 33;
                friend2.waitFlag = true;

                friendList.Add(friends);
                waitFriendList.Add(friend2);

                UI_Load.Instance.CompleteLoad();
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(RefreshFriendData);
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    public void RefreshPushData()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/notifications", "GET", null, (uwr) => {
            Response<ResponsePush> response = JsonUtility.FromJson<Response<ResponsePush>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                pushLists.noticeNotifications = response.result.noticeNotifications;
                pushLists.friendReqNotifications = response.result.friendReqNotifications;
                pushLists.groupReqNotifications = response.result.groupReqNotifications;
                pushLists.etcNotifications = response.result.etcNotifications;

                //테스트용

                ResponsePushNotice notice = new ResponsePushNotice();
                notice.notificationId = 1;
                notice.userId = 1;
                notice.category = UI_Signal.SignalType.NOTICE_TWO.ToString();
                notice.createAt = "2022-06-12 11:55:21";
                notice.readStatus = "NOT_READ";
                notice.content = "[공지] 첫번째 공지사항";
                pushLists.noticeNotifications.Add(notice);

                ResponsePushFriend friend = new ResponsePushFriend();
                friend.notificationId = 2;
                friend.userId = 2;
                friend.category = UI_Signal.SignalType.FRIEND_REQUEST.ToString();
                friend.createAt = "2022-06-12 11:55:30";
                friend.readStatus = "NOT_READ";
                friend.content = "플래닛good3님에게 친구 초대!!";
                friend.friendId = 10;
                friend.confirmStatus = "NOT_CONFIRM";
                pushLists.friendReqNotifications.Add(friend);

                ResponsePushGroup group = new ResponsePushGroup();
                group.notificationId = 3;
                group.userId = 3;
                group.category = UI_Signal.SignalType.GROUP_REQUEST.ToString();
                group.createAt = "2022-06-12 11:55:40";
                group.readStatus = "NOT_READ";
                group.content = "플래닛good3님을 그룹 목표에 초대함!!";
                group.goalId = 1;
                group.confirmStatus = "NOT_CONFIRM";
                pushLists.groupReqNotifications.Add(group);

                ResponsePushNotice etc = new ResponsePushNotice();
                etc.notificationId = 4;
                etc.userId = 4;
                etc.category = UI_Signal.SignalType.PRIVATE_FAVORITE.ToString();
                etc.createAt = "2022-06-12 11:56:21";
                etc.readStatus = "NOT_READ";
                etc.content = "개인 투두 좋아요";
                pushLists.etcNotifications.Add(etc);
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(RefreshPushData);
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    void Start()
    {
        DataInit();
    }
}
