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

    Text goalRate; 
    long goalId;
    string rate = "";

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        todo = Get<GameObject>((int)GameObjects.systemTodo);
        goal = Get<GameObject>((int)GameObjects.systemGoal);

        goalRate = GetText((int)Texts.systemGoalRate_txt);

        Canvas.ForceUpdateCanvases();

        BindEvent(goal, GoalClick, Define.TouchEvent.Touch);

        SetGoalContent();
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

    public void SetGoalContent(string rate, long goalId, List<TodoItem> todolist)
    {
        this.rate = rate;
        this.goalId = goalId;
        todoList = todolist;
    }

    private void SetGoalContent()
    {
        goalRate.text = rate + "%";

        Debug.Log(todoList.Count);
        foreach (TodoItem item in todoList)
        {
            UI_GtodoContent todoItem = Managers.UI.MakeSubItem<UI_GtodoContent>("GoalList", todo.transform, "Gtodo_content");
            //todoItem.Setting(goalId, item.todoMemberId, item.todoTitle, item.likeFlag, item.likeCount, item.completeFlag);
            todoItem.Setting(goalId, item.todoMemberId, item.todoTitle, item.likeFlag, item.likeCount, item.completeFlag);
        }

        Canvas.ForceUpdateCanvases();

        todo.SetActive(false);
    }
}
