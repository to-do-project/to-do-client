using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class UI_GoalList : UI_Base
{

    enum GameObjects
    {
        Content,
    }

    GameObject goalAddbtn;
    GameObject goalParent;

    Response<List<ResponseMainTodo>> res;
    public Action<UnityWebRequest> callback;
    Action innerCallback;

    public override void Init()
    {
        //API 호출해서 목표 정보 받아옴

        //goalContent 생성
        //todoContent 생성
        //todo enable false하기

        //목표추가 버튼
        //Managers.UI.MakeSubItem<>();
        //Debug.Log("GoalList 호출");
        innerCallback -= SendGoalListRequest;
        innerCallback += SendGoalListRequest;

        Bind<GameObject>(typeof(GameObjects));
        goalParent = Get<GameObject>((int)GameObjects.Content);


        Canvas.ForceUpdateCanvases();
        
/*        //운영자 미션 생성
        if (Managers.Player.GetString(Define.MISSION_STATUS) != null)
        {
            Managers.UI.MakeSubItem<UI_SystemMission>("GoalList", goalParent.transform, "SystemMission");
        }*/

        Canvas.ForceUpdateCanvases();
        
        callback -= GoalInit;
        callback += GoalInit;

        StartCoroutine(GoalInitiate());

        //SendGoalListRequest();
        //Managers.Todo.UserTodoInstantiate(callback);
        //GoalInit();
        /*Managers.UI.MakeSubItem<UI_GgoalContent>("GoalList",goalParent.transform, "Ggoal_content");
        Managers.UI.MakeSubItem<UI_PgoalContent>("GoalList",goalParent.transform, "Pgoal_content");
        Managers.UI.MakeSubItem<UI_GoalAdd>("GoalList", goalParent.transform, "goalAdd_btn");
*/
    }

    void Start()
    {
        Init();
    }

    void SendGoalListRequest()
    {
        Managers.Todo.UserTodoInstantiate(callback);
    }

    private void GoalInit()
    {
        Transform[] childList = goalParent.GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            foreach (Transform child in childList)
            {
                if (child != goalParent.transform)
                {
                    Managers.Resource.Destroy(child.gameObject);
                }
            }
        }

        foreach (ResponseMainTodo item in Managers.Todo.goalList)
        {
            if (item.groupFlag)
            {
                UI_GgoalContent goal = Managers.UI.MakeSubItem<UI_GgoalContent>("GoalList", goalParent.transform, "Ggoal_content");
                /*                    goal.SetGoalName(item.goalTitle);
                                    goal.SetGoalRate(item.percentage.ToString());*/

                goal.SetGgoalContent(item.goalTitle, item.percentage.ToString(), item.goalId, item.getTodoMainResList, item.managerFlag);
                Canvas.ForceUpdateCanvases();
            }
            else
            {
                UI_PgoalContent goal = Managers.UI.MakeSubItem<UI_PgoalContent>("GoalList", goalParent.transform, "Pgoal_content");
                /*                    goal.SetGoalName(item.goalTitle);
                                    goal.SetGoalRate(item.percentage.ToString());*/
                goal.SetPgoalContent(item.goalTitle, item.percentage.ToString(), item.goalId, item.getTodoMainResList, item.openFlag);
                Canvas.ForceUpdateCanvases();
            }


        }
        goalAddbtn = Managers.UI.MakeSubItem<UI_GoalAdd>("GoalList", goalParent.transform, "goalAdd_btn").gameObject;
        goalAddbtn.transform.SetAsLastSibling();
    }

    private void GoalInit(UnityWebRequest request)
    {
        Debug.Log("목표 띄우기");
        res = JsonUtility.FromJson<Response<List<ResponseMainTodo>>>(request.downloadHandler.text);
        if (res.isSuccess)
        {
            Managers.Todo.goalList = res.result;

            Transform[] childList = goalParent.GetComponentsInChildren<Transform>();
            if (childList != null)
            {
                foreach(Transform child in childList)
                {
                    if (child != goalParent.transform)
                    {
                        Managers.Resource.Destroy(child.gameObject);
                    }
                }
            }

            foreach (ResponseMainTodo item in Managers.Todo.goalList)
            {
                if (item.groupFlag)
                {
                    UI_GgoalContent goal = Managers.UI.MakeSubItem<UI_GgoalContent>("GoalList", goalParent.transform, "Ggoal_content");
                    /*                    goal.SetGoalName(item.goalTitle);
                                        goal.SetGoalRate(item.percentage.ToString());*/
                   
                    goal.SetGgoalContent(item.goalTitle, item.percentage.ToString(), item.goalId, item.getTodoMainResList, item.managerFlag);
                    Canvas.ForceUpdateCanvases();
                }
                else
                {
                    UI_PgoalContent goal = Managers.UI.MakeSubItem<UI_PgoalContent>("GoalList", goalParent.transform, "Pgoal_content");
                    /*                    goal.SetGoalName(item.goalTitle);
                                        goal.SetGoalRate(item.percentage.ToString());*/
                    goal.SetPgoalContent(item.goalTitle,item.percentage.ToString(),item.goalId,item.getTodoMainResList, item.openFlag);
                    Canvas.ForceUpdateCanvases();
                }

                
            }
            goalAddbtn = Managers.UI.MakeSubItem<UI_GoalAdd>("GoalList", goalParent.transform, "goalAdd_btn").gameObject;
            goalAddbtn.transform.SetAsLastSibling();
        }
        else
        {
            if (res.code == 6000 || res.code == 6004 || res.code == 6006)
            {
                Managers.Player.SendTokenRequest(innerCallback);
            }
        }



    }


    IEnumerator GoalInitiate()
    {


        while (Managers.Todo.goalList == null)
        {
            Debug.Log("아직 로딩안됨");
            yield return null;
        }

        GoalInit();
    }
}
