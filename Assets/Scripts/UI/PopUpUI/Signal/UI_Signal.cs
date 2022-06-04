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

        AddSignal("[����] �������� 1", SignalType.Announce.ToString());
        AddSignal("[����] �������� 2", SignalType.Announce.ToString());
        AddSignal("ȫ�浿���� ģ�� ��û�� ���½��ϴ�.", SignalType.Request.ToString());
        AddSignal("�׷�'������'��ǥ�� ���������� 'to-do'�� �Ϸ��߽��ϴ�.", SignalType.GroupClear.ToString());
        AddSignal("�׷�'������'��ǥ�� ���������� ȫ�浿���� �����߽��ϴ�.", SignalType.GroupCheer.ToString());
        AddSignal("�׷�'������'��ǥ�� �ʴ�޾ҽ��ϴ�.", SignalType.GroupInvite.ToString());
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
