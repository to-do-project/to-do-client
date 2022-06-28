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
    bool canFade = true;

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
        canvasGroup.alpha = 0;
        loadSceneName = sceneName;
        StartCoroutine(ToLoadSceneProcess());
    }

    public void InstantLoad(string sceneName)
    {
        canClick = false;
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        loadSceneName = sceneName;
        StartCoroutine(InstantLoadSceneProcess());
    }
    public void CompleteLoad()
    {
        canFade = true;
    }

    void ExLoad()    // 실제 가고싶은 씬 로드
    {
        canClick = false;
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(ExLoadSceneProcess());
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
        AsyncOperation op = SceneManager.LoadSceneAsync(Define.Scene.Loading.ToString());
        op.allowSceneActivation = false;

        canFade = true;
        yield return StartCoroutine(Fade(true));
        op.allowSceneActivation = true;
        yield break;
    }

    private IEnumerator ExLoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        while (op.isDone == false)
        {
            yield return null;
        }
        if (loadSceneName == Define.Scene.Main.ToString()) canFade = false;
    }

    private IEnumerator InstantLoadSceneProcess()
    {
        yield return StartCoroutine(ToLoadSceneProcess());
        SceneManager.sceneLoaded += OnSceneLoaded;
        yield return StartCoroutine(ExLoadSceneProcess());
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        while(canFade == false)
        {
            yield return null;  // canFade가 true가 될 때까지 대기
        }

        float time = 0.5f;
        while (time >= 0 && !isFadeIn)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }

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
