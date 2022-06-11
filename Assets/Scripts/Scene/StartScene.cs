using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    void Start()
    {
        UI_Load.Instance.ToLoad(Define.Scene.Login.ToString());
    }
}
