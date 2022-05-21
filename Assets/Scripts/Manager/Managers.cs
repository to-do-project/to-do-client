using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 매니저 총괄 관리
/// 자기자신을 포함해서 모든 매니저 인스턴스 싱글톤으로 관리
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
    PlayerManager player = new PlayerManager();
    static WebManager web;

    public static InputManager Input { get { return Instance().input; } }
    public static ResourceManager Resource { get { return Instance().resource; } }
    public static UIManager UI { get { return Instance().ui; } }
    public static SceneManagerEx Scene { get { return Instance().scene; } }
    public static SoundManager Sound { get { return Instance().sound; } }
    public static DataManager Data { get { return Instance().data; } }
    public static WebManager Web { get { return web; } }
    public static PlayerManager Player { get { return Player; } }
    

    void Start()
    {
        Init();
    }

    private void Update()
    {
        Input.OnUpdate();
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


            //다른 매니저들 Init도 여기서 해주기
            instance.data.Init();
            instance.sound.Init();

            if (web.InternetCheck())
            {
                Debug.Log("no internet");
            }
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Input.Clear();
    }

}
