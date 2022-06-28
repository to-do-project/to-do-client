using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{


    void Start()
    {
        //UI_Load.Instance.ToLoad(Define.Scene.Login.ToString());

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
