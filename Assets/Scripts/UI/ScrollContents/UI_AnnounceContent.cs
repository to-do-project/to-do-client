using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_AnnounceContent : UI_Base
{
    enum Buttons
    {
        Expand_btn,
    }

    enum Texts
    {
        Head_txt,
        Toggle_txt,
    }

    bool toggle = false;
    Text headTxt, toggleTxt;
    GameObject parent;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.Expand_btn).gameObject, ExpandBtnClick, Define.TouchEvent.Touch);

        Bind<Text>(typeof(Texts));
        headTxt = GetText((int)Texts.Head_txt);
        toggleTxt = GetText((int)Texts.Toggle_txt);
        toggleTxt.gameObject.SetActive(toggle);
    }

    public void SetHead(string text)
    {
        headTxt.text = text;
    }
    public void SetSub(string text)
    {
        toggleTxt.text = text;
    }
    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public void ExpandBtnClick(PointerEventData data)
    {
        toggle = !toggle;
        toggleTxt.gameObject.SetActive(toggle);
        SizeRefresh();
        parent.GetComponent<UI_Announce>().SizeRefresh();
    }
    public void SizeRefresh()
    {
        ContentSizeFitter fitter = gameObject.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
    }
}
