using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_FriendUI : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
        Fighting_btn,
    }

    enum Texts
    {
        date_txt,
        title_txt,
    }

    enum GameObjects
    {
        GoalList,
    }

    //GameObject goalList;


    public override void Init()
    {
        base.Init();

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();

        if (UIcam != cam)
        {
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        Text title = GetText((int)Texts.title_txt);
        title.text = Managers.Player.GetString(Define.NICKNAME) + "님의 목표리스트";

        Text date = GetText((int)Texts.date_txt);
        DateTime today = DateTime.Now;
        date.text = today.ToString("yyyy") + "." + today.ToString("mm") + "." + today.ToString("dd");

        SetBtn((int)Buttons.Back_btn, (data) => {
            FindObjectOfType<UI_FriendMain>().DestroyAll();
            Managers.UI.ActiveAllUI();
            Managers.Player.GetPlanet().SetActive(true);
            Destroy(gameObject);
        });

        SetBtn((int)Buttons.Fighting_btn, (data) => {
            Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/NotComplete"));
        });
    }

    private void Start()
    {
        Init();
    }
}
