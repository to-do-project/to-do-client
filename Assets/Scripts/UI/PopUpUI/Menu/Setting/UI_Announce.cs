using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Announce : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
    }

    enum Contents
    {
        AnnounceContent,
    }

    GameObject content;
    List<GameObject> contentList;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();
        contentList = new List<GameObject>();

        Bind<GameObject>(typeof(Contents));
        content = Get<GameObject>((int)Contents.AnnounceContent);

        AddAnnounce("�������� 1", "������Ʈ ����\n��\n��\n��");
        AddAnnounce("�������� 2", "������Ʈ ����\n��\n��\n��\n��");
        AddAnnounce("�������� 3", "������Ʈ ����\n��\n��");
        AddAnnounce("�������� 4", "������Ʈ ����\n��\nī\nŸ\n��\n��");
        AddAnnounce("�������� 5", "������Ʈ ����");
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }

    private void AddAnnounce(string head, string sub)
    {
        GameObject contents = Managers.Resource.Instantiate("UI/ScrollContents/AnnounceContent");
        contents.transform.SetParent(content.transform, false);
        contentList.Add(contents);

        UI_AnnounceContent announce = Util.GetOrAddComponent<UI_AnnounceContent>(contents);
        announce.Init();
        announce.SetHead(head);
        announce.SetSub(sub);
        announce.SetParent(gameObject);

        SizeRefresh();
    }

    private void Start()
    {
        Init();
    }
    public void SizeRefresh()
    {
        ContentSizeFitter fitter = content.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
    }
}
