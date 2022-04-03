using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
        {
            return null;
        }

        return transform.gameObject;
        //transform �t����
    }

    //��ü�� �ڽĵ� �� ���� �̸��� �ִ���
    //�̸� �Է� ���ϸ� Ÿ�Կ� �ش��ϸ� return
    //��������� ã�� ������ -> �ڽ��� �ڽı��� ã�� ������
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            return null;
        }

        //���� �ڽĸ�
        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (transform.name == name || string.IsNullOrEmpty(name))
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
        }
        //�ڽ��� �ڽı���
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (component.name == name || string.IsNullOrEmpty(name))
                    return component;
            }
        }

        return null;
    }

    //Ư�� ������Ʈ ã�Ƽ� ����, ������ �߰��ؼ� ������
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }


}
