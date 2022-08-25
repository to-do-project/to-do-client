using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UI_GroupAddTodo : UI_Base
{

    enum Buttons
    {
        touchScale,
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

    long goalId;
    UI_GroupGoalCreater parent;

    public override void Init()
    {

        callback -= ResponseAction;
        callback += ResponseAction;

        innerAction -= InfoGather;
        innerAction += InfoGather;

        Bind<Button>(typeof(Buttons));
        Bind<InputField>(typeof(InputFields));

        todoName = GetInputfiled((int)InputFields.todo_inputfield);
        todoName.DeactivateInputField();
        todoName.interactable = false;
        todoName.onEndEdit.AddListener(delegate
        {
            InfoGather();
        });

        addBtn = GetButton((int)Buttons.touchScale).gameObject;
        BindEvent(addBtn, AddBtnClick);
        //BindEvent(this.gameObject, AddBtnClick);
    }

    void Start()
    {
        Init();
    }



    private void AddBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        //InfoGather();
        todoName.interactable = true;
        todoName.ActivateInputField();
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

    public void SettingParent(UI_GroupGoalCreater go)
    {
        parent = go;
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

                UI_OwnerTodoContent todoItem = Managers.UI.MakeSubItem<UI_OwnerTodoContent>("TodoGroup", this.transform.parent, "OwnerTodo_content");
                todoItem.Setting(goalId, res.result.todoMemberId, val.title, false, 0, false);
                this.transform.SetAsLastSibling();

                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform.parent);
                Canvas.ForceUpdateCanvases();
                todoName.text = "";

                parent.Setting(goalId);
                Managers.Todo.SendMainGoalRequest(Managers.Player.GetString(Define.USER_ID));

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
            todoName.DeactivateInputField();
            todoName.interactable = false;
        }


    }

}
