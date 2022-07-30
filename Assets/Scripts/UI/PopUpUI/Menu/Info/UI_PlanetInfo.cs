using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 행성 정보 UI에 사용되는 스크립트
public class UI_PlanetInfo : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
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
    // ================================ //

    // 텍스트 오브젝트
    Text planetNameTxt, levelTitleTxt, ageTxt, levelTxt, pointTxt,
         avgPlanCompleteTxt, getGoodTxt, giveGoodTxt;
    // 행성 이미지
    Image planet;

    // 초기화
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

        SetTexts();

        InitData();
    }

    // 웹 통신 이후 데이터 초기화
    void InitData()
    {
        // 웹 통신 헤더 값 
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // 행성 정보 웹 통신
        Managers.Web.SendUniRequest("api/planet/my-info", "GET", null, (uwr) => {
            // 웹 응답 json 데이터를 유니티 데이터로 전환
            Response<ResponsePlanetInfo> response = JsonUtility.FromJson<Response<ResponsePlanetInfo>>(uwr.downloadHandler.text);

            // 웹 통신 성공 시
            if (response.code == 1000)
            {
                // Debug.Log(response.result);
                // 응답 데이터 저장
                Managers.Player.SetInt(Define.LEVEL, response.result.level);
                Managers.Player.SetInt(Define.AGE, (int)response.result.age);
                Managers.Player.SetInt(Define.POINT, response.result.point);
                Managers.Player.SetInt(Define.AVG_COMPLETE, response.result.avgGoalCompleteRate);
                Managers.Player.SetInt(Define.GET_GOOD, response.result.getFavoriteCount);
                Managers.Player.SetInt(Define.GIVE_GOOD, response.result.putFavoriteCount);

                SetTexts();
            }
            // 토큰 오류 시
            else if(response.code == 6000)
            {
                Debug.Log(response.message);
                Managers.Player.SendTokenRequest(InitData);
            }
            // 기타 오류 시
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    // 텍스트 초기화
    void SetTexts()
    {
        if(PlayerPrefs.HasKey(Define.NICKNAME))
        {
            planetNameTxt.text = Managers.Player.GetString(Define.NICKNAME) + "님의 행성";
        }

        if (PlayerPrefs.HasKey(Define.LEVEL))
        {
            levelTitleTxt.text = "Lv. " + Managers.Player.GetInt(Define.LEVEL).ToString();
            levelTxt.text = levelTitleTxt.text;
        }

        if (PlayerPrefs.HasKey(Define.AGE))
        {
            ageTxt.text = Managers.Player.GetInt(Define.AGE).ToString() + " 일";
        }

        if (PlayerPrefs.HasKey(Define.POINT))
        {
            pointTxt.text = Managers.Player.GetInt(Define.POINT).ToString() + " Point";
        }

        if (PlayerPrefs.HasKey(Define.AVG_COMPLETE))
        {
            avgPlanCompleteTxt.text = Managers.Player.GetInt(Define.AVG_COMPLETE).ToString() + " %";
        }

        if (PlayerPrefs.HasKey(Define.GET_GOOD))
        {
            getGoodTxt.text = Managers.Player.GetInt(Define.GET_GOOD).ToString() + " 회";
        }

        if (PlayerPrefs.HasKey(Define.GIVE_GOOD))
        {
            giveGoodTxt.text = Managers.Player.GetInt(Define.GIVE_GOOD).ToString() + " 회";
        }
    }

    // 버튼 이벤트 설정
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // 뒤로가기 버튼
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }

    void Start()
    {
        Init();
    }
}
