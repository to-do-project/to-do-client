using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SystemFriendMission : UI_Base
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

    void Start()
    {
        
    }

    public override void Init()
    {


        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        todo = Get<GameObject>((int)GameObjects.systemTodo);
        goal = Get<GameObject>((int)GameObjects.systemGoal);
        goalRate = GetText((int)Texts.systemGoalRate_txt);

        Canvas.ForceUpdateCanvases();

        BindEvent(goal, GoalClick, Define.TouchEvent.Touch);

        SetGgoalContent();
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

    public void SetGgoalContent(string rate, long goalId, List<TodoItem> todolist)
    {
        this.rate = rate;
        this.goalId = goalId;
        todoList = todolist;
    }


    private void SetGgoalContent()
    {
        goalRate.text = rate + "%";


        foreach (TodoItem item in todoList)
        {
            Canvas.ForceUpdateCanvases();
            UI_PtodoFriendContent todoItem = Managers.UI.MakeSubItem<UI_PtodoFriendContent>("GoalList", todo.transform, "Ptodo_FriendContent");
            todoItem.Setting(goalId, item.todoMemberId, item.todoTitle, item.likeFlag, item.likeCount, item.completeFlag);
        }

        Canvas.ForceUpdateCanvases();

        todo.SetActive(false);
    }

    public void SetPercentage(int percentage)
    {
        //Debug.Log(percentage);
        goalRate.text = percentage.ToString() + "%";
    }
}
