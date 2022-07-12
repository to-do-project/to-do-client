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
        GroupCheck,
    }

    GameObject todo, goal;

    List<TodoItem> todoList;
    Text goalTitle;
    Text goalRate;

    long goalId;
    string title, rate = "";
    bool open, creater;

    GameObject groupCheck;

    public override void Init()
    {


        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        todo = Get<GameObject>((int)GameObjects.Todo);
        goal = Get<GameObject>((int)GameObjects.Goal);
        goalTitle = GetText((int)Texts.Goal_txt);
        goalRate = GetText((int)Texts.GoalRate_txt);
        groupCheck = Get<GameObject>((int)GameObjects.GroupCheck);

        GameObject groupCheckBtn = Get<GameObject>((int)GameObjects.groupCheck_btn);

        Canvas.ForceUpdateCanvases();

        BindEvent(goal, GoalClick, Define.TouchEvent.Touch);
        BindEvent(groupCheckBtn, GroupCheckClicked, Define.TouchEvent.Touch);

        SetGgoalContent();

    }

    void Start()
    {
        Init();
    }

    private void GroupCheckClicked(PointerEventData data)
    {
        Managers.Sound.PlayPopupSound();
        UI_GroupGoalCreater ui = Managers.UI.ShowPopupUI<UI_GroupGoalCreater>("GroupGoalCreaterView", "Main");
        ui.Setting(goalId);
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
        goalRate.text = rate+"%";


        foreach (TodoItem item in todoList)
        {
            UI_GtodoContent todoItem = Managers.UI.MakeSubItem<UI_GtodoContent>("GoalList", todo.transform, "Gtodo_content");
            //todoItem.Setting(goalId, item.todoMemberId, item.todoTitle, item.likeFlag, item.likeCount, item.completeFlag);
            todoItem.Setting(goalId, item.todoMemberId, item.todoTitle, item.likeFlag,item.likeCount, item.completeFlag);
        }

        groupCheck.transform.SetAsLastSibling();

        Canvas.ForceUpdateCanvases();

        todo.SetActive(false);
    }

    public void SetPercentage(int percentage)
    {
        //Debug.Log(percentage);
        goalRate.text = percentage.ToString() + "%";
    }
}
