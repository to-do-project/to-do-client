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


    //�༺ ������ ������ �༺ �̹��� ��ü�� ���⼭ ó��
    //���� ĳ��
    //�༺ instantiate, ������ instantiate



    public Action<UnityWebRequest, Action> tokenCallback;
    Action<UnityWebRequest> mainCallback;
    Action<UnityWebRequest> arrangeCallback;


    Action firstCallback;

    GameObject planet;
    GameObject character;


    Response<ResponseMainPlanet> Mainres;
    Response<string> Tokenres;
    Response<string> Arrangeres;


    List<MainItemList> placedItemList;
    List<GameObject> realPlacedItemList;

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
        Debug.Log("�÷��̾� �Ŵ��� ����");

        mainCallback -= PlanetResponseAction;
        mainCallback += PlanetResponseAction;

        tokenCallback -= TokenResponseAction;
        tokenCallback += TokenResponseAction;

        firstCallback -= FirstInstantiate;
        firstCallback += FirstInstantiate;

        arrangeCallback -= ArrangementResponseAction;
        arrangeCallback += ArrangementResponseAction;


        //��ū Ȯ�� & ��߱� (�ڵ��α��� ����)
        if (PlayerPrefs.HasKey(Define.JWT_ACCESS_TOKEN) && PlayerPrefs.HasKey(Define.JWT_REFRESH_TOKEN))
        {
            Debug.Log("Token ����");

            Managers.Scene.LoadScene(Define.Scene.Main);
            SendTokenRequest(firstCallback);
        }
        else
        {
            Debug.Log("No token");
            //��ū ������
        }


    }

    
    //�༺ ����ȭ�� ��ȸ API ȣ�� �� action
    private void PlanetResponseAction(UnityWebRequest request)
    {
        if (Mainres != null)
        {
            Mainres = JsonUtility.FromJson<Response<ResponseMainPlanet>>(request.downloadHandler.text);

            if (Mainres.isSuccess)
            {
                if (Mainres.code == 1000)
                {
                    Debug.Log("�༺ ����");
                    //�༺, ������, ĳ���� ����
                    PlanetInstantiate(Mainres.result.planetColor, Mainres.result.level);
                    placedItemList = Mainres.result.planetItemList;
                    CharacterInstantiate(Mainres.result.characterItem);
                    ItemInstantiate();
                }
            }

            else
            {

            }
            Mainres = null;
        }

    }

    //��ū ��߱� APi ȣ�� �� action
    private void TokenResponseAction(UnityWebRequest request, Action callback)
    {
        
        if (Tokenres != null)
        {
            Tokenres = JsonUtility.FromJson<Response<string>>(request.downloadHandler.text);

            Debug.Log(Tokenres.message);
            if (Tokenres.isSuccess)
            {

                Debug.Log("��ū Ȯ��");
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

    //���� �Ϸ� �� ������ ��ġ API ȣ�� action
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

    //�༺ ���� ȭ�� ���� API ȣ��
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

    //��ο� api�� ���� API ��� �� ��ū Ȯ�� �ʿ�
    //��ū Ȯ�� �� �� action�� ���ڷ� ����
    //TokenResponseAction���� ���� Ȯ�� �� ���ڷ� ���� callback invoke
    public void SendTokenRequest(Action innnerCallback)
    {
        Tokenres = new Response<string>();
        RequestToken val = new RequestToken { deviceToken = UnityEngine.SystemInfo.deviceUniqueIdentifier }; //����̽� ��ū ���� �ʿ�
        header[0] = Define.JWT_REFRESH_TOKEN;
        header[1] = "User-Id";

        headerValue[0] = PlayerPrefs.GetString(Define.JWT_REFRESH_TOKEN);
        headerValue[1] = PlayerPrefs.GetString(Define.USER_ID);

        Managers.Web.SendPostRequest<string>("access-token", val, tokenCallback, innnerCallback, header, headerValue);
    }

    //��ġ�� ������ ���� ������ ���� API ȣ��
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

    //token ��߱� API ���� �� �ҷ��� �׼�
    public void FirstInstantiate()
    {
        Debug.Log("ù��° ����");
        SendSettingPlanetRequest(PlayerPrefs.GetString(Define.USER_ID));
    }

    //�༺ ���� 
    private void PlanetInstantiate(string color, int level)
    {
        //blue,green,red �� �������� Ȯ�� �� �ش� �༺ ����
        //������ Ȯ��

        if (planet == null)
        {
            string path = "Planet/" + color + "_" +
                    level.ToString();


            planet = Managers.Resource.Instantiate(path);
            DontDestroyOnLoad(planet);


        }

    }

    //ĳ���� ����
    private void CharacterInstantiate(long characterColor)
    {
        //Debug.Log("character ");
        if (planet != null)
        {
            if (character == null)
            {
                string path = "Character/" + "Character_" + characterColor.ToString();
                Vector3 pos = new Vector3(0, 5.81f, planet.transform.position.z);

                character = Managers.Resource.Instantiate(pos, path, planet.transform);
                DontDestroyOnLoad(character);
            }
        }
    }

    // �༺ ������ ��
    public void UpgradePlanet()
    {

    }

    //������ ����
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


    //������ ���������ϵ��� ��� ��ȯ
    private void ChangeItemMode(GameObject go, Define.Scene type)
    {
        ItemController child = Util.FindChild<ItemController>(go, "ItemInner", true);
        child.ChangeMode(type);
    }

    //���� ������ ���� �����۵��� ������ ���� List ���·� ��ȯ
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
            
            //������ ������ ������
            if (index == -1)
            {
                List<MainItemPosition> pos = new List<MainItemPosition>();
                pos.Add(new MainItemPosition { posX = posX, posY = posY });

                MainItemList tmp = new MainItemList { itemCode = code, positionList = pos };
                placedItemList.Add(tmp);

            }
            //������ ��ġ�� �߰�
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

    public bool CheckItemFixState()
    {
        if (realPlacedItemList != null)
        {
            //�ϳ��� ������ ���°� �ƴ� ������ ���� �� false ��ȯ
            for(int i = 0; i < realPlacedItemList.Count; i++)
            {
                ItemController child = Util.FindChild<ItemController>(realPlacedItemList[i], "ItemInner", true);

                if (!child.GetFixState())
                {
                    return false;
                }
            }
        }

        return true;
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
