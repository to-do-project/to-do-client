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
    Image profile;

    long goalId;
    string memberName, rate, color = "";
    bool wait;

    const string profileName = "Art/UI/Profile/Profile_Color_3x";


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
        profile = GetImage((int)Images.profile_img);

        Canvas.ForceUpdateCanvases();

        BindEvent(goal, GoalClick, Define.TouchEvent.Touch);

        Canvas.ForceUpdateCanvases();

        SetGoalContent();

    }

    public void GoalClick(PointerEventData data)
    {

        if (todo.activeSelf)
        {

            todo.SetActive(false);

        }
        else
        {

            todo.SetActive(true);

        }

        Canvas.ForceUpdateCanvases();

    }

    public void SetGoalContent(string name, string color, string rate, long goalId, List<GetTodoMembers> todolist, bool wait)
    {
        this.memberName = name;
        this.color = color;
        this.rate = rate;
        this.goalId = goalId;
        todoList = todolist;
        this.wait = wait;
    }

    private void SetGoalContent()
    {
        nickname.text = memberName;
        SetImage(color);
        if (wait)
        {

            goalAttend.text = "수락 대기 중";
            goalRate.text = "";

            todo.SetActive(false);

            return;
        }

        goalRate.text = rate + "%";
        //SetImage();

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

    public void SetImage(string color)
    {
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profile.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }
}
