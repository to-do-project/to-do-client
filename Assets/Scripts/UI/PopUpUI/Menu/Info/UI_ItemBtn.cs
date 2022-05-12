using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemBtn : MonoBehaviour
{
    bool isCharItem;
    string itemName;
    int price;
    int curHave;
    int maxHave;

    public void InitButton(bool isCharItem, string itemName, int price, int curHave, int maxHave)
    {
        this.isCharItem = isCharItem;
        this.itemName = itemName;
        this.price = price;
        this.curHave = curHave;
        this.maxHave = maxHave;
    }
}
