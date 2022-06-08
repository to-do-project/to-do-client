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
        edit_btn,
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

        GameObject edit = Get<GameObject>((int)GameObjects.edit_btn);

        Canvas.ForceUpdateCanvases();

        BindEvent(goal, GoalClick, Define.TouchEvent.Touch);
        BindEvent(edit, EditBtnClick);

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

    public void EditBtnClick(PointerEventData data)
    {
        Debug.Log("Editbtn click");
        Managers.UI.ShowPopupUI<UI_GoalModify>("GoalModifyView","Main");
    }

    public void SetGoalName(string name)
    {

    }

    public void SetGoalRate(string rate)
    {

    }
}
