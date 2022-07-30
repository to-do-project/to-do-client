using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

// UI_Goal�� ��ũ��Ʈ�� FriendUI�� �°� ������ ��ũ��Ʈ
// ���ʿ��� �ڵ� ���� �� ������Ʈ �� ��ũ��Ʈ �� ���ۼ� �� ��ũ��Ʈ
public class UI_FriendGoal : UI_Base
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
    UI_FriendUI parent;

    public override void Init()
    {
        //API ȣ���ؼ� ��ǥ ���� �޾ƿ�

        //goalContent ����
        //todoContent ����
        //todo enable false�ϱ�

        //��ǥ�߰� ��ư
        //Managers.UI.MakeSubItem<>();
        //Debug.Log("GoalList ȣ��");
        innerCallback -= SendGoalListRequest;
        innerCallback += SendGoalListRequest;

        Bind<GameObject>(typeof(GameObjects));
        goalParent = Get<GameObject>((int)GameObjects.Content);


        Canvas.ForceUpdateCanvases();

        /*        //��� �̼� ����
                if (Managers.Player.GetString(Define.MISSION_STATUS) != null)
                {
                    Managers.UI.MakeSubItem<UI_SystemMission>("GoalList", goalParent.transform, "SystemMission");
                }*/

        Canvas.ForceUpdateCanvases();

        callback -= GoalInit;
        callback += GoalInit;

        SendGoalListRequest();

        //StartCoroutine(GoalInitiate());

        //Managers.Todo.UserTodoInstantiate(callback);
        //GoalInit();
        /*Managers.UI.MakeSubItem<UI_GgoalContent>("GoalList",goalParent.transform, "Ggoal_content");
        Managers.UI.MakeSubItem<UI_PgoalContent>("GoalList",goalParent.transform, "Pgoal_content");
        Managers.UI.MakeSubItem<UI_GoalAdd>("GoalList", goalParent.transform, "goalAdd_btn");
*/

        parent = FindObjectOfType<UI_FriendUI>();
    }

    void Start()
    {
        Init();
    }

    void SendGoalListRequest()
    {
        UserTodoInstantiate(callback);
    }


    void UserTodoInstantiate(Action<UnityWebRequest> callback)
    {
        SendMainGoalRequest(FindObjectOfType<UI_FriendUI>().GetUserId().ToString(), callback);
    }

    void SendMainGoalRequest(string userId, Action<UnityWebRequest> callback)
    {
        res = new Response<List<ResponseMainTodo>>();

        Managers.Web.SendGetRequest("api/goals/main/", userId, callback, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());
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
                UI_GgoalFriendContent goal = Managers.UI.MakeSubItem<UI_GgoalFriendContent>("GoalList", goalParent.transform, "Ggoal_FriendContent");
                /*                    goal.SetGoalName(item.goalTitle);
                                    goal.SetGoalRate(item.percentage.ToString());*/

                goal.SetGgoalContent(item.goalTitle, item.percentage.ToString(), item.goalId, item.getTodoMainResList, item.managerFlag);
                Canvas.ForceUpdateCanvases();
            }
            else
            {
                UI_PgoalFriendContent goal = Managers.UI.MakeSubItem<UI_PgoalFriendContent>("GoalList", goalParent.transform, "Pgoal_FriendContent");
                /*                    goal.SetGoalName(item.goalTitle);
                                    goal.SetGoalRate(item.percentage.ToString());*/
                goal.SetPgoalContent(item.goalTitle, item.percentage.ToString(), item.goalId, item.getTodoMainResList, item.openFlag);
                Canvas.ForceUpdateCanvases();
            }


        }
    }

    private void GoalInit(UnityWebRequest request)
    {
        Debug.Log("��ǥ ����");
        res = JsonUtility.FromJson<Response<List<ResponseMainTodo>>>(request.downloadHandler.text);
        if (res.isSuccess)
        {
            Managers.Todo.goalList = res.result;

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
                    UI_GgoalFriendContent goal = Managers.UI.MakeSubItem<UI_GgoalFriendContent>("GoalList", goalParent.transform, "Ggoal_FriendContent");
                    /*                    goal.SetGoalName(item.goalTitle);
                                          goal.SetGoalRate(item.percentage.ToString());*/

                    goal.SetGgoalContent(item.goalTitle, item.percentage.ToString(), item.goalId, item.getTodoMainResList, item.managerFlag);
                    Canvas.ForceUpdateCanvases();
                }
                else
                {
                    UI_PgoalFriendContent goal = Managers.UI.MakeSubItem<UI_PgoalFriendContent>("GoalList", goalParent.transform, "Pgoal_FriendContent");
                    /*                    goal.SetGoalName(item.goalTitle);
                                          goal.SetGoalRate(item.percentage.ToString());*/
                    goal.SetPgoalContent(item.goalTitle, item.percentage.ToString(), item.goalId, item.getTodoMainResList, item.openFlag);
                    Canvas.ForceUpdateCanvases();
                }


            }
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
            Debug.Log("���� �ε��ȵ�");
            yield return null;
        }

        GoalInit();
    }
}
