using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_PgoalContent : UI_Base
{
    enum GameObjects
    {
        Goal,
        Todo,
    }

    GameObject todo, goal;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        todo = Get<GameObject>((int)GameObjects.Todo);
        goal = Get<GameObject>((int)GameObjects.Goal);

        todo.SetActive(false);

        //BindEvent(goal, GoalClick, Define.TouchEvent.Touch);
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
