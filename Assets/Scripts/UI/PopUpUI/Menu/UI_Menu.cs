using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RequestTest
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
public class UI_Menu : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
        Profile_btn,
        Collector_btn,
        Friend_btn,
        PlanetInfo_btn,
        Store_btn,
        Setting_btn,
        Deco_btn,
        Refresh_btn,
        Login_btn,
    }

    enum Images
    {
        Profile_image,
    }

    const string pathName = "Menu";
    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    Image profileImage;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Image>(typeof(Images));

        profileImage = GetImage((int)Images.Profile_image);

        //열기 애니메이션 실행
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, /*(data) =>
        {
            Response<string> res;

            res = new Response<string>();

            RequestSignUp val = new RequestSignUp
            {
                email = "tester@gmail.com",
                password = "test1234",
                nickname = "test",
                planetColor = "RED",
                deviceToken = "testtest"
            };
            Managers.Web.SendPostRequest<ResponseSignUp>("join", val, (uwr) =>
            {
                if (res != null)
                {
                    res = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

                    if (res.isSuccess)
                    {
                        switch (res.code)
                        {
                            case 1000:
                                Debug.Log(uwr.downloadHandler.text);
                                Testing.instance.DeviceToken = val.deviceToken;
                                Testing.instance.AccessToken = uwr.GetResponseHeader("Jwt-Access-Token");
                                Testing.instance.RefreshToken = uwr.GetResponseHeader("Jwt-Refresh-Token");
                                Debug.Log(uwr.GetResponseHeader("Jwt-Access-Token"));
                                Debug.Log(uwr.GetResponseHeader("Jwt-Refresh-Token"));
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
            });
        });*/ ClosePopupUI); // 테스트용 계정을 만들기위한 메소드. ClosePopupUI가 기존 메소드

        SetBtn((int)Buttons.Profile_btn, (data) => { Managers.UI.ShowPopupUI<UI_Profile>("ProfileView", $"{pathName}/Profile"); });

        SetBtn((int)Buttons.Collector_btn, (data) => { Managers.UI.ShowPopupUI<UI_Collector>("CollectorView", $"{pathName}/Target"); });

        SetBtn((int)Buttons.Friend_btn, (data) => { Managers.UI.ShowPopupUI<UI_Friend>("FriendView", $"{pathName}/Friend"); });

        SetBtn((int)Buttons.PlanetInfo_btn, (data) => { Managers.UI.ShowPopupUI<UI_PlanetInfo>("PlanetInfoView", $"{pathName}/Info"); });

        SetBtn((int)Buttons.Store_btn, (data) => { Managers.UI.ShowPopupUI<UI_ItemStore>("ItemStoreView", $"{pathName}/ItemStore"); });

        SetBtn((int)Buttons.Setting_btn, (data) => { Managers.UI.ShowPopupUI<UI_Setting>("SettingView", $"{pathName}/Setting"); });

        SetBtn((int)Buttons.Deco_btn, (data) => { Managers.UI.ShowPopupUI<UI_Deco>("DecoView", $"{pathName}/Deco"); });

        SetBtn((int)Buttons.Refresh_btn, (data) => {
            string[] hN = { Define.JWT_REFRESH_TOKEN,
                            "User-Id" };
            string[] hV = { Managers.Player.GetString(Define.JWT_REFRESH_TOKEN),
                            Managers.Player.GetString(Define.USER_ID) };

            RequestLogout request = new RequestLogout();
            request.deviceToken = Managers.Player.GetString(Define.DEVICETOKEN);

            Managers.Web.SendUniRequest("access-token", "POST", request, (uwr) => {
                Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
                if (response.code == 1000)
                {
                    Debug.Log(response.result);
                    Debug.Log(Managers.Player.GetString(Define.JWT_ACCESS_TOKEN));

                    Managers.Player.SetString(Define.JWT_ACCESS_TOKEN, uwr.GetResponseHeader(Define.JWT_ACCESS_TOKEN));
                    Managers.Player.SetString(Define.JWT_REFRESH_TOKEN, uwr.GetResponseHeader(Define.JWT_REFRESH_TOKEN));

                    Debug.Log(uwr.GetResponseHeader("Jwt-Access-Token"));
                    Debug.Log(Managers.Player.GetString(Define.JWT_ACCESS_TOKEN));
                }
                else
                {
                    Debug.Log(response.message);
                }
            }, hN, hV);
        });

        SetBtn((int)Buttons.Login_btn, (data) => {

            RequestLogin request = new RequestLogin
            {
                email = "tester@gmail.com",
                deviceToken = "testtest",
                password = "test1234"
            };

            Managers.Web.SendUniRequest("login", "POST", request, (uwr) => {
                Response<RequestTest> response = JsonUtility.FromJson<Response<RequestTest>>(uwr.downloadHandler.text);
                if (response.code == 1000)
                {
                    Debug.Log(response.result.email);
                    Managers.Player.SetString(Define.JWT_ACCESS_TOKEN, uwr.GetResponseHeader(Define.JWT_ACCESS_TOKEN));
                    Managers.Player.SetString(Define.JWT_REFRESH_TOKEN, uwr.GetResponseHeader(Define.JWT_REFRESH_TOKEN));

                    Managers.Player.SetString(Define.EMAIL, response.result.email);
                    Managers.Player.SetString(Define.NICKNAME, response.result.nickname);
                    Managers.Player.SetString("User-Id", response.result.userId.ToString());
                    Managers.Player.SetString(Define.PLANET_ID, response.result.userId.ToString());
                    Managers.Player.SetInt(Define.PLANET_LEVEL, response.result.planetLevel);
                    Managers.Player.SetString(Define.PLANET_COLOR, response.result.planetColor);
                    Managers.Player.SetString(Define.EMAIL, response.result.email);
                    Managers.Player.SetString(Define.NICKNAME, response.result.nickname);
                    Managers.Player.SetString(Define.CHARACTER_COLOR, response.result.characterItem.ToString());
                    Debug.Log(Managers.Player.GetString(Define.USER_ID));
                }
                else
                {
                    Debug.Log(response.message);
                }
            });
        });
    }


    private void Start()
    {
        Init();
    }

    public void ChangeProfile(string color)
    {
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profileImage.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    public void TokenRefresh()
    {
        string[] hN = { Define.JWT_REFRESH_TOKEN,
                        Define.USER_ID };
        string[] hV = { Managers.Player.GetString(Define.JWT_REFRESH_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Test test = new Test();
        test.deviceToken = Managers.Player.GetString(Define.DEVICETOKEN);

        Managers.Web.SendUniRequest("access-token", "POST", test, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                Debug.Log(response.result);
                Managers.Player.SetString(Define.JWT_ACCESS_TOKEN, uwr.GetResponseHeader(Define.JWT_ACCESS_TOKEN));
                Managers.Player.SetString(Define.JWT_REFRESH_TOKEN, uwr.GetResponseHeader(Define.JWT_REFRESH_TOKEN));
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }
}