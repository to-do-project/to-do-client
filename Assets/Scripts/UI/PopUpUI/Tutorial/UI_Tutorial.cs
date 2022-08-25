using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tutorial : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
    enum Buttons
    {
        skip_btn,
        pre_btn,
        next_btn,
        start_btn,
    }
    // ================================ //

    [Header("����View ����")]
    public GameObject container;
    public GameObject[] contents;
    public GameObject[] views;

    [Header("ScrollView ������Ʈ")]
    public ScrollRect scrollView;

    [Header("�����̴� ��ġ")]
    public Transform[] points;
    public GameObject curPoint;

    GameObject nextBtn, startBtn;
    RectTransform contentTrans;
    int index = 1;

    // �ʱ�ȭ
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
        // ���콺�� ���� �� scrollView�� ��ġ���� �޾ƿ� ����, ������ �����̵� �Ǵ�
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

    // ���� �����̵�
    // 1(��), 2(��), 3(��) ��ġ�� ȭ����
    // 1(��)�� 2(��)���� �Ű� ���� ȭ������ �ѱ�� ȿ���� �Բ�
    // 2(��)�� 3(��)�� �ű� �� ScrollView ��ġ ���� �����Ͽ� ����ȿ�� �߰�
    void Left()
    {
        if (index <= 1) return;
        index--;
        
        // �󸶳� �����̵� �ߴ��� ���
        Vector2 dif = new Vector2(1440f, 0) -
            (Vector2)scrollView.transform.InverseTransformPoint(contentTrans.position) +
            new Vector2(-720f, 0);

        // ��갪�� ���� �����̵� �� ��ŭ ��ġ ������
        Vector2 point = new Vector2(-720f, 0) - dif / 3;

        point.y = 0f;
        contentTrans.anchoredPosition = point;

        // index���� ���� ������ ȭ�� �缳��
        ChangeContent(index);
    }

    // ������ �����̵�
    // 1(��), 2(��), 3(��) ��ġ�� ȭ����
    // 3(��)�� 2(��)���� �Ű� ������ ȭ������ �ѱ�� ȿ���� �Բ�
    // 2(��)�� 1(��)���� �ű� �� ScrollView ��ġ ���� �����Ͽ� ����ȿ�� �߰�
    void Right()
    {
        if (index >= views.Length - 2) return;
        index++;

        // �󸶳� �����̵� �ߴ��� ���
        Vector2 dif = new Vector2(1440f, 0) -
            new Vector2(-720f, 0) +
            (Vector2)scrollView.transform.InverseTransformPoint(contentTrans.position);

        // ��갪�� ���� �����̵� �� ��ŭ ��ġ ������
        Vector2 point = new Vector2(-720f, 0) + dif * 1.5f;

        point.y = 0f;
        contentTrans.anchoredPosition = point;

        // index���� ���� ������ ȭ�� �缳��
        ChangeContent(index);
    }

    // ������ ��ġ ����
    // contents[0] >> �����̵� ����ȭ��(����ȭ��)
    // contents[1] >> �����̵� �߾�ȭ��(����ȭ��)
    // contents[2] >> �����̵� ������ȭ��(����ȭ��)
    // index >> views���� ���� ȭ���� �� index��
    void ChangeContent(int index)
    {
        // contents[i]�� �ڽ� ������Ʈ�� ��Ȱ��ȭ�ϰ� ��� ������Ʈ�� �̵���Ų��
        for(int i = 0; i < 3; i++)
        {
            if (contents[i].transform.childCount != 0)
            {
                contents[i].transform.GetChild(0).gameObject.SetActive(false);
                contents[i].transform.GetChild(0).SetParent(container.transform, false);
            }
        }

        // index���� �´� views�� contents�� �־��ش�
        for(int i = 0; i < 3; i++)
        {
            views[index + i - 1].SetActive(true);
            views[index + i - 1].transform.SetParent(contents[i].transform, false);
        }

        // ������ index�� ��� ���� ��ư Ȱ��ȭ
        if(index >= views.Length - 2)
        {
            nextBtn.SetActive(false);
            startBtn.SetActive(true);
        } else
        {
            nextBtn.SetActive(true);
            startBtn.SetActive(false);
        }

        // �����̴� ��ġ ����(���׶��)
        curPoint.transform.position = points[index - 1].position;
    }
}
