using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager 
{
    //형식 지정해서 load
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    //Instantiate
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab :{path}");
        }

        return Object.Instantiate(prefab, parent); //Object에 있는 instatiate 사용
    }

    //위치 지정 Instantiate
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

    //오브젝트 파괴
    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }
        Object.Destroy(go);
    }
}
