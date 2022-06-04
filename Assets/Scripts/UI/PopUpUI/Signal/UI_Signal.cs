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
        Announce,
        Request,
        GroupClear,
        GroupCheer,
        GroupInvite,
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

        AddSignal("[공지] 공지사항 1", SignalType.Announce.ToString());
        AddSignal("[공지] 공지사항 2", SignalType.Announce.ToString());
        AddSignal("홍길동님이 친구 요청을 보냈습니다.", SignalType.Request.ToString());
        AddSignal("그룹'ㅇㅇㅇ'목표의 ㅇㅇㅇ님이 'to-do'를 완료했습니다.", SignalType.GroupClear.ToString());
        AddSignal("그룹'ㅇㅇㅇ'목표의 ㅇㅇㅇ님이 홍길동님을 응원했습니다.", SignalType.GroupCheer.ToString());
        AddSignal("그룹'ㅇㅇㅇ'목표에 초대받았습니다.", SignalType.GroupInvite.ToString());
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }

    private void AddSignal(string title, string type)
    {
        GameObject contents = Managers.Resource.Instantiate("UI/ScrollContents/SignalContent");
        contents.transform.SetParent(content.transform, false);
        contentList.Add(contents);

        UI_SignalContent signal = Util.GetOrAddComponent<UI_SignalContent>(contents);
        signal.Init();
        signal.SetTitle(title);
        signal.SetType(type);

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
