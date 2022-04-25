using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PlanetSet : UI_UserInfo
{
    enum Buttons
    {
        Back_btn,
        Next_btn,
    }

/*    enum ToggleGroups
    {
        PlanetRadioGroup,
    }*/
    enum Toggles
    {
        Red,
        Green,
        Blue,
    }

    Define.Planet planet = Define.Planet.EMPTY;

    GameObject nextBtn;
    //GameObject toggleGroup;
    Toggle red, green, blue;

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        //Bind<ToggleGroup>(typeof(ToggleGroups));
        Bind<Toggle>(typeof(Toggles));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);

        nextBtn = GetButton((int)Buttons.Next_btn).gameObject;
        BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);

        //toggleGroup = Get<ToggleGroup>((int)ToggleGroups.PlanetRadioGroup).gameObject;

        red = Get<Toggle>((int)Toggles.Red);
        green = Get<Toggle>((int)Toggles.Green);
        blue = Get<Toggle>((int)Toggles.Blue);

        red.onValueChanged.AddListener((bool bOn) =>
        {
            planet = Define.Planet.RED;
        });
        green.onValueChanged.AddListener((bool bOn) =>
        {
            planet = Define.Planet.GREEN;
        });
        blue.onValueChanged.AddListener((bool bOn) =>
        {
            planet = Define.Planet.BLUE;
        });
    }

    void Start()
    {
        Init();
    }


    private void NextBtnClick(PointerEventData data)
    {
        if (planet == Define.Planet.EMPTY)
        {
            Debug.Log("행성을 선택해주세요");
            return;
        }

        //유저 정보 서버에 넘기기
        loginScene.Planet = planet;

        //Managers.UI.CloseAllPopupUI();
        Managers.Scene.LoadScene(Define.Scene.Main);

/*        IEnumerable<Toggle> tg = toggleGroup.GetComponent<ToggleGroup>().ActiveToggles();
        foreach(Toggle t in tg)
        {
            Debug.Log("active "+t.name);
        }*/

        Debug.Log(planet);
    }


    
}
