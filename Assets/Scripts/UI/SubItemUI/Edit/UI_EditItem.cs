using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EditItem : UI_Base
{
    enum GameObjects
    {
        item_img,
    }

    string path;
    GameObject planet;

    public override void Init()
    {
        planet = GameObject.Find("BluePlanet");
        Bind<GameObject>(typeof(GameObjects));
        GameObject item = Get<GameObject>((int)GameObjects.item_img);

        BindEvent(item, ItemClick, Define.TouchEvent.Touch);
    }

    void Start()
    {
        Init();
    }

    void ItemClick(PointerEventData data)
    {
        string name=this.gameObject.name;
        int index = this.gameObject.name.IndexOf("(Clone)");
        if (index > 0)
        {
            name = this.gameObject.name.Substring(0, index);
        }
        path = "Items/"+name;
        Debug.Log(path);
        
        GameObject go = Managers.Resource.Instantiate(path,planet.transform.GetChild(2).transform);
        Util.FindChild<ItemController>(go, "ItemInner", true).ChangeMode(Define.Scene.Edit);
    }


}
