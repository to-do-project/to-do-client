using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class RequestTodoCreate
{
    public long goalId;
    public string title;
}

public class UI_AddTodo : UI_Base
{

    enum Buttons
    {
        todoAdd_btn,
    }

    enum InputFields
    {
        todo_inputfield,
    }

    GameObject addBtn;
    InputField todo_input;

    Action<UnityWebRequest> callback;
    Action innerAction;
    Response<string> res;
    RequestTodoCreate val;
    public override void Init()
    {

        callback -= ResponseAction;
        callback += ResponseAction;

        innerAction -= InfoGather;
        innerAction += InfoGather;

        Bind<Button>(typeof(Buttons));
        Bind<InputField>(typeof(InputFields));

        addBtn = GetButton((int)Buttons.todoAdd_btn).gameObject;
        BindEvent(addBtn, AddBtnClick);
    }

    void Start()
    {
        Init();
    }

    long goalId;

    private void AddBtnClick(PointerEventData data)
    {
        InfoGather();
    }

    private void InfoGather()
    {
        InputField todoName = GetInputfiled((int)InputFields.todo_inputfield);
        if (isValidTodo(todoName.text))
        {
            val = new RequestTodoCreate();
            val.goalId = goalId;
            val.title = todoName.text;

            res = new Response<string>();
            //상세할일 추가
            Managers.Web.SendPostRequest<RequestTodoCreate>("api/todo", val, callback, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());

        }
    }

    private bool isValidTodo(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        } 
        try
        {
            return Regex.IsMatch(text, @"^.{0,50}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public void Setting(long id)
    {
        goalId = id;
    }

    private void ResponseAction(UnityWebRequest request)
    {
        if (res != null)
        {
            res = JsonUtility.FromJson<Response<string>>(request.downloadHandler.text);

            if (res.isSuccess)
            {
                Debug.Log(res.result);

                Canvas.ForceUpdateCanvases();

                UI_PtodoContent todoItem = Managers.UI.MakeSubItem<UI_PtodoContent>("GoalList", this.transform.parent, "Ptodo_content");
                todoItem.Setting(goalId, val.title, false, 0);
                this.transform.SetAsLastSibling();
                Canvas.ForceUpdateCanvases();
            }

            else
            {
                switch (res.code)
                {
                    case 5006:
                    case 5018:
                    case 5020:
                        Debug.Log("Todo create failed");
                        break;

                    case 6023:
                        Managers.Player.SendTokenRequest(innerAction);
                        break;
                }
            }
        }


    }

}
