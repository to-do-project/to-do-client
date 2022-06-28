using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GroupGoal : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
        Accept_btn,
        Decept_btn,
    }

    enum Texts
    {
        GoalTitle_txt,
    }

    GameObject content = null;
    Text goalTitle = null;
    int count = 0;
    long id = 0;
    string text = null;
    bool check = false;
    public List<ResponseGoalRequestList> Members { private get; set; } = null;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Text>(typeof(Texts));
        goalTitle = GetText((int)Texts.GoalTitle_txt);

        if (content == null)
        {
            content = GameObject.Find("GoalContainer");
        }

        if (text != null)
        {
            goalTitle.text = text;
        }

        if(Members != null)
        {
            foreach(var tmp in Members)
            {
                if (count >= 8) break;
                GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ScrollContents/GoalContent"));
                obj.transform.SetParent(content.transform, false);

                GoalContent goal = obj.GetComponent<GoalContent>();
                goal.SetImage(tmp.profileColor);
                goal.SetName(tmp.nickname);
                goal.SetManager(tmp.managerFlag);
                count++;
            }
        }
    }

    public void Setid(long id)
    {
        this.id = id;
    }

    public void SetText(string text)
    {
        this.text = text;
        if(goalTitle != null)
        {
            goalTitle.text = text;
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Accept_btn, (data) => { WebAccept(true); });

        SetBtn((int)Buttons.Decept_btn, (data) => { WebAccept(false); });
    }

    void WebAccept(bool accept)
    {
        if (check) return;
        check = true;
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/goals/" + id + "?accept=" + (accept ? 1 : 0), "PATCH", null, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                Managers.Todo.UserTodoInstantiate((uwr) => {
                    FindObjectOfType<UI_GoalList>().callback.Invoke(uwr);
                    Managers.UI.CloseAllPopupUI();
                });
            }
            else if (response.code == 6000)
            {
                string[] hN = { Define.JWT_ACCESS_TOKEN,
                                "User-Id" };
                string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                                Managers.Player.GetString(Define.USER_ID) };

                Managers.Web.SendUniRequest("api/goals/" + id + "?accept=" + (accept ? 1 : 0), "PATCH", null, (uwr) => {
                    Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
                    if (response.isSuccess)
                    {
                        Managers.Todo.UserTodoInstantiate((uwr) => {
                            FindObjectOfType<UI_GoalList>().callback.Invoke(uwr);
                            Managers.UI.CloseAllPopupUI();
                        });
                    }
                    else if (response.code == 6000)
                    {
                        Debug.Log("토큰 발급 실패");
                    }
                    else
                    {
                        Debug.Log(response.message);
                    }
                }, hN, hV);
            }
            else
            {
                Debug.Log(response.message);
            }
            check = false;
        }, hN, hV);
    }


    private void Start()
    {
        Init();
    }
}
