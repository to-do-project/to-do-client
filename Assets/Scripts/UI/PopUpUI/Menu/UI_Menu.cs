using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;


public class UI_Menu : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
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
    // ================================ //

    Image profileImage; // ������ �̹���
    // �ؽ�Ʈ ������Ʈ
    Text profileText, ccountText;

    // ���
    const string pathName = "Menu";
    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    // �ʱ�ȭ
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

        ChangeCcount();

        // ���� UI�� ��ġ�� ���� ����
        //Managers.UI.DeactivePanelUI();
    }

    // ��ư �̺�Ʈ ����
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // �ڷΰ��� ��ư
        SetBtn((int)Buttons.Back_btn, (data) => { Managers.UI.ActivePanelUI(); ClosePopupUI();});

        // ������ ����
        SetBtn((int)Buttons.Profile_btn, (data) => { Managers.UI.ShowPopupUI<UI_Profile>("ProfileView", $"{pathName}/Profile"); 
                                                     Managers.Sound.PlayNormalButtonClickSound(); });

        // ��ǥ ������ ��ư
        SetBtn((int)Buttons.Collector_btn, (data) => { Managers.UI.ShowPopupUI<UI_Collector>("CollectorView", $"{pathName}/Target").SetParent(gameObject); 
                                                       Managers.Sound.PlayNormalButtonClickSound(); });

        // ģ�� ��ư
        SetBtn((int)Buttons.Friend_btn, (data) => { Managers.UI.ShowPopupUI<UI_Friend>("FriendView", $"{pathName}/Friend");
                                                    Managers.Sound.PlayNormalButtonClickSound(); });

        // �༺ ���� ��ư
        SetBtn((int)Buttons.PlanetInfo_btn, (data) => { Managers.UI.ShowPopupUI<UI_PlanetInfo>("PlanetInfoView", $"{pathName}/Info"); 
                                                        Managers.Sound.PlayNormalButtonClickSound(); });

        // ������ ����� ��ư
        SetBtn((int)Buttons.Store_btn, (data) => { Managers.UI.ShowPopupUI<UI_ItemStore>("ItemStoreView", $"{pathName}/ItemStore"); 
                                                   Managers.Sound.PlayNormalButtonClickSound(); });

        // ���� ��ư
        SetBtn((int)Buttons.Setting_btn, (data) => { Managers.UI.ShowPopupUI<UI_Setting>("SettingView", $"{pathName}/Setting"); 
                                                     Managers.Sound.PlayNormalButtonClickSound(); });

        // ĳ���� �ٹ̱� ��ư
        SetBtn((int)Buttons.Deco_btn, (data) => { Managers.UI.ShowPopupUI<UI_Deco>("DecoView", $"{pathName}/Deco"); 
                                                  Managers.Sound.PlayNormalButtonClickSound(); });
    }


    void Start()
    {
        Init();
    }

    // ������ �÷� ����
    // color = ������ ����
    public void ChangeColor(string color)
    {
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profileImage.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    // �г��� ����
    // nickname = ���� �� �г���
    public void ChangeNickname(string nickname)
    {
        profileText.text = nickname;
    }

    // ��ǥ ���� ���� ����
    public void ChangeCcount()
    {
        ccountText.text = dataContainer.goalList.Count.ToString();
    }
}