using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataCamera : MonoBehaviour
{
    public List<ResponseAnnounceList> announceList = null;
    public List<long> charBtnId = null;
    public List<long> planetBtnId = null;
    public List<long> invenIdList = null;
    public List<ResponseFriendList> friendList = null, waitFriendList = null;
    public ResponsePush pushLists = null;
    public List<ResponseGoalList> goalList = null;
    public ResponseSetting settings = null;

    public bool friendCheck = false;

    public void DataInit()
    {
        if (PlayerPrefs.HasKey(Define.DEVICETOKEN) == false)
            Managers.Player.SetString(Define.DEVICETOKEN, SystemInfo.deviceUniqueIdentifier);
        RefreshAnnounceData();
        RefreshItemStoreData();
        RefreshDecoData();
        RefreshFriendData();
        RefreshPushData();
        RefreshGoalData();
        RefreshSettingData();
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
        friendCheck = true;
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
                friendCheck = false;

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

    public void RefreshGoalData()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/goals/archive", "GET", null, (uwr) => {
            Response<List<ResponseGoalList>> response = JsonUtility.FromJson<Response<List<ResponseGoalList>>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                goalList = response.result;
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(RefreshGoalData);
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    public void RefreshSettingData()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id",
                        "Device-Token"};
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID),
                        Managers.Player.GetString(Define.DEVICETOKEN)};

        Managers.Web.SendUniRequest("api/alarms", "GET", null, (uwr) => {
            Response<ResponseSetting> response = JsonUtility.FromJson<Response<ResponseSetting>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                settings = response.result;
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(RefreshSettingData);
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
