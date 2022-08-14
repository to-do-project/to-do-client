using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class UI_Find : UI_PWfind
{

    Action<UnityWebRequest> callback;
    Response<string> res;

    enum Buttons
    {
        Back_btn,
        sendEmail_btn,
    }

    enum InputFields
    {
        ID_inputfield,
    }

    InputField idInputfield;
    GameObject sendEmailBtn;

    public override void Init()
    {
        base.Init();

        Bind<InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);

        sendEmailBtn = GetButton((int)Buttons.sendEmail_btn).gameObject;
        BindEvent(sendEmailBtn, SendEmailBtnClick, Define.TouchEvent.Touch);

        idInputfield = GetInputfiled((int)InputFields.ID_inputfield);


        callback -= ResponseAction;
        callback += ResponseAction;

    }

    void Start()
    {
        Init();
    }

    private void SendEmailBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        //if (IsValidEmail(idInputfield.text))
        if(Util.IsValidString(idInputfield.text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            //send email API »£√‚
            res = new Response<string>();
            RequestEmail val = new RequestEmail { email = idInputfield.text };
            Managers.Web.SendPostRequest<string>("user/temporary/pwd", val, callback);

           //
        }
        
    }

    private void ResponseAction(UnityWebRequest request)
    {
        if (res != null)
        {
            res = JsonUtility.FromJson<Response<string>>(request.downloadHandler.text);

            if (res.isSuccess)
            {
                Debug.Log(res.message);
                Managers.UI.ShowPopupUI<UI_PWAuth>("AuthView", "PWfind");
            }
            else
            {
                Debug.Log(res.message);
                Managers.UI.ShowPopupUI<UI_PWFailed>("FailedView", "PWfind");
            }

            res = null;
        }
    }
}
