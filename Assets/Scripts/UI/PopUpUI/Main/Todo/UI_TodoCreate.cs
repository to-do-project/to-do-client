using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TodoCreate : UI_Popup
{
    enum Buttons
    {
        check_btn,
        exit_btn,
    }

    enum InputFields
    {
        todoName_inputfield,
        friend_inputfield,
    }

    enum Texts
    {
        date_txt,
    }

    enum Toggles
    {
        open_toggle,
    }

    enum GameObjects
    {
        ToastMessage,
    }

    void Start()
    {
        Init();
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

        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Toggle>(typeof(Toggles));
    }


}
