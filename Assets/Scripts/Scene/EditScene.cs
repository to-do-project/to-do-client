using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditScene : BaseScene
{
    //GameObject Planet;
    //GameObject EditItem,EditItem1, EditItem2;

    public string category;

    public override void Clear()
    {
        //Managers.UI.Clear();
        //throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Edit;

        Managers.Input.SystemTouchAction -= OnBackTouched;
        Managers.Input.SystemTouchAction += OnBackTouched;


        //Planet = GameObject.Find("BluePlanet");
        //EditItem1 = Managers.Resource.Instantiate("Items/portal_00", Planet.transform.GetChild(2).transform);
        //EditItem2 = Managers.Resource.Instantiate("Items/plant_01", Planet.transform.GetChild(2).transform);
        //EditItem = Managers.Resource.Instantiate(new Vector3(3.2f, 0.9f,0f), "Items/portal_00", Planet.transform.GetChild(2).transform);


        //ChangeItemMode(EditItem);
        //ChangeItemMode(EditItem1);
        //ChangeItemMode(EditItem2);


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

    void OnBackTouched(Define.SystemEvent evt)
    {
        Managers.UI.ShowPopupUI<UI_ExitEdit>("ExitEditView","Edit");
    }
}
