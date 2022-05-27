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