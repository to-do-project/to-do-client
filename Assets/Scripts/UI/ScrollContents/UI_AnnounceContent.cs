using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_AnnounceContent : UI_Base
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
    enum Buttons
    {
        Expand_btn,
    }

    enum Texts
    {
        Head_txt,
        Toggle_txt,
    }
    // ================================ //

    bool toggle = false;        // ������ ���/��â�� ���� ��� ����
    Text headTxt, toggleTxt;    // ���� �ؽ�Ʈ, ���� �ؽ�Ʈ
    GameObject parent;          // �θ� ������Ʈ

    public override void Init() // �ʱ�ȭ
    {
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.Expand_btn).gameObject, ExpandBtnClick, Define.TouchEvent.Touch);  // ��ư �̺�Ʈ ����

        Bind<Text>(typeof(Texts));
        headTxt = GetText((int)Texts.Head_txt);     // ���� �ؽ�Ʈ ����
        toggleTxt = GetText((int)Texts.Toggle_txt); // ���� �ؽ�Ʈ ����
        toggleTxt.gameObject.SetActive(toggle);     // ��ۿ� ���� ���� Ű�� ����
    }

    public void SetHead(string text)    // ���� �ؽ�Ʈ ����
    {
        headTxt.text = text;
    }
    public void SetSub(string text)     // ���� �ؽ�Ʈ ����
    {
        toggleTxt.text = text;
    }
    public void SetParent(GameObject parent)    // �θ� ����
    {
        this.parent = parent;
    }

    public void ExpandBtnClick(PointerEventData data)   // ��ư �̺�Ʈ
    {
        toggle = !toggle;   // ��� ����
        toggleTxt.gameObject.SetActive(toggle); // ��ۿ� ���� ���� Ű�� ����
        SizeRefresh();      // ������ ����
        parent.GetComponent<UI_Announce>().SizeRefresh();   // �θ� ����� ����
    }
    public void SizeRefresh() // ������ ����
    {
        ContentSizeFitter fitter = gameObject.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
    }
}
