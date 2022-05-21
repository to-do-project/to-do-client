using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;

public class Testing : MonoBehaviour
{
    public static Testing instance = null;

    const string baseUrl = "https://dev.teamplanz.shop";

    public void Webbing(string url, string method, object obj, Action<UnityWebRequest> callback, List<string> headerName, List<string> headerValue)
    {
        StartCoroutine(CoSendWebRequest(url, method, obj, callback, headerName, headerValue));
    }

    IEnumerator CoSendWebRequest(string url, string method, object obj, Action<UnityWebRequest> callback, List<string> headerName, List<string> headerValue)
    {
        string sendUrl = $"{baseUrl}/{url}";

        byte[] jsonByte = null;
        if (obj != null)
        {
            string jsonStr = JsonUtility.ToJson(obj);
            jsonByte = Encoding.UTF8.GetBytes(jsonStr);
        }
        Debug.Log(obj);

        var uwr = new UnityWebRequest(sendUrl, method);
        uwr.uploadHandler = new UploadHandlerRaw(jsonByte); //바이트배열 업로드
        uwr.downloadHandler = new DownloadHandlerBuffer(); //응답이 왔을 때
        if(headerName != null && headerValue != null)
        {
            for(int i = 0; i < headerName.Count; i++)
            {
                uwr.SetRequestHeader(headerName[i], headerValue[i]);
            }
        }
        uwr.SetRequestHeader("Content-type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            Debug.Log("Recv " + uwr.downloadHandler.text);
            callback.Invoke(uwr);

        }


    }

    private void Start()
    {
        if(instance == null)
        {
            instance = gameObject.GetComponent<Testing>();
        }
    }
}

[Serializable]
public class Test
{
    public string deviceToken;
}