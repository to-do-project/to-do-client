using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Menu : UI_Popup
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
    private void CameraSet()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, BackBtnClick, Define.TouchEvent.Touch);

        GameObject profileBtn = GetButton((int)Buttons.Profile_btn).gameObject;
        BindEvent(profileBtn, ProfileBtnClick, Define.TouchEvent.Touch);

        GameObject ftargetBtn = GetButton((int)Buttons.FTarget_btn).gameObject;
        BindEvent(ftargetBtn, FtargetBtnClick, Define.TouchEvent.Touch);

        GameObject ptargetBtn = GetButton((int)Buttons.PTarget_btn).gameObject;
        BindEvent(ptargetBtn, PtargetBtnClick, Define.TouchEvent.Touch);

        GameObject collectorBtn = GetButton((int)Buttons.Collector_btn).gameObject;
        BindEvent(collectorBtn, CollectorBtnClick, Define.TouchEvent.Touch);

        GameObject friendBtn = GetButton((int)Buttons.Friend_btn).gameObject;
        BindEvent(friendBtn, FriendBtnClick, Define.TouchEvent.Touch);

        GameObject planetInfoBtn = GetButton((int)Buttons.PlanetInfo_btn).gameObject;
        BindEvent(planetInfoBtn, PlanetInfoBtnClick, Define.TouchEvent.Touch);

        GameObject storeBtn = GetButton((int)Buttons.Store_btn).gameObject;
        BindEvent(storeBtn, StoreBtnClick, Define.TouchEvent.Touch);

        GameObject settingBtn = GetButton((int)Buttons.Setting_btn).gameObject;
        BindEvent(settingBtn, SettingBtnClick, Define.TouchEvent.Touch);
    }

    #region ButtonEvents
    public void BackBtnClick(PointerEventData data)
    {
        //닫기 애니메이션 실행 후 삭제
        Managers.UI.ClosePopupUI();
    }
    public void ProfileBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Profile>("ProfileView", $"{pathName}/Profile");
    }
    public void FtargetBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_FTarget>("FTargetView", $"{pathName}/Target");
    }
    public void PtargetBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_PTarget>("PTargetView", $"{pathName}/Target");
    }
    public void CollectorBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Collector>("CollectorView", $"{pathName}/Target");
    }
    public void FriendBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_Friend>("FriendView", $"{pathName}/Friend");
    }
    public void PlanetInfoBtnClick(PointerEventData data)
    {
        Debug.Log("*Clicked Button* PlanetInfo");
    }
    public void StoreBtnClick(PointerEventData data)
    {
        Debug.Log("*Clicked Button* Store");
    }
    public void SettingBtnClick(PointerEventData data)
    {
        Debug.Log("*Clicked Button* Setting");
    }
    #endregion

    private void Start()
    {
        Init();
    }
}
