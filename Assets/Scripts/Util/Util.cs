using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

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

    public static string RemoveCloneString(string Oname)
    {
        string name = Oname;
        int index = Oname.IndexOf("(Clone)");
        if (index > 0)
        {
            name = Oname.Substring(0, index);
        }

        return name;
    }

    //���ӿ�����Ʈ�� �ڽ��� ��� ����
    public static void RemoveAllChild(GameObject go)
    {
        Transform[] childList = go.GetComponentsInChildren<Transform>();

        if (childList != null)
        {
            foreach (Transform child in childList)
            {
                if (child != go.transform)
                {
                    Managers.Resource.Destroy(child.gameObject);
                }
            }
        }
    }

    //���ӿ�����Ʈ�� �ڽ��� ��� ����(Ư�� ���� T�� ���� �ڽĸ�)
    public static void RemoveAllChild<T>(GameObject go) where T : UnityEngine.Component
    {
        T[] childList = go.GetComponentsInChildren<T>();

        if (childList != null)
        {
            foreach (T child in childList)
            {
                if (child.gameObject != go.gameObject)
                {
                    Managers.Resource.Destroy(child.gameObject);
                }
            }
        }
    }


    //string valid �Լ�
    //pattern�� �˻��� ����ǥ���� �ۼ�
    public static bool IsValidString(string word, string pattern)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            return false;
        }
        try
        {
            return Regex.IsMatch(word, pattern,
                RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
