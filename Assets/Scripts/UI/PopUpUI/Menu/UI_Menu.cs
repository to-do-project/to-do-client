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

        //���� �ִϸ��̼� ����
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