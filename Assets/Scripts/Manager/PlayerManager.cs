using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestArrangement
{
    public List<MainItemList> itemPositionList;
}

public class RequestToken
{
    public string deviceToken;
}

[System.Serializable]
public class ResponseMainPlanet
{
    public long userId;
    public string planetColor;
    public int level;
    public long characterItem;
    public List<MainItemList> planetItemList;
}

[System.Serializable]
public class MainItemList
{
    public string itemCode;
    public List<MainItemPosition> positionList;
}

[System.Serializable]
public class MainItemPosition
{
    public float posX;
    public float posY;
}


public class PlayerManager : MonoBehaviour
{


    //행성 레벨업 등으로 행성 이미지 교체도 여기서 처리
    //투두 캐싱
    //행성 instantiate, 아이템 instantiate



    public Action<UnityWebRequest, Action> tokenCallback;
    Action<UnityWebRequest> mainCallback;
    Action<UnityWebRequest> arrangeCallback;


    Action firstCallback;

    GameObject planet;


    Response<ResponseMainPlanet> Mainres;
    Response<string> Tokenres;
    Response<string> Arrangeres;

    public List<MainItemList> placedItemList;
    public List<GameObject> realPlacedItemList;

    string[] header = new string[2];
    string[] headerValue = new string[2];

    bool isFirst;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        realPlacedItemList = new List<GameObject>();
        //PlayerPrefs.DeleteAll();
        Debug.Log("플레이어 매니저 실행");

        mainCallback -= PlanetResponseAction;
        mainCallback += PlanetResponseAction;

        tokenCallback -= TokenResponseAction;
        tokenCallback += TokenResponseAction;

        firstCallback -= FirstInstantiate;
        firstCallback += FirstInstantiate;

        arrangeCallback -= ArrangementResponseAction;
        arrangeCallback += ArrangementResponseAction;


