using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{

    //�ܺ� ���� ����, ���� �迭�� set�� �� ����
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
        {
            Managers.Resource.Instantiate("EventSystem").name = "@EventSystem";
        }

        Object mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCam == null)
        {
            Managers.Resource.Instantiate("Camera/MainCamera").name = "MainCamera";
        }


        Object UICam = GameObject.FindGameObjectWithTag("UICamera");
        if (UICam == null)
        {
            Managers.Resource.Instantiate("Camera/UICamera").name = "UICamera";
        }

    }

    public abstract void Clear();
}