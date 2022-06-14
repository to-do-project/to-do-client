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
        //API 호출해서 목표 정보 받아옴

        //goalContent 생성
        //todoContent 생성
        //todo enable false하기

        //목표추가 버튼
        //Managers.UI.MakeSubItem<>();
        Bind<GameObject>(typeof(GameObjects));
        goalParent = Get<GameObject>((int)GameObjects.Content);


        Canvas.ForceUpdateCanvases();
        //운영자 미션 생성

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


    //innerAction 쓰는 함수 불러서 GoalInit을 콜백으로 사용하기

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
