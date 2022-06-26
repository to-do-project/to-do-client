using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class ResponseGroupGoalSearch
{
    public long goalId;
    public string goalTitle;
    public int goalPercentage;
    public bool openFlag;
    public List<GoalMemberDetails> goalMemberDetails;
}

[System.Serializable]
public class GoalMemberDetails
{
    public long goalMemberId;
    public string nickname;
    public int percentage;
    public bool managerFlag;
    public bool waitFlag;
    public List<GetTodoMembers> getTodoMembers;
}

[System.Serializable]
public class GetTodoMembers
{
    public long todoMemberId;
    public string todoTitle;
    public bool completeFlag;
    public int likeCount;
    public bool likeFlag;
}

public class UI_GroupGoalCreater : UI_Popup
{
    enum Buttons
    {
        exit_btn,
        GroupDelete_btn,
    }

    enum Texts
    {
        date_txt,
        GoalRate_txt,
        GoalTitle_txt,
        myName_txt,
        myGoalRate_txt,
        myGoalAttend_txt,
    }

    enum Images
    {
        myProfile_img,
    }

    enum GameObjects
    {
        OwnerTodo,
        Content,
        AddTodo,
    }

    long goalId;

    void Start()
    {
        Init();
    }

    GameObject scrollRoot;
    GameObject addTodo;

    Text goalTitle;
    Text goalRate;

    public override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        //시간 표시
        Text date = GetText((int)Texts.date_txt);
        DateTime today = DateTime.Now;
        date.text = today.ToString("yyyy") + "." + today.ToString("MM") + "." + today.ToString("dd");

        scrollRoot = Get<GameObject>((int)GameObjects.Content);
        addTodo = Get<GameObject>((int)GameObjects.AddTodo);
        addTodo.GetComponent<UI_GroupAddTodo>().SettingParent(this.GetComponent<UI_GroupGoalCreater>());

        goalTitle = GetText((int)Texts.GoalTitle_txt);
        goalRate = GetText((int)Texts.GoalRate_txt);

        GameObject deleteBtn = GetButton((int)Buttons.GroupDelete_btn).gameObject;
        BindEvent(deleteBtn, DeleteBtnClick);
    }

    private void DeleteBtnClick(PointerEventData data)
    {
        Managers.Web.SendUniRequest("api/goals?goal=" + goalId.ToString(), "DELETE", null, (uwr) => {
            Response<string> res = new Response<string>();

            res = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            Debug.Log(res.message);
            if (res.isSuccess)
            {
                Managers.Todo.SendMainGoalRequest(Managers.Player.GetString(Define.USER_ID));
                ClosePopupUI();
            }
            else
            {
                switch (res.code)
                {

                    case 6023:
                        //Managers.Player.SendTokenRequest(innerAction);
                        break;
                }
            }

        }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());
    }

    public void Setting(long goalId)
    {
        this.goalId = goalId;
        Debug.Log(goalId);
        Managers.Web.SendGetRequest("goals/", goalId.ToString(), (uwr) =>
         {
             Response<List<ResponseGroupGoalSearch>> res = JsonUtility.FromJson<Response<List<ResponseGroupGoalSearch>>>(uwr.downloadHandler.text);

             if (res.isSuccess)
             {
                 //res.result;


             }
             else
             {
                 switch (res.code)
                 {

                 }
             }


         }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());
    }

}
