using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_PgoalFriendContent : UI_Base
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
    }

    GameObject todo, goal;

    List<TodoItem> todoList = new List<TodoItem>();
    Text goalTitle;
    Text goalRate;

    long goalId;
    string title, rate = "";
    bool open;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        todo = Get<GameObject>((int)GameObjects.Todo);
        goal = Get<GameObject>((int)GameObjects.Goal);

        goalTitle = GetText((int)Texts.Goal_txt);
        goalRate = GetText((int)Texts.GoalRate_txt);

        Canvas.ForceUpdateCanvases();

        BindEvent(goal, GoalClick, Define.TouchEvent.Touch);

        Canvas.ForceUpdateCanvases();

        SetPgoalContent();

    }

    public void GoalClick(PointerEventData data)
    {
        Canvas.ForceUpdateCanvases();
        if (todo.activeSelf)
        {
            //todo.GetComponent<UI_PtodoContent>().ClearUI();
            UI_PtodoFriendContent[] childList = todo.GetComponentsInChildren<UI_PtodoFriendContent>();
            if (childList != null)
            {
                foreach (UI_PtodoFriendContent child in childList)
                {
                    child.ClearUI();
                }
            }

            todo.SetActive(false);

        }
        else
        {

            todo.SetActive(true);

        }

        Canvas.ForceUpdateCanvases();
    }

    public void SetPgoalContent(string name, string rate, long goalId, List<TodoItem> todolist, bool open)
    {
        title = name;
        this.rate = rate;
        this.goalId = goalId;
        todoList = todolist;
        this.open = open;
    }

    private void SetPgoalContent()
    {
        goalTitle.text = title;
        goalRate.text = rate + "%";

        Canvas.ForceUpdateCanvases();

        if (todoList.Count != 0 && todo != null)
        {
            foreach (TodoItem item in todoList)
            {
                UI_PtodoFriendContent todoItem = Managers.UI.MakeSubItem<UI_PtodoFriendContent>("GoalList", todo.transform, "Ptodo_FriendContent");


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
