using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// �ε� ȭ���� �����ϴ� ��ũ��Ʈ
public class UI_Load : MonoBehaviour, IPointerClickHandler
{
    // �̱���ȭ
    private static UI_Load instance;
    public static UI_Load Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<UI_Load>();
                if(obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Init();
                }
            }
            return instance;
        }
    }

    // �̱��� ������Ʈ�� ���� ��� ������Ʈ ����
    static UI_Load Init()
    {
        return Instantiate(Resources.Load<UI_Load>("Prefabs/UI/LoadUI/LoadView"));
    }

    [SerializeField]
    CanvasGroup canvasGroup; // alpha�� ������ ���� CanvasGroup
    GameObject loadTxt; // �ε� �ؽ�Ʈ ������Ʈ

    // Ŭ�� ���� ����, Fade ���� �� ���� üũ ����
    bool canClick = true, canFade = true;
    // �ε��� �� �̸�
    string loadSceneName;

    // �ʱ�ȭ
    void Awake()
    {
        // �̱����� �ڱ� �ڽ��� �ƴ� ��� ����
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // ī�޶� ����
        CameraSet();

        // �ؽ�Ʈ ������Ʈ Ž��
        loadTxt=transform.Find("LoadText").gameObject;

        SceneManager.sceneLoaded += (arg0, arg1) => { CameraSet(); canClick = true; };

        // �� �ε� �Ŀ��� �ı����� �ʵ��� ����
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    // Ŭ���� �ʿ� �� �ε� ȭ���� ȣ���ϴ� �Լ� (�ؽ�Ʈ Ȱ��ȭ)
    // sceneName = �ε� �� �� �̸�
    public void ToLoad(string sceneName)
    {
        // �ؽ�Ʈ Ȱ��ȭ
        loadTxt.SetActive(true);
        // �ε� ���� �ñ��� Ŭ�� �Ұ�
        canClick = false;

        // ������Ʈ Ȱ��ȭ, alpha�� 0���� �ʱ�ȭ �� �� �̸� ����
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        loadSceneName = sceneName;

        // �ε� ����
        StartCoroutine(ToLoadSceneProcess());
    }

    // Ŭ���� �ʿ� ���� �ε� ȭ���� ȣ���ϴ� �Լ� (�ؽ�Ʈ ��Ȱ��ȭ)
    // sceneName = �ε� �� �� �̸�
    public void InstantLoad(string sceneName)
    {
        // �ؽ�Ʈ ��Ȱ��ȭ
        loadTxt.SetActive(false);
        // Ŭ�� �Ұ�
        canClick = false;

        // ������Ʈ Ȱ��ȭ, alpha�� 0���� �ʱ�ȭ �� �� �̸� ����
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        loadSceneName = sceneName;

        // �ε� ����
        StartCoroutine(InstantLoadSceneProcess());
    }

    // �ε� �Ϸ� �� ȣ�� �Լ�
    public void CompleteLoad()
    {
        canFade = true;
    }

    // �ε�ȭ�鿡�� ���� ȭ������ �Ѿ�� �Լ�
    void ExLoad()
    {
        // �ε� ����� ���� Ŭ�� �Ұ�
        canClick = false;

        // ������Ʈ Ȱ��ȭ �� �ε� �Ϸ�� �̺�Ʈ �߰�
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;

        // �ε� ����
        StartCoroutine(ExLoadSceneProcess());
    }

    // ���� �ε� �Ϸ�Ǹ� ȣ��Ǵ� �Լ�
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �ε尡 ����� �Ǿ����� Ȯ��
        if(scene.name == loadSceneName)
        {
            // ���̵� �ƿ� �� �̺�Ʈ ����(�ߺ� ����)
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // ���� �� -> �ε� ȭ������ �̵����ִ� �ڷ�ƾ
    IEnumerator ToLoadSceneProcess()
    {
        // �ε� ����
        AsyncOperation op = SceneManager.LoadSceneAsync(Define.Scene.Loading.ToString());
        op.allowSceneActivation = false;

        // FadeIn �߿� �ε��� �Ϸ���� �ʵ��� ����
        canFade = true;
        yield return StartCoroutine(Fade(true));

        // FadeIn�� �Ϸ�Ǹ� �� �ε�
        op.allowSceneActivation = true;
        yield break;
    }

    // �ε� ȭ�� -> ������� ������ �̵����ִ� �ڷ�ƾ
    IEnumerator ExLoadSceneProcess()
    {
        // �ε� ����
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        // �ε� ������� ���
        while (op.isDone == false)
        {
            yield return null;
        }
        // �ε� ���� �˸�
        if (loadSceneName == Define.Scene.Main.ToString()) canFade = false;
    }

    // ��ġ ���� ��� �ε��ϴ� �Լ�
    IEnumerator InstantLoadSceneProcess()
    {
        // ���� �� -> �ε� ȭ�� (Fade In)
        yield return StartCoroutine(ToLoadSceneProcess());
        // �� �ε� �Ϸ� �̺�Ʈ ��� (Fade Out)
        SceneManager.sceneLoaded += OnSceneLoaded;
        // �ε� ȭ�� -> ������� ��
        yield return StartCoroutine(ExLoadSceneProcess());
    }

    // Fade In / Out �Լ�
    // isFadeIn = true�� FadeIn, false�� FadeOut
    IEnumerator Fade(bool isFadeIn)
    {
        // canFade�� true�� �� ������ ���
        while (canFade == false)
        {
            yield return null;
        }

        // Fade In / Out ���� �� ������ �ð� (�� �ε�� ���� ������ ����)
        float time = 0.5f;
        while (time >= 0 && !isFadeIn)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }
        
        // Fade In / Out ����
        float timer = 0f;
        while(timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }
        if(!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }

    // ī�޶� ����
    void CameraSet()
    {
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

    // Ŭ�� �� �̺�Ʈ || �ε� ����
    public void OnPointerClick(PointerEventData data)
    {
        if(canClick)
        {
            ExLoad();
        }
    }
}
