using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UI_Cheer : UI_Popup
{
    enum Texts
    {
        cheer_txt
    }

    enum Buttons
    {
        cheer_btn
    }

    long todoMemberId;
    string nickname;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

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

        Text cheerTxt = GetText((int)Texts.cheer_txt);
        cheerTxt.text = nickname + "님이 아직 to-do를 완료하지 않았습니다.";

        GameObject cheerBtn = GetButton((int)Buttons.cheer_btn).gameObject;
        BindEvent(cheerBtn, CheerBtnClick);
    }

    void CheerBtnClick(PointerEventData data)
    {
        //응원 API 날리기
        Managers.Sound.PlayNormalButtonClickSound();
        Managers.Web.SendGetRequest("api/todo/",todoMemberId.ToString(), (uwr)=> {

            Response<string> res = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            if (res.isSuccess)
            {
                ClosePopupUI();
            }
            else
            {
                switch (res.code)
                {
                    case 6023:
                        Action action = delegate { CheerBtnClick(data); };
                        Managers.Player.SendTokenRequest(action);
                        break;
                }
            }

        },Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());
    }

    public void Setting(long todoMemberId, string nickname)
    {
        this.todoMemberId = todoMemberId;
        this.nickname = nickname;
    }

}
