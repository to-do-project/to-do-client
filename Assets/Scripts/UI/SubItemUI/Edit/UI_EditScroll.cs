using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



/// <summary>
/// �κ��丮 ���� ��ũ�Ѻ�
/// </summary>
public class UI_EditScroll : UI_Base
{


    enum GameObjects
    {
        Content,
    }

    GameObject contentRoot;


    public override void Init()
    {
        //������ ������ �ִ� ������ ���� �ܾ����
        //�����۰� ��ġ�ϴ� �������� ��ũ�Ѻ信 ����

        Bind<GameObject>(typeof(GameObjects));
        contentRoot = Get<GameObject>((int)GameObjects.Content);

        /*Managers.UI.MakeSubItem<UI_EditItem>("Edit", contentRoot.transform, "plant_03");
        Managers.UI.MakeSubItem<UI_EditItem>("Edit", contentRoot.transform, "plant_04");
        Managers.UI.MakeSubItem<UI_EditItem>("Edit", contentRoot.transform, "plant_05");
        Managers.UI.MakeSubItem<UI_EditItem>("Edit", contentRoot.transform, "stone_02");
        Managers.UI.MakeSubItem<UI_EditItem>("Edit", contentRoot.transform, "stone_01");
*/

    }


    void Start()
    {
        Init();
    }



}
