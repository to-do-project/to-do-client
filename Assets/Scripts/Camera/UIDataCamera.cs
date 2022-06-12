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

    void DataInit()
    {
        RefreshAnnounceData();
        RefreshItemStoreData();
        RefreshDecoData();
        RefreshFriendData();
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

    void Start()
    {
        DataInit();
    }
}
