using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    void Start()
    {
        /*
        if (PlayerPrefs.HasKey(Define.JWT_ACCESS_TOKEN) && PlayerPrefs.HasKey(Define.JWT_REFRESH_TOKEN))
        {
            Managers.Instance();
            string[] hN = { Define.JWT_REFRESH_TOKEN,
                            "User-Id" };
            string[] hV = { Managers.Player.GetString(Define.JWT_REFRESH_TOKEN),
                            Managers.Player.GetString(Define.USER_ID) };

            RequestLogout request = new RequestLogout();
            request.deviceToken = SystemInfo.deviceUniqueIdentifier;

            Managers.Web.SendUniRequest("access-token", "POST", request, (uwr) => {
                Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
                if (response.code == 1000)
                {
                    Debug.Log(response.result);

                    Managers.Player.SetString(Define.JWT_ACCESS_TOKEN, uwr.GetResponseHeader(Define.JWT_ACCESS_TOKEN));
                    Managers.Player.SetString(Define.JWT_REFRESH_TOKEN, uwr.GetResponseHeader(Define.JWT_REFRESH_TOKEN));

                    UI_Load.Instance.ToLoad(Define.Scene.Main.ToString());
                }
                else
                {
                    Debug.Log(response.message);
                    UI_Load.Instance.ToLoad(Define.Scene.Login.ToString());
                }
            }, hN, hV);
        }
        else
        {
            UI_Load.Instance.ToLoad(Define.Scene.Login.ToString());
        }
        */
        UI_Load.Instance.ToLoad(Define.Scene.Login.ToString());
    }
}
