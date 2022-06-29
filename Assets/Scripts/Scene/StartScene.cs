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
            //��ū Ȯ��(�ڵ��α��� ����)
            if (PlayerPrefs.HasKey(Define.JWT_ACCESS_TOKEN) && PlayerPrefs.HasKey(Define.JWT_REFRESH_TOKEN))
            {
                Managers.Player.FirstInstantiate();
                UI_Load.Instance.ToLoad(Define.Scene.Main.ToString());


            }
            else
            {
                Debug.Log("No token");
                //��ū ������
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
