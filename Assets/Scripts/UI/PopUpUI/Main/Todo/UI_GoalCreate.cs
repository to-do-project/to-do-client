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

[System.Serializable]
public class ResponseMemberFind
{
    public long userId;
    public string nickname;
    public string profileColor;
}

public class UI_GoalCreate : UI_Popup
{
    enum Buttons
    {
        check_btn,
        exit_btn,
        friend_add_btn,
    }

    enum InputFields
    {
        todoName_inputfield,
        friendAdd_inputfield,
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

    enum GameObjects
    {
        ToastMessage,
        friendContent,
    }

    void Start()
    {
        Init();
    }

    InputField friendNameInputfield;
    List<ResponseMemberFind> memberList;

    Action<UnityWebRequest> callback;
    Response<string> res;

    Action innerAction;

    string openFlag = "PUBLIC";


    RequestGoalCreate val;
    GameObject checkbtn;

    GameObject toastMessage, friendRoot;
    Text toast;

    Toggle openToggle;

    public override void Init()
    {
        base.Init();
        callback -= ResponseAction;
        callback += ResponseAction;
        innerAction -= InfoGather;
        innerAction += InfoGather;

        memberList = new List<ResponseMemberFind>();

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

        friendRoot = Get<GameObject>((int)GameObjects.friendContent);

        checkbtn = GetButton((int)Buttons.check_btn).gameObject;
        BindEvent(checkbtn, CheckBtnClick);

        GameObject backBtn = GetButton((int)Buttons.exit_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI);

        GameObject friendAddBtn = GetButton((int)Buttons.friend_add_btn).gameObject;
        BindEvent(friendAddBtn, FriendAddBtnClick);

        Text date = GetText((int)Texts.date_txt);
        DateTime today = DateTime.Now;
        date.text = today.ToString("yyyy") + "." + today.ToString("MM") + "." + today.ToString("dd");

        toast = GetText((int)Texts.toast_txt);
        toastMessage = Get<GameObject>((int)GameObjects.ToastMessage);
        toastMessage.SetActive(false);

        openToggle = Get<Toggle>((int)Toggles.open_toggle);
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
    }

    private void FriendAddBtnClick(PointerEventData data)
    {
        Transform[] childList = friendRoot.GetComponentsInChildren<Transform>();

        if (childList != null)
        {
            foreach (Transform child in childList)
            {
                if (child != friendRoot.transform)
                {
                    Managers.Resource.Destroy(child.gameObject);
                }
            }
        }

        foreach (ResponseMemberFind friend in memberList)
        {
            UI_FriendAddContent content = Managers.UI.MakeSubItem<UI_FriendAddContent>("TodoGroup", friendRoot.transform, "friendAddContent");
            content.Setting(friend.userId, friend.nickname, friend.profileColor);
        }
    }

    private void InfoGather()
    {
        val = new RequestGoalCreate();

        InputField goalNameInputfield = GetInputfiled((int)InputFields.todoName_inputfield);
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

        if (memberList.Count != 0)
        {
            val.groupFlag = "GROUP";
            //val.memberList = memberList;
        }
        else
        {
            val.groupFlag = "PERSONAL";
            val.memberList = null;
        }

        res = new Response<string>();
        Managers.Web.SendPostRequest<RequestGoalCreate>("api/goals", val, callback, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());
        goalNameInputfield.text = "";
    }

    private void SearchFriendName()
    {
        string name = friendNameInputfield.text;

        if (isValidNickname(name))
        {
            openToggle.isOn = false;
            Managers.Web.SendUniRequest("api/goals/users?nickname=" + name,"POST",null,(uwr)=> { 
                Response<List<ResponseMemberFind>> res = JsonUtility.FromJson<Response<List<ResponseMemberFind>>>(uwr.downloadHandler.text);
                if (res.isSuccess)
                {
                    Transform[] childList = friendRoot.GetComponentsInChildren<Transform>();

                    if (childList != null)
                    {
                        foreach(Transform child in childList)
                        {
                            if (child != friendRoot.transform)
                            {
                                Managers.Resource.Destroy(child.gameObject);
                            }
                        }
                    }

                    foreach(ResponseMemberFind friend in res.result)
                    {
                        UI_FriendAddContent content =  Managers.UI.MakeSubItem<UI_FriendAddContent>("TodoGroup",friendRoot.transform,"friendAddContent");
                        content.Setting(friend.userId,friend.nickname,friend.profileColor);
                        memberList.Add(friend);
                    }
                }
                else
                {
                    switch (res.code) { }

                }

            }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue()) ;
        }
        //친구 검색 APi
        
    }


    private void ResponseAction(UnityWebRequest request)
    {
        if (res != null)
        {
            res = JsonUtility.FromJson<Response<string>>(request.downloadHandler.text);

            Debug.Log(res.code);
            if (res.isSuccess)
            {
                Managers.Todo.SendMainGoalRequest(Managers.Player.GetString(Define.USER_ID));
                ClosePopupUI();
            }
            else
            {
                switch (res.code)
                {

                    case 5007:
                        showToastMessage("목표를 작성해주세요.",1.2f);
                        break;
                    case 5010:
                        showToastMessage("목표명은 20자까지만 입력가능합니다.", 1.2f);
                        break;

                    case 6023:
                        Managers.Player.SendTokenRequest(innerAction);
                        break;



                }
            }
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
            return Regex.IsMatch(title, @"^.{0,20}$",
                RegexOptions.None,TimeSpan.FromMilliseconds(250));
        }
        catch(RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private bool isValidNickname(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
        {
            return false;
        }
        try
        {
            return Regex.IsMatch(nickname, @"^.{0,8}$",
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