        //토큰 확인 & 재발급 (자동로그인 상태)
        if (PlayerPrefs.HasKey(Define.JWT_ACCESS_TOKEN) && PlayerPrefs.HasKey(Define.JWT_REFRESH_TOKEN))
        {
            Debug.Log("Token 있음");

            Managers.Scene.LoadScene(Define.Scene.Main);
            SendTokenRequest(firstCallback);
        }
        else
        {
            Debug.Log("No token");
            //토큰 없으면
        }


    }

    
    private void PlanetResponseAction(UnityWebRequest request)
    {
        if (Mainres != null)
        {
            Mainres = JsonUtility.FromJson<Response<ResponseMainPlanet>>(request.downloadHandler.text);

            if (Mainres.isSuccess)
            {
                if (Mainres.code == 1000)
                {
                    Debug.Log("행성 생성");

                    PlanetInstantiate(Mainres.result.planetColor, Mainres.result.level);
                    placedItemList = Mainres.result.planetItemList;
                    ItemInstantiate();
                }
            }

            else
            {

            }
            Mainres = null;
        }

    }

    private void TokenResponseAction(UnityWebRequest request, Action callback)
    {
        
        if (Tokenres != null)
        {
            Tokenres = JsonUtility.FromJson<Response<string>>(request.downloadHandler.text);

            Debug.Log(Tokenres.message);
            if (Tokenres.isSuccess)
            {

                Debug.Log("토큰 확인");
                PlayerPrefs.SetString(Define.JWT_ACCESS_TOKEN, request.GetResponseHeader(Define.JWT_ACCESS_TOKEN));
                PlayerPrefs.SetString(Define.JWT_REFRESH_TOKEN, request.GetResponseHeader(Define.JWT_REFRESH_TOKEN));

                if (callback != null)
                {
                    callback.Invoke();
                }

            }
            else
            {
                Debug.Log(Tokenres.message);
                if (Tokenres.code ==6023)
                {
                    Managers.UI.Clear();
                    Managers.Scene.LoadScene(Define.Scene.Login);
                }
                else if (Tokenres.code == 6028)
                {
                    /*PlayerPrefs.SetString(Define.JWT_ACCESS_TOKEN, request.GetResponseHeader(Define.JWT_ACCESS_TOKEN));
                    PlayerPrefs.SetString(Define.JWT_REFRESH_TOKEN, request.GetResponseHeader(Define.JWT_REFRESH_TOKEN));
                    SendTokenRequest(tokenCallback, firstCallback);*/
                }
            }

            Tokenres = null;
        }

        else
        {
            Debug.Log("Tokenres null");
        }
    }

    private void ArrangementResponseAction(UnityWebRequest request)
    {
        if (Arrangeres != null)
        {
            Debug.Log(Arrangeres.message);
            if (Arrangeres.isSuccess)
            {

                if (Arrangeres.code == 1000)
                {
                    SendSettingPlanetRequest(PlayerPrefs.GetString(Define.USER_ID));
                    Managers.Scene.LoadScene(Define.Scene.Main);
                }
            }

            else
            {
                Debug.Log(Arrangeres.message);
            }

            Arrangeres = null;
        }
    }

    //
    public void SendSettingPlanetRequest(string userId)
    {
        Mainres = new Response<ResponseMainPlanet>();

        header[0] = Define.JWT_ACCESS_TOKEN;
        header[1] = "User-Id";

        headerValue[0] = PlayerPrefs.GetString(Define.JWT_ACCESS_TOKEN);
        //headerValue[1] = PlayerPrefs.GetString(Define.USER_ID);
        headerValue[1] = userId;

        Managers.Web.SendGetRequest("api/planet/main/", userId, mainCallback, header, headerValue);

    }

    //경로에 api가 붙은 API 사용 시 토큰 확인 필요
    //토큰 확인 후 할 action을 인자로 받음
    //TokenResponseAction에서 응답 확인 후 인자로 받은 callback invoke
    public void SendTokenRequest(Action innnerCallback)
    {
        Tokenres = new Response<string>();
        RequestToken val = new RequestToken { deviceToken = "12345" }; //디바이스 토큰 수정 필요
        header[0] = Define.JWT_REFRESH_TOKEN;
        header[1] = "User-Id";

        headerValue[0] = PlayerPrefs.GetString(Define.JWT_REFRESH_TOKEN);
        headerValue[1] = PlayerPrefs.GetString(Define.USER_ID);

        Managers.Web.SendPostRequest<string>("access-token", val, tokenCallback, innnerCallback, header, headerValue);
    }

    public void SendArrangementRequest()
    {
        
        ConvertToRequestList();
        RequestArrangement val = new RequestArrangement 
        { itemPositionList = placedItemList};


        header[0] = Define.JWT_ACCESS_TOKEN;
        header[1] = "User-Id";

        headerValue[0] = PlayerPrefs.GetString(Define.JWT_ACCESS_TOKEN);
        headerValue[1] = PlayerPrefs.GetString(Define.USER_ID);

        Managers.Web.SendUniRequest("api/inventory/planet-items/placement", "PATCH", val, arrangeCallback, header, headerValue);
    }

    public void FirstInstantiate()
    {
        Debug.Log("첫번째 생성");
        SendSettingPlanetRequest(PlayerPrefs.GetString(Define.USER_ID));
    }

    //행성 생성 & 캐릭터생성
    private void PlanetInstantiate(string color, int level)
    {
        //blue,green,red 중 무엇인지 확인 후 해당 행성 생성
        //레벨도 확인

        if (planet == null)
        {
            string path = "Planet/" + color + "_" +
                    level.ToString();


            planet = Managers.Resource.Instantiate(path);
            DontDestroyOnLoad(planet);
        }

    }

    // 행성 레벨업 시
    public void UpgradePlanet()
    {

    }

    private void ItemInstantiate()
    {
        realPlacedItemList.Clear();

        Transform[] childList = planet.transform.GetChild(2).GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != planet.transform.GetChild(2))
                {
                    Managers.Resource.Destroy(childList[i].gameObject);
                }
            }
        }

        for (int i = 0; i < placedItemList.Count; i++)
        {
            string path = "Items/" + placedItemList[i].itemCode;

            for(int j = 0; j < placedItemList[i].positionList.Count; j++)
            {
                Vector3 pos = new Vector3(placedItemList[i].positionList[j].posX, placedItemList[i].positionList[j].posY, planet.transform.GetChild(2).transform.position.z);
                GameObject tmp = Managers.Resource.Instantiate(pos, path, planet.transform.GetChild(2).transform);
                ChangeItemMode(tmp, Define.Scene.Main);

                realPlacedItemList.Add(tmp);
            }
            

        }
    }


    private void ChangeItemMode(GameObject go, Define.Scene type)
    {
        ItemController child = Util.FindChild<ItemController>(go, "ItemInner", true);
        child.ChangeMode(type);
    }

    //씬에 생성된 실제 아이템들을 서버에 보낼 List 형태로 변환
    private void ConvertToRequestList()
    {
        placedItemList.Clear();

        for (int i = 0; i < realPlacedItemList.Count; i++)
        {

            string code = Util.RemoveCloneString(realPlacedItemList[i].name);
            float posX = realPlacedItemList[i].transform.position.x;
            float posY = realPlacedItemList[i].transform.position.y;

            /*            MainItemPosition tPos = new MainItemPosition();
                        tPos.posX = posX;
                        tPos.posY = posY;*/

            Debug.Log($"{code} {posX} {posY}");

            int index = placedItemList.FindIndex(x => x.itemCode == code);
            
            //동일한 아이템 없으면
            if (index == -1)
            {
                List<MainItemPosition> pos = new List<MainItemPosition>();
                pos.Add(new MainItemPosition { posX = posX, posY = posY });

                MainItemList tmp = new MainItemList { itemCode = code, positionList = pos };
                placedItemList.Add(tmp);

            }
            //있으면 위치만 추가
            else
            {
                placedItemList[index].positionList.Add(new MainItemPosition { posX = posX, posY = posY });
            }
        }
    }

    public GameObject GetPlanet()
    {
        return planet;
    }


    public void AddItemList(GameObject go)
    {
        realPlacedItemList.Add(go);
    }

    public void RemoveItemList(GameObject go)
    {
        realPlacedItemList.Remove(go);
    }

    public int CountItem(string name)
    {
        //string fullname = name + "(Clone)";
        return realPlacedItemList.FindAll(x => x.name == name).Count;
    }

    public void ChangeItemModeList(Define.Scene type)
    {
        if (realPlacedItemList!=null)
        {
            for (int i = 0; i < realPlacedItemList.Count; i++)
            {
                ChangeItemMode(realPlacedItemList[i], type);
            }
        }

    }

    #region PlayerPrefs
    public void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);        
        PlayerPrefs.Save();
    }

    public void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    public string GetString(string key, string defalutValue = null)
    {
       return PlayerPrefs.GetString(key);
    }

    public int GetInt(string key, string defalutValue = null)
    {
        return PlayerPrefs.GetInt(key);
    }

    public float GetFloat(string key, string defalutValue = null)
    {
        return PlayerPrefs.GetFloat(key);
    }

    public string[] GetHeader(bool access = true)
    {
        if (access)
        {
            header[0] = Define.JWT_ACCESS_TOKEN;
            header[1] = "User-Id";
            return header;
        }
        else
        {
            header[0] = Define.JWT_REFRESH_TOKEN;
            header[1] = "User-Id";
            return header;
        }
    }

    public string[] GetHeaderValue(bool access = true)
    {
        if (access)
        {
            headerValue[0] = PlayerPrefs.GetString(Define.JWT_ACCESS_TOKEN);
            headerValue[1] = PlayerPrefs.GetString(Define.USER_ID);

            return headerValue;
        }
        else
        {
            headerValue[0] = PlayerPrefs.GetString(Define.JWT_REFRESH_TOKEN);
            headerValue[1] = PlayerPrefs.GetString(Define.USER_ID);

            return headerValue;
        }
    }
    #endregion
}
