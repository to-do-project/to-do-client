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

    bool firstInitiate = false;

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

        goalAddbtn = Managers.UI.MakeSubItem<UI_GoalAdd>("GoalList", goalParent.transform, "goalAdd_btn").gameObject;

        StartCoroutine(GoalInitiate());

        firstInitiate = true;
    }

    void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        //���� ó�� ���� �� �����ϰ� ��� ��쿡 enable�Ǹ� ����
        if (firstInitiate)
        {
            StartCoroutine(GoalInitiate());
        }
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
        goalAddbtn.transform.SetAsLastSibling();
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
            goalAddbtn.transform.SetAsLastSibling();
        }
        else
        {
            if (res.code == 6000 || res.code == 6004 || res.code == 6006)
            {
                Managers.Player.SendTokenRequest(innerCallback);
            }
            goalAddbtn.transform.SetAsLastSibling();
        }



    }


    IEnumerator GoalInitiate()
    {
        float timer = 0.0f;
        float delaytTime = 5.0f;
        bool isFailed = false;

        while (Managers.Todo.goalList == null)
        {
            Debug.Log("���� �ε��ȵ�");
            timer += Time.deltaTime;
            if (timer > delaytTime)
            {
                isFailed = true;
                break;
            }
            yield return null;
        }

        if (!isFailed)
        {
            GoalInit();
        }

    }
}
