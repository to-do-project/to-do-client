using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SystemMission : UI_Base
{
    enum Texts
    {
        systemGoal_txt,
        systemGoalRate_txt,
    }
    enum GameObjects
    {
        systemGoal,
        systemTodo,
    }

    GameObject todo, goal;

    List<TodoItem> todoList = new List<TodoItem>();
    Text goalTitle;
    Text goalRate;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        
        todo = Get<GameObject>((int)GameObjects.systemTodo);
        goal = Get<GameObject>((int)GameObjects.systemGoal);

        goalTitle = GetText((int)Texts.systemGoal_txt);
        goalRate = GetText((int)Texts.systemGoalRate_txt);

        Canvas.ForceUpdateCanvases();

    }

    void Start()
    {
        Init();
    }



    public void GoalClick(PointerEventData data)
    {
        Canvas.ForceUpdateCanvases();
        Managers.Sound.PlayNormalButtonClickSound();
        if (todo.activeSelf)
        {


            todo.SetActive(false);

        }
        else
        {
            todo.SetActive(true);

        }

    }

    private void SetGgoalContent()
    {

    }
}
