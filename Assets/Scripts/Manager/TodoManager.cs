using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


//API response request class 만들기
[System.Serializable]
public class ResponseMainTodo
{
    public long goalId;
    public string goalTitle;
    public bool groupFlag;
    public int percentage;
    public bool managerFlag;
    public bool openFlag;
    public List<TodoItem> getTodoMainResList;
}
[System.Serializable]
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
    Action<UnityWebRequest> callback;

    UI_GoalList goalListGameObject;

    public void Init()
    {

        callback -= MainGoalResponseAction;
        callback += MainGoalResponseAction;
    }


    public void SendMainGoalRequest(string userId, Action<UnityWebRequest> callback)
    {
        res = new Response<List<ResponseMainTodo>>();



        Managers.Web.SendGetRequest("api/goals/main/", userId ,callback, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());

        
    }

    public void SendMainGoalRequest(string userId)
    {
        res = new Response<List<ResponseMainTodo>>();

        Managers.Web.SendGetRequest("api/goals/main/", userId, callback, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());

    }

    public void UserTodoInstantiate(Action<UnityWebRequest> callback)
    {
        SendMainGoalRequest(Managers.Player.GetString(Define.USER_ID), callback);
    }

    void MainGoalResponseAction(UnityWebRequest request)
    {
        if (res != null)
        {
            res = JsonUtility.FromJson<Response<List<ResponseMainTodo>>>(request.downloadHandler.text);

            if (res.isSuccess)
            {
                goalList = res.result;

                goalListGameObject = GameObject.Find("GoalList").GetComponent<UI_GoalList>();
                goalListGameObject.callback.Invoke(request);
            }
            else
            {
                if (res.code == 6000 || res.code == 6004 || res.code == 6006)
                {
                    //Managers.Player.SendTokenRequest(innerAction);
                }
            }
        }
    }
}
