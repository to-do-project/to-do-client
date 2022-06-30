using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MemberTodoContent : UI_Base
{
    enum Buttons
    {
        like_btn,
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

    long goalId;
    long todoMemberId;
    string title;
    bool likeFlag, completeFlag;
    int likeCount;

    Text todoTitle, likeTxt;
    Toggle checkToggle;

    Action innerAction;

    const string likeImageName = "Art/UI/Button/Button(Shadow)_Line_toggle_Like_2x";
    const int fullHeart = 19;
    const int emptyHeart = 20;
    const int grayHeart = 21;

    public override void Init()
    {
        innerAction -= SendLikeClickBtnRequest;
        innerAction += SendLikeClickBtnRequest;

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Toggles));

        todoTitle = GetText((int)Texts.todo_title);
        likeTxt = GetText((int)Texts.like_txt);
        checkToggle = Get<Toggle>((int)Toggles.todoCheck_toggle);

        GameObject likeBtn = GetButton((int)Buttons.like_btn).gameObject;
        BindEvent(likeBtn, LikeBtnClick);

        SetTodo();
    }

    void Start()
    {
        Init();
    }
    
    private void SendLikeClickBtnRequest()
    {
        Managers.Web.SendUniRequest("api/todo/like/" + todoMemberId.ToString(), "POST", null, (uwr) => {

            Response<string> res = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            if (res.isSuccess)
            {

                //하트 색깔 바꾸기
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

    private void LikeBtnClick(PointerEventData data)
    {
        SendLikeClickBtnRequest();
    }

    public void Setting(long goalId, long todoId, string title, bool likeFlag, int likeCount, bool completeFlag)
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
        checkToggle.interactable = false;
    }

    private void SetLikeBtnImage()
    {
        if (likeFlag)
        {

        }
        else
        {
            if (completeFlag)
            {

            }
            else
            {

            }
        }
    }
}
