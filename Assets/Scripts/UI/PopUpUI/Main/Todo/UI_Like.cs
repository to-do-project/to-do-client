using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Like : UI_Popup
{
    enum GameObjects
    {
        Content,
    }

    enum Texts
    {
        likenum2_txt,
    }

    public override void Init()
    {
        base.Init();

        Canvas canvas = GetComponent<Canvas>();
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }

        else
        {
            Debug.Log($"{UIcam.name}");
        }

        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
    }

    private void Start()
    {
        Init();
    }
}
