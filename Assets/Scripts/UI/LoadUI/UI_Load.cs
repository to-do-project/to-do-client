using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_Load : MonoBehaviour, IPointerClickHandler
{
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

    bool canClick = true;

    [SerializeField]
    CanvasGroup canvasGroup;

    string loadSceneName;

    static UI_Load Init()
    {
        return Instantiate(Resources.Load<UI_Load>("Prefabs/UI/LoadUI/LoadView"));
    }

    void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        CameraSet();
        SceneManager.sceneLoaded += (arg0, arg1) => { CameraSet(); canClick = true; };
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    // 로딩 화면을 호출하는 함수
    public void ToLoad(string sceneName)    // sceneName에 로드 할 씬 이름 입력
    {
        canClick = false;
        gameObject.SetActive(true);
        loadSceneName = sceneName;
        StartCoroutine(ToLoadSceneProcess());
    }

    public void ExLoad()    // 실제 가고싶은 씬 로드
    {
        canClick = false;
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(ExLoadSceneProcess());
    }

    public void ExLoad(string sceneName)    // 특수 코드. ToLoad 없이 바로 로드
    {
        canClick = false;
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private IEnumerator ToLoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("Loading");
        op.allowSceneActivation = false;
        yield return StartCoroutine(Fade(true));
        op.allowSceneActivation = true;
        yield break;
    }

    private IEnumerator ExLoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        yield break;
    }

    private IEnumerator Fade(bool isFadeIn)
    {
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

    public void OnPointerClick(PointerEventData data)
    {
        if(canClick)
        {
            ExLoad();
        }
    }
}
