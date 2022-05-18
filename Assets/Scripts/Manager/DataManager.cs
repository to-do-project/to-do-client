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
    //Json ���� ��� �������ָ�
    //���̽� ������ ������ Ŭ���� �������� ��ȯ
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset testAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(testAsset.text);
    }
}

[System.Serializable]
public class Response<T>    // �������� ���� json������ FromJson<Response<T>>�� ��ȯ�� �޾ƿɴϴ�
{
    public bool isSuccess;  // ���� ����
    public int code;        // ���� �ڵ�
    public string message;  // ���� �޼���
    public T result;        // �������� ���� ������ Ŭ���� (�� ������)
}
