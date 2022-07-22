using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Collector : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
    enum Buttons
    {
        Back_btn,
    }
    // ================================ //

    GameObject content = null; // ������ �θ� ������Ʈ (CollectorContent)
    GameObject parent; // �θ� ������Ʈ (MenuView)

    public override void Init()
    {
        base.Init();

        CameraSet(); // ī�޶� ����(���)

        SetBtns();

        if (content == null)
        {
            content = GameObject.Find("CollectorContent"); // ������ �θ� ����
        }

        foreach (var tmp in dataContainer.goalList)
        {
            AddTarget(tmp.title, tmp.goalId); // ��ǥ �߰�
                   // ��ǥ �̸�, ��ǥ id
        }
    }
    public void SetParent(GameObject parent) // �θ� ����
    {
        this.parent = parent;
    }

    public void DeleteTarget(long id) // id >> ������ ��ǥ id || ���� ��ǥ ������ ����
    {
        foreach (var tmp in dataContainer.goalList)
        {
            if (tmp.goalId == id)
            {
                dataContainer.goalList.Remove(tmp); // ������ �����̳ʿ��� ����
            }
        }
        parent.GetComponent<UI_Menu>().ChangeCcount(); // �޴� ȭ���� ��ǥ ī��Ʈ �� �缳��
    }

    void AddTarget(string title, long id) // title >> ��ǥ �̸�, id >> �߰��� ��ǥ id || ���� ��ǥ ������ �߰�
    {
        GameObject target = Managers.Resource.Instantiate("UI/ScrollContents/TargetContent"); // Ÿ�� ������Ʈ ����
        target.transform.SetParent(content.transform, false);     // Ÿ�� �θ� ������ �θ� ����

        TargetContent tmp = target.GetComponent<TargetContent>();
        tmp.ChangeText(title); // Ÿ�� ������Ʈ�� �̸� �ؽ�Ʈ ����
        tmp.SetId(id);         // Ÿ�� ������Ʈ�� ��ǥ id ����
    }

    void SetBtns() // ��ư �̺�Ʈ ����
    {
        Bind<Button>(typeof(Buttons)); // ��ư ���ε�

        // �ڷΰ��� ��ư || ���� UI ����
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }

    void Start()
    {
        Init();
    }
}
