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
                Managers.Player.FirstInstantiate();
                UI_Load.Instance.ToLoad(Define.Scene.Main.ToString());


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
