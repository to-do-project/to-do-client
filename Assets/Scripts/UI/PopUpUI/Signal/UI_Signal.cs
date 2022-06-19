using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Signal : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
    }

    enum Contents
    {
        SignalContents,
    }

    public enum SignalType
    {
        NOTICE_TWO,
        FRIEND_REQUEST,
        FRIEND_ACCEPT,
        PRIVATE_FAVORITE,
        PRIVATE_CHEER,
        GROUP_REQUEST,
        GROUP_ACCEPT,
        GROUP_DONE,
        GROUP_FAVORITE,
        GROUP_CHEER,
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
        content = Get<GameObject>((int)Contents.SignalContents);

        InitContents();
    }

    void InitContents()
    {
        if(dataContainer.pushLists.noticeNotifications != null)
            foreach (var tmp in dataContainer.pushLists.noticeNotifications)
                AddSignal(tmp.notificationId, 0, tmp.content, tmp.category, tmp.readStatus);

        if (dataContainer.pushLists.friendReqNotifications != null)
            foreach (var tmp in dataContainer.pushLists.friendReqNotifications)
                AddSignal(tmp.notificationId, tmp.friendId, tmp.content, tmp.category, tmp.readStatus);

        if (dataContainer.pushLists.groupReqNotifications != null)
            foreach (var tmp in dataContainer.pushLists.groupReqNotifications)
                AddSignal(tmp.notificationId, tmp.goalId, tmp.content, tmp.category, tmp.readStatus);

        if (dataContainer.pushLists.etcNotifications != null)
            foreach (var tmp in dataContainer.pushLists.etcNotifications)
                AddSignal(tmp.notificationId, 0, tmp.content, tmp.category, tmp.readStatus);
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, (data) => {
            dataContainer.RefreshPushData();
            ClosePopupUI();
        });
    }

    private void AddSignal(long noticeId, long id, string title, string type, string readStatus)
    {
        GameObject contents = Managers.Resource.Instantiate("UI/ScrollContents/SignalContent");
        contents.transform.SetParent(content.transform, false);
        contentList.Add(contents);

        UI_SignalContent signal = Util.GetOrAddComponent<UI_SignalContent>(contents);
        signal.Init();
        signal.SetTitle(title);
        signal.SetType(type);
        signal.SetId(id);
        signal.SetNoticeId(noticeId);
        if (readStatus == "READ") signal.SetRead();

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
