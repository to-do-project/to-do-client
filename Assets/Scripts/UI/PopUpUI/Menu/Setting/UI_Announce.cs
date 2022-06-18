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
    Dictionary<long, UI_AnnounceContent> announces;

    bool check = true;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();
        contentList = new List<GameObject>();

        Bind<GameObject>(typeof(Contents));
        content = Get<GameObject>((int)Contents.AnnounceContent);

        announces = new Dictionary<long, UI_AnnounceContent>();

        InitContents();

        check = false;
    }

    void InitContents()
    {
        foreach (var tmp in dataContainer.announceList)
        {
            AddAnnounce(tmp.noticeId, tmp.title, tmp.content);
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }

    private void AddAnnounce(long key, string head, string sub)
    {
        GameObject contents = Managers.Resource.Instantiate("UI/ScrollContents/AnnounceContent");
        contents.transform.SetParent(content.transform, false);
        contentList.Add(contents);

        UI_AnnounceContent announce = Util.GetOrAddComponent<UI_AnnounceContent>(contents);
        announce.Init();
        announce.SetHead(head);
        announce.SetSub(sub);
        announce.SetParent(gameObject);
        announces.Add(key, announce);

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

    public void ExpandContent(long key)
    {
        StartCoroutine(ExExpand(key));
    }

    IEnumerator ExExpand(long key)
    {
        while (check) yield return null;
        announces[key].ExpandBtnClick(null);
    }
}
