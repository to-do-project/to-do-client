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
    public long userId;
    public string nickname;
    public string profileColor;
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
        done_btn,
        cancle_btn,
    }

    enum Texts
    {
        date_txt,
        GoalRate_txt,
        GoalTitle_txt,
        GroupDelete_txt,
        del_res_txt,
    }

    enum GameObjects
    {
        Content,
        AddTodo,
        GroupDeleteView,
        OwnerGoal_content,
        OwnerTodo,
    }


    long goalId;

    void Start()
    {
        Init();
    }

    GameObject scrollRoot;
    GameObject addTodo;
    GameObject deleteView;
    GameObject ownerGoal;
    GameObject ownerTodo;

    Text goalTitle;
    Text goalRate;
    Text deletbtnTxt;
    Text delresTxt;
    bool isManager;

    public override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        //시간 표시
        Text date = GetText((int)Texts.date_txt);
        DateTime today = DateTime.Now;
        date.text = today.ToString("yyyy") + "." + today.ToString("MM") + "." + today.ToString("dd");

        scrollRoot = Get<GameObject>((int)GameObjects.Content);
        //addTodo = Get<GameObject>((int)GameObjects.AddTodo);
        //addTodo.GetComponent<UI_GroupAddTodo>().SettingParent(this.GetComponent<UI_GroupGoalCreater>());
        ownerGoal = Get<GameObject>((int)GameObjects.OwnerGoal_content);
        ownerTodo = Get<GameObject>((int)GameObjects.OwnerTodo);

        deletbtnTxt = GetText((int)Texts.GroupDelete_txt);
        delresTxt = GetText((int)Texts.del_res_txt);

        goalTitle = GetText((int)Texts.GoalTitle_txt);
        goalRate = GetText((int)Texts.GoalRate_txt);

        deleteView = Get<GameObject>((int)GameObjects.GroupDeleteView);

        deleteView.SetActive(false);

        GameObject doneBtn = GetButton((int)Buttons.done_btn).gameObject;
        BindEvent(doneBtn, DoneBtnClick);

        GameObject cancleBtn = GetButton((int)Buttons.cancle_btn).gameObject;
        BindEvent(cancleBtn, CancleBtnClick);

        GameObject deleteBtn = GetButton((int)Buttons.GroupDelete_btn).gameObject;
        BindEvent(deleteBtn, DeleteBtnClick);

        GameObject exitBtn = GetButton((int)Buttons.exit_btn).gameObject;
        BindEvent(exitBtn, ClosePopupUI);
    }

    private void CancleBtnClick(PointerEventData data)
    {
        deleteView.SetActive(false);

    }

    private void DoneBtnClick(PointerEventData data)
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

    private void DeleteBtnClick(PointerEventData data)
    {
        
        deleteView.SetActive(true);
    }

    public void Setting(long goalId)
    {
        this.goalId = goalId;
        Debug.Log(goalId);
        Managers.Web.SendGetRequest("api/goals/", goalId.ToString(), (uwr) =>
         {
             Response<ResponseGroupGoalSearch> res = JsonUtility.FromJson<Response<ResponseGroupGoalSearch>>(uwr.downloadHandler.text);

             if (res.isSuccess)
             {
                 //자식 클리어
                 //Transform[] childList = 


                 //res.result;
                 goalTitle.text = res.result.goalTitle;
                 goalRate.text = res.result.goalPercentage.ToString() + "%";


                 Canvas.ForceUpdateCanvases();

                 //멤버별 개인
                 foreach (GoalMemberDetails item in res.result.goalMemberDetails)
                 {
                     //본인
                     if (item.userId == Managers.Player.GetUserId())
                     {

                         ownerGoal.GetComponent<UI_OwnerGoalContent>().SetGoalContent(item.nickname,item.profileColor, item.percentage.ToString(), res.result.goalId, item.getTodoMembers);
                         //매니저면
                         if (item.managerFlag)
                         {
                             addTodo = Managers.UI.MakeSubItem<UI_GroupAddTodo>("TodoGroup", ownerTodo.transform, "AddTodo").gameObject;
                             addTodo.GetComponent<UI_GroupAddTodo>().Setting(res.result.goalId);
                             deletbtnTxt.text = "그룹 목표 삭제하기";
                             isManager = true;
                             delresTxt.text = "그룹 목표를 삭제하시겠습니까?";
                             addTodo.transform.SetAsLastSibling();
                         }
                         else
                         {
                             deletbtnTxt.text = "그룹 탈퇴하기";
                             isManager = false;
                             delresTxt.text = "그룹 목표를 탈퇴하시겠습니까?";
                         }
                     }
                     else    //본인 아니면
                     {
                         UI_MemberGoalContent ui = Managers.UI.MakeSubItem<UI_MemberGoalContent>("TodoGroup", scrollRoot.transform, "MemberGoal_content") ;
                         ui.SetGoalContent(item.nickname, item.profileColor, item.percentage.ToString(), res.result.goalId, item.getTodoMembers, item.waitFlag);

                     }

                     

                 }


                 Canvas.ForceUpdateCanvases();

             }
             else
             {
                 switch (res.code)
                 {
                     case 6023:
                         break;
                 }
             }


         }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());
    }

}
