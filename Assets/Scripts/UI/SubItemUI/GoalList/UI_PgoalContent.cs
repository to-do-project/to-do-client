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

        Canvas.ForceUpdateCanvases();

        BindEvent(goal, GoalClick, Define.TouchEvent.Touch);

        todo.SetActive(false);


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
