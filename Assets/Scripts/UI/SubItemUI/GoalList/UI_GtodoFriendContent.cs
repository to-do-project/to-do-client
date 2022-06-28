using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GtodoFriendContent : UI_Base
{
    enum Buttons
    {
        friendlike_btn,
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
    bool likeFlag, completeFlag, clicked = false;
    int likeCount;

    Text todoTitle, likeTxt;
    Toggle checkToggle;

    Action innerAction;

    UI_FriendUI parent = null;

    GameObject likeBtn = null;

    const string likeImageName = "Art/UI/Button/Button(Shadow)_Line_toggle_Like_2x";
    const int fullHeart = 19;
    const int emptyHeart = 20;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Toggles));

        todoTitle = GetText((int)Texts.todo_title);
        likeTxt = GetText((int)Texts.like_txt);
        checkToggle = Get<Toggle>((int)Toggles.todoCheck_toggle);

        likeBtn = GetButton((int)Buttons.friendlike_btn).gameObject;
        SetLikeBtnImage();

        SetTodo();

        parent = FindObjectOfType<UI_FriendUI>();
    }

    void Start()
    {
        Init();
    }

    private void LikeBtnClick(PointerEventData data)
    {
        Debug.Log("실행");
        if (checkToggle.isOn)
        {
            Ex_Like();
        }
        else
        {
            parent.OnFightingView(todoMemberId);
        }
    }

    void Ex_Like()
    {
        if (clicked) return;
        clicked = true;
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/todo/like/" + todoMemberId, "POST", goalId, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            if (response.isSuccess)
            {
                Debug.Log(response.result);
                likeFlag = true;
                SetLikeBtnImage();
                clicked = false;
            }
            else if (response.code == 6000)
            {
                clicked = false;
                Managers.Player.SendTokenRequest(Ex_Like);
            }
            else
            {
                Debug.Log(response.message);
                clicked = false;
            }
        }, hN, hV);
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

    private void SetLikeBtnImage()
    {
        //like 버튼 이미지 변경
        int index;
        if (likeFlag)
        {
            Debug.Log("full heart");
            index = fullHeart;
            likeBtn.GetComponent<Button>().interactable = true;
            ClearEvent(likeBtn, LikeBtnClick);
        }
        else
        {
            Debug.Log("empty heart");
            index = emptyHeart;
            likeBtn.GetComponent<Button>().interactable = false;
            BindEvent(likeBtn, LikeBtnClick);
        }


        likeBtn.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>(likeImageName)[index];
    }
}
