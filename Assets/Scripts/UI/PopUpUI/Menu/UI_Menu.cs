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
        Login_btn,
    }

    enum Images
    {
        Profile_image,
    }

    enum Texts
    {
        Profile_text,
        Ccount_text,
    }

    const string pathName = "Menu";
    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    Image profileImage;
    Text profileText, ccountText;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Image>(typeof(Images));
        profileImage = GetImage((int)Images.Profile_image);

        if (PlayerPrefs.HasKey(Define.PROFILE_COLOR))
        {
            ChangeColor(Managers.Player.GetString(Define.PROFILE_COLOR));
        }

        Bind<Text>(typeof(Texts));
        profileText = GetText((int)Texts.Profile_text);
        ccountText = GetText((int)Texts.Ccount_text);

        if (PlayerPrefs.HasKey(Define.NICKNAME))
        {
            ChangeNickname(Managers.Player.GetString(Define.NICKNAME));
        }

        if (PlayerPrefs.HasKey("targetCount"))
        {
            ChangeCcount("0");
        }

        //열기 애니메이션 실행
    }

    void CalcDate()
    {
        System.DateTime date = System.DateTime.Now;
        int sum = date.Year * 430 + date.Month * 32 + date.Day;
        if(PlayerPrefs.HasKey("DateTime") == false)
        {
            Debug.Log("새로운 접속");
            Managers.Player.SetInt("DateTime", sum);
        } 
        else
        {
            if(Managers.Player.GetInt("DateTime") < sum)
            {
                Debug.Log("하루 이상 지났습니다");
            }
            Managers.Player.SetInt("DateTime", sum);
        }
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
                email = "testest@gmail.com",
                password = "test1234",
                nickname = "testest",
                planetColor = "RED",
                deviceToken = SystemInfo.deviceUniqueIdentifier
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
                                Managers.Player.SetString(Define.DEVICETOKEN, val.deviceToken);
                                Managers.Player.SetString(Define.JWT_ACCESS_TOKEN, uwr.GetResponseHeader(Define.JWT_ACCESS_TOKEN));
                                Managers.Player.SetString(Define.JWT_REFRESH_TOKEN, uwr.GetResponseHeader(Define.JWT_REFRESH_TOKEN));
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

        SetBtn((int)Buttons.Login_btn, (data) => {
            RequestLogin request = new RequestLogin
            {
                email = "testest@gmail.com",
                deviceToken = SystemInfo.deviceUniqueIdentifier,
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
                    Managers.Player.SetString(Define.USER_ID, response.result.userId.ToString());
                    Managers.Player.SetString(Define.PLANET_ID, response.result.userId.ToString());
                    Managers.Player.SetInt(Define.PLANET_LEVEL, response.result.planetLevel);
                    Managers.Player.SetInt(Define.CHARACTER_ITEM, (int)response.result.characterItem);
                    Managers.Player.SetString(Define.PLANET_COLOR, response.result.planetColor);
                    Managers.Player.SetString(Define.PROFILE_COLOR, response.result.profileColor);
                    Managers.Player.SetString(Define.CHARACTER_COLOR, response.result.characterItem.ToString());
                    Managers.Player.SetInt(Define.POINT, response.result.point);
                    Managers.Player.SetString(Define.DEVICETOKEN, request.deviceToken);
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

    public void ChangeColor(string color)
    {
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profileImage.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    public void ChangeNickname(string nickname)
    {
        profileText.text = nickname;
    }

    public void ChangeCcount(string count)
    {
        ccountText.text = count;
    }
}