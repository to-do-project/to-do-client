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
        //transform 퍁어줌
    }

    //객체의 자식들 중 같은 이름이 있는지
    //이름 입력 안하면 타입에 해당하면 return
    //재귀적으로 찾을 것인지 -> 자식의 자식까지 찾을 것인지
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            return null;
        }

        //직속 자식만
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
        //자식의 자식까지
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

    //특정 컴포넌트 찾아서 리턴, 없으면 추가해서 돌려줌
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
