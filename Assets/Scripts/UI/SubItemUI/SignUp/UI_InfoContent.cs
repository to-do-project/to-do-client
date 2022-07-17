using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_InfoContent : UI_Base
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

    bool toggle = false;        // ������ ���/��â�� ���� ��� ����
    Text toggleTxt;    // ���� �ؽ�Ʈ, ���� �ؽ�Ʈ
    GameObject parent;          // �θ� ������Ʈ

    public override void Init() // �ʱ�ȭ
    {
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.Expand_btn).gameObject, ExpandBtnClick, Define.TouchEvent.Touch);  // ��ư �̺�Ʈ ����

        Bind<Text>(typeof(Texts));
        toggleTxt = GetText((int)Texts.Toggle_txt); // ���� �ؽ�Ʈ ����
        toggleTxt.gameObject.SetActive(toggle);     // ��ۿ� ���� ���� Ű�� ����

        SizeRefresh();
    }
    void Start()
    {
        Init();
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
        parent.GetComponent<UI_InfoScroll>().SizeRefresh();   // �θ� ����� ����
    }


    public void SizeRefresh() // ������ ����
    {
        ContentSizeFitter fitter = gameObject.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
    }
}
