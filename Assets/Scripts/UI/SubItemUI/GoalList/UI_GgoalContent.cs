using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_GgoalContent : UI_Base
{



    enum Texts
    {
        Goal_txt,
        GoalRate_txt,
    }

    enum GameObjects
    {
        Goal,
        Todo,
        groupCheck_btn,
    }

    GameObject todo, goal;

    public override void Init()
    {


        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        todo = Get<GameObject>((int)GameObjects.Todo);
        goal = Get<GameObject>((int)GameObjects.Goal);

        GameObject groupCheckBtn = Get<GameObject>((int)GameObjects.groupCheck_btn);

        Canvas.ForceUpdateCanvases();

        BindEvent(goal, GoalClick, Define.TouchEvent.Touch);
        BindEvent(groupCheckBtn, GroupCheckClicked, Define.TouchEvent.Touch);

        todo.SetActive(false);


    }

    void Start()
    {
        Init();
    }

    private void GroupCheckClicked(PointerEventData data)
    {
        //그룹목표확인창 띄우기
        //
        Managers.UI.ShowPopupUI<UI_GroupGoalParticipants>("GroupGoalParticipantsView", "Main");
        //Managers.UI.ShowPopupUI<UI_GroupGoalCreater>("GroupGoalCreaterView","Main");

    }

    public void GoalClick(PointerEventData data)
    {
        Canvas.ForceUpdateCanvases();
        if (todo.activeSelf)
        {
            todo.SetActive(false);
           
        }
        else
        {
            todo.SetActive(true);

        }

    }

    public void SetGoalName(string name)
    {

    }

    public void SetGoalRate(string rate)
    {

    }
}
