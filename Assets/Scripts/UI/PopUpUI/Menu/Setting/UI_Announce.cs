using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Announce : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Back_btn,
    }

    enum Contents
    {
        AnnounceContent,
    }
    // ================================ //

    GameObject content;                                 // 공지사항 컨텐츠들의 부모 오브젝트
    List<GameObject> contentList;                       // 공지사항 컨텐츠(오브젝트) 리스트
    Dictionary<long, UI_AnnounceContent> announces;     // 공지사항 컨텐츠(스크립트) Dictionary

    bool check = true;  // 웹 통신 중복 호출을 막기위한 변수

    public override void Init() // 초기화
    {
        base.Init();

        CameraSet(); // 카메라 초기화

        SetBtns();   // 버튼 바인딩 및 이벤트 연결


        Bind<GameObject>(typeof(Contents));
        content = Get<GameObject>((int)Contents.AnnounceContent); // 컨텐츠 부모 오브젝트 바인딩

        contentList = new List<GameObject>(); // 리스트 초기화
        announces = new Dictionary<long, UI_AnnounceContent>();   // 딕셔너리 초기화

        InitContents(); // 공지사항 컨텐츠 생성

        check = false;  // 변수 초기화
    }

    void InitContents() // 공지사항 컨텐츠 생성
    {
        foreach (var tmp in dataContainer.announceList) // dataContainer.announceList >> 데이터 컨테이너(상속)에 있는 공지사항 리스트
        {
            AddAnnounce(tmp.noticeId, tmp.title, tmp.content);  // 공지사항 컨텐츠 생성 및 초기화
            //          공지사항 번호, 제목,    내용
        }
    }

    private void SetBtns() // 버튼 바인딩 및 이벤트 연결
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, (data) => { Managers.Sound.PlayNormalButtonClickSound(); ClosePopupUI(); });    // 팝업 제거 이벤트 연결
    }

    private void AddAnnounce(long key, string head, string sub)     // key >> 슈퍼키(번호), head >> 제목, sub >> 내용 || 공지사항 컨텐츠 생성 및 초기화
    {
        GameObject contents = Managers.Resource.Instantiate("UI/ScrollContents/AnnounceContent");   // 해당 url에 프리팹 생성
        contents.transform.SetParent(content.transform, false); // 공지사항 부모 오브젝트에 컨텐츠 연결
        contentList.Add(contents);  // 리스트에 추가

        UI_AnnounceContent announce = Util.GetOrAddComponent<UI_AnnounceContent>(contents); // 컨텐츠 스크립트 연결
        announce.Init();                // 초기화
        announce.SetHead(head);         // 제목 초기화
        announce.SetSub(sub);           // 내용 초기화
        announce.SetParent(gameObject); // 부모 전달
        announces.Add(key, announce);   // 딕셔너리에 추가

        SizeRefresh();  // 공지사항 컨텐츠 사이즈 재조절
    }

    private void Start()
    {
        Init();
    }

    public void SizeRefresh()   // 사이즈 재조절
    {
        ContentSizeFitter fitter = content.GetComponent<ContentSizeFitter>();   // 사이즈 필터 연결
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);   // 사이즈 재계산
    }

    public void ExpandContent(long key) // key >> 컨텐츠 고유번호 값 || 공지사항 컨텐츠 사이즈 조절
    {
        StartCoroutine(ExExpand(key));  // 코루틴으로 실행
    }

    IEnumerator ExExpand(long key)      // key >> 컨텐츠 고유번호 값 || 공지사항 컨텐츠 사이즈 조절
    {
        while (check) yield return null;
        announces[key].ExpandBtnClick(null);    // 공지사항 컨텐츠 사이즈 조절(toggle으로 축소/팽창)
    }
}
