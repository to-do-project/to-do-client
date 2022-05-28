using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public class WebManager : MonoBehaviour
{
    string baseUrl = "https://dev.teamplanz.shop";


    public void SendPostRequest<T>(string url, object obj, Action<UnityWebRequest> callback, string[] header = null, string[] headerValue = null)
    {
        StartCoroutine(PostRequest<T>(url, obj,callback, header, headerValue));
    }

    public void SendGetRequest(string url, string param, Action<UnityWebRequest> callback, string[] header = null, string[] headerValue = null)
    {
        StartCoroutine(GetRequest(url,param,callback, header, headerValue));
    }


    IEnumerator PostRequest<T>(string url, object obj, Action<UnityWebRequest> callback = null, string[] header = null, string[] headerValue = null)
    {
        string sendUrl = $"{baseUrl}/{url}";
        byte[] jsonByte = null;
        if (obj != null)
        {
            string jsonStr = JsonUtility.ToJson(obj);
            jsonByte = Encoding.UTF8.GetBytes(jsonStr);
        }

        using (var uwr = new UnityWebRequest(sendUrl, "POST"))
        {
            uwr.uploadHandler = new UploadHandlerRaw(jsonByte); //����Ʈ�迭 ���ε�
            uwr.downloadHandler = new DownloadHandlerBuffer(); //������ ���� ��
            uwr.SetRequestHeader("Content-type", "application/json");

            if (header != null && headerValue!=null)
            {
                for(int i = 0; i < header.Length; i++)
                {
                    uwr.SetRequestHeader(header[i], headerValue[i]);

                }
            }

            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                if (callback != null)
                {
                    //�״㿡 ������ �̺�Ʈ ȣ��
                    Debug.Log("Recv " + uwr.downloadHandler.text);
                    callback.Invoke(uwr);

                }
                //Debug.Log("Recv " + uwr.downloadHandler.text);
                //res = JsonUtility.FromJson<Response<T>>(uwr.downloadHandler.text);

            }
        }
        
    }

    IEnumerator GetRequest(string url, string param, Action<UnityWebRequest> callback = null, string[] header = null, string[] headerValue = null)
    {
        string sendUrl = $"{baseUrl}/{url}{param}";

        using (UnityWebRequest uwr = UnityWebRequest.Get(sendUrl))
        {
            if (header != null && headerValue != null)
            {
                for (int i = 0; i < header.Length; i++)
                {
                    uwr.SetRequestHeader(header[i], headerValue[i]);

                }
            }

            yield return uwr.SendWebRequest();

            if(uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                if (callback != null)
                {
                    Debug.Log("Recv " + uwr.downloadHandler.text);
                    //�״㿡 ������ �̺�Ʈ ȣ��
                    callback.Invoke(uwr);

                }
            }
        }

    }


    /*//method�� enum���� �ص� ��(get,post)
    //�����ϴ� obj
    //callback : CosendWebRequest ȣ���� �ʿ��� callback�� �ش� unitywebrequest�� ���ڷ� �޾ƿ�
    IEnumerator CoSendWebRequest(string url, string method, object obj, Action<UnityWebRequest> callback)
    {

        string sendUrl = $"{baseUrl}/{url}";


        byte[] jsonByte = null;
        if (obj != null)
        {
            string jsonStr = JsonUtility.ToJson(obj);
            jsonByte = Encoding.UTF8.GetBytes(jsonStr);
        }

        var uwr = new UnityWebRequest(sendUrl, method);
        uwr.uploadHandler = new UploadHandlerRaw(jsonByte); //����Ʈ�迭 ���ε�
        uwr.downloadHandler = new DownloadHandlerBuffer(); //������ ���� ��
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


    }*/

    //API ȣ�� ���� �θ���?
/*    public bool InternetCheck()
    {
        //���ͳݿ� ����Ǿ� �ִ��� Ȯ��
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }
        else
        {
            return true;
        }
    }*/

}
