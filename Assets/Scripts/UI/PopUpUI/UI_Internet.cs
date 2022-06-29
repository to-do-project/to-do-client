using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Internet : UI_Popup
{
    void Start()
    {
        
    }

    public override void Init()
    {
        base.Init();

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
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
    }
}
 