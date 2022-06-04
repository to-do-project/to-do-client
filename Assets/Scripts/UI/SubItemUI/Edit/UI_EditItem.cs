using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EditItem : UI_Base
{
    enum GameObjects
    {
        item_img,
    }

    enum Texts
    {
        count_txt,
    }


    string path;
    Text countTxt;

    int have, total, remain;


    public override void Init()
    {


        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));

        GameObject item = Get<GameObject>((int)GameObjects.item_img);
        countTxt = GetText((int)Texts.count_txt);

        BindEvent(item, ItemClick, Define.TouchEvent.Touch);

        countTxt.text = have.ToString() + "/" + total.ToString();
    }

    void Start()
    {
        Init();
    }

    void ItemClick(PointerEventData data)
    {


        if (remain != 0)
        {
            if (Managers.Player.CountItem(this.gameObject.name)>= have)
            {
                return;
            }
            string name = Util.RemoveCloneString(this.gameObject.name);
            path = "Items/" + name;
            Debug.Log(path);

            GameObject go = Managers.Resource.Instantiate(path, Managers.Player.GetPlanet().transform.GetChild(2).transform);
            Util.FindChild<ItemController>(go, "ItemInner", true).ChangeMode(Define.Scene.Edit);
            Managers.Player.AddItemList(go);
        }
    }

    public void SetText(int totalCount, int remainingCount, int placedCount)
    {
        have = remainingCount+placedCount;
        total = totalCount;
        remain = remainingCount;
    }



}
