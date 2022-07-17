using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    public override void Clear()
    {
        Managers.UI.Clear();
        //throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();
        if (Managers.Web.InternetCheck())
        {
            //토큰 확인(자동로그인 상태)
            if (PlayerPrefs.HasKey(Define.JWT_ACCESS_TOKEN) && PlayerPrefs.HasKey(Define.JWT_REFRESH_TOKEN))
            {

                RequestLogin val = new RequestLogin() {
                    email = Managers.Player.GetString(Define.EMAIL),
                    password = Managers.Player.GetString(Define.PASSWORD),
                    deviceToken = Managers.Player.GetString(Define.DEVICETOKEN),

                };

                Managers.Web.SendPostRequest<ResponseSignUp>("login", val, (uwr)=> {
                    Response<ResponseLogin> res = JsonUtility.FromJson<Response<ResponseLogin>>(uwr.downloadHandler.text);

                    if (res.isSuccess)
                    {
                        //Debug.Log("User Id : "+res.result.userId + " " + Managers.Player.GetUserId());


                        if (res.result.userId != Managers.Player.GetUserId())
                        {
                            UI_Load.Instance.ToLoad(Define.Scene.Login.ToString());
                        }
                        else
                        {


                            Managers.Player.SetString(Define.JWT_ACCESS_TOKEN, uwr.GetResponseHeader(Define.JWT_ACCESS_TOKEN));
                            Managers.Player.SetString(Define.JWT_REFRESH_TOKEN, uwr.GetResponseHeader(Define.JWT_REFRESH_TOKEN));
                            
                            ResponseLogin result = res.result;

                            Managers.Player.SetString(Define.EMAIL, result.email);
                            Managers.Player.SetString(Define.NICKNAME, result.nickname);
                            Managers.Player.SetString(Define.USER_ID, result.userId.ToString());
                            Managers.Player.SetString(Define.PLANET_ID, result.planetId.ToString());
                            Managers.Player.SetInt(Define.PLANET_LEVEL, result.planetLevel);
                            Managers.Player.SetString(Define.PLANET_COLOR, result.planetColor);
                            Managers.Player.SetString(Define.EMAIL, result.email);
                            Managers.Player.SetString(Define.NICKNAME, result.nickname);
                            Managers.Player.SetString(Define.CHARACTER_COLOR, result.characterItem.ToString());
                            Managers.Player.SetString(Define.PROFILE_COLOR, result.profileColor);
                            Managers.Player.SetInt(Define.MISSION_STATUS, result.missionStatus);
                            Managers.Player.SetString(Define.PASSWORD, val.password);

                            Managers.Player.FirstInstantiate();
                            UI_Load.Instance.ToLoad(Define.Scene.Main.ToString());
                        }

                    }
                    else
                    {
                        Debug.Log(res.message);
                        UI_Load.Instance.ToLoad(Define.Scene.Login.ToString());
                    }
                });







            }
            else
            {
                Debug.Log("No token");
                //토큰 없으면
                UI_Load.Instance.ToLoad(Define.Scene.Login.ToString());

            }
        }
    }

    void Awake()
    {
        //UI_Load.Instance.ToLoad(Define.Scene.Login.ToString());
        Init();
    }


}
