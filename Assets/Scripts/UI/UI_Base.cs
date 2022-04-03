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
                Debug.Log($"{type.Name} failed to bind{names[i]}");
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
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected GameObject getObject(int idx) { return Get<GameObject>(idx); }

    //�̺�Ʈ �ڵ鷯�� �̺�Ʈ ����
    //UI�̺�Ʈ Ÿ�Կ� ����
    //�̺�Ʈ �ڵ鷯�� UI�� ���̰�, ������ �Լ��� ����������
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);
        //type�� ���� ����

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }


        evt.OnDragHandler += ((PointerEventData data) => { go.transform.position = data.position; });
    }
}
