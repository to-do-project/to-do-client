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

[System.Serializable]
public class ResponseTodoCreate
{
    public long todoMemberId;
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
    InputField todoName;

    Action<UnityWebRequest> callback;
    Action innerAction;
    Response<ResponseTodoCreate> res;
    RequestTodoCreate val;
    public override void Init()
    {

        callback -= ResponseAction;
        callback += ResponseAction;

        innerAction -= InfoGather;
        innerAction += InfoGather;

        Bind<Button>(typeof(Buttons));
        Bind<InputField>(typeof(InputFields));

        todoName = GetInputfiled((int)InputFields.todo_inputfield);

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

        if (isValidTodo(todoName.text))
        {
            val = new RequestTodoCreate();
            val.goalId = goalId;
            val.title = todoName.text;

            res = new Response<ResponseTodoCreate>();
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
            res = JsonUtility.FromJson<Response<ResponseTodoCreate>>(request.downloadHandler.text);

            if (res.isSuccess)
            {
                Debug.Log(res.result);

                Canvas.ForceUpdateCanvases();

                UI_PtodoContent todoItem = Managers.UI.MakeSubItem<UI_PtodoContent>("GoalList", this.transform.parent, "Ptodo_content");
                todoItem.Setting(goalId,res.result.todoMemberId, val.title, false, 0, false);
                this.transform.SetAsLastSibling();

                todoName.text = "";

                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform.parent);
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
