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

    bool toggle = false;        // 컨텐츠 축소/팽창을 위한 토글 변수
    Text toggleTxt;    // 제목 텍스트, 내용 텍스트
    GameObject parent;          // 부모 오브젝트

    public override void Init() // 초기화
    {
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.Expand_btn).gameObject, ExpandBtnClick, Define.TouchEvent.Touch);  // 버튼 이벤트 연결

        Bind<Text>(typeof(Texts));
        toggleTxt = GetText((int)Texts.Toggle_txt); // 내용 텍스트 연결
        toggleTxt.gameObject.SetActive(toggle);     // 토글에 따라 내용 키고 끄기

        SizeRefresh();
    }
    void Start()
    {
        Init();
    }

    public void SetParent(GameObject parent)    // 부모 설정
    {
        this.parent = parent;
    }


    public void ExpandBtnClick(PointerEventData data)   // 버튼 이벤트
    {
        toggle = !toggle;   // 토글 변경
        toggleTxt.gameObject.SetActive(toggle); // 토글에 따라 내용 키고 끄기
        SizeRefresh();      // 사이즈 재계산
        parent.GetComponent<UI_InfoScroll>().SizeRefresh();   // 부모 사이즈도 재계산
    }


    public void SizeRefresh() // 사이즈 재계산
    {
        ContentSizeFitter fitter = gameObject.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
    }
}
