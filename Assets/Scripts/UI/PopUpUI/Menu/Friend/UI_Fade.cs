using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Fade : MonoBehaviour
{
    Canvas canvas;              // Fade In, Out 하고 싶은 UI에 붙여서 사용
    CanvasGroup canvasGroup;

    float time = 0.5f;
    float delay = 3;
    float curDelay = 0;

    void Start()
    {
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

    void Update()
    {
        curDelay += Time.deltaTime;
        if(curDelay > delay)
        {
            curDelay = -100;
            StartCoroutine(FadeOut(time));
        }
    }

    public IEnumerator FadeIn(float time)
    {
        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }
    public IEnumerator FadeOut(float time)
    {
        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
