using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RequestGoalCreate
{
    public string title;
    public string openFlag;
    public string groupFlag;
    public List<long> memberList;
}

public class UI_GoalCreate : UI_Popup
{
    enum Buttons
    {
        check_btn,
        exit_btn,
    }

    enum InputFields
    {
        todoName_inputfield,
        friendAdd_inputfield,
    }

    enum Texts
    {
        date_txt,
    }

    enum Toggles
    {
        open_toggle,
    }

    enum GameObjects
    {
        ToastMessage,
    }

    void Start()
    {
        Init();
    }

    InputField friendNameInputfield;
    List<long> memberList;

    Action<UnityWebRequest> callback;
    Response<string> res;
    string openFlag = "PUBLIC";

    RequestGoalCreate val;

    public override void Init()
    {
        base.Init();

        Canvas canvas = GetComponent<Canvas>();
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }

        else
        {
            Debug.Log($"{UIcam.name}");
        }

        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Toggle>(typeof(Toggles));

        friendNameInputfield = GetInputfiled((int)InputFields.friendAdd_inputfield);
        friendNameInputfield.onEndEdit.AddListener(delegate { SearchFriendName(); });

        GameObject checkbtn = GetButton((int)Buttons.check_btn).gameObject;
        BindEvent(checkbtn, CheckBtnClick);

        GameObject backBtn = GetButton((int)Buttons.exit_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI);

        Text date = GetText((int)Texts.date_txt);
        DateTime today = DateTime.Now;
        date.text = today.ToString("yyyy") + "." + today.ToString("mm") + "." + today.ToString("dd");


        Toggle openToggle = Get<Toggle>((int)Toggles.open_toggle);
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
        //goal 추가 API 날리기

        if (val != null)
        {
            //API 날리기
            //PlayerManager tokenrequest 날리고 이너 액션으로 생성 API 날리기
            //Managers.Web.SendGetRequest("/api/goals",null,);
            val = null;
        }


        ClosePopupUI();
    }

    private void InfoGather()
    {
        val = new RequestGoalCreate();

        InputField todoNameInputfield = GetInputfiled((int)InputFields.todoName_inputfield);
        if (IsValidTitle(todoNameInputfield.text))
        {
            val.title = todoNameInputfield.text;
        }
        else
        {

        }
        val.openFlag = openFlag;

        if (memberList.Count != 0)
        {
            val.groupFlag = "GROUP";
            val.memberList = memberList;
        }
        else
        {
            val.groupFlag = "PERSONAL";
            val.memberList = null;
        }

        


    }

    private void SearchFriendName()
    {
        string name = friendNameInputfield.text;

        //친구 검색 APi

        //있으면 memberList에 추가
    }


    private void ResponseAction()
    {
        if (res != null)
        {

            res = null;
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
            return Regex.IsMatch(title, @"^.{0,50}$",
                RegexOptions.None,TimeSpan.FromMilliseconds(250));
        }
        catch(RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
