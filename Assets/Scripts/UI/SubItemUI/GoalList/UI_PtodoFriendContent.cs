using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class UI_PtodoFriendContent : UI_Base
{
    enum Buttons
    {
        friendlike_btn,
        like_num,
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
    bool likeFlag, completeFlag, clicked = false, isInit = false;
    int likeCount;

    InputField todoInputfield;
    Text todoTitle, likeTxt;
    Toggle checkToggle;
    UI_FriendUI parent;
    GameObject likeBtn = null, likeNumBtn = null;

    const string likeImageName = "Art/UI/Button/Button(Shadow)_Line_toggle_Like_2x";
    const int pinkHeart = 19;
    const int emptyHeart = 20;
    const int grayHeart = 21;

    public override void Init()
    {

        //Debug.Log("GoalId " + goalId);
        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Toggles));

        todoInputfield = GetInputfiled((int)InputFields.todo_inputfield);
        todoTitle = GetText((int)Texts.todo_title);
        likeTxt = GetText((int)Texts.like_txt);
        checkToggle = Get<Toggle>((int)Toggles.todoCheck_toggle);


        likeBtn = GetButton((int)Buttons.friendlike_btn).gameObject;
        likeNumBtn = GetButton((int)Buttons.like_num).gameObject;
        SetLikeBtnImage();

        SetTodo();

        todoInputfield.DeactivateInputField();
        todoInputfield.interactable = false;

        parent = FindObjectOfType<UI_FriendUI>();
    }

    void Start()
    {
        Init();
    }

    private void LikeBtnClick(PointerEventData data)
    {
        if(checkToggle.isOn)
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
        if (likeFlag) return;
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
                likeCount++;
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
        Canvas.ForceUpdateCanvases();

        todoTitle.text = title;
        likeTxt.text = likeCount.ToString();
        checkToggle.isOn = completeFlag;

        SetLikeBtnImage();

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    public void ClearUI()
    {
        todoTitle.text = title;
        todoInputfield.DeactivateInputField();
    }

    private void SetLikeBtnImage()
    {
        //like 버튼 이미지 변경
        int index;
        if (checkToggle.isOn == false)
        {
            index = emptyHeart;
            likeNumBtn.SetActive(false);
        }
        else if (likeCount == 0 && likeFlag == false)
        {
            index = grayHeart;
            likeNumBtn.SetActive(false);
        }
        else if (likeCount != 0 && likeFlag)
        {
            index = grayHeart;
            likeNumBtn.SetActive(true);
            likeNumBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            index = pinkHeart;
            likeNumBtn.SetActive(true);
            likeNumBtn.GetComponent<Button>().interactable = true;
        }

        if (isInit == false)
            BindEvent(likeBtn, LikeBtnClick);
        isInit = true;

        likeBtn.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>(likeImageName)[index];
    }
}
