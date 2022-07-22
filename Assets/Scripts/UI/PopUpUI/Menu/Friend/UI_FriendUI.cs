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
    GameObject btn = null;
    long memberId = 0, userId = 0;
    bool clicked = false;
    public string nickname { private get; set; } = "";

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
        title.text = nickname + "님의 목표리스트";

        Text date = GetText((int)Texts.date_txt);
        DateTime today = DateTime.Now;
        date.text = today.ToString("yyyy") + "." + today.ToString("MM") + "." + today.ToString("dd");
        Debug.Log(today);

        SetBtn((int)Buttons.Back_btn, (data) => {
            FindObjectOfType<UI_FriendMain>().DestroyAll();
            Managers.Sound.PlayNormalButtonClickSound();
            Managers.UI.ActiveAllUI();
            Managers.Player.GetPlanet().SetActive(true);
            Destroy(gameObject);
        });

        btn = SetBtn((int)Buttons.Fighting_btn, (data) => {
            Ex_Like();
            Managers.Sound.PlayNormalButtonClickSound();
            btn.SetActive(false);
        });

        btn.SetActive(false);
    }

    public void OnFightingView(long id)
    {
        memberId = id;
        if(btn.activeSelf == false)
        {
            btn.SetActive(true);
            Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/NotComplete"));
            Managers.Sound.PlayPopupSound();
        }
    }

    public void SetUserId(long userId)
    {
        this.userId = userId;
    }

    public long GetUserId()
    {
        return userId;
    }

    void Ex_Like()
    {
        if (clicked) return;
        clicked = true;
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/todo/" + memberId, "GET", null, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            if (response.isSuccess)
            {
                Debug.Log(response.result);
                clicked = false;
            }
            else if (response.code == 6000)
            {
                clicked = false;
                Managers.Player.SendTokenRequest(Ex_Like);
            }
            else
            {
                Debug.Log(response.message);
                clicked = false;
            }
        }, hN, hV);
    }

    private void Start()
    {
        Init();
    }
}
