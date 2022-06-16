using System.Collections;
using System.Collections.Generic;
using System;

public class MenuData
{
    //Menu에서 사용할 데이터들 양식 정리
}

[Serializable]
public class RequestHeaderBase
{
    public List<string> headerName;
    public List<string> headerValue;
}

[Serializable]
public class RequestLogout
{
    public string deviceToken;
}

[Serializable]
public class RequestDelete
{
    public string password;
}

[Serializable]
public class RequestProfileColor
{
    public string profileColor;
}

[Serializable]
public class RequestNickChange
{
    public string nickname;
}

[Serializable]
public class RequestPswdChange
{
    public string oldPassword;
    public string newPassword;
}

[Serializable]
public class RequestBuyItem
{
    public long itemId;
    public int count;
    public int totalPrice;
}

[Serializable]
public class RequestTokenRefresh
{
    public string deviceToken;
}

// -------- 위 Request -------- 아래 Response ---------

[Serializable]
public class ResponseStoreItemsId
{
    public string nickname;
    public int point;
    public List<long> characterItemIdList;
    public List<long> planetItemIdList;
}

[Serializable]
public class ResponseItemDetail
{
    public long itemId;
    public string name;
    public string description;
    public int price;
    public string type;
    public int minCnt;
    public int maxCnt;
}

[Serializable]
public class ResponseBuyItem
{
    public long itemId;
    public int minCnt;
    public int maxCnt;
    public int point;
}

[Serializable]
public class ResponseCloset
{
    public List<long> characterItemIdList;
}

[Serializable]
public class ResponsePlanetInfo
{
    public long age;
    public int level;
    public int point;
    public int avgGoalCompleteRate;
    public int getFavoriteCount;
    public int putFavoriteCount;
}

[Serializable]
public class ResponseAnnounce
{
    public int totalNoticeCnt;
    public List<ResponseAnnounceList> noticeList;
}

[Serializable]
public class ResponseAnnounceList
{
    public long noticeId;
    public string title;
    public string content;
    public string createAt;
}

[Serializable]
public class ResponseFriend
{
    public List<ResponseFriendList> waitFriends;
    public List<ResponseFriendList> friends;
}

[Serializable]
public class ResponseFriendList
{
    public long friendId;
    public long userId;
    public string nickName;
    public string profileColor;
    public bool waitFlag;
}

[Serializable]
public class ResponseSearchFriend
{
    public long userId;
    public string nickname;
    public string profileColor;
    public int planetLevel;
}

[Serializable]
public class ResponsePush
{
    public List<ResponsePushNotice> noticeNotifications;
    public List<ResponsePushFriend> friendReqNotifications;
    public List<ResponsePushGroup> groupReqNotifications;
    public List<ResponsePushNotice> etcNotifications;
}

[Serializable]
public class ResponsePushNotice
{
    public long notificationId;
    public long userId;
    public string category;
    public string content;
    public string createAt;
    public string readStatus;
}

[Serializable]
public class ResponsePushFriend : ResponsePushNotice
{
    public long friendId;
    public string confirmStatus;
}

[Serializable]
public class ResponsePushGroup : ResponsePushNotice
{
    public long goalId;
    public string confirmStatus;
}