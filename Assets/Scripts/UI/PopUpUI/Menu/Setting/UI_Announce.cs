using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Announce : UI_Popup
{
    enum Buttons
    {
        Back_btn,
    }

    enum Contents
    {
        Content,
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
        content = Get<GameObject>((int)Contents.Content);

        AddAnnounce("공지사항 1", "업데이트 내용\n가\n나\n다");
        AddAnnounce("공지사항 2", "업데이트 내용\n라\n마\n바\n사");
        AddAnnounce("공지사항 3", "업데이트 내용\n아\n자");
        AddAnnounce("공지사항 4", "업데이트 내용\n차\n카\n타\n파\n하");
        AddAnnounce("공지사항 5", "업데이트 내용");
    }

    private void CameraSet()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);
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
