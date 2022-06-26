using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_MemberGoalContent : UI_Base
{

    enum Texts
    {
        name_txt,
        GoalRate_txt,
        GoalAttend_txt,
    }
    enum GameObjects
    {
        Goal,
        Todo,
    }

    enum Images
    {
        profile_img,
    }

    GameObject todo, goal;

    List<GetTodoMembers> todoList = new List<GetTodoMembers>();
    Text nickname;
    Text goalRate;
    Text goalAttend;

    long goalId;
    string title, rate = "";
    bool wait;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        todo = Get<GameObject>((int)GameObjects.Todo);
        goal = Get<GameObject>((int)GameObjects.Goal);

        nickname = GetText((int)Texts.name_txt);
        goalRate = GetText((int)Texts.GoalRate_txt);
        goalAttend = GetText((int)Texts.GoalAttend_txt);

        Canvas.ForceUpdateCanvases();

        BindEvent(goal, GoalClick, Define.TouchEvent.Touch);

        Canvas.ForceUpdateCanvases();

        SetGoalContent();

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

    public void SetGoalContent(string name, string rate, long goalId, List<GetTodoMembers> todolist, bool wait)
    {
        title = name;
        this.rate = rate;
        this.goalId = goalId;
        todoList = todolist;
        this.wait = wait;
    }

    private void SetGoalContent()
    {
        if (wait)
        {
            goalAttend.text = "수락 대기 중";
            goalRate.text = "";

            todo.SetActive(false);

            return;
        }

        nickname.text = title;
        goalRate.text = rate + "%";

        Canvas.ForceUpdateCanvases();


        if (todoList.Count != 0 && todo != null)
        {
            Debug.Log("Count: " + todoList.Count);
            foreach (GetTodoMembers item in todoList)
            {
                Debug.Log($"{item.todoTitle} {item.todoMemberId}");
                UI_MemberTodoContent todoItem = Managers.UI.MakeSubItem<UI_MemberTodoContent>("TodoGroup", todo.transform, "MemberTodo_content");


                todoItem.Setting(goalId, item.todoMemberId, item.todoTitle, item.likeFlag, item.likeCount, item.completeFlag);
            }
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
