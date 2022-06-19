using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class RequestTodoModify
{
    public long goalId;
    public string title;
    public string openFlag;
}

public class UI_GoalModify : UI_Popup
{
    enum Buttons
    {
        exit_btn,
        todoStore_btn,
        todoDelete_btn,
        check_btn,
    }

    enum Texts
    {
        date_txt,
        toast_txt,
    }

    enum Toggles
    {
        open_toggle,
    }

    enum InputFields
    {
        todoName_inputfield,
    }

    enum GameObjects
    {
        ToastMessage,
    }

    string title;
    bool open;
    bool group;
    long goalId;



    string openFlag;
    RequestTodoModify val;
    Response<string> modifyRes;
    Action innerAction;

    InputField goalNameInputfield;


    GameObject toastMessage;
    Text toast;

    void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();

        innerAction -= InfoGather;
        innerAction += InfoGather;


        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Toggle>(typeof(Toggles));
        Bind<GameObject>(typeof(GameObjects));
        Bind<InputField>(typeof(InputFields));

        GameObject checkbtn = GetButton((int)Buttons.check_btn).gameObject;
        BindEvent(checkbtn, CheckBtnClick);

        GameObject backBtn = GetButton((int)Buttons.exit_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI);

        GameObject todoDelBtn = GetButton((int)Buttons.todoDelete_btn).gameObject;
        BindEvent(todoDelBtn, DeleteBtnClick);

        GameObject todoStoreBtn = GetButton((int)Buttons.todoStore_btn).gameObject;
        BindEvent(todoStoreBtn, StoreBtnClick);

        goalNameInputfield = GetInputfiled((int)InputFields.todoName_inputfield);
        goalNameInputfield.placeholder.GetComponent<Text>().text = title;

        toast = GetText((int)Texts.toast_txt);
        toastMessage = Get<GameObject>((int)GameObjects.ToastMessage);
        toastMessage.SetActive(false);


        Toggle openToggle = Get<Toggle>((int)Toggles.open_toggle);
        openToggle.isOn = open;

        openToggle.onValueChanged.AddListener((bool bOn) =>
        {
            if (openToggle.isOn)
            {
                openFlag = "PRIVATE";
            }
            else
            {
                openFlag = "PUBLIC";
            }
        });
    }

    private void CheckBtnClick(PointerEventData data)
    {
        InfoGather();
        //ClosePopupUI();
    }

    private void DeleteBtnClick(PointerEventData data)
    {
        Managers.Web.SendUniRequest("api/goals?goal=" + goalId.ToString(),  "DELETE", null, (uwr) => {
            Response<string> res = new Response<string>();

            res = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            if (res.isSuccess)
            {
                ClosePopupUI();
            }
            else
            {
                switch (res.code)
                {

                    case 6023:
                        Managers.Player.SendTokenRequest(innerAction);
                        break;
                }
            }

        }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());
    }

    private void StoreBtnClick(PointerEventData data)
    {
        Managers.Web.SendUniRequest("/api/goals/archive/" + goalId.ToString(),  "POST", null, (uwr) => {
            Response<string> res = new Response<string>();

            res = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            if (res.isSuccess)
            {
                ClosePopupUI();
            }
            else
            {
                switch (res.code)
                {

                    case 6023:
                        Managers.Player.SendTokenRequest(innerAction);
                        break;
                }
            }

        }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());
    }


    private void InfoGather()
    {
        val = new RequestTodoModify();

        val.goalId = goalId;

        //InputField goalNameInputfield = GetInputfiled((int)InputFields.todoName_inputfield);
        if (IsValidTitle(goalNameInputfield.text))
        {
            val.title = goalNameInputfield.text;
        }
        else
        {
            showToastMessage("목표를 다시 입력해주세요.", 1.2f);
            return;
        }
        val.openFlag = openFlag;


        modifyRes = new Response<string>();
        Managers.Web.SendUniRequest("api/goals","PATCH",val,(uwr)=> {
            modifyRes = JsonUtility.FromJson <Response<string>>(uwr.downloadHandler.text);

            if (modifyRes.isSuccess)
            {
                Managers.Todo.SendMainGoalRequest(Managers.Player.GetString(Define.USER_ID));
                ClosePopupUI();
            }
            else
            {
                switch (modifyRes.code)
                {
                    case 5007:
                        showToastMessage("목표를 작성해주세요.", 1.2f);
                        break;
                    case 5010:
                        showToastMessage("목표명은 20자까지만 입력가능합니다.", 1.2f);
                        break;

                    case 6023:
                        Managers.Player.SendTokenRequest(innerAction);
                        break;

                }
            }
        }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());


    }


    public void Setting(long goalId, string title, bool open, bool group)
    {
        this.goalId = goalId;
        this.title = title;
        this.open = open;
        this.group = group;
        if (this.open)
        {
            openFlag = "PUBLIC";
        }
        else
        {
            openFlag = "PRIVATE";
        }
    }


    private bool IsValidTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return false;
        }
        try
        {
            return Regex.IsMatch(title, @"^.{0,20}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private void showToastMessage(string msg, float time)
    {
        StartCoroutine(showToastMessageCoroutine(msg, time));
    }

    private IEnumerator showToastMessageCoroutine(string msg, float time)
    {

        float elapsedTime = 0.0f;

        toastMessage.SetActive(true);
        toast.text = msg;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return null;

        toast.text = "";
        toastMessage.SetActive(false);
    }
}
