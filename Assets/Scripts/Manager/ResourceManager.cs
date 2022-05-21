using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager 
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab :{path}");
        }

        return Object.Instantiate(prefab, parent); //Object에 있는 instatiate 사용
    }

    //위치 지정
    public GameObject Instantiate(Vector3 pos, string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab :{path}");
        }
        GameObject go = Object.Instantiate(prefab, parent);
        go.transform.position = pos;

        return go; //Object에 있는 instatiate 사용
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }
        Object.Destroy(go);
    }
}
