using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tutorial : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        skip_btn,
        pre_btn,
        next_btn,
        start_btn,
    }
    // ================================ //

    [Header("스냅View 관련")]
    public GameObject container;
    public GameObject[] contents;
    public GameObject[] views;

    [Header("ScrollView 오브젝트")]
    public ScrollRect scrollView;

    [Header("슬라이더 위치")]
    public Transform[] points;
    public GameObject curPoint;

    GameObject nextBtn, startBtn;
    RectTransform contentTrans;
    int index = 1;

    // 초기화
    public override void Init()
    {
        base.Init();

        CameraSet();

        GetComponent<Canvas>().sortingOrder = 100;

        if (scrollView == null) scrollView = GetComponent<ScrollRect>();

        contentTrans = scrollView.content.GetComponent<RectTransform>();

        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.skip_btn, (data) => {
            Managers.Player.Init();
            UI_Load.Instance.InstantLoad("Main");
            Managers.Clear();
        });

        SetBtn((int)Buttons.pre_btn, (data) => {
            if (index <= 1) return;
            index--;
            ChangeContent(index);
        });

        nextBtn = SetBtn((int)Buttons.next_btn, (data) => {
            if (index >= views.Length - 2) return;
            index++;
            ChangeContent(index);
        });

        startBtn = SetBtn((int)Buttons.start_btn, (data) => {
            Managers.Player.Init();
            UI_Load.Instance.InstantLoad("Main");
            Managers.Clear();
        });

        ChangeContent(index);
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        // 마우스를 땟을 때 scrollView의 위치값을 받아와 왼쪽, 오른쪽 슬라이드 판단
        Vector2 pos = (Vector2)scrollView.transform.InverseTransformPoint(contentTrans.position);

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(pos.x > 1440 * 1 / 4 - 720)
            {
                Left();
            }
            if(pos.x < -1440 * 1 / 4 - 720)
            {
                Right();
            }
        }
    }

    // 왼쪽 슬라이드
    // 1(왼), 2(중), 3(오) 위치의 화면을
    // 1(왼)을 2(중)으로 옮겨 왼쪽 화면으로 넘기는 효과와 함께
    // 2(중)을 3(오)로 옮긴 후 ScrollView 위치 값을 조정하여 스냅효과 추가
    void Left()
    {
        if (index <= 1) return;
        index--;
        
        // 얼마나 슬라이드 했는지 계산
        Vector2 dif = new Vector2(1440f, 0) -
            (Vector2)scrollView.transform.InverseTransformPoint(contentTrans.position) +
            new Vector2(-720f, 0);

        // 계산값을 토대로 슬라이드 한 만큼 위치 재조정
        Vector2 point = new Vector2(-720f, 0) - dif / 3;

        point.y = 0f;
        contentTrans.anchoredPosition = point;

        // index값에 맞춰 컨텐츠 화면 재설정
        ChangeContent(index);
    }

    // 오른쪽 슬라이드
    // 1(왼), 2(중), 3(오) 위치의 화면을
    // 3(오)을 2(중)으로 옮겨 오른쪽 화면으로 넘기는 효과와 함께
    // 2(중)을 1(왼)으로 옮긴 후 ScrollView 위치 값을 조정하여 스냅효과 추가
    void Right()
    {
        if (index >= views.Length - 2) return;
        index++;

        // 얼마나 슬라이드 했는지 계산
        Vector2 dif = new Vector2(1440f, 0) -
            new Vector2(-720f, 0) +
            (Vector2)scrollView.transform.InverseTransformPoint(contentTrans.position);

        // 계산값을 토대로 슬라이드 한 만큼 위치 재조정
        Vector2 point = new Vector2(-720f, 0) + dif * 1.5f;

        point.y = 0f;
        contentTrans.anchoredPosition = point;

        // index값에 맞춰 컨텐츠 화면 재설정
        ChangeContent(index);
    }

    // 컨텐츠 위치 조정
    // contents[0] >> 슬라이드 왼쪽화면(이전화면)
    // contents[1] >> 슬라이드 중앙화면(현재화면)
    // contents[2] >> 슬라이드 오른쪽화면(다음화면)
    // index >> views에서 현재 화면이 될 index값
    void ChangeContent(int index)
    {
        // contents[i]의 자식 오브젝트를 비활성화하고 대기 오브젝트로 이동시킨다
        for(int i = 0; i < 3; i++)
        {
            if (contents[i].transform.childCount != 0)
            {
                contents[i].transform.GetChild(0).gameObject.SetActive(false);
                contents[i].transform.GetChild(0).SetParent(container.transform, false);
            }
        }

        // index값에 맞는 views를 contents에 넣어준다
        for(int i = 0; i < 3; i++)
        {
            views[index + i - 1].SetActive(true);
            views[index + i - 1].transform.SetParent(contents[i].transform, false);
        }

        // 마지막 index인 경우 시작 버튼 활성화
        if(index >= views.Length - 2)
        {
            nextBtn.SetActive(false);
            startBtn.SetActive(true);
        } else
        {
            nextBtn.SetActive(true);
            startBtn.SetActive(false);
        }

        // 슬라이더 위치 조정(동그라미)
        curPoint.transform.position = points[index - 1].position;
    }
}
