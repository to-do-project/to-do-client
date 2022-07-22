using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Application : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
    enum Buttons
    {
        Back_btn,
    }
    // ================================ //

    public override void Init() // �ʱ�ȭ
    {
        base.Init();

        CameraSet(); // ī�޶� ���� (���)

        SetBtns();
    }

    void SetBtns() // ��ư �̺�Ʈ ����
    {
        Bind<Button>(typeof(Buttons)); // ��ư ���ε�

        // �ڷΰ��� ��ư || Ŭ���� ���, ���� UI ����
        SetBtn((int)Buttons.Back_btn, (data) => { Managers.Sound.PlayNormalButtonClickSound(); ClosePopupUI(); });
    }

    void Start()
    {
        Init();
    }
}
