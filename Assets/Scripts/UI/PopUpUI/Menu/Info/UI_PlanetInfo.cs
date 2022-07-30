using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �༺ ���� UI�� ���Ǵ� ��ũ��Ʈ
public class UI_PlanetInfo : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
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

    // �ؽ�Ʈ ������Ʈ
    Text planetNameTxt, levelTitleTxt, ageTxt, levelTxt, pointTxt,
         avgPlanCompleteTxt, getGoodTxt, giveGoodTxt;
    // �༺ �̹���
    Image planet;

    // �ʱ�ȭ
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

    // �� ��� ���� ������ �ʱ�ȭ
    void InitData()
    {
        // �� ��� ��� �� 
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // �༺ ���� �� ���
        Managers.Web.SendUniRequest("api/planet/my-info", "GET", null, (uwr) => {
            // �� ���� json �����͸� ����Ƽ �����ͷ� ��ȯ
            Response<ResponsePlanetInfo> response = JsonUtility.FromJson<Response<ResponsePlanetInfo>>(uwr.downloadHandler.text);

            // �� ��� ���� ��
            if (response.code == 1000)
            {
                // Debug.Log(response.result);
                // ���� ������ ����
                Managers.Player.SetInt(Define.LEVEL, response.result.level);
                Managers.Player.SetInt(Define.AGE, (int)response.result.age);
                Managers.Player.SetInt(Define.POINT, response.result.point);
                Managers.Player.SetInt(Define.AVG_COMPLETE, response.result.avgGoalCompleteRate);
                Managers.Player.SetInt(Define.GET_GOOD, response.result.getFavoriteCount);
                Managers.Player.SetInt(Define.GIVE_GOOD, response.result.putFavoriteCount);

                SetTexts();
            }
            // ��ū ���� ��
            else if(response.code == 6000)
            {
                Debug.Log(response.message);
                Managers.Player.SendTokenRequest(InitData);
            }
            // ��Ÿ ���� ��
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    // �ؽ�Ʈ �ʱ�ȭ
    void SetTexts()
    {
        if(PlayerPrefs.HasKey(Define.NICKNAME))
        {
            planetNameTxt.text = Managers.Player.GetString(Define.NICKNAME) + "���� �༺";
        }

        if (PlayerPrefs.HasKey(Define.LEVEL))
        {
            levelTitleTxt.text = "Lv. " + Managers.Player.GetInt(Define.LEVEL).ToString();
            levelTxt.text = levelTitleTxt.text;
        }

        if (PlayerPrefs.HasKey(Define.AGE))
        {
            ageTxt.text = Managers.Player.GetInt(Define.AGE).ToString() + " ��";
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
            getGoodTxt.text = Managers.Player.GetInt(Define.GET_GOOD).ToString() + " ȸ";
        }

        if (PlayerPrefs.HasKey(Define.GIVE_GOOD))
        {
            giveGoodTxt.text = Managers.Player.GetInt(Define.GIVE_GOOD).ToString() + " ȸ";
        }
    }

    // ��ư �̺�Ʈ ����
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // �ڷΰ��� ��ư
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }

    void Start()
    {
        Init();
    }
}
