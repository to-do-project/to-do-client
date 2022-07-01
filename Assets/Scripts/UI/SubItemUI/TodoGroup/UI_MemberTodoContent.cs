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

    long goalId;
    long todoMemberId;
    string title;
    bool likeFlag, completeFlag;
    int likeCount;


    Text todoTitle, likeTxt;
    Toggle checkToggle;

    Action innerAction;
    GameObject likeBtn, likeNumBtn;

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

        likeBtn = GetButton((int)Buttons.like_btn).gameObject;
        BindEvent(likeBtn, LikeBtnClick);

        likeNumBtn = GetButton((int)Buttons.like_num).gameObject;
        BindEvent(likeNumBtn, LikeNumBtnClick);

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
                likeBtn.GetComponent<Button>().interactable = false;
                ClearEvent(likeBtn, LikeBtnClick);

                ChangeLikeBtnImage(fullHeart);
                likeTxt.text = (likeCount + 1).ToString();
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


        if (completeFlag)
        {
            if (!likeFlag)
            {
                SendLikeClickBtnRequest();
            }
        }
        else
        {

            UI_Cheer ui =  Managers.UI.ShowPopupUI<UI_Cheer>("CheerView","Main");
            ui.Setting(todoMemberId, this.GetComponentInParent<UI_MemberGoalContent>().GetNickname());
        }
    }

    private void LikeNumBtnClick(PointerEventData data)
    {
        UI_Like ui = Managers.UI.ShowPopupUI<UI_Like>("LikeView", "Main");
        ui.Setting(todoMemberId.ToString());
       // Debug.Log("todoMember id " + todoMemberId.ToString());
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
        checkToggle.interactable = false;

        SetLikeBtnImage();

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    private void SetLikeBtnImage()
    {
        int index;
        if (completeFlag)
        {
            //상대가 투두 완료 했으면
            if (likeFlag)
            {
                //내가 좋아요 눌렀음
                index = fullHeart;
                likeBtn.GetComponent<Button>().interactable = false;
                ClearEvent(likeBtn, LikeBtnClick);
            }
            else
            {
                //내가 좋아요 안눌렀음
                index = grayHeart;
                likeBtn.GetComponent<Button>().interactable = true;
                BindEvent(likeBtn, LikeBtnClick);
            }

            likeNumBtn.GetComponent<Button>().interactable = true;
            BindEvent(likeNumBtn, LikeNumBtnClick);
        }
        else
        {
            index = emptyHeart;

            likeNumBtn.GetComponent<Button>().interactable = false;
            ClearEvent(likeNumBtn, LikeNumBtnClick);

        }

        likeBtn.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>(likeImageName)[index];

    }

    private void ChangeLikeBtnImage(int index)
    {
        likeBtn.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>(likeImageName)[index];
    }
}
