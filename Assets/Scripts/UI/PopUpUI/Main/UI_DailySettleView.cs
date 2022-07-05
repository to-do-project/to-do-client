using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DailySettleView : UI_Popup
{
    enum Texts
    {
        point_txt,
        exp_txt,
    }
    enum Buttons
    {
        check_btn,
    }

    void Start()
    {
        Init();
    }

    Text pointTxt, expTxt;
    int point, exp;

    public override void Init()
    {
        base.Init();

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();

        if (UIcam != cam)
        {
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GameObject checkBtn = GetButton((int)Buttons.check_btn).gameObject;
        BindEvent(checkBtn, CheckBtnClick);

        pointTxt = GetText((int)Texts.point_txt);
        expTxt = GetText((int)Texts.exp_txt);


        Debug.Log($"point : {point}, origin point : {Managers.Player.GetInt(Define.POINT)}");
        Debug.Log($"exp : {exp}, origin exp : {Managers.Player.GetInt(Define.EXP)}");

        int newpoint = point - Managers.Player.GetInt(Define.POINT);
        int newexp = exp - Managers.Player.GetInt(Define.EXP);

        pointTxt.text = newpoint.ToString() + " point";
        expTxt.text = newexp.ToString() + " exp";

        Managers.Player.SetInt(Define.POINT, point);
        Managers.Player.SetInt(Define.EXP, exp);



    }

    private void CheckBtnClick(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
    }

    public void Setting(int point, int exp)
    {
        this.point = point;
        this.exp = exp;
    }
}
