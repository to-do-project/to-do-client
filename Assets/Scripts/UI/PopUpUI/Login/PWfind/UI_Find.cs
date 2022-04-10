using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Find : UI_PWfind
{
    enum Buttons
    {
        Back_btn,
        sendEmail_btn,
    }

    enum InputFields
    {
        ID_inputfield,
    }

    GameObject sendEmailBtn;

    public override void Init()
    {
        base.Init();

        Bind<InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.UIEvent.Click);

        sendEmailBtn = GetButton((int)Buttons.sendEmail_btn).gameObject;
        BindEvent(sendEmailBtn, SendEmailBtnClick, Define.UIEvent.Click);
    }

    void Start()
    {
        Init();
    }

    private void SendEmailBtnClick(PointerEventData data)
    {
        //send email API »£√‚

        Managers.UI.ShowPopupUI<UI_PWAuth>("AuthView", "PWfind");
    }
}
