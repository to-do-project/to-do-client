using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;


public class UI_Menu : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
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

    Image profileImage; // 프로필 이미지
    // 텍스트 오브젝트
    Text profileText, ccountText;

    // 경로
    const string pathName = "Menu";
    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    // 초기화
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

        // 메인 UI가 겹치는 것을 방지
        //Managers.UI.DeactivePanelUI();
    }

    // 버튼 이벤트 설정
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // 뒤로가기 버튼
        SetBtn((int)Buttons.Back_btn, (data) => { Managers.UI.ActivePanelUI(); ClosePopupUI();});

        // 프로필 버든
        SetBtn((int)Buttons.Profile_btn, (data) => { Managers.UI.ShowPopupUI<UI_Profile>("ProfileView", $"{pathName}/Profile"); 
                                                     Managers.Sound.PlayNormalButtonClickSound(); });

        // 목표 보관함 버튼
        SetBtn((int)Buttons.Collector_btn, (data) => { Managers.UI.ShowPopupUI<UI_Collector>("CollectorView", $"{pathName}/Target").SetParent(gameObject); 
                                                       Managers.Sound.PlayNormalButtonClickSound(); });

        // 친구 버튼
        SetBtn((int)Buttons.Friend_btn, (data) => { Managers.UI.ShowPopupUI<UI_Friend>("FriendView", $"{pathName}/Friend");
                                                    Managers.Sound.PlayNormalButtonClickSound(); });

        // 행성 정보 버튼
        SetBtn((int)Buttons.PlanetInfo_btn, (data) => { Managers.UI.ShowPopupUI<UI_PlanetInfo>("PlanetInfoView", $"{pathName}/Info"); 
                                                        Managers.Sound.PlayNormalButtonClickSound(); });

        // 아이템 스토어 버튼
        SetBtn((int)Buttons.Store_btn, (data) => { Managers.UI.ShowPopupUI<UI_ItemStore>("ItemStoreView", $"{pathName}/ItemStore"); 
                                                   Managers.Sound.PlayNormalButtonClickSound(); });

        // 설정 버튼
        SetBtn((int)Buttons.Setting_btn, (data) => { Managers.UI.ShowPopupUI<UI_Setting>("SettingView", $"{pathName}/Setting"); 
                                                     Managers.Sound.PlayNormalButtonClickSound(); });

        // 캐릭터 꾸미기 버튼
        SetBtn((int)Buttons.Deco_btn, (data) => { Managers.UI.ShowPopupUI<UI_Deco>("DecoView", $"{pathName}/Deco"); 
                                                  Managers.Sound.PlayNormalButtonClickSound(); });
    }


    void Start()
    {
        Init();
    }

    // 프로필 컬러 변경
    // color = 프로필 색상
    public void ChangeColor(string color)
    {
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profileImage.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    // 닉네임 변경
    // nickname = 변경 할 닉네임
    public void ChangeNickname(string nickname)
    {
        profileText.text = nickname;
    }

    // 목표 보관 개수 변경
    public void ChangeCcount()
    {
        ccountText.text = dataContainer.goalList.Count.ToString();
    }
}