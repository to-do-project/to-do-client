using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_OwnerGoalContent : UI_Base
{
    enum Texts
    {
        myName_txt,
        myGoalRate_txt,
    }
    enum GameObjects
    {
        OwnerGoal,
        OwnerTodo,
    }

    enum Images
    {
        myProfile_img,
    }

    GameObject todoAdd;
    GameObject todo, goal;

    List<GetTodoMembers> todoList = new List<GetTodoMembers>();
    Text nicknameTxt;
    Text goalRateTxt;

    long goalId;
    string nickname, rate, color = "";

    Image profileImg;

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

        todo = Get<GameObject>((int)GameObjects.OwnerTodo);
        goal = Get<GameObject>((int)GameObjects.OwnerGoal);

        nicknameTxt = GetText((int)Texts.myName_txt);
        goalRateTxt = GetText((int)Texts.myGoalRate_txt);
        profileImg = GetImage((int)Images.myProfile_img);

        Canvas.ForceUpdateCanvases();

        //BindEvent(goal, GoalClick, Define.TouchEvent.Touch);

        //todoAdd = Get<GameObject>((int)GameObjects.AddTodo);


        Canvas.ForceUpdateCanvases();

        //SetGoalContent();

    }

    public void GoalClick(PointerEventData data)
    {
/*        Canvas.ForceUpdateCanvases();
        if (todo.activeSelf)
        {

            todo.SetActive(false);

        }
        else
        {

            todo.SetActive(true);

        }
*/
    }

    public void SetGoalContent(string name, string color, string rate, long goalId, List<GetTodoMembers> todolist)
    {
        this.nickname = name;
        this.color = color;
        this.rate = rate;
        this.goalId = goalId;
        todoList = todolist;

        SetGoalContent();
    }

    private void SetGoalContent()
    {
        nicknameTxt.text = nickname;
        goalRateTxt.text = rate + "%";
        SetImage(color);

        Canvas.ForceUpdateCanvases();


        if (todoList.Count != 0 && todo != null)
        {
            Debug.Log("Count: " + todoList.Count);
            foreach (GetTodoMembers item in todoList)
            {
                Debug.Log($"{item.todoTitle} {item.todoMemberId}");
                UI_OwnerTodoContent todoItem = Managers.UI.MakeSubItem<UI_OwnerTodoContent>("TodoGroup", todo.transform, "OwnerTodo_content");


                todoItem.Setting(goalId, item.todoMemberId, item.todoTitle, item.likeFlag, item.likeCount, item.completeFlag);
            }
        }

        //todoAdd.transform.SetAsLastSibling();

        Canvas.ForceUpdateCanvases();

    }

    public void SetPercentage(int percentage)
    {
        //Debug.Log(percentage);
        goalRateTxt.text = percentage.ToString() + "%";
    }

    public void SetImage(string color)
    {
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profileImg.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }
}
