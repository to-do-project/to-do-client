using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditScene : BaseScene
{
    GameObject Planet;
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Edit;

        Planet = GameObject.Find("BluePlanet");
        GameObject go = Managers.Resource.Instantiate("Items/Square", Planet.transform.GetChild(1).transform);

    }

    private void Awake()
    {
        Init();
    }

    void Update()
    {

    }
}
