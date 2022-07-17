using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_InfoScroll : UI_Base
{

    enum GameObjects
    {
        UseContent,
        PersonalContent,
        Content,
    }

    GameObject use, personal;
    GameObject content;


    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        use = Get<GameObject>((int)GameObjects.UseContent);
        personal = Get<GameObject>((int)GameObjects.PersonalContent);

        content = Get<GameObject>((int)GameObjects.Content);

        Util.GetOrAddComponent<UI_InfoContent>(use).SetParent(gameObject);
        Util.GetOrAddComponent<UI_InfoContent>(personal).SetParent(gameObject);


        SizeRefresh();
    }

    void Start()
    {
        Init();
    }

    public void SizeRefresh()   // ������ ������
    {
        ContentSizeFitter fitter = content.GetComponent<ContentSizeFitter>();   // ������ ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);   // ������ ����
    }

}
