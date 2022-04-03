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

    ResourceManager resource = new ResourceManager();
    UIManager ui = new UIManager();
    SceneManagerEx scene = new SceneManagerEx();
    SoundManager sound = new SoundManager();
    DataManager data = new DataManager();

    public static ResourceManager Resource { get { return Instance().resource; } }
    public static UIManager UI { get { return Instance().ui; } }
    public static SceneManagerEx Scene { get { return Instance().scene; } }
    public static SoundManager Sound { get { return Instance().sound; } }
    public static DataManager Data { get { return Instance().data; } }

    void Start()
    {
        Init();
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

            //�ٸ� �Ŵ����� Init�� ���⼭ ���ֱ�
            instance.data.Init();
            instance.sound.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
    }
}
