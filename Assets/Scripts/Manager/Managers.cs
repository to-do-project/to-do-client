using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 매니저 총괄 관리
/// 자기자신을 포함해서 모든 매니저 인스턴스 싱글톤으로 관리
/// </summary>
public class Managers : MonoBehaviour
{
    static Managers instance;
    static Managers Instance { get { Init(); return instance; } }


    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    static void Init()
    {
        if(instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<Managers>();

            //다른 매니저들 Init도 여기서 해주기
        }
    }
}
