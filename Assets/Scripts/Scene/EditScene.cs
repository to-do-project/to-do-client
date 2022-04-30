using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditScene : BaseScene
{
    GameObject Planet;
    GameObject EditItem, EditItem2;


    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Edit;

        Planet = GameObject.Find("BluePlanet");
        EditItem2 = Managers.Resource.Instantiate("Items/Square", Planet.transform.GetChild(1).transform);
        EditItem = Managers.Resource.Instantiate("Items/Square1",Planet.transform.GetChild(1).transform);

        ChangeItemMode(EditItem);
        ChangeItemMode(EditItem2);
    }

    private void Awake()
    {
        Init();
    }

    void Update()
    {

    }

    private void ChangeItemMode(GameObject go)
    {
        ItemController child = Util.FindChild<ItemController>(go, "ItemInner", true);
        child.ChangeMode(SceneType);
    }
}
