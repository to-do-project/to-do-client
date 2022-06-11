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
    public string groupFlag;
    public int percentage;
    public string memberRole;
    public List<TodoItem> getIdoMainResList;
}

public class TodoItem
{
    public long todoMemberId;
    public string todoTitle;
    public string completeFlag;
    public int likeCount;
    public int likeFlag;
}

public class TodoManager
{
    public List<ResponseMainTodo> goalList; //목표 리스트
    
    Response<ResponseMainTodo> res;
    Action innerAction;
    Action<UnityWebRequest> callback;



    public void Init()
    {


    }


    public void SendMainGoalRequest()
    {
        res = new Response<ResponseMainTodo>();

        //Managers.Web.SendGetRequest("api/goals",)
    }

    

}
