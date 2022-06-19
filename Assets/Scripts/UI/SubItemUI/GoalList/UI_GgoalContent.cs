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

    List<TodoItem> todoList;
    Text goalTitle;
    Text goalRate;

    long goalId;
    string title, rate = "";
    bool open, creater;

    public override void Init()
    {


        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        todo = Get<GameObject>((int)GameObjects.Todo);
        goal = Get<GameObject>((int)GameObjects.Goal);
        goalTitle = GetText((int)Texts.Goal_txt);
        goalRate = GetText((int)Texts.GoalRate_txt);

        GameObject groupCheckBtn = Get<GameObject>((int)GameObjects.groupCheck_btn);

        Canvas.ForceUpdateCanvases();

        BindEvent(goal, GoalClick, Define.TouchEvent.Touch);
        BindEvent(groupCheckBtn, GroupCheckClicked, Define.TouchEvent.Touch);

        todo.SetActive(false);
        SetGgoalContent();

    }

    void Start()
    {
        Init();
    }

    private void GroupCheckClicked(PointerEventData data)
    {
        //그룹목표확인창 띄우기
        //
        if (creater)
        {
            Managers.UI.ShowPopupUI<UI_GroupGoalCreater>("GroupGoalCreaterView", "Main");

        }
        else
        {
            Managers.UI.ShowPopupUI<UI_GroupGoalParticipants>("GroupGoalParticipantsView", "Main");

        }
        //
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

    public void SetGgoalContent(string name, string rate, long goalId, List<TodoItem> todolist, bool creater)
    {
        title = name;
        this.rate = rate;
        this.goalId = goalId;
        todoList = todolist;
        this.creater = creater;
    }

    private void SetGgoalContent()
    {
        goalTitle.text = title;
        goalRate.text = rate;


        foreach (TodoItem todo in todoList)
        {
            
        }
    }
}
