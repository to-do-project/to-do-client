using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_GoalList : UI_Base
{

    enum GameObjects
    {
        Content,
    }

    GameObject goalAddbtn;
    GameObject goalParent;

    public override void Init()
    {
        //API ȣ���ؼ� ��ǥ ���� �޾ƿ�

        //goalContent ����
        //todoContent ����
        //todo enable false�ϱ�

        //��ǥ�߰� ��ư
        //Managers.UI.MakeSubItem<>();
        Bind<GameObject>(typeof(GameObjects));
        goalParent = Get<GameObject>((int)GameObjects.Content);


        Managers.UI.MakeSubItem<UI_GgoalContent>("GoalList",goalParent.transform, "Ggoal_content");
        Managers.UI.MakeSubItem<UI_PgoalContent>("GoalList",goalParent.transform, "Pgoal_content");
        Managers.UI.MakeSubItem<UI_GoalAdd>("GoalList", goalParent.transform, "goalAdd_btn");
        
    }

    void Start()
    {
        Init();
    }

}
