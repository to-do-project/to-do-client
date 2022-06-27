using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GtodoContent : UI_Base
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

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Toggles));

        todoTitle = GetText((int)Texts.todo_title);
        likeTxt = GetText((int)Texts.like_txt);
        checkToggle = Get<Toggle>((int)Toggles.todoCheck_toggle);

        GameObject likeBtn = GetButton((int)Buttons.like_btn).gameObject;
        BindEvent(likeBtn, LikeBtnClick);

        SetTodo();

        checkToggle.onValueChanged.AddListener((bool bOn) => {

            string flag = bOn ? "true" : "false";


            Managers.Web.SendUniRequest("api/todo/" + todoMemberId.ToString() + "?flag=" + flag, "PATCH", null, (uwr) =>
            {
                Response<ResponseTodoCheck> res = JsonUtility.FromJson<Response<ResponseTodoCheck>>(uwr.downloadHandler.text);

                if (res.isSuccess)
                {

                    this.transform.parent.parent.gameObject.GetComponent<UI_GgoalContent>().SetPercentage(res.result.percentage);
                }
                else
                {
                    Debug.Log(res.message);
                    switch (res.code)
                    {
                        case 6023:
                            //Managers.Player.SendTokenRequest(innerAction);
                            break;

                    }
                }

            }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());

        });
    }

    void Start()
    {
        Init();
    }

    private void LikeBtnClick(PointerEventData data)
    {
        UI_Like ui = Managers.UI.ShowPopupUI<UI_Like>("LikeView", "Main");
        ui.Setting(todoMemberId.ToString());
        Debug.Log("todoMember id " + todoMemberId.ToString());
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
    }

}
