using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


//API response request class 만들기
public class ResponseMainTodo
{
    public long goalId;
    public string goalTitle;
    public bool groupFlag;
    public int percentage;
    public bool managerFlag;
    public List<TodoItem> getIdoMainResList;
}

public class TodoItem
{
    public long todoMemberId;
    public string todoTitle;
    public bool completeFlag;
    public int likeCount;
    public bool likeFlag;
}

public class TodoManager
{
    public List<ResponseMainTodo> goalList; //목표 리스트
    
    Response<List<ResponseMainTodo>> res;
    Action innerAction;
    Action<UnityWebRequest> callback;



    public void Init()
    {
        innerAction -= UserTodoInstantiate;
        innerAction += UserTodoInstantiate;

        callback -= MainGoalResponseAction;
        callback += MainGoalResponseAction;
    }


    public void SendMainGoalRequest(string userId)
    {
        res = new Response<List<ResponseMainTodo>>();


        Managers.Web.SendGetRequest("api/goals",userId ,callback, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());

        
    }

    public void UserTodoInstantiate()
    {
        SendMainGoalRequest(Managers.Player.GetString(Define.USER_ID));
    }

    void MainGoalResponseAction(UnityWebRequest request)
    {
        if (res != null)
        {
            res = JsonUtility.FromJson<Response<List<ResponseMainTodo>>>(request.downloadHandler.text);

            if (res.isSuccess)
            {
                goalList = res.result;
            }
            else
            {
                if(res.code==6000 || res.code==6004 || res.code == 6006)
                {
                    Managers.Player.SendTokenRequest(innerAction);
                }
            }
        }
    }
}
