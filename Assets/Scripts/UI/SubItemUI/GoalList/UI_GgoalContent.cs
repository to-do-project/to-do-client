using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_GgoalContent : UI_Base
{

    enum Buttons
    {
        groupCheck_btn,
    }

    enum Texts
    {
        Goal_txt,
        GoalRate_txt,
    }

    enum GameObjects
    {
        Goal,
        Todo,
    }

    GameObject todo, goal;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        Bind<GameObject>(typeof(GameObjects));
        todo = Get<GameObject>((int)GameObjects.Todo);
        goal = Get<GameObject>((int)GameObjects.Goal);

        //todo.SetActive(false);

        //BindEvent(goal, GoalClick, Define.TouchEvent.Touch);
    }

    void Start()
    {
        Init();
    }

    private void GroupCheckClicked(PointerEventData data)
    {
        //그룹목표확인창 띄우기
        //
    }

    public void GoalClick()
    {
        
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
