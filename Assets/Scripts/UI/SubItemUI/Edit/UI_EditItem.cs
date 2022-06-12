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

    int total, remain, placed;


    public override void Init()
    {


        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));

        GameObject item = Get<GameObject>((int)GameObjects.item_img);
        countTxt = GetText((int)Texts.count_txt);

        BindEvent(item, ItemClick, Define.TouchEvent.Touch);

        countTxt.text = remain.ToString() + "/" + total.ToString();
    }

    void Start()
    {
        Init();
    }

    void ItemClick(PointerEventData data)
    {
        //total remainÀ¸·Î ¹Ù²ã¾ßÛ¤±
        if (Managers.Player.CountItem(this.gameObject.name) >= total)
        {
            Debug.Log(Managers.Player.CountItem(this.gameObject.name) +" "+total);
            return;
        }
        string name = Util.RemoveCloneString(this.gameObject.name);
        path = "Items/" + name;
        Debug.Log(path);

        GameObject go = Managers.Resource.Instantiate(path, Managers.Player.GetPlanet().transform.GetChild(2).transform);
        ItemController ic = Util.FindChild<ItemController>(go, "ItemInner", true);

        ic.ChangeMode(Define.Scene.Edit);
        ic.ChangeFixState(true);


        Managers.Player.AddItemList(go);
    }

    public void SetText(int totalCount, int remainingCount, int placedCount)
    {
        total = totalCount;
        remain = remainingCount;
        placed = placedCount;
    }



}
