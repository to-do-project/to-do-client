using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlanetInfo : UI_Popup
{
    enum Buttons
    {
        Back_btn,
    }

    enum Images
    {
        Planet_img
    }

    enum Texts
    {
        PlanetName_txt,
        LevelTitle_txt,
        Age_txt,
        Level_txt,
        Point_txt,
        AvgPlanComplete_txt,
        GetGood_txt,
        GiveGood_txt,
    }

    Text planetNameTxt, levelTitleTxt, ageTxt, levelTxt, pointTxt,
         avgPlanCompleteTxt, getGoodTxt, giveGoodTxt;
    Image planet;

    private GameObject content = null;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        planetNameTxt = GetText((int)Texts.PlanetName_txt);
        levelTitleTxt = GetText((int)Texts.LevelTitle_txt);
        ageTxt = GetText((int)Texts.Age_txt);
        levelTxt = GetText((int)Texts.Level_txt);
        pointTxt = GetText((int)Texts.Point_txt);
        avgPlanCompleteTxt = GetText((int)Texts.AvgPlanComplete_txt);
        getGoodTxt = GetText((int)Texts.GetGood_txt);
        giveGoodTxt = GetText((int)Texts.GiveGood_txt);

        planet = GetImage((int)Images.Planet_img);

        if (content == null)
        {
            content = GameObject.Find("Content");
        }
    }

    private void CameraSet()
    {
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
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);
    }

    private void Start()
    {
        Init();
    }
}
