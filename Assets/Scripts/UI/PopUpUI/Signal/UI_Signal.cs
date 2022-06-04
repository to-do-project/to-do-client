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

        AddSignal("[因走] 因走紫牌 1", SignalType.Announce.ToString());
        AddSignal("[因走] 因走紫牌 2", SignalType.Announce.ToString());
        AddSignal("畠掩疑還戚 庁姥 推短聖 左蛎柔艦陥.", SignalType.Request.ToString());
        AddSignal("益血'ししし'鯉妊税 ししし還戚 'to-do'研 刃戟梅柔艦陥.", SignalType.GroupClear.ToString());
        AddSignal("益血'ししし'鯉妊税 ししし還戚 畠掩疑還聖 誓据梅柔艦陥.", SignalType.GroupCheer.ToString());
        AddSignal("益血'ししし'鯉妊拭 段企閤紹柔艦陥.", SignalType.GroupInvite.ToString());
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
