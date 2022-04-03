using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{

    public void Init()
    {

    }

    // Loader -> T type
    //Json 파일 경로 전달해주면
    //제이슨 파일을 지정한 클래스 형식으로 변환
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset testAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(testAsset.text);
    }
}
