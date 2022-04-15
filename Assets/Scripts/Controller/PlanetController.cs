using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    void Start()
    {
        Init();
    }

    void Init()
    {
        Managers.Input.TouchAction -= EnterArrayMode;
        Managers.Input.TouchAction += EnterArrayMode;
    }

    void EnterArrayMode(Define.TouchEvent evt)
    {
        if(evt != Define.TouchEvent.Press)
        {
            return;
        }

        //raycast�� �༺���� Ȯ��
        
        //Managers.Resource.Instantiate("Camera/ZoomCam");
        //Debug.Log("Enter Array Mode");
    }
}
