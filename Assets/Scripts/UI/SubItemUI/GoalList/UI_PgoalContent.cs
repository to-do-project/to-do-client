using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_PgoalContent : UI_Base
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
        edit_btn,
    }

    GameObject todoAdd;
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

        GameObject edit = Get<GameObject>((int)GameObjects.edit_btn);

        Canvas.ForceUpdateCanvases();

        BindEvent(goal, GoalClick, Define.TouchEvent.Touch);
        BindEvent(edit, EditBtnClick);

        todoAdd = Managers.UI.MakeSubItem<UI_AddTodo>("GoalList", todo.transform, "AddTodo").gameObject;


        Canvas.ForceUpdateCanvases();
        
        SetPgoalContent();

    }

    public void GoalClick(PointerEventData data)
    {
        Canvas.ForceUpdateCanvases();
        Managers.Sound.PlayNormalButtonClickSound();
        if (todo.activeSelf)
        {
            //todo.GetComponent<UI_PtodoContent>().ClearUI();
            UI_PtodoContent[] childList = todo.GetComponentsInChildren<UI_PtodoContent>();
            if (childList != null)
            {
                foreach(UI_PtodoContent child in childList)
                {
                    child.ClearUI();
                }
            }

            todo.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform.parent.GetComponent<ContentSizeFitter>().transform);
            Canvas.ForceUpdateCanvases();

        }
        else
        {

            todo.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform.parent.GetComponent<ContentSizeFitter>().transform);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.GetComponent<ContentSizeFitter>().transform);
            Canvas.ForceUpdateCanvases();
        }

    }

    public void EditBtnClick(PointerEventData data)
    {
        Debug.Log("Editbtn click");
        Managers.Sound.PlayPopupSound();
        UI_GoalModify gm = Managers.UI.ShowPopupUI<UI_GoalModify>("GoalModifyView","Main");
        gm.Setting(goalId, title,open, false); //open은 임시로, API 바뀌면 넣어야함
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
        goalRate.text = rate+"%";

        todoAdd.GetComponent<UI_AddTodo>().Setting(goalId);
        todoAdd.GetComponent<UI_AddTodo>().SettingParent(this);

        Canvas.ForceUpdateCanvases();


        if (todoList.Count != 0 && todo!=null)
        {
            //Debug.Log("Count: " + todoList.Count);
            foreach (TodoItem item in todoList)
            {
                //Debug.Log($"{item.todoTitle} {item.todoMemberId}");
                UI_PtodoContent todoItem = Managers.UI.MakeSubItem<UI_PtodoContent>("GoalList", todo.transform, "Ptodo_content");
                
                
                todoItem.Setting(goalId,item.todoMemberId, item.todoTitle, item.likeFlag, item.likeCount, item.completeFlag);
            }
        }
        
        todoAdd.transform.SetAsLastSibling();

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.GetComponent<ContentSizeFitter>().transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform.parent.GetComponent<ContentSizeFitter>().transform);

        Canvas.ForceUpdateCanvases();

        todo.SetActive(false);
    }

    public void SetPercentage(int percentage)
    {
        //Debug.Log(percentage);
        goalRate.text = percentage.ToString() + "%";
    }
}
