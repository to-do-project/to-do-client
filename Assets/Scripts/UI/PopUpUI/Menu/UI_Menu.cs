using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

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

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Profile_btn, (data) => { Managers.UI.ShowPopupUI<UI_Profile>("ProfileView", $"{pathName}/Profile"); });

        SetBtn((int)Buttons.Collector_btn, (data) => { Managers.UI.ShowPopupUI<UI_Collector>("CollectorView", $"{pathName}/Target"); });

        SetBtn((int)Buttons.Friend_btn, (data) => { Managers.UI.ShowPopupUI<UI_Friend>("FriendView", $"{pathName}/Friend"); });

        SetBtn((int)Buttons.PlanetInfo_btn, (data) => { Managers.UI.ShowPopupUI<UI_PlanetInfo>("PlanetInfoView", $"{pathName}/Info"); });

        SetBtn((int)Buttons.Store_btn, (data) => { Managers.UI.ShowPopupUI<UI_ItemStore>("ItemStoreView", $"{pathName}/ItemStore"); });

        SetBtn((int)Buttons.Setting_btn, (data) => { Managers.UI.ShowPopupUI<UI_Setting>("SettingView", $"{pathName}/Setting"); });

        SetBtn((int)Buttons.Deco_btn, (data) => { Managers.UI.ShowPopupUI<UI_Deco>("DecoView", $"{pathName}/Deco"); });
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
        List<string> hN = new List<string>();
        List<string> hV = new List<string>();

        hN.Add("Jwt-Refresh-Token");
        hN.Add("User-Id");
        hV.Add("Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiIxIiwicm9sZSI6IlJPTEVfVVNFUiIsImlhdCI6MTY1MDYxOTAyNCwiZXhwIjoxNjUwNjIwODI0fQ.odEo-InfJFThh60QDXiSWjfP9rVzk6foxFDBDzG2hoc");
        // hV.Add("Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE2NTA3MDM4MTYsImV4cCI6MTY1MTU2NzgxNn0.dshtPR1lsKm_zmg80rHwEqLjuAjvJaCQpKyd1nPnpIY");
        hV.Add("1");

        Test test = new Test();
        test.deviceToken = "testing";

        Testing.instance.Webbing("access-token", "POST", test, (data) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(data.downloadHandler.text);
            if (response.isSuccess)
            {
                Debug.Log(response.result);
                Debug.Log(data.GetResponseHeader("Jwt-Access-Token"));
                Debug.Log(data.GetResponseHeader("Jwt-Refresh-Token"));
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }
}