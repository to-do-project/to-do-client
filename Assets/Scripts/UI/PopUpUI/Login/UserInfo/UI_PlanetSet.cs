using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RequestSignUp
{
    public string email;
    public string password;
    public string nickname;
    public string planetColor;
    public string deviceToken;
}

public class ResponseSignUp
{
    public long userId;
    public long planetId;
    public string email;
    public string nickname;
    public string characterColor;
    public string profileColor;
    public int point;
    public int missionStatus;
    public string deviceToken;
}

public class UI_PlanetSet : UI_UserInfo
{

    Action<UnityWebRequest> callback;
    Response<string> res; //인증번호 발송 용


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
        nextBtn.GetComponent<Button>().interactable = false;


        //toggleGroup = Get<ToggleGroup>((int)ToggleGroups.PlanetRadioGroup).gameObject;

        red = Get<Toggle>((int)Toggles.Red);
        green = Get<Toggle>((int)Toggles.Green);
        blue = Get<Toggle>((int)Toggles.Blue);

        red.onValueChanged.AddListener((bool bOn) =>
        {
            if (red.isOn)
            {
                planet = Define.Planet.RED;
                nextBtn.GetComponent<Button>().interactable = true;
                BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
            else
            {
                planet = Define.Planet.EMPTY;
                nextBtn.GetComponent<Button>().interactable = false;
                ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
        });
        green.onValueChanged.AddListener((bool bOn) =>
        {
            if (green.isOn)
            {
                planet = Define.Planet.GREEN;
                nextBtn.GetComponent<Button>().interactable = true;
                BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
            else
            {
                planet = Define.Planet.EMPTY;
                nextBtn.GetComponent<Button>().interactable = false;
                ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
        });
        blue.onValueChanged.AddListener((bool bOn) =>
        {
            if (blue.isOn)
            {
                planet = Define.Planet.BLUE;
                nextBtn.GetComponent<Button>().interactable = true;
                BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }           
            else
            {
                planet = Define.Planet.EMPTY;
                nextBtn.GetComponent<Button>().interactable = false;
                ClearEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
            }
        });

        callback -= ResponseAction;
        callback += ResponseAction;

    }

    void Start()
    {
        Init();
    }


    private void NextBtnClick(PointerEventData data)
    {
        if (planet == Define.Planet.EMPTY)
        {
            //Debug.Log("행성을 선택해주세요");
            return;
        }

        //유저 정보 서버에 넘기기
        loginScene.Planet = planet;

        string sPlanet="";
        switch (loginScene.Planet) {
            case Define.Planet.BLUE:
                sPlanet = "BLUE";
                break;
            case Define.Planet.GREEN:
                sPlanet = "GREEN";
                break;
            case Define.Planet.RED:
                sPlanet = "RED";
                break;
        }


        res = new Response<string>();

        RequestSignUp val = new RequestSignUp
        {
            email = loginScene.Email,
            password = loginScene.Pw,
            nickname = loginScene.Nickname,
            planetColor = sPlanet,
            deviceToken = "12345"

        };
        Managers.Web.SendPostRequest<ResponseSignUp>("join",val,callback);


        //Managers.UI.CloseAllPopupUI();

/*        IEnumerable<Toggle> tg = toggleGroup.GetComponent<ToggleGroup>().ActiveToggles();
        foreach(Toggle t in tg)
        {
            Debug.Log("active "+t.name);
        }*/

        //Debug.Log(planet);
    }

    private void ResponseAction(UnityWebRequest request)
    {
        if (res != null)
        {
            res = JsonUtility.FromJson<Response<string>>(request.downloadHandler.text);

            if (res.isSuccess)
            {
                switch (res.code) 
                {
                    case 1000:
                        Managers.Scene.LoadScene(Define.Scene.Main);
                        break;
                    default:
                        Debug.Log(res.message);
                        break;
                }

            }
            else
            {
                Debug.Log(res.message);
            }

            res = null;
        }
    }

    
}
