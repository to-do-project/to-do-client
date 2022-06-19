using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

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
    string title;
    bool likeFlag;
    int likeCount;

    InputField todoInputfield;
    Text todoTitle,likeTxt;


    public override void Init()
    {

        Debug.Log("GoalId " + goalId);
        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Toggles));

        todoInputfield = GetInputfiled((int)InputFields.todo_inputfield);
        todoTitle = GetText((int)Texts.todo_title);
        likeTxt = GetText((int)Texts.like_txt);

        GameObject likeBtn = GetButton((int)Buttons.like_btn).gameObject;
        GameObject editBtn = GetButton((int)Buttons.edit_btn).gameObject;
        BindEvent(likeBtn, LikeBtnClick);
        BindEvent(editBtn, EditBtnClick);


        
        todoInputfield.onEndEdit.AddListener(delegate
        {
            SendTodoModifyRequest();
            todoInputfield.DeactivateInputField();
            todoInputfield.interactable = false;
        });

    }

    void Start()
    {
        Init();
    }

    private void LikeBtnClick(PointerEventData data)
    {

    }

    private void EditBtnClick(PointerEventData data)
    {
        todoInputfield.interactable = true;
        todoInputfield.ActivateInputField();

    }

    private void SendTodoModifyRequest()
    {
        if (IsValidTitle(todoInputfield.text))
        {

        }
    }


    public void Setting(long id, string title, bool likeFlag, int likeCount) 
    {
        //goalId = id;

        Debug.Log($"setting {id} {title}");

        goalId = id;
        this.title = title;
        this.likeFlag = likeFlag;
        this.likeCount = likeCount;
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
}
