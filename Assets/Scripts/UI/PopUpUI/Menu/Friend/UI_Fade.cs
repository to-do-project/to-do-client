using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fade In, Out 하고 싶은 UI에 붙여서 사용
public class UI_Fade : MonoBehaviour
{
    Canvas canvas;           // 캔버스 컴포넌트
    CanvasGroup canvasGroup; // 캔버스 그룹 컴포넌트

    // time = Fade 시간, delay = 오브젝트 유지 시간, curDelay = 시간 계산
    float time = 0.5f, delay = 3, curDelay = 0;

    void Start()
    {
        // 변수 초기화 및 카메라 초기화
        canvas = GetComponent<Canvas>();
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

        canvas.sortingOrder = 100;
        canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(FadeIn(time));
    }

    // curDelay에 시간 저장
    void Update()
    {
        curDelay += Time.deltaTime;
        if(curDelay > delay)
        {
            curDelay = -100;
            StartCoroutine(FadeOut(time));
        }
    }

    // FadeIn 코루틴
    // time = FadeIn 시간
    public IEnumerator FadeIn(float time)
    {
        // CanvasGroup의 alpha값을 0에서 1으로 시간에 따라 조정
        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    // FadeOut 코루틴
    // time = FadeOut 시간
    public IEnumerator FadeOut(float time)
    {
        // CanvasGroup의 alpha값을 1에서 0으로 시간에 따라 조정
        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
