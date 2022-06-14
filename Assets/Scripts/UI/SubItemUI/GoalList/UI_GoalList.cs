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


        Canvas.ForceUpdateCanvases();
        //��� �̼� ����

        if (Managers.Player.GetString(Define.SYSTEM_MISSION) != null)
        {
            Managers.UI.MakeSubItem<UI_SystemMission>("GoalList", goalParent.transform, "SystemMission");

        }

        Canvas.ForceUpdateCanvases();
        goalAddbtn = Managers.UI.MakeSubItem<UI_GoalAdd>("GoalList", goalParent.transform, "goalAdd_btn").gameObject;


        //GoalInit();
        /*Managers.UI.MakeSubItem<UI_GgoalContent>("GoalList",goalParent.transform, "Ggoal_content");
        Managers.UI.MakeSubItem<UI_PgoalContent>("GoalList",goalParent.transform, "Pgoal_content");
        Managers.UI.MakeSubItem<UI_GoalAdd>("GoalList", goalParent.transform, "goalAdd_btn");
*/
    }

    void Start()
    {
        Init();
    }


    //innerAction ���� �Լ� �ҷ��� GoalInit�� �ݹ����� ����ϱ�

    private void GoalInit()
    {
        if (Managers.Todo.goalList != null)
        {
            foreach (ResponseMainTodo item in Managers.Todo.goalList)
            {
                if (item.groupFlag)
                {
                    UI_GgoalContent goal = Managers.UI.MakeSubItem<UI_GgoalContent>("GoalList", goalParent.transform, "Ggoal_content");
                    /*                goal.SetGoalName();
                                    goal.SetGoalRate();*/

                }
                else
                {

                }

                goalAddbtn.transform.SetAsLastSibling();
            }
        }
        
    }
}
