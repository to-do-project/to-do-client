using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PWfind : UI_Popup
{
    protected LoginScene loginScene;

    public override void Init()
    {
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

        loginScene = FindObjectOfType<LoginScene>();

    }

}
