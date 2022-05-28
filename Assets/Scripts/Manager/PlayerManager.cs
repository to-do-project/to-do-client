using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestToken
{
    public string deviceToken;
}

[System.Serializable]
public class ResponseMainPlanet
{
    public int level;
    public long characterItem;
    public List<MainItemList> planetItemList;
}

[System.Serializable]
public class MainItemList
{
    public long itemId;
    public List<MainItemPosition> positionList;
}

[System.Serializable]
public class MainItemPosition
{
    public float posX;
    public float posy;
}


public class PlayerManager : MonoBehaviour
{


    //행성 레벨업 등으로 행성 이미지 교체도 여기서 처리
    //투두 캐싱
    //행성 instantiate, 아이템 instantiate



    Action<UnityWebRequest> tokenCallback;

    GameObject planet;

    Action<UnityWebRequest> callback;
    Response<ResponseMainPlanet> Mainres;
    Response<string> Tokenres;

    List<MainItemList> itemList;

    string[] header = new string[2];
    string[] headerValue = new string[2];

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        //PlayerPrefs.DeleteAll();
        Debug.Log("플레이어 매니저 실행");

        callback -= ResponseAction;
        callback += ResponseAction;

        //토큰 확인 & 재발급 (자동로그인 상태)
        if (PlayerPrefs.HasKey(Define.JWT_ACCESS_TOKEN) && PlayerPrefs.HasKey(Define.JWT_REFRESH_TOKEN))
        {
            Debug.Log("Token 있음");
            //토큰 확인
            Tokenres = new Response<string>();
            RequestToken val = new RequestToken { deviceToken = "12345" }; //디바이스 토큰 수정 필요
            header[0] = Define.JWT_REFRESH_TOKEN;
            header[1] = "User-Id";

            headerValue[0] = PlayerPrefs.GetString(Define.JWT_REFRESH_TOKEN);
            headerValue[1] = PlayerPrefs.GetString(Define.USER_ID);

            Managers.Web.SendPostRequest<string>("access-token", val, callback,  header, headerValue);

        }
        else
        {
            Debug.Log("No token");
            //토큰 없으면
        }

        //Managers.Web.SendPostRequest<ResponseMainPlanet>("");

    }

    private void ResponseAction(UnityWebRequest request)
    {
        if (Mainres != null)
        {
            Mainres = JsonUtility.FromJson<Response<ResponseMainPlanet>>(request.downloadHandler.text);

            if (Mainres.isSuccess)
            {
                if(Mainres.code == 1000)
                {
                    itemList = Mainres.result.planetItemList;
                    ItemInstantiate();
                }
            }
            Mainres = null;
        }

        else if(Tokenres != null)
        {
            Tokenres = JsonUtility.FromJson<Response<string>>(request.downloadHandler.text);

            if (Tokenres.isSuccess)
            {
                if (Tokenres.code == 1000)
                {
                    PlayerPrefs.SetString(Define.JWT_ACCESS_TOKEN, request.GetResponseHeader(Define.JWT_ACCESS_TOKEN));
                    PlayerPrefs.SetString(Define.JWT_REFRESH_TOKEN, request.GetResponseHeader(Define.JWT_REFRESH_TOKEN));

                    PlanetInstantiate();

                    //Item 불러오기
                    Mainres = new Response<ResponseMainPlanet>();

                    header[0] = Define.JWT_ACCESS_TOKEN;
                    header[1] = "User-Id";

                    headerValue[0] = PlayerPrefs.GetString(Define.JWT_ACCESS_TOKEN);
                    headerValue[1] = PlayerPrefs.GetString(Define.USER_ID);

                    Managers.Web.SendGetRequest("api/planet/main/", PlayerPrefs.GetString(Define.USER_ID),callback, header, headerValue);
                }
            }
            else
            {
                Debug.Log(Tokenres.message);
                if (Tokenres.code == 6023)
                {
                    Managers.UI.Clear();
                    Managers.Scene.LoadScene(Define.Scene.Login);
                }
            }

            Tokenres = null;
        }
    }

    private void TokenResponseAction(UnityWebRequest request)
    {
        if (Tokenres != null)
        {
            Tokenres = JsonUtility.FromJson<Response<string>>(request.downloadHandler.text);

            if (Tokenres.isSuccess)
            {

            }
            else
            {
                
                if (Tokenres.code ==6023)
                {
                    Managers.UI.Clear();
                    Managers.Scene.LoadScene(Define.Scene.Login);
                }
            }
        }
    }

        //행성 생성
        private void PlanetInstantiate()
    {
        //blue,green,red 중 무엇인지 확인 후 해당 행성 생성
        //레벨도 확인

        if (planet == null)
        {
            //PlayerPrefs에 정보가 있으면
            if (PlayerPrefs.HasKey(Define.USER_ID) && PlayerPrefs.HasKey(Define.PLANET_ID))
            {
                string path = "Planet/" + PlayerPrefs.GetString(Define.PLANET_COLOR) + "_" +
                    PlayerPrefs.GetInt(Define.PLANET_LEVEL).ToString();


                planet = Managers.Resource.Instantiate(path);
                DontDestroyOnLoad(planet);
            }
        }

    }

    // 행성 레벨업 시
    public void UpgradePlanet()
    {

    }

    private void ItemInstantiate()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            string path = "Items/" + itemList[i].itemId;
            //GameObject tmp = Managers.Resource.Instantiate(path, planet.transform.GetChild(2).transform);
            //ChangeItemMode(tmp, Define.Scene.Main);

        }
    }


    private void ChangeItemMode(GameObject go, Define.Scene type)
    {
        ItemController child = Util.FindChild<ItemController>(go, "ItemInner", true);
        child.ChangeMode(type);
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

            return header;
        }
        else
        {
            headerValue[0] = PlayerPrefs.GetString(Define.JWT_REFRESH_TOKEN);
            headerValue[1] = PlayerPrefs.GetString(Define.USER_ID);

            return header;
        }
    }
    #endregion
}
