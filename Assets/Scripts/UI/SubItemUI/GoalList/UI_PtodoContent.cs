using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RequestTodoModify
{
    public string title;
}

[System.Serializable]
public class ResponseTodoCheck
{
    public int percentage;
}


public class UI_PtodoContent : UI_Base
{
    enum Buttons
    {
        like_btn,
        edit_btn,
    }

    enum Texts 
    {
        like_txt,
        todo_title,
    }
    enum Toggles
    {
        todoCheck_toggle
    }

    enum InputFields
    {
        todo_inputfield,
    }

    long goalId;
    long todoMemberId;
    string title;
    bool likeFlag, completeFlag;
    int likeCount;

    InputField todoInputfield;
    Text todoTitle,likeTxt;
    Action innerAction;
    Toggle checkToggle;

    public override void Init()
    {
        innerAction -= SendTodoModifyRequest;
        innerAction += SendTodoModifyRequest;

        //Debug.Log("GoalId " + goalId);
        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Toggles));

        todoInputfield = GetInputfiled((int)InputFields.todo_inputfield);
        todoTitle = GetText((int)Texts.todo_title);
        likeTxt = GetText((int)Texts.like_txt);
        checkToggle = Get<Toggle>((int)Toggles.todoCheck_toggle);


        GameObject likeBtn = GetButton((int)Buttons.like_btn).gameObject;
        GameObject editBtn = GetButton((int)Buttons.edit_btn).gameObject;
        BindEvent(likeBtn, LikeBtnClick);
        BindEvent(editBtn, EditBtnClick);

        SetTodo();
        
        todoInputfield.onEndEdit.AddListener(delegate
        {
            SendTodoModifyRequest();
            todoInputfield.DeactivateInputField();
            //todoInputfield.interactable = false;
        });

        checkToggle.onValueChanged.AddListener((bool bOn)=> {
            if (checkToggle.isOn)
            {
                Managers.Web.SendPostRequest<ResponseTodoCheck>("api/todo/" + todoMemberId.ToString(), null, (uwr) =>
                {
                    Response<ResponseTodoCheck> res = JsonUtility.FromJson<Response<ResponseTodoCheck>>(uwr.downloadHandler.text);

                    if (res.isSuccess)
                    {

                        this.transform.parent.parent.gameObject.GetComponent<UI_PgoalContent>().SetPercentage(res.result.percentage);

                    }
                    else
                    {
                        Debug.Log(res.message);
                        switch (res.code)
                        {
                            case 6023:
                                Managers.Player.SendTokenRequest(innerAction);
                                break;

                        }
                    }

                }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());
            }
            else
            {
                Managers.Web.SendUniRequest("api/todo/" + todoMemberId.ToString(),"PATCH", null, (uwr) =>
                {
                    Response<ResponseTodoCheck> res = JsonUtility.FromJson<Response<ResponseTodoCheck>>(uwr.downloadHandler.text);

                    if (res.isSuccess)
                    {

                        this.transform.parent.parent.gameObject.GetComponent<UI_PgoalContent>().SetPercentage(res.result.percentage);
                    }
                    else
                    {
                        Debug.Log(res.message);
                        switch (res.code)
                        {
                            case 6023:
                                Managers.Player.SendTokenRequest(innerAction);
                                break;

                        }
                    }

                }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());

            }

        });

    }

    void Start()
    {
        Init();
    }

    private void LikeBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Like>("LikeView","Main");
    }

    private void EditBtnClick(PointerEventData data)
    {
        todoTitle.text = "";
        todoInputfield.interactable = true;
        todoInputfield.ActivateInputField();

    }

    private void SendTodoModifyRequest()
    {
        if (IsValidTitle(todoInputfield.text))
        {

            RequestTodoModify val = new RequestTodoModify();
            val.title = todoInputfield.text;
            Managers.Web.SendPostRequest<RequestTodoModify>("api/todo/change/" + todoMemberId.ToString(), val, (uwr) => { 
                Response<string> res = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

                Debug.Log(res.message);
                if (res.isSuccess)
                {
                    todoTitle.text = todoInputfield.text;
                    title = todoInputfield.text;
                }
                else
                {
                    switch (res.code)
                    {

                        case 6023:
                            Managers.Player.SendTokenRequest(innerAction);
                            break;
                    }
                }


            }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());

        }
    }


    public void Setting(long goalId,long todoId, string title, bool likeFlag, int likeCount, bool completeFlag) 
    {
        //goalId = id;

        //Debug.Log($"setting {goalId} {title}");

        this.goalId = goalId;
        this.todoMemberId = todoId;
        this.title = title;
        this.likeFlag = likeFlag;
        this.likeCount = likeCount;
        this.completeFlag = completeFlag;
    }

    private void SetTodo()
    {
        todoTitle.text = title;
        likeTxt.text = likeCount.ToString();
        checkToggle.isOn = completeFlag;
    }

    private bool IsValidTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return false;
        }
        try
        {
            return Regex.IsMatch(title, @"^.{0,50}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public void ClearUI()
    {
        todoTitle.text = title;
        todoInputfield.DeactivateInputField();
    }
}
