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

        Managers.Todo.goalFriendAddAction -= AddfriendList;
        Managers.Todo.goalFriendAddAction += AddfriendList;


        memberList = new List<ResponseMemberFind>();

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
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
        

        friendNameInputfield.onEndEdit.AddListener(delegate { 
            SearchFriendName();
            //friendNameInputfield.caretPosition = friendNameInputfield.text.Length;
            //friendNameInputfield.ForceLabelUpdate();
        });

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

                if (memberList.Count != 0)
                {
                    openToggle.interactable = false;
                }

                openFlag = "PRIVATE";
                
            }
            else
            {
                openFlag = "PUBLIC";
            }
        });

        BindEvent(openToggle.gameObject, ToggleClick);
    }

    private void CheckBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        InfoGather();

    }

    private void ToggleClick(PointerEventData data)
    {
        if (!openToggle.interactable)
        {
            showToastMessage(toastMessage, toast, "그룹 목표는 비공개할 수 없습니다.", 1.2f);
        }
        else
        {
            Managers.Sound.PlayNormalButtonClickSound();
        }
    }

    private void FriendAddBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        /*        Transform[] childList = friendRoot.GetComponentsInChildren<Transform>();

                if (childList != null)
                {
                    foreach (Transform child in childList)
                    {
                        if (child != friendRoot.transform)
                        {
                            Debug.Log(child.name);
                            Managers.Resource.Destroy(child.gameObject);
                        }
                    }
                }*/
        Util.RemoveAllChild(friendRoot);

        foreach (ResponseMemberFind friend in memberList)
        {
            UI_AddedFriendContent content = Managers.UI.MakeSubItem<UI_AddedFriendContent>("TodoGroup", friendRoot.transform, "addedFriendContent");
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
            showToastMessage(toastMessage, toast, "목표를 다시 입력해주세요.", 1.2f);
            return;
        }
        val.openFlag = openFlag;

        if (memberList.Count != 0)
        {
            val.groupFlag = "GROUP";
            val.memberList = MakeMemberList();
        }
        else
        {
            val.groupFlag = "PERSONAL";
            val.memberList = null;
        }
        goalNameInputfield.text = "";

        ClosePopupUI();

        res = new Response<string>();
        Managers.Web.SendPostRequest<RequestGoalCreate>("api/goals", val, callback, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());

    }

    private void SearchFriendName()
    {

        string name = friendNameInputfield.text;

        /*        UI_FriendAddContent[] childList = friendRoot.GetComponentsInChildren<UI_FriendAddContent>();
                if (childList != null)
                {
                    foreach (UI_FriendAddContent child in childList)
                    {
                        if (child.gameObject != friendRoot.gameObject)
                        {
                            Debug.Log("serach friend name in "+child.name);
                            Managers.Resource.Destroy(child.gameObject);
                        }
                    }
                }*/
        Util.RemoveAllChild<UI_FriendAddContent>(friendRoot);



        if (isValidNickname(name))
        {
            Managers.Web.SendGetRequest("api/goals/users?nickname=",name,(uwr)=> { 
                Response<List<ResponseMemberFind>> res = JsonUtility.FromJson<Response<List<ResponseMemberFind>>>(uwr.downloadHandler.text);
                if (res.isSuccess)
                {

                    foreach(ResponseMemberFind friend in res.result)
                    {
                        UI_FriendAddContent content =  Managers.UI.MakeSubItem<UI_FriendAddContent>("TodoGroup",friendRoot.transform,"friendAddContent");
                        content.Setting(friend.userId,friend.nickname,friend.profileColor);
                        //memberList.Add(friend);

                    }
                }
                else
                {
                    switch (res.code) {
                        case 6023:
                            Action action = delegate ()
                            {
                                SearchFriendName();
                            };
                            Managers.Player.SendTokenRequest(action);
                            break;
                        case 5030:
                            Debug.Log("No Search Friend");
                            showToastMessage(toastMessage, toast, "해당하는 친구가 없습니다.", 1.2f);
                            break;
                    }

                }

            }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue()) ;
        }
        
        
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
                //ClosePopupUI();
            }
            else
            {
                switch (res.code)
                {

                    case 5007:
                        showToastMessage(toastMessage, toast,"목표를 작성해주세요.",1.2f);
                        break;
                    case 5010:
                        showToastMessage(toastMessage, toast,"목표명은 20자까지만 입력가능합니다.", 1.2f);
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



    public void AddfriendList(ResponseMemberFind val)
    {
        if (memberList.Find(x=> x.userId==val.userId)==null)
        {
            //Debug.Log(val.nickname);
            //friendNameInputfield.text = friendNameInputfield.text + " ";
            memberList.Add(val);

            if (openToggle.IsInteractable())
            {
                openToggle.interactable = false;
                openToggle.isOn = false;

            }

        }

        friendRoot = Get<GameObject>((int)GameObjects.friendContent);
        
        Transform[] childList = friendRoot.GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            foreach (Transform child in childList)
            {
                if (child.gameObject != friendRoot.gameObject)
                {
                    Debug.Log("AddfriendList in " + child.name);
                    Managers.Resource.Destroy(child.gameObject);
                }
            }
        }

        foreach (ResponseMemberFind friend in memberList)
        {
            UI_AddedFriendContent content = Managers.UI.MakeSubItem<UI_AddedFriendContent>("TodoGroup", friendRoot.transform, "addedFriendContent");
            content.Setting(friend.userId, friend.nickname, friend.profileColor);
        }
    }
    


    private List<long> MakeMemberList()
    {
        List<long> list = new List<long>();
        
        foreach(ResponseMemberFind item in memberList)
        {
            list.Add(item.userId);
        }

        return list;
    }

    private void DeleteFriendList(long userId)
    {

    }
}
