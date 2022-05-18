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

[System.Serializable]
public class Response<T>    // 서버에서 받은 json파일을 FromJson<Response<T>>로 변환해 받아옵니다
{
    public bool isSuccess;  // 성공 여부
    public int code;        // 오류 코드
    public string message;  // 오류 메세지
    public T result;        // 서버에서 받은 데이터 클래스 (실 데이터)
}
