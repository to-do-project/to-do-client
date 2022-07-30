using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_FriendUI : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
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
    // ================================ //

    public string nickname { private get; set; } = ""; // �г���

    //GameObject goalList;
    GameObject btn = null;

    long memberId = 0, userId = 0; // ģ�� ��� Id, ���� Id
    bool clicked = false; // �� ��� üũ

    public void SetUserId(long userId)
    {
        this.userId = userId;
    }

    public long GetUserId()
    {
        return userId;
    }

    // �ʱ�ȭ
    public override void Init()
    {
        base.Init();

        // ī�޶� ����
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
        // ============ //

        // �ȵ���̵� �ڷΰ��� ��ư �̺�Ʈ ���� �� ����
        Managers.Input.SystemTouchAction = OnFriendBackTouched;

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        Text title = GetText((int)Texts.title_txt);
        title.text = nickname + "���� ��ǥ����Ʈ";

        Text date = GetText((int)Texts.date_txt);
        DateTime today = DateTime.Now;
        date.text = today.ToString("yyyy") + "." + today.ToString("MM") + "." + today.ToString("dd");
        // Debug.Log(today);

        // �ڷΰ��� ��ư
        SetBtn((int)Buttons.Back_btn, (data) => {
            // ģ�� �༺, ĳ���� ������Ʈ ����
            FindObjectOfType<UI_FriendMain>().DestroyAll();

            // ��ư�� ��� �� ���� ������Ʈ Ȱ��ȭ
            Managers.Sound.PlayNormalButtonClickSound();
            Managers.UI.ActiveAllUI();
            Managers.Player.GetPlanet().SetActive(true);
            Destroy(gameObject);
        });

        // �����ϱ� ��ư
        btn = SetBtn((int)Buttons.Fighting_btn, (data) => {
            // �� ��� �� ��ư�� ���
            Ex_Like();
            Managers.Sound.PlayNormalButtonClickSound();
            btn.SetActive(false);
        });

        // �����ϱ� ��ư ��Ȱ��ȭ
        btn.SetActive(false);
    }

    // �����ϱ� ��ư Ȱ��ȭ
    // id = ��� id
    public void OnFightingView(long id)
    {
        memberId = id;
        if(btn.activeSelf == false)
        {
            // �����ϱ� ��ư Ȱ��ȭ
            btn.SetActive(true);

            // ģ�� ���� �̴޼� �˾� �˸� ���� �� ȿ���� ���
            Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup/Menu/Friend/NotComplete"));
            Managers.Sound.PlayPopupSound();
        }
    }

    // �����ϱ� �� ���
    void Ex_Like()
    {
        if (clicked) return;
        clicked = true;

        // �� ��� ��� ��
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // �����ϱ� �� ���
        Managers.Web.SendUniRequest("api/todo/" + memberId, "GET", null, (uwr) => {
            // �� ���� json �����͸� ����Ƽ �����ͷ� ��ȯ
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            // ��� ���� ��
            if (response.isSuccess)
            {
                // Debug.Log(response.result);
                clicked = false;
            }
            // ��ū ���� ��
            else if (response.code == 6000)
            {
                clicked = false;
                Managers.Player.SendTokenRequest(Ex_Like);
            }
            // ��Ÿ ���� ��
            else
            {
                Debug.Log(response.message);
                clicked = false;
            }
        }, hN, hV);
    }

    // �ȵ���̵� �ڷΰ��� �̺�Ʈ
    void OnFriendBackTouched(Define.SystemEvent evt)
    {
        if (evt != Define.SystemEvent.Back)
        {
            return;
        }

        Managers.Input.SystemTouchAction = OnBackTouched;

        FindObjectOfType<UI_FriendMain>().DestroyAll();
        Managers.UI.ActiveAllUI();
        Managers.Player.GetPlanet().SetActive(true);
        Destroy(gameObject);
    }

    void OnBackTouched(Define.SystemEvent evt)
    {
        if (evt != Define.SystemEvent.Back)
        {
            return;
        }

        Managers.UI.CloseAppOrUI();
        Managers.UI.ActivePanelUI();

    }

    void Start()
    {
        Init();
    }
}
