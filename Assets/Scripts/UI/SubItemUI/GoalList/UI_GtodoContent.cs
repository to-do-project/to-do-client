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
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Toggles));

        todoTitle = GetText((int)Texts.todo_title);
        likeTxt = GetText((int)Texts.like_txt);
        checkToggle = Get<Toggle>((int)Toggles.todoCheck_toggle);

        likeBtn = GetButton((int)Buttons.like_btn).gameObject;
        likeNumBtn = GetButton((int)Buttons.like_num).gameObject;
        likeBtn.GetComponent<Button>().interactable = false;
        //BindEvent(likeBtn, LikeBtnClick);
        BindEvent(likeNumBtn, LikeNumBtnClick);
       
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

    }

    private void LikeNumBtnClick(PointerEventData data)
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
        Canvas.ForceUpdateCanvases();

        todoTitle.text = title;
        likeTxt.text = likeCount.ToString();
        checkToggle.isOn = completeFlag;

        SetLikeBtnImage();

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    private void SetLikeBtnImage()
    {
        //like 버튼 이미지 변경
        int index;
        if (likeFlag)
        {
            Debug.Log("full heart");
            index = fullHeart;
            likeNumBtn.GetComponent<Button>().interactable = true;
            BindEvent(likeNumBtn, LikeNumBtnClick);
        }
        else
        {
            Debug.Log("empty heart");
            index = emptyHeart;
            likeNumBtn.GetComponent<Button>().interactable = false;
            ClearEvent(likeNumBtn, LikeNumBtnClick);
        }


        likeBtn.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>(likeImageName)[index];
    }

}
