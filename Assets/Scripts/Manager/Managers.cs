using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� �Ŵ��� �Ѱ� ����
/// �ڱ��ڽ��� �����ؼ� ��� �Ŵ��� �ν��Ͻ� �̱������� ����
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

            //�ٸ� �Ŵ����� Init�� ���⼭ ���ֱ�
        }
    }
}
