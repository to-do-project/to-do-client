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


    //�༺ ������ ������ �༺ �̹��� ��ü�� ���⼭ ó��
    //���� ĳ��
    //�༺ instantiate, ������ instantiate


    GameObject planet;

    Action<UnityWebRequest> callback;
    Response<ResponseMainPlanet> Mainres;
    Response<string> Tokenres;

    string[] header = new string[2];
    string[] headerValue = new string[2];

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        Debug.Log("�÷��̾� �Ŵ��� ����");

        callback -= ResponseAction;
        callback += ResponseAction;

        //��ū Ȯ�� & ��߱� (�ڵ��α��� ����)
        if (PlayerPrefs.HasKey(Define.JWT_ACCESS_TOKEN) && PlayerPrefs.HasKey(Define.JWT_REFRESH_TOKEN))
        {
            Debug.Log("Token ����");
            //��ū Ȯ��
            Tokenres = new Response<string>();
            RequestToken val = new RequestToken { deviceToken = "12345" }; //����̽� ��ū ���� �ʿ�
            header[0] = Define.JWT_REFRESH_TOKEN;
            header[1] = "User-Id";

            headerValue[0] = PlayerPrefs.GetString(Define.JWT_REFRESH_TOKEN);
            headerValue[1] = PlayerPrefs.GetString(Define.USER_ID);

            Managers.Web.SendPostRequest<string>("access-token", val, callback,  header, headerValue);

        }
        else
        {
            Debug.Log("No token");
            //��ū ������
        }

        //Managers.Web.SendPostRequest<ResponseMainPlanet>("");

    }

    private void ResponseAction(UnityWebRequest request)
    {
        if (Mainres != null)
        {
            Mainres = JsonUtility.FromJson<Response<ResponseMainPlanet>>(request.downloadHandler.text);

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

                    //Item �ҷ�����
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
            }

            Tokenres = null;
        }
    }

    //�༺ ����
    private void PlanetInstantiate()
    {
        //blue,green,red �� �������� Ȯ�� �� �ش� �༺ ����
        //������ Ȯ��

        if (planet == null)
        {
            //PlayerPrefs�� ������ ������
            if (PlayerPrefs.HasKey(Define.USER_ID) && PlayerPrefs.HasKey(Define.PLANET_ID))
            {
                string path = "Planet/" + PlayerPrefs.GetString(Define.PLANET_COLOR) + "_" +
                    PlayerPrefs.GetInt(Define.PLANET_LEVEL).ToString();


                planet = Managers.Resource.Instantiate(path);
                DontDestroyOnLoad(planet);
            }
        }

    }

    // �༺ ������ ��
    public void UpgradePlanet()
    {

    }

    private void ItemInstantiate()
    {
        
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

    #endregion
}
