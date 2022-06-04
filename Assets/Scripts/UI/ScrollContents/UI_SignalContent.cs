using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_SignalContent : UI_Base, IPointerClickHandler
{
    enum Texts
    {
        Text,
    }

    Text title;
    string type;

    //GameObject parent;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        title = GetText((int)Texts.Text);
    }

    public void SetTitle(string text)
    {
        title.text = text;
    }

    public void SetType(string type)
    {
        this.type = type;
    }

    public void OnPointerClick(PointerEventData data)
    {
        if(type == UI_Signal.SignalType.Announce.ToString()) {
            Managers.UI.ShowPopupUI<UI_Announce>("AnnounceView", "Menu/Setting");
        } else if (type == UI_Signal.SignalType.Request.ToString()) {
            Managers.UI.ShowPopupUI<UI_Friend>("FriendView", "Menu/Friend");
        } else if (type == UI_Signal.SignalType.GroupClear.ToString()) {
            Managers.UI.ClosePopupUI();
        } else if (type == UI_Signal.SignalType.GroupCheer.ToString()) {
            Managers.UI.ClosePopupUI();
        } else if (type == UI_Signal.SignalType.GroupInvite.ToString()) {
            Managers.UI.ClosePopupUI();
        } else {
            Debug.Log("잘못된 타입 >> " + type);
        }
    }

    /*public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public void SizeRefresh()
    {
        ContentSizeFitter fitter = gameObject.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
    }*/
}
