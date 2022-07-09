using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� �Ŵ��� �Ѱ� ����
/// �ڱ��ڽ��� �����ؼ� ��� �Ŵ��� �ν��Ͻ� �̱������� ����
/// </summary>
public class Managers : MonoBehaviour
{
    static Managers instance;
    public static Managers Instance()
    {
        Init();
        return instance;
    }

    InputManager input = new InputManager();
    ResourceManager resource = new ResourceManager();
    UIManager ui = new UIManager();
    SceneManagerEx scene = new SceneManagerEx();
    SoundManager sound = new SoundManager();
    DataManager data = new DataManager();
    TodoManager todo = new TodoManager();
    static PlayerManager player;
    static WebManager web;

    bool internetFlag;

    public static InputManager Input { get { return Instance().input; } }
    public static ResourceManager Resource { get { return Instance().resource; } }
    public static UIManager UI { get { return Instance().ui; } }
    public static SceneManagerEx Scene { get { return Instance().scene; } }
    public static SoundManager Sound { get { return Instance().sound; } }
    public static DataManager Data { get { return Instance().data; } }
    public static TodoManager Todo { get { return Instance().todo; } }
    public static WebManager Web { get { Instance(); return web; } }
    public static PlayerManager Player { get { Instance(); return player; } }
    

    void Start()
    {
        Init();
        internetFlag = true;
    }

    private void Update()
    {
        Input.OnUpdate();
        if (!Web.InternetCheck())
        {
            //�ε� ������ ���ư���
            if (internetFlag)
            {
                UI.ShowPopupUI<UI_Internet>("InternetView");
                internetFlag = false;
            }

        }
        else
        {
            if (!internetFlag)
            {
                internetFlag = true;
                Scene.LoadScene(Define.Scene.Start);
            }
        }
    }

    static void Init()
    {
        if(instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<Managers>();

            web = go.AddComponent<WebManager>();
            player = go.AddComponent<PlayerManager>();

            //�ٸ� �Ŵ����� Init�� ���⼭ ���ֱ�
            instance.data.Init();
            instance.sound.Init();
            instance.todo.Init();

/*
            if (web.InternetCheck())
            {
                Debug.Log("no internet");
            }*/
        }
    }

    public static void Clear()
    {
        //Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Input.Clear();
    }

}
