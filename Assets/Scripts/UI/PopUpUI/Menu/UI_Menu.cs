using UnityEngine.UI;

public class UI_Menu : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
        Profile_btn,
        FTarget_btn,
        PTarget_btn,
        Collector_btn,
        Friend_btn,
        PlanetInfo_btn,
        Store_btn,
        Setting_btn,
    }

    string pathName = "Menu";

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        //열기 애니메이션 실행
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Profile_btn, (data) => { Managers.UI.ShowPopupUI<UI_Profile>("ProfileView", $"{pathName}/Profile"); });

        SetBtn((int)Buttons.FTarget_btn, (data) => { Managers.UI.ShowPopupUI<UI_FTarget>("FTargetView", $"{pathName}/Target"); });

        SetBtn((int)Buttons.PTarget_btn, (data) => { Managers.UI.ShowPopupUI<UI_PTarget>("PTargetView", $"{pathName}/Target"); });

        SetBtn((int)Buttons.Collector_btn, (data) => { Managers.UI.ShowPopupUI<UI_Collector>("CollectorView", $"{pathName}/Target"); });

        SetBtn((int)Buttons.Friend_btn, (data) => { Managers.UI.ShowPopupUI<UI_Friend>("FriendView", $"{pathName}/Friend"); });

        SetBtn((int)Buttons.PlanetInfo_btn, (data) => { Managers.UI.ShowPopupUI<UI_PlanetInfo>("PlanetInfoView", $"{pathName}/Info"); });

        SetBtn((int)Buttons.Store_btn, (data) => { Managers.UI.ShowPopupUI<UI_ItemStore>("ItemStoreView", $"{pathName}/ItemStore"); });

        SetBtn((int)Buttons.Setting_btn, (data) => { Managers.UI.ShowPopupUI<UI_Setting>("SettingView", $"{pathName}/Setting"); });
    }

    private void Start()
    {
        Init();
    }
}
