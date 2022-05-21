using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EditScroll : UI_Base
{
    enum GameObjects
    {
        Content,
    }

    GameObject root;
    
    public override void Init()
    {
        //������ ������ �ִ� ������ ���� �ܾ����
        //�����۰� ��ġ�ϴ� �������� ��ũ�Ѻ信 ����

        Bind<GameObject>(typeof(GameObjects));
        root = Get<GameObject>((int)GameObjects.Content);

        Managers.UI.MakeSubItem<UI_EditItem>("Edit",root.transform,"portal_00");
    }

    void Start()
    {
        Init();
    }

}
