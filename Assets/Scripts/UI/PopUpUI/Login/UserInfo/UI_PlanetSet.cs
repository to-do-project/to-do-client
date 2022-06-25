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

[System.Serializable]
public class ResponseSignUp
{
    public long userId;
    public long planetId;
    public int planetLevel;
    public string planetColor;
    public string email;
    public string nickname;
    public long characterItem;
    public string profileColor;
    public int point;
    public int missionStatus;
    public string deviceToken;
}

public class UI_PlanetSet : UI_UserInfo
{

    Action<UnityWebRequest> callback;
    Response<ResponseSignUp> res; 


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


        res = new Response<ResponseSignUp>();

        RequestSignUp val = new RequestSignUp
        {
            email = loginScene.Email,
            password = loginScene.Pw,
            nickname = loginScene.Nickname,
            planetColor = sPlanet,
            deviceToken = UnityEngine.SystemInfo.deviceUniqueIdentifier

        };
        Managers.Web.SendPostRequest<ResponseSignUp>("join",val,callback);


    }

    private void ResponseAction(UnityWebRequest request)
    {
        if (res != null)
        {
            res = JsonUtility.FromJson<Response<ResponseSignUp>>(request.downloadHandler.text);


            if (res.isSuccess)
            {
                switch (res.code) 
                {
                    case 1000:



                        //토큰 저장
                        Managers.Player.SetString(Define.JWT_ACCESS_TOKEN, request.GetResponseHeader(Define.JWT_ACCESS_TOKEN));
                        Managers.Player.SetString(Define.JWT_REFRESH_TOKEN, request.GetResponseHeader(Define.JWT_REFRESH_TOKEN));
                        Debug.Log(Managers.Player.GetString(Define.JWT_ACCESS_TOKEN));
                        Debug.Log(Managers.Player.GetString(Define.JWT_REFRESH_TOKEN));

                        //Debug.Log(res.result);

                        ResponseSignUp result = res.result;

                        //Debug.Log(result.email);

                        if (result != null)
                        {
                            Managers.Player.SetString(Define.EMAIL, result.email);
                            Managers.Player.SetString(Define.NICKNAME, result.nickname);
                            Managers.Player.SetString(Define.USER_ID, result.userId.ToString());
                            Managers.Player.SetString(Define.PLANET_ID, result.planetId.ToString());
                            Managers.Player.SetInt(Define.PLANET_LEVEL, result.planetLevel);
                            Managers.Player.SetString(Define.PLANET_COLOR, result.planetColor);
                            Managers.Player.SetString(Define.EMAIL, result.email);
                            Managers.Player.SetString(Define.NICKNAME, result.nickname);
                            Managers.Player.SetString(Define.CHARACTER_COLOR, result.characterItem.ToString());

                            Managers.Player.Init();
                            UI_Load.Instance.InstantLoad("Main");
                            //Managers.Scene.LoadScene(Define.Scene.Main);
                        }

                        else
                        {
                            Debug.Log("result is null");
                        }


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
