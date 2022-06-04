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

    public void SendPostRequest<T>(string url, object obj, Action<UnityWebRequest, Action> callback, Action innerAction = null, string[] header = null, string[] headerValue = null)
    {
        StartCoroutine(PostRequest<T>(url, obj, callback,innerAction, header, headerValue));
    }

    public void SendGetRequest(string url, string param, Action<UnityWebRequest> callback, string[] header = null, string[] headerValue = null)
    {
        StartCoroutine(GetRequest(url,param,callback, header, headerValue));
    }

    /// <summary>
    /// 모든 웹 통신에 범용성 있게 사용할 수 있게 만든 함수
    /// </summary>
    /// <param name="url">          API 주소와 GET 통신시 Parameter값을 보내는 변수 </param>
    /// <param name="method">       GET, POST, PATCH, DELETE와 같은 API에서 사용 할 메소드를 보내는 변수 </param>
    /// <param name="obj">          데이터 클래스를 받아 Request Body로 넣어 줄 변수 </param>
    /// <param name="callback">     웹 통신 성공 시 실행 할 함수 </param>
    /// <param name="header">       헤더 이름들의 string 배열 </param>
    /// <param name="headerValue">  헤더 값들의 string 배열 </param>
    public void SendUniRequest(string url, string method, object obj, Action<UnityWebRequest> callback, string[] header = null, string[] headerValue = null)
    {
        StartCoroutine(UniRequest(url, method, obj, callback, header, headerValue));
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
            uwr.uploadHandler = new UploadHandlerRaw(jsonByte); //바이트배열 업로드
            uwr.downloadHandler = new DownloadHandlerBuffer(); //응답이 왔을 때
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
                    //그담에 수행할 이벤트 호출
                    Debug.Log("Recv " + uwr.downloadHandler.text);
                    callback.Invoke(uwr);

                }
                //Debug.Log("Recv " + uwr.downloadHandler.text);
                //res = JsonUtility.FromJson<Response<T>>(uwr.downloadHandler.text);

            }
        }
        
    }

    IEnumerator PostRequest<T>(string url, object obj, Action<UnityWebRequest,Action> callback = null, Action innerAction = null, string[] header = null, string[] headerValue = null)
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
            uwr.uploadHandler = new UploadHandlerRaw(jsonByte); //바이트배열 업로드
            uwr.downloadHandler = new DownloadHandlerBuffer(); //응답이 왔을 때
            uwr.SetRequestHeader("Content-type", "application/json");

            if (header != null && headerValue != null)
            {
                for (int i = 0; i < header.Length; i++)
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
                    //그담에 수행할 이벤트 호출
                    Debug.Log("Recv " + uwr.downloadHandler.text);
                    callback.Invoke(uwr, innerAction);

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
                    //그담에 수행할 이벤트 호출
                    callback.Invoke(uwr);

                }
            }
        }

    }

    // 프로젝트 전체 API 통신에 사용 가능한 코드
    IEnumerator UniRequest(string url, string method, object obj, Action<UnityWebRequest> callback, string[] header, string[] headerValue)
    {
        string sendUrl = $"{baseUrl}/{url}"; // baseUrl = 서버 주소, url = API 주소와 Get 파라미터

        byte[] jsonByte = null; // POST 통신용 RequestBody에 들어갈 JsonByte 변수
        if (obj != null)
        {
            string jsonStr = JsonUtility.ToJson(obj);   // 직렬화 된 오브젝트를 Json화
            jsonByte = Encoding.UTF8.GetBytes(jsonStr); // Json을 Byte화
        }

        var uwr = new UnityWebRequest(sendUrl, method);     // url 및 메소드 설정
        uwr.uploadHandler = new UploadHandlerRaw(jsonByte); // Json바이트를 RequestBody에 등록
        uwr.downloadHandler = new DownloadHandlerBuffer();  // 결과를 받기 위한 DownloadHandler

        if (header != null && headerValue != null)          // 헤더가 있다면
            for (int i = 0; i < header.Length; i++)
                uwr.SetRequestHeader(header[i], headerValue[i]);    // 헤더 설정

        uwr.SetRequestHeader("Content-type", "application/json");

        yield return uwr.SendWebRequest();  // 웹 통신 후 응답 대기

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError) // 에러 발생시
        {
            Debug.Log(uwr.error);   // 에러 로그 출력
        }
        else 
        {
            Debug.Log("Recv " + uwr.downloadHandler.text);
            callback.Invoke(uwr);   // 성공 시 메소드 실행
        }
    }


    /*//method는 enum으로 해도 됨(get,post)
    //전송하는 obj
    //callback : CosendWebRequest 호출한 쪽에서 callback에 해당 unitywebrequest를 인자로 받아옴
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
        uwr.uploadHandler = new UploadHandlerRaw(jsonByte); //바이트배열 업로드
        uwr.downloadHandler = new DownloadHandlerBuffer(); //응답이 왔을 때
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

    //API 호출 전에 부르기?
    /*    public bool InternetCheck()
        {
            //인터넷에 연결되어 있는지 확인
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
