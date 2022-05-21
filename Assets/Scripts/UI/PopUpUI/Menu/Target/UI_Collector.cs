using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Collector : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
        Test_btn
    }

    private GameObject content = null;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        if (content == null)
        {
            content = GameObject.Find("CollectorContent");
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Test_btn, (data) => { AddTarget("��ǥ ����", "�� �� ��"); });
    }

    private void Start()
    {
        Init();
    }

    //API���� ������ �����ͼ� �������� ����ֱ�(�ֽż� ����)
    void AddTarget(string name, string period)
    {
        GameObject target = Managers.Resource.Instantiate("UI/ScrollContents/TargetContent");
        target.transform.SetParent(content.transform);
        target.transform.localScale = new Vector3(1, 1, 1);
    }
}
