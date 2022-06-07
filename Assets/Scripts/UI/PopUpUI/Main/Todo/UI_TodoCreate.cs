using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TodoCreate : UI_Popup
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
    }

    private void CheckBtnClick(PointerEventData data)
    {
        InfoGather();


    }

    private void InfoGather()
    {
        InputField todoNameInputfield = GetInputfiled((int)InputFields.todoName_inputfield);
        

    }

    private void SearchFriendName()
    {
        string name = friendNameInputfield.text;

        //친구 검색 APi

        //있으면 memberList에 추가
    }

}
