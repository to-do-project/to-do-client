using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Announce : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
    enum Buttons
    {
        Back_btn,
    }

    enum Contents
    {
        AnnounceContent,
    }
    // ================================ //

    GameObject content;                                 // �������� ���������� �θ� ������Ʈ
    List<GameObject> contentList;                       // �������� ������(������Ʈ) ����Ʈ
    Dictionary<long, UI_AnnounceContent> announces;     // �������� ������(��ũ��Ʈ) Dictionary

    bool check = true;  // �� ��� �ߺ� ȣ���� �������� ����

    public override void Init() // �ʱ�ȭ
    {
        base.Init();

        CameraSet(); // ī�޶� �ʱ�ȭ

        SetBtns();   // ��ư ���ε� �� �̺�Ʈ ����


        Bind<GameObject>(typeof(Contents));
        content = Get<GameObject>((int)Contents.AnnounceContent); // ������ �θ� ������Ʈ ���ε�

        contentList = new List<GameObject>(); // ����Ʈ �ʱ�ȭ
        announces = new Dictionary<long, UI_AnnounceContent>();   // ��ųʸ� �ʱ�ȭ

        InitContents(); // �������� ������ ����

        check = false;  // ���� �ʱ�ȭ
    }

    void InitContents() // �������� ������ ����
    {
        foreach (var tmp in dataContainer.announceList) // dataContainer.announceList >> ������ �����̳�(���)�� �ִ� �������� ����Ʈ
        {
            AddAnnounce(tmp.noticeId, tmp.title, tmp.content);  // �������� ������ ���� �� �ʱ�ȭ
            //          �������� ��ȣ, ����,    ����
        }
    }

    private void SetBtns() // ��ư ���ε� �� �̺�Ʈ ����
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, (data) => { Managers.Sound.PlayNormalButtonClickSound(); ClosePopupUI(); });    // �˾� ���� �̺�Ʈ ����
    }

    private void AddAnnounce(long key, string head, string sub)     // key >> ����Ű(��ȣ), head >> ����, sub >> ���� || �������� ������ ���� �� �ʱ�ȭ
    {
        GameObject contents = Managers.Resource.Instantiate("UI/ScrollContents/AnnounceContent");   // �ش� url�� ������ ����
        contents.transform.SetParent(content.transform, false); // �������� �θ� ������Ʈ�� ������ ����
        contentList.Add(contents);  // ����Ʈ�� �߰�

        UI_AnnounceContent announce = Util.GetOrAddComponent<UI_AnnounceContent>(contents); // ������ ��ũ��Ʈ ����
        announce.Init();                // �ʱ�ȭ
        announce.SetHead(head);         // ���� �ʱ�ȭ
        announce.SetSub(sub);           // ���� �ʱ�ȭ
        announce.SetParent(gameObject); // �θ� ����
        announces.Add(key, announce);   // ��ųʸ��� �߰�

        SizeRefresh();  // �������� ������ ������ ������
    }

    private void Start()
    {
        Init();
    }

    public void SizeRefresh()   // ������ ������
    {
        ContentSizeFitter fitter = content.GetComponent<ContentSizeFitter>();   // ������ ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);   // ������ ����
    }

    public void ExpandContent(long key) // key >> ������ ������ȣ �� || �������� ������ ������ ����
    {
        StartCoroutine(ExExpand(key));  // �ڷ�ƾ���� ����
    }

    IEnumerator ExExpand(long key)      // key >> ������ ������ȣ �� || �������� ������ ������ ����
    {
        while (check) yield return null;
        announces[key].ExpandBtnClick(null);    // �������� ������ ������ ����(toggle���� ���/��â)
    }
}
