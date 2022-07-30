using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 로딩 화면을 제어하는 스크립트
public class UI_Load : MonoBehaviour, IPointerClickHandler
{
    // 싱글톤화
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

    // 싱글톤 오브젝트가 없을 경우 오브젝트 생성
    static UI_Load Init()
    {
        return Instantiate(Resources.Load<UI_Load>("Prefabs/UI/LoadUI/LoadView"));
    }

    [SerializeField]
    CanvasGroup canvasGroup; // alpha값 조정을 위한 CanvasGroup
    GameObject loadTxt; // 로딩 텍스트 오브젝트

    // 클릭 가능 여부, Fade 실행 중 여부 체크 변수
    bool canClick = true, canFade = true;
    // 로드할 씬 이름
    string loadSceneName;

    // 초기화
    void Awake()
    {
        // 싱글톤이 자기 자신이 아닌 경우 삭제
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // 카메라 설정
        CameraSet();

        // 텍스트 오브젝트 탐색
        loadTxt=transform.Find("LoadText").gameObject;

        SceneManager.sceneLoaded += (arg0, arg1) => { CameraSet(); canClick = true; };

        // 씬 로드 후에도 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    // 클릭이 필요 한 로딩 화면을 호출하는 함수 (텍스트 활성화)
    // sceneName = 로드 할 씬 이름
    public void ToLoad(string sceneName)
    {
        // 텍스트 활성화
        loadTxt.SetActive(true);
        // 로딩 종료 시까지 클릭 불가
        canClick = false;

        // 오브젝트 활성화, alpha값 0으로 초기화 및 씬 이름 저장
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        loadSceneName = sceneName;

        // 로딩 시작
        StartCoroutine(ToLoadSceneProcess());
    }

    // 클릭이 필요 없는 로딩 화면을 호출하는 함수 (텍스트 비활성화)
    // sceneName = 로드 할 씬 이름
    public void InstantLoad(string sceneName)
    {
        // 텍스트 비활성화
        loadTxt.SetActive(false);
        // 클릭 불가
        canClick = false;

        // 오브젝트 활성화, alpha값 0으로 초기화 및 씬 이름 저장
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        loadSceneName = sceneName;

        // 로딩 시작
        StartCoroutine(InstantLoadSceneProcess());
    }

    // 로드 완료 시 호출 함수
    public void CompleteLoad()
    {
        canFade = true;
    }

    // 로드화면에서 실제 화면으로 넘어가는 함수
    void ExLoad()
    {
        // 로드 종료시 까지 클릭 불가
        canClick = false;

        // 오브젝트 활성화 및 로드 완료시 이벤트 추가
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 로딩 시작
        StartCoroutine(ExLoadSceneProcess());
    }

    // 씬이 로드 완료되면 호출되는 함수
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 로드가 제대로 되었는지 확인
        if(scene.name == loadSceneName)
        {
            // 페이드 아웃 및 이벤트 제거(중복 방지)
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // 기존 씬 -> 로딩 화면으로 이동해주는 코루틴
    IEnumerator ToLoadSceneProcess()
    {
        // 로딩 시작
        AsyncOperation op = SceneManager.LoadSceneAsync(Define.Scene.Loading.ToString());
        op.allowSceneActivation = false;

        // FadeIn 중에 로딩이 완료되지 않도록 조정
        canFade = true;
        yield return StartCoroutine(Fade(true));

        // FadeIn이 완료되면 씬 로드
        op.allowSceneActivation = true;
        yield break;
    }

    // 로딩 화면 -> 가고싶은 씬으로 이동해주는 코루틴
    IEnumerator ExLoadSceneProcess()
    {
        // 로딩 시작
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        // 로딩 종료까지 대기
        while (op.isDone == false)
        {
            yield return null;
        }
        // 로딩 종료 알림
        if (loadSceneName == Define.Scene.Main.ToString()) canFade = false;
    }

    // 터치 없이 즉시 로드하는 함수
    IEnumerator InstantLoadSceneProcess()
    {
        // 기존 씬 -> 로딩 화면 (Fade In)
        yield return StartCoroutine(ToLoadSceneProcess());
        // 씬 로드 완료 이벤트 등록 (Fade Out)
        SceneManager.sceneLoaded += OnSceneLoaded;
        // 로딩 화면 -> 가고싶은 씬
        yield return StartCoroutine(ExLoadSceneProcess());
    }

    // Fade In / Out 함수
    // isFadeIn = true시 FadeIn, false시 FadeOut
    IEnumerator Fade(bool isFadeIn)
    {
        // canFade가 true가 될 때까지 대기
        while (canFade == false)
        {
            yield return null;
        }

        // Fade In / Out 시작 전 딜레이 시간 (씬 로드로 인한 버벅임 방지)
        float time = 0.5f;
        while (time >= 0 && !isFadeIn)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }
        
        // Fade In / Out 실행
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

    // 카메라 설정
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

    // 클릭 시 이벤트 || 로드 시작
    public void OnPointerClick(PointerEventData data)
    {
        if(canClick)
        {
            ExLoad();
        }
    }
}
