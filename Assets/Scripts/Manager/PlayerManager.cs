using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager 
{
    GameObject planet;

    void Start()
    {
        Init();
    }

    void Init()
    {
        //��ū Ȯ��


        PlanetInstantiate();
        ItemInstantiate();
    }

    //�༺ ����
    private void PlanetInstantiate()
    {
        //blue,green,red �� �������� Ȯ�� �� �ش� �༺ ����
    }

    private void ItemInstantiate()
    {

    }

    public void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }
}
