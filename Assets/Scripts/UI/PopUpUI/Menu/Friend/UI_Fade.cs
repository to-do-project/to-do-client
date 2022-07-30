using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fade In, Out �ϰ� ���� UI�� �ٿ��� ���
public class UI_Fade : MonoBehaviour
{
    Canvas canvas;           // ĵ���� ������Ʈ
    CanvasGroup canvasGroup; // ĵ���� �׷� ������Ʈ

    // time = Fade �ð�, delay = ������Ʈ ���� �ð�, curDelay = �ð� ���
    float time = 0.5f, delay = 3, curDelay = 0;

    void Start()
    {
        // ���� �ʱ�ȭ �� ī�޶� �ʱ�ȭ
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

    // curDelay�� �ð� ����
    void Update()
    {
        curDelay += Time.deltaTime;
        if(curDelay > delay)
        {
            curDelay = -100;
            StartCoroutine(FadeOut(time));
        }
    }

    // FadeIn �ڷ�ƾ
    // time = FadeIn �ð�
    public IEnumerator FadeIn(float time)
    {
        // CanvasGroup�� alpha���� 0���� 1���� �ð��� ���� ����
        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    // FadeOut �ڷ�ƾ
    // time = FadeOut �ð�
    public IEnumerator FadeOut(float time)
    {
        // CanvasGroup�� alpha���� 1���� 0���� �ð��� ���� ����
        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
