using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    //Enum���� �Ѱ��ָ� �̸����� ã�Ƽ� �˾Ƽ� �����ϰ�
    //���� �ڵ�ȭ
    //reflextion �̿�
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }

            if (objects[i] == null)
            {
                Debug.Log($"{type.Name} failed to bind {names[i]}");
            }
        }
    }

    //���ε��ص� ������Ʈ���� ���ϴ� ������Ʈ ã�Ƽ� ��������
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
        {
            return null;
        }

        return objects[idx] as T;

    }

    //Ÿ�Ժ��� Get �Լ� ������(���� ���϶��)
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected InputField GetInputfiled(int idx) { return Get<InputField>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected GameObject getObject(int idx) { return Get<GameObject>(idx); }


    protected void SetCanvasCamera()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();

        if ((UIcam == null) || (UIcam != cam))
        {
            canvas.worldCamera = cam;
        }

        /*        Canvas canvas = GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                Camera UIcam = canvas.worldCamera;
                if (UIcam == null)
                {
                    Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
                    canvas.worldCamera = cam;
                }
                else if(UIcam != cam)
                {
                    Debug.Log($"{UIcam.name}");
                }*/

        /*        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();

        if (UIcam != cam)
        {
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }*/
    }

    //�̺�Ʈ �ڵ鷯�� �̺�Ʈ ����
    //UI�̺�Ʈ Ÿ�Կ� ����
    //�̺�Ʈ �ڵ鷯�� UI�� ���̰�, ������ �Լ��� ����������
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.TouchEvent type = Define.TouchEvent.Touch)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);
        //type�� ���� ����

        switch (type)
        {
            case Define.TouchEvent.Touch:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.TouchEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }

    }

    public static void ClearEvent(GameObject go, Action<PointerEventData> action, Define.TouchEvent type = Define.TouchEvent.Touch)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);
        switch (type)
        {
            case Define.TouchEvent.Touch:
                evt.OnClickHandler -= action;
                break;
            case Define.TouchEvent.Drag:
                evt.OnDragHandler -= action;
                break;
        }
    }

    public void showToastMessage(GameObject toastMessage, Text toast, string msg, float time, bool closePopup = false)
    {
        Debug.Log("show toast message");
        StartCoroutine(showToastMessageCoroutine(toastMessage,toast, msg, time, closePopup));
    }

    private IEnumerator showToastMessageCoroutine(GameObject toastMessage, Text toast, string msg, float time, bool closePopup = false)
    {
        Debug.Log("in show toast message");
        toastMessage.SetActive(true);
        toast.text = msg;

        yield return fadeInOut(toastMessage.GetComponent<CanvasGroup>(), 0.3f, true);

        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return fadeInOut(toastMessage.GetComponent<CanvasGroup>(), 0.3f, false);

        toast.text = "";
        toastMessage.SetActive(false);
        if (closePopup)
        {
            Managers.UI.ClosePopupUI();
        }
    }


    private IEnumerator fadeInOut(CanvasGroup target, float durationTime, bool inOut)
    {
        float start, end;
        if (inOut)
        {
            start = 0.0f;
            end = 1.0f;
        }
        else
        {
            start = 1.0f;
            end = 0f;
        }

        //Color current = Color.clear; /* (0, 0, 0, 0) = ������ ����, ���� 100% */
        float elapsedTime = 0.0f;

        while (elapsedTime < durationTime)
        {
            float alpha = Mathf.Lerp(start, end, elapsedTime / durationTime);
            target.alpha = alpha;
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}
