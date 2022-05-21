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
        //토큰 확인


        PlanetInstantiate();
        ItemInstantiate();
    }

    //행성 생성
    private void PlanetInstantiate()
    {
        //blue,green,red 중 무엇인지 확인 후 해당 행성 생성
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
